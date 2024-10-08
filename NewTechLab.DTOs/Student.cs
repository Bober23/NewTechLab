using System.ComponentModel.DataAnnotations;

namespace NewTechLab.DTOs
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }
        public string Fio { get; set; }
        public string NumberZk { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
    }
}
