<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ADMS/ADMS_Master.Master"
    CodeBehind="ADMS_Home.aspx.vb" Inherits="WNB_Admin.ADMS_Home" Title="Airport Database Management System (ADMS)" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
 <div style="background-color: "AliceBlue">
      <%--  <table style="width:100%">
            <tr>
                <td style="width: 100%" align="center" valign="top">
                    <img id="Img1" src="../Images/ADMS_hdrbg_main.jpg" style="width: 100%; height: 100px;"
                        alt="Title" runat="server" />
                </td>
            </tr>
        </table>--%>
         
            
           <table style="width: 100%">
           <tr>
           <td width="95%">
            <asp:Menu ID="MnuMaiin" runat="server" Font-Names="Trebuchet MS, Arial" Orientation="Horizontal"
            Width="150px" DisappearAfter="500" Height="14" CssClass="MenuStyle1" DynamicHoverStyle-Height="10px"
            StaticDisplayLevels="1">
            <StaticMenuItemStyle BackColor="AliceBlue" ForeColor="Black" ItemSpacing="10px" VerticalPadding="2px"
                BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px" Width="190" Height="14" />
            <StaticHoverStyle BackColor="DarkOrange" ForeColor="SteelBlue" BorderStyle="Solid"
                BorderWidth="1px" />
            <DynamicMenuStyle CssClass="MenuStyle1" />
            <DynamicMenuItemStyle BackColor="AliceBlue" ForeColor="Black" VerticalPadding="5px"
                BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px" Width="190" CssClass="MenuStyle1"
                Height="10" />
            <DynamicHoverStyle BackColor="AliceBlue" ForeColor="SteelBlue" BorderStyle="Solid"
                BorderWidth="1px" />
        </asp:Menu>
           </td>
           <td width="5%">
            <%--<asp:LinkButton ID="LnkBtnLogout"  runat="server" Visible="true" Font-Names="Times New Roman"
                                Font-Size="Small" Font-Bold="true" 
            CausesValidation="false" >Logout</asp:LinkButton>--%>
           </td>
           </tr>
           </table>
       
           
    </div>
    <div>
    <table style="width: 100%">
    <tr>
    <td style="width: 1%"></td>
    <td style="width: 10%">
     <asp:Button Text="Select Airport" ID="BtnSelectAirport" runat="server" Style="font-weight: 700;
                        height: 26px;" />
    </td>
    <td style="width: 10%">
     <asp:Button Text="Show Airports On Google Earth" 
            ID="btnShowallAirportsOnGoogleEarth" runat="server" Style="font-weight: 700;
                        height: 26px;" />
    </td>
    
    <td style="width: 70%"></td>
    </tr>
    </table>
    </div>
    <div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <table style="width: 50%"; align="center">
        <tr>
        <td align="center">
             <asp:Label runat="server" ID="LblappTitle" Text="Jetstar Airport Database Management system" Font-Bold="True" Font-Size="X-Large"  ></asp:Label>
        </td>
        </tr>
            <tr>
                <td >
                    <img src="JetstarLogo.jpg" style="width: 644px; height: 121px" />
                     <asp:Label  runat="server"  ID="lblVersion" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
               
            </tr>
        </table>
        
        <table id="TblTitle" style="border-color: #C0C0C0; border-style: solid; width: 50%;"
            align="center" border="2">
            <tr style="border-width: medium; padding: inherit; height: 40px;">
                <td style="width: 10%">
                    <asp:Label runat="server" ID="LblUser" Text="User" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
                <td colspan="3" style="width: 30%;" align="center">
                    <asp:Label runat="server" ID="LblUserId" Text="" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
            </tr>
            <tr style="height: 40px;">
                <td>
                    <asp:Label runat="server" ID="Label1" Text="Next AIRAC Cycle" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
                <td align="center">
                    <asp:Label runat="server" ID="LblNextARICCycle" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label3" Text="Commencing" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
                <td align="center">
                    <asp:Label runat="server" ID="LblCommencing" Font-Bold="True" Font-Size="Larger"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
     
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
            </div>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
