<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ADMS_ParameterValues.aspx.vb"
    Inherits="WNB_Admin.ADMS_ParameterValues" MasterPageFile="~/ADMS/ADMS_Master.Master"
    Title="Parameter Values" %>

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
            function AllowNumericOnlyWithDecimal(e) {

//            isIE = document.all ? 1 : 0
//            keyEntry = !isIE ? e.which : event.keyCode;

              keyEntry = event.keyCode || event.which;
                
            if (keyEntry >= 46 && keyEntry <= 46)
                return true;
            else if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            if (keyEntry >= 45 && keyEntry <= 45)
                return true;
            else {
                return false;
            }
        }
        function ValidateControls(strCmd)
        {
            var bReturn = 'true';            
            var strErrorMessage = '';
            
            if (strCmd != '' && (strCmd == 'INSERT' || strCmd == 'UPDATE')) // Generate Report Button
            {                   
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtValue').value.trim() == '') {
                    strErrorMessage = strErrorMessage + '\n - Required Parameter Value.';
                    bReturn = 'false';
                }                         
                                
            if (bReturn == 'false') {
                alert(strErrorMessage);
                return false;
            }  
            else {

                return true;
            }          
        }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 100%">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td class="tr" align="center">
                                        &nbsp;
                                        <asp:Label ID="lblListTitle" runat="server" Text="Update Parameter" CssClass="ReportTitle"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <fieldset>
                                <legend><span class="ReportTitle" style="font-size: small;">Parameter</span></legend>
                                <table cellpadding="0" cellspacing="0" width="50%" border="0" style="border: solid 0px Black;">
                                    <tr>
                                        <td class="Label" style="text-align: right;" width="20%">
                                            <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label><span style="color: Blue">&nbsp;</span>
                                            <asp:HiddenField ID="hdnSelectedId" runat="server" />
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="lblNameValue" runat="server" CssClass="Label" Style="width: 96%;"></asp:Label>
                                        </td>
                                        <td class="Label" style="text-align: right;" width="5%">
                                            <asp:Label ID="lblValue" runat="server" Text="Value:"></asp:Label><span style="color: Blue">&nbsp;</span>
                                        </td>
                                        <td width="20%">
                                            <asp:TextBox ID="txtValue" runat="server" CssClass="textbox" Style="width: 96%;"></asp:TextBox>
                                        </td>
                                        <td align="right" width="10%" valign="middle">
                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="Button" Width="80px" />
                                        </td>
                                        <td valign="middle" width="30%">
                                            &nbsp;
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Button" 
                                                Width="80px" />
                                            <asp:Button ID="BtnClose" runat="server" CssClass="Button" Text="Close" 
                                                Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr style="height: 3px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px;">
                            <fieldset>
                                <table cellpadding="0" cellspacing="0" width="100%" border="0" style="border: solid 0px Black;">
                                    <tr>
                                        <td style="padding-left: 5px; padding-bottom: 10px; padding-top: 4px;">
                                            <div style="position: absolute; margin-top: 80px; margin-left: 500px;">
                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                                    DisplayAfter="50" DynamicLayout="true">
                                                    <ProgressTemplate>
                                                        <img border="0" src="../Images/loading.gif" />
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                            <div id="DivUpper" style="overflow: auto; width: 100%; height: 470px;" runat="server">
                                                <asp:GridView ID="gvData" HeaderStyle-CssClass="bgDark" runat="server" Width="25%"
                                                    RowStyle-Height="20px" HeaderStyle-Height="20px" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="White" AutoGenerateColumns="false" ShowHeader="true" GridLines="none"
                                                    CellPadding="2" CellSpacing="0" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                    PageSize="12" RowStyle-CssClass="gdItem" AlternatingRowStyle-CssClass="gdAlternativeItem"
                                                    RowStyle-HorizontalAlign="Left" AllowSorting="true" ShowFooter="false" FooterStyle-HorizontalAlign="left"
                                                    PagerStyle-CssClass="gdPagerItem" FooterStyle-CssClass="gdFooterItem">
                                                    <Columns>
                                                        <asp:BoundField ItemStyle-Width="42%" HeaderText="Parameter Name" DataField="ParameterName"
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField ItemStyle-Width="42%" HeaderText="Value" DataField="LimitValue" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" ID="imgBtnEdit" ToolTip="Edit" ImageUrl="~/Images/edit.gif"
                                                                    CommandName="Edit" />
                                                                <asp:HiddenField ID="hidName" runat="server" Value='<%# Container.DataItem("ParameterName")%>' />
                                                                <asp:HiddenField ID="hidValue" runat="server" Value='<%# Container.DataItem("LimitValue")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
