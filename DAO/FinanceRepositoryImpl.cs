using DAO;
using entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MyExceptions;
using Utils;
//does it need to implemented with datatables or we can just use addwithvalue instead?
namespace DAO
{
    public class FinanceRepositoryImpl : IFinanceRepository
    {

        public User GetUserByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE email = @Email";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = Convert.ToInt32(reader["user_id"]),
                        Username = reader["username"].ToString(),
                        Password = reader["password"].ToString(),
                        Email = reader["email"].ToString()
                    };
                }

                return null;
            }
        }


        public bool LoginAdmin(string username, string password)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Admins WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public int LoginUser(string username, string password)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "SELECT user_id FROM Users WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                con.Open();
                var result = cmd.ExecuteScalar();

                if (result == null)
                {
                    throw new UserNotFoundException("Invalid username or password.");
                }

                return Convert.ToInt32(result);
            }
        }

        public bool CreateUser(User user)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Users (username, password, email) VALUES (@username, @password, @email)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@email", user.Email);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool CreateExpense(Expense expense)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "INSERT INTO Expenses (user_id, amount, category_id, date, description) VALUES (@userId, @amount, @categoryId, @date, @description)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", expense.UserId);
                cmd.Parameters.AddWithValue("@amount", expense.Amount);
                cmd.Parameters.AddWithValue("@categoryId", expense.CategoryId);
                cmd.Parameters.AddWithValue("@date", expense.Date);
                cmd.Parameters.AddWithValue("@description", expense.Description);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public List<Expense> GetExpensesByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            List<Expense> expenses = new List<Expense>();

            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = @"
            SELECT e.expense_id, e.user_id, e.amount, e.category_id, c.category_name, e.date, e.description
            FROM Expenses e
            JOIN Expensecategories c ON e.category_id = c.category_id
            WHERE e.user_id = @userId AND e.date BETWEEN @startDate AND @endDate
            ORDER BY e.date";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    expenses.Add(new Expense
                    {
                        ExpenseId = Convert.ToInt32(reader["expense_id"]),
                        UserId = Convert.ToInt32(reader["user_id"]),
                        Amount = Convert.ToDecimal(reader["amount"]),
                        CategoryId = Convert.ToInt32(reader["category_id"]),
                        CategoryName = reader["category_name"].ToString(),
                        Date = Convert.ToDateTime(reader["date"]),
                        Description = reader["description"].ToString()
                    });
                }
            }

            return expenses;
        }

        public bool DeleteUser(int userId)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "DELETE FROM Users WHERE user_id = @userId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", userId);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        public bool DeleteExpense(int expenseId, int userId)
        {
            try
            {
                using (SqlConnection con = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Expenses WHERE expense_id = @expenseId AND user_id = @userId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@expenseId", expenseId);
                    cmd.Parameters.AddWithValue("@userId", userId);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        throw new ExpenseNotFoundException("Expense not found or you don't have permission to delete it.");
                    }

                    return true;
                }
            }
            catch (ExpenseNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public bool AdminDeleteExpense(int expenseId)
        {
            try
            {
                using (SqlConnection con = DBConnection.GetConnection())
                {
                    string query = "DELETE FROM Expenses WHERE expense_id = @expenseId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@expenseId", expenseId);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                    {
                        throw new ExpenseNotFoundException("Expense not found.");
                    }

                    return true;
                }
            }
            catch (ExpenseNotFoundException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public List<Expense> GetAllExpenses(int userId)
        {
            List<Expense> expenses = new List<Expense>();

            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = @"
            SELECT 
                e.expense_id, 
                u.username, 
                c.category_name, 
                e.amount, 
                e.date, 
                e.description
            FROM Expenses e
            JOIN Users u ON e.user_id = u.user_id
            JOIN ExpenseCategories c ON e.category_id = c.category_id
            WHERE e.user_id = @userId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userId", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Expense expense = new Expense
                    {
                        ExpenseId = Convert.ToInt32(reader["expense_id"]),
                        Username = reader["username"].ToString(),                  
                        CategoryName = reader["category_name"].ToString(),        
                        Amount = Convert.ToDecimal(reader["amount"]),
                        Date = Convert.ToDateTime(reader["date"]),
                        Description = reader["description"].ToString()
                    };
                    expenses.Add(expense);
                }
            }

            return expenses;
        }

        public bool UpdateExpense(int userId, Expense expense)
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string query = "UPDATE Expenses SET amount = @amount, category_id = @categoryId, date = @date, description = @description WHERE expense_id = @expenseId AND user_id = @userId";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@amount", expense.Amount);
                cmd.Parameters.AddWithValue("@categoryId", expense.CategoryId);
                cmd.Parameters.AddWithValue("@date", expense.Date);
                cmd.Parameters.AddWithValue("@description", expense.Description);
                cmd.Parameters.AddWithValue("@expenseId", expense.ExpenseId);
                cmd.Parameters.AddWithValue("@userId", userId);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    // Nothing was updated – throw custom exception
                    throw new ExpenseNotFoundException("Expense with the given ID was not found or doesn't belong to the user.");
                }

                return true;
            }
        }
    }
}

/*
 * Notes:
 * ExecuteNonQuery()==Use for: INSERT, UPDATE, DELETE, or DDL commands like CREATE TABLE
 * ExecuteReader()==Use for: SELECT queries that return rows of data
 * ExecuteScalar()==Use for: Queries that return a single value (e.g., SELECT COUNT(*), MAX, MIN, or a single column)
 * cmd.ExecuteNonQuery()== Executes the query and returns affected rows
 * Soo, the 'using (SqlConnection con = DBConnection.GetConnection())' is used for automatically closing connections after opening instead of manually closing em.
 * */