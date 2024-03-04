<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="MoveDataToProd.aspx.vb" Inherits="WNB_Admin.MoveDataToProd" 
    title="Data Transfer" %>
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
         
     function ValidateControls(strCmd)
        {
            return confirm("Are you sure want to move data from Pre Production Database to Production Database?");
        }
    </script>
    
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>
    <div style="width: 100%">
        <table cellpadding="0" cellspacing="0" style="width:100%;">
            <tr>
                <td class="tr" align="center">
                    <asp:Label ID="lblReportTitle" runat="server" Text="Move Data From Pre Production To Prodution" CssClass="ReportTitle" ></asp:Label>
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
                        <tr style="height:45px;">
                           
                            <td align="center" >
                                <asp:Button ID="btnMove" runat="server" Text="Move Data From Pre Production To Production" CssClass="Button" Width="450px" />
                               
                            </td>
                            
                            <td style="width:200px;">
                                 <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                            AssociatedUpdatePanelID="UpdatePanel1"
                                            DisplayAfter="25" DynamicLayout="true" >                                                                                                    
                                            <ProgressTemplate>
                                                <img border="0" src="../Images/loading.gif" />
                                            </ProgressTemplate>                                                                                                    
                                        </asp:UpdateProgress> 
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height:10px;"><td>
            </td></tr>
                       
        </table>       
     

      </ContentTemplate>
      </asp:UpdatePanel>
        
    </div>
</asp:Content>
