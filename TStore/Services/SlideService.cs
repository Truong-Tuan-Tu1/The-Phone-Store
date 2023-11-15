using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services;

public class SlideService : BaseService<Slide>, ISlideService
{
    public SlideService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}