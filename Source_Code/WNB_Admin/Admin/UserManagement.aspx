<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="UserManagement.aspx.vb" Inherits="WNB_Admin.UserManagement" 
    title="User Management" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>User Management</title>
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
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_TxtUserId').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required User Id.';
                    bReturn = 'false';
                }
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_TxtPassword').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Password.';
                    bReturn = 'false';
                } 
                
                if(document.getElementById('ctl00_ContentPlaceHolder1_ddlLocation').selectedIndex <= 0 ) {
                    strErrorMessage = strErrorMessage + '\n - Required Base Location.';
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" > 
        <ContentTemplate> 
            <table cellpadding="0" cellspacing="0" style="width:100%;">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" style="width:100%;">
                            <tr>
                                <td class="tr" align="center">&nbsp;
                                    <asp:Label ID="lblListTitle" runat="server" Text="User Management" CssClass="ReportTitle" ></asp:Label>
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
                                                            <asp:Label ID="lblExistUser" runat="server" Text="Existing Users" CssClass="ReportTitle" ></asp:Label>
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
                                                                <img src="../Images/loading.gif"  border="0" />                                                                
                                                            </ProgressTemplate>                                                                                                    
                                                        </asp:UpdateProgress> 
                                                    </div>  
                                                    <div id="DivUpper" style="overflow: auto; height: 320px;" runat="server">
                                                        <asp:GridView ID="gvUsers" HeaderStyle-CssClass="bgDark"   runat="server"  width="98%" 
                                                            RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="White"
                                                            AutoGenerateColumns="false" ShowHeader="true"  GridLines="none" CellPadding="2" CellSpacing="0" AllowPaging="false" PagerSettings-Mode="Numeric" PageSize="12"  
                                                            RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass ="gdAlternativeItem" 
                                                            RowStyle-HorizontalAlign="Left" AllowSorting="true" 
                                                            ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                            PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem" >
                                                                <Columns>
                                                                    <asp:BoundField ItemStyle-Width="22%" HeaderText="User Id" DataField="User_Id" HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField ItemStyle-Width="20%" HeaderText="Location" DataField="Location_Id" HeaderStyle-HorizontalAlign="Left" />
                                                                    <asp:BoundField ItemStyle-Width="20%" HeaderText="Disabled" DataField="IsDisabled" HeaderStyle-HorizontalAlign="Left" />
                                                                     <%--<asp:BoundField ItemStyle-Width="32%" HeaderText="Roles" DataField="Roles" HeaderStyle-HorizontalAlign="Left" />--%>
                                                                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               

                                                                        <ItemTemplate>

                                                                            <asp:Label ID="LblRoles" runat="server" Text='<%# Container.DataItem("Roles")%>'>

                                                                            </asp:Label>

                                                                        </ItemTemplate>

                                                                    </asp:TemplateField> 
                                                                    
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                            <asp:HiddenField ID="hidPassword" runat="server" Value='<%# Container.DataItem("Password")%>' />
                                                                            <asp:HiddenField ID="hidUserType" runat="server" Value='<%# Container.DataItem("User_Type")%>' />
                                                                            <asp:HiddenField ID="hidUserSNO" runat="server" Value='<%# Container.DataItem("UserSNO")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>   
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton runat="server" ID="imgBtnDelete" ToolTip="Delete" ImageUrl="~/Images/trash.gif" CommandName="Delete" OnClientClick="return confirm('Are you sure you want delete User details?');"   />
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
                                                            <asp:Label ID="lblCreateUpdateUser" runat="server" Text="Create/Update User" CssClass="ReportTitle" ></asp:Label>
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
                                                        <td class="Label" style="width:20%;text-align:right;">
                                                            <asp:Label ID="lblUserId" runat="server" Text="User Id"></asp:Label><input id="HdnUserId" type="hidden" runat="server" /><asp:HiddenField ID="hdnUserSNO" runat="server" />
                                                            <span style="color: Blue">*&nbsp;</span>
                                                        </td>
                                                        <td style="width:25%;">
                                                            <asp:TextBox ID="TxtUserId" runat="server" CssClass="textbox" style="width:95%" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                        <td class="Label" style="width:20%;text-align:right;">
                                                            <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                                                            <span style="color: Blue">*&nbsp;</span>
                                                        </td>
                                                        <td >
                                                            <asp:TextBox ID="TxtPassword" runat="server" MaxLength="15" CssClass="textbox" style="width:75%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>                                                            
                                                        <td class="Label" style="width:20%;text-align:right;">
                                                            <asp:Label ID="lblBaseLocation" runat="server" Text="Base Location"></asp:Label>
                                                            <span style="color: Blue">*&nbsp;</span>
                                                        </td>
                                                        <td style="width:25%;">
                                                            <asp:DropDownList ID="ddlLocation" runat="server" DataTextField="Station_Id" DataValueField="Station_Id" Width="90px" Height="22px" CssClass="Label"></asp:DropDownList>
                                                        </td>
                                                        <td class="Label" style="width:20%;text-align:right;">
                                                            <asp:Label ID="lblDisabled" runat="server" Text="Disabled"></asp:Label>
                                                            <span style="color: Blue">&nbsp;</span>
                                                        </td>
                                                        <td >
                                                            <asp:CheckBox ID="ChkDisabled" runat="server" style="margin:0" />                                                            
                                                        </td>
                                                    </tr>
                                                    <tr style="height:5px;">
                                                        <td colspan="4"> </td>
                                                    </tr> 
                                                    <tr>
                                                        <td class="Label" style="width:20%;text-align:right;">
                                                            <asp:Label ID="lblRoles" runat="server" Text="Roles"></asp:Label>
                                                            <span style="color: Blue">*&nbsp;</span>
                                                        </td>
                                                        <td colspan="3" style="text-align:left;">
                                                            <table cellpadding="0" cellspacing="0"  style="width:98%;border:solid 1px Black;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBoxList ID="LstRoles" runat="server" RepeatColumns="3" RepeatDirection="Vertical">
                                                                        </asp:CheckBoxList>
                                                                    </td>
                                                                </tr>
                                                            </table>                                                                
                                                        </td>
                                                    </tr>
                                                    <tr style="height:5px;">
                                                        <td colspan="4"> </td>
                                                    </tr> 
                                                    <tr style="height:30px;">
                                                        <td  colspan="4" style="border-top:solid 1px Black;padding-left:5px;" align="left">
                                                            <asp:Button ID="BtnUpdate" runat="server" Text="Create User" CssClass="Button"  Width="100px" />
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
