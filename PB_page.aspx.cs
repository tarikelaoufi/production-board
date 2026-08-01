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

        /*
         * Planned stops are independent from the product rate.
         *
         * H1: 5-minute team meeting
         * H5: 20-minute planned pause
         * H8: 10-minute workstation organization
         */
        private static readonly int[] PlannedStopMinutesByHour =
        {
            5,
            0,
            0,
            0,
            20,
            0,
            0,
            10
        };

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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ProductChangeResult
            ChangeProduct(
                ProductChangeRequest request)
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
                    return ProductChangeResult.Failed(
                        "Your session has expired. Sign in again.");
                }

                if (context.Session == null)
                {
                    return ProductChangeResult.Failed(
                        "The authenticated session is not available.");
                }

                if (request == null)
                {
                    return ProductChangeResult.Failed(
                        "No product-change data was received.");
                }

                string role =
                    Convert.ToString(
                        context.Session[
                            "CurrentUserRole"],
                        CultureInfo.InvariantCulture);

                if (!IsSupportedRole(role))
                {
                    return ProductChangeResult.Failed(
                        "Your account does not have access to this action.");
                }

                if (string.Equals(
                        role,
                        ApplicationRoles.Viewer,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return ProductChangeResult.Failed(
                        "Viewer accounts cannot change products.");
                }

                ConnectionFactory connectionFactory =
                    new ConnectionFactory();

                ProductionBoardProductPlanRepository repository =
                    new ProductionBoardProductPlanRepository(
                        connectionFactory);

                string boardTeamName =
                    repository.GetBoardTeamName(
                        request.BoardId);

                if (string.IsNullOrWhiteSpace(
                        boardTeamName))
                {
                    return ProductChangeResult.Failed(
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
                        return ProductChangeResult.Failed(
                            "You may view other teams, but you can change products only on your assigned team board.");
                    }
                }

                string changedBy =
                    context.User.Identity.Name ??
                    string.Empty;

                return repository.ChangeProduct(
                    request,
                    changedBy);
            }
            catch (Exception exception)
            {
                return ProductChangeResult.Failed(
                    "The product could not be changed: " +
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
                    "Other team board \u00B7 read-only";
            }
            else
            {
                ReadOnlyModeLabel.Text =
                    "Viewer mode \u00B7 read-only";
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

            ConnectionFactory connectionFactory =
                new ConnectionFactory();

            ProductionBoardProductPlanRepository productRepository =
                new ProductionBoardProductPlanRepository(
                    connectionFactory);

            ProductPlanData productPlan =
                productRepository.EnsureAndLoadPlan(
                    board.Id,
                    board.ProductName);

            ApplyProductPlan(
                board.Hours,
                productPlan.Hours);

            ProductLabel.Text =
                HttpUtility.HtmlEncode(
                    productPlan.HeaderProductText);

            ProductLabel.Attributes[
                "data-product-mode"] =
                productPlan.IsMixed
                    ? "mixed"
                    : "single";

            BindBoardHours(
                board.Hours);

            RegisterProductPlanScript(
                productPlan);
        }

        private static void ApplyProductPlan(
            IList<HourModel> hours,
            IList<BoardHourProductPlanItem> productHours)
        {
            if (hours == null)
            {
                return;
            }

            IDictionary<int, BoardHourProductPlanItem> planByHour =
                (productHours ??
                 new List<BoardHourProductPlanItem>())
                    .Where(item => item != null)
                    .GroupBy(item => item.HourNumber)
                    .ToDictionary(
                        group => group.Key,
                        group => group.First());

            int targetCumulative = 0;

            foreach (HourModel hour in
                     hours
                         .Where(item => item != null)
                         .OrderBy(item => item.HourNumber))
            {
                BoardHourProductPlanItem planItem;

                if (planByHour.TryGetValue(
                        hour.HourNumber,
                        out planItem))
                {
                    hour.TargetQuantity =
                        planItem.TargetQuantity;
                }

                targetCumulative +=
                    hour.TargetQuantity;

                hour.TargetCumulative =
                    targetCumulative;
            }
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

                            targetQuantity =
                                hour.TargetQuantity,

                            targetCumulative =
                                hour.TargetCumulative,

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
            hour.comment || '\u2014');
    }
})();";

            ClientScript.RegisterStartupScript(
                GetType(),
                "bind-production-board-hours",
                script,
                true);
        }

        private void RegisterProductPlanScript(
            ProductPlanData productPlan)
        {
            JavaScriptSerializer serializer =
                new JavaScriptSerializer();

            string json =
                serializer
                    .Serialize(
                        productPlan ??
                        new ProductPlanData())
                    .Replace(
                        "</",
                        "<\\/");

            string script =
                "window.productionBoardProductPlan = " +
                json +
                ";";

            ClientScript.RegisterStartupScript(
                GetType(),
                "bind-production-board-product-plan",
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


namespace PFF
{
    public sealed class ProductChangeRequest
    {
        public int BoardId { get; set; }

        public int NewProductId { get; set; }

        public int EffectiveHourNumber { get; set; }

        public int ChangeoverMinutes { get; set; }

        public string Reason { get; set; }
    }

    public sealed class ProductChangeResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public ProductPlanData Plan { get; set; }

        public static ProductChangeResult Failed(
            string message)
        {
            return new ProductChangeResult
            {
                Success = false,
                Message =
                    message ??
                    "The product could not be changed.",
                Plan = null
            };
        }
    }

    public sealed class ProductOptionItem
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int StandardRatePerHour { get; set; }

        public decimal UnitsPerMinute
        {
            get
            {
                return StandardRatePerHour / 60m;
            }
        }
    }

    public sealed class BoardHourProductPlanItem
    {
        public int HourNumber { get; set; }

        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int RatePerHour { get; set; }

        public int PlannedStopMinutes { get; set; }

        public int ChangeoverMinutes { get; set; }

        public int AvailableMinutes { get; set; }

        public int TargetQuantity { get; set; }

        public int TargetCumulative { get; set; }

        public bool IsProductChange { get; set; }

        public string PreviousProductName { get; set; }

        public string ChangeReason { get; set; }
    }

    public sealed class ProductPlanData
    {
        public ProductPlanData()
        {
            Products =
                new List<ProductOptionItem>();

            Hours =
                new List<BoardHourProductPlanItem>();
        }

        public string HeaderProductText { get; set; }

        public bool IsMixed { get; set; }

        public IList<ProductOptionItem> Products { get; set; }

        public IList<BoardHourProductPlanItem> Hours { get; set; }
    }

    internal sealed class ProductionBoardProductPlanRepository
    {
        private const string MigrationFileName =
            "ProductionBoard.Data/Sql/004_CreateProductsAndProductChanges.sql";

        private static readonly object SchemaVerificationLock =
            new object();

        private static bool _schemaVerified;

        private static readonly int[] PlannedStopMinutesByHour =
        {
            5,
            0,
            0,
            0,
            20,
            0,
            0,
            10
        };

        private readonly ConnectionFactory _connectionFactory;

        public ProductionBoardProductPlanRepository(
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

        public ProductPlanData EnsureAndLoadPlan(
            int boardId,
            string fallbackInitialProductName)
        {
            if (boardId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(boardId));
            }

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            {
                connection.Open();

                EnsureSchema(connection);

                bool needsPersistence =
                    NeedsPlanPersistence(
                        connection,
                        boardId);

                IList<ProductOptionItem> products =
                    LoadActiveProducts(
                        connection,
                        null);

                BoardInitialProduct initialProduct =
                    LoadAndEnsureInitialProduct(
                        connection,
                        null,
                        boardId,
                        fallbackInitialProductName);

                IDictionary<int, ProductChangeSnapshot> changes =
                    LoadChanges(
                        connection,
                        null,
                        boardId);

                ProductPlanData plan =
                    BuildPlan(
                        products,
                        initialProduct,
                        changes);

                /*
                 * A normal page view is read-only. The hourly product
                 * snapshot is written only once for newly created or
                 * legacy boards. Product changes already persist their
                 * recalculated plan in ChangeProduct().
                 */
                if (needsPersistence)
                {
                    using (SqlTransaction transaction =
                           connection.BeginTransaction())
                    {
                        try
                        {
                            PersistPlan(
                                connection,
                                transaction,
                                boardId,
                                plan.Hours);

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return plan;
            }
        }

        public ProductChangeResult ChangeProduct(
            ProductChangeRequest request,
            string changedBy)
        {
            string validationError;

            if (!TryValidateRequest(
                    request,
                    out validationError))
            {
                return ProductChangeResult.Failed(
                    validationError);
            }

            using (SqlConnection connection =
                   _connectionFactory.CreateConnection())
            {
                connection.Open();

                EnsureSchema(connection);

                using (SqlTransaction transaction =
                       connection.BeginTransaction())
                {
                    try
                    {
                        IList<ProductOptionItem> products =
                            LoadActiveProducts(
                                connection,
                                transaction);

                        ProductOptionItem newProduct =
                            products.FirstOrDefault(
                                product =>
                                    product.Id ==
                                    request.NewProductId);

                        if (newProduct == null)
                        {
                            return ProductChangeResult.Failed(
                                "The selected product does not exist or is inactive.");
                        }

                        BoardInitialProduct initialProduct =
                            LoadAndEnsureInitialProduct(
                                connection,
                                transaction,
                                request.BoardId,
                                null);

                        IDictionary<int, ProductChangeSnapshot> changes =
                            LoadChanges(
                                connection,
                                transaction,
                                request.BoardId);

                        ProductSnapshot previousProduct =
                            ResolveProductBeforeHour(
                                initialProduct,
                                changes,
                                request.EffectiveHourNumber);

                        if (previousProduct.ProductId ==
                                newProduct.Id &&
                            !changes.ContainsKey(
                                request.EffectiveHourNumber))
                        {
                            return ProductChangeResult.Failed(
                                newProduct.Name +
                                " is already active at H" +
                                request.EffectiveHourNumber
                                    .ToString(
                                        CultureInfo.InvariantCulture) +
                                ".");
                        }

                        int maximumChangeoverMinutes =
                            60 -
                            GetPlannedStopMinutes(
                                request.EffectiveHourNumber);

                        if (request.ChangeoverMinutes >
                            maximumChangeoverMinutes)
                        {
                            return ProductChangeResult.Failed(
                                "The changeover cannot exceed " +
                                maximumChangeoverMinutes.ToString(
                                    CultureInfo.InvariantCulture) +
                                " minutes for this hour because it already contains a planned stop.");
                        }

                        UpsertChange(
                            connection,
                            transaction,
                            request,
                            previousProduct,
                            newProduct,
                            changedBy);

                        changes =
                            LoadChanges(
                                connection,
                                transaction,
                                request.BoardId);

                        ProductPlanData plan =
                            BuildPlan(
                                products,
                                initialProduct,
                                changes);

                        PersistPlan(
                            connection,
                            transaction,
                            request.BoardId,
                            plan.Hours);

                        UpdateBoardTimestamp(
                            connection,
                            transaction,
                            request.BoardId);

                        transaction.Commit();

                        return new ProductChangeResult
                        {
                            Success = true,
                            Message =
                                "Product changed successfully from H" +
                                request.EffectiveHourNumber.ToString(
                                    CultureInfo.InvariantCulture) +
                                ".",
                            Plan = plan
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

                        return ProductChangeResult.Failed(
                            "The product could not be changed: " +
                            exception.Message);
                    }
                }
            }
        }

        private static bool TryValidateRequest(
            ProductChangeRequest request,
            out string validationError)
        {
            validationError =
                string.Empty;

            if (request == null)
            {
                validationError =
                    "No product-change data was received.";

                return false;
            }

            if (request.BoardId <= 0)
            {
                validationError =
                    "The production board ID is invalid.";

                return false;
            }

            if (request.NewProductId <= 0)
            {
                validationError =
                    "Select a valid product.";

                return false;
            }

            if (request.EffectiveHourNumber < 1 ||
                request.EffectiveHourNumber > 8)
            {
                validationError =
                    "The effective hour must be between H1 and H8.";

                return false;
            }

            if (request.ChangeoverMinutes < 0 ||
                request.ChangeoverMinutes > 60)
            {
                validationError =
                    "Changeover duration must be between 0 and 60 minutes.";

                return false;
            }

            string reason =
                request.Reason == null
                    ? string.Empty
                    : request.Reason.Trim();

            if (reason.Length > 500)
            {
                validationError =
                    "The reason cannot exceed 500 characters.";

                return false;
            }

            request.Reason =
                reason;

            return true;
        }

        private static void EnsureSchema(
            SqlConnection connection)
        {
            if (_schemaVerified)
            {
                return;
            }

            lock (SchemaVerificationLock)
            {
                if (_schemaVerified)
                {
                    return;
                }

                const string sql = @"
SELECT
    CASE
        WHEN OBJECT_ID(N'dbo.ProductionProducts', N'U') IS NOT NULL
         AND OBJECT_ID(N'dbo.ProductionBoardProductChanges', N'U') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoards', N'InitialProductId') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoards', N'InitialProductRatePerHourSnapshot') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoardHours', N'ProductId') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoardHours', N'ProductNameSnapshot') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoardHours', N'RatePerHourSnapshot') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoardHours', N'PlannedStopMinutes') IS NOT NULL
         AND COL_LENGTH(N'dbo.ProductionBoardHours', N'ChangeoverMinutes') IS NOT NULL
        THEN 1
        ELSE 0
    END;";

                using (SqlCommand command =
                       new SqlCommand(sql, connection))
                {
                    bool isReady =
                        Convert.ToInt32(
                            command.ExecuteScalar(),
                            CultureInfo.InvariantCulture) == 1;

                    if (!isReady)
                    {
                        throw new InvalidOperationException(
                            "The product-rate database migration is missing. Run " +
                            MigrationFileName +
                            " in SQL Server.");
                    }
                }

                _schemaVerified = true;
            }
        }

        private static bool NeedsPlanPersistence(
            SqlConnection connection,
            int boardId)
        {
            const string sql = @"
SELECT
    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM dbo.ProductionBoardHours
            WHERE ProductionBoardId = @BoardId
              AND
              (
                  ProductId IS NULL
                  OR ProductNameSnapshot IS NULL
                  OR RatePerHourSnapshot IS NULL
              )
        )
        THEN 1
        ELSE 0
    END;";

            using (SqlCommand command =
                   new SqlCommand(sql, connection))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                return Convert.ToInt32(
                    command.ExecuteScalar(),
                    CultureInfo.InvariantCulture) == 1;
            }
        }

        private static IList<ProductOptionItem>
            LoadActiveProducts(
                SqlConnection connection,
                SqlTransaction transaction)
        {
            const string sql = @"
SELECT
    Id,
    Code,
    Name,
    StandardRatePerHour
FROM dbo.ProductionProducts
WHERE IsActive = 1
ORDER BY Name;";

            IList<ProductOptionItem> products =
                new List<ProductOptionItem>();

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            using (SqlDataReader reader =
                   command.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(
                        new ProductOptionItem
                        {
                            Id =
                                reader.GetInt32(
                                    reader.GetOrdinal("Id")),

                            Code =
                                reader.GetString(
                                    reader.GetOrdinal("Code")),

                            Name =
                                reader.GetString(
                                    reader.GetOrdinal("Name")),

                            StandardRatePerHour =
                                reader.GetInt32(
                                    reader.GetOrdinal(
                                        "StandardRatePerHour"))
                        });
                }
            }

            if (products.Count == 0)
            {
                throw new InvalidOperationException(
                    "No active product is configured.");
            }

            return products;
        }

        private static BoardInitialProduct
            LoadAndEnsureInitialProduct(
                SqlConnection connection,
                SqlTransaction transaction,
                int boardId,
                string fallbackInitialProductName)
        {
            const string boardSql = @"
SELECT
    ProductName,
    InitialProductId,
    InitialProductRatePerHourSnapshot
FROM dbo.ProductionBoards
WHERE Id = @BoardId;";

            string boardProductName;
            int? initialProductId;
            int? initialRate;

            using (SqlCommand command =
                   new SqlCommand(
                       boardSql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException(
                            "The selected production board does not exist.");
                    }

                    boardProductName =
                        reader.IsDBNull(
                            reader.GetOrdinal("ProductName"))
                            ? fallbackInitialProductName
                            : reader.GetString(
                                reader.GetOrdinal("ProductName"));

                    initialProductId =
                        reader.IsDBNull(
                            reader.GetOrdinal("InitialProductId"))
                            ? (int?)null
                            : reader.GetInt32(
                                reader.GetOrdinal("InitialProductId"));

                    initialRate =
                        reader.IsDBNull(
                            reader.GetOrdinal(
                                "InitialProductRatePerHourSnapshot"))
                            ? (int?)null
                            : reader.GetInt32(
                                reader.GetOrdinal(
                                    "InitialProductRatePerHourSnapshot"));
                }
            }

            if (initialProductId.HasValue &&
                initialRate.HasValue)
            {
                ProductOptionItem storedProduct =
                    LoadProductById(
                        connection,
                        transaction,
                        initialProductId.Value);

                return new BoardInitialProduct
                {
                    ProductId =
                        storedProduct.Id,

                    ProductCode =
                        storedProduct.Code,

                    ProductName =
                        string.IsNullOrWhiteSpace(
                            boardProductName)
                            ? storedProduct.Name
                            : boardProductName.Trim(),

                    RatePerHour =
                        initialRate.Value
                };
            }

            string productName =
                !string.IsNullOrWhiteSpace(
                    boardProductName)
                    ? boardProductName.Trim()
                    : fallbackInitialProductName;

            ProductOptionItem resolvedProduct =
                LoadProductByNameOrCode(
                    connection,
                    transaction,
                    productName);

            if (resolvedProduct == null)
            {
                throw new InvalidOperationException(
                    "The board's initial product '" +
                    productName +
                    "' is not configured in dbo.ProductionProducts.");
            }

            const string updateSql = @"
