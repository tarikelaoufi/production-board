using System;

using global::ProductionBoard.Core.DTOs;
using global::ProductionBoard.Core.Enums;
using global::ProductionBoard.Core.Interfaces;

using UserModel =
    global::ProductionBoard.Core.Models.ApplicationUser;

using AssignmentModel =
    global::ProductionBoard.Core.Models.UserShiftAssignment;

namespace ProductionBoard.Core.Services
{
    public sealed class AuthenticationService
    {
        private const int MaximumFailedAttempts = 5;

        private static readonly TimeSpan LockoutDuration =
            TimeSpan.FromMinutes(15);

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(
                    nameof(userRepository));
            }

            if (passwordHasher == null)
            {
                throw new ArgumentNullException(
                    nameof(passwordHasher));
            }

            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public AuthenticationResult Authenticate(
            string username,
            string password,
            DateTime currentUtcTime)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                return AuthenticationResult.Failure(
                    "Enter your username and password.");
            }

            string cleanUsername =
                username.Trim();

            UserModel user =
                _userRepository.FindByUsername(
                    cleanUsername);

            /*
             * Use the same error for unknown and disabled users.
             * This avoids revealing whether an account exists.
             */
            if (user == null || !user.IsActive)
            {
                return AuthenticationResult.Failure(
                    "Invalid username or password.");
            }

            if (user.LockedUntilUtc.HasValue &&
                user.LockedUntilUtc.Value >
                currentUtcTime)
            {
                return AuthenticationResult.Failure(
                    "This account is temporarily locked. Try again later.");
            }

            bool passwordIsValid =
                _passwordHasher.VerifyPassword(
                    password,
                    user.PasswordHash);

            if (!passwordIsValid)
            {
                return HandleFailedLogin(
                    user,
                    currentUtcTime);
            }

            if (!IsSupportedRole(user.Role))
            {
                return AuthenticationResult.Failure(
                    "This account does not have a valid platform role.");
            }

            AssignmentModel assignment = null;

            if (string.Equals(
                    user.Role,
                    ApplicationRoles.TeamLeader,
                    StringComparison.OrdinalIgnoreCase))
            {
                assignment =
                    _userRepository.GetCurrentAssignment(
                        user.Id,
                        DateTime.Today);

                if (assignment == null)
                {
                    return AuthenticationResult.Failure(
                        "No active team and shift assignment exists for this account.");
                }
            }

            _userRepository.RecordSuccessfulLogin(
                user.Id,
                currentUtcTime);

            user.FailedLoginCount = 0;
            user.LockedUntilUtc = null;
            user.LastLoginAtUtc = currentUtcTime;

            return AuthenticationResult.Success(
                user,
                assignment);
        }

        private AuthenticationResult HandleFailedLogin(
            UserModel user,
            DateTime currentUtcTime)
        {
            int failedLoginCount =
                user.FailedLoginCount + 1;

            DateTime? lockedUntilUtc = null;

            if (failedLoginCount >=
                MaximumFailedAttempts)
            {
                lockedUntilUtc =
                    currentUtcTime.Add(
                        LockoutDuration);
            }

            _userRepository.RecordFailedLogin(
                user.Id,
                failedLoginCount,
                lockedUntilUtc);

            if (lockedUntilUtc.HasValue)
            {
                return AuthenticationResult.Failure(
                    "Too many failed attempts. The account is locked for 15 minutes.");
            }

            int remainingAttempts =
                MaximumFailedAttempts -
                failedLoginCount;

            if (remainingAttempts <= 0)
            {
                return AuthenticationResult.Failure(
                    "Invalid username or password.");
            }

            return AuthenticationResult.Failure(
                "Invalid username or password. " +
                remainingAttempts +
                " attempt(s) remaining.");
        }

        private static bool IsSupportedRole(
            string role)
        {
            return string.Equals(
                       role,
                       ApplicationRoles.Admin,
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   string.Equals(
                       role,
                       ApplicationRoles.TeamLeader,
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   string.Equals(
                       role,
                       ApplicationRoles.Viewer,
                       StringComparison.OrdinalIgnoreCase);
        }
    }
}

