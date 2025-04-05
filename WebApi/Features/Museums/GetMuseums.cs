using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.MuseumContracts;
using WebApi.Data;
using WebApi.Mappers;

namespace WebApi.Features.Museums;

public static class GetMuseums
{
    public class Query : IRequest<IEnumerable<MuseumResponse>>
    {
        
    }
    
    internal sealed class Handler(AppDbContext context) : IRequestHandler<Query, IEnumerable<MuseumResponse>>
    {
        private readonly AppDbContext _context = context;
        
        public async Task<IEnumerable<MuseumResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var mapper = new MuseumMapper();
            var museums = await _context.Museums
                .Select(museum => mapper.ToResponse(museum))
                .ToListAsync(cancellationToken);
            
            return museums;
        }
    }
}

public class GetMuseumsEndpoint(ISender sender) : EndpointWithoutRequest<IEnumerable<MuseumResponse>>
{
    private readonly ISender _sender = sender;

    public override void Configure()
    {
        Get("api/museums");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var query = new GetMuseums.Query();
        var museums = await _sender.Send(query, cancellationToken);
        
        await SendOkAsync(museums, cancellationToken);
    }
} 