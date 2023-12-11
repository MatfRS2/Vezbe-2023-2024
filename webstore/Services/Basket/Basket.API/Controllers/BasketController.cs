using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Basket.API.GrpcServices;
using EventBus.Messages.Events;
using MassTransit;


namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly CouponGrpcService _couponGrpcService;
    private readonly ILogger<BasketController> _logger;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;


    public BasketController(IBasketRepository repository, CouponGrpcService couponGrpcService,
        ILogger<BasketController> logger, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _couponGrpcService = couponGrpcService ?? throw new ArgumentNullException(nameof(couponGrpcService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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
    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType(typeof(void), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        

        var basket = await _repository.GetBasket(basketCheckout.BuyerUsername);
        if (basket is null)
        {
            return BadRequest();
        }

        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
        await _publishEndpoint.Publish(eventMessage);

        await _repository.DeleteBasket(basketCheckout.BuyerUsername);
        
        return Accepted();
    }

    
}