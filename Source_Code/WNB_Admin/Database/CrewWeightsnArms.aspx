<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrewWeightsnArms.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.CrewWeightsnArms" Title="Crew Weights and Arms" %>

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

    <style type="text/css">
        .style2
        {
            width: 127px;
        }
        .style4
        {
            text-decoration: none;
            color: #000000;
            height: 30px;
            text-align: left;
            font-family: Verdana;
            font-size: 11px;
            width: 163px;
        }
    </style>
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
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td valign="top" class="style2">
                                        <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Crew Weights and Arms</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="60%" border="0">
                                                        <tr style="height: 30px;">
                                                            <td class="style4" style="width: 5%; text-align: left;">
                                                                &nbsp;<asp:Label ID="lblAircraftId" runat="server" Text="Aircraft Id"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="width: 5%; text-align: left;">
                                                                <asp:DropDownList ID="ddlAirCraftId" runat="server" AutoPostBack="true" CssClass="textbox"
                                                                    DataTextField="aircraft_name" DataValueField="aircraft_id" Width="250" />
                                                                <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                <asp:HiddenField ID="hidTableId" runat="server" />
                                                                <asp:HiddenField ID="hidVersionNo" runat="server" />
                                                            </td>
                                                            <td style="width: 5%; text-align: left;" >
                                                                &nbsp;<asp:Label ID="Label1" runat="server" Text="Sub Fleet"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="width: 5%; text-align: left;">
                                                                <asp:DropDownList ID="ddlSublfeet" runat="server" AutoPostBack="true" CssClass="textbox"
                                                                    DataTextField="SubFleet_ID" DataValueField="SubFleet_ID" Width="250" />
                                                            </td>
                                                            <td>
                                                                <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
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
                                                                                <asp:GridView ID="gvFlightDeck" runat="server" AutoGenerateColumns="false" CellPadding="2"
                                                                                    CellSpacing="0" EnableViewState="true" FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal"
                                                                                    HeaderStyle-CssClass="bgDark" HeaderStyle-ForeColor="White" HeaderStyle-Height="20px"
                                                                                    HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="gdItem" RowStyle-Height="20px"
                                                                                    RowStyle-HorizontalAlign="Left" ShowFooter="false" ShowHeader="true" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Galley_Designation" HeaderText="Flight Deck-Seat" />
                                                                                        <asp:TemplateField HeaderText="Arms Meter">
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
                                                                                <asp:GridView ID="gvCabinCrew" runat="server" AutoGenerateColumns="false" CellPadding="2"
                                                                                    CellSpacing="0" EnableViewState="true" FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal"
                                                                                    HeaderStyle-CssClass="bgDark" HeaderStyle-ForeColor="White" HeaderStyle-Height="20px"
                                                                                    HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="gdItem" RowStyle-Height="20px"
                                                                                    RowStyle-HorizontalAlign="Left" ShowFooter="false" ShowHeader="true" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="Galley_Designation" HeaderText="Cabin Crew-Seat" />
                                                                                        <asp:TemplateField HeaderText="Arms Meter">
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
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
