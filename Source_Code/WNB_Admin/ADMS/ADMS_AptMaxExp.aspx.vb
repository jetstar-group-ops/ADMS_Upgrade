Imports WNB_Admin_BO

Partial Public Class ADMS_AptMaxExp
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

        Dim strAptMaxExpCode, strAptMaxExpDesc As String
        strAptMaxExpCode = CStr(CType(gvData.Rows(e.NewEditIndex).Cells(2).FindControl("hidAptMaxExpCode"), HiddenField).Value)
        strAptMaxExpDesc = CStr(CType(gvData.Rows(e.NewEditIndex).Cells(2).FindControl("hidAptMaxExpDesc"), HiddenField).Value)
        EditCategory(strAptMaxExpCode, strAptMaxExpDesc)

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Not IsValidate("UPDATE") Then
            Exit Sub
        End If

        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_AptMaxExp
        Try


            intResult = objBO.UpdateAptMaxExp(txtCode.Text.Trim, txtDesc.Text.Trim, Session("UserId"))

            If intResult = 1 Then
                btnUpdate.Visible = False
                'btnCancel.Visible = False
                btnCreate.Visible = True
                'btnClear.Visible = True
                Clearcontrols()
                txtCode.ReadOnly = False
                gvData.EditIndex = -1
                BindGridViewData()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Airport Max Code details have been updated successfully.');", True)
                txtCode.Focus()
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Updating Airport Max Code details.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while Updating Airport Max Code details.');", True)
        End Try
    End Sub

    Private Sub gvData_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvData.RowDeleting
        Dim strSelectedCat As String
        strSelectedCat = CStr(CType(gvData.Rows(e.RowIndex).Cells(2).FindControl("hidAptMaxExpCode"), HiddenField).Value)
        DeleteAirportMax(strSelectedCat)
    End Sub

    Private Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        If Not IsValidate("INSERT") Then
            Exit Sub
        End If

        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_AptMaxExp

        Try
            intResult = objBO.CreateAptMaxExp(txtCode.Text.Trim, txtDesc.Text.Trim, Session("UserId"))

            If intResult = 1 Then
                BindGridViewData()
                Clearcontrols()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Airport Max Code has been saved successfully.');", True)
                txtCode.Focus()

            ElseIf intResult = 0 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Airport Max Code details already exist for " & txtCode.Text & " .');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Creating Airport Max Code.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while Creating Airport Max Code.');", True)
        Finally
            objBO = Nothing
        End Try
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Clearcontrols()
        If btnUpdate.Visible = True Then
            btnUpdate.Visible = False
            btnCreate.Visible = True
        End If

    End Sub
    '
#End Region

#Region "--Methos Or Functions--"

    ''' <summary>
    ''' To Delete AirportMax
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteAirportMax(ByVal strSelectedCat As String)
        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_AptMaxExp

        Try
            intResult = objBO.DeleteAptMaxExp(strSelectedCat, Session("UserId"))

            If intResult = 1 Then
                BindGridViewData()
                btnUpdate.Visible = False
                'btnCancel.Visible = False
                btnCreate.Visible = True
                'btnClear.Visible = True

                Clearcontrols()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Airport Max Code has been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Airport Max Code is in use, could not deleted.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while deleting Airport Max Code.');", True)
        Finally
            objBO = Nothing
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

                If String.IsNullOrEmpty(txtCode.Text) Then
                    strErrorMessage.Append("\n - Required Code.")
                    bReturn = False
                End If

                If String.IsNullOrEmpty(txtDesc.Text) Then
                    strErrorMessage.Append("\n - Required Description.")
                    bReturn = False
                End If

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating AirportMax Code.');", True)
        End Try

        Return bReturn
    End Function
    ''' <summary>
    ''' Bind GridView with datasource
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindGridViewData()
        Dim dtBU As New DataTable
        Dim objBO As New ADMS_BAL_AptMaxExp

        Try
            dtBU = objBO.GetAptMaxExp(Session("UserId"))
            lblRcount.Text = "Record's :" + dtBU.Rows.Count.ToString()

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
        Finally
            objBO = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Clear vlaue of Page controls
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Clearcontrols()
        Try
            txtCode.Text = ""
            txtDesc.Text = ""
            txtCode.Focus()
            txtCode.ReadOnly = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' To Update AptMAxExp Code, page controls should be filled with selected AptMAxExp details
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditCategory(ByVal strCatId As String, ByVal strCatDesc As String)
        Dim dtSelected As New DataTable

        Try
            txtCode.Text = strCatId
            txtDesc.Text = strCatDesc

            btnUpdate.Visible = True
            btnCreate.Visible = False


            txtCode.ReadOnly = True

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Response.Redirect("ADMS_Home.aspx")
    End Sub

End Class