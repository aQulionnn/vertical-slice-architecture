using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.ArtifactContracts;
using WebApi.Contracts.MuseumContracts;
using WebApi.Data;

namespace WebApi.Features.Museums;

public static class GetMuseum
{
    public class Query : IRequest<MuseumResponse?>
    {
        public Guid Id { get; set; }
    }
    
    internal sealed class Handler(AppDbContext context) : IRequestHandler<Query, MuseumResponse?>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<MuseumResponse?> Handle(Query request, CancellationToken cancellationToken)
        {
            var museum = await _context.Museums
                .Where(x => x.Id == request.Id)
                .Select(x => new MuseumResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    EstablishedAt = x.EstablishedAt,
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            return museum;
        }
    }
}

public class GetMuseumEndpoint(ISender sender) : EndpointWithoutRequest<MuseumResponse?>
{
    private readonly ISender _sender = sender;
    
    public override void Configure()
    {
        Get("api/museums/{id:guid}");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<Guid>("id");
        var query = new GetMuseum.Query { Id = id };
        var museum = await _sender.Send(query, cancellationToken);

        if (museum is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }
        
        await SendOkAsync(museum, cancellationToken);
    }
}