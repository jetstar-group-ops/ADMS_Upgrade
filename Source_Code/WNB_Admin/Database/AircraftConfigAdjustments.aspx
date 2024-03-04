<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AircraftConfigAdjustments.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.AircraftConfigAdjustments" %>

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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%">
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
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td style="width: 130px;" valign="top">
                                        <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 70%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td colspan="4" style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Aircraft Configuration Adjustments</span>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            </tr>
                                            <tr style="height: 30px;">
                                                <td class="Label" style="width: 3%; text-align: right;">
                                                    <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                </td>
                                                <td style="width: 5%;">
                                                    <asp:DropDownList ID="ddlAircraftID" DataTextField="aircraft_name" DataValueField="aircraft_id"
                                                        runat="server" Height="23px" Width="250" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                    <asp:HiddenField ID="hidTableId" runat="server" />
                                                </td>
                                                <td class="Label" width="5%" style="text-align: right;">
                                                    <asp:Label ID="lblModelName" runat="server" Text="Sub fleet"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                </td>
                                                <td style="width: 5%;">
                                                    <asp:DropDownList ID="ddlSubfleetID" runat="server" DataTextField="SubFleet_ID" DataValueField="SubFleet_ID"
                                                        AutoPostBack="true" Height="32px" Width="250">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="3" valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:GridView ID="gvAircraftConfigAdj" HeaderStyle-CssClass="bgDark" runat="server"
                                                                    Width="100%" RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-ForeColor="White" AutoGenerateColumns="false" ShowHeader="true" GridLines="none"
                                                                    CellPadding="2" CellSpacing="0" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                    PageSize="10" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                                                    RowStyle-HorizontalAlign="Left" AllowSorting="true" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem">
                                                                    <Columns>
                                                                        <asp:BoundField ItemStyle-Width="10%" HeaderText="Aircraft Configuration" DataField="AirCraft_Config"
                                                                            HeaderStyle-HorizontalAlign="Left" />
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText ="Aircraft Configuration Enabled" ItemStyle-Width="3%">
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="ddlEnabled" runat="server" SelectedValue='<%# Container.DataItem("is_enabled") %>'
                                                                                    DataValueField="is_enabled">
                                                                                    <asp:ListItem Value="None" Text=""></asp:ListItem>
                                                                                    <asp:ListItem Value="0" Text="Yes">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="1" Text="No">No</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%" HeaderText ="Empty Weight Adjustment - Kg" >
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TxtEmptyWeight" runat="server" Text='<%#  Container.DataItem("Empty_Weight")  %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%" HeaderText ="Empty Arm Adjustment - Mtrs">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TxtArm" runat="server" Text='<%#  Container.DataItem("Arm")  %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="HidReferenceCLID" runat="server" Value='<%#  Container.DataItem("air_conf_adjust_id")  %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="3" valign="top">
                                        <asp:Button ID="btnSave" runat="server" CssClass="Button" Text="Save" Width="100px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </td> </tr> </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
