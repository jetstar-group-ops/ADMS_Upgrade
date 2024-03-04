<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ServiceDefinitionEdit.aspx.vb" MasterPageFile="~/MasterPage.Master" Inherits="WNB_Admin.ServiceDefinitionEdit" %>
<%@ Register Src="~/SubMenu.ascx" TagName="Menuctrl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../StyleSheet/WNB_Sheet.css" rel="stylesheet" type="text/css" />  
    <script language="javascript" type="text/javascript">
     
   
  </script>
    <style type="text/css">
        .style2
    {
        width: 127px;
    }
        .style3
        {
            width: 14px;
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
                                                  <td style="border-bottom:solid 1px Black;padding-left:30Px;"><span style="font-weight:bold;">Service Definition - Edit</span></td>
                                             </tr> 
                                             <tr style="height:180px;">
                                                <td valign="top" style="padding-left:10Px;padding-top:10Px;padding-right:10Px;">  
                                                     <table cellpadding="0" cellspacing="0" width="90%" border="0">
                                                        <tr>
                                                            <td>
                                                                 <table cellpadding="0" cellspacing="0" border="0" width="100%" style="border:solid 1px Black;">
                                                                    <tr>
                                                                        <td style="height:10px;" colspan="4">&nbsp;</td>
                                                                     </tr>
                                                                    
                                                                    
                                                                    <tr>
                                                                    <td class="style3">
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        Start Flight No
                                                                    <asp:TextBox runat ="server" ID="txtStartFlightNo"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                    
                                                                    End Flight No &nbsp;
                                                                        <asp:TextBox ID="txtEndFlightNo" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;</td>
                                                                    </tr>
                                                                    
                                                                   
                                                                    <tr style="height:30px;">
                                                                        <td class="style3">&nbsp;</td>
                                                                        <td class="Label" style="width:30%;text-align:left;">
                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                        <td style="width:30%;text-align:left;" ID="ddlAircraft">
                                                                            <asp:HiddenField ID="hidFunctionId" runat="server" /><asp:HiddenField ID="hidTableId" runat="server" /><asp:HiddenField ID="hidVersionNo" runat="server" /><asp:HiddenField ID="HidServiceDefinitionId" runat="server" /><asp:HiddenField ID="hidServicedefclID" runat="server" /><asp:HiddenField ID="hidflightdesigref" runat="server" />
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
                                                                            </table>
                                                                            </fieldset>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                        </td>
                                                                    </tr> 
                                                                     <tr style="height:30px;">
                                                                        <td  colspan="5" style="border-top:solid 1px Black;padding-left:5px;" align="left">
                                                                            <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="Button" Width="80px"  />
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
