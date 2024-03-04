<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="WNB_Admin.Login1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Technical Support Reporting & Administrative Tool</title>
</head>
<body style="margin:0;">
    <form id="form1" runat="server" style="height: 100%">
    <table cellpadding="0" border="0" cellspacing="0" style="width: 100%;">
        <tr style="height: 100px">
            <td align="center" valign="top">
                <img id="Img1" src="~/Images/hdrbg_main.jpg" style="width: 100%; height: 100%" alt="Title"
                    runat="server" />
            </td>
        </tr>
        <tr style="height: 310px">
            <td valign="middle" align="center">
               
                <asp:Login ID="Login1" runat="server" BackColor="#EFF3FB" BorderColor="Black" BorderStyle="Double"
                    BorderWidth="1px" Font-Names="Verdana" Font-Size="Small" BorderPadding="4" ForeColor="#660033"
                    Height="150px" Width="250px" RememberMeText="&lt;font color=black&gt;Remember me next time.&lt;/font&gt;">
                    <TextBoxStyle BorderStyle="Inset" Font-Size="0.8em" Width="130px" />
                    <LoginButtonStyle BackColor="#003366" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana"
                        Font-Size="0.8em" ForeColor="White" Height="21px" />
                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                    <LabelStyle HorizontalAlign="Left" VerticalAlign="Middle" ForeColor="Black" />
                    <TitleTextStyle BackColor="#003366" Font-Bold="True" ForeColor="#FFFFFF" Font-Size="0.9em" />
                </asp:Login>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
