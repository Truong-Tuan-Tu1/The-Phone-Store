using Microsoft.AspNetCore.Identity;

namespace TStore.Models;

public class AppUser : IdentityUser
{
    public string? Address { get; set; }
    public string? FullName { get; set; }
}