using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTechLab.DTOs
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid Id { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public bool IsBanned { get; set; }
        public bool HasSpecialRegistration { get; set; } = true;
        public int minPasswordLengthIfAdmin { get; set; }
    }
}
