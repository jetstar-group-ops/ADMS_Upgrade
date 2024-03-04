<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UnderFloorConfiguration.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.UnderFloorConfiguration" %>

<%@ Register Src="~/SubMenu.ascx" TagName="Menuctrl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />  
    <script language="javascript" type="text/javascript">
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
                                      <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Underfloor -Configuration Details</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border: solid 1px Black;">
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 20%;">
                                                                            <asp:DropDownList ID="ddlAircraft" runat="server" CssClass="Label" Height="20px"
                                                                                Width="200px" DataTextField="aircraft_Name" DataValueField="aircraft_Id" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                            <asp:HiddenField ID="hidTableId" runat="server" />
                                                                        </td>
                                                                        <td class="Label" style="width: 20%; text-align: right;">
                                                                            <asp:Label ID="Label1" runat="server" Text="Aircraft Configuration"></asp:Label><span
                                                                                style="color: Blue">&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 20%;">
                                                                            <asp:DropDownList ID="ddlAircarftConfig" runat="server" CssClass="Label" Height="20px"
                                                                                Width="200px" DataTextField="Air_Config_Cl_id" DataValueField="Air_Config_Cl_id"
                                                                                AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td class="Label" style="width: 20%; text-align: right;">
                                                                            <asp:Label ID="Label2" runat="server" Text="ULD Configuration default"></asp:Label><span
                                                                                style="color: Blue">&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 20%;">
                                                                            <asp:DropDownList ID="ddlULDConfiguration" runat="server" CssClass="Label" Height="20px"
                                                                                Width="200px" DataTextField="default_uld_Conf_Cl_id" DataValueField="default_uld_Conf_Cl_id"
                                                                                AutoPostBack="true">
                                                                            </asp:DropDownList>
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
                                                                        <td align="left" colspan="2">
                                                                            <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                                                    DisplayAfter="50" DynamicLayout="true">
                                                                                    <ProgressTemplate>
                                                                                        <img border="0" src="../Images/loading.gif" />
                                                                                    </ProgressTemplate>
                                                                                </asp:UpdateProgress>
                                                                            </div>
                                                                            <div id="DivUpper" style="overflow: auto;" runat="server">
                                                                                <asp:GridView ID="gvPosition" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                                                                    FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal" HeaderStyle-CssClass="bgDark"
                                                                                    HeaderStyle-ForeColor="White" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                                                    RowStyle-CssClass="gdItem" RowStyle-Height="20px" RowStyle-HorizontalAlign="Left"
                                                                                    Width="100%">
                                                                                    <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderText="Position" DataField="pos_ref1" />
                                                                                        <asp:BoundField HeaderText="ULDRef" DataField="default_uld_cl_id" />
                                                                                        <asp:TemplateField HeaderText ="Action">
                                                                                        <ItemTemplate >
                                                                                            <asp:HiddenField runat="server" ID="hidConfigID"  Value='<%# Container.DataItem("uld_conf_id")%>'  />
                                                                                            <asp:HiddenField runat="server" ID="hidVersion"  Value='<%# Container.DataItem("Version_No")%>'  />
                                                                                             <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                                        </ItemTemplate>
                                                                                        
                                                                                       </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <FooterStyle CssClass="gdFooterItem" />
                                                                                    <HeaderStyle CssClass="bgDark" ForeColor="White" Height="20px" HorizontalAlign="Left" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                            <asp:Label ID="lblSortExp" runat="server" Visible="False" />
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
                                                            <td>
                                                                <asp:Button runat="server" ID="btnAdd" Text="Add" CssClass="Button" 
                                                                    Width="70px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
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