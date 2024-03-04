<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ViewLoadSheet.aspx.vb" Inherits="WNB_Admin.ViewLoadSheet" 
    title="View Load Sheet" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .body
        {
            font-family: Verdana;
            font-size: 11px;
        }
        .ReportTitle
        {
            font-size: medium;
            font-family: Times New Roman;
            font-weight: bold;
        }
        .Label
        {
            text-decoration: none;
            color: #000000;
            height: 30px;
            text-align: left;
            font-family: Verdana;
            font-size: 11px;
        }
        .Button
        {
            font-weight: bold;
            border: solid 1px Black;
            height: 22px;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdHeader
        {
            background-color: #E4E4EC;
            font-family: Verdana;
            font-size: 11px;
        }
        .bgDark
        {
            background-color: #666666;
            color: White;
            font-weight: bold;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdItem
        {
            background-color: #FFFFFF;
            color: #000000;
            width: 50px;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdAlternativeItem
        {
            background-color: #EEEEEE;
            color: #000000;
            font-family: Verdana;
            font-size: 11px;
        }
        .gdPagerItem
        {
            font-family: Verdana;
            font-size: 11px;
        }
        .gdFooterItem
        {
            font-family: Verdana;
            font-size: 11px;
        }
        .textbox
        {
            font-family: Verdana;
            font-size: 11px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function SetDivSize()
        {
            var divWidth=parseInt((parseInt(screen.width) * 97)/100)+5;
            var divHeight=parseInt((parseInt(screen.height) * 52)/100);
            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.width=divWidth + "px";           
            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.height=divHeight + "px";
           
        }      
     function ValidateControls(strCmd)
        {
            var bReturn = 'true';            
            var strErrorMessage = '';
                                 
            
            if (strCmd != '' && (strCmd == 'VIEWLOADSHEET' )) // Generate Report Button
            {
                                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtDate').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required View Load Sheet Date.';
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <div style="width: 100%">
        <table cellpadding="0" cellspacing="0" style="width:100%;">
            <tr>
                <td class="tr" align="center">
                    <asp:Label ID="lblReportTitle" runat="server" Text="View Load Sheet" CssClass="ReportTitle" ></asp:Label>
                </td>
            </tr>
            <tr style="height:10px;"><td > </td></tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" > 
       

        <ContentTemplate>
        <table cellpadding="0" cellspacing="0" width="100%" border="0">
            <tr>
                <td>
                     <table cellpadding="0" cellspacing="0" width="100%" style="border:solid 1px Black;">
                        <tr style="height:32px;">
                            <td class="Label" style="width:8%;text-align:right;"><asp:Label ID="lblDate" runat="server" Text="Load Sheet Date" ></asp:Label><span style="color: Blue">*&nbsp;</span></td>
                            <td style="width:20%;">
                                 <asp:TextBox ID="txtDate" runat="server" CssClass="textbox" Width="80px" onKeyPress="javascript: return false;"
                                            onPaste="javascript: return false;"></asp:TextBox>
                                            <asp:ImageButton ID="imgDate" runat="server" ImageUrl="~/Images/Calendar.gif" CausesValidation="false" ImageAlign="AbsMiddle" Height="15px" Width="15px" />
                                            <cc1:CalendarExtender ID="calDate" PopupPosition="BottomRight" EnabledOnClient="true" PopupButtonID="imgDate" TargetControlID="txtDate" runat="server" Format="dd-MMM-yyyy" >
                                            </cc1:CalendarExtender>&nbsp;&nbsp;<asp:Button ID="btnViewLoadSheet" runat="server" Text="View Load Sheet" CssClass="Button" Width="150px" />
                            </td>
                          
                            
                            <td > </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height:10px;"><td>
            </td></tr>
            <tr>
                <td style="width:100%;">
                    <table cellpadding="0" cellspacing="0" style="border:solid 1px Black;width:100%;" >
                        <tr>
                            <td align="left"  >
                                  <div style="position:absolute; margin-top:80px; margin-left:500px;">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                            AssociatedUpdatePanelID="UpdatePanel1"
                                            DisplayAfter="50" DynamicLayout="true" >                                                                                                    
                                            <ProgressTemplate>
                                                <img border="0" src="../Images/loading.gif" />
                                            </ProgressTemplate>                                                                                                    
                                        </asp:UpdateProgress> 
                                  </div>                   
                                  <div id="DivUpper" style="overflow: auto;" runat="server">
                                         <asp:GridView ID="gvViewLoadSheet" HeaderStyle-CssClass="bgDark" runat="server" Width="99%"
                                                    RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="White" AutoGenerateColumns="false" ShowHeader="true" GridLines="none"
                                                    CellPadding="2" CellSpacing="0" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                    PageSize="12" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                                    RowStyle-HorizontalAlign="Left" AllowSorting="true" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                    PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                                        <ItemTemplate>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Load Sheet">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="HyperLink1" runat="server" 
                                                                NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"GeneratedFile") %>' 
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"GeneratedFileName") %>' Target="_blank"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                              
                                                </Columns>
                                            </asp:GridView>  
                                 </div> 
                                                                      
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
