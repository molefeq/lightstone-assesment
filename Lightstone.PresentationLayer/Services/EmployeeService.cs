using Lightstone.DataAccess.Repository;
using Lightstone.PresentationLayer.ModelBuilders;
using Lightstone.PresentationLayer.ViewModels;

namespace Lightstone.PresentationLayer.Services
{
    public class EmployeeService
    {
        private static EmployeeService _Instance;

        public static EmployeeService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new EmployeeService();
                }

                return _Instance;
            }
        }

        private EmployeeService() { }

        public EmployeeViewModel Login(string emailAddress)
        {
            using(UnitOfWork unitOfWork = new UnitOfWork())
            {
                return EmployeeModelBuilder.Instance.MapToViewModel(unitOfWork.EmployeeRepository.GetEmployeeByEmail(emailAddress, "Team,EmployeeType"));
            }
        }
    }
}
