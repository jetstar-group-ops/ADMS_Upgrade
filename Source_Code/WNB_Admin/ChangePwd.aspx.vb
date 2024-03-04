Public Partial Class ChangePwd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            BtnChangePwd.Attributes.Add("onClick", "return ValidateControls('CHANGE');")
        End If
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Response.Redirect("Home.aspx")
    End Sub

    Private Sub BtnChangePwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnChangePwd.Click

        If Not IsValidate("CHANGE") Then
            Exit Sub
        End If

        Dim lsUserName As String = Session("UserID") & ""
        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()
        Dim dtUserDetails As DataTable = Nothing
        Dim intResult As Integer = 0

        Try
            dtUserDetails = objBo.GetUserDetails(lsUserName, TxtCurrentPwd.Text)

            If dtUserDetails.Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Invalid Password.');", True)
            Else
                intResult = objBo.ChangePwd(lsUserName, TxtNewPwd.Text)
                If intResult = 1 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Successfully changed password.'); window.location='Home.aspx';", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error Occured while changing password.'); window.location='Home.aspx';", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error Occured while changing password.'); window.location='Home.aspx';", True)
        Finally
            objBo = Nothing
        End Try
        
    End Sub

    Private Function IsValidate(ByVal strCmd As String) As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder
        Dim lsSelectedfunctionalities As String = ""

        Try
            If Not strCmd Is Nothing AndAlso (String.Equals(strCmd, "CHANGE")) Then

                If Not TxtCurrentPwd Is Nothing AndAlso String.Equals(TxtCurrentPwd.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Current Password.")
                    bReturn = False
                End If

                If Not TxtNewPwd Is Nothing AndAlso String.Equals(TxtNewPwd.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required New Password.")
                    bReturn = False
                End If

                If Not TxtConfirmPwd Is Nothing AndAlso String.Equals(TxtConfirmPwd.Text.Trim, String.Empty) Then
                    strErrorMessage.Append("\n - Required Confirm New Password.")
                    bReturn = False
                End If

                If (Not String.Equals(TxtNewPwd.Text.Trim, String.Empty)) AndAlso (Not String.Equals(TxtConfirmPwd.Text.Trim, String.Empty)) Then
                    If Not String.Equals(TxtNewPwd.Text.Trim, TxtConfirmPwd.Text.Trim) Then
                        strErrorMessage.Append("\n - New Password and Confirm New Password does not match.")
                        bReturn = False
                    End If
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