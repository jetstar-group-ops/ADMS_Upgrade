Partial Public Class SubfleetDetails
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


                GetAircrafts()
                GetVersionNo()
                If Not Request.QueryString("AID") Is Nothing Then
                    ddlAircraftId.Text = Request.QueryString("AID")

                End If

                If Not Request.QueryString("SFID") Is Nothing Then
                    txtSubfleetId.Text = Request.QueryString("SFID")
                    GetSubfleetDetail()
                Else
                    BtnSave.Text = "ADD"
                    ClearControls()
                End If







                If Session("DatabaseUpgrading") = "0" Then
                    DisableControls()
                End If

                BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

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

            ddlAircraftId.DataSource = dtAircrafts
            ddlAircraftId.DataBind()

            ddlAircraftId.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSave.Click
        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If
        Dim intResult As Integer = 0
        Try

            If ddlAircraftId.Enabled = True Then


                intResult = go_Bo.CreateSubfleet(txtSubfleetId.Text.ToString.Trim, ddlAircraftId.SelectedValue.ToString.Trim, _
                                                txtMaxTaxiWt.Text.ToString.Trim, txtMaxTakeoffWt.Text.ToString.Trim, txtLandingWt.Text.ToString.Trim, _
                                                txtMaxZeroFuelWt.Text.ToString.Trim, txtFlightDeckWt.Text.ToString.Trim, txtCabinCrewWt.Text.ToString.Trim, _
                                                 Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Successfully Added a new Subfleet.');", True)

                    GetVersionNo()
                    ClearControls()
                    'Response.Redirect("Subfleet.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
                    'PrepareForFristLoad(hidTableId.Value)
                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Subfleet already exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while Adding a new Subfleet.');", True)
                End If

            Else

                intResult = go_Bo.UpdateSubfleet(txtSubfleetId.Text.ToString.Trim, ddlAircraftId.SelectedValue.ToString.Trim, _
                                                txtMaxTaxiWt.Text.ToString.Trim, txtMaxTakeoffWt.Text.ToString.Trim, txtLandingWt.Text.ToString.Trim, _
                                                txtMaxZeroFuelWt.Text.ToString.Trim, txtFlightDeckWt.Text.ToString.Trim, txtCabinCrewWt.Text.ToString.Trim, _
                                                 Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert(' Successfully updated the Subfleet.');", True)

                    'Response.Redirect("Subfleet.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)

                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Subfleet details does not exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Error occured while updating the Subfleet.');", True)
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Subfleet : " & ex.ToString & ". ');", True)

        End Try


    End Sub
    Private Sub DisableControls()
        BtnSave.Visible = False
        ddlAircraftId.Enabled = False
        txtSubfleetId.Enabled = False
        txtMaxTaxiWt.Enabled = False
        txtMaxTakeoffWt.Enabled = False
        txtLandingWt.Enabled = False
        txtMaxZeroFuelWt.Enabled = False
        txtFlightDeckWt.Enabled = False
        txtCabinCrewWt.Enabled = False
    End Sub
    Private Sub ClearControls()
        BtnSave.Text = "Add"
        ddlAircraftId.Enabled = True
        ddlAircraftId.Text = ""
        txtSubfleetId.Text = ""
        txtMaxTaxiWt.Text = ""
        txtMaxTakeoffWt.Text = ""
        txtLandingWt.Text = ""
        txtMaxZeroFuelWt.Text = ""
        txtFlightDeckWt.Text = ""
        txtCabinCrewWt.Text = ""
        ddlAircraftId.Focus()
    End Sub
    Private Sub GetSubfleetDetail()

        Dim dtSubfleet As DataTable
        Dim strFilter As String = ""

        Try
            dtSubfleet = go_Bo.Get_Subfleet(ddlAircraftId.SelectedValue.ToString.Trim, txtSubfleetId.Text)

            If dtSubfleet.Rows.Count > 0 Then
                'txtSubfleetId.Text = dtSubfleet.Rows(0)("subfleet_Id").ToString.Trim
                txtMaxTaxiWt.Text = dtSubfleet.Rows(0)("max_taxi_weight").ToString.Trim
                txtMaxTakeoffWt.Text = dtSubfleet.Rows(0)("max_takeoff_weight").ToString.Trim
                txtLandingWt.Text = dtSubfleet.Rows(0)("max_landing_weight").ToString.Trim
                txtMaxZeroFuelWt.Text = dtSubfleet.Rows(0)("max_zero_fuel_weight").ToString.Trim
                txtFlightDeckWt.Text = dtSubfleet.Rows(0)("flight_deck_weight").ToString.Trim
                txtCabinCrewWt.Text = dtSubfleet.Rows(0)("cabin_crew_weight").ToString.Trim
                hidVersionNo.Value = dtSubfleet.Rows(0)("Version_No").ToString.Trim
            End If

            BtnSave.Text = "Update"
            ddlAircraftId.Enabled = False

            ddlAircraftId.Focus()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function IsValidate(ByVal strCmd As String) As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder
        Dim lsSelectedfunctionalities As String = ""

        Try
            If Not strCmd Is Nothing AndAlso (String.Equals(strCmd, "CREATE") Or String.Equals(strCmd, "UPDATE")) Then

                If Not ddlAircraftId Is Nothing AndAlso String.Equals(ddlAircraftId.SelectedItem.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Aircraft Id.")
                    bReturn = False
                End If

                If Not txtSubfleetId Is Nothing AndAlso String.Equals(txtSubfleetId.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Subfleet Id.")
                    bReturn = False
                End If


                If Not txtMaxTaxiWt Is Nothing AndAlso Not String.Equals(txtMaxTaxiWt.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMaxTaxiWt.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Max Taxi Weight.")
                        bReturn = False
                    End If
                End If

                If Not txtMaxTakeoffWt Is Nothing AndAlso Not String.Equals(txtMaxTakeoffWt.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMaxTakeoffWt.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Max Takeoff Weight.")
                        bReturn = False
                    End If
                End If


                If Not txtLandingWt Is Nothing AndAlso Not String.Equals(txtLandingWt.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtLandingWt.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Landing Weight.")
                        bReturn = False
                    End If
                End If

                If Not txtMaxZeroFuelWt Is Nothing AndAlso Not String.Equals(txtMaxZeroFuelWt.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMaxZeroFuelWt.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Max zero fuel weight Weight.")
                        bReturn = False
                    End If
                End If

                If Not txtFlightDeckWt Is Nothing AndAlso Not String.Equals(txtFlightDeckWt.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtFlightDeckWt.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Flight deck weight.")
                        bReturn = False
                    End If
                End If

                If Not txtCabinCrewWt Is Nothing AndAlso Not String.Equals(txtCabinCrewWt.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtCabinCrewWt.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Cabin Crew Weight.")
                        bReturn = False
                    End If
                End If

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Subfleet details.');", True)
        End Try

        Return bReturn
    End Function
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
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("Subfleet.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub

    Protected Sub ddlAircraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraftId.SelectedIndexChanged
        If (BtnSave.Text <> "Add") Then
            GetSubfleetDetail()
        End If

    End Sub
End Class