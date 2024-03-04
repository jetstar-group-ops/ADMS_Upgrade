Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.IO

Partial Public Class AnalyticalReport
    Inherits System.Web.UI.Page

    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Server.ScriptTimeout = 960000000

        If IsPostBack = False Then
            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.WNB_FULL_ACCESS, "", 0) = False Then

                lsMessage = "You don't have permission on Weight and Balance System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If


            BtnGenReport.Attributes.Add("onClick", "return ValidateControls('" & HdnReportId.Value & "');")

            FillDDLIPADUDID(ddlIpadUDId, "")
            FillDDLIPADVerNo(ddlDBVersionNo, "")

            btnExport.Enabled = False
            
        End If
    End Sub

    Private Sub FillDDLIPADUDID(ByRef ddlControl As System.Web.UI.WebControls.DropDownList, ByVal strFirstItem As String)

       

      
        Dim dtIPADUDIDs As DataTable
        Try

            dtIPADUDIDs = go_Bo.Get_IPADUDID()

            ddlControl.DataSource = dtIPADUDIDs
            ddlControl.DataBind()
            ddlControl.Items.Insert(0, New ListItem(strFirstItem, ""))

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while populating IPAD UDID Dropdownlist.');", True)
        End Try
    End Sub

    Private Sub FillDDLIPADVerNo(ByRef ddlControl As System.Web.UI.WebControls.DropDownList, ByVal strFirstItem As String)




        Dim dtIPADVerNos As DataTable
        Try

            dtIPADVerNos = go_Bo.Get_IPADVersionNo()

            ddlControl.DataSource = dtIPADVerNos
            ddlControl.DataBind()
            ddlControl.Items.Insert(0, New ListItem(strFirstItem, ""))

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while populating IPAD DB Version No Dropdownlist.');", True)
        End Try
    End Sub

    Private Sub GenerateReport()

        go_Bo.StructureFile = Request.MapPath(Request.ApplicationPath) & "\AnalyticalRpt\AnalyticalReport_Structure.xml"

        Dim lo_ReportFilterCriteria As DataTable
        Dim lo_ReportData As New DataSet
        Dim drFtCriteria As DataRow
        Dim cryRpt As New ReportDocument
        Dim ls_ReportTitle As String = ""
        Dim strFilterCriteria As New Text.StringBuilder
        Dim ls_AppRootPath As String = ""
        Dim IsExcludeLIPad As Integer
        Dim strExcludeDBVerNo As String = ""




        'CRV_CPReport.ToolbarStyle.Width = Unit.Pixel(800)
        Try

            IsExcludeLIPad = Convert.ToInt16(chkRptOption.Checked)

            If Not ddlDBVersionNo.SelectedValue.ToString.Trim.Equals(String.Empty) Then
                strExcludeDBVerNo = radDBVerExclude.SelectedValue.ToString.Trim
            End If


            lo_ReportData = go_Bo.GetIPadDBVersionHistoryReport(ddlIpadUDId.SelectedValue.ToString.Trim, IsExcludeLIPad, ddlDBVersionNo.SelectedValue.ToString.Trim, strExcludeDBVerNo)

            If ddlIpadUDId.SelectedIndex > 0 Then
                strFilterCriteria.Append("IPAD UDID : ").Append(ddlIpadUDId.SelectedItem.Text).Append(vbCrLf)
            End If

            If ddlDBVersionNo.SelectedIndex > 0 Then
                strFilterCriteria.Append(radDBVerExclude.SelectedItem.Text.ToString.Trim).Append(" : ").Append(ddlDBVersionNo.SelectedItem.Text).Append(vbCrLf)
            End If

            'strFilterCriteria.Remove(strFilterCriteria.ToString.LastIndexOf(vbCrLf), vbCrLf.ToString.Length)


            HdnReportFileName.Value = "Analytical_IPadDBVerHistory.rpt"
            HdnReportColNames.Value = "IPAD_UDID,Update_DT,PreviousDbVersionString,CurrentDbVersionString,IpAddress,Device_Serial_Number"
            HdnReportColCaptions.Value = "IPAD UDID,Update Date,Previous DB Version String,Current DB Version String,IP Address,Device Serial Number"
            ls_ReportTitle = "Sum-of-Sector Reporting"


            lo_ReportFilterCriteria = New DataTable()
            lo_ReportFilterCriteria.Columns.Add("FilterCriteria", System.Type.GetType("System.String"))

            drFtCriteria = lo_ReportFilterCriteria.NewRow()
            drFtCriteria("FilterCriteria") = strFilterCriteria.ToString

            Session("Filter") = strFilterCriteria.ToString

            lo_ReportFilterCriteria.Rows.Add(drFtCriteria)
            lo_ReportData.Tables.Add(lo_ReportFilterCriteria)

            ls_AppRootPath = Request.MapPath(Request.ApplicationPath)

            If File.Exists(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xml")) = True Then
                File.Delete(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xml"))
            End If
            lo_ReportData.WriteXml(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xml"))


            cryRpt.Load(ls_AppRootPath & "\AnalyticalRpt\" & HdnReportFileName.Value)
            cryRpt.SetDataSource(lo_ReportData.Tables(0))

            'Set Crystal Report Parameter Field value
            cryRpt.SetParameterValue("FilterCriteria", strFilterCriteria.ToString)



            CRV_CPReport.ReportSource = cryRpt
            CRV_CPReport.DataBind()

            'Delete file if already exist
            If File.Exists(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xls")) = True Then
                File.Delete(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xls"))
            End If

            'Export Report data into Excel file
            cryRpt.ExportToDisk(ExportFormatType.Excel, _
                                ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xls"))


            If File.Exists(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".pdf")) = True Then
                File.Delete(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".pdf"))
            End If

            cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, _
                                        ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".pdf"))


            'Delete file if already exist
            If File.Exists(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".csv")) = True Then
                File.Delete(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".csv"))
            End If

            'Export Report data into CSV file
            ExportToCsv(lo_ReportData.Tables(0), _
                        ls_AppRootPath, Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ""), _
                        ls_ReportTitle, strFilterCriteria.ToString)

            pnlExport.Enabled = True

            UpdatePanel2.Update()

            btnExport.Enabled = True

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert(' " & ex.Message.ToString().Replace("'", "") & ".');", True)
        Finally
            lo_ReportFilterCriteria = Nothing
            lo_ReportData = Nothing

        End Try
    End Sub
    Private Sub BtnGenReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenReport.Click
        GenerateReport()
    End Sub


    ''' <summary>
    ''' Export Report data into CSV file
    ''' </summary>
    ''' <param name="poDataTable">Datasource</param>
    ''' <param name="ps_AppRootPath">Application root path</param>
    ''' <param name="ps_ExportedFileName">CSV file name</param>
    ''' <param name="ps_FilterCriteria">Filter Criteria for Data</param>
    ''' <param name="ps_ReportTitle">Report Title</param>
    ''' <remarks></remarks>
    Private Sub ExportToCsv(ByVal poDataTable As System.Data.DataTable, _
                        ByVal ps_AppRootPath As String, _
                        ByVal ps_ExportedFileName As String, _
                        ByVal ps_ReportTitle As String, ByVal ps_FilterCriteria As String)

        Dim lo_FileStreamObj As FileStream = Nothing
        Dim lo_StreamWriterObj As StreamWriter = Nothing
        Dim ls_fileName As String
        Dim ls_RowData As String = ""
        Dim ls_FilePath As String
        Dim ls_ColName As String = ""
        Dim ls_ReportColNames As String

        Try
            ls_FilePath = ps_AppRootPath
            If ls_FilePath.EndsWith("\") = False Then ls_FilePath = ls_FilePath & "\"

            ls_fileName = ps_ExportedFileName & ".csv"
            lo_FileStreamObj = New FileStream(ls_FilePath & "Temp\" & ls_fileName, FileMode.Create, FileAccess.Write)
            lo_StreamWriterObj = New StreamWriter(lo_FileStreamObj)
            lo_StreamWriterObj.WriteLine(ps_ReportTitle)
            'lo_StreamWriterObj.WriteLine(vbCrLf)
            lo_StreamWriterObj.WriteLine(ps_FilterCriteria)
            'lo_StreamWriterObj.WriteLine(vbCrLf)
            lo_StreamWriterObj.WriteLine(HdnReportColCaptions.Value)
            'lo_StreamWriterObj.WriteLine(vbCrLf)
            ls_ReportColNames = HdnReportColNames.Value

            'Read value from Datasource and write into CSV file
            For I = 0 To poDataTable.Rows.Count - 1

                ls_RowData = ""
                Dim intFirstCol As Integer = 0

                For J = 0 To ls_ReportColNames.Split(",").Length - 1

                    If intFirstCol = 0 Then
                        ls_RowData = poDataTable.Rows(I)(ls_ReportColNames.Split(",")(J).Trim)
                        intFirstCol = 1
                    Else
                        ls_RowData = ls_RowData & "," & poDataTable.Rows(I)(ls_ReportColNames.Split(",")(J))
                    End If
                Next

                lo_StreamWriterObj.WriteLine(ls_RowData)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            If lo_StreamWriterObj IsNot Nothing Then
                lo_StreamWriterObj.Close()
            End If

            If lo_FileStreamObj IsNot Nothing Then
                lo_FileStreamObj.Close()
            End If
        End Try

    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Try
            If ddlExportFormat.SelectedValue = ExportFormatType.Excel Then

                Response.Redirect(Request.ApplicationPath & "\Temp\" & Session("UserId") & _
                             "_" & HdnReportFileName.Value.Replace(".rpt", ".xls"), False)
            ElseIf ddlExportFormat.SelectedValue = ExportFormatType.PortableDocFormat Then

                Response.Redirect(Request.ApplicationPath & "\Temp\" & Session("UserId") & _
                 "_" & HdnReportFileName.Value.Replace(".rpt", ".pdf"), False)
            Else
                Response.Redirect(Request.ApplicationPath & "\Temp\" & Session("UserId") & _
                                 "_" & HdnReportFileName.Value.Replace(".rpt", ".csv"), False)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                                                "alert('Error occured while Exporting Report.');", True)
        End Try
    End Sub

  
    Private Sub CRV_CPReport_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles CRV_CPReport.Navigate
        'Dim dsReportData As New DataSet
        'Dim ls_AppRootPath As String
        'Dim cryRpt As New ReportDocument

        'ls_AppRootPath = Request.MapPath(Request.ApplicationPath)
        'dsReportData.ReadXml(ls_AppRootPath & "\Temp\" & Session("UserId") & "_" & HdnReportFileName.Value.Replace(".rpt", ".xml"))

        'cryRpt.Load(ls_AppRootPath & "\AnalyticalRpt\" & HdnReportFileName.Value)
        'cryRpt.SetDataSource(dsReportData.Tables(0))

        ''cryRpt.SetParameterValue("FilterCriteria", dsReportData.Tables(1).Rows(0)("FilterCriteria"))
        'cryRpt.SetParameterValue("FilterCriteria", Session("Filter"))

        'CRV_CPReport.ReportSource = cryRpt
        'CRV_CPReport.DataBind()
        GenerateReport()
    End Sub
End Class