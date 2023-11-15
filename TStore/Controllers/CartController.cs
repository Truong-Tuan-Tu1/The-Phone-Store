using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Controllers
{

    public class CartController : Controller
    {
        private readonly IOrderDetailService orderDetailService;
        private readonly IOrdersService ordersService;
        private readonly IProductService productService;
        private readonly UserManager<AppUser> userManager;
        private readonly ICartService cartService;
        private readonly ICartItemService cartItemService;

        public CartController(IOrderDetailService orderDetailService, IOrdersService ordersService, IProductService productService, UserManager<AppUser> userManager, ICartService cartService, ICartItemService cartItemService)
        {
            this.orderDetailService = orderDetailService;
            this.ordersService = ordersService;
            this.productService = productService;
            this.userManager = userManager;
            this.cartService = cartService;
            this.cartItemService = cartItemService;
        }

        [HttpPost("/api/v1/cart/update-quantity")]
        public async Task<IActionResult> UpdateQuantityView(int id, int quantity)
        {
            var result = cartItemService.UpdateQuantity(id, quantity);
            if (!result)
            {
                return Json(new { statusCode = 400, msg = "FAILED", data = result });
            }

            return Json(new { statusCode = 200, msg = "SUCCESS", data = result });
        }

        public async Task<IActionResult> Index(string type)
        {
            var user = User.Identity;
            string currentUserId = "";
            if (user != null && user.IsAuthenticated)
            {
                currentUserId = user.Name;
            }

            if (!user.IsAuthenticated)
            {
                return Redirect("/dang-nhap");
            }


            var currentUser = await userManager.FindByEmailAsync(currentUserId);
            var currentCart = cartService.FindAll().Where(x => x.UserId == currentUser.Id).FirstOrDefault();

            if (currentCart != null)
            {
                var productsInCart = from item in cartItemService.FindAll()
                                     join product in productService.FindAll()
                                     on item.ProductId equals product.Id
                                     where item.CartId == currentCart.Id
                                     select new
                                     {
                                         CartItemId = item.Id,
                                         Id = product.Id,
                                         Name = product.ProductName,
                                         Quantity = item.Quantity,
                                         ThumbnailFilePath = product.ThumbnailFilePath,
                                         Desc = product.Desc,
                                         Price = product.Price,
                                         TotalPrice = item.Quantity * product.Price,
                                     };

                if (!string.IsNullOrEmpty(type) && type.ToUpper().Equals("AJAX"))
                {
                    return Json(new { statusCode = 200, msg = "SUCCESS", data = productsInCart });
                }

                ViewBag.PRODUCT_IN_CART = productsInCart;
                ViewBag.COUNT_ITEM = productsInCart.Count();
            }
            else
            {
                ViewBag.PRODUCT_IN_CART = null;
                ViewBag.COUNT_ITEM = 0;
            }

            ViewBag.USER_ADDRESS = currentUser.Address;
            ViewBag.USER_FULLNAME = currentUser.FullName;
            return View();
        }

        [HttpGet("/api/v1/cart/count")]
        public async Task<IActionResult> GetCountCart()
        {
            var user = User.Identity;
            string currentUserId = "";
            if (user != null && user.IsAuthenticated)
            {
                currentUserId = user.Name;
            }

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { statusCode = 400, msg = "FAILED", data = 0 });
            }

            var currentUser = await userManager.FindByEmailAsync(currentUserId);
            var currentCart = cartService.FindAll().Where(x => x.UserId == currentUser.Id).FirstOrDefault();
            if (currentCart == null)
            {
                return Json(new { statusCode = 400, msg = "FAILED", data = 0 });
            }

            int count = cartItemService.FindAll().Where(x => x.CartId == currentCart.Id).GroupBy(x => x.ProductId).Count();

            return Json(new { statusCode = 200, msg = "SUCCESS", data = count });
        }

        [HttpPost("/api/v1/cart/add")]
        public async Task<IActionResult> AddCart(int productId, int quantity)
        {
            var user = User.Identity;
            string currentUserId = "";
            if (user != null && user.IsAuthenticated)
            {
                currentUserId = user.Name;
            }

            if (!user.IsAuthenticated)
            {
                return Redirect("/dang-nhap");
            }

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { statusCode = 400, msg = "FAILED", data = 0 });
            }

            var currentUser = await userManager.FindByEmailAsync(currentUserId);
            var product = productService.FindById(productId);
            var cartUser = cartService.FindAll().Where(x => x.UserId == currentUser.Id).FirstOrDefault();

            if (cartUser == null)
            {
                Cart cart = new Cart { UserId = currentUser.Id };
                cartService.Create(cart);
            }

            var currentCart = cartService.FindAll().Where(x => x.UserId == currentUser.Id).First();

            var cartItemExists = cartItemService.FindAll().Where(x => x.ProductId == product.Id && x.CartId == currentCart.Id).FirstOrDefault();

            if (cartItemExists != null)
            {
                if (quantity > 1)
                {
                    cartItemService.UpdateQuantity(cartItemExists.Id, quantity);
                }
                else
                {
                    cartItemService.UpdateQuantity(cartItemExists.Id, cartItemExists.Quantity + 1);
                }
            }
            else
            {
                CartItem newCartItem = new CartItem { CartId = currentCart.Id, ProductId = product.Id, Quantity = quantity };
                cartItemService.Create(newCartItem);
            }


            return Json(new { statusCode = 200, msg = "SUCCESS", data = cartItemService.FindAll().Where(x => x.CartId == currentCart.Id).GroupBy(x => x.ProductId).Count() });
        }

        [HttpGet("/api/v1/cart/delete")]
        public async Task<IActionResult> DeleteItemCart(int cartItemId)
        {
            var result = cartItemService.DeleteById(cartItemId);

            if (!result)
            {
                return Json(new { statusCode = 400, msg = "FAILED", data = "" });
            }

            return Json(new { statusCode = 200, msg = "SUCCESS", data = cartItemId });
        }

        [HttpPost("/api/v1/cart/add-order")]
        public async Task<IActionResult> AddOrder(string address, string fullName, OrderDetail[] products)
        {
            var user = User.Identity;
            string currentUserId = "";
            if (user != null && user.IsAuthenticated)
            {
                currentUserId = user.Name;
            }

            if (!user.IsAuthenticated)
            {
                return Redirect("/dang-nhap");
            }
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { statusCode = 400, msg = "FAILED", data = 0 });
            }

            var currentUser = await userManager.FindByEmailAsync(currentUserId);
            var orderDate = DateTime.Now;
            if (currentUser.Address != null)
            {
                if (currentUser.Address != address)
                {
                    currentUser.Address = address;
                }
            }
            else
            {
                currentUser.Address = address;
            }

            if (currentUser.FullName != null)
            {
                if (currentUser.FullName != fullName)
                {
                    currentUser.FullName = fullName;
                }
            }
            else
            {
                currentUser.FullName = fullName;
            }

            await userManager.UpdateAsync(currentUser);
            var currentCart = cartService.FindAll().Where(x => x.UserId == currentUser.Id).FirstOrDefault();


            var order = new Orders { CustomerId = currentUser.Id, OrderDate = orderDate };

            var created = ordersService.Create(order);

            var orderLast = ordersService.FindAll().Where(x => x.CustomerId == currentUser.Id).LastOrDefault();

            if (orderLast != null)
            {
                foreach (var item in products)
                {
                    var orderDetail = new OrderDetail { OrderId = orderLast.Id, ProductName = item.ProductName, Price = item.Price, Quantity = item.Quantity };
                    orderDetailService.Create(orderDetail);
                }
            }

            cartService.DeleteById(currentCart.Id);


            return Json(new { statusCode = 200, msg = "SUCCESS", data = created });
        }
    }
}
