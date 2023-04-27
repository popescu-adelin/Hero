using DataLogic.Enities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataLogic.Data
{
    public class Seed
    {
        public static async Task SeedUser(DataContext dataContext)
        {
            if (await dataContext.Users.AnyAsync()) return;

            Console.WriteLine("Adding seed from \n" + Environment.CurrentDirectory + "//DataLogic\\Data\\UserSeedData.json");

            var userData = await File.ReadAllTextAsync(Environment.CurrentDirectory + "\\Data\\UserSeedData.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var users = JsonSerializer.Deserialize<List<User>>(userData, options);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.Username = user.Username.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word"));
                user.PasswordSalt = hmac.Key;

                dataContext.Users.Add(user);
            }

            await dataContext.SaveChangesAsync();

        }
    }
}
