//using Discount.GRPC.Services;

using Discount.Common.DTOs;
using Discount.Common.Extentions;
using Discount.GRPC;
using Discount.GRpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDiscountCommonExtentions();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<CouponDTO, GetDiscountResponse>().ReverseMap();
    configuration.CreateMap<CouponDTO, GetRandomDiscountsResponse.Types.Coupon>().ReverseMap();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CouponService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();