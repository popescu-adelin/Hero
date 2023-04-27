using DataLogic.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


namespace DataLogic.Data
{
    public class Seed
    {
        public static async Task SeedUser(DataContext dataContext)
        {
            if (await dataContext.Users.AnyAsync()) return;

            Console.WriteLine("Adding seed....");

            var userData = await File.ReadAllTextAsync("C:\\2023-learning-repo-Adelin\\src\\Hero\\BE\\DataLogic\\Data\\UserSeedData.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var users = JsonSerializer.Deserialize<List<User>>(userData, options);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.HeroName = user.HeroName;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word"));
                user.PasswordSalt = hmac.Key;

                dataContext.Users.Add(user);
            }

            await dataContext.SaveChangesAsync();

        }
    }
}
