using Lab4.Models.User;
using System.Text.Json;

namespace Lab4.Utils
{
    public static class AccountFileStorage
    {
        private static string folder = "accounts";

        public static UserModel LoadData(string email, string password, bool isCheckPassword = true)
        {
            string content = "";
            var path = Path.Combine(Directory.GetCurrentDirectory(), folder, email);
            if(!File.Exists(path)) return null;
            using(var reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }
            var model = JsonSerializer.Deserialize<UserModel>(content);
            if (isCheckPassword && !model.Password.Equals(password)) return null;
            return model;
        }
        public static bool IsExist(string email)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), folder, email);
            return File.Exists(path);
        }
        public static void SaveData(UserModel user)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), folder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            path = Path.Combine(path, user.Email);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string content = JsonSerializer.Serialize(user);
            using(var writer = new StreamWriter(path))
            {
                writer.Write(content);
            }
        }
    }
}
