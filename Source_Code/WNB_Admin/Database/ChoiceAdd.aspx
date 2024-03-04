<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChoiceAdd.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ChoiceIDAdd" %>
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
                
                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtChoice').selectedIndex <= 0) {
                    strErrorMessage = strErrorMessage + '\n - Required Choice ID.';
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
              .style3
              {
                  width: 20%;
              }
              .style4
              {
                  text-decoration: none;
                  color: #000000;
                  height: 30px;
                  text-align: left;
                  font-family: Verdana;
                  font-size: 11px;
                  width: 98px;
              }
              .style5
              {
                  text-decoration: none;
                  color: #000000;
                  height: 30px;
                  text-align: left;
                  font-family: Verdana;
                  font-size: 11px;
                  width: 13%;
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
                                                  <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">ADD Choice ID</span></td>
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
                                                                        <td class="style5" style="text-align:left;">
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                                            </td>
                                                                        <td style="text-align:left;"  id="ddlAircraft" class="style3">
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" /><asp:HiddenField ID="hidVersionNo" runat="server" />
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
                                                                              <tr>
                                                                                  <td colspan="5" style="height:10px;">
                                                                                      &nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="5">
                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td style="width:10px;">
                                                                                            &nbsp;</td>
                                                                                        <td class="style1" valign="top">
                                                                                            <fieldset>
                                                                                                <table border="0" style="height:27px; width:92%;">
                                                                                                    <tr>
                                                                                                        <td class="style4">
                                                                                                            <asp:Label ID="lblAircraftId" runat="server" Text="Aircraft Id"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlAircraftId" runat="server" AutoPostBack="True" 
                                                                                                                DataTextField="aircraft_name" DataValueField="aircraft_id" Height="23px" 
                                                                                                                Width="189px">
                                                                                                            </asp:DropDownList>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="style4">
                                                                                                            Choice List ID</td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlChoiceList" runat="server" AutoPostBack="true" 
                                                                                                                DataTextField="choice_list_id" DataValueField="choice_list_id" Height="18px" 
                                                                                                                Width="187px">
                                                                                                            </asp:DropDownList>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="style4">
                                                                                                            Choice ID</td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtChoice" runat="server" Width="186px"></asp:TextBox>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="style4">
                                                                                                            Description</td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtdescription" runat="server" Height="56px" 
                                                                                                                TextMode="MultiLine" Width="191px"></asp:TextBox>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="style4">
                                                                                                            Default</td>
                                                                                                        <td>
                                                                                                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" Width="73px">
                                                                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                            </asp:RadioButtonList>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="style4">
                                                                                                            Choice ID State</td>
                                                                                                        <td>
                                                                                                            <asp:RadioButtonList ID="RadioButtonList2" runat="server">
                                                                                                                <asp:ListItem Value="1">Active</asp:ListItem>
                                                                                                                <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                                                                            </asp:RadioButtonList>
                                                                                                            <br />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </fieldset>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
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
