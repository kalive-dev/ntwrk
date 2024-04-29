namespace ntwrk.Api.Functions.User
{
    public interface IUserFunction
    {
        User? Authenticate(string loginId, string password);
        User? Register(string loginId, string userName, string password);
        User GetUserById(int id);
    }
}
