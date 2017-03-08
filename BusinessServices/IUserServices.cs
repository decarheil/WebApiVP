namespace BusinessServices
{
    public interface IUserServices
    {
        bool Authenticate(string email, string password);

        string GetAccessKey(string email);
    }
}
