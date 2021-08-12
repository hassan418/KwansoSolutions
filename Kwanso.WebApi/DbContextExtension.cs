using Kwanso.Model.Poco;
using Kwanso.Repository.DataAccess;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwanso.WebApi
{
    public static class DbContextExtension
    {
        public static bool AllMigrationsApplied(this KwansoContext context)
        {
            if (context != null)
            {
                var applied = context.GetService<IHistoryRepository>()
                    .GetAppliedMigrations()
                    .Select(m => m.MigrationId);

                var total = context.GetService<IMigrationsAssembly>()
                    .Migrations
                    .Select(m => m.Key);

                return !total.Except(applied).Any();
            }
            else
            {
                return true;
            }
        }

        public static void EnsureSeeded(this KwansoContext context)
        {
            SeedRegisterUser(context);
        }

        private static void SeedRegisterUser(KwansoContext context)
        {
            var users = context.Users.ToList();
            if(users.Count() < 1)
            {
                Users user = new Users
                {
                    Email = "user@domain.com",
                    Password = "password",
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now,
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
