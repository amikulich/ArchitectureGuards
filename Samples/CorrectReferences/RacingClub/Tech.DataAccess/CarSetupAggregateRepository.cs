using Tech.Domain;

namespace Tech.DataAccess
{
    public class CarSetupAggregateRepository : ICarSetupAggregateRepository
    {
        public CarSetupAggregate Get(int carId)
        {
            return new CarSetupAggregate();
        }

        public void Save(CarSetupAggregate aggregate)
        {

        }
    }
}