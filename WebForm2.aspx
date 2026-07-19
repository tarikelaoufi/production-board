<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="PFF.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><br />
            <asp:Label ID="Label1" runat="server" Text="Label1 = "></asp:Label><asp:Label ID="Label2" runat="server" Text="Label 1"></asp:Label> <br />
            <asp:Label ID="Label3" runat="server" Text="Label2 = "></asp:Label> <asp:Label ID="Label4" runat="server" Text="Label 2"></asp:Label><br />
            <asp:Button ID="Button1" runat="server" Text="Switch" OnClick="Button1_Click1" /><br />
            <asp:Button ID="Button2" runat="server" Text="Reset" OnClick="Button2_Click" />
            <br />
        </div>
    </form>
</body>
</html>
