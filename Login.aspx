<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="PFF.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />

    <meta
        name="viewport"
        content="width=device-width, initial-scale=1" />

    <title>Production Board - Login</title>

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
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 24px;
            font-family: Arial, Helvetica, sans-serif;
            color: #172b4d;
            background:
                radial-gradient(
                    circle at top left,
                    rgba(255, 255, 255, 0.35),
                    transparent 38%
                ),
                linear-gradient(
                    145deg,
                    #6aabd2,
                    #278bc4
                );
        }

        .login-wrapper {
            width: 100%;
            max-width: 430px;
        }

        .brand {
            margin-bottom: 24px;
            text-align: center;
            color: #ffffff;
        }

        .brand-mark {
            display: flex;
            align-items: center;
            justify-content: center;
            width: 68px;
            height: 68px;
            margin: 0 auto 14px;
            border: 1px solid rgba(255, 255, 255, 0.55);
            border-radius: 20px;
            background: rgba(255, 255, 255, 0.18);
            font-size: 30px;
            font-weight: bold;
        }

        .brand h1 {
            margin: 0;
            font-size: 28px;
        }

        .brand p {
            margin: 8px 0 0;
            color: rgba(255, 255, 255, 0.86);
            font-size: 14px;
        }

        .login-card {
            padding: 32px;
            border: 1px solid rgba(255, 255, 255, 0.75);
            border-radius: 22px;
            background: rgba(255, 255, 255, 0.96);
            box-shadow: 0 24px 65px rgba(22, 72, 102, 0.28);
        }

        .login-card h2 {
            margin: 0;
            color: #173b53;
            font-size: 24px;
        }

        .subtitle {
            margin: 8px 0 25px;
            color: #667085;
            font-size: 14px;
            line-height: 1.5;
        }

        .field {
            margin-bottom: 19px;
        }

        .field label {
            display: block;
            margin-bottom: 8px;
            color: #344054;
            font-size: 14px;
            font-weight: bold;
        }

        .form-input {
            width: 100%;
            height: 48px;
            padding: 0 14px;
            border: 1px solid #d0d5dd;
            border-radius: 11px;
            outline: none;
            color: #101828;
            background: #ffffff;
            font-size: 15px;
        }

        .form-input:focus {
            border-color: #4599c8;
            box-shadow: 0 0 0 4px rgba(69, 153, 200, 0.15);
        }

        .remember-row {
            margin: 2px 0 21px;
            color: #475467;
            font-size: 14px;
        }

        .remember-row input {
            width: 16px;
            height: 16px;
            margin-right: 7px;
            vertical-align: middle;
        }

        .login-button {
            width: 100%;
            min-height: 48px;
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
            box-shadow: 0 12px 24px rgba(39, 139, 196, 0.25);
        }

        .login-button:hover {
            background: linear-gradient(
                135deg,
                #5f9fc6,
                #197eb8
            );
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
            line-height: 1.45;
        }

        .security-note {
            margin-top: 22px;
            padding-top: 18px;
            border-top: 1px solid #eaecf0;
            color: #667085;
            font-size: 12px;
            line-height: 1.55;
            text-align: center;
        }

        @media (max-width: 520px) {
            body {
                padding: 16px;
            }

            .login-card {
                padding: 25px 20px;
            }
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <main class="login-wrapper">
            <header class="brand">
                <div class="brand-mark">PB</div>

                <h1>Production Board</h1>

                <p>Private company production platform</p>
            </header>

            <section class="login-card">
                <h2>Sign in</h2>

                <p class="subtitle">
                    Use the account provided by the platform administrator.
                </p>

                <asp:Label
                    ID="ErrorMessageLabel"
                    runat="server"
                    CssClass="error-message"
                    Visible="false" />

                <div class="field">
                    <label for="UsernameTextBox">
                        Username
                    </label>

                    <asp:TextBox
                        ID="UsernameTextBox"
                        runat="server"
                        CssClass="form-input"
                        MaxLength="80"
                        autocomplete="username"
                        placeholder="Enter your username" />
                </div>

                <div class="field">
                    <label for="PasswordTextBox">
                        Password
                    </label>

                    <asp:TextBox
                        ID="PasswordTextBox"
                        runat="server"
                        CssClass="form-input"
                        TextMode="Password"
                        MaxLength="200"
                        autocomplete="current-password"
                        placeholder="Enter your password" />
                </div>

                <div class="remember-row">
                    <asp:CheckBox
                        ID="RememberMeCheckBox"
                        runat="server"
                        Text="Keep me signed in" />
                </div>

                <asp:Button
                    ID="LoginButton"
                    runat="server"
                    Text="Sign in"
                    CssClass="login-button"
                    OnClick="LoginButton_Click" />

                <div class="security-note">
                    Access is limited to authorized company employees.
                    Login attempts and production changes may be audited.
                </div>
            </section>
        </main>
    </form>
</body>
</html>