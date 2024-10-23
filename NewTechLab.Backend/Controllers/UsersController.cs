using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTechLab.Backend.Model;
using NewTechLab.DTOs;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace NewTechLab.Backend.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string _filePath = "users.txt";
        private readonly bool _updateDbAfterChanges = true;
        private bool hasNoDB;

        public UsersController()
        {
            UsersListContainer.FilePath = _filePath;
            FileInfo info = new FileInfo(_filePath);
            if (!info.Exists)
            {
                hasNoDB = true;
                UsersListContainer.Users = new List<User>() { new User() { Id = Guid.NewGuid(), Login = "ADMIN", Password = "admin", RegistrationDateTime = DateTime.Now } };
            }
        }
        [HttpPost]
        public IActionResult TryToLogin(LoginRequest request)
        {
            var users = UsersListContainer.Users;
            if (users != null && users.FirstOrDefault(x => x.Login == request.Login && x.Password == request.Password) != null)
            {
                var user = users.FirstOrDefault(x => x.Login == request.Login && x.Password == request.Password);
                if (user.IsBanned)
                {
                    return StatusCode(412);
                }
                return Ok(user);
            }
            return BadRequest();
        }
        [HttpGet("DBCondition")]
        public IActionResult GetDBCondition()
        {
            if (hasNoDB)
            {
                return NotFound();
            }
            if (UsersListContainer.Users == null)
            {
                return NoContent();
            }
            return Ok();
        }

        [HttpGet("LoadDB")]
        public IActionResult LoadDB(string code)
        {
            UsersListContainer.AdminKey = CryptoHelper.HashKeyFromString(code);
            if (!hasNoDB)
            {
                CryptoHelper.DecryptUserList();
            }
            if (UsersListContainer.Users.FirstOrDefault(x => x.Login == "ADMIN") != null)
            {
                if (hasNoDB)
                {
                    CryptoHelper.EncryptUserList();
                }
                return Ok();
            } 
            return BadRequest("Неверный код");
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetPassLength(Guid id)
        {
            var users = UsersListContainer.Users;
            var admin = users.FirstOrDefault(x => x.Login == "ADMIN");
            var user = users.FirstOrDefault(x => x.Id == id);
            if (admin != null && user != null)
            {
                return Ok(new RegistrationProps() { MinLegth = admin.minPasswordLengthIfAdmin, UseSpecialCheck = user.HasSpecialRegistration });
            }
            return BadRequest();
        }

        [HttpPost("NewUser")]
        public IActionResult AddNewUser(LoginRequest request)
        {
            var users = UsersListContainer.Users;
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
                UsersListContainer.Users = users;
                if (_updateDbAfterChanges)
                {
                    CryptoHelper.EncryptUserList();
                }
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = UsersListContainer.Users;
            return Ok(users);
        }

        [HttpPost("Update")]
        public IActionResult UpdateUsers(List<User> users)
        {
            UsersListContainer.Users = users;
            if (_updateDbAfterChanges)
            {
                CryptoHelper.EncryptUserList();
            }
            return Ok();
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangeUserPassword(LoginRequest request)
        {
            var users = UsersListContainer.Users;
            if (users != null && users.FirstOrDefault(x => x.Login == request.Login) != null)
            {
                users.FirstOrDefault(x => x.Login == request.Login).Password = request.Password;
                UsersListContainer.Users = users;
                if (_updateDbAfterChanges)
                {
                    CryptoHelper.EncryptUserList();
                }
                return Ok(users.FirstOrDefault(x => x.Login == request.Login));
            }
            return BadRequest();
        }
    }
}
