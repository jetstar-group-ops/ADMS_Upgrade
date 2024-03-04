
Imports CrystalDecisions.CrystalReports.Engine
Imports System.IO
Imports System.Web.Configuration
Imports CrystalDecisions.Shared
Imports WNB_Admin_BO

Partial Public Class ADMS_TextReportPrint
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
                LstExportFormats.Items.Add("Portable Document Format")
                LstExportFormats.Items(LstExportFormats.Items.Count - 1).Value = _
                        CrystalDecisions.Shared.ExportFormatType.PortableDocFormat & ",.pdf"

                LstExportFormats.Items.Add("Rich Text Format")
                LstExportFormats.Items(LstExportFormats.Items.Count - 1).Value = _
                        CrystalDecisions.Shared.ExportFormatType.RichText & ",.rtf"

                LstExportFormats.Items.Add("Excel Format")
                LstExportFormats.Items(LstExportFormats.Items.Count - 1).Value = _
                        CrystalDecisions.Shared.ExportFormatType.Excel & ",.xls"

                lblICAO.Text = Request("Icao") & ""
                lblRwyId.Text = Request("RwyId") & ""
                lblRwyMod.Text = Request("RwyMod") & ""
                lblAirlineCode.Text = Session("Airlinecode")
                GenerateReport()

                'GenerateReport_Test()
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while getting Print (Std) report. \n" & ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Public Sub GenerateReport()
        Try
            'Dim cryRpt As New ReportDocument
            'Dim cryRpt As New TextFileReport
            Dim myLogonInfo As New TableLogOnInfo()

            'Dim DbUserName As String
            'Dim DbUserPwd As String
            'Dim DbName As String
            'Dim DbServerName As String
            'Dim loMyCRReport As New TextFileReport
            'Dim rptPath As String

            'rptPath = Request.MapPath(Request.ApplicationPath)
            'cryRpt.Load(Request.MapPath("..\ADMS\TextFileReport.rpt"))
            'DbServerName = System.Configuration.ConfigurationManager.AppSettings("DbServerName")
            'DbName = System.Configuration.ConfigurationManager.AppSettings("DbName")
            'DbUserName = System.Configuration.ConfigurationManager.AppSettings("DbUserName")
            'DbUserPwd = System.Configuration.ConfigurationManager.AppSettings("DbUserPwd")

            'cryRpt.SetDatabaseLogon(DbUserName, DbUserPwd)

            'For Each myTable In cryRpt.Database.Tables
            '    myLogonInfo = myTable.LogOnInfo
            '    myLogonInfo.ConnectionInfo.ServerName = DbServerName
            '    myLogonInfo.ConnectionInfo.DatabaseName = DbName
            '    If DbUserName = "" Then
            '        myLogonInfo.ConnectionInfo.IntegratedSecurity = True
            '    Else
            '        myLogonInfo.ConnectionInfo.UserID = DbUserName
            '        myLogonInfo.ConnectionInfo.Password = DbUserPwd
            '        myLogonInfo.ConnectionInfo.IntegratedSecurity = False
            '    End If
            '    myTable.ApplyLogOnInfo(myLogonInfo)
            'Next myTable

            Dim loDataSetForMainReport As DataSet
            Dim loADMS_BAL_TextReportPrint As New ADMS_BAL_TextReportPrint

            loDataSetForMainReport = loADMS_BAL_TextReportPrint.GetChangeRequestReport(Session("UserId"), _
                                    lblICAO.Text, lblRwyId.Text, lblRwyMod.Text, lblAirlineCode.Text)

            loDataSetForMainReport.Tables(0).TableName = "Airport"
            loDataSetForMainReport.Tables(1).TableName = "Runway"
            loDataSetForMainReport.Tables(2).TableName = "Obstacle"
            loDataSetForMainReport.Tables(3).TableName = "Intersection"

            Dim loMyTxtFilePrntReport As New TextfilePrintReport
            loMyTxtFilePrntReport.SetDataSource(loDataSetForMainReport)

            'loMyTxtFilePrntReport.Subreports(0).SetDataSource(loDataSetForMainReport.Tables(1))
            'loMyTxtFilePrntReport.Subreports(1).SetDataSource(loDataSetForMainReport.Tables(2))


            'loMyTxtFilePrntReport.SetParameterValue("ReportTitle", "Jetstar records for the following airports have been changed between " & _
            ' txtFromDate.Text & " and " & txtToDate.Text & " inclusive.")

            'loMyTxtFilePrntReport.SetParameterValue("ICAO", lblICAO.Text)
            'loMyTxtFilePrntReport.SetParameterValue("RwyId", lblRwyId.Text)
            'loMyTxtFilePrntReport.SetParameterValue("RwyMod", lblRwyMod.Text)
            'loMyTxtFilePrntReport.SetParameterValue("AirlineCode", lblAirlineCode.Text)


            'cryRpt.SetDataSource(loDataSetForMainReport.Tables(0))
            'cryRpt.SetParameterValue("ICAO", lblICAO.Text)
            'cryRpt.SetParameterValue("RwyId", lblRwyId.Text)
            'cryRpt.SetParameterValue("RwyMod", lblRwyMod.Text)
            'cryRpt.SetParameterValue("AirlineCode", lblAirlineCode.Text)

            'cryRpt.Subreports(0).SetDataSource(loDataSetForMainReport.Tables(1))
            'cryRpt.Subreports(1).SetDataSource(loDataSetForMainReport.Tables(2))

            'cryRpt.Subreports(0).Load(Request.MapPath("..\ADMS\ObstacleSubReport.rpt"))
            'For Each myTable In cryRpt.Subreports(1).Database.Tables
            'myLogonInfo = myTable.LogOnInfo
            'myLogonInfo.ConnectionInfo.ServerName = DbServerName
            'myLogonInfo.ConnectionInfo.DatabaseName = DbName
            'If DbUserName = "" Then
            '    myLogonInfo.ConnectionInfo.IntegratedSecurity = True
            'Else
            '    myLogonInfo.ConnectionInfo.UserID = DbUserName
            '    myLogonInfo.ConnectionInfo.Password = DbUserPwd
            '    myLogonInfo.ConnectionInfo.IntegratedSecurity = False
            'End If
            'myTable.ApplyLogOnInfo(myLogonInfo)
            ' Next myTable

            CRV_TextReportPrint.ReuseParameterValuesOnRefresh = True
            CRV_TextReportPrint.ReportSource = loMyTxtFilePrntReport

            'Dim loTextFileReport As New TextFileReport

            'loTextFileReport.SetParameterValue("ICAO", lblICAO.Text)
            'loTextFileReport.SetParameterValue("RwyId", lblRwyId.Text)
            'loTextFileReport.SetParameterValue("RwyMod", lblRwyMod.Text)
            'loTextFileReport.SetParameterValue("AirlineCode", lblAirlineCode.Text)

            Dim lsExportFileName As String = ""
            lsExportFileName = Request.MapPath(Request.ApplicationPath) & "\Temp\" & _
                Session("UserId") & "_PrintStd" & _
                LstExportFormats.SelectedValue.ToString.Split(",")(1)

            loMyTxtFilePrntReport.ExportToDisk(LstExportFormats.SelectedValue.ToString.Split(",")(0), lsExportFileName)

            HdnExportFileName.Value = "..\Temp\" & _
                Session("UserId") & "_PrintStd" & _
                LstExportFormats.SelectedValue.ToString.Split(",")(1)

            BtnExportReport.Enabled = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GenerateReport_Test()
        Try
            'Dim cryRpt As New ReportDocument
            Dim myLogonInfo As New TableLogOnInfo()

            Dim loMyCRReport As New TextFileReport

            Dim DbUserName As String
            Dim DbUserPwd As String
            Dim DbName As String
            Dim DbServerName As String
            'Dim rptPath As String

            'rptPath = Request.MapPath(Request.ApplicationPath)

            loMyCRReport.Load(Request.MapPath("..\ADMS\TextFileReport.rpt"))
            DbServerName = System.Configuration.ConfigurationManager.AppSettings("DbServerName")
            DbName = System.Configuration.ConfigurationManager.AppSettings("DbName")
            DbUserName = System.Configuration.ConfigurationManager.AppSettings("DbUserName")
            DbUserPwd = System.Configuration.ConfigurationManager.AppSettings("DbUserPwd")

            'cryRpt.SetDatabaseLogon(DbUserName, DbUserPwd)
            For Each myTable In loMyCRReport.Database.Tables
                myLogonInfo = myTable.LogOnInfo
                myLogonInfo.ConnectionInfo.ServerName = DbServerName
                myLogonInfo.ConnectionInfo.DatabaseName = DbName
                If DbUserName = "" Then
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = True
                Else
                    myLogonInfo.ConnectionInfo.UserID = DbUserName
                    myLogonInfo.ConnectionInfo.Password = DbUserPwd
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = False
                End If
                myTable.ApplyLogOnInfo(myLogonInfo)
            Next myTable

            loMyCRReport.SetParameterValue("ICAO", lblICAO.Text)
            loMyCRReport.SetParameterValue("RwyId", lblRwyId.Text)
            loMyCRReport.SetParameterValue("RwyMod", lblRwyMod.Text)
            loMyCRReport.SetParameterValue("AirlineCode", lblAirlineCode.Text)

            'cryRpt.Subreports(0).Load(Request.MapPath("..\ADMS\ObstacleSubReport.rpt"))
            For Each myTable In loMyCRReport.Subreports(1).Database.Tables
                myLogonInfo = myTable.LogOnInfo
                myLogonInfo.ConnectionInfo.ServerName = DbServerName
                myLogonInfo.ConnectionInfo.DatabaseName = DbName
                If DbUserName = "" Then
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = True
                Else
                    myLogonInfo.ConnectionInfo.UserID = DbUserName
                    myLogonInfo.ConnectionInfo.Password = DbUserPwd
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = False
                End If
                myTable.ApplyLogOnInfo(myLogonInfo)
            Next myTable

            CRV_TextReportPrint.ReuseParameterValuesOnRefresh = True
            CRV_TextReportPrint.ReportSource = loMyCRReport

            Dim loTextFileReport As New TextFileReport

            loTextFileReport.SetParameterValue("ICAO", lblICAO.Text)
            loTextFileReport.SetParameterValue("RwyId", lblRwyId.Text)
            loTextFileReport.SetParameterValue("RwyMod", lblRwyMod.Text)
            loTextFileReport.SetParameterValue("AirlineCode", lblAirlineCode.Text)

            Dim lsExportFileName As String = ""
            lsExportFileName = Request.MapPath(Request.ApplicationPath) & "\Temp\" & _
                Session("UserId") & "_PrintStd" & _
                LstExportFormats.SelectedValue.ToString.Split(",")(1)

            'loTextFileReport.ExportToDisk(LstExportFormats.SelectedValue.ToString.Split(",")(0), lsExportFileName)
            loMyCRReport.ExportToDisk(LstExportFormats.SelectedValue.ToString.Split(",")(0), lsExportFileName)

            HdnExportFileName.Value = "..\Temp\" & _
                Session("UserId") & "_PrintStd" & _
                LstExportFormats.SelectedValue.ToString.Split(",")(1)

            BtnExportReport.Enabled = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnShowRunway_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowRunway.Click
        Try
            If lblRwyId.Text = "" And lblRwyMod.Text = "" Then
                Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & lblICAO.Text & "")
            Else
                Response.Redirect("ADMS_RunwayDM.aspx?ICAO=" & lblICAO.Text & "&RwyId=" & lblRwyId.Text & "&RwyMod=" & lblRwyMod.Text)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while showing runway details. \n" & ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub BtnExportReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExportReport.Click
        Try
            GenerateReport()
            Response.Redirect(HdnExportFileName.Value)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while getting Print (Std) report. \n" & ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub


    Private Sub CRV_TextReportPrint_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles CRV_TextReportPrint.Navigate
        Try
            GenerateReport()
            'GenerateReport_Test()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while generating change record report. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub
End Class