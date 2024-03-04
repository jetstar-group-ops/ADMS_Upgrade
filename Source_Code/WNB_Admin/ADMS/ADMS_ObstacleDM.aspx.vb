Imports WNB_Admin_BO

Partial Public Class ADMS_ObstacleDM
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

            'Checking Readonly Rights
            If Convert.ToString(Session("UserId")).ToUpper() <> Convert.ToString("Admin").ToUpper Then
                If loBO.IsUserHasPermission(Session("UserId"), _
                        WNB_Common.Enums.Functionalities.ADMSReadOnlyRights, "", 0) = True Then
                    ReadOnlyRightsToControls()
                End If
            End If

            If Me.IsPostBack = False Then
                DivObstacleDetails.Attributes.Add("style", "display:none;")

                txtReference.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                'TxtComments.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                TxtDist.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtElev.Attributes.Add("OnKeyPress", " return AllowNegativeValue(this);")
                txtOffset.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                TxtLonDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLonMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLonSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                txtOffset.Attributes.Add("OnKeyPress", " return AllowNegativeValue(this);")


                If Request("ICAO") <> String.Empty And Request("RwyId") <> String.Empty _
                    And Request("RwyMod") <> String.Empty Then

                    txtICAO_Display.Text = Request("ICAO")
                    txtRwyId_Display.Text = Request("RwyId")
                    txtVersion_Display.Text = Request("RwyMod")

                    Dim dsObstacles As DataSet
                    Dim objObstacleBO As New ADMS_BAL_ODM

                    dsObstacles = objObstacleBO.Get_Obstacles(Session("UserId"), 0, txtICAO_Display.Text.Trim, txtRwyId_Display.Text, txtVersion_Display.Text)

                    If dsObstacles IsNot Nothing Then
                        If dsObstacles.Tables(0).Rows.Count > 0 Then
                            DgrdObstacles.DataSource = dsObstacles
                            DgrdObstacles.DataBind()
                            'Else
                            '    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            '       "Message", "alert('Invalid Obstacle details." & "');", True)
                        End If
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Invalid request. \n" & "');", True)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                     "Message", "alert('Error while getting Obstacle details. \n" & _
                     ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub ReadOnlyRightsToControls()
        Try
            BtnCreateObstacle.Enabled = False
            btnDelete.Enabled = False
            btnUpdate.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub ShowObstacles()
        Try
            Dim dsObstacles As DataSet
            Dim objObstacleBO As New ADMS_BAL_ODM

            dsObstacles = objObstacleBO.Get_Obstacles(Session("UserId"), 0, txtICAO_Display.Text.Trim, txtRwyId_Display.Text, txtVersion_Display.Text)

            If dsObstacles IsNot Nothing Then
                DgrdObstacles.DataSource = dsObstacles
                DgrdObstacles.DataBind()
            End If


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DgrdAirports_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DgrdObstacles.ItemCommand

        Dim intObstacleId As Integer

        Try
            intObstacleId = e.CommandArgument.ToString.Split(",")(1)
            HdnObstacleId.Value = intObstacleId

            'btnDelete.Enabled = True
            PnlObstacleDetails.Enabled = True
            PnlObstacleDetails.Visible = True
            BtnUpdate.Text = "Update" 'swami'
            btnDelete.Enabled = True  'swami'
            If e.CommandArgument.ToString.StartsWith("E") = True Then
                'Edit obstacle
                GetAndShowObstacles(intObstacleId, txtICAO_Display.Text, txtRwyId_Display.Text, txtVersion_Display.Text)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                '            "window.setTimeout('open_Obstacle_Details_box()',500);", True)

            ElseIf e.CommandArgument.ToString.StartsWith("D") = True Then
                ''Delete obstacle
                'Dim objObstacleBO As New BO.ADMS_BAL_ODM
                'Dim liResult As Integer

                'Try
                '    liResult = objObstacleBO.Delete_Obstacle(Session("UserId"), intObstacleId, txtICAO_Display.Text, txtRwyId_Display.Text, txtVersion_Display.Text)
                '    If liResult = 1 Then
                '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                '       "Message", "alert('Successfully deleted the obstacle details." & "');", True)
                '        DivObstacleDetails.Attributes.Add("style", "display:none;")
                '    Else
                '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                '       "Message", "alert('Failed to delete obstacle details; please contact support team." & "');", True)
                '    End If
                'Catch ex As Exception
                '    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                '        "Message", "alert('Error while deleting obstacle details. please contact support team. \n" & _
                '            ex.Message.Replace("'", "") & "');", True)
                '    Exit Sub
                'End Try
                'ShowObstacles()
            ElseIf e.CommandArgument.ToString.StartsWith("C") = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('This feature will be implemnted is phase-2 of this system." & _
                              "');", True)
                Exit Sub
            ElseIf e.CommandArgument.ToString.StartsWith("P") = True Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('This feature will be implemnted is phase-2 of this system." & _
                              "');", True)
                Exit Sub
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while getting obstacle details. \n" & ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub GetAndShowObstacles(ByVal intObstacleId As Integer, ByVal strICAO As String, ByVal strRwyId As String, ByVal strVersion As String)
        Try
            Dim objObstacleDetails As DataSet
            Dim objObstacleBO As New ADMS_BAL_ODM

            objObstacleDetails = objObstacleBO.Get_Obstacles(Session("UserId"), intObstacleId, strICAO, strRwyId, strVersion)
            ShowObstacleDetails(objObstacleDetails.Tables(0))
            DivObstacleDetails.Attributes.Add("style", "display:inline;")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ShowObstacleDetails(ByVal ObjObstacleDetails As DataTable)
        Try

            If ObjObstacleDetails.Rows.Count > 0 Then
                TxtDist.Text = ObjObstacleDetails.Rows(0)("Distance") & ""
                TxtElev.Text = ObjObstacleDetails.Rows(0)("Elevation") & ""
                txtOffset.Text = ObjObstacleDetails.Rows(0)("LatOffSet") & ""
                txtReference.Text = ObjObstacleDetails.Rows(0)("ObsRef") & ""

                ddlLatDir.SelectedIndex = -1
                If ddlLatDir.Items.FindByValue(ObjObstacleDetails.Rows(0)("LatDir") & "") IsNot Nothing Then
                    ddlLatDir.SelectedValue = ObjObstacleDetails.Rows(0)("LatDir") & ""
                End If
                TxtLatDeg.Text = ObjObstacleDetails.Rows(0)("LatDeg") & ""
                TxtLatMin.Text = ObjObstacleDetails.Rows(0)("LatMin") & ""
                TxtLatSec.Text = Math.Round(Convert.ToDecimal(ObjObstacleDetails.Rows(0)("LatSec")), 2) & ""

                ddlLonDir.SelectedIndex = -1
                If ddlLonDir.Items.FindByValue(ObjObstacleDetails.Rows(0)("LonDir") & "") IsNot Nothing Then
                    ddlLonDir.SelectedValue = ObjObstacleDetails.Rows(0)("LonDir") & ""
                End If
                TxtLonDeg.Text = ObjObstacleDetails.Rows(0)("LonDeg") & ""
                TxtLonMin.Text = ObjObstacleDetails.Rows(0)("LonMin") & ""
                TxtLonSec.Text = Math.Round(Convert.ToDecimal(ObjObstacleDetails.Rows(0)("LonSec")), 2) & ""

                TxtComments.Text = ObjObstacleDetails.Rows(0)("Comment") & ""
                ChkActive.Checked = ObjObstacleDetails.Rows(0)("Active") & ""
                If ObjObstacleDetails.Rows(0)("ChangeDateTime") & "" <> "" Then
                    LblUpdatedOn.Text = Date.Parse(ObjObstacleDetails.Rows(0)("ChangeDateTime") & "").ToString("dd-MM-yyyy HH:mm")
                End If

                LblUpdatedBy.Text = ObjObstacleDetails.Rows(0)("ChangeUser") & ""
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Try
            MakeFormInNavigationMode()
            DivObstacleDetails.Attributes.Add("style", "display:none;")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                 "Message", "alert('Error while cancelling the edit operation.\n" & ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub MakeFormInNavigationMode()
        Try
            'PnlAirportDetails.Enabled = False
            'PnlAirportDetails.Visible = False
            'DgrdAirports.Enabled = True
            'BtnCreateAirport.Enabled = True
            'BtnSelectAirport.Enabled = True

            PnlObstacleDetails.Enabled = False
            PnlObstacleDetails.Visible = False
            'DgrdAirports.Enabled = True
            'BtnCreateAirport.Enabled = True
            'BtnSelectAirport.Enabled = True
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub ClearFormForNewObstacle()
        Try
            TxtDist.Text = ""
            TxtElev.Text = ""
            txtOffset.Text = ""
            txtReference.Text = ""
            TxtLatDeg.Text = ""
            TxtLatMin.Text = ""
            TxtLatSec.Text = ""
            TxtLonDeg.Text = ""
            TxtLonMin.Text = ""
            TxtLonSec.Text = ""
            TxtComments.Text = ""
            ChkActive.Checked = True
            LblUpdatedBy.Text = ""
            LblUpdatedOn.Text = ""
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Try
            If ValidateData() = False Then
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                '       "window.setTimeout('open_Obstacle_Details_box()',500);", True)
                Exit Sub
            End If

            Dim objObstacleDetails As DataSet
            Dim ObjObstacleBO As New ADMS_BAL_ODM
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loMyNewObstacleDetails As New DataSet
            Dim liResult As Integer
            Dim loDataRow As DataRow
            Dim intObstacleId As Integer
            Dim loRunway As DataSet


            objObstacleDetails = ObjObstacleBO.Get_Obstacles(Session("UserId"), "-1", "-1", "-1", "-1")
            loMyNewObstacleDetails.Tables.Add(objObstacleDetails.Tables(0).Copy)
            loMyNewObstacleDetails.Tables(0).Columns("ChangeDateTime").DataType = System.Type.GetType("System.String")
            objObstacleDetails.Dispose()

            loMyNewObstacleDetails.Tables(0).Rows.Clear()
            loDataRow = loMyNewObstacleDetails.Tables(0).NewRow
            loMyNewObstacleDetails.Tables(0).Rows.Add(loDataRow)
            loMyNewObstacleDetails.Tables(0).AcceptChanges()

            If HdnObstacleId.Value <> "" Then
                intObstacleId = Convert.ToInt32(HdnObstacleId.Value)
            Else
                intObstacleId = 0
            End If

            PolukateObstacleData(intObstacleId, loMyNewObstacleDetails.Tables(0))


            'Validating Runway active or not---------------------
            loRunway = loADMS_BAL_RDM.GetRunWay(Session("UserId"), loMyNewObstacleDetails.Tables(0).Rows(0)("ICAO"), _
                                                loMyNewObstacleDetails.Tables(0).Rows(0)("RwyId"), _
                                                loMyNewObstacleDetails.Tables(0).Rows(0)("RwyMod"))
            If Not loRunway Is Nothing Then
                If loRunway.Tables(0).Rows.Count > 0 Then
                    If Convert.ToBoolean(loRunway.Tables(0).Rows(0)("Active")) = False Then
                        If Convert.ToBoolean(loMyNewObstacleDetails.Tables(0).Rows(0)("Active")) = True Then
                            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                              "Message", "alert('The Runway of this obstacle is Inactive, You can not make this obstacle Active." & "');", True)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            '--------------------------------


            'Validating duplicate obstacle-----------------
            objObstacleDetails = ObjObstacleBO.Get_Obstacles(Session("UserId"), intObstacleId, _
                                loMyNewObstacleDetails.Tables(0).Rows(0)("ICAO"), _
                                loMyNewObstacleDetails.Tables(0).Rows(0)("RwyId"), _
                                loMyNewObstacleDetails.Tables(0).Rows(0)("RwyMod"))

            If Not objObstacleDetails Is Nothing Then
                If objObstacleDetails.Tables(0).Rows.Count > 0 And intObstacleId = 0 Then
                    Dim dtCheckDuplicate As DataTable
                    objObstacleDetails.Tables(0).DefaultView.RowFilter = "Distance='" & loMyNewObstacleDetails.Tables(0).Rows(0)("Distance") & "'"
                    dtCheckDuplicate = objObstacleDetails.Tables(0).DefaultView.ToTable()
                    If dtCheckDuplicate.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('This Obstacle entry is already exist." & "');", True)
                        Exit Sub
                    End If
                    dtCheckDuplicate = Nothing
                End If
            End If
            objObstacleDetails = Nothing
            '--------------------------------------------


            liResult = ObjObstacleBO.CreateUpdate_Obstacles(Session("UserId"), intObstacleId, txtICAO_Display.Text, txtRwyId_Display.Text, txtVersion_Display.Text, loMyNewObstacleDetails)

            Dim strAddUpdate As String

            If BtnUpdate.Text = "Update" Then
                strAddUpdate = "Updated"
            Else
                If liResult = 1 Then
                    strAddUpdate = "Added"
                Else
                    strAddUpdate = "Add"
                End If
            End If

            If liResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully " & strAddUpdate & " the Obstacle details." & "');", True)
                DivObstacleDetails.Attributes.Add("style", "display:none;")
                ShowObstacles()
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to " & strAddUpdate & " the Obstacle details; please contact support team." & "');", True)
                DivObstacleDetails.Attributes.Add("style", "display:inline;")
            End If

            ObjObstacleBO = Nothing
            loADMS_BAL_RDM = Nothing
            loMyNewObstacleDetails = Nothing
            loRunway = Nothing


        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
            '            "window.setTimeout('open_Obstacle_Details_box()',500);", True)
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while updating obstacle details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub


    Private Sub PolukateObstacleData(ByVal intObstacleId As Integer, ByRef ObjObstacleDetails As DataTable)
        Try
            ObjObstacleDetails.Rows(0)("Obstacle_Id") = intObstacleId
            ObjObstacleDetails.Rows(0)("ICAO") = txtICAO_Display.Text
            ObjObstacleDetails.Rows(0)("RwyId") = txtRwyId_Display.Text
            ObjObstacleDetails.Rows(0)("RwyMod") = txtVersion_Display.Text
            ObjObstacleDetails.Rows(0)("Distance") = TxtDist.Text
            ObjObstacleDetails.Rows(0)("Elevation") = IIf(TxtElev.Text = "", 0, TxtElev.Text)
            ObjObstacleDetails.Rows(0)("LatOffSet") = txtOffset.Text
            ObjObstacleDetails.Rows(0)("ObsRef") = txtReference.Text

            ObjObstacleDetails.Rows(0)("LatDir") = ddlLatDir.SelectedValue
            ObjObstacleDetails.Rows(0)("LatDeg") = TxtLatDeg.Text
            ObjObstacleDetails.Rows(0)("LatMin") = TxtLatMin.Text
            ObjObstacleDetails.Rows(0)("LatSec") = TxtLatSec.Text

            ObjObstacleDetails.Rows(0)("LonDir") = ddlLonDir.SelectedValue
            ObjObstacleDetails.Rows(0)("LonDeg") = TxtLonDeg.Text
            ObjObstacleDetails.Rows(0)("LonMin") = TxtLonMin.Text
            ObjObstacleDetails.Rows(0)("LonSec") = TxtLonSec.Text

            ObjObstacleDetails.Rows(0)("Comment") = TxtComments.Text

            ObjObstacleDetails.Rows(0)("Active") = IIf(ChkActive.Checked, 1, 0)
            ObjObstacleDetails.Rows(0)("ChangeUser") = Session("UserId")
            ObjObstacleDetails.Rows(0)("ChangeDateTime") = Date.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")

            LblUpdatedOn.Text = Date.Now.ToString("dd-MM-yyyy HH:mm")
            LblUpdatedBy.Text = Session("UserId")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function ValidateData() As Boolean
        Dim lsErrMsg As String = ""
        Dim lbResult As Boolean = True
        Try
            If ChkActive.Checked = True Then

                If TxtDist.Text.Trim = "" Then
                    lsErrMsg = "Please enter Distance for Obstacle."
                    GoTo ReturnResult
                End If

                If TxtElev.Text.Trim = "" Then
                    lsErrMsg = "Please enter Elevation for Obstacle."
                    GoTo ReturnResult
                End If

                If txtOffset.Text.Trim = "" Then
                    lsErrMsg = "Please enter Offset for Obstacle."
                    GoTo ReturnResult
                End If

                If txtReference.Text.Trim = "" Then
                    lsErrMsg = "Please enter Reference for Obstacle."
                    GoTo ReturnResult
                End If

                'validation of Latitude values
                If TxtLatDeg.Text.Trim = "" Then
                    lsErrMsg = "Please enter Latitude value (Degree) for the Obstacle."
                    GoTo ReturnResult
                End If
                If TxtLatMin.Text.Trim = "" Then
                    lsErrMsg = "Please enter Latitude value (MIN) for the Obstacle."
                    GoTo ReturnResult
                End If
                If TxtLatSec.Text.Trim = "" Then
                    lsErrMsg = "Please enter Latitude value (SEC) for the Obstacle."
                    GoTo ReturnResult
                End If

                'validation of Longitude values
                If TxtLonDeg.Text.Trim = "" Then
                    lsErrMsg = "Please enter Longitude value (Degree) for the Obstacle."
                    GoTo ReturnResult
                End If
                If TxtLonMin.Text.Trim = "" Then
                    lsErrMsg = "Please enter Longitude value (MIN) for the Obstacle."
                    GoTo ReturnResult
                End If
                If TxtLonSec.Text.Trim = "" Then
                    lsErrMsg = "Please enter Longitude value (SEC) for the Obstacle."
                    GoTo ReturnResult
                End If

            End If

