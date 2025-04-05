using Riok.Mapperly.Abstractions;
using WebApi.Contracts.MuseumContracts;
using WebApi.Entities;
using WebApi.Features.Museums;

namespace WebApi.Mappers;

[Mapper]
public partial class MuseumMapper
{
    public partial MuseumResponse ToResponse(Museum museum);
    public partial CreateMuseum.Command ToCommand(CreateMuseumRequest request);
}