Public Partial Class UserManagement
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()
    Private go_Users As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""

            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.UserAdministration, _
                                    "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permissions to Create and Update User Profiles."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If
                BtnUpdate.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

                FillUsers()
                FlllLocations()
                FillRoles()
                BtnUpdate.Text = "Create User"
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If

        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub

    Private Sub FillUsers()
        Try
            go_Users = go_Bo.GetUsers()
            gvUsers.DataSource = go_Users
            gvUsers.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FillRoles()
        Dim loRoles As DataTable

        Try
            loRoles = go_Bo.GetRoles

            LstRoles.DataTextField = "Role_Name"
            LstRoles.DataValueField = "Role_Id"
            LstRoles.DataSource = loRoles
            LstRoles.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FlllLocations()

        Dim dtPorts As DataTable

        Try

            dtPorts = go_Bo.GetStations()
            ddlLocation.DataSource = dtPorts
            ddlLocation.DataBind()
            ddlLocation.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub CreateUpdateUserProfile()

        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If

        Dim RoleIds As String = ""
        Dim strUserId As String = ""
        Dim strLocationId As String = ""
        Dim strPassword As String = ""
        Dim intResult As Integer = 0
        Dim intIsDisabled As Integer = 0
        Dim dtUsers As DataTable

        Try

            strUserId = TxtUserId.Text.Trim
            strLocationId = ddlLocation.SelectedValue
            intIsDisabled = IIf(ChkDisabled.Checked = True, 1, 0)
            strPassword = TxtPassword.Text.Trim
            RoleIds = SelectedRoles()


            If BtnUpdate.Text.Trim() = "Create User" Then
                dtUsers = go_Bo.GetUserProfile(strUserId, 0)
                If dtUsers.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('User already exists with the same name.');", True)
                    Exit Sub
                End If
            Else
                dtUsers = go_Bo.GetUserProfile(strUserId, Convert.ToInt32(hdnUserSNO.Value))
                If dtUsers.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Unable to update,user already exists with the same name.');", True)
                    Exit Sub
                End If
            End If

            intResult = go_Bo.CreateUpdateUserProfile(strUserId, strLocationId, strPassword, intIsDisabled, RoleIds)

            If intResult = 1 Then
                If BtnUpdate.Text.Trim() <> "Update" Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Successfully created a new user.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert('Successfully updated user.');", True)
                End If
                SetAllListItems(False, LstRoles)
                TxtUserId.Text = ""
                TxtPassword.Text = ""
                ddlLocation.SelectedIndex = 0
                ChkDisabled.Checked = False
                LstRoles.Enabled = True
                ChkDisabled.Enabled = True
                BtnUpdate.Text = "Create User"
                BtnCancel.Visible = False
                BtnClear.Visible = True
                TxtUserId.Enabled = True
                gvUsers.EditIndex = -1
                FillUsers()
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while creating/updating user.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                      "alert('Error occured while creating/updating user : " & ex.Message.ToString & ".');", True)
        End Try

    End Sub

    
    Private Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        CreateUpdateUserProfile()
    End Sub

    Private Function SelectedRoles() As String
        Dim RoleIds As String = ""

        Try
            For I = 0 To LstRoles.Items.Count - 1
                If LstRoles.Items(I).Selected = True Then
                    If RoleIds = "" Then
                        RoleIds = LstRoles.Items(I).Value
                    Else
                        RoleIds = RoleIds & "," & LstRoles.Items(I).Value
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return RoleIds

    End Function

    Private Sub SetAllListItems(ByVal NewValue As Boolean, ByRef lstControl As System.Web.UI.WebControls.CheckBoxList)
        Try
            For I As Integer = 0 To lstControl.Items.Count - 1
                lstControl.Items(I).Selected = NewValue
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub gvUsers_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvUsers.RowDeleting
        Dim strUserId As String = ""
        Dim intUserType As Integer = 0
        Dim intUserSNO As Integer = 0

        Try
            intUserSNO = CInt(CType(gvUsers.Rows(e.RowIndex).Cells(4).FindControl("hidUserSNO"), HiddenField).Value)
            intUserType = CInt(CType(gvUsers.Rows(e.RowIndex).Cells(4).FindControl("hidUserType"), HiddenField).Value)
            strUserId = gvUsers.Rows(e.RowIndex).Cells(0).Text

            
            If intUserType = WNB_Common.Enums.UserTypes.SystemUser Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('This user is a System User, which can not be deleted.');", True)

            Else
                DeleteroleDetails(intUserSNO, strUserId)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the role : " & ex.ToString & ". ');", True)
        End Try
    End Sub



    Private Sub gvUsers_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvUsers.RowEditing
        Dim strUserId As String = ""
        Dim strLocationId As String = ""
        Dim strIsdisabled As String = ""
        Dim strRoles As String = ""
        Dim strPassword As String = ""
        Dim intUserType As Integer = 0
        Dim intUserSNO As Integer = 0

        Try

            intUserSNO = CInt(CType(gvUsers.Rows(e.NewEditIndex).Cells(4).FindControl("hidUserSNO"), HiddenField).Value)
            intUserType = CInt(CType(gvUsers.Rows(e.NewEditIndex).Cells(4).FindControl("hidUserType"), HiddenField).Value)
            strPassword = CType(gvUsers.Rows(e.NewEditIndex).Cells(4).FindControl("hidPassword"), HiddenField).Value
            strUserId = gvUsers.Rows(e.NewEditIndex).Cells(0).Text
            strLocationId = gvUsers.Rows(e.NewEditIndex).Cells(1).Text
            strIsdisabled = gvUsers.Rows(e.NewEditIndex).Cells(2).Text

            strRoles = CType(gvUsers.Rows(e.NewEditIndex).Cells(3).FindControl("LblRoles"), Label).Text
            'strRoles = gvUsers.Rows(e.NewEditIndex).Cells(3).Text

            If intUserType = WNB_Common.Enums.UserTypes.SystemUser Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('This user is a System User, which can not be edited.');", True)

            Else

                TxtUserId.Text = ""
                BtnUpdate.Text = "Update"
                BtnCancel.Visible = True
                BtnClear.Visible = False
                TxtUserId.Visible = True
                TxtUserId.Text = strUserId
                hdnUserSNO.Value = intUserSNO
                TxtPassword.Text = strPassword
                TxtUserId.Enabled = False

                If ddlLocation.Items.FindByValue(strLocationId) IsNot Nothing Then
                    ddlLocation.SelectedIndex = -1
                    ddlLocation.Items.FindByValue(strLocationId).Selected = True
                End If

                ChkDisabled.Checked = IIf(strIsdisabled = "Yes", True, False)

                SetAllListItems(False, LstRoles)

                strRoles = strRoles.Replace("<BR>", ",")

                For I = 0 To strRoles.Split(",").Count - 1
                    If LstRoles.Items.FindByText(strRoles.Split(",")(I)) IsNot Nothing Then
                        LstRoles.Items.FindByText(strRoles.Split(",")(I)).Selected = True
                    End If
                Next



                If intUserType = WNB_Common.Enums.UserTypes.SystemUser Then
                    LstRoles.Enabled = False
                    ChkDisabled.Enabled = False
                    ChkDisabled.Checked = False
                Else
                    LstRoles.Enabled = True
                    ChkDisabled.Enabled = True
                End If

            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the User : " & ex.ToString & ". ');", True)

        End Try

    End Sub

    Private Sub DeleteroleDetails(ByVal intUserSNO As Integer, ByVal strUserId As String)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.DeleteUser(intUserSNO, strUserId)

            If intResult = 1 Then

                SetAllListItems(False, LstRoles)
                TxtUserId.Text = ""
                TxtPassword.Text = ""
                ddlLocation.SelectedIndex = 0
                ChkDisabled.Checked = False
                gvUsers.EditIndex = -1
                FillUsers()
                LstRoles.Enabled = True
                ChkDisabled.Enabled = True
                BtnUpdate.Text = "Create User"
                BtnCancel.Visible = False
                BtnClear.Visible = True
                TxtUserId.Enabled = True

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('User details have been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while deleting User details.');", True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        Try
            SetAllListItems(False, LstRoles)
            TxtUserId.Text = ""
            TxtPassword.Text = ""
            ddlLocation.SelectedIndex = 0
            ChkDisabled.Checked = False
            LstRoles.Enabled = True
            ChkDisabled.Enabled = True
            BtnUpdate.Text = "Create User"
            BtnCancel.Visible = False
            BtnClear.Visible = True
            TxtUserId.Enabled = True
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                 ex.ToString, True)
        End Try
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Try
            SetAllListItems(False, LstRoles)
            TxtUserId.Text = ""
            TxtPassword.Text = ""
            ddlLocation.SelectedIndex = 0
            ChkDisabled.Checked = False
            LstRoles.Enabled = True
            ChkDisabled.Enabled = True
            BtnUpdate.Text = "Create User"
            BtnCancel.Visible = False
            BtnClear.Visible = True
            TxtUserId.Enabled = True
            gvUsers.EditIndex = -1
            FillUsers()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                 ex.ToString, True)
        End Try

    End Sub

    Private Function IsValidate(ByVal strCmd As String) As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder
        Dim RoleIds As String = ""

        Try
            If Not strCmd Is Nothing AndAlso (String.Equals(strCmd, "CREATE") Or String.Equals(strCmd, "UPDATE")) Then

                If Not TxtUserId Is Nothing AndAlso String.Equals(TxtUserId.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required User Id.")
                    bReturn = False
                End If

                If Not TxtPassword Is Nothing AndAlso String.Equals(TxtPassword.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Password.")
                    bReturn = False
                End If

                If ddlLocation.SelectedIndex <= 0 Then
                    strErrorMessage.Append("\n - Required Base Location.")
                    bReturn = False
                End If

                RoleIds = SelectedRoles()
                If RoleIds.Equals(String.Empty) Then
                    strErrorMessage.Append("\n - Required least one Role for the user.")
                    bReturn = False
                End If

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating CSO User details.');", True)
        End Try

        Return bReturn
    End Function
End Class