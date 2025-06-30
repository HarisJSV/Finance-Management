using DAO;
using entity;
using MyExceptions;
using System;
using System.Collections.Generic;

namespace FinanceManagement
{
    public class FinanceApp
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;// To include the indian currency
            IFinanceRepository repo = new FinanceRepositoryImpl();
            int loggedInUserId;

            while (true)
            {
                Console.WriteLine("\n--- Welcome to Finance Management System ---");
                Console.WriteLine("1. Register (User)");
                Console.WriteLine("2. Login as User");
                Console.WriteLine("3. Login as Admin");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Enter username: ");
                        string uname = Console.ReadLine();

                        Console.Write("Enter password: ");
                        string pass = Console.ReadLine();

                        Console.Write("Enter email: ");
                        string email = Console.ReadLine();

                        User newUser = new User { Username = uname, Password = pass, Email = email };
                        Console.WriteLine(repo.CreateUser(newUser) ? "User registered!" : "Registration failed.");
                        break;

                    case "2":
                        try
                        {
                            Console.Write("Enter username: ");
                            string uuser = Console.ReadLine();

                            Console.Write("Enter password: ");
                            string upass = Console.ReadLine();

                            loggedInUserId = repo.LoginUser(uuser, upass);
                            Console.WriteLine("User logged in successfully!");
                            ShowUserMenu(repo, loggedInUserId);
                        }
                        catch (UserNotFoundException e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                        break;


                    case "3":
                        Console.Write("Enter admin username: ");
                        string adminUser = Console.ReadLine();

                        Console.Write("Enter admin password: ");
                        string adminPass = Console.ReadLine();

                        if (repo.LoginAdmin(adminUser, adminPass))
                        {
                            Console.WriteLine("Admin logged in successfully!");
                            ShowAdminMenu(repo);
                        }
                        else Console.WriteLine("Admin login failed.");
                        break;

                    case "0":
                        Console.WriteLine("Exited.");
                        return;

                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
        static void ShowUserMenu(IFinanceRepository repo, int userId)
        {
            while (true)
            {
                Console.WriteLine("\n--- User Menu ---");
                Console.WriteLine("1. Add Expense");
                Console.WriteLine("2. View My Expenses");
                Console.WriteLine("3. Update Expense");
                Console.WriteLine("4. Delete User");
                Console.WriteLine("5. Delete Expense");
                Console.WriteLine("6. Expense Report");
                Console.WriteLine("0. Logout");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine());

                        Console.Write("Enter category ID: ");
                        int categoryId = int.Parse(Console.ReadLine());

                        Console.Write("Enter description: ");
                        string desc = Console.ReadLine();

                        Expense newExp = new Expense
                        {
                            UserId = userId,
                            Amount = amount,
                            CategoryId = categoryId,
                            Date = DateTime.Now,
                            Description = desc
                        };

                        Console.WriteLine(repo.CreateExpense(newExp) ? "Expense added!" : "Failed to add expense.");
                        break;

                    case "2":
                        List<Expense> userExpenses = repo.GetAllExpenses(userId);

                        if (userExpenses.Count == 0)
                        {
                            Console.WriteLine("No expenses found.");
                        }
                        else
                        {
                            Console.WriteLine($"\n--- Your Expenses ---");
                            foreach (var exp in userExpenses)
                            {
                                Console.WriteLine($"ID: {exp.ExpenseId}, Category: {exp.CategoryName}, Amount: {exp.Amount}, Date: {exp.Date.ToShortDateString()}, Desc: {exp.Description}");
                            }
                        }
                        break;

                    case "3":
                        try
                        {
                            Console.Write("Enter Expense ID to update: ");
                            int updateId = int.Parse(Console.ReadLine());

                            Console.Write("Enter new amount: ");
                            decimal newAmt = decimal.Parse(Console.ReadLine());

                            Console.Write("Enter new category ID: ");
                            int newCatId = int.Parse(Console.ReadLine());

                            Console.Write("Enter new description: ");
                            string newDesc = Console.ReadLine();

                            Expense updatedExp = new Expense
                            {
                                ExpenseId = updateId,
                                UserId = userId,
                                Amount = newAmt,
                                CategoryId = newCatId,
                                Date = DateTime.Now,
                                Description = newDesc
                            };

                            Console.WriteLine(repo.UpdateExpense(userId, updatedExp) ? "Expense updated!" : "Failed to update.");
                        }
                        catch (ExpenseNotFoundException e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                        break;

                    case "4":
                        Console.Write("Are you sure you want to delete your account?(Yes/No):");
                        string confimation = Console.ReadLine();
                        if (confimation.ToLower() == "yes")
                        {
                            Console.WriteLine(repo.DeleteUser(userId) ? "User deleted!" : "User deletion failed.");
                            return;

                        }
                        else
                            Console.WriteLine("Was worried we were gonna lose you there :)");
                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter Expense ID to delete: ");
                            int delExpId = int.Parse(Console.ReadLine());

                            Console.WriteLine(repo.DeleteExpense(delExpId, userId) ? "Expense deleted!" : "Deletion failed.");
                        }
                        catch (ExpenseNotFoundException e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                        break;


                    case "6":
                        Console.Write("Enter start date (yyyy-mm-dd): ");
                        DateTime start = DateTime.Parse(Console.ReadLine());

                        Console.Write("Enter end date (yyyy-mm-dd): ");
                        DateTime end = DateTime.Parse(Console.ReadLine());

                        List<Expense> report = repo.GetExpensesByDateRange(userId, start, end);
                        if (report.Count == 0)
                            Console.WriteLine("No expenses found in this range.");
                        else
                        {
                            decimal total = 0;
                            Console.WriteLine($"\n--- Report from {start.ToShortDateString()} to {end.ToShortDateString()} ---");
                            foreach (var exp in report)
                            {
                                Console.WriteLine($"₹{exp.Amount} on {exp.Date.ToShortDateString()} - {exp.CategoryName}: {exp.Description}");
                                total += exp.Amount;
                            }
                            Console.WriteLine($"\nTotal: ₹{total}");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Logging out...");
                        return;

                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }

        static void ShowAdminMenu(IFinanceRepository repo)
        {
            while (true)
            {
                Console.WriteLine("\n--- Admin Menu ---");
                Console.WriteLine("1. Add User");
                Console.WriteLine("2. Delete User");
                Console.WriteLine("3. Add Expenses");
                Console.WriteLine("4. Delete Expense");
                Console.WriteLine("5. Update Expense");
                Console.WriteLine("6. View All Expenses (by User ID)");
                Console.WriteLine("7. Expense Report");
                Console.WriteLine("0. Logout");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter username: ");
                        string uname = Console.ReadLine();

                        Console.Write("Enter password: ");
                        string pass = Console.ReadLine();

                        Console.Write("Enter email: ");
                        string email = Console.ReadLine();

                        User newUser = new User { Username = uname, Password = pass, Email = email };
                        Console.WriteLine(repo.CreateUser(newUser) ? "User added!" : "User creation failed.");
                        break;

                    case "2":
                        Console.Write("Enter user ID to delete: ");
                        int userId = int.Parse(Console.ReadLine());

                        Console.WriteLine(repo.DeleteUser(userId) ? "User deleted!" : "User deletion failed.");
                        break;

                    case "3":
                        Console.Write("Enter user ID to add expenses: ");
                        int newuserId = int.Parse(Console.ReadLine());

                        Console.Write("Enter amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine());

                        Console.Write("Enter category ID: ");
                        int categoryId = int.Parse(Console.ReadLine());

                        Console.Write("Enter description: ");
                        string desc = Console.ReadLine();

                        Expense newExp = new Expense
                        {
                            UserId = newuserId,
                            Amount = amount,
                            CategoryId = categoryId,
                            Date = DateTime.Now,
                            Description = desc
                        };

                        Console.WriteLine(repo.CreateExpense(newExp) ? "Expense added!" : "Failed to add expense.");
                        break;

                    case "4":
                        Console.Write("Enter expense ID to delete: ");
                        int expId = int.Parse(Console.ReadLine());

                        Console.WriteLine(repo.AdminDeleteExpense(expId) ? "Expense deleted!" : "Expense deletion failed.");
                        break;

                    case "5":
                        Console.Write("Enter expense ID to update: ");
                        int expUpdId = int.Parse(Console.ReadLine());

                        Console.Write("Enter user ID: ");
                        int uid = int.Parse(Console.ReadLine());

                        Console.Write("Enter new amount: ");
                        decimal newAmt = decimal.Parse(Console.ReadLine());

                        Console.Write("Enter new category ID: ");
                        int catId = int.Parse(Console.ReadLine());

                        Console.Write("Enter new description: ");
                        string newDesc = Console.ReadLine();

                        Expense updated = new Expense
                        {
                            ExpenseId = expUpdId,
                            UserId = uid,
                            Amount = newAmt,
                            CategoryId = catId,
                            Date = DateTime.Now,
                            Description = newDesc
                        };

                        Console.WriteLine(repo.UpdateExpense(uid, updated) ? "Expense updated!" : "Update failed.");
                        break;

                    case "6":
                        Console.Write("Enter user ID to view expenses: ");
                        int listUid = int.Parse(Console.ReadLine());

                        List<Expense> userExpenses = repo.GetAllExpenses(listUid);

                        if (userExpenses.Count == 0)
                        {
                            Console.WriteLine("No expenses found.");
                        }
                        else
                        {
                            Console.WriteLine($"--- Expenses for user {userExpenses[0].Username} ---");
                            foreach (var exp in userExpenses)
                            {
                                Console.WriteLine($"ID: {exp.ExpenseId}, Category: {exp.CategoryName}, Amount: {exp.Amount}, Date: {exp.Date.ToShortDateString()}, Desc: {exp.Description}");
                            }
                        }
                        break;

                    case "7":
                        Console.Write("Enter user ID to generate an expense report: ");
                        int reportuserId = int.Parse(Console.ReadLine());

                        Console.Write("Enter start date (yyyy-mm-dd): ");
                        DateTime start = DateTime.Parse(Console.ReadLine());

                        Console.Write("Enter end date (yyyy-mm-dd): ");
                        DateTime end = DateTime.Parse(Console.ReadLine());

                        List<Expense> report = repo.GetExpensesByDateRange(reportuserId, start, end);
                        if (report.Count == 0)
                            Console.WriteLine("No expenses found in this range.");
                        else
                        {
                            decimal total = 0;
                            Console.WriteLine($"\n--- Report from {start.ToShortDateString()} to {end.ToShortDateString()} ---");
                            foreach (var exp in report)
                            {
                                Console.WriteLine($"₹{exp.Amount} on {exp.Date.ToShortDateString()} - {exp.CategoryName}: {exp.Description}");
                                total += exp.Amount;
                            }
                            Console.WriteLine($"\nTotal: ₹{total}");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Logging out...");
                        return;

                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
    }
}
