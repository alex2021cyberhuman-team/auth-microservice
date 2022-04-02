using System;
using System.Text.RegularExpressions;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public static class UserValidationConfiguration
{
    public const string AcceptedPasswordRegexPattern =
        @"^.*(?=.*[A-Z].*)(?=.*[!@#$&*._-].*)(?=.*[0-9].*)(?=.*[a-z].*).*$";

    public const string AcceptedUsernameRegexPattern =
        @"^[а-яА-Яa-zA-Z0-9@._-]+$";

    public static readonly Regex AcceptedPasswordRegex = new(
        AcceptedPasswordRegexPattern, RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(25));

    public static readonly Regex AcceptedUsernameRegex = new(
        AcceptedUsernameRegexPattern, RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(25));
}
