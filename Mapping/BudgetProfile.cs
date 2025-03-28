using AutoMapper;
using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Mapping
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<Budget, BudgetDTO>();
            CreateMap<Budget, BudgetSummaryDTO>()
                .ForMember(dest => dest.ProgressPercentage,
                    opt => opt.MapFrom(src => src.TotalAmount > 0 ?
                        (src.CurrentSpending / src.TotalAmount) * 100 : 0));

            CreateMap<CreateBudgetDTO, Budget>();
            CreateMap<UpdateBudgetDTO, Budget>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<BudgetCategory, BudgetCategoryDTO>();
            CreateMap<CreateBudgetCategoryDTO, BudgetCategory>();
        }
    }
}