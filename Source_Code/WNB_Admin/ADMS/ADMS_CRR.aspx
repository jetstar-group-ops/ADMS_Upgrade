<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_CRR.aspx.vb" Inherits="WNB_Admin.ADMS_CRR" Title="Change Record Report" %>

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
                    <asp:Label runat="server" ID="LblTitle" Text="Change Record Report" Font-Bold="True"
                        Font-Size="16pt" ></asp:Label>
                </td>
            </tr>
        </table>
        </br>
        <table id="Table1" style="width: 100%">
            <tr>
               
                <td style="width: 15%">
                    <asp:Label ID="lblFromDate" runat="server" Text="Start Date"></asp:Label>
                    <asp:TextBox ID="txtFromDate" runat="server" Width="80px" onKeyPress="javascript: return false;"
                        onPaste="javascript: return false;"></asp:TextBox>
                    <asp:ImageButton ID="imgFromDate" runat="server" ImageUrl="~/Images/Calendar.gif"
                        CausesValidation="false" ImageAlign="AbsMiddle" Height="15px" Width="15px" />
                    <cc1:CalendarExtender ID="calFromDate" PopupPosition="BottomRight" EnabledOnClient="true"
                        PopupButtonID="imgFromDate" TargetControlID="txtFromDate" runat="server" Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td style="width: 15%">
                    <asp:Label ID="lblTodate" runat="server" Text="End Date"></asp:Label>
                    <asp:TextBox ID="txtToDate" runat="server" Width="80px" onKeyPress="javascript: return false;"
                        onPaste="javascript: return false;"></asp:TextBox>
                    <asp:ImageButton ID="imgToDate" runat="server" ImageUrl="~/Images/Calendar.gif" CausesValidation="false"
                        ImageAlign="AbsMiddle" Height="15px" Width="15px" />
                    <cc1:CalendarExtender ID="calToDate" PopupPosition="BottomRight" EnabledOnClient="true"
                        PopupButtonID="imgToDate" TargetControlID="txtToDate" runat="server" Format="dd-MMM-yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td style="width: 5%">
                    <asp:Button ID="BtnGenerateReport" runat="server" Text="Generate Report" />
                </td>
                <td style="width: 8%" align="right">
                    <asp:Label ID="Label1" runat="server" Text="Export Format"></asp:Label>
                </td>
                <td style="width: 5%">
                    <asp:DropDownList ID="LstExportFormats" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="width: 8%">
                    <asp:Button ID="BtnExportReport" runat="server" Text="Export Report" />
                </td>
                 <td style="width: 5%">
                    <asp:Button Text="Close" ID="BtnClose" runat="server" Style="font-weight: 700" />
                </td>
                <td style="width: 20%">
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <table id="Table2" style="width: 100%">
            <tr>
                <td>
                    <CR:CrystalReportViewer ID="CrvChangeRequestReport" runat="server" AutoDataBind="True"
                        DisplayGroupTree="False" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
                        Height="1039px" Width="100%" HasCrystalLogo="False" HasDrillUpButton="False"
                        HasExportButton="False" HasGotoPageButton="False" HasSearchButton="False" HasToggleGroupTreeButton="False"
                        HasViewList="False" HasZoomFactorList="False" EnableDrillDown="False" EnableTheming="True"
                        HasPrintButton="False" Font-Size="Large" ShowAllPageIds="True" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
