using Discount.GRPC;

namespace Basket.API.GrpcServices;

public class CouponGrpcService
{
    private readonly  CouponProtoService.CouponProtoServiceClient _couponProtoServiceClient;
    
    public CouponGrpcService(CouponProtoService.CouponProtoServiceClient couponProtoServiceClient)
    {
        _couponProtoServiceClient = couponProtoServiceClient ??
                                    throw new ArgumentNullException(nameof(couponProtoServiceClient));
    }

    public async Task<GetDiscountResponse> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest();
        discountRequest.ProductName = productName;

        return await _couponProtoServiceClient.GetDiscountAsync(discountRequest);
    }
    
    
    public async Task<GetRandomDiscountsResponse> GetRandomDiscount(int numberOfDiscounts)
    {
        var getRandomDiscountsRequest = new GetRandomDiscountsRequest();
        getRandomDiscountsRequest.NumberOfDiscounts=numberOfDiscounts;

        return await _couponProtoServiceClient.GetRandomDiscountsAsync(getRandomDiscountsRequest);
    }
}