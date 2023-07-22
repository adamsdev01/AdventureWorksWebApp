namespace AdventureWorksWebApp.ViewModels
{
    public class EmployeeViewModel
    {
        
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }    
        public string JobTitle { get; set; }
        public DateTime HireDate { get; set; }
        public bool? IsSalaried { get; set; }
    }
}
