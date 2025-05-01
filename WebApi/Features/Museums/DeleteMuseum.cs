using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.MuseumContracts;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Mappers;

namespace WebApi.Features.Museums;

public static class DeleteMuseum
{
    public class Command : IRequest<Museum?>
    {
        public Guid Id { get; set; }
    }
    
    internal sealed class Handler(AppDbContext context) : IRequestHandler<Command, Museum?>
    {
        private readonly AppDbContext _context = context;

        public async Task<Museum?> Handle(Command request, CancellationToken cancellationToken)
        {
            var museum = await _context.Museums.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (museum is null) return null;

            _context.Museums.Remove(museum);
            await _context.SaveChangesAsync(cancellationToken);

            return museum;
        }
    }
}

public class DeleteMuseumEndpoint(ISender sender)
    : EndpointWithoutRequest<MuseumResponse?>
{
    private readonly ISender _sender = sender;

    public override void Configure()
    {
        Delete("api/museums/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var id = Route<Guid>("id");
        var command = new DeleteMuseum.Command { Id = id };
        var museum = await _sender.Send(command, cancellationToken);

        if (museum is null)
        {
            await SendNotFoundAsync(cancellationToken);
            return;
        }

        var mapper = new MuseumMapper();
        await SendOkAsync(mapper.ToResponse(museum), cancellationToken);
    }
} 