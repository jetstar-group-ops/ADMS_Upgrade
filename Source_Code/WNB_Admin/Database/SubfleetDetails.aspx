<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SubfleetDetails.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.SubfleetDetails" Title="Subfleet Details" %>

<%@ Register Src="~/SubMenu.ascx" TagName="Menuctrl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
     var isShift=false;

        function keyUP(keyCode)
        {
             if(keyCode==16)
                isShift = false;
        }


        function isNumeric(keyCode)
        {
              if(keyCode==16)
                    isShift = true;         

              return ((keyCode >= 48 && keyCode <= 57 || keyCode == 8 || keyCode == 46 || keyCode == 190 ||(keyCode >= 96 && keyCode <= 105)) && isShift == false);

        }

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
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_ddlAircraftId').selectedIndex <= 0) {
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
            width: 348px;
        }
        .style2
        {
            width: 127px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                        <table cellpadding="0" cellspacing="0" border="1" style="width: 100%; border: solid 1px Black;">
                                            <tr style="height: 20px;">
                                                <td style="border-bottom: solid 1px Black; padding-left: 30Px;">
                                                    <span style="font-weight: bold;">Sub fleet Details</span>
                                                </td>
                                            </tr>
                                            <tr style="height: 180px;">
                                                <td valign="top" style="padding-left: 10Px; padding-top: 10Px; padding-right: 10Px;">
                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="60%" style="border: solid 1px Black;">
                                                                    <tr>
                                                                        <td style="height: 10px;" colspan="4">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 30px;">
                                                                        <td class="Label" style="width: 9%; text-align: left;">
                                                                            &nbsp;&nbsp;&nbsp;
                                                                            <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft Id"></asp:Label><span
                                                                                style="color: Blue">*&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 15%; text-align: left;" id="ddlAircraft">&nbsp;
                                                                            <asp:DropDownList ID="ddlAircraftId" runat="server" AutoPostBack="True" DataTextField="aircraft_name"
                                                                                DataValueField="aircraft_id" Height="26px" Width="200px">
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                            <asp:HiddenField ID="hidTableId" runat="server" />
                                                                            <asp:HiddenField ID="hidVersionNo" runat="server" />
                                                                        </td>
                                                                        <td class="Label" width="10%" style="text-align: left;">
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                                                                ID="Label1" runat="server" Text="Sub Fleet Id"></asp:Label><span style="color: Blue">*&nbsp;</span>
                                                                        </td>
                                                                        <td style="width: 15%; text-align: left;">
                                                                            <asp:TextBox ID="txtSubfleetId" runat="server" MaxLength="2000" CssClass="textbox"
                                                                                Style="width: 150px;" Width="175px"></asp:TextBox>
                                                                        </td>
                                                                       
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 10px;" colspan="4">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td style="width: 10px;">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td valign="top" class="style1">
                                                                                        <fieldset>
                                                                                            <table border="0" style="height: 27px; width: 100%;">
                                                                                                <tr>
                                                                                                    <td class="Label">
                                                                                                        Max Taxi weight&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                        <asp:TextBox ID="txtMaxTaxiWt" runat="server" MaxLength="10" onkeydown="return isNumeric(event.keyCode);"
                                                                                                            onkeyup="keyUP(event.keyCode)" Style="width: 100px;" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="Label">
                                                                                                        Max Take Off Weight&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                        <asp:TextBox ID="txtMaxTakeoffWt" runat="server" MaxLength="15" onkeydown="return isNumeric(event.keyCode);"
                                                                                                            onkeyup="keyUP(event.keyCode)" Style="width: 100px;" ToolTip="Enter only numeric value"></asp:TextBox>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="Label">
                                                                                                        Max Landing Weight&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                        <asp:TextBox ID="txtLandingWt" runat="server" MaxLength="10" onkeydown="return isNumeric(event.keyCode);"
                                                                                                            onkeyup="keyUP(event.keyCode)" Style="width: 100px;" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="Label">
                                                                                                        Max Zero Fuel Weight&nbsp;&nbsp;&nbsp;
                                                                                                        <asp:TextBox ID="txtMaxZeroFuelWt" runat="server" MaxLength="10" onkeydown="return isNumeric(event.keyCode);"
                                                                                                            onkeyup="keyUP(event.keyCode)" Style="width: 100px;" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </fieldset>
                                                                                    </td>
                                                                        </td>
                                                                        <td valign="top" style="width: 260px;">
                                                                            <fieldset>
                                                                                <table border="0" style="height: 134px; width: 131%;">
                                                                                    <tr>
                                                                                        <td style="width: 260px;" valign="top">
                                                                                            <table border="0" style="height: 27px; width: 164%;">
                                                                                                <tr>
                                                                                                    <td class="Label">
                                                                                                        Flight Deck Weight&nbsp;&nbsp;
                                                                                                        <asp:TextBox ID="txtFlightDeckWt" runat="server" MaxLength="10" onkeydown="return isNumeric(event.keyCode);"
                                                                                                            onkeyup="keyUP(event.keyCode)" Style="width: 100px;" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        <br />
                                                                                                    </td>
                                                                                                    <tr>
                                                                                                        <td class="Label">
                                                                                                            Cabin Crew Weight&nbsp;
                                                                                                            <asp:TextBox ID="txtCabinCrewWt" runat="server" MaxLength="10" onkeydown="return isNumeric(event.keyCode);"
                                                                                                                onkeyup="keyUP(event.keyCode)" Style="width: 100px;" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                            <br />
                                                                                                        </td>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </fieldset> &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 30px;">
                                                            <td colspan="5" style="border-top: solid 1px Black; padding-left: 5px;" align="left">
                                                                <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="Button" Width="80px" />
                                                                &nbsp;&nbsp;
                                                                <asp:Button ID="BtnCancel" runat="server" CssClass="Button" Text="Cancel" Width="100px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="height: 10px;">
                                                <td>
                                                    &nbsp;
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
                </td> </tr> </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
