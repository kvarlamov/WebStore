﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Database
{
    public class WebStoreContextInitializer
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WebStoreContextInitializer> _Log;

        public WebStoreContextInitializer(WebStoreContext db, UserManager<User> UserManager, RoleManager<Role> RoleManager, ILogger<WebStoreContextInitializer> log)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _Log = log;
        }

        public async Task InitializeAsync()
        {
            var db = _db.Database;
            //if (await db.EnsureDeletedAsync())
            //{
            //    //База данных существовала и была успешно удалена
            //}

            //await db.EnsureCreatedAsync();
            await db.MigrateAsync(); // Автоматическое создание и миграция базы до последней версии

            await IdentityInitializeAsync();

            if (await _db.Products.AnyAsync()) return;

            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Sections.AddRangeAsync(TestData.Sections);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                transaction.Commit();
            }

            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Brands.AddRangeAsync(TestData.Brands);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                transaction.Commit();
            }

            using (var transaction = await db.BeginTransactionAsync())
            {
                await _db.Products.AddRangeAsync(TestData.Products);

                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await db.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");

                transaction.Commit();
            }
        }

        private async Task IdentityInitializeAsync()
        {
            if(!await _RoleManager.RoleExistsAsync(Role.Administrator))
            {
                _Log.LogInformation("Can't find role in database");
                var result = await _RoleManager.CreateAsync(new Role { Name = Role.Administrator });

                if (result.Succeeded)
                    _Log.LogInformation("Admin Role was added succesfully");
                else
                    _Log.LogError("Error when adding admin role {0}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await _RoleManager.RoleExistsAsync(Role.User))
            {
                var result = await _RoleManager.CreateAsync(new Role { Name = Role.User });

                if (result.Succeeded)
                    _Log.LogInformation("User Role was added succesfully");
                else
                    _Log.LogError("Error when adding user role {0}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if(await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.AdminPasswordDefault);

                if (creation_result.Succeeded)
                {
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                }
                else
                    throw new InvalidOperationException($"Ошибка при создании администратора в БД {string.Join(", ", creation_result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
