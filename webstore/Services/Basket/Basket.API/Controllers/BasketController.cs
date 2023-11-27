using Basket.API.Entities;
using Basket.API.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Basket.API.GrpcServices;


namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly CouponGrpcService _couponGrpcService;
    private readonly ILogger<BasketController> _logger;
    public BasketController(IBasketRepository repository
                            ,CouponGrpcService couponGrpcService
                            ,ILogger<BasketController> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _couponGrpcService = couponGrpcService ?? throw new ArgumentNullException(nameof(couponGrpcService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
    {
        var basket = await _repository.GetBasket(username);

        return Ok(basket);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]

    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        foreach (var item in basket.Items)
        {
            try
            {
                var coupon = await _couponGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error while retrieving coupon for item {ProductName}: {message}", item.ProductName, e.Message);
            }
        }
        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{username}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]

    public async Task<ActionResult<ShoppingCart>> DeleteBasket(string username)
    {
        await _repository.DeleteBasket(username);
        return Ok();
    }


    
}