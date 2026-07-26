using global::ProductionBoard.Core.Models;

namespace ProductionBoard.Core.DTOs
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public ApplicationUser User { get; set; }

        public UserShiftAssignment Assignment { get; set; }

        public static AuthenticationResult Success(
            ApplicationUser user,
            UserShiftAssignment assignment)
        {
            return new AuthenticationResult
            {
                Succeeded = true,
                User = user,
                Assignment = assignment,
                ErrorMessage = string.Empty
            };
        }

        public static AuthenticationResult Failure(
            string errorMessage)
        {
            return new AuthenticationResult
            {
                Succeeded = false,
                ErrorMessage = errorMessage,
                User = null,
                Assignment = null
            };
        }
    }
}