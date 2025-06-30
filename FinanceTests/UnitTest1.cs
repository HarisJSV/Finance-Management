using DAO;
using entity;
using NUnit.Framework;
using System;
using Utils;
using MyExceptions; 
namespace FinanceSystemTests
{
    public class Tests
    {
        [Test]
        public void SuccessfulCreateUser()//Tests whether the user is created successfully
        {
            var repo = new FinanceRepositoryImpl();
            var user = new User
            {
                Username = "testuser12",
                Password = "password12",
                Email = "test@example12.com"
            };

            var result = repo.CreateUser(user);

            Assert.That(result, Is.True, "User should be created successfully.");//Since we are using NUnit 4+, we gotta use constraint based assert functions since the old ones like Assert.isnull are deprecated.(I suffered for hours)
        }

        [Test]
        public void SuccessfulCreateExpense()//tests whether expense has been created successfully
        {
            var repo = new FinanceRepositoryImpl();
            int userId = repo.LoginUser("testuser", "password123"); // Use a user from the DB or create a new one
            var expense = new Expense
            {
                UserId = userId,
                Amount = 500,
                CategoryId = 1,
                Date = DateTime.Now,
                Description = "Groceries"
            };

            var result = repo.CreateExpense(expense);

            Assert.That(result, Is.True, "Expense should be added successfully.");
        }

        [Test]
        public void GetAllExpensesbasedonUserID()
        {
            var repo = new FinanceRepositoryImpl();
            int userId = repo.LoginUser("testuser", "password123");
            var expenses = repo.GetAllExpenses(userId);

            Assert.That(expenses, Is.Not.Null, "Expense list should not be null.");
            Assert.That(expenses.Count, Is.GreaterThanOrEqualTo(0), "Expenses list should be returned.");
        }


        [Test]
        public void LoginUser_UserNotFoundException()//It should throw usernotfound exception upon entering invalid credentials
        {
            
            var repo = new FinanceRepositoryImpl();

            Assert.Throws<UserNotFoundException>(() =>
            {
                repo.LoginUser("nonexistent", "wrongpassword");
            });
        }

        [Test]
        public void UpdateExpense_ThrowExpenseNotFoundException()//It should throw Exceptionnotfound exception upon entering invalid expense id.
        {
            var repo = new FinanceRepositoryImpl();
            int userId = repo.LoginUser("testuser", "password123");

            var expense = new Expense
            {
                ExpenseId = -999, 
                UserId = userId,
                Amount = 100,
                CategoryId = 1,
                Date = DateTime.Now,
                Description = "Invalid"
            };

            Assert.Throws<ExpenseNotFoundException>(() =>
            {
                repo.UpdateExpense(userId, expense);
            });
        }

    }
}
