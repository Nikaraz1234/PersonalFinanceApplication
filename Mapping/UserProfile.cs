using AutoMapper;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<User, UserProfileDTO>()
                .ForMember(dest => dest.MonthlyIncome, opt => opt.Ignore()) 
                .ForMember(dest => dest.MonthlyExpenses, opt => opt.Ignore()); 

            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
