<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="FindBoard.aspx.cs"
    Inherits="PFF.FindBoard"
    ResponseEncoding="utf-8"
    Culture="en-US"
    UICulture="en" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
          content="width=device-width, initial-scale=1" />

    <title>Find Production Board</title>

    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />

    <style type="text/css">
        :root {
            --blue: #1f709d;
            --blue-soft: #edf7fb;
            --ink: #263743;
            --muted: #687b86;
            --line: #d7e3e9;
            --danger: #b42318;
        }

        * {
            box-sizing: border-box;
        }

        html,
        body {
            min-height: 100%;
            margin: 0;
        }

        body {
            font-family: Arial, Helvetica, sans-serif;
            color: var(--ink);
            background: #ffffff;
        }

        button,
        input,
        select {
            font: inherit;
        }

        /* =========================
           SIDE MENU
           ========================= */

        .side {
            position: fixed;
            inset: 0 auto 0 0;
            z-index: 20;
            width: 72px;
            overflow: hidden;
            border-right: 1px solid #dce7ec;
            background: #ffffff;
            box-shadow: 7px 0 24px rgba(30, 75, 99, 0.07);
            transition: width 0.2s;
        }

        .side.open {
            width: 230px;
        }

        .toggle {
            width: 100%;
            height: 72px;
            border: 0;
            cursor: pointer;
            color: var(--blue);
            background: #ffffff;
            font-size: 27px;
        }

        .nav {
            display: flex;
            flex-direction: column;
            gap: 7px;
            padding: 7px 10px;
        }

        .nav a {
            display: flex;
            align-items: center;
            height: 47px;
            padding: 0 14px;
            border-radius: 11px;
            color: #5c7481;
            text-decoration: none;
            white-space: nowrap;
        }

        .nav a:hover,
        .nav a.active {
            color: var(--blue);
            background: var(--blue-soft);
        }

        .nav i {
            width: 32px;
            text-align: center;
        }

        .nav span {
            margin-left: 10px;
            opacity: 0;
            font-size: 14px;
            font-weight: 700;
            transition: opacity 0.15s;
        }

        .side.open .nav span {
            opacity: 1;
        }

        /* =========================
           PAGE
           ========================= */

        .page {
            min-height: 100vh;
            margin-left: 72px;
            padding: 18px 22px 30px;
            transition: margin-left 0.2s;
        }

        .page.open {
            margin-left: 230px;
        }

        .wrap {
            width: 100%;
            max-width: 1450px;
            margin: 0 auto;
        }

        .toolbar {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 16px;
            margin-bottom: 15px;
        }

        .toolbar h1 {
            margin: 0;
            color: #1e506b;
            font-size: 27px;
        }

        .toolbar p {
            margin: 6px 0 0;
            color: var(--muted);
            font-size: 13px;
        }

        .toolbar-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
        }

        .toolbar-link {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            min-height: 40px;
            padding: 0 13px;
            border: 1px solid #d2e0e7;
            border-radius: 9px;
            color: var(--blue);
            background: #ffffff;
            text-decoration: none;
            font-size: 13px;
            font-weight: 700;
        }

        /* =========================
           SEARCH
           ========================= */

        .search-card {
            margin-bottom: 16px;
            padding: 20px;
            border: 1px solid var(--line);
            border-radius: 14px;
            background: #ffffff;
            box-shadow: 0 10px 30px rgba(30, 70, 92, 0.07);
        }

        .access-notice {
            display: block;
            margin-bottom: 16px;
            padding: 11px 13px;
            border: 1px solid #cfe2ec;
            border-radius: 9px;
            color: #35596d;
            background: #f4fafc;
            font-size: 13px;
            line-height: 1.45;
        }

        .error {
            display: block;
            margin-bottom: 15px;
            padding: 11px 13px;
            border: 1px solid #f2c7c3;
            border-radius: 9px;
            color: var(--danger);
            background: #fff5f4;
            font-size: 13px;
            font-weight: 700;
        }

        .filters {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 14px;
        }

        .field {
            min-width: 0;
        }

        .field label {
            display: block;
            margin-bottom: 7px;
            color: #354c59;
            font-size: 12px;
            font-weight: 800;
        }

        .control {
            width: 100%;
            min-height: 43px;
            padding: 8px 11px;
            border: 1px solid #cbd9e0;
            border-radius: 9px;
            outline: none;
            color: #253d4a;
            background: #ffffff;
            font-size: 13px;
        }

        .control:focus {
            border-color: #65a6c8;
            box-shadow: 0 0 0 4px rgba(101, 166, 200, 0.13);
        }

        .assigned-value {
            display: flex;
            align-items: center;
            min-height: 43px;
            padding: 8px 11px;
            border: 1px solid #cfe2ec;
            border-radius: 9px;
            color: var(--blue);
            background: var(--blue-soft);
            font-size: 13px;
            font-weight: 800;
        }

        .search-actions {
            display: flex;
            justify-content: flex-end;
            gap: 9px;
            margin-top: 16px;
        }

        .button {
            min-height: 41px;
            padding: 0 16px;
            border-radius: 9px;
            cursor: pointer;
            font-size: 13px;
            font-weight: 800;
        }

        .button-secondary {
            border: 1px solid #cbd9e0;
            color: #405763;
            background: #ffffff;
        }

        .button-primary {
            border: 1px solid var(--blue);
            color: #ffffff;
            background: var(--blue);
        }

        /* =========================
           RESULTS
           ========================= */

        .results-card {
            overflow: hidden;
            border: 1px solid var(--line);
            border-radius: 14px;
            background: #ffffff;
            box-shadow: 0 10px 30px rgba(30, 70, 92, 0.07);
        }

        .results-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            padding: 14px 17px;
            border-bottom: 1px solid var(--line);
            background: #f8fbfc;
        }

        .results-header h2 {
            margin: 0;
            color: #294b5e;
            font-size: 17px;
        }

        .result-count {
            color: var(--muted);
            font-size: 12px;
            font-weight: 700;
        }

        .table-scroll {
            overflow-x: auto;
        }

        .results-table {
            width: 100%;
            min-width: 930px;
            border-collapse: collapse;
        }

        .results-table th,
        .results-table td {
            padding: 12px 13px;
            border-bottom: 1px solid #e2eaee;
            text-align: left;
            font-size: 13px;
        }

        .results-table th {
            color: #4a626f;
            background: #ffffff;
            font-size: 11px;
            letter-spacing: 0.04em;
            text-transform: uppercase;
        }

        .results-table tbody tr:hover {
            background: #f7fbfd;
        }

        .results-table tbody tr:last-child td {
            border-bottom: 0;
        }

        .open-link {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            min-height: 33px;
            padding: 0 11px;
            border: 1px solid #b9d3e0;
            border-radius: 8px;
            color: var(--blue);
            background: #ffffff;
            text-decoration: none;
            font-size: 12px;
            font-weight: 800;
            white-space: nowrap;
        }

        .empty {
            padding: 40px 20px;
            color: var(--muted);
            text-align: center;
            font-size: 14px;
        }

        /* =========================
           TOASTS
           ========================= */

        .toast-container {
            position: fixed;
            top: 18px;
            right: 18px;
            z-index: 9999;
            display: flex;
            width: min(390px, calc(100vw - 32px));
            flex-direction: column;
            gap: 10px;
            pointer-events: none;
        }

        .toast {
            position: relative;
            display: grid;
            grid-template-columns: 36px minmax(0, 1fr) 28px;
            align-items: start;
            gap: 10px;
            overflow: hidden;
            padding: 13px 13px 15px;
            border: 1px solid transparent;
            border-radius: 12px;
            opacity: 0;
            transform: translateX(24px);
            box-shadow: 0 14px 40px rgba(25, 57, 75, 0.18);
            pointer-events: auto;
            transition:
                opacity 0.22s ease,
                transform 0.22s ease;
        }

        .toast.visible {
            opacity: 1;
            transform: translateX(0);
        }

        .toast.leaving {
            opacity: 0;
            transform: translateX(24px);
        }

        .toast-success {
            border-color: #a9dbb8;
            color: #175f32;
            background: #effaf3;
        }

        .toast-warning {
            border-color: #efd294;
            color: #775817;
            background: #fff9e9;
        }

        .toast-error {
            border-color: #efb9b3;
            color: #a12b22;
            background: #fff3f2;
        }

        .toast-info {
            border-color: #b8d8e8;
            color: #245d79;
            background: #f0f8fc;
        }

        .toast-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 34px;
            height: 34px;
            border-radius: 999px;
            background: rgba(255, 255, 255, 0.72);
            font-size: 16px;
        }

        .toast-content {
            min-width: 0;
            padding-top: 1px;
        }

        .toast-title {
            display: block;
            margin-bottom: 3px;
            font-size: 13px;
            font-weight: 800;
        }

        .toast-message {
            display: block;
            font-size: 12px;
            line-height: 1.45;
        }

        .toast-close {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 28px;
            height: 28px;
            padding: 0;
            border: 0;
            cursor: pointer;
            color: currentColor;
            background: transparent;
            opacity: 0.68;
        }

        .toast-close:hover {
            opacity: 1;
        }

        .toast-progress {
            position: absolute;
            right: 0;
            bottom: 0;
            left: 0;
            height: 3px;
            background: currentColor;
            opacity: 0.4;
            transform-origin: left center;
            animation: toast-progress 4.5s linear forwards;
        }

        @keyframes toast-progress {
            from {
                transform: scaleX(1);
            }

            to {
                transform: scaleX(0);
            }
        }

        @media (max-width: 950px) {
            .filters {
                grid-template-columns: 1fr 1fr;
            }
        }

        @media (max-width: 650px) {
            .page {
                padding: 13px 10px 22px;
            }

            .toolbar {
                flex-direction: column;
            }

            .filters {
                grid-template-columns: 1fr;
            }

            .toast-container {
                top: 10px;
                right: 10px;
                width: calc(100vw - 20px);
            }
        }

        /* Compact mode for a 14-inch laptop at browser zoom 100%. */
        @media (max-width: 1500px), (max-height: 850px) {
            .side {
                width: 58px;
            }

            .side.open {
                width: 200px;
            }

            .toggle {
                height: 54px;
                font-size: 22px;
            }

            .nav {
                gap: 4px;
                padding: 4px 7px;
            }

            .nav a {
                height: 38px;
                padding: 0 9px;
                border-radius: 8px;
            }

            .nav i {
                width: 27px;
                font-size: 15px;
            }

            .nav span {
                margin-left: 7px;
                font-size: 12px;
            }

            .page {
                margin-left: 58px;
                padding: 9px 12px 18px;
            }

            .page.open {
                margin-left: 200px;
            }

            .toolbar {
                align-items: center;
                margin-bottom: 8px;
            }

            .toolbar h1 {
                font-size: 21px;
            }

            .toolbar p {
                display: none;
            }

            .toolbar-link {
                min-height: 32px;
                padding: 0 10px;
                font-size: 11px;
            }

            .search-card {
                padding: 14px;
            }

            .filters {
                gap: 10px;
            }

            .control,
            .assigned-value {
                min-height: 37px;
                padding: 7px 9px;
                font-size: 12px;
            }

            .field label {
                margin-bottom: 5px;
                font-size: 11px;
            }

            .search-actions {
                margin-top: 11px;
            }

            .button {
                min-height: 35px;
                padding: 0 12px;
                font-size: 11px;
            }

            .results-table th,
            .results-table td {
                padding: 9px 10px;
                font-size: 11px;
            }

            .toast-container {
                top: 10px;
                right: 10px;
                width: min(350px, calc(100vw - 20px));
            }

            .toast {
                grid-template-columns: 31px minmax(0, 1fr) 25px;
                gap: 8px;
                padding: 10px 10px 12px;
                border-radius: 10px;
            }

            .toast-icon {
                width: 30px;
                height: 30px;
                font-size: 14px;
            }

            .toast-title {
                font-size: 12px;
            }

            .toast-message {
                font-size: 11px;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div
            id="toastContainer"
            class="toast-container"
            aria-live="polite"
            aria-atomic="true">
        </div>

        <aside id="side" class="side">
            <button type="button"
                    class="toggle"
                    onclick="toggleMenu()"
                    title="Menu">
                <i class="fa-solid fa-bars"></i>
            </button>

            <nav class="nav">
                <a href="PB_page.aspx">
                    <i class="fa-solid fa-table-cells-large"></i>
                    <span>Production Board</span>
                </a>

                <a href="WeeklySyntheses.aspx">
                    <i class="fa-solid fa-calendar-week"></i>
                    <span>Weekly synthesis</span>
                </a>

                <a class="active" href="FindBoard.aspx">
                    <i class="fa-solid fa-magnifying-glass"></i>
                    <span>Find a board</span>
                </a>

                <a href="BoardSetup.aspx">
                    <i class="fa-solid fa-sliders"></i>
                    <span>Change board</span>
                </a>

                <a href="Logout.aspx">
                    <i class="fa-solid fa-right-from-bracket"></i>
                    <span>Sign out</span>
                </a>
            </nav>
        </aside>

        <main id="page" class="page">
            <div class="wrap">
                <header class="toolbar">
                    <div>
                        <h1>Find a Production Board</h1>

                        <p>
                            Search saved boards and open the complete hourly table.
                        </p>
                    </div>

                    <div class="toolbar-actions">
                        <a class="toolbar-link" href="BoardSetup.aspx">
                            <i class="fa-solid fa-plus"></i>
                            Open or create board
                        </a>

                        <a class="toolbar-link" href="Logout.aspx">
                            <i class="fa-solid fa-right-from-bracket"></i>
                            Sign out
                        </a>
                    </div>
                </header>

                <section class="search-card">
                    <asp:Label
                        ID="AccessNoticeLabel"
                        runat="server"
                        CssClass="access-notice" />

                    <asp:Label
                        ID="ErrorMessageLabel"
                        runat="server"
                        CssClass="error"
                        Visible="false" />

                    <div class="filters">
                        <asp:Panel
                            ID="AssignedTeamPanel"
                            runat="server"
                            CssClass="field"
                            Visible="false">

                            <label>Authorized team</label>

                            <asp:Label
                                ID="AssignedTeamLabel"
                                runat="server"
                                CssClass="assigned-value" />
                        </asp:Panel>

                        <asp:Panel
                            ID="TeamFilterPanel"
                            runat="server"
                            CssClass="field">

                            <label for="TeamDropDownList">
                                Team
                            </label>

                            <asp:DropDownList
                                ID="TeamDropDownList"
                                runat="server"
                                CssClass="control">

                                <asp:ListItem
                                    Text="All teams"
                                    Value="" />

                                <asp:ListItem
                                    Text="Team A"
                                    Value="Team A" />

                                <asp:ListItem
                                    Text="Team B"
                                    Value="Team B" />

                                <asp:ListItem
                                    Text="Team C"
                                    Value="Team C" />
                            </asp:DropDownList>
                        </asp:Panel>

                        <div class="field">
                            <label for="ShiftDropDownList">
                                Shift
                            </label>

                            <asp:DropDownList
                                ID="ShiftDropDownList"
                                runat="server"
                                CssClass="control">

                                <asp:ListItem
                                    Text="All shifts"
                                    Value="" />

                                <asp:ListItem
                                    Text="Morning"
                                    Value="Morning" />

                                <asp:ListItem
                                    Text="Afternoon"
                                    Value="Afternoon" />

                                <asp:ListItem
                                    Text="Night"
                                    Value="Night" />
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <label for="ProductionLineDropDownList">
                                Production line
                            </label>

                            <asp:DropDownList
                                ID="ProductionLineDropDownList"
                                runat="server"
                                CssClass="control">

                                <asp:ListItem
                                    Text="All production lines"
                                    Value="" />

                                <asp:ListItem
                                    Text="Production Line 1"
                                    Value="Production Line 1" />

                                <asp:ListItem
                                    Text="Production Line 2"
                                    Value="Production Line 2" />

                                <asp:ListItem
                                    Text="Production Line 3"
                                    Value="Production Line 3" />
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <label for="ProductDropDownList">
                                Product
                            </label>

                            <asp:DropDownList
                                ID="ProductDropDownList"
                                runat="server"
                                CssClass="control">

                                <asp:ListItem
                                    Text="All products"
                                    Value="" />

                                <asp:ListItem
                                    Text="Product 1"
                                    Value="Product 1" />

                                <asp:ListItem
                                    Text="Product 2"
                                    Value="Product 2" />

                                <asp:ListItem
                                    Text="Product 3"
                                    Value="Product 3" />
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <label for="BoardDateTextBox">
                                Board date
                            </label>

                            <asp:TextBox
                                ID="BoardDateTextBox"
                                runat="server"
                                CssClass="control"
                                TextMode="Date" />
                        </div>
                    </div>

                    <div class="search-actions">
                        <asp:Button
                            ID="ClearButton"
                            runat="server"
                            Text="Clear"
                            CssClass="button button-secondary"
                            OnClick="ClearButton_Click" />

                        <asp:Button
                            ID="SearchButton"
                            runat="server"
                            Text="Search boards"
                            CssClass="button button-primary"
                            OnClick="SearchButton_Click" />
                    </div>
                </section>

                <section class="results-card">
                    <div class="results-header">
                        <h2>Saved boards</h2>

                        <asp:Label
                            ID="ResultCountLabel"
                            runat="server"
                            CssClass="result-count" />
                    </div>

                    <asp:Panel
                        ID="EmptyResultsPanel"
                        runat="server"
                        CssClass="empty"
                        Visible="false">

                        No production board matches the selected filters.
                    </asp:Panel>

                    <asp:Panel
                        ID="ResultsTablePanel"
                        runat="server"
                        CssClass="table-scroll"
                        Visible="false">

                        <table class="results-table">
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Team</th>
                                    <th>Shift</th>
                                    <th>Line</th>
                                    <th>Product</th>
                                    <th>Last update</th>
                                    <th></th>
                                </tr>
                            </thead>

                            <tbody>
                                <asp:Repeater
                                    ID="ResultsRepeater"
                                    runat="server">

                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval(
                                                        "BoardDate",
                                                        "{0:yyyy-MM-dd}") %>
                                            </td>

                                            <td>
                                                <%# Eval("TeamName") %>
                                            </td>

                                            <td>
                                                <%# Eval("ShiftName") %>
                                            </td>

                                            <td>
                                                <%# Eval("LineName") %>
                                            </td>

                                            <td>
                                                <%# Eval("ProductName") %>
                                            </td>

                                            <td>
                                                <%# FormatLastUpdate(
                                                        Eval("UpdatedAt"),
                                                        Eval("CreatedAt")) %>
                                            </td>

                                            <td>
                                                <a
                                                    class="open-link"
                                                    href='<%#
                                                        "PB_page.aspx?boardId=" +
                                                        Eval("Id") %>'>

                                                    <i class="fa-solid fa-arrow-up-right-from-square"></i>
                                                    Open board
                                                </a>
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
        function toggleMenu() {
            document
                .getElementById("side")
                .classList
                .toggle("open");

            document
                .getElementById("page")
                .classList
                .toggle("open");
        }

        function showToast(
            message,
            type,
            title) {

            var container =
                document.getElementById(
                    "toastContainer"
                );

            if (!container) {
                return;
            }

            var allowedTypes = {
                success: true,
                warning: true,
                error: true,
                info: true
            };

            var cleanType =
                allowedTypes[type]
                    ? type
                    : "info";

            var toastTitles = {
                success: "Search completed",
                warning: "No board available",
                error: "Search error",
                info: "Information"
            };

            var toastIcons = {
                success: "fa-solid fa-circle-check",
                warning: "fa-solid fa-triangle-exclamation",
                error: "fa-solid fa-circle-xmark",
                info: "fa-solid fa-circle-info"
            };

            var toast =
                document.createElement("div");

            toast.className =
                "toast toast-" + cleanType;

            toast.setAttribute(
                "role",
                cleanType === "error"
                    ? "alert"
                    : "status"
            );

            var icon =
                document.createElement("span");

            icon.className =
                "toast-icon";

            var iconElement =
                document.createElement("i");

            iconElement.className =
                toastIcons[cleanType];

            icon.appendChild(iconElement);

            var content =
                document.createElement("span");

            content.className =
                "toast-content";

            var titleElement =
                document.createElement("span");

            titleElement.className =
                "toast-title";

            titleElement.textContent =
                title ||
                toastTitles[cleanType];

            var messageElement =
                document.createElement("span");

            messageElement.className =
                "toast-message";

            messageElement.textContent =
                message || "";

            content.appendChild(
                titleElement
            );

            content.appendChild(
                messageElement
            );

            var closeButton =
                document.createElement("button");

            closeButton.type =
                "button";

            closeButton.className =
                "toast-close";

            closeButton.setAttribute(
                "aria-label",
                "Close notification"
            );

            closeButton.innerHTML =
                '<i class="fa-solid fa-xmark"></i>';

            var progress =
                document.createElement("span");

            progress.className =
                "toast-progress";

            toast.appendChild(icon);
            toast.appendChild(content);
            toast.appendChild(closeButton);
            toast.appendChild(progress);

            container.appendChild(toast);

            window.requestAnimationFrame(
                function () {
                    toast.classList.add(
                        "visible"
                    );
                }
            );

            var removed = false;

            function removeToast() {
                if (removed) {
                    return;
                }

                removed = true;

                toast.classList.add(
                    "leaving"
                );

                window.setTimeout(
                    function () {
                        if (toast.parentNode) {
                            toast.parentNode
                                .removeChild(toast);
                        }
                    },
                    230
                );
            }

            closeButton.addEventListener(
                "click",
                removeToast
            );

            window.setTimeout(
                removeToast,
                4500
            );
        }
    </script>
    <script src="Scripts/app-language.js"></script>
</body>
</html>
