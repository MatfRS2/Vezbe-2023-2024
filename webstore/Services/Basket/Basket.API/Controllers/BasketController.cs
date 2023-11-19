using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class BasketController: ControllerBase
{
    private readonly IBasketRepository _repository;

    public BasketController(IBasketRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("username")]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]

    public async Task<ActionResult<ShoppingCart>?> GetBasket(string username)
    {
        var basket = _repository.GetBasket(username);
        if (basket is null)
            return Ok(new ShoppingCart(username));
        
        return Ok(basket);
    }
    [HttpPut]
    [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]

    public async Task<ActionResult<ShoppingCart>?> UpdateBasket([FromBody] ShoppingCart basket)
    {
        var result = await _repository.UpdateBasket(basket);
        return Ok(result);
    }
    [HttpDelete("{username}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]

    public async Task<IActionResult> DeleteBasket(string username)
    {
        await _repository.DeleteBasket(username);
        return Ok();
    }
}