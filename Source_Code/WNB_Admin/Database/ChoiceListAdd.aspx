<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChoiceListAdd.aspx.vb"  MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ChoiceListAdd" %>
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
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtChoiceListID').text=='') {
                    strErrorMessage = strErrorMessage + '\n - Required Choice List.';
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
                  height: 213px;
              }
        .style2
    {
        width: 127px;
    }
              .style3
              {
                  width: 20%;
                  height: 48px;
              }
              .style6
              {
                  height: 48px;
              }
              .style7
              {
                  width: 10px;
                  height: 213px;
              }
        </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="width: 110%">
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                     <ContentTemplate>
                <table cellpadding="0" cellspacing="0" style="width:100%;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td class="tr" align="center">&nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" CssClass="ReportTitle" ></asp:Label>
                                    </td>
                                </tr>
                            </table>                
                        </td>
                    </tr>                          
                    <tr>
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" >
                                 <tr>
                                     <td valign="top" class="style2" >
                                         <uc1:Menuctrl ID="MenuItem" runat="server" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width:100%;border:solid 1px Black;">
                                             <tr style="height:20px;">
                                                  <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">Choice List ID - ADD</span></td>
                                             </tr> 
                                             <tr style="height:180px;">
                                                <td valign="top" style="padding-left:10Px;padding-top:10Px;padding-right:10Px;">  
                                                     <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                        <tr>
                                                            <td>
                                                                 <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border:solid 1px Black;">
                                                                    
                                                                    <tr>
                                                                        
                                                                        
                                                                        <td style="text-align:left;"  id="ddlAircraft" class="style3">
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" />
                                                                        </td>
                                                                        
                                                                        <td class="style6" >  <div style="position:absolute; margin-top:80px; margin-left:500px;">
                                                                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                                                                        AssociatedUpdatePanelID="UpdatePanel1"
                                                                                        DisplayAfter="50" DynamicLayout="true" >                                                                                                    
                                                                                        <ProgressTemplate>
                                                                                            <img alt="" border="0" src="../Images/loading.gif" />
                                                                                        </ProgressTemplate>                                                                                                    
                                                                                    </asp:UpdateProgress> 
                                                                              </div>    
                                                                            <asp:HiddenField ID="hidTableId" runat="server" />
                                                                            <asp:HiddenField ID="hidVersionNo" runat="server" />
                                                                        </td>
                                                                              
                                                                    </tr>
                                                                    
                                                                     
                                                                    <tr>
                                                                        <td colspan="5">
                                                                             <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td class="style7"></td>
                                                                                       <td valign="top" class="style1">
                                                                                        <fieldset>
                                                                                            
                                                                                                <table  border="0" style="height:27px; width:92%;">
		                                                                                            <tr >
                        	                                                                            <td class="Label">Choice List ID&nbsp;
                                                                                                            <asp:TextBox ID="txtChoiceListID" runat="server" Width = "200" maxlength="20" 
                                                                                                               
                                                                                                                 ></asp:TextBox>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr >
                        	                                                                            <td class="Label">Description&nbsp;&nbsp; &nbsp;&nbsp;
                                                                                                            <asp:TextBox ID="txtDescription" runat="server" maxlength="15" 
                                                                                                                
                                                                                                                style="width:100px;" Height="76px" 
                                                                                                                TextMode="MultiLine" Width="150px"></asp:TextBox>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>   
                                                                                                    <tr >
                        	                                                                            <td class="Label">Choice ID State<asp:RadioButtonList ID="RadioButtonList1" 
                                                                                                                runat="server">
                                                                                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                                                                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                                                                            </asp:RadioButtonList>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>  
                                                                                                                                                                                          
                                                                                                </table>
                                                                                         </fieldset>
                                                                                    </td>
                                                                                    	
                                                                                    
                                                                                    </td>
                                                                                   
                                                                                       
                                                                                                    </tr>
                                                                                                    
                                                                                                </table>
                                                                                         
                                                                                    </td>
                                                                                   
                                                                                </tr>                                                                    
                                                                            </table>
                                                                            
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                    </tr> 
                                                                     <tr style="height:30px;">
                                                                        <td  colspan="5" style="border-top:solid 1px Black;padding-left:5px;" align="left">
                                                                            <asp:Button ID="BtnAdd" runat="server" Text="Add" CssClass="Button" Width="80px"  />
                                                                            &nbsp;&nbsp;
                                                                            <asp:Button ID="BtnCancel" runat="server" CssClass="Button" Text="Cancel" 
                                                                                Width="100px" />
                                                                            
                                                                        </td>                                                            
                                                                    </tr>                                                                       
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:10px;"><td>
                                                            &nbsp;</td></tr>
                                                                  
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
    