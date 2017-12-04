using System.Linq;

namespace Lightstone.DataAccess.Repository
{
    public class RequestStatusRepository : GenericRepository<RequestStatu>
    {
        public RequestStatusRepository(LightstoneEntities context) : base(context) { }

        public virtual RequestStatu GetRequestStatusByCode(string requestStatusCode)
        {
            IQueryable<RequestStatu> query = from status in _Context.RequestStatus
                                         where status.RequestStatusCode == requestStatusCode
                                         select status;

            return query.FirstOrDefault<RequestStatu>();
        }
    }
}
