Public Partial Class AircraftConfigAdjustments
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Aircraft Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If


                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If


                GetAircrafts()

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If
    End Sub

    Private Sub GetAircrafts()

        Dim dtAircrafts As DataTable

        Try
            dtAircrafts = go_Bo.Get_Aircrafts("")

            ddlAircraftID.DataSource = dtAircrafts
            ddlAircraftID.DataBind()

            ddlAircraftID.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetSubFleet()

        Dim dtSublfleet As DataTable

        Try
            dtSublfleet = go_Bo.Get_Subfleet(ddlAircraftID.SelectedValue.ToString, "")

            ddlSubfleetID.DataSource = dtSublfleet
            ddlSubfleetID.DataBind()
            ddlSubfleetID.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlAirCraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraftID.SelectedIndexChanged
        If (ddlAircraftID.SelectedValue.ToString <> String.Empty) Then
            GetSubFleet()
        End If
    End Sub

    Protected Sub ddlSublfeetID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSubfleetID.SelectedIndexChanged



        Dim dtGallyArms As DataTable


        Try
            If (ddlSubfleetID.SelectedIndex <> -1) Then


                dtGallyArms = go_Bo.Get_Aircraft_Config_Adjustments(hidTableId.Value.Trim, ddlAircraftID.SelectedValue.ToString, ddlSubfleetID.SelectedValue.ToString)
                gvAircraftConfigAdj.DataSource = dtGallyArms
                gvAircraftConfigAdj.DataBind()


            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim intResult As Integer = 0
        Dim strAircraftId As String = ""
        Dim strSubfleetId As String = ""
        Dim intVersionNo As Integer = 0
        Dim isEnabled As String = ""
        Dim AirCraftCLID As System.Int64 = 0
        Dim Arm As Decimal = 0.0
        Dim EmptyWeight As Integer = 0




        Try



            Dim GalleyDesignation As String

            For intRCnt As Integer = 0 To gvAircraftConfigAdj.Rows.Count - 1

                GalleyDesignation = gvAircraftConfigAdj.Rows(intRCnt).Cells(0).Text

                strAircraftId = gvAircraftConfigAdj.Rows(intRCnt).Cells(0).Text
                If (CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(5).FindControl("hidVersionNo"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(5).FindControl("hidVersionNo"), HiddenField).Value)
                End If
                If (CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(1).FindControl("ddlEnabled"), DropDownList).Text <> "") Then
                    isEnabled = Convert.ToInt32(CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(1).FindControl("ddlEnabled"), DropDownList).Text)
                    If (isEnabled = "0") Then
                        isEnabled = 0
                    Else
                        isEnabled = 1
                    End If
                Else
                    isEnabled = ""
                End If
                If (CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(2).FindControl("TxtEmptyWeight"), TextBox).Text <> "") Then
                    EmptyWeight = Convert.ToInt32(CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(2).FindControl("TxtEmptyWeight"), TextBox).Text)
                End If
                If (CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(3).FindControl("TxtArm"), TextBox).Text <> "") Then
                    Arm = CDec(CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(3).FindControl("TxtArm"), TextBox).Text)
                End If
                If (CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(4).FindControl("HidReferenceCLID"), HiddenField).Value <> "") Then
                    AirCraftCLID = Convert.ToInt64(CType(gvAircraftConfigAdj.Rows(intRCnt).Cells(4).FindControl("HidReferenceCLID"), HiddenField).Value)
                End If
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If
                'CABIN_CREW_DESIGN
                intResult = go_Bo.Create_Update_AirCraftConfigAdj(Convert.ToInt64(AirCraftCLID), GalleyDesignation, isEnabled, EmptyWeight, Arm, ddlAircraftID.SelectedValue.ToString, intVersionNo, ddlSubfleetID.SelectedValue.ToString, "Admin")

            Next


            If intResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert(' Successfully updated the Crew Galley Arm.');", True)

            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Crew Galley Arm does not exists.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert('Error occured while updating the Crew Galley Arm.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Crew Weight Arms : " & ex.ToString & ". ');", True)

        End Try

    End Sub
End Class