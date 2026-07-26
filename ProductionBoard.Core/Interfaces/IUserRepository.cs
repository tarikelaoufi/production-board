using System;
using global::ProductionBoard.Core.Models;

namespace ProductionBoard.Core.Interfaces
{
    public interface IUserRepository
    {
        ApplicationUser FindByUsername(string username);

        UserShiftAssignment GetCurrentAssignment(
            int userId,
            DateTime effectiveDate);

        void RecordSuccessfulLogin(
            int userId,
            DateTime loginTimeUtc);

        void RecordFailedLogin(
            int userId,
            int failedLoginCount,
            DateTime? lockedUntilUtc);
    }
}