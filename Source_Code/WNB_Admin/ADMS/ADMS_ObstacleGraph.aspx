<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_ObstacleGraph.aspx.vb" Inherits="WNB_Admin.ADMS_ObstacleGraph"
    Title="Obstacle Plot" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
      function PrintObstacleGraph() {
         window.print();
      }
  </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table id="Table1" style="width: 100%">
            <tr bgcolor="#999999" align="center">
                <td>
                    <asp:Label runat="server" ID="LblTitle" Text="Obstacle Plot" Font-Bold="True" Font-Size="Large"  
                        ForeColor="White"></asp:Label>
                </td>
            </tr>
        </table>
        <table id="Table2" style="width: 100%">
            <tr>
                <td width="5%">
                    <asp:Button Text="Print Plot" ID="btnPrintPlot" runat="server" OnClientClick="javascript:return PrintObstacleGraph();" Style="font-weight: 700" />
                </td>
                <td width="5%">
                    <asp:Button Text="Close" ID="btnShowObstacle" runat="server" Style="font-weight: 700" />
                </td>
                <td style="width: 5%" align="right">
                    <asp:Label ID="Label1" Text="ICAO: " runat="server"></asp:Label>
                </td>
                <td style="width: 5%">
                    <asp:Label ID="lblICAO" runat="server"></asp:Label>
                </td>
                <td style="width: 5%" align="right">
                    <asp:Label ID="Label2" Text="Runway: " runat="server"></asp:Label>
                </td>
                <td style="width: 5%">
                    <asp:Label ID="lblRwyId" runat="server"></asp:Label>
                </td>
                <td style="width: 5%" align="right">
                    <asp:Label ID="Label3" Text="Version: " runat="server"></asp:Label>
                </td>
                <td style="width: 5%">
                    <asp:Label ID="lblRwyMod" runat="server"></asp:Label>
                </td>
                <td style="width: 5%" align="right">
                    <asp:Label ID="Label4" Text="Airline: " runat="server"></asp:Label>
                </td>
                <td style="width: 5%">
                    <asp:Label ID="lblAirlineCode" runat="server"></asp:Label>
                </td>
                <td style="width: 10%">
                    <asp:Label ID="lblGreen" runat="server" Width="25px" Text="   " BackColor="Green" >&nbsp;</asp:Label>
                    <asp:Label ID="Label5" runat="server" Text="Inactive Obstacles"></asp:Label>
                </td>
                <td style="width: 20%">
                    <asp:Label ID="lblRed" runat="server" Text="   "  Width="25px" BackColor="Red" >&nbsp;</asp:Label>
                    <asp:Label ID="Label6" runat="server" Text="Active Obstacles"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <hr />
    <div>
        <table id="TblTitle" style="width: 100%; height: 100%;">
            <tr>
                <td width="100%">
                    <asp:Chart ID="ChartObstaclePlot" Width="800px" runat="server" Height="600px" 
                        BorderlineColor="Black">
                        <BorderSkin BorderWidth="2" />
                    </asp:Chart>
                </td>
            </tr>
        </table>
    </div>
    <%--Below Chart is for reference of developing the above chart. Please do not Delete Chart1--%>
    <div>
        <asp:Chart ID="Chart1" runat="server" Visible="False" Width="527px">
            <BorderSkin SkinStyle="Raised" />
            <Titles>
                <asp:Title Name="Title1HelloTitle">
                </asp:Title>
            </Titles>
            <Series>
                <asp:Series ChartArea="ChartArea1" CustomProperties="PointWidth=0.01" MarkerSize="2"
                    Name="Series1" Color="Red" Legend="Legend1">
                    <Points>
                        <asp:DataPoint XValue="4330" YValues="0" />
                        <asp:DataPoint XValue="4330" YValues="500" />
                        <asp:DataPoint XValue="11000" YValues="0" />
                        <asp:DataPoint XValue="11000" YValues="750" />
                    </Points>
                </asp:Series>
                <asp:Series ChartArea="ChartArea1" ChartType="Line" Name="Series2" XAxisType="Secondary"
                    YAxisType="Secondary" Color="0, 0, 192" Font="Microsoft Sans Serif, 1pt" 
                    Legend="Legend1">
                    <Points>
                        <asp:DataPoint YValues="1200" BorderWidth="1" Color="White" MarkerSize="5" />
                        <asp:DataPoint YValues="1050" XValue="3350" />
                    </Points>
                    <EmptyPointStyle Color="Blue" />
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea BorderDashStyle="Solid" Name="ChartArea1">
                    <AxisY Maximum="1500" Minimum="200" IsMarksNextToAxis="False">
                        <MajorGrid Enabled="False" />
                    </AxisY>
                    <AxisX Maximum="25000" Minimum="0" Title="Hello" 
                        LineDashStyle="NotSet">
                        <MajorGrid Enabled="False" LineColor="DimGray" />
                        <MajorTickMark Enabled="False" />
                    </AxisX>
                    <AxisX2 Maximum="25000" Minimum="0" TitleForeColor="Transparent" 
                        IsStartedFromZero="False">
                        <MajorGrid Enabled="False" />
                        <LabelStyle ForeColor="White" />
                    </AxisX2>
                    <AxisY2 IsReversed="True" Maximum="1500" Minimum="0" Title="y title " 
                        TitleForeColor="Transparent">
                        <MajorGrid Enabled="False" />
                    </AxisY2>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
</asp:Content>
