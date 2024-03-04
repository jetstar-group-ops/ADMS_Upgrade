Public Partial Class ServiceDefinitionEdit
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Sub fleet Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If

                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If

                GetVersionNo()

                If Not Request.QueryString("SID") Is Nothing Then
                    HidServiceDefinitionId.Value = Request.QueryString("SID")
                End If

                If Not Request.QueryString("SDID") Is Nothing Then
                    hidServicedefclID.Value = Request.QueryString("SDID").ToString()
                End If

                If Not Request.QueryString("FDID") Is Nothing Then
                    hidflightdesigref.Value = Request.QueryString("FDID").ToString()
                Else
                    BtnSave.Text = "ADD"
                End If

                GetServiceDefinition()
                If Session("DatabaseUpgrading") = "0" Then
                    'DisableControls()
                End If

                'BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If
    End Sub
    Private Sub GetVersionNo()

        Dim dtOperationalLimit As DataTable

        Try
            dtOperationalLimit = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtOperationalLimit.Rows.Count > 0 Then
                hidVersionNo.Value = dtOperationalLimit.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetServiceDefinition()
        Dim dtServiceDefinition As DataTable
        dtServiceDefinition = go_Bo.Get_ServiceDefinition(HidServiceDefinitionId.Value, hidServicedefclID.Value, hidflightdesigref.Value)

        If (Not dtServiceDefinition Is Nothing And dtServiceDefinition.Rows.Count > 0) Then
            txtStartFlightNo.Text = dtServiceDefinition.Rows(0)("start_flight_number")
            txtEndFlightNo.Text = dtServiceDefinition.Rows(0)("end_flight_number")
            hidVersionNo.Value = dtServiceDefinition.Rows(0)("Version_no")
        End If
        
    End Sub
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSave.Click
        Dim intResult As Integer = 0
        Dim servdefID As Integer = 0
        Dim intVersionNo As Integer = 0
        Dim servdefclID As String = ""
        Dim flightdesigref As String = ""


        Dim end_flight_no As Integer = 0
        Try

            Dim start_flight_no As Integer = 0

            

            intResult = go_Bo.Create_Update_ServiceDefinition(HidServiceDefinitionId.Value, hidServicedefclID.Value, hidflightdesigref.Value, txtStartFlightNo.Text.Trim, txtEndFlightNo.Text.Trim, hidVersionNo.Value)




            If intResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert(' Successfully updated the Service Definition.');", True)

            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Service Definition does not exists.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert('Error occured while updating the Service Definition.');", True)
            End If






        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Operational Limit: " & ex.ToString & ". ');", True)

        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("ServiceDefinitions.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub
End Class