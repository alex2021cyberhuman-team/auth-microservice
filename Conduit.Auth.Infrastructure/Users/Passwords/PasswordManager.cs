using System;
using System.Security.Cryptography;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Passwords;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Conduit.Auth.Infrastructure.Users.Passwords;

public class PasswordManager : IPasswordManager
{
    /* =======================
     * HASHED PASSWORD FORMATS
     * =======================
     * PBKDF2 with HMAC-SHA256, 256-bit salt, 256-bit subkey, 10000 iterations.
     * Format: { 0x01, prf (UInt32), iter count (UInt32), salt length (UInt32), salt, subkey }
     * (All UInt32s are stored big-endian.)
     */

    private const int _iterCount = 10000;

    private readonly RandomNumberGenerator _rng =
        RandomNumberGenerator.Create();

    private byte[] HashPassword(
        string password)
    {
        return HashPassword(password, _rng, KeyDerivationPrf.HMACSHA256,
            _iterCount, 256 / 8, 256 / 8);
    }

    private static byte[] HashPassword(
        string password,
        RandomNumberGenerator rng,
        KeyDerivationPrf prf,
        int iterCount,
        int saltSize,
        int numBytesRequested)
    {
        var salt = new byte[saltSize];
        rng.GetBytes(salt);
        var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount,
            numBytesRequested);

        var outputBytes = new byte[12 + salt.Length + subkey.Length];
        WriteNetworkByteOrder(outputBytes, 0, (uint)prf);
        WriteNetworkByteOrder(outputBytes, 4, (uint)iterCount);
        WriteNetworkByteOrder(outputBytes, 8, (uint)saltSize);
        Buffer.BlockCopy(salt, 0, outputBytes, 12, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 12 + saltSize, subkey.Length);
        return outputBytes;
    }

    private static uint ReadNetworkByteOrder(
        byte[] buffer,
        int offset)
    {
        return ((uint)buffer[offset + 0] << 24) |
               ((uint)buffer[offset + 1] << 16) |
               ((uint)buffer[offset + 2] << 8) |
               buffer[offset + 3];
    }

    private static bool VerifyHashedPassword(
        byte[] hashedPassword,
        string password)
    {
        try
        {
            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 0);
            var iterCount = (int)ReadNetworkByteOrder(hashedPassword, 4);
            var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 8);

            // Read the salt: must be >= 256 bits
            if (saltLength < 256 / 8)
            {
                return false;
            }

            var salt = new byte[saltLength];
            Buffer.BlockCopy(hashedPassword, 12, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 256 bits
            var subkeyLength = hashedPassword.Length - 12 - salt.Length;
            if (subkeyLength < 256 / 8)
            {
                return false;
            }

            var expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(hashedPassword, 12 + salt.Length, expectedSubkey,
                0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf,
                iterCount, subkeyLength);
            return CryptographicOperations.FixedTimeEquals(actualSubkey,
                expectedSubkey);
        }
        catch
        {
            // This should never occur except in the case of a malformed payload, where
            // we might go off the end of the array. Regardless, a malformed payload
            // implies verification failed.
            return false;
        }
    }

    private static void WriteNetworkByteOrder(
        byte[] buffer,
        int offset,
        uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }

    #region IPasswordManager Members

    /// <summary>
    ///     Returns a hashed representation of the supplied
    ///     <paramref name="password" /> for the specified
    ///     <paramref name="user" />.
    /// </summary>
    /// <param name="user">The user whose password is to be hashed.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>
    ///     A hashed representation of the supplied <paramref name="password" /> for
    ///     the specified
    ///     <paramref name="user" />.
    /// </returns>
    public string HashPassword(
        string password,
        User user)
    {
        return Convert.ToBase64String(HashPassword(password));
    }

    /// <summary>
    ///     Returns a <see cref="bool" /> indicating the result of a password hash
    ///     comparison.
    /// </summary>
    /// <param name="user">The user whose password should be verified.</param>
    /// <param name="plainPassword">The password supplied for comparison.</param>
    /// <remarks>Implementations of this method should be time consistent.</remarks>
    public bool VerifyPassword(
        string plainPassword,
        User user)
    {
        var hashedPassword = user.Password;

        var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

        // read the format marker from the hashed password
        return decodedHashedPassword.Length != 0 &&
               VerifyHashedPassword(decodedHashedPassword, plainPassword);
    }

    #endregion
}
