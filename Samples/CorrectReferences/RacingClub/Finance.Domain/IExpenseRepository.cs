using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Domain
{
    public interface IExpenseRepository
    {
        Expense Get(int teamId);
    }
}
