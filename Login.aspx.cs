using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using global::ProductionBoard.Core.DTOs;
using global::ProductionBoard.Core.Services;
using global::ProductionBoard.Data;
using global::ProductionBoard.Data.Repositories;

namespace PFF
{
    public partial class Login : Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            ErrorMessageLabel.Visible = false;

            if (Request.IsAuthenticated)
            {
                RedirectSafely(
                    "BoardSetup.aspx");
            }
        }

        protected void LoginButton_Click(
            object sender,
            EventArgs e)
        {
            ErrorMessageLabel.Visible = false;

            try
            {
                AuthenticationService authenticationService =
                    CreateAuthenticationService();

                AuthenticationResult result =
                    authenticationService.Authenticate(
                        UsernameTextBox.Text,
                        PasswordTextBox.Text,
                        DateTime.UtcNow);

                if (!result.Succeeded)
                {
                    ShowError(
                        result.ErrorMessage);

                    PasswordTextBox.Text =
                        string.Empty;

                    return;
                }

                Session.Clear();

                Session["CurrentUserId"] =
                    result.User.Id;

                Session["CurrentUsername"] =
                    result.User.Username;

                Session["CurrentFullName"] =
                    result.User.FullName;

                Session["CurrentUserRole"] =
                    result.User.Role;

                Session["MustChangePassword"] =
                    result.User.MustChangePassword;

                if (result.Assignment != null)
                {
                    Session["CurrentAssignmentId"] =
                        result.Assignment.Id;

                    Session["CurrentTeamName"] =
                        result.Assignment.TeamName;

                    Session["CurrentShiftName"] =
                        result.Assignment.ShiftName;
                }

                FormsAuthentication.SetAuthCookie(
                    result.User.Username,
                    RememberMeCheckBox.Checked);

                RedirectSafely(
                    "BoardSetup.aspx");
            }
            catch (Exception)
            {
                ShowError(
                    "Login is temporarily unavailable. Check the database connection and try again.");
            }
        }

        private static AuthenticationService
            CreateAuthenticationService()
        {
            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            UserRepository userRepository =
                new UserRepository(
                    connectionFactory);

            PasswordHasher passwordHasher =
                new PasswordHasher();

            return new AuthenticationService(
                userRepository,
                passwordHasher);
        }

        private void ShowError(string message)
        {
            ErrorMessageLabel.Text =
                HttpUtility.HtmlEncode(
                    string.IsNullOrWhiteSpace(message)
                        ? "Authentication failed."
                        : message);

            ErrorMessageLabel.Visible = true;
        }

        private void RedirectSafely(string url)
        {
            Response.Redirect(
                url,
                false);

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}