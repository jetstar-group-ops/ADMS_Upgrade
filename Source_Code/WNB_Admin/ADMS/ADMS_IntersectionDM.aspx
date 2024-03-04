<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_IntersectionDM.aspx.vb" Inherits="WNB_Admin.ADMS_IntersectionDM"
    Title="Intersections Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../JS/Absenteeism/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery-ui.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.form.js" type="text/javascript"></script>

    <link href="../JS/Absenteeism/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        //          function open_Intersection_Details_box() 
        //          {
        //            $('div#DivIntersectionDetails').parent().appendTo('form#ADMS_IntersectionDM');
        //            $("#DivIntersectionDetails").dialog({
        //                modal: true,
        //                bgiframe: true,
        //                width: 1000,
        //                height: 400,
        //                title: "Edit Intersection Details",
        //                open: function(type, data) { $(this).parent().appendTo("form"); },
        //                showDialog: ('#DivIntersectionDetails')
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
        //            $("#DivIntersectionDetails").dialog("close");
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
            return confirm("Are you sure, you want to delete this intersection data, you wouldn't be able to retrive once deleted.?");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptMgr" runat="server">
    </asp:ScriptManager>
    <div>
        <table id="TblTitle" style="width: 100%">
            <tr bgcolor="#DADADA">
                <td align="center">
                    <asp:Label runat="server" ID="LblTitle" Text="Intersections Details" Font-Bold="True"
                        Font-Size="14pt"></asp:Label>
                </td>
            </tr>
            <td style="height: 5px">
            </td>
            <tr bgcolor="#DADADA" visible="false">
                <td>
                    <asp:Button Text="Create New Intersection" ID="BtnCreateIntersection" runat="server"
                        Style="font-weight: 700" Visible="false" />
                    <asp:Button Text="Close" ID="BtnShowrunwayDetails" runat="server" Style="font-weight: 700"
                        Visible="False" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div id="DivIntersections" style="overflow: auto; height: 0px;" runat="server">
            <asp:HiddenField ID="HdnIcao" runat="server" />
            <asp:HiddenField ID="HdnRwyId" runat="server" />
            <asp:HiddenField ID="HdnRwyMod" runat="server" />
            <asp:HiddenField ID="HdnIntersectionId" runat="server" />
            <table id="TblIntersections" style="width: 90%">
                <tr>
                    <td>
                        <asp:DataGrid ID="DgrdIntersections" runat="server" AutoGenerateColumns="false" Width="98%"
                            BackColor="White" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" CellPadding="2"
                            EnableViewState="true" GridLines="Both" Visible="False">
                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                            <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" BorderStyle="Groove" />
                            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                            <ItemStyle BackColor="White" HorizontalAlign="Left" />
                            <HeaderStyle Height="20px" BackColor="#DADADA" Font-Bold="True" HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="ICAO" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblIcao" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ICAO") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Width="5%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Rwy ID" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblRwyId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyId") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Width="25%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Rwy Mod" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblRwyMod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyMod") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Width="25%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Ident" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblIdent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Ident") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Width="25%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Displacement (m)" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDeltaFieldLength" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DeltaFieldLength") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle Width="25%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnEdit" Text='Edit' runat="server" CommandArgument='<%# "E," & DataBinder.Eval(Container.DataItem,"Intersection_Id") %>'>
                                        </asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnCheckIntersection" Text='Check Intersection' runat="server" CommandArgument='<%# "C," & DataBinder.Eval(Container.DataItem,"Intersection_Id") %>'>
                                        </asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                </asp:TemplateColumn>
                                <%-- <asp:TemplateColumn HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnPrintCola" Text='Print COLA' runat="server" CommandArgument='<%# "P," & DataBinder.Eval(Container.DataItem,"Intersection_Id") %>'>
                                        </asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                </asp:TemplateColumn>--%>
                                <asp:TemplateColumn HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnDelete" OnClientClick="javascript:return ConfirmDelete();" Text='Delete'
                                            runat="server" CommandArgument='<%# "D," & DataBinder.Eval(Container.DataItem,"Intersection_Id") %>'>
                                        </asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                                </asp:TemplateColumn>
                            </Columns>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle CssClass="Freezing" Font-Names="Times New Roman" Font-Size="Medium" />
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div id="DivIntersectionDetails" style="display: inline" runat="server">
            <table id="Table1" style="width: 100%">
                <tr>
                    <td>
                        <asp:Button ID="btnDelete" Text="Delete" runat="server" OnClientClick="javascript:return ConfirmDelete();"
                            Style="font-weight: 700" />
                        <asp:Button ID="BtnUpdate" Text="Update" runat="server" Style="font-weight: 700" />
                        <asp:Button ID="BtnCancel" Text="Close" runat="server" Style="font-weight: 700" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnlIntersectionDetails" runat="server">
                <table style="width: 100%; border-style: solid">
                    <tr>
                        <td style="width: 60%">
                            <table style="width: 100%">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label1" runat="server" Text="ICAO"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblIcao" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" Text="Runway"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblRwyId" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label3" runat="server" Text="Version"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="LblRwyMod" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label4" runat="server" Text="Intersection"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtIdent" runat="server" MaxLength="60" Width="70px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label5" runat="server" Text="Displacement (m)"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TxtDeltaFieldLength" runat="server" MaxLength="50" Width="50px"></asp:TextBox>
                                    </td>
                                    <td width="13%" align="right">
                                        <asp:Label ID="Label6" runat="server" Text="Elevation at Intersection(ft)"></asp:Label>
                                    </td>
                                    <td width="5%">
                                        <asp:TextBox ID="TxtElevStartTORA" runat="server" Width="50px"></asp:TextBox>
                                    </td>
                                    <td width="7%" align="right">
                                        <asp:Label ID="Label10" runat="server" Text="Lineup Angle"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="LstLineUpAngle" runat="server">
                                            <asp:ListItem>0</asp:ListItem>
                                            <asp:ListItem>90</asp:ListItem>
                                            <asp:ListItem>180</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td width="15%">
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
                    </tr>
                </table>
                <table style="width: 100%; border-style: solid">
                    <tr>
                        <td width="8%">
                            <asp:CheckBox ID="ChkActive" Text="Active?" runat="server" />
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="Label12" runat="server" Text="Updated On:"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:Label ID="LblUpdatedOn" runat="server" Width="164px"></asp:Label>
                        </td>
                        <td width="10%" align="right">
                            <asp:Label ID="Label13" runat="server" Text="Updated By:"></asp:Label>
                        </td>
                        <td width="15%" align="left">
                            <asp:Label ID="LblUpdatedBy" runat="server"></asp:Label>
                        </td>
                        <td width="10%">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
