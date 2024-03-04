Imports WNB_Admin_BO

Partial Public Class ADMS_RunwayDM
    Inherits System.Web.UI.Page

    Dim loADMS_BAL_RDM As ADMS_BAL_RDM

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
            'Checking Readonly Rights
            If Convert.ToString(Session("UserId")).ToUpper() <> Convert.ToString("Admin").ToUpper Then
                If loBO.IsUserHasPermission(Session("UserId"), _
                        WNB_Common.Enums.Functionalities.ADMSReadOnlyRights, "", 0) = True Then
                    ReadOnlyRightsToControls()
                End If
            End If


            If Me.IsPostBack = False Then

                txtTora.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtToda.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtASDA.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtLDA.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtSlope.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                txtWidth.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtSotElev.Attributes.Add("OnKeyPress", " return AllowNegativeValue(this);")
                txtDispThr.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtDispTo.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtResaTo.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtResaLand.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtShoulder.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtHdg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtGA.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                txtLatDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtLatMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtLatSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                txtLonDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtLonMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                txtLonSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                'star added by charudatt on 03_07_2015'
                txtVersion.Attributes.Add("OnKeyPress", "return AllowNumeric(this);")
                'End added by charudatt on 03_07_2015'

                LoadControls()


            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while getting Runway details. \n" & _
                   ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub ReadOnlyRightsToControls()
        Try
            btnAddIntersection.Enabled = False
            btnDelete.Enabled = False
            btnUpdate.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadControls()
        Try

            loADMS_BAL_RDM = New ADMS_BAL_RDM


            Dim dsRunWay As New DataSet
            Dim liIntersectionOrObstCount As Integer
            btnUpdate.Text = "Add"
            If Request("ICAO") <> String.Empty And Request("RwyId") <> String.Empty And Request("RwyMod") <> String.Empty Then
                'Edit esting runway
                btnUpdate.Text = "Update"
                dsRunWay = loADMS_BAL_RDM.GetRunWay(Session("UserId"), Request("ICAO"), Request("RwyId"), Request("RwyMod"))
                If Not dsRunWay Is Nothing Then
                    If dsRunWay.Tables(0).Rows.Count = 0 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Invalid runway details." & "');", True)
                        Exit Sub
                    End If
                    If dsRunWay.Tables(1).Rows.Count > 0 Then
                        liIntersectionOrObstCount = dsRunWay.Tables(1).Rows.Count
                    ElseIf dsRunWay.Tables(2).Rows.Count > 0 Then
                        liIntersectionOrObstCount = dsRunWay.Tables(2).Rows.Count
                    End If

                    fillApplics()
                    fillControls(dsRunWay, liIntersectionOrObstCount)
                    fillIntersections(dsRunWay)

                    If dsRunWay.Tables(2).Rows.Count > 0 Then
                        HypLnkObstacle.Text = dsRunWay.Tables(2).Rows.Count
                    Else
                        HypLnkObstacle.Text = "No Obstacle Data"
                        HypLnkObstacle.ForeColor = Drawing.Color.Red
                        HypLnkObstacle.Enabled = False
                    End If

                End If
            ElseIf Request("ICAO") <> String.Empty And Request("RwyId") = String.Empty And Request("RwyMod") = String.Empty Then
                lblICAO.Text = Request("ICAO")
                LblUpdatedBy.Text = Session("UserId")
                'LblUpdatedOn.Text = Date.Now.ToString("dd-MM-yyyy HH:mm")
                chkActive.Checked = True
                'btnUpdate.Text = "Add"
                HypLnkObstacle.Visible = False
                btnDelete.Enabled = False
                btnPrintStd.Enabled = False
                btnSplay.Enabled = False
                btnShowObstacle.Enabled = False
                btnAddIntersection.Enabled = False
                chkActive.Checked = False
                fillApplics()

            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Invalid request. \n" & "');", True)
                Exit Sub
            End If

            HypLnkObstacle.NavigateUrl = "ADMS_ObstacleDM.aspx?ICAO=" & lblICAO.Text & "&RwyId=" & txtRunway.Text & "&RwyMod=" & txtVersion.Text

            loADMS_BAL_RDM = Nothing
            dsRunWay = Nothing

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub fillApplics()
        Try
            Dim loADMS_BAL_AptMaxExp As New ADMS_BAL_AptMaxExp
            Dim loADMS_BAL_DBExport As New ADMS_BAL_DatabaseExp


            'Airport Max Code----
            Dim loAirportMaxExp As DataTable
            loAirportMaxExp = loADMS_BAL_AptMaxExp.GetAptMaxExp(Session("UserId"))
            ddlAcftApplic.Items.Clear()

            For I = 0 To loAirportMaxExp.Rows.Count - 1
                ddlAcftApplic.Items.Add(loAirportMaxExp.Rows(I)("Desc"))
                ddlAcftApplic.Items(ddlAcftApplic.Items.Count - 1).Value = loAirportMaxExp.Rows(I)("Code")
            Next
            loADMS_BAL_AptMaxExp = Nothing
            '----------------------------

            'Export Applic i.e. Database Export code------------
            Dim loExportApplic As DataTable
            loExportApplic = loADMS_BAL_DBExport.GetDbExport(Session("UserId"))
            ddlExportApplic.Items.Clear()

            For I = 0 To loExportApplic.Rows.Count - 1
                ddlExportApplic.Items.Add(loExportApplic.Rows(I)("Desc"))
                ddlExportApplic.Items(ddlExportApplic.Items.Count - 1).Value = loExportApplic.Rows(I)("Code")
            Next
            loADMS_BAL_DBExport = Nothing
            '----------------------------------------------------


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub fillControls(ByVal dsRunWay As DataSet, ByVal liIntersectionOrObstCount As Integer)
        Try
            lblICAO.Text = dsRunWay.Tables(0).Rows(0)("ICAO") & ""
            txtRunway.Text = dsRunWay.Tables(0).Rows(0)("RwyId") & ""
            txtVersion.Text = dsRunWay.Tables(0).Rows(0)("RwyMod") & ""
            txtRunway.ReadOnly = True
            txtVersion.ReadOnly = True

            'If liIntersectionOrObstCount > 0 Then
            '    txtRunway.ReadOnly = True
            '    txtVersion.ReadOnly = True
            'Else
            '    txtRunway.ReadOnly = False
            '    txtVersion.ReadOnly = False
            'End If

            txtTora.Text = dsRunWay.Tables(0).Rows(0)("TORA") & ""
            txtToda.Text = dsRunWay.Tables(0).Rows(0)("TODA") & ""
            txtASDA.Text = dsRunWay.Tables(0).Rows(0)("ASDA") & ""
            txtLDA.Text = dsRunWay.Tables(0).Rows(0)("LDA") & ""
            txtSlope.Text = Math.Round(dsRunWay.Tables(0).Rows(0)("Slope"), 3) & ""
            ddlSlop.SelectedValue = dsRunWay.Tables(0).Rows(0)("SlopeDir") & ""
            txtWidth.Text = dsRunWay.Tables(0).Rows(0)("Width") & ""
            txtSotElev.Text = dsRunWay.Tables(0).Rows(0)("ElevStartTORA") & ""
            txtDispThr.Text = dsRunWay.Tables(0).Rows(0)("DispThr") & ""
            txtDispTo.Text = dsRunWay.Tables(0).Rows(0)("DispTO") & ""
            txtResaTo.Text = dsRunWay.Tables(0).Rows(0)("ResaTo") & ""
            txtResaLand.Text = dsRunWay.Tables(0).Rows(0)("ResaLd") & ""
            txtShoulder.Text = dsRunWay.Tables(0).Rows(0)("Shoulder") & ""
            'txtAcftApp.Text = dsRunWay.Tables(0).Rows(0)("AcftApp") & ""
            ddlAcftApplic.SelectedValue = dsRunWay.Tables(0).Rows(0)("AcftApp") & ""
            txtHdg.Text = dsRunWay.Tables(0).Rows(0)("MagHdg") & ""
            ddlLineupAngle.SelectedValue = dsRunWay.Tables(0).Rows(0)("LineUpAngle") & ""
            txtGA.Text = Math.Round(dsRunWay.Tables(0).Rows(0)("GaGrad"), 2) & ""
            ddlLatitude.SelectedValue = dsRunWay.Tables(0).Rows(0)("LatDir") & ""
            txtLatDeg.Text = dsRunWay.Tables(0).Rows(0)("LatDeg") & ""
            txtLatMin.Text = dsRunWay.Tables(0).Rows(0)("LatMin") & ""
            txtLatSec.Text = Math.Round(Convert.ToDecimal(dsRunWay.Tables(0).Rows(0)("LatSec")), 2) & ""
            ddlLongitude.SelectedValue = dsRunWay.Tables(0).Rows(0)("LonDir") & ""
            txtLonDeg.Text = dsRunWay.Tables(0).Rows(0)("LonDeg") & ""
            txtLonMin.Text = dsRunWay.Tables(0).Rows(0)("LonMin") & ""
            txtLonSec.Text = Math.Round(Convert.ToDecimal(dsRunWay.Tables(0).Rows(0)("LonSec")), 2) & ""
            txtTakeOffAllEngines.Text = dsRunWay.Tables(0).Rows(0)("AEProc") & ""
            txtTakeOffEngineFailure.Text = dsRunWay.Tables(0).Rows(0)("EOProc") & ""
            txtPostProc.Text = dsRunWay.Tables(0).Rows(0)("PostProc") & ""
            txtTakeOffComment.Text = dsRunWay.Tables(0).Rows(0)("Comment") & ""
            txtLandingCommment.Text = dsRunWay.Tables(0).Rows(0)("LandComment") & ""
            chkExpRnw.Checked = dsRunWay.Tables(0).Rows(0)("ExpCmt") & ""
            chkDontAppendTO.Checked = dsRunWay.Tables(0).Rows(0)("DontApndToEop") & ""
            chkActive.Checked = dsRunWay.Tables(0).Rows(0)("Active") & ""
            If dsRunWay.Tables(0).Rows(0)("ChangeDateTime") & "" <> "" Then
                LblUpdatedOn.Text = Date.Parse(dsRunWay.Tables(0).Rows(0)("ChangeDateTime")).ToString("dd-MM-yyyy HH:mm")
            End If
            LblUpdatedBy.Text = dsRunWay.Tables(0).Rows(0)("ChangeUser") & ""
            ddlExportApplic.SelectedValue = dsRunWay.Tables(0).Rows(0)("ExpApp") & ""

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub fillIntersections(ByVal dsIntersection As DataSet)
        Try

            If dsIntersection.Tables(1).Rows.Count > 0 Then
                DgrdIntersection.DataSource = dsIntersection.Tables(1)
                DgrdIntersection.DataBind()
                tdIntersectionData.Visible = False
                lblNoIntersectionData.Visible = False
            Else
                tdIntersectionData.Visible = True
                lblNoIntersectionData.Visible = True
                lblNoIntersectionData.ForeColor = Drawing.Color.Red
                lblNoIntersectionData.Font.Bold = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try

            If ValidateData() = False Then
                Exit Sub
            End If


            Dim loRunwayDetails As DataSet
            loADMS_BAL_RDM = New ADMS_BAL_RDM
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim loMyNewRunwayDetails As New DataSet
            Dim lintResult As Integer
            Dim loAirport As DataSet

            loRunwayDetails = loADMS_BAL_RDM.GetRunWay(Session("UserId"), "-1", "-1", "-1")
            loMyNewRunwayDetails.Tables.Add(loRunwayDetails.Tables(0).Copy)
            loMyNewRunwayDetails.Tables(0).Columns("ChangeDateTime").DataType = System.Type.GetType("System.String")
            loRunwayDetails.Dispose()

            loMyNewRunwayDetails.Tables(0).Rows.Clear()

            Dim loDataRow As DataRow

            loDataRow = loMyNewRunwayDetails.Tables(0).NewRow
            loMyNewRunwayDetails.Tables(0).Rows.Add(loDataRow)
            loMyNewRunwayDetails.Tables(0).AcceptChanges()

            PopulateRunwayData(loMyNewRunwayDetails.Tables(0))

            'Validating Airport Active or not
            loAirport = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), loMyNewRunwayDetails.Tables(0).Rows(0)("ICAO"))
            If Not loAirport Is Nothing Then
                If loAirport.Tables(0).Rows.Count > 0 Then
                    If Convert.ToBoolean(loAirport.Tables(0).Rows(0)("Active")) = False Then
                        If Convert.ToBoolean(loMyNewRunwayDetails.Tables(0).Rows(0)("Active")) = True Then
                            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                               "Message", "alert('The Airport of this runway is Inactive, You can not make this runway active." & "');", True)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            '-----------------------


            'Validating Duplicate runway
            loRunwayDetails = loADMS_BAL_RDM.GetRunWay(Session("UserId"), loMyNewRunwayDetails.Tables(0).Rows(0)("ICAO") _
                             , loMyNewRunwayDetails.Tables(0).Rows(0)("RwyId"), loMyNewRunwayDetails.Tables(0).Rows(0)("RwyMod"))
            If Not loRunwayDetails Is Nothing Then
                If loRunwayDetails.Tables(0).Rows.Count > 0 And txtRunway.ReadOnly = False And txtVersion.ReadOnly = False Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                        "Message", "alert('This Runway entry is already exist." & "');", True)
                    Exit Sub
                End If
            End If
            loRunwayDetails = Nothing
            '-------

            lintResult = loADMS_BAL_RDM.CreateUpdate_Runway(Session("UserId"), _
                                                            lblICAO.Text, _
                                                            txtRunway.Text, _
                                                            txtVersion.Text, _
                                                            loMyNewRunwayDetails)

            If lintResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully updated the Runway details." & "');", True)

                ClearFormForNewData() 'added by swami 01032024 on' When 'Update' button is pressed to add a new runway, allow the data for the runway to be entered without first closing and reopening the runway.

            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to updated the Runway details; please contact support team." & "');", True)
            End If

            LoadControls()
            'Response.Redirect("ADMS_RunwayDM.aspx?RwyId=" & txtRunway.Text & "&Icao=" & lblICAO.Text & "&RwyMod=" & txtVersion.Text & "")

            loADMS_BAL_RDM = Nothing
            loADMS_BAL_ADM = Nothing
            loMyNewRunwayDetails = Nothing
            loAirport = Nothing
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while updating Runway details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Function ValidateData() As Boolean

        Dim lsErrMsg As String = ""
        Dim lbResult As Boolean = True

        ' If chkActive.Checked = True Then


        If txtRunway.Text.Trim = "" Then
            lsErrMsg = "Please enter Runway code."
            GoTo ReturnResult
        End If

        If txtVersion.Text.Trim = "" Then
            lsErrMsg = "Please enter Version for Runway."
            GoTo ReturnResult
        End If

        If txtTora.Text.Trim = "" Then
            lsErrMsg = "Please enter TORA for Runway."
            GoTo ReturnResult
        End If

        If txtToda.Text.Trim = "" Then
            lsErrMsg = "Please enter TODA for Runway."
            GoTo ReturnResult
        End If

        If txtASDA.Text.Trim = "" Then
            lsErrMsg = "Please enter ASDA for Runway."
            GoTo ReturnResult
        End If
        If txtLDA.Text.Trim = "" Then
            lsErrMsg = "Please enter LDA for Runway."
            GoTo ReturnResult
        End If
        If txtSlope.Text.Trim = "" Then
            lsErrMsg = "Please enter Slope for Runway."
            GoTo ReturnResult
        End If
        If txtWidth.Text.Trim = "" Then
            lsErrMsg = "Please enter Width for Runway."
            GoTo ReturnResult
        End If
        If txtSotElev.Text.Trim = "" Then
            lsErrMsg = "Please enter SOT Elev(ft) for Runway."
            GoTo ReturnResult
        End If
        If txtDispThr.Text.Trim = "" Then
            lsErrMsg = "Please enter SOT Disp Thr (m) for Runway."
            GoTo ReturnResult
        End If
        If txtDispTo.Text.Trim = "" Then
            lsErrMsg = "Please enter Disp TO (m) for Runway."
            GoTo ReturnResult
        End If
        If txtResaTo.Text.Trim = "" Then
            lsErrMsg = "Please enter RESA TO (m) for Runway."
            GoTo ReturnResult
        End If
        If txtResaLand.Text.Trim = "" Then
            lsErrMsg = "Please enter RESA LAND (m) for Runway."
            GoTo ReturnResult
        End If
        If txtShoulder.Text.Trim = "" Then
            lsErrMsg = "Please enter Shoulder(m) for Runway."
            GoTo ReturnResult
        End If
        If txtHdg.Text.Trim = "" Then
            lsErrMsg = "Please enter Hdg for Runway."
            GoTo ReturnResult
        End If
        If txtGA.Text.Trim = "" Then
            lsErrMsg = "Please enter G/A (%) for Runway."
            GoTo ReturnResult
        End If
        If txtLatDeg.Text.Trim = "" Then
            lsErrMsg = "Please enter Latitude Degdree for Runway."
            GoTo ReturnResult
        End If
        If txtLatMin.Text.Trim = "" Then
            lsErrMsg = "Please enter Latitude Min for Runway."
            GoTo ReturnResult
        End If
        If txtLatSec.Text.Trim = "" Then
            lsErrMsg = "Please enter Latitude Sec for Runway."
            GoTo ReturnResult
        End If
        If txtLonDeg.Text.Trim = "" Then
            lsErrMsg = "Please enter Longiture Degree for Runway."
            GoTo ReturnResult
        End If
        If txtLonMin.Text.Trim = "" Then
            lsErrMsg = "Please enter Longiture Min for Runway."
            GoTo ReturnResult
        End If
        If txtLonSec.Text.Trim = "" Then
            lsErrMsg = "Please enter Longiture Sec for Runway."
            GoTo ReturnResult
        End If
        'End If
