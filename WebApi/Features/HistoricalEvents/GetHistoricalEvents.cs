using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.HistoricalEventContracts;
using WebApi.Data;

namespace WebApi.Features.HistoricalEvents;

public static class GetHistoricalEvents
{
    public class Query : IRequest<IEnumerable<HistoricalEventResponse>>
    {
        
    }
    
    internal sealed class Hadler(AppDbContext context) : IRequestHandler<Query, IEnumerable<HistoricalEventResponse>>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<IEnumerable<HistoricalEventResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var historicalEvents = await _context.HistoricalEvents
                .Select(x => new HistoricalEventResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    EventDate = x.EventDate,
                    Location = x.Location,
                })
                .ToListAsync(cancellationToken);
            
            return historicalEvents;
        }
    }
}

public class GetHistoricalEventsEndpoint(ISender sender) : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<IEnumerable<HistoricalEventResponse>>
{
    private readonly ISender _sender = sender;
    
    [HttpGet("api/historical-events")]
    public override async Task<ActionResult<IEnumerable<HistoricalEventResponse>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetHistoricalEvents.Query();
        var historicalEvents = await _sender.Send(query, cancellationToken);
        
        return Ok(historicalEvents);
    }
}