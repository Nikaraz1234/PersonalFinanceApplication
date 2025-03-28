namespace PersonalFinanceApplication.Interfaces
{

    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string storedHash, string providedPassword);
    }
}