ReturnResult:

        If lsErrMsg <> "" Then
            lbResult = False
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
              "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)
        End If

        Return lbResult

    End Function


    Private Function ClearFormForNewData() As Boolean

        
        txtRunway.Text = ""
        txtVersion.Text = ""
        txtTora.Text = ""
        txtToda.Text = ""
        txtASDA.Text = ""
        txtLDA.Text = ""
        txtSlope.Text = ""
        txtWidth.Text = ""
        txtSotElev.Text = ""
        txtDispThr.Text = ""
        txtDispTo.Text = ""
        txtResaTo.Text = ""
        txtResaLand.Text = ""
        txtShoulder.Text = ""
        txtHdg.Text = ""
        txtGA.Text = ""
        txtLatDeg.Text = ""
        txtLatMin.Text = ""
        txtLatSec.Text = ""
        txtLonDeg.Text = ""
        txtLonMin.Text = ""
        txtLonSec.Text = ""
        txtLandingCommment.Text = ""
        txtTakeOffAllEngines.Text = ""
        txtTakeOffComment.Text = ""
        txtTakeOffEngineFailure.Text = ""
        txtPostProc.Text = ""
        chkActive.Checked = False




    End Function

    Private Sub PopulateRunwayData(ByRef loMyNewRunwayDetails As DataTable)
        Try
            loMyNewRunwayDetails.Rows(0)("ICAO") = lblICAO.Text
            loMyNewRunwayDetails.Rows(0)("RwyId") = txtRunway.Text
            loMyNewRunwayDetails.Rows(0)("RwyMod") = txtVersion.Text
            loMyNewRunwayDetails.Rows(0)("TORA") = txtTora.Text
            loMyNewRunwayDetails.Rows(0)("TODA") = txtToda.Text
            loMyNewRunwayDetails.Rows(0)("ASDA") = txtASDA.Text
            loMyNewRunwayDetails.Rows(0)("LDA") = txtLDA.Text
            loMyNewRunwayDetails.Rows(0)("Slope") = txtSlope.Text
            loMyNewRunwayDetails.Rows(0)("SlopeDir") = ddlSlop.SelectedValue
            loMyNewRunwayDetails.Rows(0)("Width") = txtWidth.Text
            loMyNewRunwayDetails.Rows(0)("ElevStartTORA") = txtSotElev.Text
            loMyNewRunwayDetails.Rows(0)("DispThr") = txtDispThr.Text
            loMyNewRunwayDetails.Rows(0)("DispTO") = txtDispTo.Text
            loMyNewRunwayDetails.Rows(0)("ResaTo") = txtResaTo.Text
            loMyNewRunwayDetails.Rows(0)("ResaLd") = txtResaLand.Text
            loMyNewRunwayDetails.Rows(0)("Shoulder") = txtShoulder.Text
            loMyNewRunwayDetails.Rows(0)("MagHdg") = txtHdg.Text
            loMyNewRunwayDetails.Rows(0)("LineUpAngle") = ddlLineupAngle.SelectedValue
            loMyNewRunwayDetails.Rows(0)("GaGrad") = txtGA.Text
            loMyNewRunwayDetails.Rows(0)("LatDir") = ddlLatitude.SelectedValue
            loMyNewRunwayDetails.Rows(0)("LatDeg") = txtLatDeg.Text
            loMyNewRunwayDetails.Rows(0)("LatMin") = txtLatMin.Text
            loMyNewRunwayDetails.Rows(0)("LatSec") = txtLatSec.Text
            loMyNewRunwayDetails.Rows(0)("LonDir") = ddlLongitude.SelectedValue
            loMyNewRunwayDetails.Rows(0)("LonDeg") = txtLonDeg.Text
            loMyNewRunwayDetails.Rows(0)("LonMin") = txtLonMin.Text
            loMyNewRunwayDetails.Rows(0)("LonSec") = txtLonSec.Text
            loMyNewRunwayDetails.Rows(0)("AEProc") = txtTakeOffAllEngines.Text
            loMyNewRunwayDetails.Rows(0)("EOProc") = txtTakeOffEngineFailure.Text
            loMyNewRunwayDetails.Rows(0)("PostProc") = txtPostProc.Text
            loMyNewRunwayDetails.Rows(0)("Comment") = txtTakeOffComment.Text
            loMyNewRunwayDetails.Rows(0)("LandComment") = txtLandingCommment.Text
            loMyNewRunwayDetails.Rows(0)("ExpCmt") = chkExpRnw.Checked
            loMyNewRunwayDetails.Rows(0)("DontApndToEop") = chkDontAppendTO.Checked
            loMyNewRunwayDetails.Rows(0)("AcftApp") = ddlAcftApplic.SelectedValue  'txtAcftApp.Text
            loMyNewRunwayDetails.Rows(0)("Active") = chkActive.Checked
            loMyNewRunwayDetails.Rows(0)("ChangeDateTime") = Date.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            loMyNewRunwayDetails.Rows(0)("ChangeUser") = Session("UserId")
            loMyNewRunwayDetails.Rows(0)("DontCheckSlope") = False
            loMyNewRunwayDetails.Rows(0)("ExpApp") = ddlExportApplic.SelectedValue

            'LblUpdatedOn.Text = Date.Now.ToString("dd-MM-yyyy HH:mm")
            'LblUpdatedBy.Text = Session("UserId")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim loRunwayDetails As DataSet
        Try
            Dim lintResult As Integer

            loADMS_BAL_RDM = New ADMS_BAL_RDM

            'Validation to check Intersections or obstacles are present before deletion
            loRunwayDetails = loADMS_BAL_RDM.GetRunWay(Session("UserId"), lblICAO.Text _
                            , txtRunway.Text, txtVersion.Text)
            If Not loRunwayDetails Is Nothing Then
                Dim strMsg As String = ""
                If loRunwayDetails.Tables(1).Rows.Count > 0 Then
                    strMsg = "Intersection(s)"
                End If
                If loRunwayDetails.Tables(2).Rows.Count > 0 Then
                    If strMsg <> "" Then
                        strMsg = strMsg & ", Obstacle(s)"
                    Else
                        strMsg = strMsg & "Obstacle(s)"
                    End If
                End If
                If strMsg <> "" Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                              "Message", "alert('Please delete " & strMsg & " of this runway first." & "');", True)
                    Exit Sub
                End If
            End If
            '------------------------------------

            lintResult = loADMS_BAL_RDM.Delete_Runway(Session("UserId"), lblICAO.Text, txtRunway.Text, txtVersion.Text)

            If lintResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully Deleted the Runway details." & "');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to Delete the Runway details; please contact support team." & "');", True)
            End If
            Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & lblICAO.Text)
            loADMS_BAL_RDM = Nothing
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Error while Deleting Runway details.\n" & _
               ex.Message.Replace("'", "") & "');", True)
        Finally
            loRunwayDetails = Nothing
        End Try
    End Sub

    'Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
    '    Try
    '        Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & lblICAO.Text)
    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
    '           "Message", "alert('Error while Closing Runway details.\n" & _
    '           ex.Message.Replace("'", "") & "');", True)
    '    End Try
    'End Sub

    Protected Sub btnShowObstacle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowObstacle.Click
        Try
            Response.Redirect("ADMS_ObstacleDM.aspx?ICAO=" & lblICAO.Text & "&RwyId=" & txtRunway.Text & "&RwyMod=" & txtVersion.Text)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while showing obstacle details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnPrintStd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintStd.Click
        Try
            Response.Redirect("ADMS_TextReportPrint.aspx?ICAO=" & lblICAO.Text & "&RwyId=" & _
                             txtRunway.Text & "&RwyMod=" & txtVersion.Text & "&AirlineCode=" & Session("AirliineCode"))

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while printing.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    'Protected Sub btnPrintCola_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintCola.Click
    '    Try
    '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
    '              "Message", "alert('This feature will be implemnted is phase-2 of this system." & _
    '                        "');", True)
    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
    '            "Message", "alert('Error while printing.\n" & _
    '            ex.Message.Replace("'", "") & "');", True)
    '    End Try
    'End Sub

    Protected Sub btnAddIntersection_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddIntersection.Click
        Try
            Response.Redirect("ADMS_IntersectionDM.aspx?ICAO=" & lblICAO.Text & "&RwyId=" & _
                              txtRunway.Text & "&RwyMod=" & txtVersion.Text & "&Add=1")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while showing obstacle details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Try
            Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & lblICAO.Text)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Error while Closing Runway details.\n" & _
               ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnSplay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSplay.Click
        Try
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loADMS_BAL_KML As New ADMS_BAL_KML
            Dim lsKMLFileOutputfilePath As String = ""
            Dim lsKMLInputStyles As String = Server.MapPath("~/ADMS/Airport_KML_Styles.txt")
            Dim lsKMLOutputFilePath As String = Server.MapPath("~/Temp/KMLFiles\")
            Dim ldsRunway As DataSet

            If Not System.IO.Directory.Exists(lsKMLOutputFilePath) Then
                System.IO.Directory.CreateDirectory(lsKMLOutputFilePath)
            End If

            ldsRunway = loADMS_BAL_RDM.GetRunWay(Session("UserId"), lblICAO.Text, txtRunway.Text, txtVersion.Text)
            lsKMLFileOutputfilePath = loADMS_BAL_KML.CreateObstacleSplay_KMLFile(Session("UserId"), ldsRunway, lsKMLInputStyles, lsKMLOutputFilePath)

            Dim file As System.IO.FileInfo = New System.IO.FileInfo(lsKMLFileOutputfilePath)

            If file.Exists Then
                'Response.ClearHeaders()
                'Response.AppendHeader("Content-Encoding", "none;")
                Response.Redirect("../Temp/KMLfiles/" & file.Name, False)
            End If

            loADMS_BAL_KML = Nothing
            file = Nothing

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while generating Splay Google Earth file.\n" & _
       ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & lblICAO.Text)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Error while Closing Runway details.\n" & _
               ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

End Class