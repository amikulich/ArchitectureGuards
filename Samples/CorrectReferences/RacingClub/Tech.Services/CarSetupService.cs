using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Core;
using Tech.Domain;

namespace Tech.Services
{
    public class CarSetupService : ICarSetupService
    {
        private readonly ICarSetupAggregateRepository _carSetupAggregateRepository;

        public CarSetupService(ICarSetupAggregateRepository carSetupAggregateRepository)
        {
            _carSetupAggregateRepository = carSetupAggregateRepository;
        }

        public void InstallTyres(int carId, double pressure)
        {
            var aggregate = _carSetupAggregateRepository.Get(carId);

            aggregate.InstallTyres(pressure);

            _carSetupAggregateRepository.Save(aggregate);
        }
    }
}
