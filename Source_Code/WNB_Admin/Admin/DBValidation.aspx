<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="DBValidation.aspx.vb" Inherits="WNB_Admin.DBValidation" Title="Database Sanity Checks" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .body
        {
            font-family: Verdana;
            font-size: 11px;
        }
        .ReportTitle
        {
            font-size: medium;
            font-family: Times New Roman;
            font-weight: bold;
        }
        .Label
        {
            text-decoration: none;
            color: #000000;
            height: 30px;
            text-align: left;
            font-family: Verdana;
            font-size: 11px;
        }
        .Button
        {
            font-weight: bold;
            border: solid 1px Black;
            height: 22px;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdHeader
        {
            background-color: #E4E4EC;
            font-family: Verdana;
            font-size: 11px;
        }
        .bgDark
        {
            background-color: #666666;
            color: White;
            font-weight: bold;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdItem
        {
            background-color: #FFFFFF;
            color: #000000;
            width: 50px;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdAlternativeItem
        {
            background-color: #EEEEEE;
            color: #000000;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdPagerItem
        {
            font-family: Verdana;
            font-size: 11px;
        }
        .gdFooterItem
        {
            font-family: Verdana;
            font-size: 11px;
        }
        .textbox
        {
            font-family: Verdana;
            font-size: 11px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function SetDivSize()
        {
            var divWidth=parseInt((parseInt(screen.width) * 97)/100)+5;
            var divHeight=parseInt((parseInt(screen.height) * 52)/100);
            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.width=divWidth + "px";           
            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.height=divHeight + "px";
           
        }      
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%">
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td class="tr" align="center">
                    <asp:Label ID="lblReportTitle" runat="server" Text="Database Sanity Checks" CssClass="ReportTitle"></asp:Label>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
        </table>
        
        <div style="width: 100%">
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
               <%-- <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td class="tr" align="center">
                                    &nbsp;
                                    <asp:Label ID="lblListTitle" runat="server" Text="Weight and Balance Database Details"
                                        CssClass="ReportTitle"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                <tr style="height: 5px;">
                    <td>
                    </td>
                </tr>
                <tr id="trMsg" runat="server" style="display: none; height: 0Px;">
                    <td align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 700px; border: solid 1px Black;">
                            <tr style="height: 10px;">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="width: 30%; text-align: left; padding-left: 20px;">
                                    <asp:Label ID="lblCurrDBVar" runat="server" Text="Current DataBase Version"></asp:Label>
                                    <span style="color: Blue">&nbsp;</span>
                                </td>
                                <td style="text-align: left; font-weight: bold;">
                                    <asp:Label ID="lblCurrDBVarVal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="width: 30%; text-align: left; padding-left: 20px;">
                                    <asp:Label ID="lblPubDate" runat="server" Text="Published Date"></asp:Label>
                                    <span style="color: Blue">&nbsp;</span>
                                </td>
                                <td style="text-align: left; font-weight: bold;">
                                    <asp:Label ID="lblPubDateVal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="width: 30%; text-align: left; padding-left: 20px;">
                                    <asp:Label ID="lblPubBy" runat="server" Text="Published By"></asp:Label>
                                    <span style="color: Blue">&nbsp;</span>
                                </td>
                                <td style="text-align: left; font-weight: bold;">
                                    <asp:Label ID="lblPubByVal" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td colspan="2" style="border-top: solid 1px Black; padding-left: 5px;" align="center">
                                    <asp:Button ID="btnDBUpgrade" runat="server" Text="Start Database Upgrade Session"
                                        CssClass="Button" Width="250px" Visible="false" />
                                    <asp:Button ID="btnPublishDB" runat="server" Text="Publish Database" CssClass="Button"
                                        Width="170px" Visible="false" />
                                    <asp:Button ID="btnDBUpgradeCancel" runat="server" Text="Cancel Database Upgrade Session"
                                        CssClass="Button" Width="250px" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%" style="border: solid 1px Black;">
                                <tr style="height: 32px;">
                                    <td width="50%" align="right">
                                        <asp:Button ID="btnSanityChecks" runat="server" Text="Database Sanity Checks" CssClass="Button"
                                            Width="200px" />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Button" Width="100px" />
                                    </td>
                                    <td width="10px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <table cellpadding="0" cellspacing="0" style="border: solid 1px Black; width: 100%;">
                                <tr>
                                    <td align="left">
                                        <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                DisplayAfter="50" DynamicLayout="true">
                                                <ProgressTemplate>
                                                    <img border="0" src="../Images/loading.gif" />
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                        <div id="DivUpper" style="overflow: auto;" runat="server">
                                            <asp:GridView ID="gvSanityCheck" HeaderStyle-CssClass="bgDark" runat="server" Width="99%"
                                                RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-ForeColor="White" AutoGenerateColumns="false" ShowHeader="true" GridLines="none"
                                                CellPadding="2" CellSpacing="0" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                PageSize="12" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                                RowStyle-HorizontalAlign="Left" AllowSorting="true" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem">
                                                <Columns>
                                                    <asp:BoundField ItemStyle-Width="1px" HeaderText="" />
                                                    <asp:BoundField ItemStyle-Width="10%" HeaderText="Sanity Check Id" DataField="Sanity_Check_Id"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField ItemStyle-Width="30%" HeaderText="Sanity Rule" DataField="Sanity_Rule"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField ItemStyle-Width="10%" HeaderText="Result" DataField="Result" HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField ItemStyle-Width="49%" HeaderText="Description" DataField="Error_Description"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td>
                                        <asp:DataGrid ID="dgRptHeader" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                                            BorderStyle="Solid" GridLines="Both" ItemStyle-Width="100%" Width="100%" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                                            <Columns>
                                                <asp:BoundColumn ItemStyle-Width="10%" DataField="Sanity_Check_Id" HeaderText="Sanity Check Id">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn ItemStyle-Width="30%" DataField="Sanity_Rule" HeaderText="Sanity Rule">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn ItemStyle-Width="10%" DataField="Result" HeaderText="Result"></asp:BoundColumn>
                                                <asp:BoundColumn ItemStyle-Width="50%" DataField="Error_Description" HeaderText="Description">
                                                </asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
