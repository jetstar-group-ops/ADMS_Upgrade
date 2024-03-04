Imports WNB_Admin_BO

Partial Public Class ADMS_CRR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.ADMS, "", 0) = False Then

                lsMessage = "You don't have permission on Airport Database Management System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            If Me.IsPostBack = False Then
                BtnExportReport.Enabled = False

                txtToDate.Text = Date.Now.ToString("dd-MMM-yyyy")
                txtFromDate.Text = Date.Now.AddMonths(-1).ToString("dd-MMM-yyyy")

                LstExportFormats.Items.Add("Portable Document Format")
                LstExportFormats.Items(LstExportFormats.Items.Count - 1).Value = _
                        CrystalDecisions.Shared.ExportFormatType.PortableDocFormat & ",.pdf"

                LstExportFormats.Items.Add("Rich Text Format")
                LstExportFormats.Items(LstExportFormats.Items.Count - 1).Value = _
                        CrystalDecisions.Shared.ExportFormatType.RichText & ",.rtf"

                LstExportFormats.Items.Add("Excel Format")
                LstExportFormats.Items(LstExportFormats.Items.Count - 1).Value = _
                        CrystalDecisions.Shared.ExportFormatType.Excel & ",.xls"
            Else

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while generating change record report. \n" & _
                   ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub BtnGenerateReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerateReport.Click
        Try
            GenerateCRReport()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while generating change record report. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub GenerateCRReport()

        Dim lsErrMsg As String = ""

        If txtFromDate.Text.Trim = "" Then
            lsErrMsg = "Please specify start date for the report."
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
             "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)

            Exit Sub
        End If

        If txtToDate.Text.Trim = "" Then
            lsErrMsg = "Please specify end date for the report."
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
             "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)

            Exit Sub
        End If

        Dim loCRReport As DataSet
        Dim loADMS_BAL_CRR As New ADMS_BAL_CRR

        loCRReport = loADMS_BAL_CRR.GetChangeRequestReport(Session("UserId"), _
                    txtFromDate.Text, txtToDate.Text & " 23:59:59")

        loCRReport.Tables(0).TableName = "TblChangeRecordReport"

        Dim loMyCRReport As New CRReport

        loMyCRReport.SetDataSource(loCRReport)
        loMyCRReport.SetParameterValue("ReportTitle", "Jetstar records for the following airports have been changed between " & _
                txtFromDate.Text & " and " & txtToDate.Text & " inclusive.")

        CrvChangeRequestReport.ReuseParameterValuesOnRefresh = True
        CrvChangeRequestReport.ReportSource = loMyCRReport

        Dim lsExportFileName As String = ""
        lsExportFileName = Request.MapPath(Request.ApplicationPath) & "\Temp\" & _
            Session("UserId") & "_CRReport" & _
            LstExportFormats.SelectedValue.ToString.Split(",")(1)

        loMyCRReport.ExportToDisk(LstExportFormats.SelectedValue.ToString.Split(",")(0), lsExportFileName)

        HdnExportFileName.Value = "..\Temp\" & _
            Session("UserId") & "_CRReport" & _
            LstExportFormats.SelectedValue.ToString.Split(",")(1)

        BtnExportReport.Enabled = True
    End Sub

    Private Sub BtnExportReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportReport.Click
        GenerateCRReport()
        Response.Redirect(HdnExportFileName.Value)
    End Sub

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Response.Redirect("ADMS_Home.aspx")
    End Sub

    Private Sub CrvChangeRequestReport_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles CrvChangeRequestReport.Navigate
        Try
            GenerateCRReport()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while generating change record report. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub
End Class