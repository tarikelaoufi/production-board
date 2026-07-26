using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using global::ProductionBoard.Core.Enums;
using global::ProductionBoard.Core.Services;
using global::ProductionBoard.Data;
using global::ProductionBoard.Data.Repositories;

using BoardModel =
    global::ProductionBoard.Core.Models.ProductionBoard;

using HourModel =
    global::ProductionBoard.Core.Models.ProductionHour;

namespace PFF
{
    public partial class PB_page : Page
    {
        private const string CurrentBoardIdViewStateKey =
            "CurrentProductionBoardId";

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

            LoadRequestedProductionBoard();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ProductionHoursSaveResult
            SaveProductionHours(
                ProductionHoursSaveRequest request)
        {
            try
            {
                HttpContext context =
                    HttpContext.Current;

                if (context == null ||
                    context.User == null ||
                    context.User.Identity == null ||
                    !context.User.Identity.IsAuthenticated)
                {
                    return ProductionHoursSaveResult.Failed(
                        "Your session has expired. Sign in again.");
                }

                if (context.Session == null)
                {
                    return ProductionHoursSaveResult.Failed(
                        "The authenticated session is not available.");
                }

                if (request == null)
                {
                    return ProductionHoursSaveResult.Failed(
                        "No production-hour data was received.");
                }

                string role =
                    Convert.ToString(
                        context.Session[
                            "CurrentUserRole"],
                        CultureInfo.InvariantCulture);

                if (!IsSupportedRole(role))
                {
                    return ProductionHoursSaveResult.Failed(
                        "Your account does not have access to this action.");
                }

                if (string.Equals(
                        role,
                        ApplicationRoles.Viewer,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return ProductionHoursSaveResult.Failed(
                        "Viewer accounts cannot modify production boards.");
                }

                ConnectionFactory connectionFactory =
                    new ConnectionFactory();

                ProductionBoardHourSaveRepository repository =
                    new ProductionBoardHourSaveRepository(
                        connectionFactory);

                string boardTeamName =
                    repository.GetBoardTeamName(
                        request.BoardId);

                if (string.IsNullOrWhiteSpace(
                        boardTeamName))
                {
                    return ProductionHoursSaveResult.Failed(
                        "The selected production board does not exist.");
                }

                if (string.Equals(
                        role,
                        ApplicationRoles.TeamLeader,
                        StringComparison.OrdinalIgnoreCase))
                {
                    string assignedTeamName =
                        Convert.ToString(
                            context.Session[
                                "CurrentTeamName"],
                            CultureInfo.InvariantCulture);

                    if (string.IsNullOrWhiteSpace(
                            assignedTeamName) ||
                        !string.Equals(
                            assignedTeamName.Trim(),
                            boardTeamName.Trim(),
                            StringComparison.OrdinalIgnoreCase))
                    {
                        return ProductionHoursSaveResult.Failed(
                            "You may view other teams, but you can edit only your assigned team board.");
                    }
                }

                return repository.SaveHours(
                    request.BoardId,
                    request.Hours);
            }
            catch (Exception exception)
            {
                return ProductionHoursSaveResult.Failed(
                    "The production hours could not be saved: " +
                    exception.Message);
            }
        }

        private void LoadRequestedProductionBoard()
        {
            try
            {
                string role =
                    GetSessionText(
                        "CurrentUserRole");

                if (!IsSupportedRole(role))
                {
                    SignOutInvalidSession();
                    return;
                }

                BoardModel board;

                int requestedBoardId;

                if (int.TryParse(
                        Request.QueryString["boardId"],
                        NumberStyles.Integer,
                        CultureInfo.InvariantCulture,
                        out requestedBoardId) &&
                    requestedBoardId > 0)
                {
                    board =
                        CreateProductionBoardService()
                            .GetById(
                                requestedBoardId);

                    if (board == null)
                    {
                        ShowPageError(
                            "The requested production board does not exist.");

                        return;
                    }
                }
                else
                {
                    string teamName;
                    string shiftName;
                    string lineName;
                    string productName;
                    DateTime boardDate;

                    if (!TryGetBoardContext(
                            out teamName,
                            out shiftName,
                            out lineName,
                            out productName,
                            out boardDate))
                    {
                        RedirectSafely(
                            "BoardSetup.aspx");

                        return;
                    }

                    bool isViewer =
                        string.Equals(
                            role,
                            ApplicationRoles.Viewer,
                            StringComparison.OrdinalIgnoreCase);

                    if (isViewer)
                    {
                        board =
                            FindExistingBoard(
                                boardDate,
                                teamName,
                                shiftName,
                                lineName,
                                productName);

                        if (board == null)
                        {
                            ShowPageError(
                                "No saved board matches this selection. Use Find Board to search existing tables.");

                            return;
                        }
                    }
                    else
                    {
                        board =
                            CreateProductionBoardService()
                                .GetOrCreate(
                                    boardDate,
                                    teamName,
                                    shiftName,
                                    lineName,
                                    productName);
                    }
                }

                if (!CanCurrentUserViewBoard(
                        role,
                        board))
                {
                    ShowPageError(
                        "You are not authorized to open this production board.");

                    return;
                }

                CurrentBoardId =
                    board.Id;

                CurrentBoardIdHiddenField.Value =
                    board.Id.ToString(
                        CultureInfo.InvariantCulture);

                StoreSelectedBoardContext(
                    board);

                BindProductionBoard(
                    board);

                ConfigureAccessMode(
                    role,
                    board);

                ConfigureAdjacentBoardLinks(
                    role,
                    board);

                lblCOUCOU.Text =
                    string.Empty;
            }
            catch (Exception)
            {
                ShowPageError(
                    "The production board could not be loaded. Check the SQL Server connection and try again.");
            }
        }

        private BoardModel FindExistingBoard(
            DateTime boardDate,
            string teamName,
            string shiftName,
            string lineName,
            string productName)
        {
            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            ProductionBoardRepository repository =
                new ProductionBoardRepository(
                    connectionFactory);

            BoardModel board =
                repository.Find(
                    boardDate,
                    teamName,
                    shiftName,
                    lineName,
                    productName);

            if (board == null)
            {
                return null;
            }

            ProductionCalculationService calculationService =
                new ProductionCalculationService();

            calculationService.CalculateCumulativeValues(
                board.Hours);

            return board;
        }

        private static ProductionBoardService
            CreateProductionBoardService()
        {
            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            ProductionBoardRepository repository =
                new ProductionBoardRepository(
                    connectionFactory);

            ProductionCalculationService calculationService =
                new ProductionCalculationService();

            return new ProductionBoardService(
                repository,
                calculationService);
        }

        private static ProductionBoardQueryService
            CreateProductionBoardQueryService()
        {
            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            ProductionBoardQueryRepository repository =
                new ProductionBoardQueryRepository(
                    connectionFactory);

            return new ProductionBoardQueryService(
                repository);
        }

        private void ConfigureAccessMode(
            string role,
            BoardModel board)
        {
            bool canEdit =
                string.Equals(
                    role,
                    ApplicationRoles.Admin,
                    StringComparison.OrdinalIgnoreCase)
                ||
                (
                    string.Equals(
                        role,
                        ApplicationRoles.TeamLeader,
                        StringComparison.OrdinalIgnoreCase)
                    &&
                    string.Equals(
                        board.TeamName,
                        GetSessionText(
                            "CurrentTeamName"),
                        StringComparison.OrdinalIgnoreCase)
                );

            CanEditProductionBoardHiddenField.Value =
                canEdit
                    ? "true"
                    : "false";

            ReadOnlyModeLabel.Visible =
                !canEdit;

            if (canEdit)
            {
                ReadOnlyModeLabel.Text =
                    string.Empty;
            }
            else if (string.Equals(
                         role,
                         ApplicationRoles.TeamLeader,
                         StringComparison.OrdinalIgnoreCase))
            {
                ReadOnlyModeLabel.Text =
                    "Other team board · read-only";
            }
            else
            {
                ReadOnlyModeLabel.Text =
                    "Viewer mode · read-only";
            }
        }

        private void ConfigureAdjacentBoardLinks(
            string role,
            BoardModel board)
        {
            /*
             * All authenticated application roles may navigate
             * through the previous and next team boards.
             * Team leaders still edit only their own team; when
             * another team board is open it becomes read-only.
             */
            ProductionBoardQueryService queryService =
                CreateProductionBoardQueryService();

            int? previousBoardId =
                queryService.FindPreviousBoardId(
                    board.Id,
                    null);

            int? nextBoardId =
                queryService.FindNextBoardId(
                    board.Id,
                    null);

            PreviousBoardLink.Visible =
                previousBoardId.HasValue;

            PreviousBoardLink.NavigateUrl =
                previousBoardId.HasValue
                    ? "PB_page.aspx?boardId=" +
                      previousBoardId.Value.ToString(
                          CultureInfo.InvariantCulture)
                    : string.Empty;

            NextBoardLink.Visible =
                nextBoardId.HasValue;

            NextBoardLink.NavigateUrl =
                nextBoardId.HasValue
                    ? "PB_page.aspx?boardId=" +
                      nextBoardId.Value.ToString(
                          CultureInfo.InvariantCulture)
                    : string.Empty;
        }

        private bool CanCurrentUserViewBoard(
            string role,
            BoardModel board)
        {
            if (board == null)
            {
                return false;
            }

            return IsSupportedRole(role);
        }

        private bool TryGetBoardContext(
            out string teamName,
            out string shiftName,
            out string lineName,
            out string productName,
            out DateTime boardDate)
        {
            teamName =
                GetPreferredSessionText(
                    "SelectedTeamName",
                    "value1");

            shiftName =
                GetPreferredSessionText(
                    "SelectedShiftName",
                    "value2");

            lineName =
                GetPreferredSessionText(
                    "SelectedLineName",
                    "value3");

            productName =
                GetPreferredSessionText(
                    "SelectedProductName",
                    "value5");

            boardDate =
                DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(teamName) ||
                string.IsNullOrWhiteSpace(shiftName) ||
                string.IsNullOrWhiteSpace(lineName) ||
                string.IsNullOrWhiteSpace(productName))
            {
                return false;
            }

            object dateValue =
                Session[
                    "SelectedBoardDate"]
                ??
                Session["value4"];

            if (dateValue == null)
            {
                return false;
            }

            if (dateValue is DateTime)
            {
                boardDate =
                    ((DateTime)dateValue).Date;

                return true;
            }

            string dateText =
                Convert.ToString(
                    dateValue,
                    CultureInfo.InvariantCulture);

            DateTime parsedDate;

            bool parsed =
                DateTime.TryParse(
                    dateText,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsedDate)
                ||
                DateTime.TryParse(
                    dateText,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None,
                    out parsedDate);

            if (!parsed)
            {
                return false;
            }

            boardDate =
                parsedDate.Date;

            return true;
        }

