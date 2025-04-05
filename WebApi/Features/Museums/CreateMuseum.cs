using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.MuseumContracts;
using WebApi.Data;
using WebApi.Entities;
using WebApi.Mappers;

namespace WebApi.Features.Museums;

public static class CreateMuseum
{
    public class Command : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime EstablishedAt { get; set; }
    }
    
    internal sealed class Handler(AppDbContext context) : IRequestHandler<Command, Guid>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var museum = new Museum
            {
                Name = request.Name,
                Location = request.Location,
                Description = request.Description,
                EstablishedAt = request.EstablishedAt
            };
            
            await _context.Museums.AddAsync(museum, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return museum.Id;
        }
    }
}

public class CreateMuseumEndpoint(ISender sender): Endpoint<CreateMuseumRequest, Results<Created<Guid>, BadRequest>>
{
    private readonly ISender _sender = sender;    
    
    public override void Configure()
    {
        Post("api/museums");
        AllowAnonymous();
    }

    public override async Task<Results<Created<Guid>, BadRequest>> ExecuteAsync(CreateMuseumRequest request, CancellationToken cancellationToken)
    {
        var mapper = new MuseumMapper();
        var command = mapper.ToCommand(request);
        
        var museumId = await _sender.Send(command, cancellationToken);

        if (museumId == Guid.Empty)
            return TypedResults.BadRequest();
        
        return TypedResults.Created($"api/museums/{museumId}", museumId);
    }
} 