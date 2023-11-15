using Microsoft.AspNetCore.Mvc;
using TStore.Interfaces;

namespace TStore.Components;

public class MenuTopViewComponent : ViewComponent
{
    private readonly IMenuService menuService;

    public MenuTopViewComponent(IMenuService menuService)
    {
        this.menuService = menuService;
    }

    public async Task<IViewComponentResult> InvokeAsync() => View(this.menuService.FindAll().OrderBy(x => x.Position).ToList());
}