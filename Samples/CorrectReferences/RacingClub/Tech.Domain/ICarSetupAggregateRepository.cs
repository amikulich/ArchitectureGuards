namespace Tech.Domain
{
    public interface ICarSetupAggregateRepository
    {
        CarSetupAggregate Get(int carId);
        void Save(CarSetupAggregate aggregate);
    }
}