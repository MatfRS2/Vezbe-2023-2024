using AutoMapper;
using Discount.Common.Repositories;
using Discount.GRPC;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.GRpc.Services;

public class CouponService: CouponProtoService.CouponProtoServiceBase
{
    private readonly ICouponRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CouponService> _logger;
    
    public CouponService(ICouponRepository repository, IMapper mapper, ILogger<CouponService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName)
                     ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName = {request.ProductName} is not found"));

        _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}",
            coupon.ProductName, coupon.Amount);

        return _mapper.Map<GetDiscountResponse>(coupon);
    }

    public override async Task<GetRandomDiscountsResponse> GetRandomDiscounts(GetRandomDiscountsRequest request, ServerCallContext context)
    {
        var coupons = await _repository.GetRandomDiscounts(request.NumberOfDiscounts);

        var response = new GetRandomDiscountsResponse();
        response.Coupons.AddRange(_mapper.Map<IEnumerable<GetRandomDiscountsResponse.Types.Coupon>>(coupons));
        response.TotalAmount = response.Coupons.Sum(coupon => coupon.Amount);

        _logger.LogInformation("Retrieved {numberOfDiscounts} random coupon(s) of total value {amount}", 
            request.NumberOfDiscounts, response.TotalAmount);

        return response;
    }
}