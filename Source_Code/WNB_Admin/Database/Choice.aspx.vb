Partial Public Class ChoiceID
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.AircraftConfig, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Update Choice Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If

                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If

                GetAircrafts()
                'BindGridData(hidTableId.Value)

                'btnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

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

            ddlAircraftID.DataSource = dtAircrafts
            ddlAircraftID.DataBind()

            ddlAircraftID.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Protected Sub gvChoice_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvChoice.PageIndexChanging
        gvChoice.PageIndex = e.NewPageIndex
        'Bind data to the GridView control.
        BindGridData()
    End Sub
    Protected Sub gvChoice_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChoice.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(6).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

                Dim imgDelete As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnDelete"), ImageButton)
                imgDelete.Visible = False
            End If
        End If
    End Sub
    Protected Sub gvChoice_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvChoice.RowDeleting
        Dim strChoices_id As String = ""

        Dim intVersionNo As Integer

        Try

            strChoices_id = gvChoice.Rows(e.RowIndex).Cells(0).Text
            intVersionNo = CInt(CType(gvChoice.Rows(e.RowIndex).Cells(4).FindControl("hidVersionNo"), HiddenField).Value)


            DeleteChoice_Details(strChoices_id, intVersionNo)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the Choice List : " & ex.ToString & ". ');", True)
        End Try
    End Sub
    Private Sub DeleteChoice_Details(ByVal strChoices_id As String, ByVal intVersionNo As Integer)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.DeleteChoices(strChoices_id, intVersionNo)

            If intResult = 1 Then
                gvChoice.EditIndex = -1
                BindGridData()

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Choice List details have been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while deleting Choice List.');", True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub gvChoice_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvChoice.RowEditing
        Dim strChoices_id As String = ""

        Try

            strChoices_id = gvChoice.Rows(e.NewEditIndex).Cells(0).Text

            Response.Redirect("ChoiceAdd.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&AID=" & ddlAircraftID.SelectedValue.ToString.Trim & "&CID=" & strChoices_id)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the Choice : " & ex.ToString & ". ');", True)

        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("ChoiceAdd.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub

    Private Sub BindGridData()

        Dim dtChoice As DataTable
        Try
            If (ddlAircraftID.SelectedIndex <> -1) Then


                dtChoice = go_Bo.Get_Choices(ddlAircraftID.SelectedValue.ToString, System.DBNull.Value.ToString)
                gvChoice.DataSource = dtChoice
                gvChoice.DataBind()

            End If



        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlAircraftID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraftID.SelectedIndexChanged


        BindGridData()

    End Sub

    Protected Sub gvChoice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChoice.RowDataBound

        Try


            Dim lblStatus As Label
            lblStatus = CType(e.Row.Cells(2).FindControl("lblStatus"), Label)
            If lblStatus.Text = "1" Then

                lblStatus.Text = "Active"

            ElseIf lblStatus.Text = "0" Then
                lblStatus.Text = "Inactive"

            End If

        Catch ex As Exception

        End Try
    End Sub
End Class