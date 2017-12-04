using System.Linq;

namespace Lightstone.DataAccess.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>
    {
        public EmployeeRepository(LightstoneEntities context) : base(context) { }

        public virtual Employee GetEmployeeByEmail(string emailAddress, string includeProperties = "")
        {
            IQueryable<Employee> query = from employee in _Context.Employees
                                         where employee.EmailAddress == emailAddress
                                         select employee;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = Include(query, includeProperties);
            }

            return query.FirstOrDefault<Employee>();
        }
    }
}
