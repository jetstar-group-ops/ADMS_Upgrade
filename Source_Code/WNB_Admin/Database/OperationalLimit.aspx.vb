Public Partial Class OperationalLimits
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Operational Limits Details."
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
                GetVersionNo()

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

            ddlAirCraftId.DataSource = dtAircrafts
            ddlAirCraftId.DataBind()

            ddlAirCraftId.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetSubFleet()

        Dim dtSublfleet As DataTable

        Try
            dtSublfleet = go_Bo.Get_Subfleet(ddlAirCraftId.SelectedValue.ToString, "")

            ddlSublfeet.DataSource = dtSublfleet
            ddlSublfeet.DataBind()
            ddlSublfeet.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub ddlAirCraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAirCraftId.SelectedIndexChanged
        If (ddlAirCraftId.SelectedValue.ToString <> String.Empty) Then
            GetSubFleet()
        End If
    End Sub
    Protected Sub ddlSublfeet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSublfeet.SelectedIndexChanged



        Dim dtaircraftConfiguration As DataTable
       
        Try
            If (ddlSublfeet.SelectedIndex <> -1) Then


                dtaircraftConfiguration = go_Bo.Get_Aircraft_Config_Adjustments(hidTableId.Value.Trim, ddlAirCraftId.SelectedValue.ToString, ddlSublfeet.SelectedValue.ToString)
                ddlaircraftConfiguration.DataSource = dtaircraftConfiguration
                ddlaircraftConfiguration.DataBind()

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Private Sub GetVersionNo()

        Dim dtSubfleet As DataTable

        Try
            dtSubfleet = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtSubfleet.Rows.Count > 0 Then
                hidVersionNo.Value = dtSubfleet.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlaircraftConfiguration_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlaircraftConfiguration.SelectedIndexChanged
        GetOperationalLimit()

    End Sub
    Private Sub GetOperationalLimit()
        Dim dtOperationalLimit1 As DataTable
        Dim dtOperationalLimit2 As DataTable
        Dim dtOperationalLimit3 As DataTable
        Dim dtOperationalLimit4 As DataTable

        Try
            If (ddlSublfeet.SelectedIndex <> -1) Then

                If (ddlaircraftConfiguration.SelectedIndex <> -1) Then

                    dtOperationalLimit1 = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, ddlaircraftConfiguration.SelectedValue.ToString.Trim, "FORWARD", ddlSublfeet.SelectedValue.ToString.Trim)
                    gvForward.DataSource = dtOperationalLimit1
                    gvForward.DataBind()

                    dtOperationalLimit2 = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, ddlaircraftConfiguration.SelectedValue.ToString.Trim, "AFTLIMIT", ddlSublfeet.SelectedValue.ToString.Trim)
                    gvAFTLimit.DataSource = dtOperationalLimit2
                    gvAFTLimit.DataBind()

                    dtOperationalLimit3 = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, ddlaircraftConfiguration.SelectedValue.ToString.Trim, "FORWARDZERO", ddlSublfeet.SelectedValue.ToString.Trim)
                    gvForwardZero.DataSource = dtOperationalLimit3
                    gvForwardZero.DataBind()

                    dtOperationalLimit4 = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, ddlaircraftConfiguration.SelectedValue.ToString.Trim, "AFTZERO", ddlSublfeet.SelectedValue.ToString.Trim)
                    gvAFTZero.DataSource = dtOperationalLimit4
                    gvAFTZero.DataBind()

                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Protected Sub gvForward_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvForward.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

            End If
        End If
    End Sub
    Protected Sub gvForward_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvForward.RowEditing
        Dim strAircraftConfig_cl_Id As String = ""
        Dim strSubfleetId As String = ""
        Dim stroplomitId As String = ""

        Try
            stroplomitId = (CType(gvForward.Rows(e.NewEditIndex).Cells(2).FindControl("HidoplimitID"), HiddenField).Value)
            strAircraftConfig_cl_Id = (CType(gvForward.Rows(e.NewEditIndex).Cells(2).FindControl("HidAirconfigclID"), HiddenField).Value)
            strSubfleetId = (CType(gvForward.Rows(e.NewEditIndex).Cells(2).FindControl("HidSubfleetID"), HiddenField).Value)
            Response.Redirect("OperationalLimitEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&ACID=" & strAircraftConfig_cl_Id & "&SFID=" & strSubfleetId & "&OPID=" & stroplomitId)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing Operational Limit : " & ex.ToString & ". ');", True)

        End Try
    End Sub
    Protected Sub gvAFTLimit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAFTLimit.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

            End If
        End If
    End Sub
    Protected Sub gvAFTLimit_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvAFTLimit.RowEditing
        Dim strAircraftConfig_cl_Id As String = ""
        Dim strSubfleetId As String = ""
        Dim stroplomitId As String = ""

        Try
            stroplomitId = (CType(gvAFTLimit.Rows(e.NewEditIndex).Cells(2).FindControl("HidoplimitID"), HiddenField).Value)
            strAircraftConfig_cl_Id = (CType(gvAFTLimit.Rows(e.NewEditIndex).Cells(2).FindControl("HidAirconfigclID"), HiddenField).Value)
            strSubfleetId = (CType(gvAFTLimit.Rows(e.NewEditIndex).Cells(2).FindControl("HidSubfleetID"), HiddenField).Value)
            Response.Redirect("OperationalLimitEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&ACID=" & strAircraftConfig_cl_Id & "&SFID=" & strSubfleetId & "&OPID=" & stroplomitId)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing Operational Limit : " & ex.ToString & ". ');", True)

        End Try
    End Sub
    Protected Sub gvForwardZero_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvForwardZero.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

            End If
        End If
    End Sub
    Protected Sub gvForwardZero_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvForwardZero.RowEditing
        Dim strAircraftConfig_cl_Id As String = ""
        Dim strSubfleetId As String = ""
        Dim stroplomitId As String = ""

        Try
            stroplomitId = (CType(gvForwardZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidoplimitID"), HiddenField).Value)
            strAircraftConfig_cl_Id = (CType(gvForwardZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidAirconfigclID"), HiddenField).Value)
            strSubfleetId = (CType(gvForwardZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidSubfleetID"), HiddenField).Value)
            Response.Redirect("OperationalLimitEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&ACID=" & strAircraftConfig_cl_Id & "&SFID=" & strSubfleetId & "&OPID=" & stroplomitId)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing Operational Limit : " & ex.ToString & ". ');", True)

        End Try
    End Sub
    Protected Sub gvAFTZero_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAFTZero.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"
            End If
        End If
    End Sub
    Protected Sub gvAFTZero_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvAFTZero.RowEditing
        Dim strAircraftConfig_cl_Id As String = ""
        Dim strSubfleetId As String = ""
        Dim stroplomitId As String = ""
        Dim stroplID As Integer = 0

        Try
            stroplID = (CType(gvAFTZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidoplID"), HiddenField).Value)
            stroplomitId = (CType(gvAFTZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidoplimitID"), HiddenField).Value)
            strAircraftConfig_cl_Id = (CType(gvAFTZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidAirconfigclID"), HiddenField).Value)
            strSubfleetId = (CType(gvAFTZero.Rows(e.NewEditIndex).Cells(2).FindControl("HidSubfleetID"), HiddenField).Value)
            Response.Redirect("OperationalLimitEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&ACID=" & strAircraftConfig_cl_Id & "&SFID=" & strSubfleetId & "&OPID=" & stroplomitId & "&OPLID=" & stroplID)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing Operational Limit : " & ex.ToString & ". ');", True)

        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("OperationalLimitEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&OPLID=0" & "&ACID=" & ddlaircraftConfiguration.SelectedValue.ToString.Trim & "&SFID=" & ddlSublfeet.SelectedValue.ToString.Trim)
    End Sub
End Class