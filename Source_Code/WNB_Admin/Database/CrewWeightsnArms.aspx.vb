Partial Public Class CrewWeightsnArms
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
                GetVersionNo()

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

            ddlAirCraftId.DataSource = dtAircrafts
            ddlAirCraftId.DataBind()

            ddlAirCraftId.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetSubFleet()

        Dim dtSublfleet As DataTable

        Try
            dtSublfleet = go_Bo.Get_Subfleet(ddlAirCraftId.SelectedValue.ToString, "")

            ddlSublfeet.DataSource = dtSublfleet
            ddlSublfeet.DataBind()
            ddlSublfeet.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlAirCraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAirCraftId.SelectedIndexChanged
        If (ddlAirCraftId.SelectedValue.ToString <> String.Empty) Then
            GetSubFleet()
        End If
    End Sub

    Protected Sub ddlSublfeet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSublfeet.SelectedIndexChanged



        Dim dtFlightDeck As DataTable
        Dim dtCabinCrew As DataTable

        Try
            If (ddlSublfeet.SelectedIndex <> -1) Then


                dtFlightDeck = go_Bo.Get_GalleyArms(hidTableId.Value.Trim, ddlAirCraftId.SelectedValue.ToString, "FLIGHT_DECK", ddlSublfeet.SelectedValue.ToString)
                gvFlightDeck.DataSource = dtFlightDeck
                gvFlightDeck.DataBind()


                dtCabinCrew = go_Bo.Get_GalleyArms(hidTableId.Value.Trim, ddlAirCraftId.SelectedValue.ToString, "CABIN_CREW", ddlSublfeet.SelectedValue.ToString)
                gvCabinCrew.DataSource = dtCabinCrew
                gvCabinCrew.DataBind()

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
        Dim CrewGallyArmID As System.Int64 = 0
        Dim Arm As Decimal = 0.0



        Try



            Dim GalleyDesignation As String

            For intRCnt As Integer = 0 To gvFlightDeck.Rows.Count - 1

                GalleyDesignation = gvFlightDeck.Rows(intRCnt).Cells(0).Text

                strAircraftId = gvFlightDeck.Rows(intRCnt).Cells(0).Text
                If (CType(gvFlightDeck.Rows(intRCnt).Cells(2).FindControl("hidVersionNo"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvFlightDeck.Rows(intRCnt).Cells(2).FindControl("hidVersionNo"), HiddenField).Value)
                End If
                If (CType(gvFlightDeck.Rows(intRCnt).Cells(1).FindControl("TxtArm"), TextBox).Text <> "") Then
                    Arm = CDec(CType(gvFlightDeck.Rows(intRCnt).Cells(1).FindControl("TxtArm"), TextBox).Text)
                End If
                If (CType(gvFlightDeck.Rows(intRCnt).Cells(3).FindControl("hidCrewGalleyArmID"), HiddenField).Value <> "") Then
                    CrewGallyArmID = Convert.ToInt64(CType(gvFlightDeck.Rows(intRCnt).Cells(3).FindControl("hidCrewGalleyArmID"), HiddenField).Value)
                End If
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If
                'FLIGHT_DECK_DESIG
                intResult = go_Bo.Create_Update_GalleyArms(Convert.ToInt64(CrewGallyArmID), GalleyDesignation, "FLIGHT_DECK", Arm, ddlAirCraftId.SelectedValue.ToString, intVersionNo, ddlSublfeet.SelectedValue.ToString, "Admin")

            Next


            For intRCnt As Integer = 0 To gvCabinCrew.Rows.Count - 1

                GalleyDesignation = gvCabinCrew.Rows(intRCnt).Cells(0).Text

                strAircraftId = gvCabinCrew.Rows(intRCnt).Cells(0).Text
                If (CType(gvCabinCrew.Rows(intRCnt).Cells(2).FindControl("hidVersionNo"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvCabinCrew.Rows(intRCnt).Cells(2).FindControl("hidVersionNo"), HiddenField).Value)
                End If
                If (CType(gvCabinCrew.Rows(intRCnt).Cells(1).FindControl("TxtArm"), TextBox).Text <> "") Then
                    Arm = CDec(CType(gvCabinCrew.Rows(intRCnt).Cells(1).FindControl("TxtArm"), TextBox).Text)
                End If
                If (CType(gvCabinCrew.Rows(intRCnt).Cells(3).FindControl("hidCrewGalleyArmID"), HiddenField).Value <> "") Then
                    CrewGallyArmID = Convert.ToInt64(CType(gvCabinCrew.Rows(intRCnt).Cells(3).FindControl("hidCrewGalleyArmID"), HiddenField).Value)
                End If
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If
                'CABIN_CREW_DESIGN
                intResult = go_Bo.Create_Update_GalleyArms(Convert.ToInt64(CrewGallyArmID), GalleyDesignation, "CABIN_CREW", Arm, ddlAirCraftId.SelectedValue.ToString, intVersionNo, ddlSublfeet.SelectedValue.ToString, "Admin")

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

    Private Sub GetVersionNo()

        Dim dtSubfleet As DataTable

        Try
            dtSubfleet = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtSubfleet.Rows.Count > 0 Then
                hidVersionNo.Value = dtSubfleet.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class