<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubMenu.ascx.vb" Inherits="WNB_Admin.SubMenu" %>
<div>
    <asp:Menu ID="subMenu" runat="server" Font-Names="Trebuchet MS, Arial" Orientation="Vertical"
        Width="130px" DisappearAfter="50" Height="20" CssClass="MenuStyle1" DynamicHoverStyle-Height="10px"
        StaticDisplayLevels="1">
        <StaticMenuItemStyle BackColor="AliceBlue" ForeColor="Black" ItemSpacing="5px" HorizontalPadding="0px"
            BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px" Width="130" Height="20"  />
        <StaticHoverStyle BackColor="AliceBlue" ForeColor="SteelBlue" BorderStyle="Solid"
            BorderWidth="1px" />
            <DynamicMenuStyle CssClass="MenuStyle1" />
        <DynamicMenuItemStyle BackColor="AliceBlue" ForeColor="Black" VerticalPadding="0px"
            BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px" Width="130" CssClass="MenuStyle1"
            Height="20" />
        <DynamicHoverStyle BackColor="AliceBlue" ForeColor="SteelBlue" BorderStyle="Solid"
            BorderWidth="1px" />
    </asp:Menu>
</div> 