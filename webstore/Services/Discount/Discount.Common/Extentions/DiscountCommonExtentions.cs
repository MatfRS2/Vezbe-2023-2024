using Discount.Common.Context;
using Discount.Common.DTOs;
using Discount.Common.Entities;
using Discount.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Common.Extentions;

public static class DiscountCommonExtentions
{
    public static void AddDiscountCommonExtentions(this IServiceCollection services)
    {
        services.AddScoped<ICouponContext, CouponContext>();
        services.AddScoped<ICouponRepository, CouponRepository>();
        
        services.AddAutoMapper(configuration =>
        {
            configuration.CreateMap<CouponDTO, Coupon>().ReverseMap();
        });
    }
}