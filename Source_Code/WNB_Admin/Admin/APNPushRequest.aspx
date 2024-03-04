<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="APNPushRequest.aspx.vb" Inherits="WNB_Admin.APNPushRequest" Title="APN Push Request" %>

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
    
        function ResetValue(Status)
        {
            if(Status == "All")
            {
                document.getElementById("ctl00_ContentPlaceHolder1_rdCheckAll").checked = true;
                document.getElementById("ctl00_ContentPlaceHolder1_rdCheckSingle").checked = false;
                document.getElementById("trDeviceCombo").style.display = 'none';
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_rdCheckAll").checked = false ;
                document.getElementById("ctl00_ContentPlaceHolder1_rdCheckSingle").checked = true;
                document.getElementById("trDeviceCombo").style.display = '';
            }
        
        }
    
        function ToggleDeviceCombo( control)
        {
             
             if(document.getElementById(control.id).value == "rdCheckAll")
             {
                document.getElementById("trDeviceCombo").style.display = 'none';
             }
             else
             {
              document.getElementById("trDeviceCombo").style.display = '';
             }
             //rdAllorSingleDevice
            //rdAllorSingleDevice 
            //document.getElementById("tdDeviceCombo").disply = status ;
        }
  
        function SetDivSize()
        {
            var divWidth=parseInt((parseInt(screen.width) * 97)/100)+5;
            var divHeight=parseInt((parseInt(screen.height) * 52)/100);
//            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.width=divWidth + "px";           
//            document.getElementById('ctl00_ContentPlaceHolder1_DivUpper').style.height=divHeight + "px";
           
        }  
        
         function ValidateControls(strCmd)
        {
            var bReturn = 'true';            
            var strErrorMessage = '';
            var  txtNotificationValue ;
                                 
            
            if (strCmd != '' && (strCmd == 'SENDREQUEST' )) // Generate Report Button
            {
                                                
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtNotification').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Notification Message.';
                    bReturn = 'false';
                }
                
                txtNotificationValue =document.getElementById('ctl00_ContentPlaceHolder1_txtNotification').value.trim()
                
               if(txtNotificationValue.length > 150)
               {
               
                strErrorMessage = strErrorMessage + '\n - Message Can\'t be greater than 150 charactors.';
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="500"  >
    </asp:ScriptManager>
    <div style="width: 100%">
        <table cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td class="tr" align="center">
                    <asp:Label ID="lblReportTitle" runat="server" Text="APN Push Request" CssClass="ReportTitle"></asp:Label>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td>
                            <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                    DisplayAfter="50" DynamicLayout="true">
                                    <ProgressTemplate>
                                        <img border="0" src="../Images/loading.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <table cellpadding="0" cellspacing="0" width="100%" style="border: solid 1px Black;">
                                <tr style="height: 45px;">
                                    <td class="Label" style="width: 15%; text-align: right;">
                                        <asp:Label ID="lblNotification" runat="server" Text="Notification Message"></asp:Label>
                                        <span style="color: Blue">*&nbsp;</span>
                                    </td>
                                    <td width="45%">
                                        <asp:TextBox ID="txtNotification" runat="server" MaxLength="150" CssClass="textbox"
                                            Style="width: 86%" TextMode="MultiLine" Height="20px"></asp:TextBox>
                                    </td>
                                    <td class="Label" width="5%">
                                        <input id="chkBadge" runat="server" type="checkbox" /><label id="lblRPTOption">Badge</label>
                                    </td>
                                    <td width="15%" >
                                        <asp:RadioButton ID="rdCheckAll"  Checked="true" GroupName ="Common"  OnClick="ToggleDeviceCombo(this)" Text="All" runat ="server" />
                                        <asp:RadioButton ID="rdCheckSingle"  Checked="false" GroupName ="Common" OnClick="ToggleDeviceCombo(this)" Text="Single Device" runat ="server" />
                                                                            
                                    </td>
                                    <td align="right" width="14%">
                                        <asp:Button ID="btnAPNPushRequest" runat="server" Text="APN Push Request" CssClass="Button"
                                            Width="120px" />
                                    </td>
                                    <td width="10px">
                                    </td>
                                </tr>
                                <tr id ="trDeviceCombo"  style ="display:none" >
                                    <td style ="width:15%;text-align: right;" class="Label" >
                                    <asp:Label runat="server" Text ="IPAD UDID"   />&nbsp;
                                    </td>
                                    
                                    <td colspan ="4" >
                                        <asp:DropDownList runat ="server" ID="CmbDevice" Style="width: 47%"  />
                                    </td>
                                </tr>
                                <tr>
                                    <asp:GridView ID="grdAPNResults" runat="server" AutoGenerateColumns="true" RowStyle-Height="20px"
                                        HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="White"
                                        ShowHeader="true" GridLines="Both" CellPadding="2" CellSpacing="0" AllowPaging="false"
                                        PagerSettings-Mode="Numeric" PageSize="12" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                        RowStyle-HorizontalAlign="Left" AllowSorting="false" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                        PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem" Height="100px">
                                        <RowStyle CssClass="gdItem" Height="20px" HorizontalAlign="Left" />
                                        <FooterStyle CssClass="gdFooterItem" HorizontalAlign="Center" />
                                        <PagerStyle CssClass="gdPagerItem" />
                                        <HeaderStyle CssClass="bgDark" ForeColor="White" Height="20px" HorizontalAlign="Left" />
                                        <AlternatingRowStyle CssClass="gdAlternativeItem" />
                                    </asp:GridView>
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
                            <table cellpadding="0" cellspacing="0" style="border: solid 0px Black; width: 100%;">
                                <tr>
                                    <td align="left">
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
