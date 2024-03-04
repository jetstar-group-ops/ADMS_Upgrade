Public Partial Class MoveDataToProd
    Inherits System.Web.UI.Page

    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.WNB_FULL_ACCESS, "", 0) = False Then

                lsMessage = "You don't have permission on Weight and Balance System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            btnMove.Attributes.Add("onClick", "return ValidateControls();")

        End If
    End Sub

    Private Sub btnMove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMove.Click
        Try
            Dim intResult As Integer = 0


            intResult = go_Bo.Push_Pre_Production_Data_To_Production(Session("UserId").ToString.Trim)
            If intResult = 1 Then

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Pre Production Data have been moved successfully to Production.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while moving Pre Production Data to Production.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Error occured while moving Pre Production Data to Production.');", True)

        End Try
    End Sub
End Class