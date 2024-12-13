
    using System.Security.Cryptography;

    namespace shop.commands
    {
        static class PasswordManager
        {
            public static string ReadPassword()
            {
                string password = string.Empty;
                ConsoleKeyInfo key;

                while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        password += key.KeyChar;
                        Console.Write("*");
                    }
                }

                Console.WriteLine();
                return password;
            }

            private static string HashPassword(string password)
            {
                return SecurePasswordHasher.Hash(password);
            }

            public static string ReadAndHashPassword()
            {
                string password = ReadPassword();
                return HashPassword(password);
            }
            
            public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
            {
                return SecurePasswordHasher.Verify(enteredPassword, storedHashedPassword);
            }
        }
    }


public static class SecurePasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 20;

    // Use RandomNumberGenerator for generating salt
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private static string Hash(string password, int iterations)
    {
        byte[] salt = GenerateSalt();
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Combine salt and hash
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert to base64
        string base64Hash = Convert.ToBase64String(hashBytes);

        // Format hash with extra information
        return $"$HASH|V1${iterations}${base64Hash}";
    }

    public static string Hash(string password)
    {
        return Hash(password, 10000);
    }

    private static bool IsHashSupported(string hashString)
    {
        return hashString.Contains("HASH|V1$");
    }

    public static bool Verify(string password, string hashedPassword)
    {
        if (!IsHashSupported(hashedPassword))
        {
            throw new NotSupportedException("The hash-type is not supported");
        }

        var splittedHashString = hashedPassword.Replace("$HASH|V1$", "").Split('$');
        var iterations = int.Parse(splittedHashString[0]);
        var base64Hash = splittedHashString[1];

        var hashBytes = Convert.FromBase64String(base64Hash);

        // Extract the salt
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Compare the hash bytes
        for (var i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }
        return true;
    }
}
