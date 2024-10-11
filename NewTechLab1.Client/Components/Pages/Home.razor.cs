using NewTechLab.DTOs;
using System.Net.Http.Json;

namespace NewTechLab1.Client.Components.Pages
{
    partial class Home
    {
        private Student _newStudent = new Student();
        private bool _loading;
        private List<Student> _students = new List<Student>();
        private HashSet<Student> _selectedStudents = new HashSet<Student>();
        private Filter filter = new Filter();
        
        protected override async Task OnInitializedAsync()
        {
            using (var client = new HttpClient())
            {
                _students = await client.GetFromJsonAsync<List<Student>>("http://localhost:5282/Students");
            }
            await base.OnInitializedAsync();
        }

        private async Task OnLoadButtonClicked()
        {
            if (_newStudent.BirthDate > DateTime.Now || _newStudent.BirthDate == DateTime.MinValue) 
            {
                return;
            }
            if (_newStudent.AdmissionDate > DateTime.Now || _newStudent.AdmissionDate<_newStudent.BirthDate || _newStudent.AdmissionDate == DateTime.MinValue)
            {
                return;
            }
            if (_newStudent.Fio == string.Empty || _newStudent.NumberZk == string.Empty)
            {
                return;
            }
            _loading = true;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync<Student>("http://localhost:5282/Students", _newStudent);
                if (response.IsSuccessStatusCode)
                {
                    _newStudent = new Student();
                    _students = await client.GetFromJsonAsync<List<Student>>("http://localhost:5282/Students");
                }
            }
            _loading = false;
        }
        private async Task OnDeleteButtonClicked()
        {
            _loading = true;
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync<List<Student>>("http://localhost:5282/Students/Delete", _selectedStudents.ToList());
                if (response.IsSuccessStatusCode)
                {
                    _newStudent = new Student();
                    _students = await client.GetFromJsonAsync<List<Student>>("http://localhost:5282/Students");
                }
            }
            _loading = false;
        }
        private async Task OnFilterButtonClicked()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync<Filter>("http://localhost:5282/Students/Filter", filter);
                _students = await response.Content.ReadFromJsonAsync<List<Student>>();
            }
        }
        private async Task OnFilterClearButtonClicked()
        {
            using (var client = new HttpClient())
            {
                filter = new Filter();
                _students = await client.GetFromJsonAsync<List<Student>>("http://localhost:5282/Students");
            }
        }
    }
}
