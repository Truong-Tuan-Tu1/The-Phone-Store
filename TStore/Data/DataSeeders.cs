using Microsoft.AspNetCore.Identity;
using TStore.Constants;
using TStore.Models;

namespace TStore.Data;

public static class DataSeeders
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
    {
        var userManager = service.GetService<UserManager<AppUser>>();
        var roleManager = service.GetService<RoleManager<IdentityRole>>();

        await roleManager.CreateAsync(new IdentityRole(RoleConstant.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(RoleConstant.Member.ToString()));

        //create Admin

        var user = new AppUser
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        var userInDb = await userManager.FindByEmailAsync(user.Email);
        if (userInDb == null)
        {
            await userManager.CreateAsync(user, "123456");
            await userManager.AddToRoleAsync(user, RoleConstant.Admin.ToString());
        }
    }
}