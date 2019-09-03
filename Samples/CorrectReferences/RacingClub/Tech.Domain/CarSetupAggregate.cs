namespace Tech.Domain
{
    public class CarSetupAggregate
    {
        public double Pressure { get; private set; }

        public double Temperature { get; private set; }

        public void InstallTyres(double pressure)
        {
            Pressure = pressure;
            //uncomment this to break architecture consistency
            //var expense = new Expense();
        }
    }
}