using Discount.Common.DTOs;
using Discount.Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController: ControllerBase
{
    private readonly ICouponRepository _couponRepository;

    public DiscountController(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
    }

    [HttpGet("productName")]
    [ProducesResponseType(typeof(CouponDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CouponDTO>> GetDiscount(string productName)
    {
        var coupon = await _couponRepository.GetDiscount(productName);
        if (coupon is null)
            return NotFound();
        
        return Ok(coupon);
    }
}