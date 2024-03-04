<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Home.aspx.vb" Inherits="WNB_Admin.Home1" 
    title="Technical Support Reporting & Administrative Tool" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div style="width: 100%">
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <%--<tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td class="tr" align="center">&nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" Text="Weight and Balance Database Details" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height:10px;"><td > </td></tr>
                            </table>                
                        </td>
                    </tr>--%>
                    <tr style="height:5px;">
                        <td></td>
                    </tr>
                    <tr id="trMsg" runat="server"  style="display:none;height:0Px;"><td align="center"><asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label></td> </tr>
                                  
                  <%--  <tr>
                        <td align="center">
                            <table cellpadding="0" cellspacing="0" border="0" style="width:700px;border:solid 1px Black;">
                                 <tr style="height:10px;">
                                    <td colspan="2"></td>
                                 </tr>
                                 <tr>
                                     <td class="Label" style="width:30%;text-align:left;padding-left:20px;">
                                        <asp:Label ID="lblCurrDBVar" runat="server" Text="Current DataBase Version"></asp:Label>
                                        <span style="color: Blue">&nbsp;</span>
                                    </td>
                                    <td style="text-align:left;font-weight:bold;">
                                        <asp:Label ID="lblCurrDBVarVal" runat="server" Text=""></asp:Label>                                        
                                    </td>
                                </tr> 
                                <tr>
                                     <td class="Label" style="width:30%;text-align:left;padding-left:20px;">
                                        <asp:Label ID="lblPubDate" runat="server" Text="Published Date"></asp:Label>
                                        <span style="color: Blue">&nbsp;</span>
                                    </td>
                                    <td style="text-align:left;font-weight:bold;">
                                        <asp:Label ID="lblPubDateVal" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>  
                                
                                <tr>
                                     <td class="Label" style="width:30%;text-align:left;padding-left:20px;">
                                        <asp:Label ID="lblPubBy" runat="server" Text="Published By"></asp:Label>
                                        <span style="color: Blue">&nbsp;</span>
                                    </td>
                                    <td style="text-align:left;font-weight:bold;">
                                        <asp:Label ID="lblPubByVal" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>  
                                  <tr style="height:10px;">
                                    <td colspan="2"></td>
                                 </tr>
                                 <tr style="height:30px;">
                                    <td  colspan="2" style="border-top:solid 1px Black;padding-left:5px;" align="center">
                                        <asp:Button ID="btnDBUpgrade" runat="server" Text="Start Database Upgrade Session" CssClass="Button"  Width="250px" visible="false" />
                                        <asp:Button ID="btnPublishDB" runat="server" Text="Publish Database" CssClass="Button" Width="170px" visible="false" />                           
                                        <asp:Button ID="btnDBUpgradeCancel" runat="server" Text="Cancel Database Upgrade Session" CssClass="Button" Width="250px" visible="false" />                           
                                    </td>                                                            
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        
        
    </div>
</asp:Content>
