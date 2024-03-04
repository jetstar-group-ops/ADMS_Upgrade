<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UlDPositionEdit.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.UlDPositionEdit" %>

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
                                                    <span style="font-weight: bold;">Underfloor -Position Details</span>
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
                                                                        <td style="width: 30%;">
                                                                            <asp:Label ID="lblAircraft" runat="server" AutoPostBack="true" 
                                                                                CssClass="Label"  Height="20px" Width="200px" Font-Bold="True"/>
                                                                            
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                            <asp:HiddenField ID="hidTableId" runat="server" />
                                                                            <asp:HiddenField ID="hidPosID" runat="server" />
                                                                            <asp:HiddenField ID="hidversion" runat="server" />
                                                                        </td>
                                                                        <td class="Label" style="width: 20%; text-align: right;">
                                                                            <asp:Label ID="Label1" runat="server" Text="Aircraft Configuration"></asp:Label><span
                                                                                style="color: Blue">&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 10%;">
                                                                            <asp:Label ID="lblAircarftConfig" runat="server" AutoPostBack="true" 
                                                                                CssClass="Label"  Height="20px" Width="200px" Font-Bold="True"/>
                                                                            
                                                                        </td>
                                                                        <td class="Label" style="width: 20%; text-align: right;">
                                                                            <asp:Label ID="Label2" runat="server" Text="ULD Configuration default"></asp:Label><span
                                                                                style="color: Blue">&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 10%;">
                                                                            <asp:Label ID="lblULDConfiguration" runat="server" AutoPostBack="true" 
                                                                                CssClass="Label"  Height="20px" Width="200px" Font-Bold="True"/>
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style=" text-align: right;">
                                                                            Position </td>
                                                                        <td >
                                                                        <asp:TextBox runat="server" ID="txtPosition" ></asp:TextBox> 
                                                                            </td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            Uld Ref</td>
                                                                        <td >
                                                                            <asp:DropDownList ID="ddlUldRef" runat="server" AutoPostBack="true"
                                                                            DataTextField="ULDDefinition" Width="200" DataValueField ="ULDDefinition" 
                                                                            >
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            Max Load
                                                                        </td>
                                                                        <td >
                                                                            <asp:TextBox ID="TxtMaxLoad" runat="server"
                                                                            
                                                                            ></asp:TextBox>
                                                                        </td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                    </tr>
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            Pos Arm</td>
                                                                        <td >
                                                                            <asp:TextBox ID="TxtPosArm" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
                                                                        <td class="Label" style="width: 10%; text-align: right;">
                                                                            &nbsp;</td>
                                                                        <td >
                                                                            &nbsp;</td>
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
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
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
                                                                            </div>
                                                                        
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
                                                                <asp:Button runat="server" ID="btnsave" Text="Save" CssClass="Button" 
                                                                    Width="70px" />
                                                                &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="Button" Text="Cancel" 
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
