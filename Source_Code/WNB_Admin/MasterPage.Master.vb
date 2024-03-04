
Imports System.Data
Partial Public Class MasterPage1
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If


        Dim strURL As String
        Dim strURL1 As String

        strURL = DirectCast(Request, System.Web.HttpRequest).AppRelativeCurrentExecutionFilePath.ToString()
        strURL1 = strURL.Substring(strURL.LastIndexOf("/") + 1)
        If (strURL1 <> "Login.aspx") Then
            If Session("UserId") & "" <> "" Then
                LblUserDetails.Text = "Login: " + "<b>" + Session("UserId") + "</b>"
                lblUserDetails1.Text = "Base: " + "<b>" + Session("UserLocationId") + "</b>"
                If Me.IsPostBack = False Then
                    If Not Request.QueryString("FID") Is Nothing Then
                        hidFunctionId.Value = CInt(Request.QueryString("FID").ToString())
                    End If
                    CreateMenus()
                    LnkBtnLogout.Visible = True
                    LnkChangePwd.Visible = True
                End If
            Else
                LblUserDetails.Text = ""
                Response.Redirect("~/Login.aspx")
            End If
        End If

    End Sub

    Protected Sub ImgBtnLogout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnLogout.Click
        Session("UserId") = Nothing

        Session.Abandon()
        LblUserDetails.Text = ""
        Response.Redirect("~/Login.aspx")

    End Sub

    Private Function GetSubFunctionalities(ByVal ParentId As String) As DataTable
        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()
        Return objBo.GetFunctionalities(ParentId)

    End Function
    Private Function GetReportsForFunctionality(ByVal FunctionalityId As String) As DataTable
        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()
        Return objBo.GetReportsForFunctionality(FunctionalityId)
    End Function
    Private Sub AddSubMenuItems(ByRef MnuItem As MenuItem, ByVal FunctionalityId As String)
        Dim subMenu As New MenuItem()
        Dim dtReport As New DataTable()

        dtReport = GetReportsForFunctionality(FunctionalityId)
        Dim dtReportsTemp As New DataTable()
        dtReportsTemp = dtReport.DefaultView.ToTable()
        For Each dr As DataRow In dtReportsTemp.Rows
            subMenu = New MenuItem()
            subMenu.Text = dr("Report_Title").ToString()
            subMenu.NavigateUrl = dr("Report_File_Name").ToString()
            MnuItem.ChildItems.Add(subMenu)

        Next
    End Sub

    Private Sub CreateMenus()
        Try
            Dim dtTopReportGroupsDT As DataTable
            Dim objBO As New WNB_Admin_BO.WNB_Admin_BO()
            Dim I As Integer

            dtTopReportGroupsDT = objBO.GetFunctionalities("")

            Dim lo_Home As New MenuItem
            lo_Home.Text = "Home"
            lo_Home.NavigateUrl = "Home.aspx"
            Menu1.Items.Add(lo_Home)


            For I = 0 To dtTopReportGroupsDT.Rows.Count - 1
                Dim lo_MenuItem As New MenuItem

                lo_MenuItem.Text = dtTopReportGroupsDT.Rows(I)("Functionality_Name")
                lo_MenuItem.Selectable = False
                lo_MenuItem.NavigateUrl = dtTopReportGroupsDT.Rows(I)("Functionality_Url").ToString()
                
                AddSubReportGroups(lo_MenuItem, dtTopReportGroupsDT.Rows(I)("Functionality_Id"))
                If dtTopReportGroupsDT.Rows(I)("IsSubMenu").ToString.Trim.Equals("1") Then
                    AddSubMenuItems(lo_MenuItem, dtTopReportGroupsDT.Rows(I)("Functionality_Id"))
                End If
                Menu1.Items.Add(lo_MenuItem)
            Next

            'Dim lo_Administration As New MenuItem
            'Dim lo_RoleManagement As New MenuItem
            'Dim lo_UserManagement As New MenuItem
            'Dim lo_CSOUserManagement As New MenuItem

            'lo_Administration.Text = "Administration"
            'lo_Administration.Selectable = False

            'lo_RoleManagement.Text = "User Management"
            'lo_RoleManagement.NavigateUrl = "Admin/UserManagement.aspx"

            'lo_UserManagement.Text = "Role Management"
            'lo_UserManagement.NavigateUrl = "Admin/RoleManagement.aspx"

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'lo_Administration.ChildItems.Add(lo_UserManagement)
            'lo_Administration.ChildItems.Add(lo_RoleManagement)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Menu1.Items.Add(lo_Administration)

        Catch ex As Exception

        End Try
    End Sub


    Private Sub AddSubReportGroups(ByRef po_MenuItem As Global.System.Web.UI.WebControls.MenuItem, _
                                   ByVal ReportGroupId As Integer)

        Dim dtReportGroupsDT As DataTable

        Dim objBO As New WNB_Admin_BO.WNB_Admin_BO()
        Dim I As Integer

        dtReportGroupsDT = objBO.GetFunctionalities(ReportGroupId)

        If dtReportGroupsDT.Rows.Count = 0 Then
            po_MenuItem.Selectable = True

        Else
            For I = 0 To dtReportGroupsDT.Rows.Count - 1
                Dim lo_SubMenuItem As New MenuItem

                lo_SubMenuItem.Selectable = False
                lo_SubMenuItem.Text = dtReportGroupsDT.Rows(I)("Functionality_Name")

                AddSubReportGroups(lo_SubMenuItem, dtReportGroupsDT.Rows(I)("Functionality_Id"))
                AddSubMenuItems(lo_SubMenuItem, dtReportGroupsDT.Rows(I)("Functionality_Id"))
                po_MenuItem.ChildItems.Add(lo_SubMenuItem)


            Next
        End If
    End Sub

    Protected Sub LnkChangePwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkChangePwd.Click
        Response.Redirect("~/ChangePwd.aspx")
    End Sub

End Class