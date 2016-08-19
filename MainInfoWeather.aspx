<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainInfoWeather.aspx.cs" Inherits="InfoWeather.MainInfoWeather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 664px">
    <form id="form1" runat="server">
    <div style="height: 685px">
    
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
        <asp:Label ID="LabelLocation" runat="server" Text="Location"></asp:Label>
        <br />
        <asp:Label ID="LabelTime" runat="server" Text="Time"></asp:Label>
        <br />
        <asp:Label ID="LabelWind" runat="server" Text="Wind"></asp:Label>
        <br />
        <asp:Label ID="LabelVisibility" runat="server" Text="Visibility"></asp:Label>
        <br />
        <asp:Label ID="LabelSkyConditions" runat="server" Text="SkyConditions"></asp:Label>
        <br />
        <asp:Label ID="LabelTemperature" runat="server" Text="Temperature"></asp:Label>
        <br />
        <asp:Label ID="LabelDewPoint" runat="server" Text="DewPoint"></asp:Label>
        <br />
        <asp:Label ID="LabelRelativeHumidity" runat="server" Text="RelativeHumidity"></asp:Label>
        <br />
        <asp:Label ID="LabelPressure" runat="server" Text="Pressure"></asp:Label>
        <br />
        <br />
        <asp:Label ID="LabelTest" runat="server" Text="LabelTest"></asp:Label>
        <br />
        <br />
        <asp:DropDownList ID="DropDownListParameterToCompa" runat="server">
            <asp:ListItem>Temperature</asp:ListItem>
            <asp:ListItem>Wind</asp:ListItem>
            <asp:ListItem>DewPoint</asp:ListItem>
            <asp:ListItem>RelativeHumidity</asp:ListItem>
            <asp:ListItem>Pressure</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="ButtonBestCity" runat="server" OnClick="ButtonBestCity_Click" Text="Best City" />
    
        <br />
    
        <br />
        <asp:Label ID="LabelBestCityTemp" runat="server" Text="Best city"></asp:Label>
    
    </div>
    </form>
</body>
</html>
