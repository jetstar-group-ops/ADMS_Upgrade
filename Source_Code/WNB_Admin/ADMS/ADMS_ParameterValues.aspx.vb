Imports WNB_Admin_BO

Partial Public Class ADMS_ParameterValues
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
            Try
                Dim ls_Message As String = ""
                BindGridViewData()
                txtValue.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")
                btnUpdate.Attributes.Add("onClick", "return ValidateControls('UPDATE');")
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                         "Message", "alert('Error while getting airports details. \n" & _
                         ex.Message.Replace("'", "") & "');", True)
            End Try
        End If
    End Sub
    Private Sub gvData_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvData.RowEditing
        Dim strSelectedName As String
        Dim sngSelectedValue As Single
        strSelectedName = CStr(CType(gvData.Rows(e.NewEditIndex).Cells(2).FindControl("hidName"), HiddenField).Value)
        sngSelectedValue = CDbl(CType(gvData.Rows(e.NewEditIndex).Cells(2).FindControl("hidValue"), HiddenField).Value)
        EditParameter(strSelectedName, sngSelectedValue)
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Not IsValidate() Then
            Exit Sub
        End If

        Dim intResult As Integer = 0
        Dim objBO As New ADMS_BAL_ParaValues

        Try
            intResult = objBO.UpdateParameter(lblNameValue.Text.Trim(), _
                                CSng(txtValue.Text.ToString.Trim), Session("UserId"))
            If intResult = 1 Then

                gvData.EditIndex = -1
                BindGridViewData()
                Clearcontrols()
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Parameter Value have been updated successfully.');", True)
            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Parameter Value details already exist.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Updating Parameter Value.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
             "alert('Error occured while Updating Parameter Value.');", True)
        End Try
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        Clearcontrols()
    End Sub
#End Region

#Region "--Methos Or Functions--"
    ''' <summary>
    ''' Bind GridView with datasource
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindGridViewData()
        Dim dtBU As New DataTable
        Dim objBO As New ADMS_BAL_ParaValues

        Try
            dtBU = objBO.GetParameters(Session("UserId"))
            gvData.DataSource = dtBU
            gvData.DataBind()
            gvData.EditIndex = -1

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' To Update Parameter Value, page controls should be filled with selected Parameter Value
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditParameter(ByVal strSelectedName As String, ByVal sngSelectedValue As Single)
        Try
            lblNameValue.Text = strSelectedName.Trim()
            txtValue.Text = sngSelectedValue.ToString()

            btnUpdate.Visible = True
            btnClear.Visible = True

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Validate Page controls for different action
    ''' </summary>
    ''' <remarks></remarks>
    Private Function IsValidate() As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder

        Try

            If String.IsNullOrEmpty(txtValue.Text) Then
                strErrorMessage.Append("\n - Required Parameter Value.")
                bReturn = False
            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Parameter details.');", True)
        End Try

        Return bReturn
    End Function

    ''' <summary>
    ''' Clear vlaue of Page controls
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Clearcontrols()
        txtValue.Text = ""
        lblNameValue.Text = ""
    End Sub
#End Region

    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Response.Redirect("ADMS_Home.aspx")
    End Sub
End Class