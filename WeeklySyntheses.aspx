<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="WeeklySyntheses.aspx.cs"
    Inherits="PFF.WeeklySyntheses" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Weekly Synthesis</title>

    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />

    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.3/dist/chart.umd.min.js"></script>

    <style type="text/css">
        :root {
            --blue: #1f709d;
            --blue-soft: #edf7fb;
            --ink: #263743;
            --muted: #687b86;
            --line: #d7e3e9;
        }

        * { box-sizing: border-box; }
        html, body { min-height: 100%; margin: 0; }
        body { font-family: Arial, Helvetica, sans-serif; color: var(--ink); background: #fff; }
        button, input, select { font: inherit; }

        .side {
            position: fixed;
            inset: 0 auto 0 0;
            z-index: 20;
            width: 72px;
            overflow: hidden;
            border-right: 1px solid #dce7ec;
            background: #fff;
            box-shadow: 7px 0 24px rgba(30,75,99,.07);
            transition: width .2s;
        }

        .side.open { width: 230px; }
        .toggle { width: 100%; height: 72px; border: 0; cursor: pointer; color: var(--blue); background: #fff; font-size: 27px; }
        .nav { display: flex; flex-direction: column; gap: 7px; padding: 7px 10px; }
        .nav a { display: flex; align-items: center; height: 47px; padding: 0 14px; border-radius: 11px; color: #5c7481; text-decoration: none; white-space: nowrap; }
        .nav a:hover, .nav a.active { color: var(--blue); background: var(--blue-soft); }
        .nav i { width: 32px; text-align: center; }
        .nav span { margin-left: 10px; opacity: 0; font-size: 14px; font-weight: 700; transition: opacity .15s; }
        .side.open .nav span { opacity: 1; }

        .page { min-height: 100vh; margin-left: 72px; padding: 16px 20px 30px; transition: margin-left .2s; }
        .page.open { margin-left: 230px; }
        .wrap { width: 100%; max-width: 1550px; margin: 0 auto; }

        .toolbar { display: flex; align-items: flex-start; justify-content: space-between; gap: 16px; margin-bottom: 14px; }
        .toolbar h1 { margin: 0; color: #1e506b; font-size: 27px; }
        .toolbar p { margin: 6px 0 0; color: var(--muted); font-size: 13px; }
        .toolbar-actions { display: flex; flex-wrap: wrap; gap: 8px; }
        .toolbar-link { display: inline-flex; align-items: center; gap: 7px; min-height: 39px; padding: 0 13px; border: 1px solid #d2e0e7; border-radius: 9px; color: var(--blue); background: #fff; text-decoration: none; font-size: 13px; font-weight: 700; }

        .filter-card, .chart-card, .table-card, .kpi-card {
            border: 1px solid var(--line);
            border-radius: 14px;
            background: #fff;
            box-shadow: 0 10px 30px rgba(30,70,92,.07);
        }

        .filter-card { margin-bottom: 14px; padding: 16px; }
        .filter-grid { display: grid; grid-template-columns: repeat(4, minmax(0,1fr)) auto; align-items: end; gap: 11px; }
        .field label { display: block; margin-bottom: 6px; color: #405966; font-size: 11px; font-weight: 800; }
        .control { width: 100%; min-height: 39px; padding: 7px 10px; border: 1px solid #cad9e1; border-radius: 8px; outline: none; color: #263f4b; background: #fff; font-size: 12px; }
        .control:focus { border-color: #68a8c9; box-shadow: 0 0 0 4px rgba(104,168,201,.13); }
        .filter-button { min-height: 39px; padding: 0 15px; border: 1px solid var(--blue); border-radius: 8px; cursor: pointer; color: #fff; background: var(--blue); font-size: 12px; font-weight: 800; }

        .message { display: block; margin-bottom: 13px; padding: 10px 12px; border: 1px solid #efd0cc; border-radius: 9px; color: #a7372f; background: #fff5f4; font-size: 12px; font-weight: 700; }

        .kpi-grid { display: grid; grid-template-columns: repeat(5, minmax(0,1fr)); gap: 11px; margin-bottom: 14px; }
        .kpi-card { min-width: 0; padding: 14px; }
        .kpi-label { display: block; margin-bottom: 7px; color: #73848e; font-size: 10px; font-weight: 800; letter-spacing: .04em; text-transform: uppercase; }
        .kpi-value { display: block; overflow: hidden; color: #1c6289; font-size: 22px; font-weight: 800; text-overflow: ellipsis; white-space: nowrap; }
        .kpi-subtitle { display: block; margin-top: 4px; color: #83919a; font-size: 10px; }

        .charts-grid { display: grid; grid-template-columns: 1.25fr 1fr 1fr; gap: 12px; margin-bottom: 14px; }
        .chart-card { min-width: 0; padding: 14px; }
        .chart-title { margin: 0 0 11px; color: #335669; font-size: 14px; }
        .chart-container { position: relative; height: 260px; }

        .table-card { overflow: hidden; }
        .table-header { display: flex; align-items: center; justify-content: space-between; gap: 12px; padding: 13px 16px; border-bottom: 1px solid var(--line); background: #f8fbfc; }
        .table-header h2 { margin: 0; color: #325266; font-size: 16px; }
        .week-range { color: #6f808a; font-size: 11px; font-weight: 700; }
        .table-scroll { overflow-x: auto; }
        table { width: 100%; min-width: 850px; border-collapse: collapse; }
        th, td { padding: 10px 12px; border-bottom: 1px solid #e1eaee; text-align: left; font-size: 12px; }
        th { color: #526975; background: #fff; font-size: 10px; letter-spacing: .04em; text-transform: uppercase; }
        tbody tr:hover { background: #f7fbfd; }
        tbody tr:last-child td { border-bottom: 0; }
        .efficiency-pill { display: inline-flex; align-items: center; min-height: 25px; padding: 0 8px; border-radius: 999px; color: #24643a; background: #edf8f0; font-size: 10px; font-weight: 800; }
        .empty { padding: 36px 18px; color: #70818b; text-align: center; font-size: 13px; }

        @media (max-width: 1180px) {
            .filter-grid { grid-template-columns: repeat(2, minmax(0,1fr)); }
            .charts-grid { grid-template-columns: 1fr 1fr; }
            .chart-card:first-child { grid-column: 1 / -1; }
            .kpi-grid { grid-template-columns: repeat(3, minmax(0,1fr)); }
        }

        @media (max-width: 720px) {
            .page { padding: 12px 9px 22px; }
            .toolbar { flex-direction: column; }
            .filter-grid, .charts-grid, .kpi-grid { grid-template-columns: 1fr; }
            .chart-card:first-child { grid-column: auto; }
        }

        @media (max-width:1500px), (max-height:850px) {
            .side { width: 58px; }
            .side.open { width: 200px; }
            .toggle { height: 54px; font-size: 22px; }
            .nav { gap: 4px; padding: 4px 7px; }
            .nav a { height: 38px; padding: 0 9px; border-radius: 8px; }
            .nav i { width: 27px; font-size: 15px; }
            .nav span { margin-left: 7px; font-size: 12px; }
            .page { margin-left: 58px; padding: 8px 11px 18px; }
            .page.open { margin-left: 200px; }
            .toolbar { align-items: center; margin-bottom: 8px; }
            .toolbar h1 { font-size: 21px; }
            .toolbar p { display: none; }
            .toolbar-link { min-height: 31px; padding: 0 9px; font-size: 11px; }
            .filter-card { padding: 11px; }
            .control, .filter-button { min-height: 34px; font-size: 11px; }
            .kpi-card { padding: 10px; }
            .kpi-value { font-size: 18px; }
            .chart-container { height: 210px; }
            th, td { padding: 8px 9px; font-size: 10px; }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:HiddenField
            ID="ChartDataHiddenField"
            runat="server"
            ClientIDMode="Static" />

        <aside id="side" class="side">
            <button type="button" class="toggle" onclick="toggleMenu()" title="Menu">
                <i class="fa-solid fa-bars"></i>
            </button>

            <nav class="nav">
                <a href="PB_page.aspx">
                    <i class="fa-solid fa-table-cells-large"></i><span>Production Board</span>
                </a>

                <a class="active" href="WeeklySyntheses.aspx">
                    <i class="fa-solid fa-chart-column"></i><span>Weekly Synthesis</span>
                </a>

                <a href="FindBoard.aspx">
                    <i class="fa-solid fa-magnifying-glass"></i><span>Find a board</span>
                </a>

                <a href="BoardSetup.aspx">
                    <i class="fa-solid fa-sliders"></i><span>Change board</span>
                </a>

                <a href="Logout.aspx">
                    <i class="fa-solid fa-right-from-bracket"></i><span>Sign out</span>
                </a>
            </nav>
        </aside>

        <main id="page" class="page">
            <div class="wrap">
                <header class="toolbar">
                    <div>
                        <h1>Weekly Synthesis</h1>
                        <p>Production, target, scrap and efficiency by day and team.</p>
                    </div>

                    <div class="toolbar-actions">
                        <a class="toolbar-link" href="PB_page.aspx">
                            <i class="fa-solid fa-arrow-left"></i>
                            Production Board
                        </a>
                    </div>
                </header>

                <asp:Label
                    ID="ErrorMessageLabel"
                    runat="server"
                    CssClass="message"
                    Visible="false" />

                <section class="filter-card">
                    <div class="filter-grid">
                        <div class="field">
                            <label for="WeekDateTextBox">Week containing</label>
                            <asp:TextBox
                                ID="WeekDateTextBox"
                                runat="server"
                                TextMode="Date"
                                CssClass="control" />
                        </div>

                        <div class="field">
                            <label for="TeamDropDownList">Team</label>
                            <asp:DropDownList
                                ID="TeamDropDownList"
                                runat="server"
                                CssClass="control" />
                        </div>

                        <div class="field">
                            <label for="ProductionLineDropDownList">Production line</label>
                            <asp:DropDownList
                                ID="ProductionLineDropDownList"
                                runat="server"
                                CssClass="control" />
                        </div>

                        <div class="field">
                            <label for="ProductDropDownList">Product</label>
                            <asp:DropDownList
                                ID="ProductDropDownList"
                                runat="server"
                                CssClass="control" />
                        </div>

                        <asp:Button
                            ID="ApplyFilterButton"
                            runat="server"
                            Text="Apply filters"
                            CssClass="filter-button"
                            OnClick="ApplyFilterButton_Click" />
                    </div>
                </section>

                <section class="kpi-grid">
                    <div class="kpi-card">
                        <span class="kpi-label">Best team</span>
                        <asp:Label ID="BestTeamLabel" runat="server" CssClass="kpi-value" Text="—" />
                        <span class="kpi-subtitle">Highest actual production</span>
                    </div>

                    <div class="kpi-card">
                        <span class="kpi-label">Target</span>
                        <asp:Label ID="TotalTargetLabel" runat="server" CssClass="kpi-value" Text="0" />
                        <span class="kpi-subtitle">Weekly target</span>
                    </div>

                    <div class="kpi-card">
                        <span class="kpi-label">Actual</span>
                        <asp:Label ID="TotalActualLabel" runat="server" CssClass="kpi-value" Text="0" />
                        <span class="kpi-subtitle">Weekly production</span>
                    </div>

                    <div class="kpi-card">
                        <span class="kpi-label">Scrap</span>
                        <asp:Label ID="TotalScrapLabel" runat="server" CssClass="kpi-value" Text="0" />
                        <span class="kpi-subtitle">Weekly scrap</span>
                    </div>

                    <div class="kpi-card">
                        <span class="kpi-label">Efficiency</span>
                        <asp:Label ID="EfficiencyLabel" runat="server" CssClass="kpi-value" Text="0%" />
                        <span class="kpi-subtitle">Actual compared with target</span>
                    </div>
                </section>

                <section class="charts-grid">
                    <article class="chart-card">
                        <h2 class="chart-title">Daily target vs actual</h2>
                        <div class="chart-container">
                            <canvas id="dailyProductionChart"></canvas>
                        </div>
                    </article>

                    <article class="chart-card">
                        <h2 class="chart-title">Production by team</h2>
                        <div class="chart-container">
                            <canvas id="teamProductionChart"></canvas>
                        </div>
                    </article>

                    <article class="chart-card">
                        <h2 class="chart-title">Scrap distribution</h2>
                        <div class="chart-container">
                            <canvas id="scrapChart"></canvas>
                        </div>
                    </article>
                </section>

                <section class="table-card">
                    <div class="table-header">
                        <h2>Weekly details</h2>
                        <asp:Label ID="WeekRangeLabel" runat="server" CssClass="week-range" />
                    </div>

                    <asp:Panel
                        ID="EmptyResultsPanel"
                        runat="server"
                        CssClass="empty"
                        Visible="false">
                        No production data is available for the selected week and filters.
                    </asp:Panel>

                    <asp:Panel
                        ID="ResultsTablePanel"
                        runat="server"
                        CssClass="table-scroll"
                        Visible="false">
                        <table>
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Day</th>
                                    <th>Team</th>
                                    <th>Target</th>
                                    <th>Actual</th>
                                    <th>Scrap</th>
                                    <th>Efficiency</th>
                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater ID="WeeklyResultsRepeater" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("BoardDate", "{0:yyyy-MM-dd}") %></td>
                                            <td><%# FormatDayName(Eval("BoardDate")) %></td>
                                            <td><%# Eval("TeamName") %></td>
                                            <td><%# Eval("TargetQuantity") %></td>
                                            <td><%# Eval("ActualQuantity") %></td>
                                            <td><%# Eval("ScrapQuantity") %></td>
                                            <td>
                                                <span class="efficiency-pill">
                                                    <%# Eval("EfficiencyPercent", "{0:0.0}%") %>
                                                </span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </asp:Panel>
                </section>
            </div>
        </main>
    </form>

    <script type="text/javascript">
        var dailyProductionChartInstance = null;
        var teamProductionChartInstance = null;
        var scrapChartInstance = null;

        function toggleMenu() {
            document.getElementById("side").classList.toggle("open");
            document.getElementById("page").classList.toggle("open");
        }

        function getWeeklyLanguage() {
            if (window.AppLanguage) {
                return window.AppLanguage.getLanguage();
            }

            try {
                return window.localStorage.getItem(
                    "productionBoardLanguage"
                ) === "fr"
                    ? "fr"
                    : "en";
            } catch (error) {
                return "en";
            }
        }

        function weeklyText(
            englishText,
            frenchText) {

            return getWeeklyLanguage() === "fr"
                ? frenchText
                : englishText;
        }

        function translateDayLabel(label) {
            var cleanLabel =
                String(label || "").trim();

            if (getWeeklyLanguage() !== "fr") {
                return cleanLabel;
            }

            var days = {
                "Monday": "Lundi",
                "Tuesday": "Mardi",
                "Wednesday": "Mercredi",
                "Thursday": "Jeudi",
                "Friday": "Vendredi",
                "Saturday": "Samedi",
                "Sunday": "Dimanche",
                "Mon": "Lun",
                "Tue": "Mar",
                "Wed": "Mer",
                "Thu": "Jeu",
                "Fri": "Ven",
                "Sat": "Sam",
                "Sun": "Dim"
            };

            return days[cleanLabel] ||
                cleanLabel;
        }

        function renderWeeklyCharts() {
            var dataElement = document.getElementById("ChartDataHiddenField");

            if (!dataElement || !dataElement.value || typeof Chart === "undefined") {
                return;
            }

            var data;

            try {
                data = JSON.parse(dataElement.value);
            } catch (error) {
                return;
            }

            destroyChart(dailyProductionChartInstance);
            destroyChart(teamProductionChartInstance);
            destroyChart(scrapChartInstance);

            dailyProductionChartInstance = new Chart(
                document.getElementById("dailyProductionChart"),
                {
                    type: "line",
                    data: {
                        labels: data.dayLabels.map(
                            translateDayLabel
                        ),
                        datasets: [
                            {
                                label: weeklyText(
                                    "Target",
                                    "Objectif"
                                ),
                                data: data.dailyTarget,
                                tension: 0.25,
                                borderWidth: 2
                            },
                            {
                                label: weeklyText(
                                    "Actual",
                                    "Réel"
                                ),
                                data: data.dailyActual,
                                tension: 0.25,
                                borderWidth: 2
                            }
                        ]
                    },
                    options: createCartesianOptions()
                }
            );

            teamProductionChartInstance = new Chart(
                document.getElementById("teamProductionChart"),
                {
                    type: "bar",
                    data: {
                        labels: data.teamLabels,
                        datasets: [
                            {
                                label: weeklyText(
                                    "Target",
                                    "Objectif"
                                ),
                                data: data.teamTarget,
                                borderWidth: 1
                            },
                            {
                                label: weeklyText(
                                    "Actual",
                                    "Réel"
                                ),
                                data: data.teamActual,
                                borderWidth: 1
                            }
                        ]
                    },
                    options: createCartesianOptions()
                }
            );

            scrapChartInstance = new Chart(
                document.getElementById("scrapChart"),
                {
                    type: "doughnut",
                    data: {
                        labels: data.teamLabels,
                        datasets: [
                            {
                                label: weeklyText(
                                    "Scrap",
                                    "Rebut"
                                ),
                                data: data.teamScrap,
                                borderWidth: 1
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                position: "bottom"
                            }
                        }
                    }
                }
            );
        }

        function createCartesianOptions() {
            return {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    mode: "index",
                    intersect: false
                },
                plugins: {
                    legend: {
                        position: "bottom"
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            };
        }

        function destroyChart(chart) {
            if (chart && typeof chart.destroy === "function") {
                chart.destroy();
            }
        }

        document.addEventListener(
            "DOMContentLoaded",
            renderWeeklyCharts
        );

        window.addEventListener(
            "appLanguageChanged",
            renderWeeklyCharts
        );
    </script>
    <script src="Scripts/app-language.js"></script>
</body>
</html>
