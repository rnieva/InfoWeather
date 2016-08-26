<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainInfoWeather.aspx.cs" Inherits="InfoWeather.MainInfoWeather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 664px">
    <form id="form1" runat="server">
    <div style="height: 685px">
        Info Weather
        
        <asp:Table ID="Table1" 
            runat="server" 
            Font-Size="" 
            Width="786px" 
            Font-Names="Times New Roman"
            BackColor=""
            BorderColor="black"
            BorderWidth="2"
            ForeColor=""
            CellPadding="5"
            CellSpacing="2"
            >
            <asp:TableRow 
                ID="TableRow1" 
                runat="server" 
                BackColor=""
                >
                <asp:TableCell>Select Source Weather:  
                        <asp:DropDownList ID="DropDownList1SelectSource" runat="server">
                        </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow2" 
                runat="server" 
                BackColor=""
                >
                <asp:TableCell> Select Country:
                                <asp:DropDownList ID="DropDownList1SelectCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="CityDropList1">
                                </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow3" 
                runat="server" 
                BackColor=""
                align=""
                >
                <asp:TableCell> Select City: 
                        <asp:DropDownList ID="DropDownList2SelectCity" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow4" 
                runat="server" 
                BackColor=""
                align="center"
                >
                <asp:TableCell>
                             <asp:Button ID="Button1GetInfoWeather" runat="server" OnClick="GetInfoWeather" Text="Get Info Weather" />
                </asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow5" 
                runat="server" 
                BackColor=""
                align=""
                >
                <asp:TableCell>
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
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Image ID="ImgSkyConditions" runat="server" style="width:50px;height:50px;" /></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow6" 
                runat="server" 
                BackColor=""
                align=""
                >
                <asp:TableCell>
                            <asp:Label ID="LabelTest" runat="server" Text="LabelTest"></asp:Label></asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow7" 
                runat="server" 
                BackColor=""
                >
                <asp:TableCell>
                    <asp:DropDownList ID="DropDownListParameterToCompa" runat="server">
                        <asp:ListItem>Temperature</asp:ListItem>
                        <asp:ListItem>Wind</asp:ListItem>
                        <asp:ListItem>Visibility</asp:ListItem>
                        <asp:ListItem>DewPoint</asp:ListItem>
                        <asp:ListItem>RelativeHumidity</asp:ListItem>
                        <asp:ListItem>Pressure</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="ButtonBestCity" runat="server" OnClick="ButtonBestCity_Click" Text="Best City" />
                    <br />
                    <asp:Label ID="LabelBestCityTemp" runat="server" Text="Best city"></asp:Label>
                </asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow 
                ID="TableRow8" 
                runat="server" 
                BackColor=""
                >
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell></asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow 
                runat="server" 
                BackColor=""
                >
                <asp:TableCell 
                    ColumnSpan="15" 
                    HorizontalAlign="Right"
                    Font-Italic="true"
                    Font-Size="small" 
                    >
                    Info Weather v1.0
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>

    </div>
    </form>
</body>
</html>
