Public Partial Class ChoiceIDAdd
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Choice Details."
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
                    ddlAircraftId.Text = Request.QueryString("AID").ToString()
                End If

                GetAircrafts()
                GetVersionNo()
                If Not Request.QueryString("CID") Is Nothing Then
                    txtChoice.Text = Request.QueryString("CID")
                    GetChoices()

                Else
                    BtnAdd.Text = "ADD"
                    ClearControls()
                End If
                GetChoiceList()
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
    Private Sub GetAircrafts()

        Dim dtAircrafts As DataTable

        Try
            dtAircrafts = go_Bo.Get_Aircrafts("")

            ddlAircraftId.DataSource = dtAircrafts
            ddlAirCraftId.DataBind()

            ddlAirCraftId.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetChoiceList()

        Dim dtChoiceList As DataTable

        Try
            dtChoiceList = go_Bo.Get_ChoiceList(System.DBNull.Value.ToString)

            ddlChoiceList.DataSource = dtChoiceList
            ddlChoiceList.DataBind()

            ddlChoiceList.Items.Insert(0, New ListItem("", ""))

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

                If Not txtChoice Is Nothing AndAlso String.Equals(txtChoice.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Choice Id.")
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
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Choice.');", True)
        End Try

        Return bReturn
    End Function
    
    Private Sub GetVersionNo()

        Dim dtChoices As DataTable

        Try
            dtChoices = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtChoices.Rows.Count > 0 Then
                hidVersionNo.Value = dtChoices.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdd.Click
        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If
        Dim intResult As Integer = 0
        Try
            If txtChoice.Enabled = True Then
                intResult = go_Bo.CreateChoices(ddlAircraftId.SelectedValue.ToString.Trim, txtChoice.Text.ToString.Trim, ddlChoiceList.SelectedValue.ToString.Trim, txtDescription.Text.ToString.Trim, _
                                               RadioButtonList1.SelectedValue.ToString.Trim, _
                                                 RadioButtonList2.SelectedValue.ToString.Trim, Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Successfully Added a new Choice.');", True)

                    GetVersionNo()
                    ClearControls()
                    'Response.Redirect("Choice.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
                    'PrepareForFristLoad(hidTableId.Value)
                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Choice already exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while Adding a new Choice.');", True)
                End If

            Else
                intResult = go_Bo.UpdateChoices(txtChoice.Text.ToString.Trim, ddlChoiceList.SelectedValue.ToString.Trim, txtDescription.Text.ToString.Trim, _
                                               RadioButtonList1.SelectedValue.ToString.Trim, _
                                                 RadioButtonList2.SelectedValue.ToString.Trim, Convert.ToInt32(hidVersionNo.Value.ToString.Trim), _
                                                Session("UserId"))

                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert(' Successfully updated the Choice.');", True)

                    'Response.Redirect("ChoiceList.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)

                ElseIf intResult = 2 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Choice does not exists.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Error occured while updating the Choice.');", True)
                End If


            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Choice : " & ex.ToString & ". ');", True)

        End Try


    End Sub
    Private Sub DisableControls()
        BtnAdd.Visible = False
        txtChoice.Enabled = False
        txtDescription.Enabled = False
        RadioButtonList1.Enabled = False
        RadioButtonList2.Enabled = False
    End Sub
    Private Sub ClearControls()
        BtnAdd.Text = "Add"
        txtChoice.Text = ""
        txtDescription.Text = ""
        txtChoice.Focus()
    End Sub
    Private Sub GetChoices()

        Dim dtChoices As DataTable
        Dim strFilter As String = ""

        Try
            dtChoices = go_Bo.Get_Choices(ddlAircraftId.SelectedValue.ToString.Trim, txtChoice.Text)

            If dtChoices.Rows.Count > 0 Then
                txtChoice.Text = dtChoices.Rows(0)("Choices_id").ToString.Trim
                ddlChoiceList.Text = dtChoices.Rows(0)("Choice_list_id").ToString.Trim
                txtDescription.Text = dtChoices.Rows(0)("description").ToString.Trim
                hidVersionNo.Value = dtChoices.Rows(0)("Version_No").ToString.Trim


                RadioButtonList1.SelectedValue = dtChoices.Rows(0)("Is_Active")
                RadioButtonList2.SelectedValue = dtChoices.Rows(0)("Is_default")
            
            End If

            BtnAdd.Text = "Update"
            txtChoice.Enabled = False
            ddlChoiceList.Enabled = False
            ddlAircraftId.Enabled = False


            txtChoice.Focus()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("Choice.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub

   
End Class