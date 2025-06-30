using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using entity;

namespace DAO
{
    public interface IFinanceRepository
    {
        bool CreateUser(User user);
        bool CreateExpense(Expense expense);
        bool DeleteUser(int userId);
        bool DeleteExpense(int expenseId,int userId);
        List<Expense> GetAllExpenses(int userId);
        bool UpdateExpense(int userId, Expense expense);
        bool LoginAdmin(string username, string password);

        bool AdminDeleteExpense(int expenseId);
        int LoginUser(string username, string password);

        List<Expense> GetExpensesByDateRange(int userId, DateTime startDate, DateTime endDate);

        User GetUserByEmail(string email);
    }
        
}


