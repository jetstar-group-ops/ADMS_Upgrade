<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="EnableDisableIPAD.aspx.vb" Inherits="WNB_Admin.EnableDisableIPAD" 
    title="Enable/Disable IPAD" %>
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
     
    </script>
    
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <div style="width: 100%">
        <table cellpadding="0" cellspacing="0" style="width:100%;">
            <tr>
                <td class="tr" align="center">
                    <asp:Label ID="lblReportTitle" runat="server" Text="Enable/Disable IPAD" CssClass="ReportTitle" ></asp:Label>
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
                            <td class="Label" style="width:8%;text-align:right;"><asp:Label ID="lblIpadUDId" runat="server" Text="IPAD UDID" ></asp:Label>&nbsp;</td>
                            <td style="width:20%;">
                                 <asp:DropDownList ID="ddlIpadUDId" runat="server"  Height="22px" DataTextField="IPAD_UDID" DataValueField="IPAD_UDID"></asp:DropDownList>&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="Button" Width="150px" />
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
                                         <asp:GridView ID="gvIPADDetails" HeaderStyle-CssClass="bgDark" runat="server" Width="99%"
                                                    RowStyle-Height="23px" HeaderStyle-Height="23px" HeaderStyle-HorizontalAlign="Left"
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
                                                    <asp:TemplateField HeaderText="IPAD UDID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIPADUDID" runat="server" Text='<%# Container.DataItem("IPAD_UDID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Employee">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEMP_No" runat="server" Text='<%# Container.DataItem("EMP_No")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Domain ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDomainID" runat="server" Text='<%# Container.DataItem("Domain_ID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Domain">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDomain" runat="server" Text='<%# Container.DataItem("Domain")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Date Of Issue">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDateOfIssue" runat="server" Text='<%# Container.DataItem("Date_Of_Issue")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Device Serial Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeviceSerialNumber" runat="server" Text='<%# Container.DataItem("Device_Serial_Number")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Is Disabled">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDisable" runat="server" Checked='<%# Container.DataItem("IsDisabled")%>'  />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField >
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnUpdate" runat="server"  Text="Update" CssClass="Button" Width="70px" CommandArgument='<%# "" & DataBinder.Eval(Container.DataItem,"IPAD_UDID") & "," & DataBinder.Eval(Container.DataItem,"EMP_No") & "," & DataBinder.Eval(Container.DataItem,"version_Nbr") & "" %>' />
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
