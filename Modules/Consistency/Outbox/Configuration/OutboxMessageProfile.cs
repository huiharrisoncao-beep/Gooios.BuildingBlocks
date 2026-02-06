using AutoMapper;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Application;
using Gooios.BuildingBlocks.Modules.Consistency.Outbox.Domain;
namespace Gooios.BuildingBlocks.Modules.Consistency.Outbox.Configuration;

public class OutboxMessageProfile : Profile
{
    public OutboxMessageProfile()
    {
        CreateMap<OutboxMessage, OutboxMessageDto>();
        CreateMap<OutboxMessageDto, OutboxMessage>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
    }
}