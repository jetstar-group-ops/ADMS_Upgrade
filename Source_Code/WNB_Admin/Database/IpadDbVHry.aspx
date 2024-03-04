<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="IpadDbVHry.aspx.vb" Inherits="WNB_Admin.IpadDbVHry" 
    title="Analytical Reports" %>
<%@ Register Src="~/SubMenu.ascx" TagName="Menuctrl" TagPrefix="uc1" %>
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
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtAirCraftId').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Aircraft Id.';
                    bReturn = 'false';
                }
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_TxtModelName').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Aircraft Model Name.';
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
                                        <asp:Label ID="lblListTitle" runat="server" Text="IPad Database Version History" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height:10px;"><td > </td></tr>
                            </table>                
                        </td>
                    </tr>
                    <tr style="height:5px;">
                        <td></td>
                    </tr>
                    <tr id="trMsg" runat="server"  style="display:none;height:0Px;"><td align="center"><asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label></td> </tr>
                                  
                    <tr>
                        <td align="center">
                            <table cellpadding="0" cellspacing="0" border="0" style="width:100%;border:solid 1px Black;">
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
                                        <asp:Button ID="btnDBUpgrade" runat="server" Text="Start Database Upgrade Session" CssClass="Button"  Width="250px" />
                                        <asp:Button ID="btnPublishDB" runat="server" Text="Publish Database" CssClass="Button" Width="170px"  />                           
                                        <asp:Button ID="btnDBUpgradeCancel" runat="server" Text="Cancel Database Upgrade Session" CssClass="Button" Width="250px"  />                           
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


<%--<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                        <asp:Label ID="lblListTitle" runat="server" Text="" CssClass="IPad Database Version History" ></asp:Label>
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
                                                <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">IPad Database Version History</span></td>
                                             </tr> 
                                             <tr style="height:180px;">
                                                <td valign="top" style="padding-left:10Px;padding-top:10Px;padding-right:10Px;">  
                                                     <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                 <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border:solid 1px Black;">
                                                                    <tr style="height:30px;">
                                                                        <td class="Label" style="width:10%;text-align:right;"><asp:Label ID="lblIpadUDId" runat="server" Text="IPAD UDID" ></asp:Label><span style="color: Blue">&nbsp;</span></td>
                                                                        <td style="width:30%;">
                                                                             <asp:DropDownList ID="ddlAircraft" runat="server" CssClass="Label" Height="20px" Width="200px" DataTextField="aircraft_Name" DataValueField="aircraft_Id" AutoPostBack="true"></asp:DropDownList><asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" />
                                                                        </td>
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
                                                                                   <asp:GridView ID="gvAircraft" HeaderStyle-CssClass="bgDark"   runat="server"  width="100%" 
                                                                                        RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="White"
                                                                                        AutoGenerateColumns="false" ShowHeader="true"  GridLines="none" CellPadding="2" CellSpacing="0" AllowPaging="true" PagerSettings-Mode="Numeric" PageSize="2"  
                                                                                        RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass ="gdAlternativeItem" 
                                                                                        RowStyle-HorizontalAlign="Left" AllowSorting="true" 
                                                                                        ShowFooter="false" FooterStyle-HorizontalAlign="Center" PagerStyle-HorizontalAlign="Right"
                                                                                        PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem" >
                                                                                        <Columns>
                                                                                            <asp:BoundField ItemStyle-Width="10%" HeaderText="AirCraft Id" DataField="aircraft_Id" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="20%" HeaderText="AirCraft Model Name" DataField="model_name" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="14%" HeaderText="Ref Chord Ori" DataField="ref_chord_origin" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="14%" HeaderText="Ref Chord Len" DataField="ref_station" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Ref Station " DataField="IU_equ_const_K" HeaderStyle-HorizontalAlign="Left" />                                                                                           
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="IU Equ Con C" DataField="IU_equ_const_C" HeaderStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="IU Equ Con K" DataField="IU_equ_const_K" HeaderStyle-HorizontalAlign="Left" />
                                                                                           
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                                                    <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
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
                                                        <tr><td><asp:Button runat="server" id="btnAdd" Text="Add Aircraft" CssClass="Button" Width="100px" /></td></tr>           
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
    
</asp:Content>--%>