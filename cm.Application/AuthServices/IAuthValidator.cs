namespace cm.Application.AuthServices
{
    public interface IAuthValidator
    {
        bool ValidateUser(string username, string password);
    }
}