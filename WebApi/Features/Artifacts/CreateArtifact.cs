using Carter;
using Mapster;
using MediatR;
using WebApi.Contracts.ArtifactContracts;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Features.Artifacts;

public static class CreateArtifact
{
    public class Command : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public int EstimatedAge  { get; set; } 
        public string Location { get; set; } = string.Empty;
        public DateTime DiscoveryDate { get; set; }
    }

    internal sealed class Handler(AppDbContext context) : IRequestHandler<Command, Guid>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var artifact = new Artifact
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Origin = request.Origin,
                Material = request.Material,
                EstimatedAge = request.EstimatedAge,
                Location = request.Location,
                DiscoveryDate = request.DiscoveryDate
            };
            
            _context.Artifacts.Add(artifact);
            await _context.SaveChangesAsync(cancellationToken);
            
            return artifact.Id;
        }
    }
}

public class CreateArticleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/artifacts", async (CreateArtifactRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateArtifact.Command>();
            var artifactId = await sender.Send(command);

            return Results.Ok(artifactId);
        });
    }
}