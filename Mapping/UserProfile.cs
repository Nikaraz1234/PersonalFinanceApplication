using AutoMapper;
using PersonalFinanceApplication.DTOs.Auth;
using PersonalFinanceApplication.DTOs.Users;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Basic mappings
            CreateMap<User, UserDTO>();
            CreateMap<User, UserSummaryDTO>();

            // Profile with calculated fields
            CreateMap<User, UserProfileDTO>()
                .ForMember(dest => dest.MonthlyIncome,
                    opt => opt.MapFrom(src => src.Transactions
                        .Where(t => t.Amount > 0 && t.Date.Month == DateTime.UtcNow.Month)
                        .Sum(t => t.Amount)))
                .ForMember(dest => dest.MonthlyExpenses,
                    opt => opt.MapFrom(src => src.Transactions
                        .Where(t => t.Amount < 0 && t.Date.Month == DateTime.UtcNow.Month)
                        .Sum(t => Math.Abs(t.Amount))));

            // Registration mapping
            CreateMap<UserRegisterDTO, User>()
                .ForMember(dest => dest.FirstName, opt => opt.Ignore()) // Or provide default
                .ForMember(dest => dest.LastName, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Budgets, opt => opt.Ignore())
                .ForMember(dest => dest.Transactions, opt => opt.Ignore())
                .ForMember(dest => dest.SavingsPots, opt => opt.Ignore())
                .ForMember(dest => dest.RecurringBills, opt => opt.Ignore());

            CreateMap<User, AuthResponseDto>()
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
        .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
        .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
        .ForMember(dest => dest.AccessToken, opt => opt.Ignore()) // Will be set manually
        .ForMember(dest => dest.TokenExpiry, opt => opt.Ignore());
        }
    }
}
