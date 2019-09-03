namespace Finance.Core
{
    public interface IExpenseService
    {
        ExpenseDto GetExpense(int teamId);
    }

    public class ExpenseDto
    {

    }
}