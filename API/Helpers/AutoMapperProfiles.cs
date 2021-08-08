using API.DTO;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<FileSetupDto,FileSetup>();
             CreateMap<FileSetup,FileSetupDto>();
             CreateMap<AllowedFileTypeDto,AllowedFileTypes>();
             CreateMap<AllowedFileTypes,AllowedFileTypes>();

        }
    }
}