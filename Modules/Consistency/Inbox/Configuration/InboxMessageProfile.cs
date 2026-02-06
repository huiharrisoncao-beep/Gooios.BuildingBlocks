using AutoMapper;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Application;
using Gooios.BuildingBlocks.Modules.Consistency.Inbox.Domain;

namespace Gooios.BuildingBlocks.Modules.Consistency.Inbox.Configuration;

public class InboxMessageProfile : Profile
{
    public InboxMessageProfile()
    {
        CreateMap<InboxMessage, InboxMessageDto>();

        CreateMap<InboxMessageDto, InboxMessage>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());
    }
}