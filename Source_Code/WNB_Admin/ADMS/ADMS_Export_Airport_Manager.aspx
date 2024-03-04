<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_Export_Airport_Manager.aspx.vb" Inherits="WNB_Admin.ADMS_Export_Airport_Manager"
    Title="Export Airport Manager" %>

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
                        <td align="Center">
                            <asp:Label runat="server" ID="LblTitle" Text="Export Airport Manager" Font-Bold="True"
                                Font-Size="X-Large"></asp:Label>
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
                            <asp:Button ID="BtnClose" runat="server" Style="font-weight: 700" Text="Close" />
                        </td>
                    </tr>
                </table>
                <hr />
                <table id="Table1" style="width: 100%">
                    <tr>
                        <td align="right" class="style1">
                            <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="12pt" Text="Airport Manager Export file path:"></asp:Label>
                        </td>
                        <td class="style2">
                            <asp:Label ID="LblExportFilePath" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:CheckBox ID="ChkExportInActiveRecords" runat="server" Font-Size="12pt" Text="Export Inactive Records" />
                        </td>
                    </tr>
                </table>
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
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                <br></br>
                </br> </br>
            </div>
            <hr />
            <br>
                <br>
                    <br>
                        <br>
                            <br></br>
                            <br></br>
                            <br></br>
                            <br></br>
                            <br></br>
                            <br></br>
                            <br></br>
                            <br></br>
                            <div style="position: absolute; margin-top: 50px; left: 500px;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                    DisplayAfter="50" DynamicLayout="true">
                                    <ProgressTemplate>
                                        <img border="0" src="../Images/loading.gif" />
                                        <asp:Label ID="LblUpdateMessage" runat="server" Font-Bold="True" Font-Size="12pt"
                                            Text="In progress, please wait ..."></asp:Label>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <br>
                                <br>
                                    <br>
                                        <br>
                                            <br></br>
                                            <br></br>
                                            <br></br>
                                            <br></br>
                                            <br></br>
                                            <br></br>
                                            <br></br>
                                            <br></br>
                                            <table id="Table4" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br>
                                                <br>
                                                    <br></br>
                                                    <br></br>
                                                    <br></br>
                                                    <br></br>
                                                    <br></br>
                                                    <br></br>
                                                    <br></br>
                                                    <br></br>
                                                    <br>
                                                        <br>
                                                            <br></br>
                                                            <br>
                                                                <br>
                                                                    <br>
                                                                        <br></br>
                                                                        <br>
                                                                            <br></br>
                                                                            <br>
                                                                                <br></br>
                                                                                <br>
                                                                                    <br></br>
                                                                                    <br>
                                                                                        <br></br>
                                                                                        <br>
                                                                                            <br></br>
                                                                                            <br>
                                                                                                <br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br></br>
                                                                                                    <br>
                                                                                                        <br></br>
                                                                                                        <br>
                                                                                                            <br></br>
                                                                                                        </br>
                                                                                                    </br>
                                                                                                </br>
                                                                                            </br>
                                                                                        </br>
                                                                                    </br>
                                                                                </br>
                                                                            </br>
                                                                        </br>
                                                                    </br>
                                                                </br>
                                                            </br>
                                                        </br>
                                                    </br>
                                                </br>
                                            </br>
                                        </br>
                                    </br>
                                </br>
                            </br>
                        </br>
                    </br>
                </br>
            </br>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
