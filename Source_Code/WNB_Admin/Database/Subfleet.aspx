<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Subfleet.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.Subfleet"
title="Subfleet Details" %>
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
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_ddlAircraft').selectedIndex <= 0) {
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
                                        <asp:Label ID="lblListTitle" runat="server" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                            </table>                
                        </td>
                    </tr>                          
                    <tr>
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" >
                                 <tr>
                                    <td style="width:130px;" valign="top" >
                                         <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width:100%;border:solid 1px Black;">
                                             <tr style="height:20px;">
                                                <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">Sub Fleet Details</span></td>
                                                
                                                <tr>
                                                 <td><asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" /></td>
                                                </tr>
                                             </tr>
                                             <tr style="height:180px;">
                                                <td valign="top" style="padding-left:10Px;padding-top:10Px;padding-right:10Px;">  
                                                     <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        
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
                                                                                            <img border="0" src="../Images/loading.gif" alt=""/>
                                                                                        </ProgressTemplate>                                                                                                    
                                                                                    </asp:UpdateProgress> 
                                                                              </div> 
                                                                              <div id="DivUpper" style="overflow: auto;" runat="server">
                                                                                   <asp:GridView ID="gvSubfleet" HeaderStyle-CssClass="bgDark"   runat="server"  width="100%" 
                                                                                        RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="White"
                                                                                        AutoGenerateColumns="false" ShowHeader="true"  GridLines="none" CellPadding="2" CellSpacing="0" AllowPaging="true" PagerSettings-Mode="Numeric" PageSize="10"  
                                                                                        RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass ="gdAlternativeItem" 
                                                                                        RowStyle-HorizontalAlign="Left" AllowSorting="true" 
                                                                                        ShowFooter="false" FooterStyle-HorizontalAlign="Center" PagerStyle-HorizontalAlign="Right"
                                                                                        PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem" 
                                                                                        onrowcreated="gvSubfleet_RowCreated" 
                                                                                       onrowediting="gvSubfleet_RowEditing" 
                                                                                       onpageindexchanging="gvSubfleet_PageIndexChanging" 
                                                                                       onrowdeleting="gvSubfleet_RowDeleting">
                                                                                        <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                                                                        <Columns>
                                                                                            <asp:BoundField ItemStyle-Width="10%" HeaderText="Sub Fleet Id" 
                                                                                                DataField="subfleet_Id" HeaderStyle-HorizontalAlign="Left" >
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="10%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="20%" HeaderText="AirCraft Id" 
                                                                                                DataField="Aircraft_Id" HeaderStyle-HorizontalAlign="Left" >
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="20%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="14%" HeaderText="Max Taxi wt" 
                                                                                                DataField="max_taxi_weight" HeaderStyle-HorizontalAlign="Left" >
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="14%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="14%" HeaderText="Max Takeoff wt" 
                                                                                                DataField="max_takeoff_weight" HeaderStyle-HorizontalAlign="Left" >
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="14%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Max landing wt" 
                                                                                                DataField="max_landing_weight" HeaderStyle-HorizontalAlign="Left" >                                                                                           
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="12%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Max zero fuel wt" 
                                                                                                DataField="max_zero_fuel_weight" HeaderStyle-HorizontalAlign="Left" >
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="12%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Flight deck wt" 
                                                                                                DataField="flight_deck_weight" HeaderStyle-HorizontalAlign="Left" >
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="12%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:BoundField ItemStyle-Width="12%" HeaderText="Cabin Crew wt" 
                                                                                                DataField="cabin_crew_weight" HeaderStyle-HorizontalAlign="Left" >
                                                                                            
                                                                                            
                                                                                           
                                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                                <ItemStyle Width="12%" />
                                                                                            </asp:BoundField>
                                                                                            
                                                                                            
                                                                                           
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                                                    <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
                                                                                                    <asp:HiddenField ID="HidSubfleetID" runat="server" Value='<%# Container.DataItem("Subfleet_Id")%>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                                                            </asp:TemplateField>   
                                                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton runat="server" ID="imgBtnDelete" ToolTip="Delete" ImageUrl="~/Images/trash.gif" CommandName="Delete" OnClientClick="return confirm('Are you sure you want delete Role details?');"   />
                                                                                                </ItemTemplate>                                   
                                                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                                                            </asp:TemplateField>         
                                                                                         </Columns>                                                        
                                                                                        <FooterStyle CssClass="gdFooterItem" HorizontalAlign="Center" />
                                                                                        <PagerStyle CssClass="gdPagerItem" HorizontalAlign="Right" />
                                                                                        <HeaderStyle CssClass="bgDark" ForeColor="White" Height="20px" 
                                                                                            HorizontalAlign="Left" />
                                                                                        <AlternatingRowStyle CssClass="gdAlternativeItem" />
                                                                                    </asp:GridView>          
                                                                             </div>
                                                                              </td>
                                                                    </tr>
                                                                                         
                                                                </table>
                                                            </td>
                                                        </tr> 
                                                         <tr style="height:10px;"><td>
                                                        <tr><td><asp:Button runat="server" id="btnAdd" Text="Add Sub fleet" CssClass="Button" Width="100px" /></td></tr>           
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
    

