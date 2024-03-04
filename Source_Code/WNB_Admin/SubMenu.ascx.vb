Public Partial Class SubMenu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim intFuncId As Integer
            If Not Request.QueryString("FID") Is Nothing Then
                intFuncId = CInt(Request.QueryString("FID").ToString())
                AddSubMenuItems(intFuncId)
            End If
        End If
    End Sub

    Private Sub AddSubMenuItems(ByVal intFuncId As String)
        Dim objMenu As New MenuItem()
        Dim dtReport As New DataTable()
        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()

        dtReport = objBo.GetReportsForFunctionality(intFuncId)

        For Each dr As DataRow In dtReport.Rows
            objMenu = New MenuItem()
            objMenu.Text = dr("Report_Title").ToString()
            objMenu.NavigateUrl = dr("Report_File_Name").ToString()
            AddSubReportGroups(objMenu, dr("Report_Id"))

            subMenu.Items.Add(objMenu)
        Next

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
                'AddSubMenuItems(lo_SubMenuItem, dtReportGroupsDT.Rows(I)("Functionality_Id"))
                po_MenuItem.ChildItems.Add(lo_SubMenuItem)


            Next
        End If
    End Sub

End Class