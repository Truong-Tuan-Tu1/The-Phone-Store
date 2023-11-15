using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services;

public class AboutService : BaseService<About>, IAboutService
{
    public AboutService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }


}