Public Partial Class ChoiceListAdd
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Choice List Details."
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

                If Not Request.QueryString("CHID") Is Nothing Then
                    txtChoiceListID.Text = Request.QueryString("CHID")
                    GetChoiceList()
                Else
                    BtnAdd.Text = "ADD"
                    ClearControls()
                End If

                If Session("DatabaseUpgrading") = "0" Then
                    DisableControls()
                End If

                BtnAdd.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If
    End Sub
    
   
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click
        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If
        Dim intResult As Integer = 0
        Try
            If txtChoiceListID.Enabled = True Then
                intResult = go_Bo.CreateChoice_List(txtChoiceListID.Text.ToString.Trim, txtDescription.Text.ToString.Trim, _
                                               RadioButtonList1.SelectedValue.ToString.Trim, _
                                                 Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Successfully Added a new Choice List.');", True)

                    GetVersionNo()
                    ClearControls()
                    'Response.Redirect("ChoiceList.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
                    'PrepareForFristLoad(hidTableId.Value)
                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Choice List already exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while Adding a new Choice List.');", True)
                End If

            Else
                intResult = go_Bo.UpdateChoice_List(txtChoiceListID.Text.ToString.Trim, txtDescription.Text.ToString.Trim, _
                                               RadioButtonList1.SelectedValue.ToString.Trim, _
                                                 Convert.ToInt32(hidVersionNo.Value.ToString.Trim))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert(' Successfully updated the Choice List.');", True)

                    'Response.Redirect("ChoiceList.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)

                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Choice List does not exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Error occured while updating the Choice List.');", True)
                End If




            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Choice List : " & ex.ToString & ". ');", True)

        End Try


    End Sub
    Private Sub DisableControls()
        BtnAdd.Visible = False
        txtChoiceListID.Enabled = False
        txtDescription.Enabled = False
    End Sub
    Private Sub ClearControls()
        BtnAdd.Text = "Add"
        txtChoiceListID.Text = ""
        txtDescription.Text = ""
        txtChoiceListID.Focus()
    End Sub
    Private Sub GetChoiceList()

        Dim dtChoice_list As DataTable
        Dim strFilter As String = ""

        Try
            dtChoice_list = go_Bo.Get_ChoiceList(txtChoiceListID.Text.Trim.Replace(" ", "_"))

            If dtChoice_list.Rows.Count > 0 Then
                'txtChoiceListId.Text = dtChoice_list.Rows(0)("Choice_list_id").ToString.Trim
                txtDescription.Text = dtChoice_list.Rows(0)("description").ToString.Trim
                hidVersionNo.Value = dtChoice_list.Rows(0)("Version_No").ToString.Trim
                RadioButtonList1.SelectedValue = dtChoice_list.Rows(0)("Is_Active")
            End If

            BtnAdd.Text = "Update"
            txtChoiceListID.Enabled = False

            txtChoiceListID.Focus()
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

                If Not txtChoiceListID Is Nothing AndAlso String.Equals(txtChoiceListID.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Choice List Id.")
                    bReturn = False
                End If

                If Not txtDescription Is Nothing AndAlso String.Equals(txtDescription.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Description.")
                    bReturn = False
                End If


            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Choice List.');", True)
        End Try

        Return bReturn
    End Function
    Private Sub GetVersionNo()

        Dim dtChoice_List As DataTable

        Try
            dtChoice_List = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtChoice_List.Rows.Count > 0 Then
                hidVersionNo.Value = dtChoice_List.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("ChoiceList.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub
End Class