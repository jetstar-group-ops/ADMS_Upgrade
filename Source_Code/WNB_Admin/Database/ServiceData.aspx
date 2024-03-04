<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ServiceData.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ServiceData" %>
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
    &nbsp;&nbsp;
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 110%">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td class="tr" align="center">
                                        &nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" CssClass="ReportTitle"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top" class="style2">
                                        <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Service Data</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="80%" border="0">
                                                        <tr style="height: 30px;">
                                                            <td class="style4" style="width: 15%; text-align: left;">
                                                                &nbsp;<asp:Label ID="lblAircraftId" runat="server" Text="Aircraft Id"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="text-align: left;" class="style6">
                                                                <asp:DropDownList ID="ddlAirCraftId" runat="server" AutoPostBack="true" CssClass="textbox"
                                                                    DataTextField="aircraft_name" DataValueField="aircraft_id" Width="202px" 
                                                                     />
                                                                <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                <asp:HiddenField ID="hidTableId" runat="server" />
                                                                <asp:HiddenField ID="hidVersionNo" runat="server" />
                                                            </td>
                                                            
                                                            <td style="text-align: left;" class="style5">
                                                                &nbsp;</td>
                                                            <td class="style7">
                                                                <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                                        DisplayAfter="50" DynamicLayout="true">
                                                                        <ProgressTemplate>
                                                                            <img alt="" border="0" src="../Images/loading.gif" />
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </div>
                                                                Service Type</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlServiceType" runat="server" Height="24px" 
                                                                        Width="120px" DataValueField="choices_id" 
                                                                        DataTextField="choices_id" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                            </td>
                                                            <tr>
                                                                <td colspan="2" valign="top">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <asp:GridView ID="gvServiceData" runat="server" AutoGenerateColumns="false" CellPadding="2"
                                                                                    CellSpacing="0" EnableViewState="true" FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal"
                                                                                    HeaderStyle-CssClass="bgDark" HeaderStyle-ForeColor="White" HeaderStyle-Height="20px"
                                                                                    HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="gdItem" RowStyle-Height="20px"
                                                                                    RowStyle-HorizontalAlign="Left" ShowFooter="false" ShowHeader="true" 
                                                                                    Width="78%">
                                                                                    <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                                                                    <Columns>
                                                                                        <asp:BoundField ItemStyle-Width="10%" HeaderText="Passenger Type" DataField="category_choicelistid"
                                                                                            HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Passenger Weight">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="Txtweight" runat="server" Text='<%#  Container.DataItem("weight")  %>' >
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Occupies Seat">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddloccupiesSeat" runat="server" SelectedValue='<%# Container.DataItem("occupies_seat") %>'
                                                                                                    DataValueField="occupies_seat">
                                                                                                    <asp:ListItem Value="None" Text=""></asp:ListItem>
                                                                                                    <asp:ListItem Value="0" Text="Yes">Yes</asp:ListItem>
                                                                                                    <asp:ListItem Value="1" Text="No">No</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hidServiceDataID" runat="server" Value='<%#  Container.DataItem("Servicedata_id")  %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                         <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%#  Container.DataItem("Version_no")  %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                            
                                                                                    </Columns>
                                                                                    <FooterStyle CssClass="gdFooterItem" />
                                                                                    <HeaderStyle CssClass="bgDark" ForeColor="White" Height="20px" 
                                                                                        HorizontalAlign="Left" />
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                
                                                                
                                                                
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="Button" Text="Save" 
                                                                            Width="70px" />
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    
                                                                </tr>
                                                            </tr>
                                                            </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
