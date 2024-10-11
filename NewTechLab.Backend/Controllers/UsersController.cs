using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTechLab.DTOs;
using System.Xml.Serialization;

namespace NewTechLab.Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _filePath = "users.txt";
        public UsersController()
        {
            FileInfo info = new FileInfo(_filePath);
            if (!info.Exists)
            {
                SerializeAndSave(_filePath, new List<User>() { new User() { Id = Guid.NewGuid(), Login = "ADMIN", Password = "admin", RegistrationDateTime = DateTime.Now } });
            }
        }
        [HttpPost]
        public IActionResult TryToLogin(LoginRequest request)
        {
            var users = ReadAndDeserialize(_filePath);
            if (users != null && users.FirstOrDefault(x => x.Login == request.Login && x.Password == request.Password) != null)
            {
                return Ok(users.FirstOrDefault(x => x.Login == request.Login && x.Password == request.Password));
            }
            return BadRequest();
        }

        [HttpPost("NewUser")]
        public IActionResult AddNewUser(LoginRequest request)
        {
            var users = ReadAndDeserialize(_filePath);
            if (users != null)
            {
                var user = new User
                {
                    Login = request.Login,
                    Password = request.Password,
                    RegistrationDateTime = DateTime.Now,
                    Id = Guid.NewGuid(),
                };
                users.Add(user);
                SerializeAndSave(_filePath, users);
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = ReadAndDeserialize(_filePath);
            return Ok(users);
        }

        [HttpPost]
        public IActionResult UpdateUsers(List<User> users)
        {
            SerializeAndSave(_filePath, users);
            return Ok();
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangeUserPassword(LoginRequest request)
        {
            var users = ReadAndDeserialize(_filePath);
            if (users != null && users.FirstOrDefault(x => x.Login == request.Login) != null)
            {
                users.FirstOrDefault(x => x.Login == request.Login).Password = request.Password;
                SerializeAndSave(_filePath, users);
                return Ok(users.FirstOrDefault(x => x.Login == request.Login));
            }
            return BadRequest();
        }

        private List<User> ReadAndDeserialize(string path)
        {
            var serializer = new XmlSerializer(typeof(List<User>));
            using (var reader = new StreamReader(path))
            {
                return (List<User>)serializer.Deserialize(reader);
            }
        }

        private void SerializeAndSave(string path, List<User> data)
        {
            var serializer = new XmlSerializer(typeof(List<User>));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, data);
            }
        }
    }
}
