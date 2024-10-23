using NewTechLab.DTOs;

namespace NewTechLab.Backend.Model
{
    public static class UsersListContainer
    {
        public static List<User> Users { get; set; }
        public static string FilePath { get; set; }
        public static byte[] AdminKey { get; set; }
    }
}
