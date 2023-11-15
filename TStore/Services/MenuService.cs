using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;
using TStore.Utils;

namespace TStore.Services;

public class MenuService : BaseService<Menu>, IMenuService
{
    public MenuService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override bool Create(Menu entity)
    {

        entity.Alias = HandleSeoName.GenerateSEONameNoPrefix(entity.Title);

        return base.Create(entity);
    }

}