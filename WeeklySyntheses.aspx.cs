using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using global::ProductionBoard.Core.Enums;
using global::ProductionBoard.Core.Services;
using global::ProductionBoard.Data;
using global::ProductionBoard.Data.Repositories;

using FilterModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisFilter;

using ResultModel =
    global::ProductionBoard.Core.Models.WeeklySynthesisResult;

using RowModel =
    global::ProductionBoard.Core.Models.WeeklyTeamDayResult;

namespace PFF
{
    public partial class WeeklySyntheses : Page
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

            string role =
                GetSessionText("CurrentUserRole");

            if (!IsSupportedRole(role))
            {
                SignOutInvalidSession();
                return;
            }

            if (IsPostBack)
            {
                return;
            }

            try
            {
                BindFilterLists();

                WeekDateTextBox.Text =
                    DateTime.Today.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                BindWeeklySynthesis();
            }
            catch (Exception)
            {
                ShowError(
                    "Weekly production data could not be loaded. Check the database connection and schema.");
            }
        }

        protected void ApplyFilterButton_Click(
            object sender,
            EventArgs e)
        {
            ErrorMessageLabel.Visible =
                false;

            BindWeeklySynthesis();
        }

        public string FormatDayName(
            object dateValue)
        {
            DateTime date;

            if (dateValue == null ||
                !DateTime.TryParse(
                    Convert.ToString(
                        dateValue,
                        CultureInfo.InvariantCulture),
                    out date))
            {
                return "—";
            }

            return date.ToString(
                "dddd",
                CultureInfo.GetCultureInfo("en-US"));
        }

        private void BindFilterLists()
        {
            WeeklySynthesisService service =
                CreateService();

            BindDropDown(
                TeamDropDownList,
                "All teams",
                service.GetTeams());

            BindDropDown(
                ProductionLineDropDownList,
                "All production lines",
                service.GetProductionLines());

            BindDropDown(
                ProductDropDownList,
                "All products",
                service.GetProducts());
        }

        private void BindWeeklySynthesis()
        {
            DateTime selectedDate;

            bool validDate =
                DateTime.TryParseExact(
                    WeekDateTextBox.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out selectedDate);

            if (!validDate)
            {
                ShowError(
                    "Select a valid date for the weekly synthesis.");

                return;
            }

            WeeklySynthesisService service =
                CreateService();

            ResultModel result =
                service.GetWeeklySynthesis(
                    new FilterModel
                    {
                        WeekStart = selectedDate,
                        TeamName = TeamDropDownList.SelectedValue,
                        LineName = ProductionLineDropDownList.SelectedValue,
                        ProductName = ProductDropDownList.SelectedValue
                    });

            BindSummary(result);
            BindTable(result);
            BindChartData(result);
        }

        private void BindSummary(
            ResultModel result)
        {
            BestTeamLabel.Text =
                HttpUtility.HtmlEncode(
                    result.BestTeamName ?? "—");

            TotalTargetLabel.Text =
                result.TotalTarget.ToString(
                    "N0",
                    CultureInfo.InvariantCulture);

            TotalActualLabel.Text =
                result.TotalActual.ToString(
                    "N0",
                    CultureInfo.InvariantCulture);

            TotalScrapLabel.Text =
                result.TotalScrap.ToString(
                    "N0",
                    CultureInfo.InvariantCulture);

            EfficiencyLabel.Text =
                result.OverallEfficiencyPercent.ToString(
                    "0.0",
                    CultureInfo.InvariantCulture) +
                "%";

            WeekRangeLabel.Text =
                result.WeekStart.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture) +
                " → " +
                result.WeekEnd.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);
        }

        private void BindTable(
            ResultModel result)
        {
            IList<RowModel> rows =
                result.Rows ??
                new List<RowModel>();

            WeeklyResultsRepeater.DataSource =
                rows;

            WeeklyResultsRepeater.DataBind();

            EmptyResultsPanel.Visible =
                rows.Count == 0;

            ResultsTablePanel.Visible =
                rows.Count > 0;
        }

        private void BindChartData(
            ResultModel result)
        {
            DateTime[] days =
                Enumerable
                    .Range(0, 7)
                    .Select(index =>
                        result.WeekStart.AddDays(index))
                    .ToArray();

            string[] teams =
                result.Rows
                    .Select(row => row.TeamName)
                    .Where(team =>
                        !string.IsNullOrWhiteSpace(team))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(team => team)
                    .ToArray();

            var chartData = new
            {
                dayLabels = days
                    .Select(day =>
                        day.ToString(
                            "ddd dd",
                            CultureInfo.GetCultureInfo("en-US")))
                    .ToArray(),

                dailyTarget = days
                    .Select(day =>
                        result.Rows
                            .Where(row =>
                                row.BoardDate.Date == day.Date)
                            .Sum(row => row.TargetQuantity))
                    .ToArray(),

                dailyActual = days
                    .Select(day =>
                        result.Rows
                            .Where(row =>
                                row.BoardDate.Date == day.Date)
                            .Sum(row => row.ActualQuantity))
                    .ToArray(),

                teamLabels = teams,

                teamTarget = teams
                    .Select(team =>
                        result.Rows
                            .Where(row =>
                                string.Equals(
                                    row.TeamName,
                                    team,
                                    StringComparison.OrdinalIgnoreCase))
                            .Sum(row => row.TargetQuantity))
                    .ToArray(),

                teamActual = teams
                    .Select(team =>
                        result.Rows
                            .Where(row =>
                                string.Equals(
                                    row.TeamName,
                                    team,
                                    StringComparison.OrdinalIgnoreCase))
                            .Sum(row => row.ActualQuantity))
                    .ToArray(),

                teamScrap = teams
                    .Select(team =>
                        result.Rows
                            .Where(row =>
                                string.Equals(
                                    row.TeamName,
                                    team,
                                    StringComparison.OrdinalIgnoreCase))
                            .Sum(row => row.ScrapQuantity))
                    .ToArray()
            };

            JavaScriptSerializer serializer =
                new JavaScriptSerializer();

            ChartDataHiddenField.Value =
                serializer.Serialize(chartData);

            ClientScript.RegisterStartupScript(
                GetType(),
                "render-weekly-charts-" +
                Guid.NewGuid().ToString("N"),
                "window.setTimeout(renderWeeklyCharts, 50);",
                true);
        }

        private static WeeklySynthesisService
            CreateService()
        {
            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            WeeklySynthesisRepository repository =
                new WeeklySynthesisRepository(
                    connectionFactory);

            return new WeeklySynthesisService(
                repository);
        }

        private static void BindDropDown(
            DropDownList dropDownList,
            string allItemsText,
            IList<string> values)
        {
            dropDownList.Items.Clear();

            dropDownList.Items.Add(
                new ListItem(
                    allItemsText,
                    string.Empty));

            foreach (string value in
                     values ?? new List<string>())
            {
                dropDownList.Items.Add(
                    new ListItem(
                        value,
                        value));
            }
        }

        private string GetSessionText(
            string key)
        {
            object value = Session[key];

            return value == null
                ? string.Empty
                : Convert.ToString(
                    value,
                    CultureInfo.InvariantCulture).Trim();
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
            Response.Redirect(url, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
