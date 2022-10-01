namespace Conduit.Auth.DomainLayer.Users.Passwords;

public interface IPasswordManager
{
    string HashPassword(
        string plainPassword,
        User user);

    bool VerifyPassword(
        string plainPassword,
        User user);
}
