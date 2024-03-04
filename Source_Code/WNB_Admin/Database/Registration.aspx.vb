Public Partial Class Registration
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

        Dim dtAirCraft As DataTable
        Dim strFilter As String = ""


        Try
            dtAirCraft = go_Bo.GetDBTableData(strTableId)

            strFilter = " 1=1 "
            If Not txtAirCraftId.Text.ToString.Trim.Equals(String.Empty) Then
                strFilter = strFilter & " AND aircraft_Id like '%" & txtAirCraftId.Text.ToString.Trim & "%' "
            End If

            If Not txtRegistrationNumber.Text.ToString.Trim.Equals(String.Empty) Then
                strFilter = strFilter & " AND registration_number like '%" & txtRegistrationNumber.Text.ToString.Trim & "%' "
            End If

            dtAirCraft.DefaultView.RowFilter = strFilter
            dtAirCraft = dtAirCraft.DefaultView.ToTable()

            gvRegistration.DataSource = dtAirCraft
            gvRegistration.DataBind()



        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Protected Sub gvRegistration_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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

    Protected Sub gvRegistration_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
        Dim strAircraftId As String = ""
        Dim intVersionNo As Integer
        Dim RegistrationID As Integer
        Try

            strAircraftId = gvRegistration.Rows(e.RowIndex).Cells(2).Text
            intVersionNo = CInt(CType(gvRegistration.Rows(e.RowIndex).Cells(7).FindControl("hidVersionNo"), HiddenField).Value)
            RegistrationID = CInt(CType(gvRegistration.Rows(e.RowIndex).Cells(7).FindControl("HidRegistrationID"), HiddenField).Value)

            DeleteRegistrationDetails(RegistrationID, intVersionNo)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while deleting the Aircraft : " & ex.ToString & ". ');", True)
        End Try
    End Sub

    Protected Sub gvRegistration_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        Dim strAircraftId As String = ""
        Dim strRegistrationID As Integer

        Try

            strAircraftId = gvRegistration.Rows(e.NewEditIndex).Cells(1).Text
            strRegistrationID = CInt(CType(gvRegistration.Rows(e.NewEditIndex).Cells(7).FindControl("HidRegistrationID"), HiddenField).Value)
            Response.Redirect("RegistrationDetail.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&AID=" & strAircraftId & "&RID=" & strRegistrationID)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the Registration : " & ex.ToString & ". ');", True)

        End Try
    End Sub

    Protected Sub gvRegistration_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvRegistration.PageIndex = e.NewPageIndex
        'Bind data to the GridView control.
        BindGridData(hidTableId.Value.ToString.Trim)
    End Sub

    Private Sub DeleteRegistrationDetails(ByVal strRegistrationID As Int32, ByVal intVersionNo As Integer)
        Dim intResult As Integer = 0

        Try
            intResult = go_Bo.DeleteRegistration(strRegistrationID, intVersionNo)

            If intResult = 1 Then
                gvRegistration.EditIndex = -1
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGridData(hidTableId.Value)
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("RegistrationDetail.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub
End Class