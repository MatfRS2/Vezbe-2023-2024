using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Command.CreateOrder;
using Ordering.Application.Features.Orders.Queries.GetListOfOrdersQueries;
using Ordering.Application.Features.Orders.Queries.ViewModels;
using Ordering.Domain.Aggregates;
using Ordering.Infrastructure.Repositories;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class OrderController: ControllerBase
{
    private readonly IMediator _mediator;


    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(IEnumerable<OrderViewModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrdersByUsername(string username)
    {
        var query = new GetListOfOrdersQuery(username);
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CreateOrder(CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}