using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Core
{
    public interface ICarSetupService
    {
        void InstallTyres(int carId, double pressure);
    }

    public class TyreDto
    {
    }
}
