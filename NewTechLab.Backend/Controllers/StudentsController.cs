using Microsoft.AspNetCore.Mvc;
using NewTechLab.Backend.Model;
using NewTechLab.DTOs;

namespace NewTechLab.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            List<Student> students = new List<Student>();
            using (var dataContext = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                students = dataContext.Student.ToList();
            }
            return Ok(students);
        }
        [HttpPost("Filter")]
        public IActionResult GetFilteredStudents(Filter filter)
        {
            if (filter.FilterDateStart == null)
            {
                filter.FilterDateStart = DateTime.MinValue;
            }
            if (filter.FilterDateEnd == DateTime.MinValue || filter.FilterDateEnd == null)
            {
                filter.FilterDateEnd = DateTime.MaxValue;
            }
            if (filter.MaxAge == 0)
            {
                filter.MaxAge = int.MaxValue;
            }
            IEnumerable<Student> students;
            using (var dataContext = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                IEnumerable<Student> studentList = dataContext.Student.ToList();
                students = studentList.Where(x => x.AdmissionDate > filter.FilterDateStart && x.AdmissionDate < filter.FilterDateEnd && GetAge(x.BirthDate.Value) < filter.MaxAge && GetAge(x.BirthDate.Value) > filter.MinAge);
            }
            return Ok(students);
        }
        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            student.Id = Guid.NewGuid();
            using (var dataContext = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                dataContext.Student.Add(student);
                await dataContext.SaveChangesAsync();
            }
            return Ok();
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteStudents(List<Student> students)
        {
            using (var dataContext = new DataContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DataContext>()))
            {
                foreach (var student in students)
                {
                    dataContext.Student.Remove(student);
                    await dataContext.SaveChangesAsync();
                }
            }
            return Ok();
        }
        private int GetAge(DateTime birthday)
        {
            int age = DateTime.Now.Year - birthday.Year;
            if (DateTime.Now.Month < DateTime.Now.Month || (DateTime.Now.Month == birthday.Month && DateTime.Now.Day < birthday.Day))
            {
                age--;
            }
            return age+8;
        }
    }
}
