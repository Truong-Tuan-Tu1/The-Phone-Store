using TStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TStore.Components;

public class ProductHomeViewComponent : ViewComponent
{
    private readonly IProductService productService;

    public ProductHomeViewComponent(IProductService productService)
    {
        this.productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync() => View(this.productService.FindAll().OrderBy(x => x.CreatedAt).Take(4).ToList());
}