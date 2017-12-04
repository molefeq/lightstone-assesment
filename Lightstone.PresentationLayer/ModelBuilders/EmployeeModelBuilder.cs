using Lightstone.DataAccess;
using Lightstone.PresentationLayer.ViewModels;

namespace Lightstone.PresentationLayer.ModelBuilders
{
    public class EmployeeModelBuilder
    {
        private static EmployeeModelBuilder _Instance;

        public static EmployeeModelBuilder Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new EmployeeModelBuilder();
                }

                return _Instance;
            }
        }

        private EmployeeModelBuilder() { }

        public EmployeeViewModel MapToViewModel(Employee employee)
        {
            if (employee == null)
            {
                return null;
            }

            EmployeeViewModel employeeViewModel = new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                EmailAddress = employee.EmailAddress,
                TeamId = employee.TeamId,
                TeamName = employee.Team.TeamName,
                IsManager = employee.EmployeeType.IsManager,
                EmployeeNumber = employee.EmployeeNumber,
                EmployeeType = employee.EmployeeType.EmployeeTypeName
            };

            return employeeViewModel;
        }
    }
}
