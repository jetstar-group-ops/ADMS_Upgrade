<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="RegistrationDetail.aspx.vb"
    Inherits="WNB_Admin.RegistrationDetail" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
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
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     
      <div style="width: 100%">
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td class="tr" align="center">&nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" Text="" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                            </table>                
                        </td>
                    </tr>                          
                    <tr>
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" >
                                 <tr>
                                       <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width:100%;border:solid 1px Black;">
                                             <tr style="height:20px;">
                                                <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">Registration</span></td>
                                             </tr> 
                                             <tr style="height:180px;">
                                                <td valign="top" style="padding-left:10Px;padding-top:10Px;padding-right:10Px;">  
                                                     <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                 <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border:solid 1px Black;">
                                                                    <tr>
                                                                        <td style="height:10px;" colspan="5">&nbsp;</td>
                                                                     </tr>
                                                                    <tr style="height:30px;">
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" style="width:15%;text-align:left;">
                                                                            <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft Id" ></asp:Label><span style="color: Blue">*&nbsp;</span>
                                                                        </td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:DropDownList ID="ddlAirCraftId" runat="server" CssClass="textbox" style="width:100px;" MaxLength="50" DataTextField="aircraft_name" DataValueField="aircraft_id" /><asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" /><asp:HiddenField ID="hidVersionNo" runat="server" />
                                                                            <asp:HiddenField ID="hdRegistrationID" runat="server" />
                                                                        </td>
                                                                        <td >  &nbsp;</td>
                                                                        <td>
                                                                            <div style="position:absolute; margin-top:80px; margin-left:500px;">
                                                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                                                                    AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="50" DynamicLayout="true">
                                                                                    <ProgressTemplate>
                                                                                        <img border="0" src="../Images/loading.gif" />
                                                                                    </ProgressTemplate>
                                                                                </asp:UpdateProgress>
                                                                            </div>
                                                                            <asp:Label ID="lblRegistrationNumber" runat="server" Text="Registration Number"></asp:Label>
                                                                            <span style="color: Blue">*&nbsp;</span>
                                                                            <asp:TextBox ID="txtRegistrationNumber" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            MSN</td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtMSN" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > &nbsp;</td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            Sub Fleet</td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtSubFleet" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > &nbsp;</td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            Seat Configuration</td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtSeatConfiguration" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > &nbsp;</td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            Load Data Sheet</td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtLoadDataSheet" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > &nbsp;</td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            Basic Weight-Kg</td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtBasicWt" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > &nbsp;</td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            Basic Arm-M</td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtBasicArm" runat="server" CssClass="textbox" 
                                                                                MaxLength="2000" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > &nbsp;</td>
                                                                        <td>
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td style="height:10px;" colspan="5">&nbsp;</td>
                                                                     </tr>
                                                                    
                                                                    <tr>
                                                                        <td style="height:10px;" colspan="5">&nbsp;</td>
                                                                     </tr>
                                                                     <tr style="height:30px;">
                                                                        <td  colspan="5" style="border-top:solid 1px Black;padding-left:5px;" align="left">
                                                                            <asp:Button ID="BtnSave" runat="server" Text="Add" CssClass="Button"  Width="100px" />
                                                                            <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="Button" Width="80px"  />
                                                                            
                                                                        </td>                                                            
                                                                    </tr>                                                                       
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:10px;"><td>
                                                        </td></tr>
                                                                  
                                                    </table>                                                     
                                                </td>
                                             </tr>  
                                             <tr><td>&nbsp;</td></tr>                                            
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
  
</asp:content>
