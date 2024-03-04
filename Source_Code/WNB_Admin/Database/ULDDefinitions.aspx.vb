Public Partial Class ULDDefinitions
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update ULD Definition details."
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
                GetULDDefinition()

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

            ddlAircraft.DataSource = dtAircrafts
            ddlAircraft.DataBind()

            ddlAircraft.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetULDDefinition()

        Dim dtULDDefinition As DataTable


        Try
            If (ddlAircraft.SelectedIndex <> -1) Then


                dtULDDefinition = go_Bo.Get_ULDDefiniton("26", ddlAircraft.SelectedValue.ToString)
                gvULDDefinition.DataSource = dtULDDefinition
                gvULDDefinition.DataBind()



            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub ddlAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraft.SelectedIndexChanged
        If (ddlAircraft.SelectedValue.ToString <> String.Empty) Then
            GetULDDefinition()
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        Dim intResult As Integer = 0
        Dim strAircraftId As String = ""
        Dim intVersionNo As Integer = 0
        Dim AllowCargo As System.Int32 = 0
        Dim UldDefinitionID As Int32 = 0
        Dim allow_bags As Int32 = 0
        Dim tare_weight As Int32 = 0
        Dim Arm As Decimal = 0.0

        Try

            Dim uld_cl_id As String

            For intRCnt As Integer = 0 To gvULDDefinition.Rows.Count - 1

                uld_cl_id = gvULDDefinition.Rows(intRCnt).Cells(0).Text

                'strAircraftId = gvULDDefinition.Rows(intRCnt).Cells(0).Text
                
                If (CType(gvULDDefinition.Rows(intRCnt).Cells(1).FindControl("ddlallowcargo"), DropDownList).Text <> "") Then
                    AllowCargo = Convert.ToInt32(CType(gvULDDefinition.Rows(intRCnt).Cells(1).FindControl("ddlallowcargo"), DropDownList).Text)
                End If
                If (CType(gvULDDefinition.Rows(intRCnt).Cells(2).FindControl("ddlallowbags"), DropDownList).Text <> "") Then
                    allow_bags = Convert.ToInt32(CType(gvULDDefinition.Rows(intRCnt).Cells(2).FindControl("ddlallowbags"), DropDownList).Text)
                End If
                If (CType(gvULDDefinition.Rows(intRCnt).Cells(3).FindControl("txttareweight"), TextBox).Text <> "") Then
                    tare_weight = Convert.ToInt32(CType(gvULDDefinition.Rows(intRCnt).Cells(3).FindControl("txttareweight"), TextBox).Text)
                End If
                If (CType(gvULDDefinition.Rows(intRCnt).Cells(4).FindControl("hidUldDefinitionID"), HiddenField).Value <> "") Then
                    UldDefinitionID = Convert.ToInt32(CType(gvULDDefinition.Rows(intRCnt).Cells(4).FindControl("hidUldDefinitionID"), HiddenField).Value)
                End If
                If (CType(gvULDDefinition.Rows(intRCnt).Cells(5).FindControl("hidVersionNo"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvULDDefinition.Rows(intRCnt).Cells(5).FindControl("hidVersionNo"), HiddenField).Value)
                End If
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If
                'Add/Update the ULD Definition
                intResult = go_Bo.Create_Update_ULDDefinition(UldDefinitionID, uld_cl_id, Convert.ToBoolean(AllowCargo), Convert.ToBoolean(allow_bags), tare_weight, ddlAircraft.SelectedValue.Trim, intVersionNo)

            Next


            If intResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert(' Successfully updated the ULD Definition.');", True)

            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('ULD Definition does not exists.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert('Error occured while updating the ULD Definition.');", True)
            End If






        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the ULD Definition : " & ex.ToString & ". ');", True)

        End Try
    End Sub
End Class