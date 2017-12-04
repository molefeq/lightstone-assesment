namespace Lightstone.PresentationLayer.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public int TeamId { get; set; }  
        public string TeamName { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EmployeeType { get; set; }
        public bool IsManager { get; set; }      
    }
}
