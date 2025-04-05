using Nelibur.ObjectMapper;
using WebApi.Contracts.HistoricalEventContracts;
using WebApi.Entities;
using WebApi.Features.HistoricalEvents;

namespace WebApi.Mappers;

public static class HistoricalEventMapper
{
    public static void Register()
    {
        TinyMapper.Bind<HistoricalEvent, HistoricalEventResponse>();
        TinyMapper.Bind<CreateHistoricalEventRequest, CreateHistoricalEvent.Command>();
    }
}