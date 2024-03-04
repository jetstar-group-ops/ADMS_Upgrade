﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UnderFloorCompartment.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.UnderFloorCompartment" %>

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
                                                    <span style="font-weight: bold;">Underfloor -Compartment Details</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="70%" border="0">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border: solid 1px Black;">
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style="width: 20%; text-align: right;">
                                                                            <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 30%;">
                                                                            <asp:DropDownList ID="ddlAircraft" runat="server" CssClass="Label" Height="20px"
                                                                                Width="200px" DataTextField="aircraft_Name" DataValueField="aircraft_Id" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                            <asp:HiddenField ID="hidTableId" runat="server" />
                                                                        </td>
                                                                        <td>
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
                                                                                <asp:GridView ID="gvUnderfloor" runat="server" AutoGenerateColumns="False" CellPadding="2"
                                                                                    FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal" HeaderStyle-CssClass="bgDark"
                                                                                    HeaderStyle-ForeColor="White" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                                                    RowStyle-CssClass="gdItem" RowStyle-Height="20px" RowStyle-HorizontalAlign="Left"
                                                                                    Width="100%">
                                                                                    <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                                                                    <Columns>
                                                                                        <asp:BoundField HeaderText="Compartment Name" DataField="Galley_Designation" />
                                                                                        <asp:TemplateField HeaderText="Max CPT Load - KG">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox runat="server" ID="txtMaxCptLoad" Text='<%# Container.DataItem("max_cpt_load")%>'></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Position Ref -1">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList runat="server" ID="ddlRef1" DataTextField="pos_name" SelectedValue='<%# Container.DataItem("Pos_ref1")%>'
                                                                                                    DataValueField="pos_name" />
                                                                                                <%--Text ='<%# Container.DataItem("Pos_ref1")%>' --%>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Position Ref -2">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList runat="server" ID="ddlRef2" DataTextField="pos_name" DataValueField="pos_name"
                                                                                                    SelectedValue='<%# Container.DataItem("Pos_ref2")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Position Ref -3">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList runat="server" DataTextField="pos_name" DataValueField="pos_name"
                                                                                                    ID="ddlRef3" SelectedValue='<%# Container.DataItem("Pos_ref3")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField  >
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField runat="server" ID="hidUndCompID" Value='<%# Container.DataItem("underfloor_comp_id")%>' />
                                                                                                <asp:HiddenField runat="server" ID="hidVersion" Value='<%# Container.DataItem("Version_No")%>' />
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
                                                                <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="Button" Width="70px" />
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
