using System;
using System.Security.Cryptography;
using global::ProductionBoard.Core.Interfaces;

namespace ProductionBoard.Core.Services
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const string AlgorithmName = "PBKDF2-SHA1";

        private const int IterationCount = 100000;

        private const int SaltSize = 16;

        private const int HashSize = 32;

        public string HashPassword(string password)
        {
            ValidatePassword(password);

            byte[] salt = new byte[SaltSize];

            using (RandomNumberGenerator randomNumberGenerator =
                   RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(salt);
            }

            byte[] passwordHash;

            using (Rfc2898DeriveBytes deriveBytes =
                   new Rfc2898DeriveBytes(
                       password,
                       salt,
                       IterationCount))
            {
                passwordHash =
                    deriveBytes.GetBytes(HashSize);
            }

            return string.Format(
                "{0}${1}${2}${3}",
                AlgorithmName,
                IterationCount,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(passwordHash));
        }

        public bool VerifyPassword(
            string password,
            string storedPasswordHash)
        {
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrWhiteSpace(storedPasswordHash))
            {
                return false;
            }

            string[] parts =
                storedPasswordHash.Split('$');

            if (parts.Length != 4)
            {
                return false;
            }

            if (!string.Equals(
                    parts[0],
                    AlgorithmName,
                    StringComparison.Ordinal))
            {
                return false;
            }

            int iterations;

            if (!int.TryParse(
                    parts[1],
                    out iterations) ||
                iterations <= 0)
            {
                return false;
            }

            byte[] salt;
            byte[] expectedHash;

            try
            {
                salt =
                    Convert.FromBase64String(
                        parts[2]);

                expectedHash =
                    Convert.FromBase64String(
                        parts[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actualHash;

            using (Rfc2898DeriveBytes deriveBytes =
                   new Rfc2898DeriveBytes(
                       password,
                       salt,
                       iterations))
            {
                actualHash =
                    deriveBytes.GetBytes(
                        expectedHash.Length);
            }

            return FixedTimeEquals(
                expectedHash,
                actualHash);
        }

        private static bool FixedTimeEquals(
            byte[] expected,
            byte[] actual)
        {
            if (expected == null ||
                actual == null ||
                expected.Length != actual.Length)
            {
                return false;
            }

            int difference = 0;

            for (int index = 0;
                 index < expected.Length;
                 index++)
            {
                difference |=
                    expected[index] ^
                    actual[index];
            }

            return difference == 0;
        }

        private static void ValidatePassword(
            string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "A password is required.",
                    nameof(password));
            }

            if (password.Length < 8)
            {
                throw new ArgumentException(
                    "The password must contain at least 8 characters.",
                    nameof(password));
            }

            if (password.Length > 200)
            {
                throw new ArgumentException(
                    "The password cannot exceed 200 characters.",
                    nameof(password));
            }
        }
    }
}