using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Core;
using Finance.Domain;

namespace Finance.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public ExpenseDto GetExpense(int teamId)
        {
            var aggregate = _expenseRepository.Get(teamId);

            return null;
        }
    }
}
