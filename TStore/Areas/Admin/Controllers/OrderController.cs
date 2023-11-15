using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TStore.Interfaces;

namespace TStore.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrderController : Controller
{
    private readonly IOrderDetailService orderDetailService;
    private readonly IOrdersService ordersService;

    public OrderController(IOrderDetailService orderDetailService, IOrdersService ordersService)
    {
        this.orderDetailService = orderDetailService;
        this.ordersService = ordersService;
    }
    public IActionResult Index()
    {
        var orders = ordersService.FindAll();

        return View(orders);
    }

    public IActionResult Detail(int? id)
    {
        if (!id.HasValue)
        {
            return NotFound();
        }



        var orderDetail = orderDetailService.FindAll().Where(x => x.OrderId == id).ToList();
        var totalQuantity = 0;
        var totalPriceAll = 0;


        foreach (var item in orderDetail)
        {
            totalQuantity += item.Quantity;
            totalPriceAll += item.Price;
        }

        ViewBag.TOTAL_QUANTITY = totalQuantity;
        ViewBag.TOTAL_PRICE_ALL = totalPriceAll;
        return View(orderDetail);
    }

}