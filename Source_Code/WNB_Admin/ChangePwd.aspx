<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ChangePwd.aspx.vb" Inherits="WNB_Admin.ChangePwd" 
    title="Change Password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />  
    <script language="javascript" type="text/javascript">
    

    function ValidateControls(strCmd)
    {
        var bReturn = 'true';   
        var strErrorMessage = '';   
        
        if (strCmd != '' && strCmd == 'CHANGE')
        {
            if (document.getElementById('ctl00_ContentPlaceHolder1_TxtCurrentPwd').value.trim() == '') {
                strErrorMessage = strErrorMessage + '\n - Required Current Password.';
                bReturn = 'false';
            }
            
            if (document.getElementById('ctl00_ContentPlaceHolder1_TxtNewPwd').value.trim() == '') {
                strErrorMessage = strErrorMessage + '\n - Required New Password.';
                bReturn = 'false';
            } 
            
            if (document.getElementById('ctl00_ContentPlaceHolder1_TxtConfirmPwd').value.trim() == '') {
                strErrorMessage = strErrorMessage + '\n - Required Confirm New Password.';
                bReturn = 'false';
            }
            
            if (document.getElementById('ctl00_ContentPlaceHolder1_TxtConfirmPwd').value.trim() != '' && document.getElementById('ctl00_ContentPlaceHolder1_TxtNewPwd').value.trim() != '' ) {
                
                var strNewPsw = document.getElementById('ctl00_ContentPlaceHolder1_TxtNewPwd').value.trim();
                var strCNewPsw = document.getElementById('ctl00_ContentPlaceHolder1_TxtConfirmPwd').value.trim();
                   
                if (strNewPsw != strCNewPsw){
                    strErrorMessage = strErrorMessage + '\n - New Password and Confirm New Password does not match.';
                    bReturn = 'false';
                }
            }        
        }
        
        if (bReturn == 'false') {
            alert(strErrorMessage);
            return false;
        }
        else {
            return true;
        }
    }
    </script>  
    
    
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="width: 100%">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td class="tr" align="center">&nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" Text="Change Password" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height:10px;"><td > </td></tr>
                            </table>                
                        </td>
                    </tr>
                    <tr style="height:5px;">
                        <td></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table cellpadding="0" cellspacing="0" border="0" style="width:40%;border:solid 1px Black;">
                                 <tr style="height:10px;">
                                    <td colspan="2"></td>
                                 </tr>
                                 <tr>
                                     <td class="Label" style="width:50%;text-align:right;">
                                        <asp:Label ID="lblCurrPsw" runat="server" Text="Current Password"></asp:Label>
                                        <span style="color: Blue">*&nbsp;</span>
                                    </td>
                                    <td style="text-align:left;">
                                        <asp:TextBox ID="TxtCurrentPwd" runat="server" CssClass="textbox" style="width:60%" MaxLength="15"  TextMode="Password" ToolTip="Maximum length is 15 characters " ></asp:TextBox>
                                    </td>
                                </tr> 
                                <tr>
                                     <td class="Label" style="width:50%;text-align:right;">
                                        <asp:Label ID="lblNewPsw" runat="server" Text="New Password"></asp:Label>
                                        <span style="color: Blue">*&nbsp;</span>
                                    </td>
                                    <td style="text-align:left;">
                                        <asp:TextBox ID="TxtNewPwd" runat="server" CssClass="textbox" style="width:60%" MaxLength="15"  TextMode="Password" ToolTip="Maximum length is 15 characters " ></asp:TextBox>
                                    </td>
                                </tr>  
                                
                                <tr>
                                     <td class="Label" style="width:50%;text-align:right;">
                                        <asp:Label ID="lblCNewPsw" runat="server" Text="Confirm New Password"></asp:Label>
                                        <span style="color: Blue">*&nbsp;</span>
                                    </td>
                                    <td style="text-align:left;">
                                        <asp:TextBox ID="TxtConfirmPwd" runat="server" CssClass="textbox" style="width:60%" MaxLength="15"  TextMode="Password" ToolTip="Maximum length is 15 characters " ></asp:TextBox>
                                    </td>
                                </tr>  
                                <tr style="height:10px;">
                                    <td colspan="2"></td>
                                 </tr>
                                <tr style="height:30px;">
                                    <td  colspan="2" style="border-top:solid 1px Black;padding-left:5px;" align="center">
                                        <asp:Button ID="BtnChangePwd" runat="server" Text="Change Password" CssClass="Button"  Width="150px" />
                                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="Button" Width="80px"  />                           
                                    </td>                                                            
                                </tr> 
                                
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>   
</asp:Content>


