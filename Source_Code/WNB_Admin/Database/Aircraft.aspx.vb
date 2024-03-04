Public Partial Class Aircraft
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
                    btnAdd.Visible=False
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

        Dim dtAirCraft As DataTable
        Dim strFilter As String = ""


        Try
            dtAirCraft = go_Bo.GetDBTableData(strTableId)

            strFilter = " 1=1 "
            If Not txtAirCraftId.Text.ToString.Trim.Equals(String.Empty) Then
                strFilter = strFilter & " AND aircraft_Id like '%" & txtAirCraftId.Text.ToString.Trim & "%' "
            End If

            If Not txtModelName.Text.ToString.Trim.Equals(String.Empty) Then
                strFilter = strFilter & " AND model_name like '%" & txtModelName.Text.ToString.Trim & "%' "
            End If

            dtAirCraft.DefaultView.RowFilter = strFilter
            dtAirCraft = dtAirCraft.DefaultView.ToTable()

            gvAircraft.DataSource = dtAirCraft
            gvAircraft.DataBind()



        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub gvAircraft_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAircraft.PageIndexChanging
        gvAircraft.PageIndex = e.NewPageIndex
        'Bind data to the GridView control.
        BindGridData(hidTableId.Value.ToString.Trim)
    End Sub

    Private Sub gvAircraft_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAircraft.RowCreated
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



    Private Sub gvAircraft_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvAircraft.RowDeleting
        Dim strAircraftId As String = ""
        Dim intVersionNo As Integer
        Try

            strAircraftId = gvAircraft.Rows(e.RowIndex).Cells(0).Text
            intVersionNo = CInt(CType(gvAircraft.Rows(e.RowIndex).Cells(7).FindControl("hidVersionNo"), HiddenField).Value)
            DeleteAircraftDetails(strAircraftId, intVersionNo)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the Aircraft : " & ex.ToString & ". ');", True)
        End Try
    End Sub

    Private Sub gvAircraft_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvAircraft.RowEditing
        Dim strAircraftId As String = ""

        Try

            strAircraftId = gvAircraft.Rows(e.NewEditIndex).Cells(0).Text

            Response.Redirect("AircraftDetail.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&AID=" & strAircraftId)
           
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the Aircraft : " & ex.ToString & ". ');", True)

        End Try
    End Sub

    Private Sub DeleteAircraftDetails(ByVal strAircraftId As String, ByVal intVersionNo As Integer)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.DeleteAircraft(strAircraftId, intVersionNo)

            If intResult = 1 Then
                gvAircraft.EditIndex = -1
                BindGridData(hidTableId.Value)

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Aircraft details have been deleted successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while deleting Aircraft details.');", True)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        BindGridData(hidTableId.Value.ToString.Trim)
    End Sub

    
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("AircraftDetail.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value )
    End Sub
End Class