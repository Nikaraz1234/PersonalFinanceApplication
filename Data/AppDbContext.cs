using Microsoft.EntityFrameworkCore;
using PersonalFinanceApplication.Models;

namespace PersonalFinanceApplication.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RecurringBill> RecurringBills { get; set; }
        public DbSet<BillPayment> BillPayments { get; set; }
        public DbSet<SavingsPot> SavingsPots { get; set; }
        public DbSet<SavingsTransaction> SavingsTransactions { get; set; }

        private readonly string _connectionString;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Budget relationships
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Budget Category relationship
            modelBuilder.Entity<BudgetCategory>()
                .HasOne(bc => bc.Budget)
                .WithMany(b => b.Categories)
                .HasForeignKey(bc => bc.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.BudgetCategory)
                .WithMany()
                .HasForeignKey(t => t.BudgetCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Savings Pot relationships
            modelBuilder.Entity<SavingsPot>()
                .HasOne(sp => sp.User)
                .WithMany(u => u.SavingsPots)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavingsTransaction>()
                .HasOne(st => st.SavingsPot)
                .WithMany(sp => sp.Transactions)
                .HasForeignKey(st => st.SavingsPotId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recurring Bill relationships
            modelBuilder.Entity<RecurringBill>()
                .HasOne(rb => rb.User)
                .WithMany(u => u.RecurringBills)
                .HasForeignKey(rb => rb.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BillPayment>()
                .HasOne(bp => bp.RecurringBill)
                .WithMany(rb => rb.Payments)
                .HasForeignKey(bp => bp.RecurringBillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision configurations
            modelBuilder.Entity<Budget>()
                .Property(b => b.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<BudgetCategory>()
                .Property(bc => bc.AllocatedAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SavingsPot>()
                .Property(sp => sp.TargetAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SavingsTransaction>()
                .Property(st => st.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RecurringBill>()
                .Property(rb => rb.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<BillPayment>()
                .Property(bp => bp.AmountPaid)
                .HasColumnType("decimal(18,2)");

            // Indexes for performance
            modelBuilder.Entity<Transaction>()
                .HasIndex(t => new { t.UserId, t.Date });

            modelBuilder.Entity<RecurringBill>()
                .HasIndex(rb => new { rb.UserId, rb.DueDay });

            modelBuilder.Entity<SavingsPot>()
                .HasIndex(sp => sp.UserId);

            // Removed UserId index from SavingsTransaction since we removed the User relationship
            modelBuilder.Entity<SavingsTransaction>()
                .HasIndex(st => st.Date);
        }
    }
}