        private void StoreSelectedBoardContext(
            BoardModel board)
        {
            Session["SelectedTeamName"] =
                board.TeamName;

            Session["SelectedShiftName"] =
                board.ShiftName;

            Session["SelectedLineName"] =
                board.LineName;

            Session["SelectedProductName"] =
                board.ProductName;

            Session["SelectedBoardDate"] =
                board.BoardDate.Date;
        }

        private void BindProductionBoard(
            BoardModel board)
        {
            if (board == null)
            {
                throw new ArgumentNullException(
                    nameof(board));
            }

            DateLabel.Text =
                board.BoardDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);

            ShiftLabel.Text =
                HttpUtility.HtmlEncode(
                    board.ShiftName);

            PLLabel.Text =
                HttpUtility.HtmlEncode(
                    board.LineName);

            TeamLabel.Text =
                HttpUtility.HtmlEncode(
                    board.TeamName);

            ProductLabel.Text =
                HttpUtility.HtmlEncode(
                    board.ProductName);

            BindBoardHours(
                board.Hours);
        }

        private void BindBoardHours(
            IList<HourModel> hours)
        {
            IDictionary<int, HourModel> hoursByNumber =
                (hours ?? new List<HourModel>())
                .Where(hour => hour != null)
                .GroupBy(hour => hour.HourNumber)
                .ToDictionary(
                    group => group.Key,
                    group => group.First());

            Label[] hourLabels =
            {
                h1Label,
                h2Label,
                h3Label,
                h4Label,
                h5Label,
                h6Label,
                h7Label,
                h8Label
            };

            Label[] targetLabels =
            {
                h1Object,
                h2Object,
                h3Object,
                h4Object,
                h5Object,
                h6Object,
                h7Object,
                h8Object
            };

            Label[] targetCumulativeLabels =
            {
                OBJ_CML1,
                OBJ_CML2,
                OBJ_CML3,
                OBJ_CML4,
                OBJ_CML5,
                OBJ_CML6,
                OBJ_CML7,
                OBJ_CML8
            };

            for (int hourNumber = 1;
                 hourNumber <= 8;
                 hourNumber++)
            {
                HourModel hour;

                if (!hoursByNumber.TryGetValue(
                        hourNumber,
                        out hour))
                {
                    hour =
                        CreateEmptyHour(
                            hourNumber);
                }

                int index =
                    hourNumber - 1;

                hourLabels[index].Text =
                    HttpUtility.HtmlEncode(
                        hour.HourLabel);

                targetLabels[index].Text =
                    hour.TargetQuantity.ToString(
                        CultureInfo.InvariantCulture);

                targetCumulativeLabels[index].Text =
                    hour.TargetCumulative.ToString(
                        CultureInfo.InvariantCulture);
            }

            RegisterHourlyValuesScript(
                hoursByNumber);
        }

