using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Factories;
using Ordering.Application.Features.Orders.Queries.ViewModels;

namespace Ordering.Application.Features.Orders.Queries.GetListOfOrdersQueries;

public class GetListOfOrdersQueryHandler: IRequestHandler<GetListOfOrdersQuery, List<OrderViewModel>>
{
    private readonly IOrderRepository _repository;
    private readonly IOrderViewModelFactory _factory;


    public GetListOfOrdersQueryHandler(IOrderRepository repository, IOrderViewModelFactory factory)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public async Task<List<OrderViewModel>> Handle(GetListOfOrdersQuery request, CancellationToken cancellationToken)
    {
        var results = await _repository.GetOrdersByUsername(request.Username);
        return results.Select(order => _factory.CreateViewModel(order)).ToList();
    }
}