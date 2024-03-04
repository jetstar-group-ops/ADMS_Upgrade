<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_TextReportPrint.aspx.vb" Inherits="WNB_Admin.ADMS_TextReportPrint"
    Title="Text Report Print" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptMgr" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:HiddenField ID="HdnExportFileName" runat="server" />
        <table id="TblTitle" style="width: 100%">
            <tr bgcolor="#DADADA">
                <td align="center">
                    <asp:Label runat="server" ID="LblTitle" Text="Text Report Print" Font-Bold="True"
                        Font-Size="X-Large"></asp:Label>
                </td>
            </tr>
        </table>
        </br>
        <table id="Table1" style="width: 100%">
            <tr>
                <td style="width: 3%" align="right">
                    <asp:Label ID="lblICAOLable" Text="ICAO:" runat="server"></asp:Label>
                </td>
                <td style="width: 3%">
                    <asp:Label ID="lblICAO" runat="server"></asp:Label>
                </td>
                <td style="width: 3%">
                    <asp:Label ID="lblRwyIdLable" Text="Runway:" runat="server"></asp:Label>
                </td>
                <td style="width: 3%">
                    <asp:Label ID="lblRwyId" runat="server"></asp:Label>
                </td>
                <td style="width: 3%">
                    <asp:Label ID="lblRwyModLable" Text="Version:" runat="server"></asp:Label>
                </td>
                <td style="width: 3%">
                    <asp:Label ID="lblRwyMod" runat="server"></asp:Label>
                    <asp:Label ID="lblAirlineCode" Visible="false" runat="server"></asp:Label>
                </td>
                <td style="width: 8%;" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Export Format"></asp:Label>
                </td>
                <td style="width: 3%">
                    <asp:DropDownList ID="LstExportFormats" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 3%">
                    <asp:Button ID="BtnExportReport" runat="server" Text="Export / Print Report" />
                </td>
                <td style="width: 3%">
                    <asp:Button Text="Close" ID="btnShowRunway" runat="server" Style="font-weight: 700" />
                </td>
                <td style="width: 30%">
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <table id="Table2" style="width: 100%">
            <tr>
                <td>
                    <CR:CrystalReportViewer ID="CRV_TextReportPrint" runat="server" AutoDataBind="True"
                        DisplayGroupTree="False" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
                        Height="1039px" Width="100%" HasCrystalLogo="False" HasDrillUpButton="False"
                        HasExportButton="False" HasGotoPageButton="False" HasSearchButton="False" HasToggleGroupTreeButton="False"
                        HasViewList="False" HasZoomFactorList="False" EnableDrillDown="False" EnableTheming="True"
                        Font-Size="Large" ShowAllPageIds="True" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
