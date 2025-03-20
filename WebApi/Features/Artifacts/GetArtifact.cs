using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.ArtifactContracts;
using WebApi.Data;

namespace WebApi.Features.Artifacts;

public static class GetArtifact
{
    public class Query : IRequest<ArtifactResponse>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler(AppDbContext context) : IRequestHandler<Query, ArtifactResponse?>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<ArtifactResponse?> Handle(Query request, CancellationToken cancellationToken)
        {
            var artifact = await _context.Artifacts
                .Where(x => x.Id == request.Id)
                .Select(x => new ArtifactResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Origin = x.Origin,
                    Material = x.Material,
                    EstimatedAge = x.EstimatedAge,
                    Location = x.Location,
                    DiscoveryDate = x.DiscoveryDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            return artifact;
        }
    }
}

public class GetArtifactEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/artifacts/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetArtifact.Query { Id = id };
            var result = await sender.Send(query);

            if (result == null)
                return Results.NotFound();
            
            return Results.Ok(result);
        });
    }
}