using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorksWebApp.Models
{
    public partial class Person
    {
        [NotMapped]
        public string? FullName { get; set; }
    }
}
