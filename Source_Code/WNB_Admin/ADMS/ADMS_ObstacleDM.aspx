<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_ObstacleDM.aspx.vb" Inherits="WNB_Admin.ADMS_ObstacleDM"
    Title="Obstacles Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../JS/Absenteeism/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery-ui.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.form.js" type="text/javascript"></script>

    <link href="../JS/Absenteeism/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        //        function open_Obstacle_Details_box() {
        //            $('div#DivObstacleDetails').parent().appendTo('form#ADMS_ObstacleDM');
        //            $("#DivObstacleDetails").dialog({
        //                modal: true,
        //                bgiframe: true,
        //                width: 850,
        //                height: 450,
        //                title: "Edit Obstacle Details",
        //                open: function(type, data) { $(this).parent().appendTo("form"); },
        //                showDialog: ('#DivObstacleDetails')
        //                //                buttons: {
        //                //                    'Close': function() {
        //                //                        $(this).dialog('close');
        //                //                    }
        //                //                }
        //            });
        //        };
        //        function showDialog(selector) {
        //            var source = $(selector).parent();
        //            $(selector).dialog({ open: function(type, data) { $(this).parent().appendTo(source); } });
        //        };
        //        function closeSearchDialog() {
        //            $("#DivObstacleDetails").dialog("close");
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

