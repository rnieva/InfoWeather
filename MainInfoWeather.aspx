<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainInfoWeather.aspx.cs" Inherits="InfoWeather.MainInfoWeather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 416px">
    <form id="form1" runat="server">
    <div style="height: 453px">
    
        Info Weather<br />
        <br />
    
        <asp:Label ID="Label1" runat="server" Text="Source"></asp:Label>
        :<br />
        <asp:DropDownList ID="DropDownList1SelectSource" runat="server">
        </asp:DropDownList>
        <br />
        Select Country:<br />
        <asp:DropDownList ID="DropDownList1SelectCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CityDropList1">
        </asp:DropDownList>
        <br />
        Select City:<br />
        <asp:DropDownList ID="DropDownList2SelectCity" runat="server" AutoPostBack="True">
        </asp:DropDownList>
        <br />
        <br />
        <asp:Button ID="Button1GetInfoWeather" runat="server" OnClick="GetInfoWeather" Text="Get Info Weather" />
        <br />
        <br />
        TempTest:<asp:Label ID="LabelTempTest" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
