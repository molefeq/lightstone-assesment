//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lightstone.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestStatu()
        {
            this.LeaveRequests = new HashSet<LeaveRequest>();
        }
    
        public int RequestStatusId { get; set; }
        public string RequestStatusCode { get; set; }
        public string RequestStatusName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}
