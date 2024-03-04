Imports WNB_Admin_BO

Partial Public Class ADMS_APTCategory
    Inherits System.Web.UI.Page
#Region "--Events--"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
        Dim lsMessage As String = ""

        If loBO.IsUserHasPermission(Session("UserId"), _
            WNB_Common.Enums.Functionalities.ADMS, "", 0) = False Then

            lsMessage = "You don't have permission on Airport Database Management System."
            Response.Redirect("../Home.aspx?Message=" & lsMessage)
            Exit Sub
        End If

        If IsPostBack = False Then

            Dim ls_Message As String = ""
            'Dim objBO As New BO

            btnUpdate.Visible = False
            'btnCancel.Visible = False

            BindGridViewData()

            btnCreate.Attributes.Add("onClick", "return ValidateControls('INSERT');")
            btnUpdate.Attributes.Add("onClick", "return ValidateControls('UPDATE');")
        End If
    End Sub
    Private Sub gvData_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvData.RowEditing
        Dim strCategoryId, strCategoryDesc As String
        strCategoryId = CStr(CType(gvData.Rows(e.NewEditIndex).Cells(2).FindControl("hidCatId"), HiddenField).Value)
        strCategoryDesc = CStr(CType(gvData.Rows(e.NewEditIndex).Cells(2).FindControl("hidCatDesc"), HiddenField).Value)
        EditCategory(strCategoryId, strCategoryDesc)
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If

        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_APTCategory
        Try

            intResult = objBO.UpdateCategory(txtCategoryId.Text.Trim, txtCategory.Text.Trim, Session("UserId"))

            If intResult = 1 Then
                btnUpdate.Visible = False
                'btnCancel.Visible = False
                btnCreate.Visible = True
                'btnClear.Visible = True
                Clearcontrols()
                txtCategoryId.ReadOnly = False
                gvData.EditIndex = -1
                BindGridViewData()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Category details have been updated successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Updating Category details.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while Updating Category details.');", True)
        End Try
    End Sub
    Private Sub gvData_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvData.RowDeleting
        Dim strSelectedCat As String
        strSelectedCat = CStr(CType(gvData.Rows(e.RowIndex).Cells(2).FindControl("hidCatId"), HiddenField).Value)
        DeleteCategory(strSelectedCat)
    End Sub
    Private Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        If Not IsValidate("INSERT") Then
            Exit Sub
        End If

        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_APTCategory

        Try
            intResult = objBO.CreateCategory(txtCategoryId.Text.Trim, txtCategory.Text.Trim, Session("UserId"))

            If intResult = 1 Then
                BindGridViewData()
                Clearcontrols()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Category has been saved successfully.');", True)

            ElseIf intResult = 0 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Category details already exist for " & txtCategoryId.Text & " .');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Creating Category.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while Creating Category.');", True)
        End Try
    End Sub
    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Try
            Clearcontrols()
            If btnUpdate.Visible = True Then
                btnUpdate.Visible = False
                btnCreate.Visible = True
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    'Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    '    txtCategoryId.ReadOnly = False
    '    btnUpdate.Visible = False
    '    'btnCancel.Visible = False
    '    btnCreate.Visible = True
    '    btnClear.Visible = True
    '    Clearcontrols()
    '    gvData.EditIndex = -1
    '    BindGridViewData()
    'End Sub
#End Region

#Region "--Methos Or Functions--"

    ''' <summary>
    ''' To Delete Category
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteCategory(ByVal strSelectedCat As String)
        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_APTCategory

        Try
            intResult = objBO.DeleteCategory(strSelectedCat, Session("UserId"))

            If intResult = 1 Then
                BindGridViewData()
                btnUpdate.Visible = False
                'btnCancel.Visible = False
                btnCreate.Visible = True
                'btnClear.Visible = True

                Clearcontrols()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Category has been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Category is in use, could not delete.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while deleting Category.');", True)
        End Try
    End Sub
    ''' <summary>
    ''' Validate Page controls for different action
    ''' </summary>
    ''' <remarks></remarks>
    Private Function IsValidate(ByVal strCmd As String) As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder

        Try
            If Not strCmd Is Nothing AndAlso (String.Equals(strCmd, "INSERT") Or String.Equals(strCmd, "UPDATE")) Then

                If String.IsNullOrEmpty(txtCategoryId.Text) Then
                    strErrorMessage.Append("\n - Required Category Id.")
                    bReturn = False
                End If

                If String.IsNullOrEmpty(txtCategory.Text) Then
                    strErrorMessage.Append("\n - Required Category.")
                    bReturn = False
                End If

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Category.');", True)
        End Try

        Return bReturn
    End Function
    ''' <summary>
    ''' Bind GridView with datasource
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindGridViewData()
        Dim dtBU As New DataTable
        Dim objBO As New ADMS_BAL_APTCategory

        Try
            dtBU = objBO.GetCategories(Session("UserId"))

            gvData.DataSource = dtBU
            gvData.DataBind()
            gvData.EditIndex = -1
            btnUpdate.Visible = False
            'btnCancel.Visible = False
            btnCreate.Visible = True
            'btnClear.Visible = True
            Clearcontrols()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' Clear vlaue of Page controls
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Clearcontrols()
        Try
            txtCategory.Text = ""
            txtCategoryId.Text = ""
            txtCategoryId.ReadOnly = False
            txtCategoryId.Focus()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' To Update Category, page controls should be filled with selected Category details
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditCategory(ByVal strCatId As String, ByVal strCatDesc As String)
        Dim dtSelected As New DataTable

        Try
            txtCategoryId.Text = strCatId
            txtCategory.Text = strCatDesc

            btnUpdate.Visible = True
            'btnCancel.Visible = True
            btnCreate.Visible = False
            'btnClear.Visible = False

            txtCategoryId.ReadOnly = True

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Response.Redirect("ADMS_Home.aspx")
    End Sub

End Class