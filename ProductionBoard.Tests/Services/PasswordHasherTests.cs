using Microsoft.VisualStudio.TestTools.UnitTesting;
using global::ProductionBoard.Core.Services;

namespace ProductionBoard.Tests.Services
{
    [TestClass]
    public class PasswordHasherTests
    {
        private const string TestPassword =
            "FakeTestPassword@123!";

        [TestMethod]
        public void HashPassword_ThenVerify_ReturnsTrue()
        {
            PasswordHasher passwordHasher =
                new PasswordHasher();

            string passwordHash =
                passwordHasher.HashPassword(
                    TestPassword);

            bool verified =
                passwordHasher.VerifyPassword(
                    TestPassword,
                    passwordHash);

            Assert.IsNotNull(passwordHash);
            Assert.IsTrue(verified);
        }

        [TestMethod]
        public void VerifyPassword_WithWrongPassword_ReturnsFalse()
        {
            PasswordHasher passwordHasher =
                new PasswordHasher();

            string passwordHash =
                passwordHasher.HashPassword(
                    TestPassword);

            bool verified =
                passwordHasher.VerifyPassword(
                    "AnotherFakePassword@456!",
                    passwordHash);

            Assert.IsFalse(verified);
        }

        [TestMethod]
        public void HashPassword_SamePassword_GeneratesDifferentHashes()
        {
            PasswordHasher passwordHasher =
                new PasswordHasher();

            string firstHash =
                passwordHasher.HashPassword(
                    TestPassword);

            string secondHash =
                passwordHasher.HashPassword(
                    TestPassword);

            Assert.AreNotEqual(
                firstHash,
                secondHash);

            Assert.IsTrue(
                passwordHasher.VerifyPassword(
                    TestPassword,
                    firstHash));

            Assert.IsTrue(
                passwordHasher.VerifyPassword(
                    TestPassword,
                    secondHash));
        }

        [TestMethod]
        public void VerifyPassword_WithInvalidHash_ReturnsFalse()
        {
            PasswordHasher passwordHasher =
                new PasswordHasher();

            bool verified =
                passwordHasher.VerifyPassword(
                    TestPassword,
                    "invalid-hash");

            Assert.IsFalse(verified);
        }
    }
}