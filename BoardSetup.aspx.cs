using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using global::ProductionBoard.Core.Enums;

namespace PFF
{
    public partial class BoardSetup : Page
    {
        protected void Page_Load(
            object sender,
            EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                RedirectSafely("Login.aspx");
                return;
            }

            if (IsPostBack)
            {
                return;
            }

            LoadCurrentUser();
        }

        protected void ContinueButton_Click(
            object sender,
            EventArgs e)
        {
            ErrorMessageLabel.Visible = false;

            string role =
                GetSessionText("CurrentUserRole");

            if (!IsSupportedRole(role))
            {
                SignOutInvalidSession();
                return;
            }

            bool isTeamLeader =
                string.Equals(
                    role,
                    ApplicationRoles.TeamLeader,
                    StringComparison.OrdinalIgnoreCase);

            string teamName;

            if (isTeamLeader)
            {
                teamName =
                    GetSessionText("CurrentTeamName");

                if (string.IsNullOrWhiteSpace(teamName))
                {
                    ShowError(
                        "No active team assignment exists for this account.");

                    return;
                }
            }
            else
            {
                teamName =
                    TeamDropDownList.SelectedValue;
            }

            string shiftName =
                ShiftDropDownList.SelectedValue;

            if (string.IsNullOrWhiteSpace(teamName))
            {
                ShowError(
                    "Select a valid team.");

                return;
            }

            if (string.IsNullOrWhiteSpace(shiftName))
            {
                ShowError(
                    "Select a valid shift.");

                return;
            }

            string productionLine =
                ProductionLineDropDownList.SelectedValue;

            if (string.IsNullOrWhiteSpace(productionLine))
            {
                ShowError(
                    "Select a production line.");

                return;
            }

            string productName =
                ProductDropDownList.SelectedValue;

            if (string.IsNullOrWhiteSpace(productName))
            {
                ShowError(
                    "Select a product.");

                return;
            }

            DateTime boardDate;

            bool validDate =
                DateTime.TryParseExact(
                    BoardDateTextBox.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out boardDate);

            if (!validDate)
            {
                ShowError(
                    "Select a valid production-board date.");

                return;
            }

            bool canEdit =
                !string.Equals(
                    role,
                    ApplicationRoles.Viewer,
                    StringComparison.OrdinalIgnoreCase);

            Session["SelectedTeamName"] =
                teamName;

            Session["SelectedShiftName"] =
                shiftName;

            Session["SelectedLineName"] =
                productionLine;

            Session["SelectedProductName"] =
                productName;

            Session["SelectedBoardDate"] =
                boardDate.Date;

            Session["CanEditProductionBoard"] =
                canEdit;

            /*
             * Temporary compatibility with the current
             * PB_page.aspx.cs implementation.
             *
             * These old session keys can be removed after
             * PB_page is completely migrated.
             */
            Session["value1"] =
                teamName;

            Session["value2"] =
                shiftName;

            Session["value3"] =
                productionLine;

            Session["value4"] =
                boardDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);

            Session["value5"] =
                productName;

            RedirectSafely("PB_page.aspx");
        }

        private void LoadCurrentUser()
        {
            string fullName =
                GetSessionText("CurrentFullName");

            string role =
                GetSessionText("CurrentUserRole");

            if (string.IsNullOrWhiteSpace(fullName) ||
                !IsSupportedRole(role))
            {
                SignOutInvalidSession();
                return;
            }

            FullNameLabel.Text =
                HttpUtility.HtmlEncode(fullName);

            RoleLabel.Text =
                HttpUtility.HtmlEncode(
                    GetRoleDisplayName(role));

            BoardDateTextBox.Text =
                DateTime.Today.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);

            bool isTeamLeader =
                string.Equals(
                    role,
                    ApplicationRoles.TeamLeader,
                    StringComparison.OrdinalIgnoreCase);

            /*
             * Team Leader:
             * - assigned team is displayed and fixed;
             * - assigned shift text is not displayed;
             * - shift dropdown stays selectable.
             *
             * Admin / Viewer:
             * - team dropdown is visible;
             * - shift dropdown is visible.
             */
            TeamLeaderAssignmentPanel.Visible =
                isTeamLeader;

            TeamSelectionPanel.Visible =
                !isTeamLeader;

            ShiftSelectionPanel.Visible =
                true;

            if (isTeamLeader)
            {
                string teamName =
                    GetSessionText("CurrentTeamName");

                string defaultShift =
                    GetSessionText("CurrentShiftName");

                if (string.IsNullOrWhiteSpace(teamName))
                {
                    ShowError(
                        "No active team assignment exists for this account.");

                    ContinueButton.Enabled = false;
                    return;
                }

                AssignedTeamLabel.Text =
                    HttpUtility.HtmlEncode(teamName);

                /*
                 * The assigned/default shift is selected in
                 * the dropdown, but the user may change it.
                 */
                SelectDropDownValue(
                    ShiftDropDownList,
                    defaultShift);
            }

            bool isViewer =
                string.Equals(
                    role,
                    ApplicationRoles.Viewer,
                    StringComparison.OrdinalIgnoreCase);

            ReadOnlyNoticeLabel.Visible =
                isViewer;
        }

        private static void SelectDropDownValue(
            DropDownList dropDownList,
            string value)
        {
            if (dropDownList == null ||
                string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            ListItem item =
                dropDownList.Items.FindByValue(
                    value.Trim());

            if (item == null)
            {
                return;
            }

            dropDownList.ClearSelection();
            item.Selected = true;
        }

        private string GetSessionText(
            string key)
        {
            object value =
                Session[key];

            if (value == null)
            {
                return string.Empty;
            }

            return Convert
                .ToString(
                    value,
                    CultureInfo.InvariantCulture)
                .Trim();
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

        private static string GetRoleDisplayName(
            string role)
        {
            if (string.Equals(
                    role,
                    ApplicationRoles.TeamLeader,
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Team Leader";
            }

            return role;
        }

        private void SignOutInvalidSession()
        {
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            RedirectSafely("Login.aspx");
        }

        private void ShowError(
            string message)
        {
            ErrorMessageLabel.Text =
                HttpUtility.HtmlEncode(message);

            ErrorMessageLabel.Visible =
                true;
        }

        private void RedirectSafely(
            string url)
        {
            Response.Redirect(
                url,
                false);

            Context.ApplicationInstance
                .CompleteRequest();
        }
    }
}
