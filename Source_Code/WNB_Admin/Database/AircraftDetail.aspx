<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AircraftDetail.aspx.vb" Inherits="WNB_Admin.AircraftDetail" 
    title="Aircraft" %>
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


                  
      function ValidateControls(strCmd)
        {
            var bReturn = 'true';            
            var strErrorMessage = '';
            var RegExp = /^[0-9]*$/;
            var RegExpDec =/^[0-9]+(\.[0-9]{1,4})?$/;  
            
            
            
            if (strCmd != '' && (strCmd == 'ADD' || strCmd == 'UPDATE')) // Generate Report Button
            {
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtAirCraftId').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Aircraft Id.';
                    bReturn = 'false';
                }
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtModelName').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Aircraft Model Name.';
                    bReturn = 'false';
                } 
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtCC').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtCC').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Constant _ C.';
                        bReturn = 'false';
                    }                    
                }   
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtCK').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtCK').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Constant _ K.';
                        bReturn = 'false';
                    }                    
                }   
                 
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtRCO').value.trim() != '') {                       
                    if (!RegExpDec.test(document.getElementById('ctl00_ContentPlaceHolder1_txtRCO').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter numeric values with up to 4 decimal places for Reference Chord Original.';
                        bReturn = 'false';
                    }                    
                } 
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtRCL').value.trim() != '') {                       
                    if (!RegExpDec.test(document.getElementById('ctl00_ContentPlaceHolder1_txtRCL').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter numeric values with up to 4 decimal places for Reference Chord Length.';
                        bReturn = 'false';
                    }                    
                } 
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtRS').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtRS').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Reference Station.';
                        bReturn = 'false';
                    }                    
                }
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtMinOPWTAdj').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtMinOPWTAdj').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Minimum OP Weight Adjustment.';
                        bReturn = 'false';
                    }                    
                }     
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtMaxOPIUAdj').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtMaxOPIUAdj').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Maximum OP Weight Adjustment.';
                        bReturn = 'false';
                    }                    
                }  
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtMinOPIUAdj').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtMinOPIUAdj').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Minimum OP IU Adjustment.';
                        bReturn = 'false';
                    }                    
                }  
                
                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtMaxOPIUAdj').value.trim() != '') {                       
                    if (!RegExp.test(document.getElementById('ctl00_ContentPlaceHolder1_txtMaxOPIUAdj').value.trim())) {
                        strErrorMessage = strErrorMessage + '\n - Enter integer values for Maximum OP IU Adjustment.';
                        bReturn = 'false';
                    }                    
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
                                    <td style="width:130px;" valign="top" >
                                         <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width:100%;border:solid 1px Black;">
                                             <tr style="height:20px;">
                                                <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">Aircraft Details</span></td>
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
                                                                            <asp:TextBox ID="txtAirCraftId" runat="server" CssClass="textbox" style="width:100px;" MaxLength="50"></asp:TextBox><asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" /><asp:HiddenField ID="hidVersionNo" runat="server" />
                                                                        </td>
                                                                        <td >  <div style="position:absolute; margin-top:80px; margin-left:500px;">
                                                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                                                                        AssociatedUpdatePanelID="UpdatePanel1"
                                                                                        DisplayAfter="50" DynamicLayout="true" >                                                                                                    
                                                                                        <ProgressTemplate>
                                                                                            <img border="0" src="../Images/loading.gif" />
                                                                                        </ProgressTemplate>                                                                                                    
                                                                                    </asp:UpdateProgress> 
                                                                              </div>    </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:10px;">&nbsp;</td>
                                                                        <td class="Label" width="15%" style="text-align:left;">
                                                                            <asp:Label ID="lblModelName" runat="server" Text="Aircraft Model Name" ></asp:Label><span style="color: Blue">*&nbsp;</span>
                                                                        </td>
                                                                        <td style="width:15%;text-align:left;">
                                                                            <asp:TextBox ID="txtModelName" runat="server" MaxLength="2000" CssClass="textbox" style="width:200px;"></asp:TextBox>
                                                                        </td>
                                                                        <td > </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td style="height:10px;" colspan="5">&nbsp;</td>
                                                                     </tr>
                                                                    <tr>
                                                                        <td colspan="5">
                                                                             <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td style="width:10px;">&nbsp;</td>
                                                                                       <td valign="top" style="width:140px;">
                                                                                        <fieldset>
                                                                                            <legend><span class="stdTextSilver">IU Equation</span></legend>
                                                                                                <table  border="0" style="height:80px;width:100%;">
		                                                                                            <tr >
                        	                                                                            <td class="Label">Constant _ C<br />
                            	                                                                            <asp:TextBox ID="txtCC" runat="server" style="width:100px;" maxlength="10" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Constant _ K<br />
                            	                                                                            <asp:TextBox ID="txtCK" runat="server" style="width:100px;" maxlength="10" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>                                                                                                   
                                                                                                </table>
                                                                                         </fieldset>
                                                                                    </td>
                                                                                    <td style="width:20px;">&nbsp;</td>	
                                                                                    <td valign="top" style="width:230px;">
                                                                                        <fieldset>
                                                                                            <legend><span class="stdTextSilver">Reference Chord</span></legend>
                                                                                                <table border="0" style="height:120px;width:100%;">
                                                                                                    <tr >
                        	                                                                            <td class="Label">Reference Chord Original - Metres<br />
                            	                                                                            <asp:TextBox ID="txtRCO" runat="server" style="width:100px;" maxlength="15" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only numeric value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Reference Chord Length - Metres<br />
                            	                                                                            <asp:TextBox ID="txtRCL" runat="server" style="width:100px;" maxlength="15" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only numeric value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Reference Station - %_Mac<br />
                            	                                                                            <asp:TextBox ID="txtRS" runat="server" style="width:100px;" maxlength="15" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                         </fieldset>									
                                                                                    </td>					                                                
                                                                                    <td style="width:20px;">&nbsp;</td>	
                                                                                    <td valign="top" style="width:260px;">
                                                                                        <fieldset>
                                                                                            <legend><span class="stdTextSilver">Operating Weight Adjustments</span></legend>
                                                                                                <table border="0" style="height:160px;width:100%;">
                                                                                                    <tr >
                        	                                                                            <td class="Label">Minimum _ OP _WT _ Adjustment_(KG)<br />
                            	                                                                            <asp:TextBox ID="txtMinOPWTAdj" runat="server" style="width:100px;" maxlength="10" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Maximum _ OP _WT _ Adjustment_(KG)<br />
                            	                                                                            <asp:TextBox ID="txtMaxOPWTAdj" runat="server" style="width:100px;" maxlength="10" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Minimum _ OP _IU _ Adjustment<br />
                            	                                                                            <asp:TextBox ID="txtMinOPIUAdj" runat="server" style="width:100px;" maxlength="10" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Maximum _ OP _IU _ Adjustment<br />
                            	                                                                            <asp:TextBox ID="txtMaxOPIUAdj" runat="server" style="width:100px;" maxlength="10" onkeyup = "keyUP(event.keyCode)" onkeydown = "return isNumeric(event.keyCode);" ToolTip="Enter only Integer value"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                         </fieldset>
                                                                                    </td>
                                                                                    <td>&nbsp;</td>
                                                                                </tr>                                                                    
                                                                            </table>
                                                                        </td>
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
  
</asp:Content>
