using AutoMapper;
using PersonalFinanceApplication.DTOs.Budgets;
using PersonalFinanceApplication.DTOs.Budgets.Categories;
using PersonalFinanceApplication.DTOs.RecurringBill;
using PersonalFinanceApplication.DTOs.SavingsPot;
using PersonalFinanceApplication.DTOs.Transaction;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Mapping
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            //Budget Mappings
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



            //RecurringBill mappings
            CreateMap<RecurringBill, RecurringBillDTO>();
            CreateMap<CreateRecurringBillDTO, RecurringBill>();
            CreateMap<UpdateRecurringBillDTO, RecurringBill>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //SavingsPot mappings
            CreateMap<SavingsPot, SavingsPotDTO>();
            CreateMap<CreateSavingsPotDTO, SavingsPot>();
            CreateMap<UpdateSavingsPotDTO, SavingsPot>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Transaction mappings
            CreateMap<CreateTransactionDTO, Transaction>();
            CreateMap<UpdateTransactionDTO, Transaction>();
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.BudgetCategory != null ? src.BudgetCategory.Name : null));
        }
    }
}