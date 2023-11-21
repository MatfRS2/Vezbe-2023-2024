using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Common.Context;

public class CouponContext: ICouponContext
{
    private readonly IConfiguration _configuration;

    public CouponContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    }
}