        private static HourModel CreateEmptyHour(
            int hourNumber)
        {
            return new HourModel
            {
                HourNumber =
                    hourNumber,

                HourLabel =
                    hourNumber.ToString(
                        CultureInfo.InvariantCulture),

                TargetQuantity = 0,
                ActualQuantity = 0,
                ScrapQuantity = 0,

                TargetCumulative = 0,
                ActualCumulative = 0,
                ScrapCumulative = 0,

                Comment =
                    string.Empty,

                StopType =
                    string.Empty,

                StopDurationMinutes =
                    0
            };
        }

        private void RegisterHourlyValuesScript(
            IDictionary<int, HourModel> hoursByNumber)
        {
            object[] payload =
                Enumerable
                    .Range(1, 8)
                    .Select(hourNumber =>
                    {
                        HourModel hour;

                        if (!hoursByNumber.TryGetValue(
                                hourNumber,
                                out hour))
                        {
                            hour =
                                CreateEmptyHour(
                                    hourNumber);
                        }

                        return new
                        {
                            hourNumber =
                                hour.HourNumber,

                            hourLabel =
                                hour.HourLabel ??
                                string.Empty,

                            actualQuantity =
                                hour.ActualQuantity,

                            actualCumulative =
                                hour.ActualCumulative,

                            scrapQuantity =
                                hour.ScrapQuantity,

                            scrapCumulative =
                                hour.ScrapCumulative,

                            comment =
                                hour.Comment ??
                                string.Empty,

                            stopType =
                                hour.StopType ??
                                string.Empty,

                            stopDurationMinutes =
                                hour.StopDurationMinutes
                        };
                    })
                    .Cast<object>()
                    .ToArray();

            JavaScriptSerializer serializer =
                new JavaScriptSerializer();

            string json =
                serializer
                    .Serialize(payload)
                    .Replace(
                        "</",
                        "<\\/");

            string script = @"
(function () {
    var hours = " + json + @";
    window.productionBoardHours = hours;

    function setText(elementId, value) {
        var element = document.getElementById(elementId);

        if (!element) {
            return;
        }

        element.textContent =
            value === null || value === undefined
                ? ''
                : value;
    }

    for (var index = 0; index < hours.length; index++) {
        var hour = hours[index];
        var number = hour.hourNumber;

        setText(
            'reel_h' + number,
            hour.actualQuantity);

        setText(
            'cumul_h' + number,
            hour.actualCumulative);

        setText(
            'rubut_h' + number,
            hour.scrapQuantity);

        setText(
            'cumulrubut_h' + number,
            hour.scrapCumulative);

        setText(
            'Commentaire_h' + number,
            hour.comment || '—');
    }
})();";

            ClientScript.RegisterStartupScript(
                GetType(),
                "bind-production-board-hours",
                script,
                true);
        }

