using AutoMapper;
using BusinessLogic.DTOs.RoleDtos;

namespace BusinessLogic.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Role Mappings
            CreateMap<Repository.Entities.Role, ResponseRoleDto>().ReverseMap();
            CreateMap<Repository.Entities.Role, RequestRoleDto>().ReverseMap();
            // Account Mappings
            CreateMap<Repository.Entities.Account, DTOs.ResponseAccountDto>().ReverseMap();
            CreateMap<Repository.Entities.Account, DTOs.RequestAccountDto>().ReverseMap();
            
            // Quiz Attempt Mappings
            CreateMap<Repository.Entities.QuizAttempt, DTOs.ResponseQuizAttemptDto>().ReverseMap();
            CreateMap<Repository.Entities.QuizAttempt, DTOs.RequestQuizAttemptDto>().ReverseMap();
            
            // Quiz Attempt Detail Mappings
            CreateMap<Repository.Entities.QuizAttemptDetail, DTOs.ResponseQuizAttemptDetailDto>().ReverseMap();
            CreateMap<Repository.Entities.QuizAttemptDetail, DTOs.RequestQuizAttemptDetailDto>().ReverseMap();
            
            // Answer Option Mappings
            CreateMap<Repository.Entities.AnswerOption, DTOs.ResponseAnswerOptionDto>().ReverseMap();
            CreateMap<Repository.Entities.AnswerOption, DTOs.RequestAnswerOptionDto>().ReverseMap();
            
            //Add other mappings here as needed
        }
    }
}
