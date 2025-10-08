using AutoMapper;

namespace BusinessLogic.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //QuizSet Mappings
            CreateMap<Repository.Entities.QuizSet, BusinessLogic.DTOs.QuizSetDtos.QuizSetRequestDto>().ReverseMap();
            CreateMap<Repository.Entities.QuizSet, BusinessLogic.DTOs.QuizSetDtos.QuizSetResponseDto>().ReverseMap();
            //Quiz Mappings
            CreateMap<Repository.Entities.Quiz, BusinessLogic.DTOs.QuizDtos.QuizRequestDto>().ReverseMap();
            CreateMap<Repository.Entities.Quiz, BusinessLogic.DTOs.QuizDtos.QuizResponseDto>().ReverseMap();
            //Role Mappings
            CreateMap<Repository.Entities.Role, BusinessLogic.DTOs.RoleDtos.ResponseRoleDto>().ReverseMap();
            CreateMap<Repository.Entities.Role, BusinessLogic.DTOs.RoleDtos.RequestRoleDto>().ReverseMap();
            // Account Mappings
            CreateMap<Repository.Entities.Account, DTOs.ResponseAccountDto>().ReverseMap();
            CreateMap<Repository.Entities.Account, DTOs.RequestAccountDto>().ReverseMap();
            //Add other mappings here as needed
        }
    }
}
