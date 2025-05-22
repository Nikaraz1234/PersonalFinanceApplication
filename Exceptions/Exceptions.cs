namespace PersonalFinanceApplication.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException() : base("Email already registered") { }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string entity) : base($"{entity} not found") { }
    }

    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Invalid token") { }
    }
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message) { }
    }

    public class InvalidCredentialsException : AuthException
    {
        public InvalidCredentialsException(string message) : base(message) { }
    }

}
