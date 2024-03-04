Public Partial Class Subfleet
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Create and Update Aircraft Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If

                Dim strFunid As String = CType(Me.Master.FindControl("hidFunctionId"), HiddenField).Value

                If Session("DatabaseUpgrading") = "0" Then
                    btnAdd.Visible = False
                End If


                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = CInt(Request.QueryString("FID").ToString())
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = CInt(Request.QueryString("TID").ToString())
                End If

                'BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")
                BindGridData(hidTableId.Value)
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If

        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub
    Private Sub BindGridData(ByVal strTableId As String)
        Dim dtsubfleet As DataTable
        Dim strFilter As String = ""
        Try
            dtsubfleet = go_Bo.GetDBTableData(strTableId)
            strFilter = " 1=1 "
           
            dtsubfleet.DefaultView.RowFilter = strFilter
            dtsubfleet = dtsubfleet.DefaultView.ToTable()

            gvSubfleet.DataSource = dtsubfleet
            gvSubfleet.DataBind()



        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub gvSubfleet_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSubfleet.PageIndexChanging
        gvSubfleet.PageIndex = e.NewPageIndex
        'Bind data to the GridView control.
        BindGridData(hidTableId.Value.ToString.Trim)
    End Sub
    Protected Sub gvSubfleet_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubfleet.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

                Dim imgDelete As ImageButton = CType(e.Row.Cells(8).FindControl("imgBtnDelete"), ImageButton)
                imgDelete.Visible = False
            End If
        End If
    End Sub
    Protected Sub gvSubfleet_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvSubfleet.RowDeleting
        Dim strAircraftId As String = ""
        Dim strSubfleetId As String = ""
        Dim intVersionNo As Integer

        Try

            strAircraftId = gvSubfleet.Rows(e.RowIndex).Cells(2).Text
            intVersionNo = CInt(CType(gvSubfleet.Rows(e.RowIndex).Cells(7).FindControl("hidVersionNo"), HiddenField).Value)
            strSubfleetId = CInt(CType(gvSubfleet.Rows(e.RowIndex).Cells(7).FindControl("HidSubfleetId"), HiddenField).Value)

            DeleteSubfleetDetails(strSubfleetId, intVersionNo)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the Subfleet : " & ex.ToString & ". ');", True)
        End Try
    End Sub
    Protected Sub gvSubfleet_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvSubfleet.RowEditing
        Dim strAircraftId As String = ""
        Dim strSubfleetId As String = ""


        Try

            strAircraftId = gvSubfleet.Rows(e.NewEditIndex).Cells(1).Text
            strSubfleetId = (CType(gvSubfleet.Rows(e.NewEditIndex).Cells(7).FindControl("HidSubfleetID"), HiddenField).Value)
            Response.Redirect("SubfleetDetails.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&AID=" & strAircraftId & "&SFID=" & strSubfleetId)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the Subfleet : " & ex.ToString & ". ');", True)

        End Try
    End Sub
    Private Sub DeleteSubfleetDetails(ByVal strSubfleetId As String, ByVal intVersionNo As Integer)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.DeleteSubfleet(strSubfleetId, intVersionNo)

            If intResult = 1 Then
                gvSubfleet.EditIndex = -1
                BindGridData(hidTableId.Value)

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Subfleet details have been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while deleting Subfleet details.');", True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("SubfleetDetails.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub

End Class