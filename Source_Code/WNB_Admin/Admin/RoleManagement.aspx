<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="RoleManagement.aspx.vb" Inherits="WNB_Admin.RoleManagement" 
    title="Role Management" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Role Management</title>
    <link href="../StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />
      
    <script language="javascript" type="text/javascript">
    
        function SetDivSize()
        {           
            var divHeight=parseInt((parseInt(screen.height) * 43)/100);
            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.height=divHeight + "px";
        }   
                  
      function ValidateControls(strCmd)
        {
            var bReturn = 'true';            
            var strErrorMessage = '';
                      
            
            if (strCmd != '' && (strCmd == 'CREATE' || strCmd == 'UPDATE')) // Generate Report Button
            {
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_TxtRoleName').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Role Name.';
                    bReturn = 'false';
                }
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_TxtRoleDescription').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Role Description.';
                    bReturn = 'false';
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td class="tr" align="center">&nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" Text="Role Management" CssClass="ReportTitle" ></asp:Label>
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
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="width:50%;" valign="top">
                                         <table cellpadding="0" cellspacing="0" style="width:100%;border:solid 1px Black;">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" style="width:100%;">
                                                        <tr>
                                                            <td class="tr" align="left">&nbsp;
                                                                <asp:Label ID="lblExistRole" runat="server" Text="Existing Roles" CssClass="ReportTitle" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:10px;"><td > </td></tr>
                                                    </table>                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left:5px;padding-bottom:10px;">
                                                    <div style="position:absolute; margin-top:80px; margin-left:500px;">
                                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                                            AssociatedUpdatePanelID="UpdatePanel1"
                                                            DisplayAfter="10" DynamicLayout="true" >                                                                                                    
                                                            <ProgressTemplate>
                                                                <img src="../Images/loading.gif" border="0" />              
                                                            </ProgressTemplate>                                                                                                    
                                                        </asp:UpdateProgress> 
                                                    </div>  
                                                    <div id="DivUpper" style="overflow: auto; height: 320px;" runat="server">
                                                        <asp:GridView ID="gvRoles" HeaderStyle-CssClass="bgDark"   runat="server"  width="98%" 
                                                            RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="White"
                                                            AutoGenerateColumns="false" ShowHeader="true"  GridLines="none" CellPadding="2" CellSpacing="0" AllowPaging="false" PagerSettings-Mode="Numeric" PageSize="12"  
                                                            RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass ="gdAlternativeItem" 
                                                            RowStyle-HorizontalAlign="Left" AllowSorting="true" 
                                                            ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                            PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem" >
                                                            <Columns>
                                                                <asp:BoundField ItemStyle-Width="35%" HeaderText="Role Name" DataField="Role_Name" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField ItemStyle-Width="59%" HeaderText="Role Description" DataField="Role_Description" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                        <asp:HiddenField ID="hidRoleId" runat="server" Value='<%# Container.DataItem("Role_Id")%>' />
                                                                        <asp:HiddenField ID="hidRoleType" runat="server" Value='<%# Container.DataItem("Role_Type")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>   
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" ID="imgBtnDelete" ToolTip="Delete" ImageUrl="~/Images/trash.gif" CommandName="Delete" OnClientClick="return confirm('Are you sure you want delete Role details?');"   />
                                                                    </ItemTemplate>                                   
                                                                </asp:TemplateField>         
                                                             </Columns>                                                        
                                                        </asp:GridView>                                     
                                                        
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width:2Px;"></td>
                                    <td valign="top">
                                         <table cellpadding="0" cellspacing="0"  style="width:100%;border:solid 1px Black;">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" style="width:100%;">
                                                        <tr>
                                                            <td class="tr" align="left">&nbsp;
                                                                <asp:Label ID="lblCreateUpdateRole" runat="server" Text="Create/Update Role" CssClass="ReportTitle" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:10px;"><td > </td></tr>
                                                    </table>                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td >
                                                     <table cellpadding="0" cellspacing="0" style="width:100%;" border="0" >
                                                        <tr>                                                            
                                                            <td class="Label" style="width:25%;text-align:right;">
                                                                <asp:Label ID="lblRole" runat="server" Text="Role Name"></asp:Label><input id="HdnRoleId" type="hidden" runat="server" />
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td >
                                                                <asp:TextBox ID="TxtRoleName" runat="server" CssClass="textbox" style="width:60%" MaxLength="50"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr> 
                                                            <td class="Label" style="width:25%;text-align:right;">
                                                                <asp:Label ID="lblRoleDescription" runat="server" Text="Role Description"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td >
                                                                <asp:TextBox ID="TxtRoleDescription" runat="server" MaxLength="2000" CssClass="textbox" style="width:96%"
                                                                    TextMode="MultiLine" Height="20px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:5px;">
                                                            <td colspan="2"> </td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="Label" style="width:25%;text-align:right;">
                                                                <asp:Label ID="lblAccess" runat="server" Text="Access Rights"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="text-align:left;">
                                                                <table cellpadding="0" cellspacing="0"  style="width:98%;border:solid 1px Black;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBoxList ID="LstFunctionalities" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                                            </asp:CheckBoxList>
                                                                        </td>
                                                                    </tr>
                                                                </table>                                                                
                                                            </td>
                                                        </tr>
                                                         <tr style="height:5px;">
                                                            <td colspan="2"> </td>
                                                        </tr> 
                                                        <tr style="height:30px;">
                                                            <td  colspan="2" style="border-top:solid 1px Black;padding-left:5px;" align="left">
                                                                <asp:Button ID="BtnSave" runat="server" Text="Create Role" CssClass="Button"  Width="100px" />
                                                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="Button" Width="80px" Visible="false" />
                                                                <asp:Button ID="BtnClear" runat="server" Text="Clear" CssClass="Button" Width="80px" />
                                                                
                                                            </td>                                                            
                                                        </tr>                                                                                                            
                                                     </table>
                                                </td>
                                            </tr>
                                        </table>
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
