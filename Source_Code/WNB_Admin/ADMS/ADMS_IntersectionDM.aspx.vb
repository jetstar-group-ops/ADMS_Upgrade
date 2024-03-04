Imports WNB_Admin_BO

Partial Public Class ADMS_IntersectionDM
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
                DivIntersectionDetails.Attributes.Add("style", "display:none;")
                Dim liIntersectionId As Integer

                HdnIcao.Value = Request("Icao") & ""
                HdnRwyId.Value = Request("RwyId") & ""
                HdnRwyMod.Value = Request("RwyMod") & ""

                LblIcao.Text = Request("Icao") & ""
                LblRwyId.Text = Request("RwyId") & ""
                LblRwyMod.Text = Request("RwyMod") & ""


                If HdnIcao.Value.Trim = "" Or HdnRwyId.Value.Trim = "" Or HdnRwyMod.Value.Trim = "" Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                                "Message", "alert('Invalid request." & "');", True)
                    BtnCreateIntersection.Enabled = False
                    BtnShowrunwayDetails.Enabled = False
                    DgrdIntersections.Enabled = False
                    Exit Sub
                Else
                    Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
                    Dim loRunwayDetails As DataSet

                    loRunwayDetails = loADMS_BAL_RDM.GetRunWay(Session("UserId"), HdnIcao.Value, _
                                                               HdnRwyId.Value, HdnRwyMod.Value)

                    If loRunwayDetails.Tables(0).Rows.Count = 0 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Invalid runway details." & "');", True)
                        BtnCreateIntersection.Enabled = False
                        BtnShowrunwayDetails.Enabled = False
                        DgrdIntersections.Enabled = False
                        Exit Sub
                    End If
                End If

                'ShowIntersections()

                TxtIdent.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                TxtDeltaFieldLength.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtElevStartTORA.Attributes.Add("OnKeyPress", " return AllowNegativeValue(this);")

                TxtLatDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")

                TxtLonDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLonMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLonSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")




                If Request("Intersection_Id") <> "" Then

                    liIntersectionId = Val(Request("Intersection_Id") & "")
                    HdnIntersectionId.Value = liIntersectionId
                    GetAndShowIntersection(liIntersectionId)
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                    '            "window.setTimeout('open_Intersection_Details_box()',500);", True)

                ElseIf Request("Add") = "1" Then
                    BtnCreateIntersection_Click(sender, e)
                End If

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                     "Message", "alert('Error while getting intersections details. \n" & _
                     ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub ReadOnlyRightsToControls()
        Try
            BtnCreateIntersection.Enabled = False
            btnDelete.Enabled = False
            btnUpdate.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub ShowIntersections()

        Dim loIntersections As DataTable
        Dim loADMS_BAL_IDM As New ADMS_BAL_IDM

        loIntersections = loADMS_BAL_iDM.GetAllIntersections(Session("UserId") & "", _
                                    HdnIcao.Value, HdnRwyId.Value, HdnRwyMod.Value)
        DgrdIntersections.DataSource = loIntersections
        DgrdIntersections.DataBind()

    End Sub

    Private Sub DgrdIntersections_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DgrdIntersections.ItemCommand

        BtnUpdate.Text = "Update"
        Dim liIntersectionId As String = ""

        Try
            liIntersectionId = e.CommandArgument.ToString.Split(",")(1)
            HdnIntersectionId.Value = liIntersectionId

            If e.CommandArgument.ToString.StartsWith("E") = True Then
                'Edit intersection
                GetAndShowIntersection(liIntersectionId)
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                '            "window.setTimeout('open_Intersection_Details_box()',500);", True)

            ElseIf e.CommandArgument.ToString.StartsWith("D") = True Then
                'Delete intersection
                Dim loADMS_BAL_IDM As New ADMS_BAL_IDM
                Dim liResult As Integer

                Try
                    liResult = loADMS_BAL_IDM.DeleteIntersection(Session("UserId"), _
                            HdnIcao.Value, HdnRwyId.Value, HdnRwyMod.Value, liIntersectionId)
                    If liResult = 1 Then
                        'GetAndShowIntersection(liIntersectionId)
                        'ShowIntersections()
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                       "Message", "alert('Successfully deleted the intersection details." & "');", True)
                        DivIntersectionDetails.Attributes.Add("style", "display:none;")
                    Else
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                       "Message", "alert('Failed to delete intersection details; please contact support team." & "');", True)
                    End If
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                        "Message", "alert('Error while deleting intersection details. \n" & _
                            ex.Message.Replace("'", "") & "');", True)
                    Exit Sub
                End Try

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
                  "Message", "alert('Error while getting intersection details. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub GetAndShowIntersection(ByVal psIntersectionId As String)

        Dim loIntersectionDetails As DataSet
        Dim loADMS_BAL_IDM As New ADMS_BAL_IDM

        loIntersectionDetails = loADMS_BAL_IDM.GetIntersectionDetails(Session("UserId"), _
                HdnIcao.Value, HdnRwyId.Value, HdnRwyMod.Value, psIntersectionId)

        If loIntersectionDetails.Tables(0).Rows.Count = 0 Then
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Intersection not found." & "');", True)

            BtnCreateIntersection.Enabled = False
            BtnShowrunwayDetails.Enabled = False
            DgrdIntersections.Enabled = False

            Exit Sub
        Else
            If loIntersectionDetails.Tables(0).Rows(0)("ICAO") & "" <> HdnIcao.Value Or _
                loIntersectionDetails.Tables(0).Rows(0)("RwyId") & "" <> HdnRwyId.Value Or _
                loIntersectionDetails.Tables(0).Rows(0)("RwyMod") & "" <> HdnRwyMod.Value Then

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Invalid intersection." & "');", True)

                BtnCreateIntersection.Enabled = False
                BtnShowrunwayDetails.Enabled = False
                DgrdIntersections.Enabled = False

                Exit Sub

            End If
        End If

        ShowIntersectionDetails(loIntersectionDetails.Tables(0))

    End Sub

    Private Sub ShowIntersectionDetails(ByVal poIntersectionDetails As DataTable)

        If poIntersectionDetails.Rows.Count > 0 Then

            LblIcao.Text = poIntersectionDetails.Rows(0)("ICAO") & ""
            LblRwyId.Text = poIntersectionDetails.Rows(0)("RwyId") & ""
            LblRwyMod.Text = poIntersectionDetails.Rows(0)("RwyMod") & ""

            TxtIdent.Text = poIntersectionDetails.Rows(0)("Ident") & ""
            TxtDeltaFieldLength.Text = poIntersectionDetails.Rows(0)("DeltaFieldLength") & ""
            TxtElevStartTORA.Text = poIntersectionDetails.Rows(0)("ElevStartTORA") & ""

            LstLatDir.SelectedIndex = -1
            If LstLatDir.Items.FindByValue(poIntersectionDetails.Rows(0)("LatDir") & "") IsNot Nothing Then
                LstLatDir.Items.FindByValue(poIntersectionDetails.Rows(0)("LatDir") & "").Selected = True
            End If

            TxtLatDeg.Text = poIntersectionDetails.Rows(0)("LatDeg") & ""
            TxtLatMin.Text = poIntersectionDetails.Rows(0)("LatMin") & ""
            TxtLatSec.Text = Math.Round(Convert.ToDecimal(poIntersectionDetails.Rows(0)("LatSec")), 2) & ""

            LstLonDir.SelectedIndex = -1
            If LstLonDir.Items.FindByValue(poIntersectionDetails.Rows(0)("LonDir") & "") IsNot Nothing Then
                LstLonDir.Items.FindByValue(poIntersectionDetails.Rows(0)("LonDir") & "").Selected = True
            End If

            TxtLonDeg.Text = poIntersectionDetails.Rows(0)("LonDeg") & ""
            TxtLonMin.Text = poIntersectionDetails.Rows(0)("LonMin") & ""
            TxtLonSec.Text = Math.Round(Convert.ToDecimal(poIntersectionDetails.Rows(0)("LonSec")), 2) & ""

            If poIntersectionDetails.Rows(0)("Active") & "" <> "" Then
                ChkActive.Checked = poIntersectionDetails.Rows(0)("Active") 'IIf(Val(poAirportDetails.Rows(0)("Active") & "") = 1, True, False)
            End If

            LstLineUpAngle.SelectedIndex = -1
            If LstLineUpAngle.Items.FindByValue(poIntersectionDetails.Rows(0)("LineUpAngle") & "") IsNot Nothing Then
                LstLineUpAngle.Items.FindByValue(poIntersectionDetails.Rows(0)("LineUpAngle") & "").Selected = True
            End If

            LblUpdatedBy.Text = poIntersectionDetails.Rows(0)("ChangeUser") & ""
            If poIntersectionDetails.Rows(0)("ChangeDateTime") & "" <> "" Then
                LblUpdatedOn.Text = Date.Parse(poIntersectionDetails.Rows(0)("ChangeDateTime") & "").ToString("dd-MM-yyyy HH:mm")
            End If

            TxtComments.Text = poIntersectionDetails.Rows(0)("Comment") & ""

            DivIntersectionDetails.Attributes.Add("style", "display:inline;")
        End If
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Try
            'HdnIntersectionId.Value = ""
            'DivIntersectionDetails.Attributes.Add("style", "display:none;")

            Response.Redirect("ADMS_RunwayDM.aspx?ICAO=" & HdnIcao.Value & _
                         "&RwyId=" & HdnRwyId.Value & "&RwyMod=" & HdnRwyMod.Value)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                 "Message", "alert('Error while cancelling the edit operation.\n" & ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub BtnCreateIntersection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCreateIntersection.Click
        BtnUpdate.Text = "Add"

        ClearFormForNewAirport()
        btnDelete.Enabled = False
        HdnIntersectionId.Value = ""
        DivIntersectionDetails.Attributes.Add("style", "display:inline;")
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
        '                "window.setTimeout('open_Intersection_Details_box()',500);", True)

    End Sub

    Private Sub ClearFormForNewAirport()

        Try

            TxtIdent.Text = ""
            TxtDeltaFieldLength.Text = ""
            TxtElevStartTORA.Text = ""

            LstLatDir.SelectedIndex = 0

            TxtLatDeg.Text = ""
            TxtLatMin.Text = ""
            TxtLatSec.Text = ""

            LstLonDir.SelectedIndex = 0

            TxtLonDeg.Text = ""
            TxtLonMin.Text = ""
            TxtLonSec.Text = ""

            ChkActive.Checked = False

            LstLineUpAngle.SelectedIndex = 0

            LblUpdatedBy.Text = ""
            LblUpdatedOn.Text = ""

            TxtComments.Text = ""

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Try
            If ValidateData() = False Then
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                '       "window.setTimeout('open_Intersection_Details_box()',500);", True)
                Exit Sub
            End If

            Dim loIntersectionDetails As DataSet
            Dim loADMS_BAL_IDM As New ADMS_BAL_IDM
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loMyNewIntersectionDetails As New DataSet
            Dim liResult As Integer
            Dim loRunway As DataSet

            loIntersectionDetails = loADMS_BAL_IDM.GetIntersectionDetails(Session("UserId"), "-1", "-1", "-1", -1)
            loMyNewIntersectionDetails.Tables.Add(loIntersectionDetails.Tables(0).Copy)
            loMyNewIntersectionDetails.Tables(0).Columns("ChangeDateTime").DataType = System.Type.GetType("System.String")
            loIntersectionDetails.Dispose()

            loMyNewIntersectionDetails.Tables(0).Rows.Clear()

            Dim loDataRow As DataRow

            loDataRow = loMyNewIntersectionDetails.Tables(0).NewRow
            loMyNewIntersectionDetails.Tables(0).Rows.Add(loDataRow)
            loMyNewIntersectionDetails.Tables(0).AcceptChanges()

            PolukateIntersectionData(loMyNewIntersectionDetails.Tables(0))

            'Validating Runway active or not---------------------
            loRunway = loADMS_BAL_RDM.GetRunWay(Session("UserId"), loMyNewIntersectionDetails.Tables(0).Rows(0)("ICAO"), _
                                                loMyNewIntersectionDetails.Tables(0).Rows(0)("RwyId"), _
                                                loMyNewIntersectionDetails.Tables(0).Rows(0)("RwyMod"))
            If Not loRunway Is Nothing Then
                If loRunway.Tables(0).Rows.Count > 0 Then
                    If Convert.ToBoolean(loRunway.Tables(0).Rows(0)("Active")) = False Then
                        If Convert.ToBoolean(loMyNewIntersectionDetails.Tables(0).Rows(0)("Active")) = True Then
                            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                              "Message", "alert('The Runway of this intersection is Inactive, You can not make this intersection active." & "');", True)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            '--------------------------------

            'Validating duplicate intersections-----------------
            loIntersectionDetails = loADMS_BAL_IDM.GetIntersectionDetails(Session("UserId"), _
                                                                          loMyNewIntersectionDetails.Tables(0).Rows(0)("ICAO"), _
                                                                          loMyNewIntersectionDetails.Tables(0).Rows(0)("RwyId"), _
                                                                          loMyNewIntersectionDetails.Tables(0).Rows(0)("RwyMod"), _
                                                                          IIf(HdnIntersectionId.Value = "", 0, HdnIntersectionId.Value))
            If Not loIntersectionDetails Is Nothing Then
                If loIntersectionDetails.Tables(0).Rows.Count > 0 And HdnIntersectionId.Value = "" Then
                    Dim dtCheckDuplicate As DataTable
                    loIntersectionDetails.Tables(0).DefaultView.RowFilter = "Ident='" & loMyNewIntersectionDetails.Tables(0).Rows(0)("Ident") & "'"
                    dtCheckDuplicate = loIntersectionDetails.Tables(0).DefaultView.ToTable
                    If dtCheckDuplicate.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                        "Message", "alert('This intersection is already exist." & "');", True)
                        dtCheckDuplicate = Nothing
                        Exit Sub
                    End If
                End If
            End If
            loIntersectionDetails = Nothing
            '--------------------------------

            liResult = loADMS_BAL_IDM.CreateUpdateIntersection(loMyNewIntersectionDetails, _
                            Session("UserId"), HdnIcao.Value, HdnRwyId.Value, HdnRwyMod.Value, HdnIntersectionId.Value)

            If HdnIntersectionId.Value <> "" Then
                GetAndShowIntersection(HdnIntersectionId.Value)
            End If


            If liResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully updated the intersection details." & "');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to updated the intersection details; please contact support team." & "');", True)
            End If
            DivIntersectionDetails.Attributes.Add("style", "display:inline;")

            loRunway = Nothing
            loADMS_BAL_IDM = Nothing
            loADMS_BAL_RDM = Nothing
            loRunway = Nothing

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while updating intersection details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub PolukateIntersectionData(ByRef poAirportDetails As DataTable)

        poAirportDetails.Rows(0)("ICAO") = LblIcao.Text
        poAirportDetails.Rows(0)("RwyId") = LblRwyId.Text
        poAirportDetails.Rows(0)("RwyMod") = LblRwyMod.Text
        poAirportDetails.Rows(0)("Ident") = TxtIdent.Text
        poAirportDetails.Rows(0)("DeltaFieldLength") = TxtDeltaFieldLength.Text
        poAirportDetails.Rows(0)("ElevStartTORA") = TxtElevStartTORA.Text

        poAirportDetails.Rows(0)("LatDir") = LstLatDir.SelectedValue
        poAirportDetails.Rows(0)("LatDeg") = TxtLatDeg.Text
        poAirportDetails.Rows(0)("LatMin") = TxtLatMin.Text
        poAirportDetails.Rows(0)("LatSec") = TxtLatSec.Text

        poAirportDetails.Rows(0)("LonDir") = LstLonDir.SelectedValue
        poAirportDetails.Rows(0)("LonDeg") = TxtLonDeg.Text
        poAirportDetails.Rows(0)("LonMin") = TxtLonMin.Text
        poAirportDetails.Rows(0)("LonSec") = TxtLonSec.Text

        poAirportDetails.Rows(0)("Active") = IIf(ChkActive.Checked, 1, 0)
        poAirportDetails.Rows(0)("LineUpAngle") = LstLineUpAngle.SelectedValue
        poAirportDetails.Rows(0)("ChangeUser") = Session("UserId")
        poAirportDetails.Rows(0)("ChangeDateTime") = Date.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")

        poAirportDetails.Rows(0)("Comment") = TxtComments.Text

        poAirportDetails.Rows(0)("Intersection_Id") = Val(HdnIntersectionId.Value & "")

        'LblUpdatedOn.Text = Date.Now.ToString("dd-MM-yyyy HH:mm")
        'LblUpdatedBy.Text = Session("UserId")

    End Sub

    Private Function ValidateData() As Boolean

        Dim lsErrMsg As String = ""
        Dim lbResult As Boolean = True

        If TxtIdent.Text.Trim = "" Then
            lsErrMsg = "Please enter identification number for intersection."
            GoTo ReturnResult
        End If

        If ChkActive.Checked = True Then

            If TxtDeltaFieldLength.Text.Trim = "" Then
                lsErrMsg = "Please enter name for the intersection."
                GoTo ReturnResult
            End If

            If TxtElevStartTORA.Text.Trim = "" Then
                lsErrMsg = "Please enter displacement value the intersection."
                GoTo ReturnResult
            End If

            If TxtLatDeg.Text.Trim = "" Or TxtLatMin.Text.Trim = "" Or TxtLatSec.Text.Trim = "" Then
                lsErrMsg = "Please enter latitude details for the intersection."
                GoTo ReturnResult
            End If

            If TxtLonDeg.Text.Trim = "" Or TxtLonMin.Text.Trim = "" Or TxtLonSec.Text.Trim = "" Then
                lsErrMsg = "Please enter longitude details for the intersection."
                GoTo ReturnResult
            End If

            'validation of TxtElevStartTORA values
            If TxtElevStartTORA.Text.Trim = "" Then
                lsErrMsg = "Please enter Magnetic Variation (Degree) for the airport."
                GoTo ReturnResult
            End If





        End If

ReturnResult:

        If lsErrMsg <> "" Then
            lbResult = False
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
              "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)
        End If

        Return lbResult

    End Function

    Private Sub BtnShowrunwayDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnShowrunwayDetails.Click
        Response.Redirect("ADMS_RunwayDM.aspx?ICAO=" & HdnIcao.Value & _
                          "&RwyId=" & HdnRwyId.Value & "&RwyMod=" & HdnRwyMod.Value)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim loADMS_BAL_IDM As New ADMS_BAL_IDM
        Dim liResult As Integer

        Try
            liResult = loADMS_BAL_IDM.DeleteIntersection(Session("UserId"), _
                    HdnIcao.Value, HdnRwyId.Value, HdnRwyMod.Value, HdnIntersectionId.Value)
            If liResult = 1 Then

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully deleted the intersection details." & "');", True)

                Response.Redirect("ADMS_RunwayDM.aspx?ICAO=" & HdnIcao.Value & _
                       "&RwyId=" & HdnRwyId.Value & "&RwyMod=" & HdnRwyMod.Value)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to delete intersection details; please contact support team." & "');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while deleting intersection details. \n" & _
                    ex.Message.Replace("'", "") & "');", True)
            Exit Sub
        End Try
    End Sub
End Class

