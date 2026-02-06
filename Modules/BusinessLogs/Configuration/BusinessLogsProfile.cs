using AutoMapper;
using Gooios.BuildingBlocks.Modules.BusinessLogs.Application;
using Gooios.BuildingBlocks.Modules.BusinessLogs.Domain;

namespace Gooios.BuildingBlocks.Modules.BusinessLogs.Configuration;

public class BusinessLogsProfile : Profile
{
    public BusinessLogsProfile()
    {
        CreateMap<BusinessLog, BusinessLogDto>();
    }
}