using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTechLab.DTOs
{
    public static class PasswordCheckHelper
    {
        public static bool IsPasswordCorrect(string password, int minLength = 5)
        {
            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            if (password == null)
            {
                return false;
            }
            if (password.Length < minLength)
            {
                return false;
            }
            foreach (char c in password)
            {
                if (Char.IsLower(c))
                    hasLower = true;
                if (Char.IsNumber(c))
                    hasDigit = true;
                if (Char.IsUpper(c))
                    hasUpper = true;
            }
            if (hasUpper && hasLower && hasDigit)
            {
                return true;
            }
            return false;
        }
    }
}
