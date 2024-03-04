<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="GalleyArms.aspx.vb" Inherits="WNB_Admin.GalleyArms" %>

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
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Galley Arms</span>
                                                </td>
                                                
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                <table cellpadding="0" cellspacing="0" width="60%" border="0">
                                                        <tr style="height: 30px;">
                                                            <td class="style4" style="width: 5%; text-align: left;">
                                                                &nbsp;<asp:Label ID="Label1" runat="server" Text="Aircraft Id"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="width: 5%; text-align: left;">
                                                        <asp:DropDownList ID="ddlAircraftID" DataTextField="aircraft_name" DataValueField="aircraft_id"
                                                            runat="server" Height="23px" Width="250px" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                      <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                <asp:HiddenField ID="hidTableId" runat="server" />
                                                                <asp:HiddenField ID="hidVersionNo" runat="server" />
                                                            </td>
                                                            <td style="width: 5%; text-align: left;" >
                                                                &nbsp;<asp:Label ID="Label2" runat="server" Text="Sub Fleet"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="width: 5%; text-align: left;">
                                                                <asp:DropDownList ID="ddlSubfleetID" runat="server" AutoPostBack="true" CssClass="textbox"
                                                                    DataTextField="SubFleet_ID" DataValueField="SubFleet_ID" Width="250" />
                                                            </td>
                                                            <td>
                                                                <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                                        DisplayAfter="50" DynamicLayout="true">
                                                                        <ProgressTemplate>
                                                                            <img border="0" src="../Images/loading.gif" />
                                                                        </ProgressTemplate>
                                                                    </asp:UpdateProgress>
                                                                </div>
                                                            </td>
                                                            <tr>
                                                                <td colspan="2" valign="top">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <asp:GridView ID="gvGalleyArms" runat="server" AutoGenerateColumns="false" CellPadding="2"
                                                                                    CellSpacing="0" EnableViewState="true" FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal"
                                                                                    HeaderStyle-CssClass="bgDark" HeaderStyle-ForeColor="White" HeaderStyle-Height="20px"
                                                                                    HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="gdItem" RowStyle-Height="20px"
                                                                                    RowStyle-HorizontalAlign="Left" ShowFooter="false" ShowHeader="true" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Galley_Designation" HeaderText="Flight Deck-Seat" />
                                                                                        <asp:TemplateField HeaderText ="Galley Arms Meter">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="TxtArm" runat="server" Text='<%#  Container.DataItem("Arm")  %>'>
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hidCrewGalleyArmID" runat="server" Value='<%# Container.DataItem("crew_galley_arm_id")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td colspan="2" valign="top">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="Button" Text="Save" Width="70px" />
                                                                </td>
                                                                <td valign="top">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                    </table>
                                                
                                                 <%--   <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td style="width: 100%;">
                                                                <table cellpadding="0" cellspacing="0" style="border: solid 1px Black; width: 100%;">
                                                                    <tr>
                                                                        <td align="left" colspan="2">
                                                                            <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                                                    DisplayAfter="50" DynamicLayout="true">
                                                                                    <ProgressTemplate>
                                                                                        <img border="0" src="../Images/loading.gif" alt="" />
                                                                                    </ProgressTemplate>
                                                                                </asp:UpdateProgress>
                                                                            </div>
                                                                            <div id="DivUpper" style="overflow: auto;" runat="server">
                                                                                <asp:GridView ID="gvGalleyArms" HeaderStyle-CssClass="bgDark" runat="server" Width="100%"
                                                                                    RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                                                    HeaderStyle-ForeColor="White" AutoGenerateColumns="false" ShowHeader="true" GridLines="none"
                                                                                    CellPadding="2" CellSpacing="0" AllowPaging="true" PagerSettings-Mode="Numeric"
                                                                                    PageSize="10" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                                                                    RowStyle-HorizontalAlign="Left" AllowSorting="true" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem">
                                                                                    <Columns>
                                                                                        <asp:BoundField ItemStyle-Width="10%" HeaderText="Galley Designation" DataField="Galley_Designation"
                                                                                            HeaderStyle-HorizontalAlign="Left" />
                                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%#  Container.DataItem("Arm")  %>'></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px;">
                                                            <td>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="Button" Width="100px" />
                                                                    </td>
                                                                </tr>
                                                    </table>--%>
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
