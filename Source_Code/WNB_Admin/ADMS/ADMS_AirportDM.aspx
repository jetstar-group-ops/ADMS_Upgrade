<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_AirportDM.aspx.vb" Inherits="WNB_Admin.ADMS_AirportDM"
    Title="Airports Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../JS/Absenteeism/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery-ui.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.form.js" type="text/javascript"></script>

    <link href="../JS/Absenteeism/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        //          function open_Airport_Details_box() 
        //          {
        //            $('div#DivAirportDetails').parent().appendTo('form#ADMS_AirportDM');
        //            $("#DivAirportDetails").dialog({
        //                modal: true,
        //                bgiframe: true,
        //                width: 1000,
        //                height: 450,
        //                title: "Edit Airport Details",
        //                open: function(type, data) { $(this).parent().appendTo("form"); },
        //                showDialog: ('#DivAirportDetails')
        ////                buttons: {
        ////                    'Close': function() {
        ////                        $(this).dialog('close');
        ////                    }
        ////                }
        //            });
        //        };
        //        function showDialog(selector) {
        //            var source = $(selector).parent();
        //            $(selector).dialog({ open: function(type, data) { $(this).parent().appendTo(source); } });
        //        };
        //        function closeSearchDialog() {
        //            $("#DivAirportDetails").dialog("close");
        //        };




        function AllowNegativeValue(e) {

            keyEntry = event.keyCode || event.which;

            if (keyEntry >= 45 && keyEntry <= 45)
                return true;
            if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else {
                return false;
            }
        }

        function AllowNumericOnly(e) {

            keyEntry = event.keyCode || event.which;

            if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else {
                return false;
            }
        }

        function AllowNumericOnlyWithDecimal(e) {

            keyEntry = event.keyCode || event.which;

            if (keyEntry >= 46 && keyEntry <= 46)
                return true;
            else if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else {
                return false;
            }
        }

        function AllowAlphaNumeric(e) {

            keyEntry = event.keyCode || event.which;
            //alert(keyEntry);
            if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else if (keyEntry >= 65 && keyEntry <= 90)
                return true;
            else if (keyEntry >= 97 && keyEntry <= 122) {
                var mychar;
                mychar = String.fromCharCode(keyEntry);
                mychar = mychar.toUpperCase();
                //alert(mychar);
                e.value = e.value + mychar;
                var evt = arguments[0] || event;
                evt.cancelBubble = true;
                return false;
            }
            else if (keyEntry == 39) {
                //alert(keyEntry);
                return false;
            }
        }

        function ConfirmCancel() {
            return confirm("Are you sure, you want to cancel this operation and ready to loss all unsaved data?");
        }

        function ConfirmDelete() {
            return confirm("Are you sure, you want to delete this airport data, you wouldn't be able to retrive once deleted.?");
        }

        function ShowDataCheckReport(ICAO) {
            //alert(ICAO);
            window.open("ADMS_DataCheckReport.aspx?ICAO=" + ICAO + "&ModuleId=A");
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptMgr" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table id="Table2" style="width: 100%">
                <tr bgcolor="#DADADA">
                    <td align="center">
                        <asp:Label runat="server" ID="LblTitle" Text="Airport Details" Font-Bold="True" Font-Size="16pt"></asp:Label>
                    </td>
                </tr>
            </table>
            <div>
                <table id="TblTitle" style="width: 100%">
                    <tr bgcolor="#DADADA">
                        <td width="10%">
                            <asp:TextBox ID="TxtSearchAirportId" MaxLength="4" runat="server" Width="50px"></asp:TextBox>
                            <asp:Button Text="Select Airport" ID="BtnSelectAirport" runat="server" Style="font-weight: 700;
                                height: 26px;" />
                            <asp:Button Text="Create New Airport" Visible="false" ID="BtnCreateAirport" runat="server"
                                Style="font-weight: 700" />
                            <asp:Button Text="Google Earth (All Airports)" Visible="false" ID="btnGoogleEarthAllAirports"
                                runat="server" Style="font-weight: 700" />
                            <asp:Button Text="Close" ID="BtnClose" runat="server" Style="font-weight: 700" Visible="False" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <asp:HiddenField ID="HdnIcao" runat="server" />
                <asp:HiddenField ID="HdnExportFileName" runat="server" />
                <%--<HeaderStyle CssClass="Freezing" Font-Names="Times New Roman" Font-Size="Medium" />--%>
                <div id="DivAirportDetails" runat="server">
                    <table id="Table4" style="width: 100%; border-style: solid;">
                        <tr>
                            <td width="10%">
                                <asp:Button ID="BtnAddRunway" Text="Add Runway" runat="server" Style="font-weight: 700" />
                                <asp:Button ID="btnGoogleEarth_SingleAirport" Text="Show Runways On Google Earth"
                                    runat="server" Style="font-weight: 700" Width="230px" />
                                <asp:Button ID="btnDataCheck" runat="server" Text="Data Check" Style="font-weight: 700" />
                                <asp:Button ID="btnPrint" runat="server" Text="Print All" Style="font-weight: 700" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table id="Table3" style="width: 100%">
                        <tr>
                            <td width="10%">
                                <asp:Button ID="btnDelete" Text="Delete" runat="server" Style="font-weight: 700"
                                    OnClientClick="javascript:return ConfirmDelete();" />
                                <asp:Button ID="BtnUpdate" Text="Update" runat="server" Style="font-weight: 700" />
                                <asp:Button ID="BtnCancel" Text="Close" runat="server" Style="font-weight: 700" />
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="PnlAirportDetails" runat="server">
                        <table style="width: 100%; border-style: solid">
                            <tr>
                                <td style="width: 60%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="ICAO"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtIcao" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="IATA"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtIata" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtName" runat="server" MaxLength="50" Width="400px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="City"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtCity" runat="server" MaxLength="60"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Country"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtCountry" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="Label6" runat="server" Text="Elevation (ft)"></asp:Label>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtElevation" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td width="15%">
                                                <asp:Label ID="Label10" runat="server" Text="Categorisation"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="LstCategory" runat="server">
                                                   
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="4" bgcolor="#DADADA">
                                                <asp:Label ID="Label7" runat="server" Text="Magnetic Variation" Style="font-weight: 700"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                <asp:DropDownList ID="LstMagDir" runat="server" Width="50px">
                                                    <asp:ListItem>E</asp:ListItem>
                                                    <asp:ListItem>W</asp:ListItem>
                                                    <asp:ListItem>T</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtMagDeg" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtMagMin" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtMagSec" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="4" bgcolor="#DADADA">
                                                <asp:Label ID="Label8" runat="server" Text="Latitude" Style="font-weight: 700"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                <asp:DropDownList ID="LstLatDir" runat="server" Width="50px">
                                                    <asp:ListItem>N</asp:ListItem>
                                                    <asp:ListItem>S</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtLatDeg" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtLatMin" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtLatSec" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="4" bgcolor="#DADADA">
                                                <asp:Label ID="Label9" runat="server" Text="Longitude" Style="font-weight: 700"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%">
                                                <asp:DropDownList ID="LstLonDir" runat="server" Width="50px">
                                                    <asp:ListItem>E</asp:ListItem>
                                                    <asp:ListItem>W</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtLonDeg" runat="server" Width="50px"></asp:TextBox>
                                            </td>
                                            <td width="10%">
                                                <asp:TextBox ID="TxtLonMin" runat="server" MaxLength="50" Width="50px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TxtLonSec" runat="server" MaxLength="50" Width="50px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="10%" bgcolor="#DADADA">
                                                <asp:Label ID="Label14" runat="server" Text="Comments" Style="font-weight: 700"></asp:Label>
                                            </td>
                                            <td width="90%" colspan="3">
                                                <asp:TextBox TextMode="MultiLine" ID="TxtComments" runat="server" Width="90%" Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 40%" valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td bgcolor="#DADADA">
                                                <asp:Label ID="Lable111" runat="server" Text="Runways" Style="font-weight: 700"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DataGrid ID="DgrdAirportRunways" runat="server" AutoGenerateColumns="false"
                                                    Width="98%" BackColor="White" BorderColor="Gray" BorderStyle="None" BorderWidth="2px"
                                                    CellPadding="2" EnableViewState="true" GridLines="Both">
                                                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" BorderStyle="Groove" />
                                                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                                    <ItemStyle BackColor="White" HorizontalAlign="Left" />
                                                    <HeaderStyle Height="20px" BackColor="#DADADA" Font-Bold="True" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Indent" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:HyperLink runat="server" ID="HlnlRunway" Text='<%# DataBinder.Eval(Container.DataItem,"RwyId") %>'
                                                                    NavigateUrl='<%# "ADMS_RunwayDM.aspx?RwyId=" & DataBinder.Eval(Container.DataItem,"RwyId") & "&Icao="  & DataBinder.Eval(Container.DataItem,"ICAO") & "&RwyMod=" & DataBinder.Eval(Container.DataItem,"RwyMod")  %>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Version" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblRwyMod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyMod") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    </Columns>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <%--<HeaderStyle CssClass="Freezing" Font-Names="Times New Roman" Font-Size="Medium" />--%>
                                                </asp:DataGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%; border-style: solid">
                            <tr>
                                <td width="5%">
                                    <asp:CheckBox ID="ChkActive" Text="Active?" runat="server" />
                                </td>
                                <td width="5%" valign="middle">
                                    <asp:Label ID="Label12" runat="server" Text="Updated On: "></asp:Label>
                                    <asp:Label ID="LblUpdatedOn" runat="server"></asp:Label>
                                </td>
                                <td width="5%" valign="middle">
                                    <asp:Label ID="Label13" runat="server" Text="Updated By: "></asp:Label>
                                    <asp:Label ID="LblUpdatedBy" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="position: absolute; margin-top: 50px; left: 500px;">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                DisplayAfter="50" DynamicLayout="true">
                                <ProgressTemplate>
                                    <img border="0" src="../Images/loading.gif" />
                                    <asp:Label ID="LblUpdateMessage" runat="server" Font-Bold="True" Font-Size="12pt"
                                        Text="In progress, Please wait ..."></asp:Label>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <table id="Table1" style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
