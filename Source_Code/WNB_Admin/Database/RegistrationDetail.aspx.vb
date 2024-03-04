Partial Public Class RegistrationDetail
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
                If Not Request.QueryString("AID") Is Nothing Then


                    ddlAirCraftId.Text = Request.QueryString("AID")


                End If

                If Not Request.QueryString("RID") Is Nothing Then
                    hdRegistrationID.Value = Request.QueryString("RID").ToString()
                    GetRegistrationDetails()
                Else
                    BtnSave.Text = "ADD"
                    ClearControls()
                End If


                If Session("DatabaseUpgrading") = "0" Then
                    DisableControls()

                End If
                'BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

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






                intResult = go_Bo.CreateRegistration(0, txtRegistrationNumber.Text.ToString.Trim, ddlAirCraftId.SelectedValue.ToString.Trim, _
                                                txtMSN.Text.ToString.Trim, txtSeatConfiguration.Text.ToString.Trim, txtLoadDataSheet.Text.ToString.Trim, _
                                                txtBasicWt.Text.ToString.Trim, txtBasicArm.Text.ToString.Trim, txtSubFleet.Text.ToString.Trim, _
                                                 Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Successfully Added a new Registration.');", True)

                    GetVersionNo()
                    ClearControls()
                    'Response.Redirect("Registration.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
                    'PrepareForFristLoad(hidTableId.Value)
                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Subfleet already exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while Adding a new Registration.');", True)
                End If

            Else

                intResult = go_Bo.UpdateRegistration(hdRegistrationID.Value, txtRegistrationNumber.Text.ToString.Trim, ddlAirCraftId.SelectedValue.ToString.Trim, _
                                                txtMSN.Text.ToString.Trim, txtSeatConfiguration.Text.ToString.Trim, txtLoadDataSheet.Text.ToString.Trim, _
                                                txtBasicWt.Text.ToString.Trim, txtBasicArm.Text.ToString.Trim, txtSubFleet.Text.ToString.Trim, _
                                                 Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert(' Successfully updated the Registration.');", True)

                    'Response.Redirect("Subfleet.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)

                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Registration details does not exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Error occured while updating the Registration.');", True)
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Registration : " & ex.ToString & ". ');", True)

        End Try


    End Sub
    
    Private Sub DisableControls()
        BtnSave.Visible = False
        ddlAirCraftId.Enabled = False
        txtRegistrationNumber.Enabled = False
        txtMSN.Enabled = False
        txtSubFleet.Enabled = False
        txtSeatConfiguration.Enabled = False
        txtLoadDataSheet.Enabled = False
        txtBasicWt.Enabled = False
        txtBasicArm.Enabled = False

    End Sub
    Private Sub ClearControls()
        BtnSave.Text = "Add"
        ddlAircraftId.Enabled = True
        ddlAircraftId.Text = ""
        txtRegistrationNumber.Text = ""
        txtSubFleet.Text = ""
        txtMSN.Text = ""
        txtSeatConfiguration.Text = ""
        txtLoadDataSheet.Text = ""
        txtBasicWt.Text = ""
        txtBasicArm.Text = ""
        ddlAircraftId.Focus()
    End Sub

    Private Sub GetRegistrationDetails()

        Dim RegistrationID = Convert.ToUInt32(hdRegistrationID.Value)
        Dim dtRegistation As DataTable
        Try
            dtRegistation = go_Bo.Get_Registration(ddlAirCraftId.SelectedValue.ToString.Trim, RegistrationID)
            If dtRegistation.Rows.Count > 0 Then


                txtMSN.Text = dtRegistation.Rows(0)("MSN").ToString.Trim
                txtRegistrationNumber.Text = dtRegistation.Rows(0)("registration_number").ToString.Trim
                txtSeatConfiguration.Text = dtRegistation.Rows(0)("seat_configuration").ToString.Trim
                txtLoadDataSheet.Text = dtRegistation.Rows(0)("load_data_sheet_ref").ToString.Trim
                txtBasicWt.Text = dtRegistation.Rows(0)("basic_weight").ToString.Trim
                txtSubFleet.Text = dtRegistation.Rows(0)("subfleet_id").ToString.Trim
                txtBasicArm.Text = dtRegistation.Rows(0)("basic_arm").ToString.Trim
                hidVersionNo.Value = dtRegistation.Rows(0)("Version_No").ToString.Trim

            End If
            BtnSave.Text = "Update"
            ddlAirCraftId.Enabled = False

            ddlAirCraftId.Focus()
        Catch ex As Exception

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

                If Not txtRegistrationNumber Is Nothing AndAlso String.Equals(txtRegistrationNumber.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Registration Number.")
                    bReturn = False
                End If


                If Not txtSeatConfiguration Is Nothing AndAlso String.Equals(txtSeatConfiguration.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Seat Configuration.")
                    bReturn = False
                End If


                If Not txtLoadDataSheet Is Nothing AndAlso String.Equals(txtLoadDataSheet.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Load Data Sheet.")
                    bReturn = False
                End If

                If Not txtBasicWt Is Nothing AndAlso String.Equals(txtBasicWt.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Basic Wt.")
                    bReturn = False
                End If

                If Not txtSubFleet Is Nothing AndAlso String.Equals(txtSubFleet.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Sub Fleet.")
                    bReturn = False
                End If

                If Not txtBasicArm Is Nothing AndAlso String.Equals(txtBasicArm.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Basic Arm.")
                    bReturn = False
                End If

                

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Registration details.');", True)
        End Try

        Return bReturn
    End Function
    Private Sub GetVersionNo()

        Dim dtRegistation As DataTable

        Try
            dtRegistation = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtRegistation.Rows.Count > 0 Then
                hidVersionNo.Value = dtRegistation.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("Registration.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub
    Protected Sub ddlAircraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAirCraftId.SelectedIndexChanged
        If (BtnSave.Text <> "Add") Then
            GetRegistrationDetails()
        End If

    End Sub
End Class