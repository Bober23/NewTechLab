using NewTechLab.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NewTechLab.Backend.Model
{
    
    public static class CryptoHelper
    {
        public static byte[] HashKeyFromString(string key)
        {
            SHA1 Hash = SHA1.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(key);
            byte[] hash = Hash.ComputeHash(inputBytes); 
            return hash;
        }

        public static void EncryptUserList()
        {
            var usersList = UsersListContainer.Users;
            var key = UsersListContainer.AdminKey;
            var filePath = UsersListContainer.FilePath;
            string usersJson = JsonSerializer.Serialize(usersList);
            byte[] ClearData = Encoding.UTF8.GetBytes(usersJson);
            RC2 Algorithm = RC2.Create();
            Algorithm.Key = key;
            MemoryStream Target = new MemoryStream();
            Algorithm.GenerateIV();
            Target.Write(Algorithm.IV, 0, Algorithm.IV.Length);
            CryptoStream cs = new CryptoStream(Target, Algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(ClearData, 0, ClearData.Length);
            cs.FlushFinalBlock();
            var encryptedBytes = Target.ToArray();
            File.WriteAllBytes(filePath, encryptedBytes);
            
        } 
        public static void DecryptUserList()
        {
            byte[] data = File.ReadAllBytes(UsersListContainer.FilePath);
            RC2 Algorithm = RC2.Create();
            Algorithm.Key = UsersListContainer.AdminKey;
            MemoryStream Target = new MemoryStream();
            int ReadPos = 0;
            byte[] IV = new byte[Algorithm.IV.Length];
            Array.Copy(data, IV, IV.Length);
            Algorithm.IV = IV;
            ReadPos += Algorithm.IV.Length;
    
            CryptoStream cs = new CryptoStream(Target, Algorithm.CreateDecryptor(),
                CryptoStreamMode.Write);
            cs.Write(data, ReadPos, data.Length - ReadPos);
            cs.FlushFinalBlock();
            string usersJson = Encoding.UTF8.GetString(Target.ToArray());
            UsersListContainer.Users = JsonSerializer.Deserialize<List<User>>(usersJson);
            Console.WriteLine(UsersListContainer.Users[0].Login);
        }
    }
}
