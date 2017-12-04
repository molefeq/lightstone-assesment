using Lightstone.Common.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lightstone.DataAccess.Repository
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>
    {
        public LeaveRequestRepository(LightstoneEntities context) : base(context) { }

        public virtual List<LeaveRequest> GetLeaveRequests(int? employeeId, int? teamId, string searchText, PagingObject pagingObject = null,
                                                                                    Expression<Func<LeaveRequest, object>> orderExpression = null, string includeProperties = "")
        {
            IQueryable<LeaveRequest> query = (from leaveRequest in _Context.LeaveRequests
                                              select leaveRequest);

            if (query == null)
            {
                return new List<LeaveRequest>();
            }

            if (teamId != null)
            {
                query = from leaveRequest in query
                        join employee in _Context.Employees on leaveRequest.EmployeeId equals employee.EmployeeId
                        where employee.TeamId == teamId.Value
                        select leaveRequest;
            }
            else
            {
                query = from leaveRequest in query
                        where leaveRequest.EmployeeId == employeeId.Value
                        select leaveRequest;
            }
            

            if (!string.IsNullOrEmpty(searchText))
            {
                query = from leaveRequest in query
                        where leaveRequest.LeaveDescription.Contains(searchText)
                        select leaveRequest;
            }

            if (query == null)
            {
                return new List<LeaveRequest>();
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = Include(query, includeProperties);
            }

            if (pagingObject != null)
            {
                return GetPagedEntityData(query, orderExpression, pagingObject).ToList();
            }

            return query.ToList();
        }
    }
}
