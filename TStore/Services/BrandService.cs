using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;
using TStore.Utils;

namespace TStore.Services;

public class BrandService : BaseService<Brand>, IBrandService
{
    public BrandService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }


    public override bool Create(Brand entity)
    {
        entity.SeoName = HandleSeoName.GenerateSEOName(entity.Name);
        return base.Create(entity);
    }

    public override bool Update(Brand entity)
    {
        return base.Update(entity);
    }
}