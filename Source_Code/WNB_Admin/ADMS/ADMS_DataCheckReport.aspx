<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ADMS_DataCheckReport.aspx.vb"
    Inherits="WNB_Admin.ADMS_DataCheckReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Check Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="TblTitle" style="width: 100%">
            <tr bgcolor="#DADADA">
                <td align="center">
                    <asp:Label runat="server" ID="LblTitle" Text="Data Check Report" Font-Bold="True"
                        Font-Size="Larger" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <table id="Table4" style="width: 100%; border-style: solid;">
        <tr>
            <td width="10%">
                <asp:Button ID="btnClose" Text="Close" runat="server" Style="font-weight: 700" />
             <asp:Button ID="btnExportErrors" Text="Export Errors" runat="server" 
                    Style="font-weight: 700" />
                <asp:Button ID="btnExportInactiveRecords" Text="Export Inactive Records" runat="server" 
                    Style="font-weight: 700" />  
            </td>
             
        </tr>
    </table>
    <br />
     <table id="Table1" style="width: 100%">
                    <tr align="left">
                        <td align="left" style="width: 20%">
                            <asp:Label ID="Label1"  runat="server" Font-Bold="False" Font-Size="12pt" Text="Data Check Error File Path:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="LblErrorFilePath" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
                        </td>
                    </tr>
                     <tr align="left">
                        <td align="left" style="width: 20%">
                            <asp:Label ID="Label2"  runat="server" Font-Bold="False" Font-Size="12pt" Text="Data Check Inactive Records File Path:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="LblInactiveFilePath" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
                        </td>
                    </tr>
                </table>
                 <br />
                 <hr />
    <div id="DivUpper" style="overflow: auto; width: 100%; height: 490px;" runat="server">
        <asp:DataGrid ID="DgrdDataCheckReport" runat="server" AutoGenerateColumns="False"
            Width="90%" BackColor="White" BorderStyle="None" BorderWidth="0px"
            CellPadding="0" ShowHeader="False" GridLines="None">
            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
            <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" BorderStyle="Groove" />
            <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
            <ItemStyle BackColor="White" HorizontalAlign="Left" />
            <HeaderStyle Height="20px" BackColor="Gray" Font-Bold="True" ForeColor="white" HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateColumn  HeaderText="ICAO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="LblIcao" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ICAO") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="5%"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="CITY" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label ID="LblCity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"City") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="5%"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn  HeaderText="RwyId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="LblRwyId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyId") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="5%"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn  HeaderText="RwyMod" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="LblRwyMod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RwyMod") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="5%"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                    <ItemTemplate >
                        <asp:Label ID="LblIntersectionOrObstacleData" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"IntersectionOrObstacleData") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle Width="5%"></ItemStyle>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%">
                    <ItemTemplate>
                        <asp:Label ID="LblErrDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ErrDescription") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    <ItemStyle></ItemStyle>
                </asp:TemplateColumn>
            </Columns>
            <ItemStyle HorizontalAlign="Left" />
            <HeaderStyle CssClass="Freezing" Font-Names="Times New Roman" Font-Size="Medium" />
        </asp:DataGrid>
    </div>
    </form>
</body>
</html>
