<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"  CodeBehind="Registration.aspx.vb" Inherits="WNB_Admin.Registration" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />  
    <script language="javascript" type="text/javascript">
    
        function SetDivSize()
        {           
            var divHeight=parseInt((parseInt(screen.height) * 40)/100);
            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.height=divHeight + "px";
        }   
         function ValidateControls(strCmd)
        {
            var bReturn = 'true';            
            var strErrorMessage = '';
                      
            
            if (strCmd != '' && (strCmd == 'CREATE' || strCmd == 'UPDATE')) // Generate Report Button
            {
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_ddlAircraftId').selectedIndex <= 0) {
                    strErrorMessage = strErrorMessage + '\n - Required Aircraft.';
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
                                        <asp:Label ID="lblListTitle" runat="server" Text="" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                            </table>                
                        </td>
                    </tr>                          
                    <tr>
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" >
                                 <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width:100%;border:solid 1px Black;">
                                             <tr style="height:20px;">
                                                <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">Registration</span></td>
                                             </tr> 
                                             <tr style="height:180px;">
                                                <td valign="top" style="padding-left:10Px;padding-top:10Px;padding-right:10Px;">  
                                                     <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                 <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border:solid 1px Black;">
                                                                    <tr style="height:30px;">
                                                                        <td class="Label" style="width:12%;text-align:right;"><asp:Label ID="lblAircraftId" runat="server" Text="Aircraft Id" ></asp:Label><span style="color: Blue">&nbsp;</span></td>
                                                                        <td style="width:15%;">
                                                                            <asp:TextBox ID="txtAirCraftId" runat="server" CssClass="textbox" style="width:90%" MaxLength="50"></asp:TextBox><asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" />
                                                                        </td>
                                                                        <td class="Label" width="18%" style="text-align:right;"><asp:Label ID="lblModelName" runat="server" Text="Registration Number" ></asp:Label><span style="color: Blue">&nbsp;</span></td>
                                                                        <td style="width:20%;">
                                                                            <asp:TextBox ID="txtRegistrationNumber" runat="server" MaxLength="2000" CssClass="textbox" style="width:90%"></asp:TextBox>
                                                                        </td>                                                                      
                                                                        <td width="12%" align="center"><asp:Button ID="btnSearch" runat="server" 
                                                                                Text="Search" CssClass="Button" Width="70px" onclick="btnSearch_Click" /></td>
                                                                        <td >  </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:10px;"><td>
                                                        </td></tr>
                                                        <tr>
                                                            <td style="width:100%;">
                                                                <table cellpadding="0" cellspacing="0" style="border:solid 1px Black;width:100%;" >
                                                                    <tr>
                                                                        <td align="left"  colspan="2">
                                                                              <div style="position:absolute; margin-top:80px; margin-left:500px;">
                                                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                                                                        AssociatedUpdatePanelID="UpdatePanel1"
                                                                                        DisplayAfter="50" DynamicLayout="true" >                                                                                                    
                                                                                        <ProgressTemplate>
                                                                                            <img border="0" src="../Images/loading.gif" />
                                                                                        </ProgressTemplate>                                                                                                    
                                                                                    </asp:UpdateProgress> 
                                                                              </div>                   
                                                                              <div id="DivUpper" style="overflow: auto;" runat="server">
                                                                                   <asp:GridView ID="gvRegistration" HeaderStyle-CssClass="bgDark"   
                                                                                       runat="server"  width="100%" 
                                                                                        RowStyle-Height="20px" HeaderStyle-Height="20px" 
                                                                                       HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="White"
                                                                                        AutoGenerateColumns="false" ShowHeader="true"  GridLines="none" 
                                                                                       CellPadding="2" CellSpacing="0" AllowPaging="true" 
                                                                                       PagerSettings-Mode="Numeric" PageSize="10"  
                                                                                        RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass ="gdAlternativeItem" 
                                                                                        RowStyle-HorizontalAlign="Left" AllowSorting="true" 
                                                                                        ShowFooter="false" FooterStyle-HorizontalAlign="Center" PagerStyle-HorizontalAlign="Right"
                                                                                        PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem" 
                                                                                       onrowcreated="gvRegistration_RowCreated" 
                                                                                       onrowediting="gvRegistration_RowEditing" 
                                                                                       onpageindexchanging="gvRegistration_PageIndexChanging" 
                                                                                       onrowdeleting="gvRegistration_RowDeleting" >
                                                                                        <Columns>
                                                                                            
                                                                                            <asp:BoundField ItemStyle-Width="20%" HeaderText="Registration Number" DataField="registration_number" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="14%" HeaderText="Aircraft Type" DataField="aircraft_id" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="14%" HeaderText="MSN" DataField="MSN" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Seat Configuration" DataField="seat_configuration" HeaderStyle-HorizontalAlign="Left" />                                                                                           
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Load Data Sheet Ref" DataField="load_data_sheet_ref" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Basic Weight" DataField="basic_weight" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Basic Arm" DataField="basic_arm" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Subfleet Id" DataField="subfleet_id" HeaderStyle-HorizontalAlign="Left" />
                                                                                           
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                                                    <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
                                                                                                    <asp:HiddenField ID="HidRegistrationID" runat="server" Value='<%# Container.DataItem("registration_id")%>' />
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
                                                                             <asp:Label id="lblSortExp" runat="server" Visible="False" />                                                    
                                                                        </td>
                                                                    </tr>
                                                                                         
                                                                </table>
                                                            </td>
                                                        </tr> 
                                                         <tr style="height:10px;"><td>
                                                        <tr><td><asp:Button runat="server" id="btnAdd" Text="Add" CssClass="Button" Width="100px" /></td></tr>           
                                                    </table>                                                     
                                                </td>
                                             </tr>  
                                             <tr><td>&nbsp;</td></tr>                                            
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
