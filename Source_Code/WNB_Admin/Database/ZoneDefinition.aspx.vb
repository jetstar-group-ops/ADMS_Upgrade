Public Partial Class ZoneDefinition
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

            ddlSubfleetID.DataSource = dtSublfleet
            ddlSubfleetID.DataBind()
            ddlSubfleetID.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlAircraftID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraftID.SelectedIndexChanged
        If (ddlAircraftID.SelectedValue.ToString <> String.Empty) Then
            GetSubFleet()
        End If
    End Sub

    Protected Sub ddlSubfleetID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSubfleetID.SelectedIndexChanged
        Dim dtZoneDefinition As DataTable


        Try
            If (ddlSubfleetID.SelectedIndex <> -1) Then


                dtZoneDefinition = go_Bo.Get_ZoneDefinition("29", ddlAircraftID.SelectedValue.ToString, ddlSubfleetID.SelectedValue.ToString)
                gvZoneDefinition.DataSource = dtZoneDefinition
                gvZoneDefinition.DataBind()


                
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
        Dim zone_definition_id As System.Int64 = 0
        Dim max_capacity As Integer = 0
        Dim first_row_number As Integer = 0
        Dim last_row_number As Integer = 0
        Dim Description As String = String.Empty
        Dim Arm As Decimal = 0.0



        Try



            Dim designation_id As String

            For intRCnt As Integer = 0 To gvZoneDefinition.Rows.Count - 1

                designation_id = gvZoneDefinition.Rows(intRCnt).Cells(0).Text

                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(1).FindControl("TxtArm"), TextBox).Text <> "") Then
                    Arm = CDec(CType(gvZoneDefinition.Rows(intRCnt).Cells(1).FindControl("TxtArm"), TextBox).Text)
                End If
                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(2).FindControl("TxtMaxCapecity"), TextBox).Text <> "") Then
                    max_capacity = Convert.ToInt32(CType(gvZoneDefinition.Rows(intRCnt).Cells(2).FindControl("TxtMaxCapecity"), TextBox).Text)
                End If
                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(3).FindControl("TxtFirstRowNumber"), TextBox).Text <> "") Then
                    first_row_number = Convert.ToInt32(CType(gvZoneDefinition.Rows(intRCnt).Cells(3).FindControl("TxtFirstRowNumber"), TextBox).Text)
                End If
                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(4).FindControl("TxtLastRowNumber"), TextBox).Text <> "") Then
                    last_row_number = CDec(CType(gvZoneDefinition.Rows(intRCnt).Cells(4).FindControl("TxtLastRowNumber"), TextBox).Text)
                End If
                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(5).FindControl("TxtDescription"), TextBox).Text <> "") Then
                    Description = CDec(CType(gvZoneDefinition.Rows(intRCnt).Cells(5).FindControl("TxtDescription"), TextBox).Text)
                End If

                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(6).FindControl("HidVersion"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvZoneDefinition.Rows(intRCnt).Cells(6).FindControl("HidVersion"), HiddenField).Value)
                End If

                If (CType(gvZoneDefinition.Rows(intRCnt).Cells(7).FindControl("HidZoneDefinationID"), HiddenField).Value <> "") Then
                    zone_definition_id = Convert.ToInt64(CType(gvZoneDefinition.Rows(intRCnt).Cells(7).FindControl("HidZoneDefinationID"), HiddenField).Value)
                End If
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If

                intResult = go_Bo.Create_Update_ZoneDefination(Convert.ToInt64(zone_definition_id), designation_id, Arm, max_capacity, first_row_number, last_row_number, Description, ddlAircraftID.SelectedValue.ToString, intVersionNo, ddlSubfleetID.SelectedValue.ToString, "Admin")

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