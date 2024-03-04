<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ZoneDefinition.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ZoneDefinition" %>

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
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    Zone Definition
                                                </td>
                                               
                                            </tr>
                                            <tr >
                                            <td>
                                            <table >
                                            <tr>
                                            <td>
                                             <tr style="height: 30px;">
                                                    <td class="Label" style="width: 5%; text-align: right;">
                                                        <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                    </td>
                                                    <td style="width: 5%;">
                                                        <asp:DropDownList ID="ddlAircraftID" DataTextField="aircraft_name" DataValueField="aircraft_id"
                                                            runat="server" Height="23px" Width="129px" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                        <asp:HiddenField ID="hidTableId" runat="server" />
                                                    </td>
                                                    <td class="Label" width="5%" style="text-align: right;">
                                                        <asp:Label ID="lblModelName" runat="server" Text="Sub fleet"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                    </td>
                                                    <td style="width: 5%;">
                                                        <asp:DropDownList ID="ddlSubfleetID" runat="server" DataTextField="SubFleet_ID" DataValueField="SubFleet_ID"
                                                            AutoPostBack="true" Height="32px" Width="135px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            
                                            </td>
                                            
                                            </tr>
                                            
                                            </table>
                                            
                                            </td>
                                            </tr>
                                            <tr style="height: 30px;">
                                                <td class="Label" style="text-align: right;">
                                                
                                                <table >
                                                <tr>
                                                <td>
                                                    <asp:GridView ID="gvZoneDefinition" runat="server" AllowPaging="true" 
                                                        AllowSorting="true" AlternatingRowStyle-CssClass="gdAlternativeItem" 
                                                        AutoGenerateColumns="false" CellPadding="2" CellSpacing="0" 
                                                        FooterStyle-CssClass="gdFooterItem" FooterStyle-HorizontalAlign="Center" 
                                                        GridLines="none" HeaderStyle-CssClass="bgDark" HeaderStyle-ForeColor="White" 
                                                        HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left" 
                                                        PagerSettings-Mode="Numeric" PagerStyle-CssClass="gdPagerItem" 
                                                        PagerStyle-HorizontalAlign="Right" PageSize="10" RowStyle-CssClass="gdItem" 
                                                        RowStyle-Height="20px" RowStyle-HorizontalAlign="Left" ShowFooter="false" 
                                                        ShowHeader="true" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="Galley_Designation" 
                                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Galley Designation" 
                                                                ItemStyle-Width="10%" />
                                                            <asp:TemplateField HeaderText ="Zone Arms (Mtrs) " ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtArm" runat="server" 
                                                                        Text='<%#  Container.DataItem("Arm")  %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText ="Zone Max Capacity" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtMaxCapecity" runat="server" 
                                                                        Text='<%#  Container.DataItem("max_capacity")  %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText ="Zone First Row no." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtFirstRowNumber" runat="server" 
                                                                        Text='<%#  Container.DataItem("first_row_number")  %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText ="Zone Last Row no." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtLastRowNumber" runat="server" 
                                                                        Text='<%#  Container.DataItem("last_row_number")  %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText ="Description" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="TxtDescription" runat="server" 
                                                                        Text='<%#  Container.DataItem("description")  %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="HidVersion" runat="server" 
                                                                        Value='<%#  Container.DataItem("Version_No")  %>'/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="HidZoneDefinationID" runat="server" 
                                                                        Value='<%#  Container.DataItem("Zone_Definition_ID")  %>'/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                                </tr>
                                                </table>
                                                
                                                </td>
                                                <td  >
                                                
                                                
                                                
                                                </td>
                                                
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td style="width: 100%;">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
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
