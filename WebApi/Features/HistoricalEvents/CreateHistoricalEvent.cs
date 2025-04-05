using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using WebApi.Contracts.HistoricalEventContracts;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Features.Museums;

namespace WebApi.Features.HistoricalEvents;

public static class CreateHistoricalEvent
{
    public class Command : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
    }
    
    internal sealed class Handler(AppDbContext context) : IRequestHandler<Command, Guid>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var historicalEvent = new HistoricalEvent
            {
                Title = request.Title,
                Description = request.Description,
                EventDate = request.EventDate,
                Location = request.Location
            };
            
            await _context.HistoricalEvents.AddAsync(historicalEvent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return historicalEvent.Id;
        }
    }
}

public class CreateHistoricalEventEndpoint(ISender sender) : EndpointBaseAsync
    .WithRequest<CreateHistoricalEventRequest>
    .WithActionResult<Guid>
{
    private readonly ISender _sender = sender;
    
    [HttpPost("api/historical-events")]
    public override async Task<ActionResult<Guid>> HandleAsync([FromBody] CreateHistoricalEventRequest request, CancellationToken cancellationToken = default)
    {
        var command = TinyMapper.Map<CreateHistoricalEvent.Command>(request);
        var historicalEvent = await _sender.Send(command, cancellationToken);
        
        return Ok(historicalEvent);
    }
}