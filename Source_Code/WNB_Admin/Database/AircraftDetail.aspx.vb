Public Partial Class AircraftDetail
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

                If Not Request.QueryString("AID") Is Nothing Then
                    txtAirCraftId.Text = Request.QueryString("AID").ToString()
                    GetAircraftDetail()
                Else
                    GetVersionNo()
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

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSave.Click
        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If

        Dim intResult As Integer = 0

        Try

            If txtAirCraftId.Enabled = True Then

               
                intResult = go_Bo.CreateAircraft(txtAirCraftId.Text.ToString.Trim, txtModelName.Text.ToString.Trim, _
                                                txtRCO.Text.ToString.Trim, txtRCL.Text.ToString.Trim, txtRS.Text.ToString.Trim, _
                                                txtCC.Text.ToString.Trim, txtCK.Text.ToString.Trim, txtMinOPWTAdj.Text.ToString.Trim, _
                                                txtMaxOPWTAdj.Text.ToString.Trim, txtMinOPIUAdj.Text.ToString.Trim, _
                                                txtMaxOPIUAdj.Text.ToString.Trim, Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Successfully Added a new Aircraft.');", True)

                    GetVersionNo()
                    ClearControls()
                    'Response.Redirect("Aircraft.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
                    'PrepareForFristLoad(hidTableId.Value)
                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Aircraft already exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while Adding a new Aircraft.');", True)
                End If

            Else

                intResult = go_Bo.UpdateAircraft(txtAirCraftId.Text.ToString.Trim, txtModelName.Text.ToString.Trim, _
                                                txtRCO.Text.ToString.Trim, txtRCL.Text.ToString.Trim, txtRS.Text.ToString.Trim, _
                                                txtCC.Text.ToString.Trim, txtCK.Text.ToString.Trim, txtMinOPWTAdj.Text.ToString.Trim, _
                                                txtMaxOPWTAdj.Text.ToString.Trim, txtMinOPIUAdj.Text.ToString.Trim, _
                                                txtMaxOPIUAdj.Text.ToString.Trim, Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert(' Successfully updated the Aircraft.');", True)

                    'Response.Redirect("Aircraft.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)

                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Aircraft details does not exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Error occured while updating the Aircraft.');", True)
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Aircraft : " & ex.ToString & ". ');", True)

        End Try

    End Sub

    Private Sub DisableControls()
        BtnSave.Visible = False
        txtAirCraftId.Enabled = False
        txtModelName.Enabled = False
        txtCC.Enabled = False
        txtCK.Enabled = False
        txtRCL.Enabled = False
        txtRCO.Enabled = False
        txtRS.Enabled = False
        txtMaxOPIUAdj.Enabled = False
        txtMaxOPWTAdj.Enabled = False
        txtMinOPIUAdj.Enabled = False
        txtMinOPWTAdj.Enabled = False
    End Sub


    Private Sub ClearControls()
        BtnSave.Text = "Add"
        txtAirCraftId.Enabled = True
        txtAirCraftId.Text = ""
        txtModelName.Text = ""
        txtCC.Text = ""
        txtCK.Text = ""
        txtRCL.Text = ""
        txtRCO.Text = ""
        txtRS.Text = ""
        txtMaxOPIUAdj.Text = ""
        txtMaxOPWTAdj.Text = ""
        txtMinOPIUAdj.Text = ""
        txtMinOPWTAdj.Text = ""
        txtAirCraftId.Focus()
    End Sub

    Private Sub GetAircraftDetail()

        Dim dtAirCraft As DataTable
        Dim strFilter As String = ""

        Try
            dtAirCraft = go_Bo.Get_Aircrafts(txtAirCraftId.Text.ToString.Trim)

            If dtAirCraft.Rows.Count > 0 Then
                txtModelName.Text = dtAirCraft.Rows(0)("model_name").ToString.Trim
                txtCC.Text = dtAirCraft.Rows(0)("IU_equ_const_C").ToString.Trim
                txtCK.Text = dtAirCraft.Rows(0)("IU_equ_const_K").ToString.Trim
                txtMaxOPIUAdj.Text = dtAirCraft.Rows(0)("max_op_IU_adjustment").ToString.Trim
                txtMaxOPWTAdj.Text = dtAirCraft.Rows(0)("max_op_weight_adjustment").ToString.Trim
                txtMinOPIUAdj.Text = dtAirCraft.Rows(0)("min_op_IU_adjustment").ToString.Trim
                txtMinOPWTAdj.Text = dtAirCraft.Rows(0)("min_op_weight_adjustment").ToString.Trim
                txtRCL.Text = dtAirCraft.Rows(0)("ref_chord_origin").ToString.Trim
                txtRCO.Text = dtAirCraft.Rows(0)("ref_chord_length").ToString.Trim
                txtRS.Text = dtAirCraft.Rows(0)("ref_station").ToString.Trim
                hidVersionNo.Value = dtAirCraft.Rows(0)("Version_No").ToString.Trim
            End If

            BtnSave.Text = "Update"
            txtAirCraftId.Enabled = False
            txtAirCraftId.Focus()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetVersionNo()

        Dim dtAirCraft As DataTable

        Try
            dtAirCraft = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)
           
            If dtAirCraft.Rows.Count > 0 Then
                hidVersionNo.Value = dtAirCraft.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If
            
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

                If Not txtAirCraftId Is Nothing AndAlso String.Equals(txtAirCraftId.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Aircraft Id.")
                    bReturn = False
                End If

                If Not TxtModelName Is Nothing AndAlso String.Equals(TxtModelName.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Aircraft Model Name.")
                    bReturn = False
                End If

                
                If Not txtCC Is Nothing AndAlso Not String.Equals(txtCC.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtCC.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Constant _ C.")
                        bReturn = False
                    End If
                End If

                If Not txtCK Is Nothing AndAlso Not String.Equals(txtCK.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtCK.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Constant _ K.")
                        bReturn = False
                    End If
                End If


                If Not txtRCO Is Nothing AndAlso Not String.Equals(txtRCO.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+(\.[0-9]{1,4})?$")
                    If Not Expression.IsMatch(txtRCO.Text) Then
                        strErrorMessage.Append("\n - Enter numeric values with up to 4 decimal places for Reference Chord Original.")
                        bReturn = False
                    End If
                End If

                If Not txtRCL Is Nothing AndAlso Not String.Equals(txtRCL.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+(\.[0-9]{1,4})?$")
                    If Not Expression.IsMatch(txtRCL.Text) Then
                        strErrorMessage.Append("\n - Enter numeric values with up to 4 decimal places for Reference Chord Length.")
                        bReturn = False
                    End If
                End If

                If Not txtRS Is Nothing AndAlso Not String.Equals(txtRS.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtRS.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Reference Station.")
                        bReturn = False
                    End If
                End If

                If Not txtMinOPWTAdj Is Nothing AndAlso Not String.Equals(txtMinOPWTAdj.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMinOPWTAdj.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Minimum OP Weight Adjustment.")
                        bReturn = False
                    End If
                End If


                If Not txtMaxOPWTAdj Is Nothing AndAlso Not String.Equals(txtMaxOPWTAdj.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMaxOPWTAdj.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Maximum OP Weight Adjustment.")
                        bReturn = False
                    End If
                End If

                If Not txtMinOPIUAdj Is Nothing AndAlso Not String.Equals(txtMinOPIUAdj.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMinOPIUAdj.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Minimum OP IU Adjustment.")
                        bReturn = False
                    End If
                End If


                If Not txtMaxOPIUAdj Is Nothing AndAlso Not String.Equals(txtMaxOPIUAdj.Text.Trim(), String.Empty) Then
                    Dim Expression As New System.Text.RegularExpressions.Regex("^[0-9]+$")
                    If Not Expression.IsMatch(txtMaxOPIUAdj.Text) Then
                        strErrorMessage.Append("\n - Enter integer values for Maximum OP IU Adjustment.")
                        bReturn = False
                    End If
                End If


            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Aircraft details.');", True)
        End Try

        Return bReturn
    End Function

    

    

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("Aircraft.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub
End Class