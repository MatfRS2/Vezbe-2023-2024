using Npgsql;

namespace Discount.Common.Context;

public interface ICouponContext
{
    NpgsqlConnection GetConnection();

}