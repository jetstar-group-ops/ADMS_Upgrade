<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_Export_Data_For_Boeing_OPT.aspx.vb" Inherits="WNB_Admin.ADMS_Export_Data_For_Boeing_OPT"
    Title="Export Data for OPT Airport Database Format V4.0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 30%;
            height: 35px;
        }
        .style2
        {
            height: 35px;
        }
        .style3
        {
            height: 55px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <asp:HiddenField ID="HdnExportFileName" runat="server" />
                <table id="TblTitle" style="width: 100%">
                    <tr bgcolor="#DADADA">
                        <td align="center">
                            <asp:Label runat="server" ID="LblTitle" Text="Export for OPT Airport Database Format V4.0"
                                Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <hr />
                <table id="Table2" style="width: 100%">
                    <tr>
                        <td style="width: 1%">
                            <asp:Button ID="BtnExport" runat="server" Style="font-weight: 700" Text="Start Export" />
                        </td>
                        <td style="width: 10%">
                            <asp:Button Text="Close" ID="BtnClose" runat="server" Style="font-weight: 700" />
                        </td>
                    </tr>
                </table>
                <hr />
                <br>
                    <table id="Table1" style="width: 100%">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="12pt" Text="Database Template"></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:Label ID="LblEmptyBoeingOptSqlieDbFile" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="Label2" runat="server" Font-Bold="False" Font-Size="12pt" Text="OPT Database"></asp:Label>
                            </td>
                            <td class="style3">
                                <asp:Label ID="LblExportedOptDatabaseFile" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="ChkExportInActiveRecords" runat="server" Font-Size="12pt" 
                                    Text="Export Inactive Records" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table id="Table3" style="width: 100%">
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%">
                            </td>
                            <td style="width: 85%">
                                <asp:Label ID="LblExportDataSummary" runat="server" Font-Bold="True" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br>
                        <br>
                            <br>
                <br></br>
                <br></br>
                <div style="position: absolute; margin-top: 50px; left: 500px;">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="50" DynamicLayout="true">
                        <ProgressTemplate>
                            <img border="0" src="../Images/loading.gif" />
                            <asp:Label ID="LblUpdateMessage" runat="server" Font-Bold="True" 
                                Font-Size="12pt" Text="Export is in progress, please wait ..."></asp:Label>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                </br>
                        </br>
                    </br>
                </br>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
