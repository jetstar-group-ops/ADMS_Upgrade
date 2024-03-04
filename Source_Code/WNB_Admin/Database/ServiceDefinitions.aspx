<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ServiceDefinitions.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ServiceDefinitions" %>
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

  
    <style type="text/css">
        .style1
        {
            width: 189px;
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
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top" class="style2">
                                        <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Service Definitions</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr style="height: 30px;">
                                                            
                                                            <td style="width: 25%; text-align: left;" >
                                                                &nbsp;<asp:Label ID="Label1" runat="server" Text="Flight Designation"></asp:Label>
                                                                <span style="color: Blue">*&nbsp;</span>
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:DropDownList ID="ddlflightdesig" runat="server" AutoPostBack="true" 
                                                                    DataTextField="choices_id" DataValueField="choices_id" Width="177px" 
                                                                     />
                                                            </td>
                                                            <td >
                                                               
                                                                Service ID</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlserviceType" runat="server" Height="24px" 
                                                                        Width="120px" DataValueField="choices_id" 
                                                                        DataTextField="choices_id" AutoPostBack="True">
                                                                    </asp:DropDownList>
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
                                                                        <tr><td>&nbsp;</td>
                                                                            <td valign="top">
                                                                                <asp:GridView ID="gvServicedefinition" runat="server" AutoGenerateColumns="false" CellPadding="2"
                                                                                    CellSpacing="0" EnableViewState="true" FooterStyle-CssClass="gdFooterItem" GridLines="Horizontal"
                                                                                    HeaderStyle-CssClass="bgDark" HeaderStyle-ForeColor="White" HeaderStyle-Height="20px"
                                                                                    HeaderStyle-HorizontalAlign="Left" RowStyle-CssClass="gdItem" RowStyle-Height="20px"
                                                                                    RowStyle-HorizontalAlign="Left" ShowFooter="false" ShowHeader="true" 
                                                                                    Width="125%">
                                                                                    <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                                                                    <Columns>
                                                                                       
                                                                                        <asp:TemplateField HeaderText="Start Flight No." ItemStyle-HorizontalAlign="left" ItemStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="Txtstart" runat="server" Text='<%#  Container.DataItem("start_flight_number")  %>'>
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                         <asp:TemplateField HeaderText="End Flight No." ItemStyle-HorizontalAlign="left" ItemStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="Txtend" runat="server" Text='<%#  Container.DataItem("end_flight_number")  %>'>
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        
                                                                                      
                                                                                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                                                    
                                                                                                     <asp:HiddenField ID="HidServiceDefinitionID" runat="server" Value='<%# Container.DataItem("Service_definition_Id")%>' />
                                                                                                      <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
                                                                                                      
                                                                                        </ItemTemplate>
                                                                                                <ItemStyle HorizontalAlign="Center" Width="3%" />
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
                                                                
                                                               
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" class="style1">
                                                                    <asp:Button ID="btnAdd" runat="server" CssClass="Button" Text="Add" 
                                                                        Width="70px" />
                                                                </td>
                                                                <td valign="top">
                                                                    &nbsp;
                                                                    <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                    <asp:HiddenField ID="hidTableId" runat="server" />
                                                                    <asp:HiddenField ID="hidVersionNo" runat="server" />
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