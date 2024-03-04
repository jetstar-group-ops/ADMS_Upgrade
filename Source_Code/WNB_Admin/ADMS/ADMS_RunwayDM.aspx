<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_RunwayDM.aspx.vb" Inherits="WNB_Admin.ADMS_RunwayDM"
    Title="Runway Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 540px;
        }
        .style2
        {
            width: 194px;
        }
        .style9
        {
            width: 541px;
        }
        .style10
        {
            height: 409px;
        }
        .style13
        {
            height: 68px;
        }
        .style14
        {
            height: 15px;
        }
    </style>

    <script src="../JS/Absenteeism/jquery-1.4.4.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery-ui.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../JS/Absenteeism/jquery.form.js" type="text/javascript"></script>

    <link href="../JS/Absenteeism/jquery-ui-1.8.6.custom.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">


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
                alert('This character is not allowed in this field');
                return false;
            }
        }

        function checkDecimal(el) {
            var ex = /^[0-9]+\.?[0-9]{0,1}$/;
            if (ex.test(el.value) == false) {
                //return false;
                alert('Decimal Value upto 2 places allowed !');
                //el.value = '';
            }

        }
        function IsOneDecimalPoint(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 1 && charCode == 46)
                return false;
            return true;
        }






        function ConfirmClose() {
            return confirm("Are you sure, you want to Close Runway information?");
        }
        function ConfirmUpdate() {
            return confirm("Are you sure, you want to Update Runway information?");
        }

        function ConfirmDelete() {
            return confirm("Are you sure, you want to delete this Runway data, you wouldn't be able to retrive once deleted.?");
        }
    </script>

    <%-- <style type="text/css">
        .style1
        {
            height: 55px;
        }
        .style2
        {
            height: 251px;
        }
        .style3
        {
            height: 381px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptMgr" runat="server">
    </asp:ScriptManager>
    <div>
        <table id="TblTitle" style="width: 100%">
            <tr bgcolor="#DADADA">
                <td width="60%" align="center">
                    <asp:Label runat="server" ID="LblTitle" Text="Runway Details" Font-Bold="True" Font-Size="14pt"></asp:Label>
                </td>
            </tr>
            <tr><td style="height:5px"></td></tr>
            <tr bgcolor="#DADADA">
                <td width="60%">
                    <asp:Button Text="Show Obstacles" ID="btnShowObstacle" runat="server" 
                        Style="font-weight: 700" />
                    <asp:Button Text="Add Intersection" ID="btnAddIntersection" runat="server" Style="font-weight: 700" />
                    <asp:Button Text="Show Splay on Google Earth" ID="btnSplay" runat="server" Style="font-weight: 700" />
                    <asp:Button ID="btnPrintStd" runat="server" Text="Print" 
                        Style="font-weight: 700" />
                    <asp:Button Text="Close" ID="btnClose" runat="server" Visible="false" Style="font-weight: 700" />
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <asp:Panel ID="PnlRunwayDetails" runat="server">
        <table style="width: 100%; border-style: solid">
            <tr>
                <td width="15%">
                    <asp:Button ID="btnDelete" runat="server" OnClientClick="javascript:return ConfirmDelete();"
                        Style="font-weight: 700" Text="Delete" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" Style="font-weight: 700" />
                    <asp:Button ID="btnCancel" runat="server" Style="font-weight: 700" 
                        Text="Close" />
                </td>
            </tr>
        </table>
        <hr />
        <table style="width: 100%; border-style: solid">
            <tr>
                <td style="width: 60%">
                    <table style="width: 100%">
                        <tr>
                            <td width="15%" align="center">
                                <asp:Label ID="lblICAO" runat="server" Text="ICAO" Font-Bold="True"></asp:Label>
                            </td>
                            <td width="5%" align="right">
                                <asp:Label ID="lblRunway" runat="server" Text="Runways:"></asp:Label>
                            </td>
                            <td width="5%">
                                <asp:TextBox ID="txtRunway" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="5%" align="right">
                                <asp:Label ID="lblVersion" runat="server" Text="Version:"></asp:Label>
                            </td>
                            <td width="5%">
                                <asp:TextBox ID="txtVersion" Width="75px" runat="server"></asp:TextBox>
                            </td>
                            <td width="5%" align="right">
                                <asp:Label ID="lblObstacle" runat="server" Text="# Obstacle:-"></asp:Label>
                            </td>
                            <td width="10%">
                                <asp:HyperLink ID="HypLnkObstacle" Target="_self" runat="server" NavigateUrl="~/ADMS/ADMS_ObstacleDM.aspx">Obstacle</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="lblId" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblTora" runat="server" Text="TORA(m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblToda" runat="server" Text="TODA(m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblASDA" runat="server" Text="ASDA(m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblLDA" runat="server" Text="LDA(m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblSlope" runat="server" Text="Slope(%)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblWidth" runat="server" Text="Width(m)"></asp:Label>
                            </td>
                             <td width="15%">
                                 <asp:Label ID="lblExportApplic"  Visible="true" runat="server" Text="Export Applic"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <asp:TextBox ID="txtTora" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtToda" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtASDA" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtLDA" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtSlope" runat="server" Width="49px"></asp:TextBox>
                                <asp:DropDownList ID="ddlSlop" runat="server">
                                    <asp:ListItem Value="Up" Text="Up" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="Dn" Text="Dn" Selected="False"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtWidth" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:DropDownList Visible="true" ID="ddlExportApplic" runat="server" Width="180px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label1" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblSotElev" runat="server" Text="SOT Elev(ft)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblDispThr" runat="server" Text="Disp Thr (m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblDispTo" runat="server" Text="Disp TO (m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblResaTo" runat="server" Text="RESA TO (m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblResaLand" runat="server" Text="RESA LAND (m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblShoulder" runat="server" Text="Shoulder (m)"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="lblArctApplic" runat="server" Text="AirportMax Applic"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <asp:TextBox ID="txtSotElev" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtDispThr" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtDispTo" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtResaTo" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResaLand" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="15%">
                                <asp:TextBox ID="txtShoulder" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:DropDownList ID="ddlAcftApplic" runat="server" Width="180px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label2" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td width="10%">
                                <asp:Label ID="lblHdg" runat="server" Text="Hdg"></asp:Label>
                            </td>
                            <td width="10%">
                                <asp:Label ID="lblLineUpAngle" runat="server" Text="Lineup Angle"></asp:Label>
                            </td>
                            <td width="10%">
                                <asp:Label ID="lblGA" runat="server" Text="G/A (%)"></asp:Label>
                            </td>
                            <td width="30%">
                                <asp:Label ID="lblLatitude" runat="server" Text="Latitude"></asp:Label>
                            </td>
                            <td width="30%">
                                <asp:Label ID="lblLongitude" runat="server" Text="Longitude"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                                <asp:TextBox ID="txtHdg" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="10%">
                                <asp:DropDownList ID="ddlLineupAngle" runat="server">
                                    <asp:ListItem Value="0" Text="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="90" Text="90"> </asp:ListItem>
                                    <asp:ListItem Value="180" Text="180"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td width="10%">
                                <asp:TextBox ID="txtGA" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="25%">
                                <asp:DropDownList ID="ddlLatitude" runat="server">
                                    <asp:ListItem Value="S" Text="S" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="N"> </asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtLatDeg" runat="server" Width="49px"></asp:TextBox>
                                <asp:TextBox ID="txtLatMin" runat="server" Width="49px"></asp:TextBox>
                                <asp:TextBox ID="txtLatSec" runat="server" Width="49px"></asp:TextBox>
                            </td>
                            <td width="25%">
                                <asp:DropDownList ID="ddlLongitude" runat="server">
                                    <asp:ListItem Value="E" Text="E" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="W" Text="W"> </asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtLonDeg" runat="server" Width="49px"></asp:TextBox>
                                <asp:TextBox ID="txtLonMin" runat="server" Width="49px"></asp:TextBox>
                                <asp:TextBox ID="txtLonSec" runat="server" Width="49px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label3" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblTakeOffAllEngines" runat="server" Text="Takeoff - All Engines Procedure"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:TextBox ID="txtTakeOffAllEngines" runat="server" TextMode="MultiLine" Width="715px"
                                    Height="55px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label7" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td class="style1" style="width: 100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td width="30%">
                                            <asp:Label ID="lblTakeOffEngineFailure" Width="300px" runat="server" Text="Takeoff - Engine-Failure Procedure"></asp:Label>
                                        </td>
                                        <td width="30%">
                                        </td>
                                        <td width="40%" align="right">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:TextBox ID="txtTakeOffEngineFailure"  runat="server" TextMode="MultiLine" Width="715px"
                                    Height="55px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label4" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td width="50%">
                                <asp:Label ID="lblTakeOffPostProcedure" Width="645px" runat="server" Text="Take-off - Post Procedure"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="50%">
                                <asp:TextBox ID="txtPostProc" runat="server" TextMode="MultiLine" Width="715px" Height="55px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label5" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style1" style="width: 300px">
                                <table style="width: 100%">
                                    <tr>
                                        <td width="35%">
                                            <asp:Label ID="lbl" Width="140px" runat="server" Text="Takeoff - Comment"></asp:Label>
                                        </td>
                                        <td width="35%">
                                            <asp:CheckBox ID="chkExpRnw" Width="269px" Visible="false" runat="server" Text="Export Rnwy for intersection Rnws?" />
                                        </td>
                                        <td width="35%">
                                            <asp:CheckBox ID="chkDontAppendTO" Width="286px" Visible="false" runat="server" Text="Don't append T/O Comment to EOP?" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:TextBox ID="txtTakeOffComment" runat="server" TextMode="MultiLine" Width="715px"
                                    Height="55px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td bgcolor="#DADADA">
                                <asp:Label ID="Label6" Height="3Px" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table style="width: 100%;">
                        <tr>
                            <td width="15%">
                                <asp:Label ID="lblLandingComment" Width="645px" runat="server" Text="Landing - Comment"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <asp:TextBox ID="txtLandingCommment" runat="server" TextMode="MultiLine" Width="715px"
                                    Height="55px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td width="15%">
                                <asp:CheckBox ID="chkActive" runat="server" Text="Active ?" />
                            </td>
                            <td width="15%" align="right">
                                <asp:Label ID="lblUpdatedOnLable" runat="server" Text="Updated On:"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="LblUpdatedOn" runat="server"></asp:Label>
                            </td>
                            <td width="15%" align="right">
                                <asp:Label ID="lblUpdatedByLable" runat="server" Text="Updated By:"></asp:Label>
                            </td>
                            <td width="15%">
                                <asp:Label ID="LblUpdatedBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="style9">
                    <table style="width: 100%; height: 873px;">
                        <tr>
                            <td class="style13">
                                <asp:DataGrid ID="DgrdIntersection" runat="server" AutoGenerateColumns="false" Width="20%"
                                    BackColor="White" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" CellPadding="2"
                                    EnableViewState="true" GridLines="Both">
                                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" BorderStyle="Groove" />
                                    <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                    <ItemStyle BackColor="White" HorizontalAlign="Left" />
                                    <HeaderStyle Height="20px" BackColor="#DADADA" Font-Bold="True" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateColumn Visible="False" HeaderText="Obstacle_Id" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInterectionId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Intersection_Id") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn Visible="False" HeaderText="Obstacle_Id" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblICAO" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ICAO") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn Visible="False" HeaderText="Obstacle_Id" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRwyId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyId") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn Visible="False" HeaderText="Obstacle_Id" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRnwMod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyMod") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle Width="5%"></ItemStyle>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Intersections" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" ID="HlnlRunway" Target="_self" Text='<%# DataBinder.Eval(Container.DataItem,"Ident") %>'
                                                    NavigateUrl='<%# "ADMS_IntersectionDM.aspx?Intersection_Id=" & DataBinder.Eval(Container.DataItem,"Intersection_Id") & "&RwyId=" & DataBinder.Eval(Container.DataItem,"RwyId") & "&Icao="  & DataBinder.Eval(Container.DataItem,"ICAO") & "&RwyMod=" & DataBinder.Eval(Container.DataItem,"RwyMod")  %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <%--<HeaderStyle CssClass="Freezing" Font-Names="Times New Roman" Font-Size="Medium" />--%>
                                </asp:DataGrid>
                            </td>
                            <td class="style13">
                            </td>
                            <td class="style13">
                            </td>
                        </tr>
                        <tr>
                            <td id="tdIntersectionData" style="height: 20px;" runat="server" visible="false"
                                class="style14">
                                <asp:Label ID="lblNoIntersectionData" Font-Bold="true" Visible="false" Height="3Px"
                                    runat="server" Text="No Intersection data" ForeColor="White"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="style10">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