        private string GetPreferredSessionText(
            string preferredKey,
            string fallbackKey)
        {
            string value =
                GetSessionText(
                    preferredKey);

            return !string.IsNullOrWhiteSpace(value)
                ? value
                : GetSessionText(
                    fallbackKey);
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

        private void ShowPageError(
            string message)
        {
            lblCOUCOU.Text =
                HttpUtility.HtmlEncode(
                    message);
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

        private int CurrentBoardId
        {
            get
            {
                object value =
                    ViewState[
                        CurrentBoardIdViewStateKey];

                if (value == null)
                {
                    return 0;
                }

                int boardId;

                return int.TryParse(
                    Convert.ToString(
                        value,
                        CultureInfo.InvariantCulture),
                    out boardId)
                    ? boardId
                    : 0;
            }

            set
            {
                ViewState[
                    CurrentBoardIdViewStateKey] =
                    value;
            }
        }
    }
}

namespace PFF
{
    public sealed class ProductionHourSaveItem
    {
        public int HourNumber { get; set; }

        public int ActualQuantity { get; set; }

        public int ScrapQuantity { get; set; }

        public string Comment { get; set; }
    }

    public sealed class ProductionHoursSaveRequest
    {
        public ProductionHoursSaveRequest()
        {
            Hours = new List<ProductionHourSaveItem>();
        }

        public int BoardId { get; set; }

        public IList<ProductionHourSaveItem> Hours { get; set; }
    }

    public sealed class ProductionHoursSaveResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int SavedCount { get; set; }

        public string UpdatedAt { get; set; }

        public static ProductionHoursSaveResult Failed(
            string message)
        {
            return new ProductionHoursSaveResult
            {
                Success = false,
                Message = message ??
                    "The production hours could not be saved.",
                SavedCount = 0,
                UpdatedAt = string.Empty
            };
        }
    }

