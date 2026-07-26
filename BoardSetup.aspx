<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="BoardSetup.aspx.cs"
    Inherits="PFF.BoardSetup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport"
          content="width=device-width, initial-scale=1" />

    <title>Production Board - Board Setup</title>

    <link rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />

    <style type="text/css">
        * {
            box-sizing: border-box;
        }

        html,
        body {
            min-height: 100%;
            margin: 0;
        }

        body {
            padding: 30px 18px;
            font-family: Arial, Helvetica, sans-serif;
            color: #172b4d;
            background: #ffffff;
        }

        .page-wrapper {
            width: 100%;
            max-width: 860px;
            margin: 0 auto;
        }

        .topbar {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 20px;
            margin-bottom: 22px;
            padding: 20px 24px;
            border: 1px solid #dbe8ef;
            border-radius: 18px;
            background: #ffffff;
            box-shadow: 0 10px 30px rgba(48, 94, 120, 0.08);
        }

        .topbar h1 {
            margin: 0;
            color: #184764;
            font-size: 24px;
        }

        .topbar p {
            margin: 7px 0 0;
            color: #667085;
            font-size: 14px;
        }

        .logout-link {
            color: #287fab;
            font-size: 14px;
            font-weight: bold;
            text-decoration: none;
        }

        .logout-link:hover {
            text-decoration: underline;
        }

        .setup-card {
            padding: 28px;
            border: 1px solid #dbe8ef;
            border-radius: 20px;
            background: #ffffff;
            box-shadow: 0 14px 40px rgba(48, 94, 120, 0.09);
        }

        .account-summary {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 14px;
            margin-bottom: 26px;
        }

        .summary-item {
            min-width: 0;
            padding: 15px;
            border-radius: 12px;
            background: #f3f8fb;
        }

        .summary-label {
            display: block;
            margin-bottom: 5px;
            color: #667085;
            font-size: 12px;
            letter-spacing: 0.04em;
            text-transform: uppercase;
        }

        .summary-value {
            display: block;
            overflow: hidden;
            color: #184764;
            font-size: 15px;
            font-weight: bold;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .assignment-box {
            margin-bottom: 24px;
            padding: 18px;
            border: 1px solid #cfe3ef;
            border-radius: 14px;
            background: #f3f9fc;
        }

        .form-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 19px;
        }

        .field {
            min-width: 0;
        }

        .field-full {
            grid-column: 1 / -1;
        }

        .field label {
            display: block;
            margin-bottom: 8px;
            color: #344054;
            font-size: 14px;
            font-weight: bold;
        }

        .form-control {
            width: 100%;
            height: 47px;
            padding: 0 13px;
            border: 1px solid #d0d5dd;
            border-radius: 10px;
            outline: none;
            color: #101828;
            background: #ffffff;
            font-size: 14px;
        }

        .form-control:focus {
            border-color: #4898c2;
            box-shadow: 0 0 0 4px rgba(72, 152, 194, 0.13);
        }

        .continue-button {
            width: 100%;
            min-height: 49px;
            margin-top: 24px;
            border: 0;
            border-radius: 11px;
            cursor: pointer;
            color: #ffffff;
            background: linear-gradient(
                135deg,
                #6aabd2,
                #278bc4
            );
            font-size: 15px;
            font-weight: bold;
        }

        .continue-button:hover {
            filter: brightness(0.97);
        }

        .continue-button:disabled {
            cursor: not-allowed;
            opacity: 0.55;
        }

        .error-message {
            display: block;
            margin-bottom: 20px;
            padding: 12px 14px;
            border: 1px solid #fecdca;
            border-radius: 10px;
            color: #b42318;
            background: #fef3f2;
            font-size: 14px;
        }

        .read-only-note {
            display: block;
            margin-top: 15px;
            padding: 11px 13px;
            border-radius: 9px;
            color: #475467;
            background: #f2f4f7;
            font-size: 13px;
            line-height: 1.45;
        }

        @media (max-width: 680px) {
            body {
                padding: 18px 12px;
            }

            .topbar {
                align-items: flex-start;
                flex-direction: column;
                padding: 18px;
            }

            .account-summary,
            .form-grid {
                grid-template-columns: 1fr;
            }

            .field-full {
                grid-column: auto;
            }

            .setup-card {
                padding: 21px;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <main class="page-wrapper">
            <header class="topbar">
                <div>
                    <h1>Open Production Board</h1>

                    <p>
                        Select the production context before continuing.
                    </p>
                </div>

                <a
                    class="logout-link"
                    href="Logout.aspx">
                    Sign out
                </a>
            </header>

            <section class="setup-card">
                <asp:Label
                    ID="ErrorMessageLabel"
                    runat="server"
                    CssClass="error-message"
                    Visible="false" />

                <div class="account-summary">
                    <div class="summary-item">
                        <span class="summary-label">
                            Signed-in user
                        </span>

                        <asp:Label
                            ID="FullNameLabel"
                            runat="server"
                            CssClass="summary-value" />
                    </div>

                    <div class="summary-item">
                        <span class="summary-label">
                            Role
                        </span>

                        <asp:Label
                            ID="RoleLabel"
                            runat="server"
                            CssClass="summary-value" />
                    </div>
                </div>

                <asp:Panel
                    ID="TeamLeaderAssignmentPanel"
                    runat="server"
                    CssClass="assignment-box"
                    Visible="false">

                    <span class="summary-label">
                        Assigned team
                    </span>

                    <asp:Label
                        ID="AssignedTeamLabel"
                        runat="server"
                        CssClass="summary-value" />
                </asp:Panel>

                <div class="form-grid">
                    <asp:Panel
                        ID="TeamSelectionPanel"
                        runat="server"
                        CssClass="field"
                        Visible="false">

                        <label for="TeamDropDownList">
                            Team
                        </label>

                        <asp:DropDownList
                            ID="TeamDropDownList"
                            runat="server"
                            CssClass="form-control">

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

                    <asp:Panel
                        ID="ShiftSelectionPanel"
                        runat="server"
                        CssClass="field"
                        Visible="true">

                        <label for="ShiftDropDownList">
                            Shift
                        </label>

                        <asp:DropDownList
                            ID="ShiftDropDownList"
                            runat="server"
                            CssClass="form-control">

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
                    </asp:Panel>

                    <div class="field">
                        <label for="ProductionLineDropDownList">
                            Production line
                        </label>

                        <asp:DropDownList
                            ID="ProductionLineDropDownList"
                            runat="server"
                            CssClass="form-control">

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
                            CssClass="form-control">

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

                    <div class="field field-full">
                        <label for="BoardDateTextBox">
                            Board date
                        </label>

                        <asp:TextBox
                            ID="BoardDateTextBox"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Date" />
                    </div>
                </div>

                <asp:Label
                    ID="ReadOnlyNoticeLabel"
                    runat="server"
                    CssClass="read-only-note"
                    Visible="false"
                    Text="Viewer access is read-only. Production modifications are not permitted." />

                <asp:Button
                    ID="ContinueButton"
                    runat="server"
                    Text="Continue to Production Board"
                    CssClass="continue-button"
                    OnClick="ContinueButton_Click" />
            </section>
        </main>
    </form>
    <script src="Scripts/app-language.js"></script>
</body>
</html>
