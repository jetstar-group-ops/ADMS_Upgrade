Public Partial Class RoleManagement
    Inherits System.Web.UI.Page

    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.RoleAdministration, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Create and Update Roles."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If

                BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")
                PrepareForFristLoad(True)
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If

        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub

    Private Sub PrepareForFristLoad(ByVal FillLists As Boolean)

        Dim loRoles As DataTable
        Dim loGroups As DataTable

        Try
            loRoles = go_Bo.GetRoles()

            gvRoles.DataSource = loRoles
            gvRoles.DataBind()

            If FillLists = True Then

                loGroups = go_Bo.GetPagesFunctionalities("")
                LstFunctionalities.DataTextField = "Functionality_Name"
                LstFunctionalities.DataValueField = "Functionality_Id"
                LstFunctionalities.DataSource = loGroups
                LstFunctionalities.DataBind()

            Else
                SetAllListItems(False, LstFunctionalities)

            End If

            HdnRoleId.Value = ""
            BtnSave.Text = "Create Role"
            BtnCancel.Visible = False
            BtnClear.Visible = True
            TxtRoleDescription.Text = ""
            TxtRoleName.Text = ""
            TxtRoleName.Focus()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub SetItemsFromDataTable(ByRef LstControl As System.Web.UI.WebControls.CheckBoxList, _
                         ByVal poDataTable As DataTable, ByVal psValueFieldName As String)

        Try

            For I = 0 To poDataTable.Rows.Count - 1
                If LstControl.Items.FindByValue(poDataTable.Rows(I)(psValueFieldName)) IsNot Nothing Then
                    LstControl.Items.FindByValue(poDataTable.Rows(I)(psValueFieldName)).Selected = True
                End If
            Next

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub SetAllListItems(ByVal NewValue As Boolean, ByRef lstControl As System.Web.UI.WebControls.CheckBoxList)
        Try
            For I = 0 To lstControl.Items.Count - 1
                lstControl.Items(I).Selected = NewValue
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function IsValueIsSelectedInList(ByVal Value As String, _
                    ByRef lstControl As System.Web.UI.WebControls.CheckBoxList) As Boolean

        Dim Result As Boolean = False
        Try
            For I = 0 To lstControl.Items.Count - 1
                If lstControl.Items(I).Value = Value And lstControl.Items(I).Selected = True Then
                    Result = True
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return Result
    End Function


    Private Function GetSelectedItemsIds(ByRef lstControl As System.Web.UI.WebControls.CheckBoxList) As String

        Dim SelectedItemsIds As String = ""
        Try
            For I = 0 To lstControl.Items.Count - 1
                If lstControl.Items(I).Selected = True Then
                    If SelectedItemsIds = "" Then
                        SelectedItemsIds = lstControl.Items(I).Value
                    Else
                        SelectedItemsIds = SelectedItemsIds & "," & lstControl.Items(I).Value
                    End If
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return SelectedItemsIds

    End Function

    Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click

        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If

        Dim DelayLocationIds As String = ""
        Dim EtdLocationIds As String = ""
        Dim intResult As Integer = 0
        Dim lsSelectedfunctionalities As String = ""

        Try
            lsSelectedfunctionalities = GetSelectedItemsIds(LstFunctionalities)

            If HdnRoleId.Value = "" Then

                Dim IsExist As Boolean
                IsExist = go_Bo.IsRoleNameExists(TxtRoleName.Text.Trim(), 0)

                If IsExist Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Role name already exists.');", True)
                Else
                    intResult = go_Bo.CreateRole(TxtRoleName.Text, TxtRoleDescription.Text, _
                                lsSelectedfunctionalities, Session("UserId"))

                    If intResult = 1 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                            "alert('Successfully created a new role.');", True)

                        PrepareForFristLoad(False)
                    Else
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                            "alert('Error occured while creating a new role.');", True)
                    End If

                End If

            Else

                Dim IsExist As Boolean
                IsExist = go_Bo.IsRoleNameExists(TxtRoleName.Text.Trim(), Convert.ToInt32(ViewState("CurrentRoleId").ToString()))

                If IsExist Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                           "alert('Role name already exists.');", True)
                Else
                    intResult = go_Bo.UpdateRole(HdnRoleId.Value, TxtRoleName.Text, TxtRoleDescription.Text, _
                               lsSelectedfunctionalities, Session("UserId"))

                    If intResult = 1 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                           "alert(' Successfully updated the role.');", True)
                        PrepareForFristLoad(False)
                    Else
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                           "alert('Error occured while updating the role. ');", True)
                    End If
                End If

            End If



        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Creating\updating the role : " & ex.ToString & ". ');", True)

        End Try
    End Sub


    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Try
            HdnRoleId.Value = ""
            TxtRoleName.Text = ""
            TxtRoleDescription.Text = ""
            SetAllListItems(False, LstFunctionalities)
            gvRoles.EditIndex = -1
            PrepareForFristLoad(False)
            TxtRoleName.Focus()
            BtnSave.Text = "Create Role"
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                 ex.ToString, True)
        End Try

    End Sub

    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        Try
            HdnRoleId.Value = ""
            TxtRoleName.Text = ""
            TxtRoleDescription.Text = ""
            SetAllListItems(False, LstFunctionalities)
            TxtRoleName.Focus()
            BtnSave.Text = "Create Role"
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                 ex.ToString, True)
        End Try

    End Sub

    Private Sub gvRoles_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvRoles.RowDeleting
        Dim intRoleId As Integer = 0
        Dim intRoleType As Integer
        Try
            intRoleId = CInt(CType(gvRoles.Rows(e.RowIndex).Cells(2).FindControl("hidRoleId"), HiddenField).Value)
            intRoleType = CInt(CType(gvRoles.Rows(e.RowIndex).Cells(2).FindControl("hidRoleType"), HiddenField).Value)

            If intRoleType = WNB_Common.Enums.RoleTypes.SystemRole Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('This Role is a System Role, which can not be deleted.');", True)

            Else
                DeleteroleDetails(intRoleId)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the role : " & ex.ToString & ". ');", True)
        End Try
    End Sub

    Private Sub gvRoles_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvRoles.RowEditing
        Dim intRoleId As Integer = 0
        Dim strRoleName As String
        Dim strRoleDescription As String
        Dim intRoleType As Integer
        Dim loRolefunctionalities As DataTable

        Try

            intRoleId = CInt(CType(gvRoles.Rows(e.NewEditIndex).Cells(2).FindControl("hidRoleId"), HiddenField).Value)
            intRoleType = CInt(CType(gvRoles.Rows(e.NewEditIndex).Cells(2).FindControl("hidRoleType"), HiddenField).Value)
            strRoleName = gvRoles.Rows(e.NewEditIndex).Cells(0).Text
            strRoleDescription = gvRoles.Rows(e.NewEditIndex).Cells(1).Text
            ViewState("CurrentRoleId") = intRoleId

            If intRoleType = WNB_Common.Enums.RoleTypes.SystemRole Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('This Role is a System Role, which can not be edited.');", True)
            Else
                loRolefunctionalities = go_Bo.GetRoleFunctionalities(intRoleId)

                SetAllListItems(False, LstFunctionalities)
                SetItemsFromDataTable(LstFunctionalities, loRolefunctionalities, "Functionality_Id")

                HdnRoleId.Value = intRoleId
                BtnSave.Text = "Update"
                BtnCancel.Visible = True
                BtnClear.Visible = False

                TxtRoleDescription.Text = strRoleDescription
                TxtRoleName.Text = strRoleName
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the role : " & ex.ToString & ". ');", True)

        End Try

        
    End Sub

    Private Sub DeleteroleDetails(ByVal intRoleId As Integer)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.Deleterole(intRoleId)

            If intResult = 1 Then
                HdnRoleId.Value = ""
                TxtRoleName.Text = ""
                TxtRoleDescription.Text = ""
                SetAllListItems(False, LstFunctionalities)
                gvRoles.EditIndex = -1
                PrepareForFristLoad(False)
                TxtRoleName.Focus()
                BtnSave.Text = "Create Role"


                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Role details have been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while deleting Role details.');", True)
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

                If Not TxtRoleName Is Nothing AndAlso String.Equals(TxtRoleName.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Role Name.")
                    bReturn = False
                End If

                If Not TxtRoleDescription Is Nothing AndAlso String.Equals(TxtRoleDescription.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Role Description.")
                    bReturn = False
                End If

                lsSelectedfunctionalities = GetSelectedItemsIds(LstFunctionalities)
                If lsSelectedfunctionalities.Equals(String.Empty) Then
                    strErrorMessage.Append("\n - Required least one functionality for the role.")
                    bReturn = False
                End If

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Role details.');", True)
        End Try

        Return bReturn
    End Function
End Class