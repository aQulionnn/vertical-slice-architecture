using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.HistoricalEventContracts;
using WebApi.Data;

namespace WebApi.Features.HistoricalEvents;

public static class GetHistoricalEvent
{
    public class Query : IRequest<HistoricalEventResponse?>
    { 
        public Guid Id { get; set; }
    }
    
    internal sealed class Handler(AppDbContext context) : IRequestHandler<Query, HistoricalEventResponse?>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<HistoricalEventResponse?> Handle(Query request, CancellationToken cancellationToken)
        {
            var historicalEvent = await _context.HistoricalEvents
                .Where(x => x.Id == request.Id)
                .Select(x => new HistoricalEventResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    EventDate = x.EventDate,
                    Location = x.Location
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            return historicalEvent;
        }
    }
}

public class GetHistoricalEventEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<HistoricalEventResponse?>
{
    private readonly ISender _sender = sender;
    
    [HttpGet("api/historical-events/{id:guid}")]
    public override async Task<ActionResult<HistoricalEventResponse?>> HandleAsync([FromRoute] Guid id, CancellationToken cancellationToken = new CancellationToken())
    {
        var query = new GetHistoricalEvent.Query { Id = id };
        var historicalEvent = await _sender.Send(query, cancellationToken);
        
        return historicalEvent is not null ? Ok(historicalEvent) : NotFound();
    }
}