//            isIE = document.all ? 1 : 0
//            keyEntry = !isIE ? e.which : event.keyCode;

            keyEntry = event.keyCode || event.which;
            
            if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else {
                return false;
            }
        }
        function AllowNumericOnlyWithDecimal(e) {

//            isIE = document.all ? 1 : 0
//            keyEntry = !isIE ? e.which : event.keyCode;

            keyEntry = event.keyCode || event.which;
            if (keyEntry >= 46 && keyEntry <= 46)
                return true;
            else if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else {
                return false;
            }
        }

        function AllowNegativeValue(e) {

            //            isIE = document.all ? 1 : 0
            //            keyEntry = !isIE ? e.which : event.keyCode;
            
            keyEntry = event.keyCode || event.which;

            if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else if (keyEntry >= 45 && keyEntry <= 45)
                return true;
            else if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else if (keyEntry >= 46 && keyEntry <= 46)
                return true;
            else {
                return false;
            }
        }


        function AllowAlphaNumeric(e) {

            //            isIE = document.all ? 1 : 0
            //            keyEntry = !isIE ? e.which : event.keyCode;
            
            keyEntry = event.keyCode || event.which;
            //alert(keyEntry);
            if (keyEntry >= 48 && keyEntry <= 57)
                return true;
            else if (keyEntry >= 40 && keyEntry <= 46)
                return true;
            else if (keyEntry >= 32 && keyEntry <= 32)
                return true;
            else if (keyEntry >= 58 && keyEntry <= 58)
                return true;
            else if (keyEntry >= 65 && keyEntry <= 90)
                return true;
            else if (keyEntry >= 97 && keyEntry <= 122) {
                var mychar;
                mychar = String.fromCharCode(keyEntry);
                //mychar = mychar.toUpperCase();
                //alert(mychar);
                e.value = e.value + mychar;
                var evt = arguments[0] || event;
                evt.cancelBubble = true;
                return false;
            }
            else if (keyEntry == 39) {
                alert('This character is not allowed in this field');
                return false;
            }
        }
        function ConfirmClose() {
            return confirm("Are you sure, you want to Close Obstacle information?");
        }

        function ConfirmCancel() {
            return confirm("Are you sure, you want to cancel this operation and ready to loss all unsaved data?");
        }

        function ConfirmDelete() {
            return confirm("Are you sure, you want to delete this Obstacle data, you wouldn't be able to retrive once deleted.?");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptMgr" runat="server">
    </asp:ScriptManager>
    
    <div>
        <table id="TblTitle" style="width: 100%">
            <tr bgcolor="#DADADA">
                <td width="20%" align="center">
                    <asp:Label runat="server" ID="LblTitle" Text="Obstacle List" Font-Bold="True" Font-Size="14pt"
                        ></asp:Label>
                </td>
            </tr>
            </table>
        <br />
        <table>
            <tr>
                <td width="6%">
                    <asp:Button Text="Add Obstacle" ID="BtnCreateObstacle" runat="server" 
                        Style="font-weight: 700" />
                </td>
                <td width="4%">
                    <asp:Button Text="Plot Obstacle" ID="btnPlotObs" runat="server" 
                        Style="font-weight: 700" />
                </td>
                <td width="5%">
                    <asp:Button Text="Close" ID="btnShowRunway" runat="server" Style="font-weight: 700" />
                </td>
                <td width="5%" align="right">
                    <asp:Label ID="Label6" runat="server" Text="Runway:"></asp:Label>
                </td>
                <td width="5%" align="right">
                    <asp:TextBox ID="txtICAO_Display" MaxLength="4" ReadOnly="true" runat="server" Width="50px"></asp:TextBox>
                </td>
                <td width="3%">
                    <asp:TextBox ID="txtRwyId_Display" MaxLength="4" ReadOnly="true" runat="server" Width="50px"></asp:TextBox>
                </td>
                <td width="5%">
                    <asp:TextBox ID="txtVersion_Display" ReadOnly="true" runat="server" Width="65px"></asp:TextBox>
                </td>
                <td width="40%">
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div id="DivObstacles" style="overflow: auto; height: 220px;" runat="server">
            <asp:HiddenField ID="HdnObstacleId" runat="server" />
            <table id="TblAirports" style="width: 100%">
                <tr>
                    <td>
                        <asp:DataGrid ID="DgrdObstacles" runat="server" AutoGenerateColumns="false" Width="98%"
                            BackColor="White" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" CellPadding="2"
                            EnableViewState="true" GridLines="Both">
                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                            <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" BorderStyle="Groove" />
                            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                            <ItemStyle BackColor="White" HorizontalAlign="Right" />
                            <HeaderStyle Height="20px" BackColor="#DADADA" Font-Bold="True"  HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateColumn Visible="False" HeaderText="Obstacle_Id" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblIcao" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Obstacle_Id") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle Width="5%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Dist (m)" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDistance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Distance") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Elev (ft)" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblElevation" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Elevation") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Offset (m)" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="LblLatOffSet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"LatOffSet") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Reference">
                                    <ItemTemplate>
                                        <asp:Label ID="LblComment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ObsRef") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" Width="70%"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnEdit" Text='View' runat="server" CommandArgument='<%# "E," & DataBinder.Eval(Container.DataItem,"Obstacle_Id") %>'>
                                        </asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Button ID="BtnDelete" OnClientClick="javascript:return ConfirmDelete();" Text='Delete'
                                            runat="server" CommandArgument='<%# "D," & DataBinder.Eval(Container.DataItem,"Obstacle_Id") %>'>
                                        </asp:Button>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></ItemStyle>
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
        <%-- style="display: none"--%>
        <div id="DivObstacleDetails" runat="server">
            <table id="Table1" style="width: 98%">
                <tr>
                    <td bgcolor="#DADADA">
                        <asp:Label runat="server" ID="Label11" Text="Obstacle Details" 
                            Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnDelete" runat="server" OnClientClick="javascript:return ConfirmDelete();"
                            Style="font-weight: 700" Text="Delete" />
                        <asp:Button ID="BtnUpdate" Text="Update" runat="server" Style="font-weight: 700" />
                        <asp:Button ID="BtnCancel" Text="Cancel" runat="server" Style="font-weight: 700" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PnlObstacleDetails" runat="server">
                <table style="width: 100%; border-style: solid">
                    <tr>
                        <td style="width: 60%">
                            <table style="width: 100%">
                                <tr>
                                    <td width="9%" align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Dist (m)"></asp:Label>
                                    </td>
                                    <td width="9%">
                                        <asp:TextBox ID="TxtDist" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                                    </td>
                                    <td width="9%" align="right">
                                        <asp:Label ID="Label2" runat="server" Text="Elev (ft)"></asp:Label>
                                    </td>
                                    <td width="9%">
                                        <asp:TextBox ID="TxtElev" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                                    </td>
                                    <td width="9%" align="right">
                                        <asp:Label ID="Label3" runat="server" Text="Offset (m)"></asp:Label>
                                    </td>
                                    <td width="9%">
                                        <asp:TextBox ID="txtOffset" runat="server" MaxLength="10" Width="50px"></asp:TextBox> (Left -)
                                    </td>
                                    <td width="9%" align="right">
                                        <asp:Label ID="Label4" runat="server" Text="Reference"></asp:Label>
                                    </td>
                                    <td width="40%">
                                        <asp:TextBox ID="txtReference" runat="server" MaxLength="300" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="4" bgcolor="#DADADA">
                                        <asp:Label ID="Label8" runat="server"  Style="font-weight: 700" Text="Latitude" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%">
                                        <asp:DropDownList ID="ddlLatDir" runat="server" Width="50px">
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
                                        <asp:Label ID="Label9" runat="server"  Style="font-weight: 700" Text="Longitude" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="10%">
                                        <asp:DropDownList ID="ddlLonDir" runat="server" Width="50px">
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
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td bgcolor="#DADADA">
                                        <asp:Label ID="Label5"  Style="font-weight: 700" runat="server" Text="Comment (for selected obstacle)" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="90%">
                                        <asp:TextBox TextMode="MultiLine" ID="TxtComments" runat="server" Width="90%" Height="80px"
                                            MaxLength="500"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 1%" valign="top">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; border-style: solid">
                    <tr>
                        <td width="10%">
                            <asp:CheckBox ID="ChkActive" Text="Active?" runat="server" />
                        </td>
                        <td width="15%" align="right">
                            <asp:Label ID="Label12" runat="server" Text="Updated On:"></asp:Label>
                        </td>
                        <td width="18%">
                            <asp:Label ID="LblUpdatedOn" runat="server"></asp:Label>
                        </td>
                        <td width="18%" align="right">
                            <asp:Label ID="Label13" runat="server" Text="Updated By:"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:Label ID="LblUpdatedBy" runat="server"></asp:Label>
                        </td>
                        <td width="10%">
                            &nbsp;
                        </td>
                        <td width="10%">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