UPDATE dbo.ProductionBoards
SET
    InitialProductId = @InitialProductId,
    InitialProductRatePerHourSnapshot = @InitialRate,
    UpdatedAt = SYSUTCDATETIME()
WHERE Id = @BoardId;";

            using (SqlCommand updateCommand =
                   new SqlCommand(
                       updateSql,
                       connection,
                       transaction))
            {
                updateCommand.Parameters.Add(
                    "@InitialProductId",
                    SqlDbType.Int).Value =
                    resolvedProduct.Id;

                updateCommand.Parameters.Add(
                    "@InitialRate",
                    SqlDbType.Int).Value =
                    resolvedProduct.StandardRatePerHour;

                updateCommand.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                updateCommand.ExecuteNonQuery();
            }

            return new BoardInitialProduct
            {
                ProductId =
                    resolvedProduct.Id,

                ProductCode =
                    resolvedProduct.Code,

                ProductName =
                    resolvedProduct.Name,

                RatePerHour =
                    resolvedProduct.StandardRatePerHour
            };
        }

        private static ProductOptionItem LoadProductById(
            SqlConnection connection,
            SqlTransaction transaction,
            int productId)
        {
            const string sql = @"
SELECT
    Id,
    Code,
    Name,
    StandardRatePerHour
FROM dbo.ProductionProducts
WHERE Id = @ProductId;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@ProductId",
                    SqlDbType.Int).Value =
                    productId;

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException(
                            "The board's initial product no longer exists.");
                    }

                    return new ProductOptionItem
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        Code =
                            reader.GetString(
                                reader.GetOrdinal("Code")),

                        Name =
                            reader.GetString(
                                reader.GetOrdinal("Name")),

                        StandardRatePerHour =
                            reader.GetInt32(
                                reader.GetOrdinal(
                                    "StandardRatePerHour"))
                    };
                }
            }
        }

        private static ProductOptionItem
            LoadProductByNameOrCode(
                SqlConnection connection,
                SqlTransaction transaction,
                string productNameOrCode)
        {
            if (string.IsNullOrWhiteSpace(
                    productNameOrCode))
            {
                return null;
            }

            const string sql = @"
SELECT TOP (1)
    Id,
    Code,
    Name,
    StandardRatePerHour
FROM dbo.ProductionProducts
WHERE Name = @Value
   OR Code = @Value
ORDER BY
    CASE WHEN Name = @Value THEN 0 ELSE 1 END,
    Id;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@Value",
                    SqlDbType.NVarChar,
                    150).Value =
                    productNameOrCode.Trim();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new ProductOptionItem
                    {
                        Id =
                            reader.GetInt32(
                                reader.GetOrdinal("Id")),

                        Code =
                            reader.GetString(
                                reader.GetOrdinal("Code")),

                        Name =
                            reader.GetString(
                                reader.GetOrdinal("Name")),

                        StandardRatePerHour =
                            reader.GetInt32(
                                reader.GetOrdinal(
                                    "StandardRatePerHour"))
                    };
                }
            }
        }

        private static IDictionary<int, ProductChangeSnapshot>
            LoadChanges(
                SqlConnection connection,
                SqlTransaction transaction,
                int boardId)
        {
            const string sql = @"
SELECT
    EffectiveHourNumber,
    NewProductId,
    NewProductCodeSnapshot,
    NewProductNameSnapshot,
    NewRatePerHourSnapshot,
    ChangeoverMinutes,
    Reason
FROM dbo.ProductionBoardProductChanges
WHERE ProductionBoardId = @BoardId
ORDER BY EffectiveHourNumber;";

            IDictionary<int, ProductChangeSnapshot> changes =
                new Dictionary<int, ProductChangeSnapshot>();

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int hourNumber =
                            Convert.ToInt32(
                                reader["EffectiveHourNumber"],
                                CultureInfo.InvariantCulture);

                        changes[hourNumber] =
                            new ProductChangeSnapshot
                            {
                                EffectiveHourNumber =
                                    hourNumber,

                                NewProductId =
                                    reader.GetInt32(
                                        reader.GetOrdinal(
                                            "NewProductId")),

                                NewProductCode =
                                    reader.GetString(
                                        reader.GetOrdinal(
                                            "NewProductCodeSnapshot")),

                                NewProductName =
                                    reader.GetString(
                                        reader.GetOrdinal(
                                            "NewProductNameSnapshot")),

                                NewRatePerHour =
                                    reader.GetInt32(
                                        reader.GetOrdinal(
                                            "NewRatePerHourSnapshot")),

                                ChangeoverMinutes =
                                    reader.GetInt32(
                                        reader.GetOrdinal(
                                            "ChangeoverMinutes")),

                                Reason =
                                    reader.IsDBNull(
                                        reader.GetOrdinal("Reason"))
                                        ? string.Empty
                                        : reader.GetString(
                                            reader.GetOrdinal("Reason"))
                            };
                    }
                }
            }

            return changes;
        }

        private static ProductPlanData BuildPlan(
            IList<ProductOptionItem> products,
            BoardInitialProduct initialProduct,
            IDictionary<int, ProductChangeSnapshot> changes)
        {
            ProductPlanData plan =
                new ProductPlanData
                {
                    Products =
                        products ??
                        new List<ProductOptionItem>()
                };

            ProductSnapshot currentProduct =
                new ProductSnapshot
                {
                    ProductId =
                        initialProduct.ProductId,

                    ProductCode =
                        initialProduct.ProductCode,

                    ProductName =
                        initialProduct.ProductName,

                    RatePerHour =
                        initialProduct.RatePerHour
                };

            int cumulativeTarget = 0;
            ISet<int> usedProducts =
                new HashSet<int>();

            for (int hourNumber = 1;
                 hourNumber <= 8;
                 hourNumber++)
            {
                ProductChangeSnapshot change = null;
                bool isChange =
                    changes != null &&
                    changes.TryGetValue(
                        hourNumber,
                        out change);

                string previousProductName =
                    string.Empty;

                int changeoverMinutes = 0;
                string changeReason =
                    string.Empty;

                if (isChange)
                {
                    previousProductName =
                        currentProduct.ProductName;

                    currentProduct =
                        new ProductSnapshot
                        {
                            ProductId =
                                change.NewProductId,

                            ProductCode =
                                change.NewProductCode,

                            ProductName =
                                change.NewProductName,

                            RatePerHour =
                                change.NewRatePerHour
                        };

                    changeoverMinutes =
                        change.ChangeoverMinutes;

                    changeReason =
                        change.Reason ??
                        string.Empty;
                }

                int plannedStopMinutes =
                    GetPlannedStopMinutes(
                        hourNumber);

                int availableMinutes =
                    Math.Max(
                        0,
                        60 -
                        plannedStopMinutes -
                        changeoverMinutes);

                int targetQuantity =
                    Convert.ToInt32(
                        Math.Round(
                            currentProduct.RatePerHour *
                            availableMinutes /
                            60m,
                            0,
                            MidpointRounding.AwayFromZero),
                        CultureInfo.InvariantCulture);

                cumulativeTarget +=
                    targetQuantity;

                usedProducts.Add(
                    currentProduct.ProductId);

                plan.Hours.Add(
                    new BoardHourProductPlanItem
                    {
                        HourNumber =
                            hourNumber,

                        ProductId =
                            currentProduct.ProductId,

                        ProductCode =
                            currentProduct.ProductCode,

                        ProductName =
                            currentProduct.ProductName,

                        RatePerHour =
                            currentProduct.RatePerHour,

                        PlannedStopMinutes =
                            plannedStopMinutes,

                        ChangeoverMinutes =
                            changeoverMinutes,

                        AvailableMinutes =
                            availableMinutes,

                        TargetQuantity =
                            targetQuantity,

                        TargetCumulative =
                            cumulativeTarget,

                        IsProductChange =
                            isChange,

                        PreviousProductName =
                            previousProductName,

                        ChangeReason =
                            changeReason
                    });
            }

            plan.IsMixed =
                usedProducts.Count > 1;

            plan.HeaderProductText =
                plan.IsMixed
                    ? "Mixed production"
                    : plan.Hours.Count > 0
                        ? plan.Hours[0].ProductName
                        : initialProduct.ProductName;

            return plan;
        }

        private static void PersistPlan(
            SqlConnection connection,
            SqlTransaction transaction,
            int boardId,
            IList<BoardHourProductPlanItem> hours)
        {
            const string sql = @"
UPDATE dbo.ProductionBoardHours
SET
    ProductId = @ProductId,
    ProductNameSnapshot = @ProductNameSnapshot,
    RatePerHourSnapshot = @RatePerHourSnapshot,
    PlannedStopMinutes = @PlannedStopMinutes,
    ChangeoverMinutes = @ChangeoverMinutes,
    TargetQuantity = @TargetQuantity,
    UpdatedAt = SYSUTCDATETIME()
WHERE ProductionBoardId = @BoardId
  AND HourNumber = @HourNumber;";

            foreach (BoardHourProductPlanItem hour in
                     hours.OrderBy(
                         item => item.HourNumber))
            {
                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection,
                           transaction))
                {
                    command.Parameters.Add(
                        "@ProductId",
                        SqlDbType.Int).Value =
                        hour.ProductId;

                    command.Parameters.Add(
                        "@ProductNameSnapshot",
                        SqlDbType.NVarChar,
                        150).Value =
                        hour.ProductName;

                    command.Parameters.Add(
                        "@RatePerHourSnapshot",
                        SqlDbType.Int).Value =
                        hour.RatePerHour;

                    command.Parameters.Add(
                        "@PlannedStopMinutes",
                        SqlDbType.Int).Value =
                        hour.PlannedStopMinutes;

                    command.Parameters.Add(
                        "@ChangeoverMinutes",
                        SqlDbType.Int).Value =
                        hour.ChangeoverMinutes;

                    command.Parameters.Add(
                        "@TargetQuantity",
                        SqlDbType.Int).Value =
                        hour.TargetQuantity;

                    command.Parameters.Add(
                        "@BoardId",
                        SqlDbType.Int).Value =
                        boardId;

                    command.Parameters.Add(
                        "@HourNumber",
                        SqlDbType.TinyInt).Value =
                        hour.HourNumber;

                    int affectedRows =
                        command.ExecuteNonQuery();

                    if (affectedRows == 0)
                    {
                        throw new InvalidOperationException(
                            "Production hour H" +
                            hour.HourNumber.ToString(
                                CultureInfo.InvariantCulture) +
                            " does not exist.");
                    }
                }
            }
        }

        private static void UpsertChange(
            SqlConnection connection,
            SqlTransaction transaction,
            ProductChangeRequest request,
            ProductSnapshot previousProduct,
            ProductOptionItem newProduct,
            string changedBy)
        {
            const string sql = @"
IF EXISTS
(
    SELECT 1
    FROM dbo.ProductionBoardProductChanges
    WHERE ProductionBoardId = @BoardId
      AND EffectiveHourNumber = @EffectiveHourNumber
)
BEGIN
    UPDATE dbo.ProductionBoardProductChanges
    SET
        PreviousProductId = @PreviousProductId,
        PreviousProductCodeSnapshot = @PreviousProductCodeSnapshot,
        PreviousProductNameSnapshot = @PreviousProductNameSnapshot,
        PreviousRatePerHourSnapshot = @PreviousRatePerHourSnapshot,
        NewProductId = @NewProductId,
        NewProductCodeSnapshot = @NewProductCodeSnapshot,
        NewProductNameSnapshot = @NewProductNameSnapshot,
        NewRatePerHourSnapshot = @NewRatePerHourSnapshot,
        ChangeoverMinutes = @ChangeoverMinutes,
        Reason = @Reason,
        ChangedBy = @ChangedBy,
        ChangedAt = SYSUTCDATETIME()
    WHERE ProductionBoardId = @BoardId
      AND EffectiveHourNumber = @EffectiveHourNumber;
END
ELSE
BEGIN
    INSERT INTO dbo.ProductionBoardProductChanges
    (
        ProductionBoardId,
        EffectiveHourNumber,
        PreviousProductId,
        PreviousProductCodeSnapshot,
        PreviousProductNameSnapshot,
        PreviousRatePerHourSnapshot,
        NewProductId,
        NewProductCodeSnapshot,
        NewProductNameSnapshot,
        NewRatePerHourSnapshot,
        ChangeoverMinutes,
        Reason,
        ChangedBy
    )
    VALUES
    (
        @BoardId,
        @EffectiveHourNumber,
        @PreviousProductId,
        @PreviousProductCodeSnapshot,
        @PreviousProductNameSnapshot,
        @PreviousRatePerHourSnapshot,
        @NewProductId,
        @NewProductCodeSnapshot,
        @NewProductNameSnapshot,
        @NewRatePerHourSnapshot,
        @ChangeoverMinutes,
        @Reason,
        @ChangedBy
    );
END;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    request.BoardId;

                command.Parameters.Add(
                    "@EffectiveHourNumber",
                    SqlDbType.TinyInt).Value =
                    request.EffectiveHourNumber;

                command.Parameters.Add(
                    "@PreviousProductId",
                    SqlDbType.Int).Value =
                    previousProduct.ProductId;

                command.Parameters.Add(
                    "@PreviousProductCodeSnapshot",
                    SqlDbType.NVarChar,
                    50).Value =
                    previousProduct.ProductCode;

                command.Parameters.Add(
                    "@PreviousProductNameSnapshot",
                    SqlDbType.NVarChar,
                    150).Value =
                    previousProduct.ProductName;

                command.Parameters.Add(
                    "@PreviousRatePerHourSnapshot",
                    SqlDbType.Int).Value =
                    previousProduct.RatePerHour;

                command.Parameters.Add(
                    "@NewProductId",
                    SqlDbType.Int).Value =
                    newProduct.Id;

                command.Parameters.Add(
                    "@NewProductCodeSnapshot",
                    SqlDbType.NVarChar,
                    50).Value =
                    newProduct.Code;

                command.Parameters.Add(
                    "@NewProductNameSnapshot",
                    SqlDbType.NVarChar,
                    150).Value =
                    newProduct.Name;

                command.Parameters.Add(
                    "@NewRatePerHourSnapshot",
                    SqlDbType.Int).Value =
                    newProduct.StandardRatePerHour;

                command.Parameters.Add(
                    "@ChangeoverMinutes",
                    SqlDbType.Int).Value =
                    request.ChangeoverMinutes;

                command.Parameters.Add(
                    "@Reason",
                    SqlDbType.NVarChar,
                    500).Value =
                    string.IsNullOrWhiteSpace(
                        request.Reason)
                        ? (object)DBNull.Value
                        : request.Reason.Trim();

                command.Parameters.Add(
                    "@ChangedBy",
                    SqlDbType.NVarChar,
                    256).Value =
                    string.IsNullOrWhiteSpace(
                        changedBy)
                        ? (object)DBNull.Value
                        : changedBy.Trim();

                command.ExecuteNonQuery();
            }
        }

        private static ProductSnapshot ResolveProductBeforeHour(
            BoardInitialProduct initialProduct,
            IDictionary<int, ProductChangeSnapshot> changes,
            int effectiveHourNumber)
        {
            ProductSnapshot current =
                new ProductSnapshot
                {
                    ProductId =
                        initialProduct.ProductId,

                    ProductCode =
                        initialProduct.ProductCode,

                    ProductName =
                        initialProduct.ProductName,

                    RatePerHour =
                        initialProduct.RatePerHour
                };

            if (changes == null)
            {
                return current;
            }

            foreach (KeyValuePair<int, ProductChangeSnapshot> pair
                     in changes.OrderBy(
                         item => item.Key))
            {
                if (pair.Key >= effectiveHourNumber)
                {
                    break;
                }

                ProductChangeSnapshot change =
                    pair.Value;

                current =
                    new ProductSnapshot
                    {
                        ProductId =
                            change.NewProductId,

                        ProductCode =
                            change.NewProductCode,

                        ProductName =
                            change.NewProductName,

                        RatePerHour =
                            change.NewRatePerHour
                    };
            }

            return current;
        }

        private static int GetPlannedStopMinutes(
            int hourNumber)
        {
            if (hourNumber < 1 ||
                hourNumber >
                PlannedStopMinutesByHour.Length)
            {
                return 0;
            }

            return PlannedStopMinutesByHour[
                hourNumber - 1];
        }

        private static void UpdateBoardTimestamp(
            SqlConnection connection,
            SqlTransaction transaction,
            int boardId)
        {
            const string sql = @"
UPDATE dbo.ProductionBoards
SET UpdatedAt = SYSUTCDATETIME()
WHERE Id = @BoardId;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                command.Parameters.Add(
                    "@BoardId",
                    SqlDbType.Int).Value =
                    boardId;

                command.ExecuteNonQuery();
            }
        }

        private sealed class BoardInitialProduct
        {
            public int ProductId { get; set; }

            public string ProductCode { get; set; }

            public string ProductName { get; set; }

            public int RatePerHour { get; set; }
        }

        private sealed class ProductSnapshot
        {
            public int ProductId { get; set; }

            public string ProductCode { get; set; }

            public string ProductName { get; set; }

            public int RatePerHour { get; set; }
        }

        private sealed class ProductChangeSnapshot
        {
            public int EffectiveHourNumber { get; set; }

            public int NewProductId { get; set; }

            public string NewProductCode { get; set; }

            public string NewProductName { get; set; }

            public int NewRatePerHour { get; set; }

            public int ChangeoverMinutes { get; set; }

            public string Reason { get; set; }
        }
    }
}
