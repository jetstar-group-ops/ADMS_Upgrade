<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Choice.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ChoiceID" %>
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
                                                <td colspan="2" style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Choice ID</span>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr style="height: 30px;">
                                                <td class="Label" style="width: 3%; text-align: right;">
                                                    <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft"></asp:Label><span style="color: Blue">&nbsp;</span>
                                                </td>
                                                <td style="width: 5%;">
                                                    <asp:DropDownList ID="ddlAircraftID" DataTextField="aircraft_name" DataValueField="aircraft_id"
                                                        runat="server" Height="23px" Width="230px" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                    <asp:HiddenField ID="hidTableId" runat="server" />
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td colspan="2" valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:GridView ID="gvChoice" HeaderStyle-CssClass="bgDark" runat="server" Width="100%"
                                                                    RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-ForeColor="White" AutoGenerateColumns="false" 
                                                                    ShowHeader="true" GridLines="none"
                                                                    CellPadding="2" CellSpacing="0" AllowPaging="true" PagerSettings-Mode="Numeric"
                                                                    PageSize="10" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                                                    RowStyle-HorizontalAlign="Left" AllowSorting="true" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="gdPagerItem" 
                                                                    FooterStyle-CssClass="gdFooterItem" EnableViewState="true">
                                                                    <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                                                    <Columns>
                                                                        <asp:BoundField ItemStyle-Width="10%" HeaderText="Choice ID" DataField="Choices_ID"
                                                                            HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                                                                       
                                                                         <asp:BoundField ItemStyle-Width="10%" HeaderText="Choice_List_ID" DataField="Choice_list_ID"
                                                                            HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                                                                        
                                                                        <asp:BoundField ItemStyle-Width="10%" HeaderText="Description" DataField="Description"
                                                                            HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                                                                        
                                                                        
                                                                         <asp:BoundField ItemStyle-Width="10%" HeaderText="Default" DataField="is_default"
                                                                            HeaderStyle-HorizontalAlign="Left" > </asp:BoundField>
                                                                             
                                                                        
                                                                      <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="3%"  >
                                                                            <ItemTemplate  >
                                                                                <asp:Label ID="lblStatus" runat="server" Text ='<%# Container.DataItem("Is_Active")%>' />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" Width="3%" />
                                                                        </asp:TemplateField>
                                                                       
                                                                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%">                               
                                                                                                <ItemTemplate>
                                                                                                    <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif" CommandName="Edit"  />
                                                                                                    <asp:HiddenField ID="hidVersionNo" runat="server" Value='<%# Container.DataItem("Version_No")%>' />
                                                                                                    <asp:HiddenField ID="HidChoiceListID" runat="server" Value='<%# Container.DataItem("Choice_List_Id")%>' />
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
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;</td>
                                    <td colspan="2" valign="top">
                                        &nbsp;<asp:Button ID="btnAdd" runat="server" CssClass="Button" Text="Add Choice ID" />
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
