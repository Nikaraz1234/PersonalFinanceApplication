using PersonalFinanceApplication.Interfaces;
using System;

namespace PersonalFinanceApplication.Services
{
    public class RecurringBillsService : IRecurringBillService
    {
        private readonly AppDbContext _context;

        public RecurringBillsService(AppDbContext context)
        {
            _context = context;
        }

    }
}
    