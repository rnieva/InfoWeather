<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainInfoWeather.aspx.cs" Inherits="InfoWeather.MainInfoWeather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 336px">
    <form id="form1" runat="server">
    <div style="height: 368px">
    
        <asp:Label ID="Label1" runat="server" Text="Source"></asp:Label>
        :<br />
        <asp:DropDownList ID="DropDownList1SelectSource" runat="server">
        </asp:DropDownList>
        <br />
    
    </div>
    </form>
</body>
</html>
