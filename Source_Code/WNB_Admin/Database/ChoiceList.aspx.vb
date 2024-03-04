Public Partial Class ChoiceList
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.AircraftConfig, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Update Choice List Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If

                Dim strFunid As String = CType(Me.Master.FindControl("hidFunctionId"), HiddenField).Value

                If Session("DatabaseUpgrading") = "0" Then
                    btnAdd.Visible = False
                End If

                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If


                BindGridData(hidTableId.Value)

                'btnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If
       
    End Sub
 
    Private Sub BindGridData(ByVal strTableId As String)
        Dim dtChoice_list As DataTable
        Dim strFilter As String = ""
        Try
            dtChoice_list = go_Bo.GetDBTableData(strTableId)
            strFilter = " 1=1 "

            dtChoice_list.DefaultView.RowFilter = strFilter
            dtChoice_list = dtChoice_list.DefaultView.ToTable()

            gvChoiceList.DataSource = dtChoice_list
            gvChoiceList.DataBind()



        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub gvChoiceList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvChoiceList.PageIndexChanging
        gvChoiceList.PageIndex = e.NewPageIndex
        'Bind data to the GridView control.
        BindGridData(hidTableId.Value.ToString.Trim)
    End Sub
    Protected Sub gvChoiceList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChoiceList.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(4).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

                Dim imgDelete As ImageButton = CType(e.Row.Cells(5).FindControl("imgBtnDelete"), ImageButton)
                imgDelete.Visible = False

                
            End If
        End If
    End Sub
    Protected Sub gvChoiceList_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvChoiceList.RowDeleting
        Dim strChoice_list_id As String = ""

        Dim intVersionNo As Integer

        Try

            strChoice_list_id = gvChoiceList.Rows(e.RowIndex).Cells(1).Text
            intVersionNo = CInt(CType(gvChoiceList.Rows(e.RowIndex).Cells(4).FindControl("hidVersionNo"), HiddenField).Value)


            DeleteChoice_listDetails(strChoice_list_id, intVersionNo)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the Choice List : " & ex.ToString & ". ');", True)
        End Try
    End Sub
    Private Sub DeleteChoice_listDetails(ByVal strChoice_list_id As String, ByVal intVersionNo As Integer)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.DeleteChoice_List(strChoice_list_id, intVersionNo)

            If intResult = 1 Then
                gvChoiceList.EditIndex = -1
                BindGridData(hidTableId.Value)

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
    Protected Sub gvChoiceList_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvChoiceList.RowEditing
        Dim strChoice_list_id As String = ""

        Try

            strChoice_list_id = gvChoiceList.Rows(e.NewEditIndex).Cells(0).Text

            Response.Redirect("ChoiceListAdd.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&CHID=" & strChoice_list_id)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the Choice List : " & ex.ToString & ". ');", True)

        End Try
    End Sub
   
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("ChoiceListAdd.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub

    Protected Sub gvChoiceList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvChoiceList.RowDataBound
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