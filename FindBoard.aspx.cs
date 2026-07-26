using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using global::ProductionBoard.Core.Enums;
using global::ProductionBoard.Core.Services;
using global::ProductionBoard.Data;
using global::ProductionBoard.Data.Repositories;

using FilterModel =
    global::ProductionBoard.Core.Models.ProductionBoardSearchFilter;

using SummaryModel =
    global::ProductionBoard.Core.Models.ProductionBoardSummary;

namespace PFF
{
    public partial class FindBoard : Page
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

            if (!ConfigureAccess())
            {
                return;
            }

            /*
             * Initial page load:
             * show recent authorized boards,
             * but do not show a success toast yet.
             */
            BindResults(
                CreateDefaultFilter(),
                false);
        }

        protected void SearchButton_Click(
            object sender,
            EventArgs e)
        {
            ErrorMessageLabel.Visible =
                false;

            FilterModel filter;

            if (!TryBuildFilter(out filter))
            {
                return;
            }

            /*
             * User explicitly launched a search,
             * so a success or no-results toast is shown.
             */
            BindResults(
                filter,
                true);
        }

        protected void ClearButton_Click(
            object sender,
            EventArgs e)
        {
            ErrorMessageLabel.Visible =
                false;

            ShiftDropDownList.ClearSelection();
            ProductionLineDropDownList.ClearSelection();
            ProductDropDownList.ClearSelection();
            BoardDateTextBox.Text =
                string.Empty;

            if (TeamFilterPanel.Visible)
            {
                TeamDropDownList.ClearSelection();
            }

            BindResults(
                CreateDefaultFilter(),
                false);

            ShowToast(
                "The search filters were cleared.",
                "info",
                "Filters cleared");
        }

        public string FormatLastUpdate(
            object updatedAtValue,
            object createdAtValue)
        {
            DateTime date;

            if (updatedAtValue != null &&
                updatedAtValue != DBNull.Value &&
                DateTime.TryParse(
                    Convert.ToString(
                        updatedAtValue,
                        CultureInfo.InvariantCulture),
                    out date))
            {
                return date.ToString(
                    "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture);
            }

            if (createdAtValue != null &&
                createdAtValue != DBNull.Value &&
                DateTime.TryParse(
                    Convert.ToString(
                        createdAtValue,
                        CultureInfo.InvariantCulture),
                    out date))
            {
                return date.ToString(
                    "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture);
            }

            return "—";
        }

        private bool ConfigureAccess()
        {
            string role =
                GetSessionText(
                    "CurrentUserRole");

            if (!IsSupportedRole(role))
            {
                SignOutInvalidSession();
                return false;
            }

            bool isTeamLeader =
                string.Equals(
                    role,
                    ApplicationRoles.TeamLeader,
                    StringComparison.OrdinalIgnoreCase);

            AssignedTeamPanel.Visible =
                isTeamLeader;

            TeamFilterPanel.Visible =
                !isTeamLeader;

            if (isTeamLeader)
            {
                string teamName =
                    GetSessionText(
                        "CurrentTeamName");

                if (string.IsNullOrWhiteSpace(teamName))
                {
                    ShowError(
                        "No active team assignment exists for this account.");

                    ShowToast(
                        "No active team assignment exists for this account.",
                        "error",
                        "Access error");

                    SearchButton.Enabled =
                        false;

                    ClearButton.Enabled =
                        false;

                    return false;
                }

                AssignedTeamLabel.Text =
                    HttpUtility.HtmlEncode(
                        teamName);

                AccessNoticeLabel.Text =
                    "Team-leader access: search is restricted to " +
                    HttpUtility.HtmlEncode(teamName) +
                    ". You may search every shift for this team.";
            }
            else if (string.Equals(
                         role,
                         ApplicationRoles.Viewer,
                         StringComparison.OrdinalIgnoreCase))
            {
                AccessNoticeLabel.Text =
                    "Viewer access: you may open boards from every team and shift, but the production table is read-only.";
            }
            else
            {
                AccessNoticeLabel.Text =
                    "Administrator access: you may search and open boards from every team and shift.";
            }

            return true;
        }

        private FilterModel CreateDefaultFilter()
        {
            string role =
                GetSessionText(
                    "CurrentUserRole");

            return new FilterModel
            {
                TeamName =
                    string.Equals(
                        role,
                        ApplicationRoles.TeamLeader,
                        StringComparison.OrdinalIgnoreCase)
                        ? GetSessionText(
                            "CurrentTeamName")
                        : null,

                MaxResults = 50
            };
        }

        private bool TryBuildFilter(
            out FilterModel filter)
        {
            filter = null;

            DateTime? boardDate =
                null;

            if (!string.IsNullOrWhiteSpace(
                    BoardDateTextBox.Text))
            {
                DateTime parsedDate;

                bool validDate =
                    DateTime.TryParseExact(
                        BoardDateTextBox.Text,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out parsedDate);

                if (!validDate)
                {
                    const string message =
                        "Select a valid board date.";

                    ShowError(message);

                    ShowToast(
                        message,
                        "error",
                        "Invalid search");

                    return false;
                }

                boardDate =
                    parsedDate.Date;
            }

            string role =
                GetSessionText(
                    "CurrentUserRole");

            string teamName =
                string.Equals(
                    role,
                    ApplicationRoles.TeamLeader,
                    StringComparison.OrdinalIgnoreCase)
                    ? GetSessionText(
                        "CurrentTeamName")
                    : TeamDropDownList.SelectedValue;

            filter =
                new FilterModel
                {
                    BoardDate =
                        boardDate,

                    TeamName =
                        teamName,

                    ShiftName =
                        ShiftDropDownList.SelectedValue,

                    LineName =
                        ProductionLineDropDownList.SelectedValue,

                    ProductName =
                        ProductDropDownList.SelectedValue,

                    MaxResults =
                        100
                };

            return true;
        }

        private void BindResults(
            FilterModel filter,
            bool showSearchToast)
        {
            try
            {
                ProductionBoardQueryService service =
                    CreateQueryService();

                IList<SummaryModel> results =
                    service.Search(filter);

                ResultsRepeater.DataSource =
                    results;

                ResultsRepeater.DataBind();

                int resultCount =
                    results == null
                        ? 0
                        : results.Count;

                ResultCountLabel.Text =
                    resultCount +
                    (
                        resultCount == 1
                            ? " board"
                            : " boards"
                    );

                EmptyResultsPanel.Visible =
                    resultCount == 0;

                ResultsTablePanel.Visible =
                    resultCount > 0;

                if (!showSearchToast)
                {
                    return;
                }

                if (resultCount == 0)
                {
                    ShowToast(
                        "There is no available production board for the selected information.",
                        "warning",
                        "No board available");

                    return;
                }

                ShowToast(
                    resultCount +
                    (
                        resultCount == 1
                            ? " production board was found."
                            : " production boards were found."
                    ),
                    "success",
                    "Search completed");
            }
            catch (Exception)
            {
                const string message =
                    "The saved boards could not be loaded. Check the SQL Server connection and try again.";

                ShowError(message);

                EmptyResultsPanel.Visible =
                    true;

                ResultsTablePanel.Visible =
                    false;

                ResultCountLabel.Text =
                    "0 boards";

                ShowToast(
                    message,
                    "error",
                    "Search error");
            }
        }

        private static ProductionBoardQueryService
            CreateQueryService()
        {
            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            ProductionBoardQueryRepository repository =
                new ProductionBoardQueryRepository(
                    connectionFactory);

            return new ProductionBoardQueryService(
                repository);
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

        private void SignOutInvalidSession()
        {
            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();

            RedirectSafely(
                "Login.aspx");
        }

        private void ShowError(
            string message)
        {
            ErrorMessageLabel.Text =
                HttpUtility.HtmlEncode(
                    message);

            ErrorMessageLabel.Visible =
                true;
        }

        private void ShowToast(
            string message,
            string type,
            string title)
        {
            string safeMessage =
                HttpUtility.JavaScriptStringEncode(
                    message ?? string.Empty);

            string safeType =
                HttpUtility.JavaScriptStringEncode(
                    type ?? "info");

            string safeTitle =
                HttpUtility.JavaScriptStringEncode(
                    title ?? string.Empty);

            string script =
                "window.setTimeout(function () {" +
                "showToast('" +
                safeMessage +
                "','" +
                safeType +
                "','" +
                safeTitle +
                "');" +
                "}, 60);";

            ClientScript.RegisterStartupScript(
                GetType(),
                "toast-" +
                Guid.NewGuid().ToString("N"),
                script,
                true);
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
