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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.LeaveRequests = new HashSet<LeaveRequest>();
            this.LeaveRequests1 = new HashSet<LeaveRequest>();
        }
    
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public int TeamId { get; set; }
        public int EmployeeTypeId { get; set; }
    
        public virtual EmployeeType EmployeeType { get; set; }
        public virtual Team Team { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveRequest> LeaveRequests1 { get; set; }
    }
}