    internal sealed class ProductionBoardHourSaveRepository
    {
        private const string HoursTableName =
            "dbo.ProductionBoardHours";

        private const string BoardsTableName =
            "dbo.ProductionBoards";

        private readonly ConnectionFactory _connectionFactory;

        public ProductionBoardHourSaveRepository(
            ConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(
                    nameof(connectionFactory));
            }

            _connectionFactory =
                connectionFactory;
        }

        public string GetBoardTeamName(
            int boardId)
        {
            if (boardId <= 0)
            {
                return null;
            }

            const string sql = @"
SELECT TeamName
FROM dbo.ProductionBoards
WHERE Id = @BoardId;";

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                connection.Open();

                object result =
                    command.ExecuteScalar();

                return result == null ||
                       result == DBNull.Value
                    ? null
                    : Convert.ToString(
                        result,
                        CultureInfo.InvariantCulture);
            }
        }

        public ProductionHoursSaveResult SaveHours(
            int boardId,
            IList<ProductionHourSaveItem> hours)
        {
            if (boardId <= 0)
            {
                return ProductionHoursSaveResult.Failed(
                    "The production board ID is invalid.");
            }

            IList<ProductionHourSaveItem> cleanHours;
            string validationError;

            if (!TryValidateHours(
                    hours,
                    out cleanHours,
                    out validationError))
            {
                return ProductionHoursSaveResult.Failed(
                    validationError);
            }

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            {
                connection.Open();

                if (!TableExists(
                        connection,
                        HoursTableName))
                {
                    return ProductionHoursSaveResult.Failed(
                        "The dbo.ProductionBoardHours table does not exist.");
                }

                if (!BoardExists(
                        connection,
                        boardId))
                {
                    return ProductionHoursSaveResult.Failed(
                        "The selected production board no longer exists.");
                }

                HourTableColumns columns =
                    ResolveHourTableColumns(
                        connection);

                if (!columns.IsValid)
                {
                    return ProductionHoursSaveResult.Failed(
                        columns.ValidationMessage);
                }

                using (SqlTransaction transaction =
                       connection.BeginTransaction())
                {
                    try
                    {
                        int savedCount = 0;
                        int actualCumulative = 0;
                        int scrapCumulative = 0;

                        foreach (ProductionHourSaveItem hour
                                 in cleanHours.OrderBy(
                                     item => item.HourNumber))
                        {
                            actualCumulative +=
                                hour.ActualQuantity;

                            scrapCumulative +=
                                hour.ScrapQuantity;

                            int affectedRows =
                                UpdateHour(
                                    connection,
                                    transaction,
                                    columns,
                                    boardId,
                                    hour,
                                    actualCumulative,
                                    scrapCumulative);

                            if (affectedRows == 0)
                            {
                                InsertHour(
                                    connection,
                                    transaction,
                                    columns,
                                    boardId,
                                    hour,
                                    actualCumulative,
                                    scrapCumulative);
                            }

                            savedCount++;
                        }

                        DateTime updatedAt =
                            UpdateBoardTimestamp(
                                connection,
                                transaction,
                                boardId);

                        transaction.Commit();

                        return new ProductionHoursSaveResult
                        {
                            Success = true,
                            Message =
                                "Production hours were saved successfully.",
                            SavedCount = savedCount,
                            UpdatedAt =
                                updatedAt.ToString(
                                    "yyyy-MM-dd HH:mm:ss",
                                    CultureInfo.InvariantCulture)
                        };
                    }
                    catch (Exception exception)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Preserve the original database error.
                        }

                        return ProductionHoursSaveResult.Failed(
                            "The production hours could not be saved: " +
                            exception.Message);
                    }
                }
            }
        }

        private static bool TryValidateHours(
            IList<ProductionHourSaveItem> hours,
            out IList<ProductionHourSaveItem> cleanHours,
            out string validationError)
        {
            cleanHours =
                new List<ProductionHourSaveItem>();

            validationError =
                string.Empty;

            if (hours == null ||
                hours.Count == 0)
            {
                validationError =
                    "No production-hour values were provided.";

                return false;
            }

            ISet<int> usedHourNumbers =
                new HashSet<int>();

            foreach (ProductionHourSaveItem hour
                     in hours)
            {
                if (hour == null)
                {
                    validationError =
                        "One of the production-hour values is invalid.";

                    return false;
                }

                if (hour.HourNumber < 1 ||
                    hour.HourNumber > 8)
                {
                    validationError =
                        "HourNumber must be between 1 and 8.";

                    return false;
                }

                if (!usedHourNumbers.Add(
                        hour.HourNumber))
                {
                    validationError =
                        "The same production hour was submitted more than once.";

                    return false;
                }

                if (hour.ActualQuantity < 0 ||
                    hour.ScrapQuantity < 0)
                {
                    validationError =
                        "Actual and scrap quantities cannot be negative.";

                    return false;
                }

                string comment =
                    hour.Comment == null
                        ? string.Empty
                        : hour.Comment.Trim();

                if (comment.Length > 1000)
                {
                    validationError =
                        "A production-hour comment cannot exceed 1000 characters.";

                    return false;
                }

                cleanHours.Add(
                    new ProductionHourSaveItem
                    {
                        HourNumber =
                            hour.HourNumber,
                        ActualQuantity =
                            hour.ActualQuantity,
                        ScrapQuantity =
                            hour.ScrapQuantity,
                        Comment =
                            comment
                    });
            }

            return true;
        }

        private static int UpdateHour(
            SqlConnection connection,
            SqlTransaction transaction,
            HourTableColumns columns,
            int boardId,
            ProductionHourSaveItem hour,
            int actualCumulative,
            int scrapCumulative)
        {
            IList<string> assignments =
                new List<string>
                {
                    QuoteIdentifier(columns.ActualQuantity) +
                    " = @ActualQuantity",

                    QuoteIdentifier(columns.ScrapQuantity) +
                    " = @ScrapQuantity"
                };

            if (!string.IsNullOrWhiteSpace(
                    columns.Comment))
            {
                assignments.Add(
                    QuoteIdentifier(columns.Comment) +
                    " = @Comment");
            }

            if (!string.IsNullOrWhiteSpace(
                    columns.ActualCumulative))
            {
                assignments.Add(
                    QuoteIdentifier(columns.ActualCumulative) +
                    " = @ActualCumulative");
            }

            if (!string.IsNullOrWhiteSpace(
                    columns.ScrapCumulative))
            {
                assignments.Add(
                    QuoteIdentifier(columns.ScrapCumulative) +
                    " = @ScrapCumulative");
            }

            DateTime timestamp =
                ResolveTimestamp(
                    columns.UpdatedAt);

            if (!string.IsNullOrWhiteSpace(
                    columns.UpdatedAt))
            {
                assignments.Add(
                    QuoteIdentifier(columns.UpdatedAt) +
                    " = @UpdatedAt");
            }

            string sql =
                "UPDATE " + HoursTableName +
                " SET " +
                string.Join(", ", assignments) +
                " WHERE " +
                QuoteIdentifier(columns.BoardForeignKey) +
                " = @BoardId AND " +
                QuoteIdentifier(columns.HourNumber) +
                " = @HourNumber;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                AddHourParameters(
                    command,
                    boardId,
                    hour,
                    actualCumulative,
                    scrapCumulative,
                    timestamp);

                return command.ExecuteNonQuery();
            }
        }

        private static void InsertHour(
            SqlConnection connection,
            SqlTransaction transaction,
            HourTableColumns columns,
            int boardId,
            ProductionHourSaveItem hour,
            int actualCumulative,
            int scrapCumulative)
        {
            IList<string> insertColumns =
                new List<string>();

            IList<string> insertValues =
                new List<string>();

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.BoardForeignKey,
                "@BoardId");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.HourNumber,
                "@HourNumber");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.ActualQuantity,
                "@ActualQuantity");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.ScrapQuantity,
                "@ScrapQuantity");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.Comment,
                "@Comment");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.HourLabel,
                "@HourLabel");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.TargetQuantity,
                "@TargetQuantity");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.TargetCumulative,
                "@TargetCumulative");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.ActualCumulative,
                "@ActualCumulative");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.ScrapCumulative,
                "@ScrapCumulative");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.StopType,
                "@StopType");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.StopDurationMinutes,
                "@StopDurationMinutes");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.CreatedAt,
                "@CreatedAt");

            AddInsertValue(
                insertColumns,
                insertValues,
                columns.UpdatedAt,
                "@UpdatedAt");

            string sql =
                "INSERT INTO " + HoursTableName +
                " (" +
                string.Join(", ", insertColumns) +
                ") VALUES (" +
                string.Join(", ", insertValues) +
                ");";

            DateTime createdAt =
                ResolveTimestamp(
                    columns.CreatedAt);

            DateTime updatedAt =
                ResolveTimestamp(
                    columns.UpdatedAt);

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                AddHourParameters(
                    command,
                    boardId,
                    hour,
                    actualCumulative,
                    scrapCumulative,
                    updatedAt);

                command.Parameters.Add(
                    "@HourLabel",
                    SqlDbType.NVarChar,
                    50).Value =
                    "H" +
                    hour.HourNumber.ToString(
                        CultureInfo.InvariantCulture);

                command.Parameters.Add(
                    "@TargetQuantity",
                    SqlDbType.Int).Value =
                    0;

                command.Parameters.Add(
                    "@TargetCumulative",
                    SqlDbType.Int).Value =
                    0;

                command.Parameters.Add(
                    "@StopType",
                    SqlDbType.NVarChar,
                    100).Value =
                    string.Empty;

                command.Parameters.Add(
                    "@StopDurationMinutes",
                    SqlDbType.Int).Value =
                    0;

                command.Parameters.Add(
                    "@CreatedAt",
                    SqlDbType.DateTime2).Value =
                    createdAt;

                command.ExecuteNonQuery();
            }
        }

        private static void AddHourParameters(
            SqlCommand command,
            int boardId,
            ProductionHourSaveItem hour,
            int actualCumulative,
            int scrapCumulative,
            DateTime updatedAt)
        {
            command.Parameters.Add(
                "@BoardId",
                SqlDbType.Int).Value =
                boardId;

            command.Parameters.Add(
                "@HourNumber",
                SqlDbType.Int).Value =
                hour.HourNumber;

            command.Parameters.Add(
                "@ActualQuantity",
                SqlDbType.Int).Value =
                hour.ActualQuantity;

            command.Parameters.Add(
                "@ScrapQuantity",
                SqlDbType.Int).Value =
                hour.ScrapQuantity;

            command.Parameters.Add(
                "@Comment",
                SqlDbType.NVarChar,
                1000).Value =
                hour.Comment ??
                string.Empty;

            command.Parameters.Add(
                "@ActualCumulative",
                SqlDbType.Int).Value =
                actualCumulative;

            command.Parameters.Add(
                "@ScrapCumulative",
                SqlDbType.Int).Value =
                scrapCumulative;

            command.Parameters.Add(
                "@UpdatedAt",
                SqlDbType.DateTime2).Value =
                updatedAt;
        }

        private static DateTime UpdateBoardTimestamp(
            SqlConnection connection,
            SqlTransaction transaction,
            int boardId)
        {
            string updatedAtColumn =
                FindFirstExistingColumn(
                    connection,
                    transaction,
                    BoardsTableName,
                    "UpdatedAt",
                    "UpdatedAtUtc",
                    "ModifiedAt",
                    "ModifiedAtUtc");

            DateTime timestamp =
                ResolveTimestamp(
                    updatedAtColumn);

            if (string.IsNullOrWhiteSpace(
                    updatedAtColumn))
            {
                return timestamp;
            }

            string sql =
                "UPDATE " + BoardsTableName +
                " SET " +
                QuoteIdentifier(updatedAtColumn) +
                " = @UpdatedAt" +
                " WHERE Id = @BoardId;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@UpdatedAt",
                    SqlDbType.DateTime2).Value =
                    timestamp;

                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                command.ExecuteNonQuery();
            }

            return timestamp;
        }

        private static bool BoardExists(
            SqlConnection connection,
            int boardId)
        {
            using (SqlCommand command =
                   new SqlCommand(
                       "SELECT COUNT(1) " +
                       "FROM dbo.ProductionBoards " +
                       "WHERE Id = @BoardId;",
                       connection))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                return Convert.ToInt32(
                    command.ExecuteScalar(),
                    CultureInfo.InvariantCulture) > 0;
            }
        }

        private static bool TableExists(
            SqlConnection connection,
            string tableName)
        {
            using (SqlCommand command =
                   new SqlCommand(
                       "SELECT OBJECT_ID(@TableName, 'U');",
                       connection))
            {
                command.Parameters.Add(
                    "@TableName",
                    SqlDbType.NVarChar,
                    256).Value =
                    tableName;

                object result =
                    command.ExecuteScalar();

                return result != null &&
                       result != DBNull.Value;
            }
        }

        private static HourTableColumns ResolveHourTableColumns(
            SqlConnection connection)
        {
            HourTableColumns columns =
                new HourTableColumns
                {
                    BoardForeignKey =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "ProductionBoardId",
                            "BoardId"),

                    HourNumber =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "HourNumber",
                            "HourIndex",
                            "HourNo"),

                    HourLabel =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "HourLabel",
                            "TimeLabel"),

                    TargetQuantity =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "TargetQuantity",
                            "ObjectiveQuantity",
                            "ObjectQuantity"),

                    ActualQuantity =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "ActualQuantity",
                            "RealQuantity",
                            "ReelQuantity",
                            "Actual",
                            "Real",
                            "Reel"),

                    ScrapQuantity =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "ScrapQuantity",
                            "RejectQuantity",
                            "RebutQuantity",
                            "Scrap",
                            "Reject",
                            "Rebut"),

                    Comment =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "Comment",
                            "Comments",
                            "CommentText",
                            "Commentaire"),

                    TargetCumulative =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "TargetCumulative",
                            "ObjectiveCumulative",
                            "ObjectCumulative"),

                    ActualCumulative =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "ActualCumulative",
                            "RealCumulative",
                            "ReelCumulative",
                            "CumulativeActual",
                            "CumulativeReal",
                            "CumulativeReel"),

                    ScrapCumulative =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "ScrapCumulative",
                            "RejectCumulative",
                            "RebutCumulative",
                            "CumulativeScrap",
                            "CumulativeReject",
                            "CumulativeRebut"),

                    StopType =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "StopType"),

                    StopDurationMinutes =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "StopDurationMinutes"),

                    CreatedAt =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "CreatedAt",
                            "CreatedAtUtc"),

                    UpdatedAt =
                        FindFirstExistingColumn(
                            connection,
                            null,
                            HoursTableName,
                            "UpdatedAt",
                            "UpdatedAtUtc",
                            "ModifiedAt",
                            "ModifiedAtUtc")
                };

            return columns;
        }

        private static string FindFirstExistingColumn(
            SqlConnection connection,
            SqlTransaction transaction,
            string tableName,
            params string[] candidates)
        {
            foreach (string candidate in candidates)
            {
                using (SqlCommand command =
                       new SqlCommand(
                           "SELECT COL_LENGTH(@TableName, @ColumnName);",
                           connection,
                           transaction))
                {
                    command.Parameters.Add(
                        "@TableName",
                        SqlDbType.NVarChar,
                        256).Value =
                        tableName;

                    command.Parameters.Add(
                        "@ColumnName",
                        SqlDbType.NVarChar,
                        128).Value =
                        candidate;

                    object result =
                        command.ExecuteScalar();

                    if (result != null &&
                        result != DBNull.Value)
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }

        private static DateTime ResolveTimestamp(
            string columnName)
        {
            return !string.IsNullOrWhiteSpace(columnName) &&
                   columnName.EndsWith(
                       "Utc",
                       StringComparison.OrdinalIgnoreCase)
                ? DateTime.UtcNow
                : DateTime.Now;
        }

        private static void AddInsertValue(
            IList<string> columns,
            IList<string> values,
            string columnName,
            string parameterName)
        {
            if (string.IsNullOrWhiteSpace(
                    columnName))
            {
                return;
            }

            columns.Add(
                QuoteIdentifier(columnName));

            values.Add(
                parameterName);
        }

        private static string QuoteIdentifier(
            string identifier)
        {
            return "[" +
                identifier.Replace(
                    "]",
                    "]]" ) +
                "]";
        }

        private sealed class HourTableColumns
        {
            public string BoardForeignKey { get; set; }

            public string HourNumber { get; set; }

            public string HourLabel { get; set; }

            public string TargetQuantity { get; set; }

            public string ActualQuantity { get; set; }

            public string ScrapQuantity { get; set; }

            public string Comment { get; set; }

            public string TargetCumulative { get; set; }

            public string ActualCumulative { get; set; }

            public string ScrapCumulative { get; set; }

            public string StopType { get; set; }

            public string StopDurationMinutes { get; set; }

            public string CreatedAt { get; set; }

            public string UpdatedAt { get; set; }

            public bool IsValid
            {
                get
                {
                    return
                        !string.IsNullOrWhiteSpace(
                            BoardForeignKey)
                        &&
                        !string.IsNullOrWhiteSpace(
                            HourNumber)
                        &&
                        !string.IsNullOrWhiteSpace(
                            ActualQuantity)
                        &&
                        !string.IsNullOrWhiteSpace(
                            ScrapQuantity);
                }
            }

            public string ValidationMessage
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(
                            BoardForeignKey))
                    {
                        return
                            "ProductionBoardHours must contain ProductionBoardId or BoardId.";
                    }

                    if (string.IsNullOrWhiteSpace(
                            HourNumber))
                    {
                        return
                            "ProductionBoardHours must contain HourNumber.";
                    }

                    if (string.IsNullOrWhiteSpace(
                            ActualQuantity))
                    {
                        return
                            "ProductionBoardHours must contain ActualQuantity, RealQuantity or ReelQuantity.";
                    }

                    if (string.IsNullOrWhiteSpace(
                            ScrapQuantity))
                    {
                        return
                            "ProductionBoardHours must contain ScrapQuantity, RejectQuantity or RebutQuantity.";
                    }

                    return string.Empty;
                }
            }
        }
    }

}