ReturnResult:

            If lsErrMsg <> "" Then
                lbResult = False
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)
            End If

        Catch ex As Exception
            Throw ex
        End Try


        Return lbResult

    End Function


    Protected Sub btnPlotObs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPlotObs.Click
        Try
            Response.Redirect("ADMS_ObstacleGraph.aspx?ICAO=" & txtICAO_Display.Text & "&RwyId=" & _
                              txtRwyId_Display.Text & "&RwyMod=" & txtVersion_Display.Text)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while getting Plot for obstacle details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub BtnCreateObstacle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCreateObstacle.Click
        Try
            HdnObstacleId.Value = ""
            BtnUpdate.Text = "Add"


            ClearFormForNewObstacle()
            ChkActive.Checked = False
            PnlObstacleDetails.Enabled = True
            PnlObstacleDetails.Visible = True
            ''btnDelete.Enabled = False
            btnDelete.Enabled = True ' Changed by swami  on 01-03-2024 
            'Delete obstacle button becomes inactive on all existing obstacles after 'Add obstacle' button is clicked	Ensure obstacle button remains active after 'Add obstacle' button is clicked            '
            TxtLatDeg.Text = 0
            TxtLatMin.Text = 0
            TxtLatSec.Text = 0.0

            TxtLonDeg.Text = 0
            TxtLonMin.Text = 0
            TxtLonSec.Text = 0.0

            DivObstacleDetails.Attributes.Add("style", "display:inline;")
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
            '                "window.setTimeout('open_Obstacle_Details_box()',500);", True)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Error while cancelling obstacle details.\n" & _
                            ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnShowRunway_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowRunway.Click
        Try
            Response.Redirect("ADMS_RunwayDM.aspx?ICAO=" & txtICAO_Display.Text & "&RwyId=" & txtRwyId_Display.Text & "&RwyMod=" & txtVersion_Display.Text)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Error while closing obstacle details.\n" & _
                            ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Try
            'Delete obstacle
            Dim objObstacleBO As New ADMS_BAL_ODM
            Dim liResult As Integer

            Try
                liResult = objObstacleBO.Delete_Obstacle(Session("UserId"), HdnObstacleId.Value, txtICAO_Display.Text, txtRwyId_Display.Text, txtVersion_Display.Text)
                If liResult = 1 Then
                    ' ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    '"Message", "alert('Successfully deleted the obstacle details." & "');", True)
                    DivObstacleDetails.Attributes.Add("style", "display:none;")
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Failed to delete obstacle details; please contact support team." & "');", True)
                End If
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Error while deleting obstacle details. please contact support team. \n" & _
                        ex.Message.Replace("'", "") & "');", True)
                Exit Sub
            End Try
            ShowObstacles()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Error while deleting obstacle details.\n" & _
                            ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub
End Class