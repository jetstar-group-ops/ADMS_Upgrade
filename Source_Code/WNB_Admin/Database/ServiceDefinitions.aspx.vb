Public Partial Class ServiceDefinitions
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Service Definition Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If


                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If


                GetFlightDesig()
                GetVersionNo()

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If
    End Sub
   
    Private Sub GetFlightDesig()
        Dim dtFlightDesig As DataTable


        Try
            dtFlightDesig = go_Bo.Get_ChoiceListByChoicelistID("", "FLIGHT_DESIG")

            ddlflightdesig.DataSource = dtFlightDesig
            ddlflightdesig.DataBind()

            ddlFlightdesig.Items.Insert(0, New ListItem("", ""))

            
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetServiceType()
        Dim dtServiceType As DataTable
        Try
            dtServiceType = go_Bo.Get_ChoiceListByChoicelistID("", "SERVICE_TYPE")

            ddlserviceType.DataSource = dtServiceType
            ddlserviceType.DataBind()

            ddlserviceType.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetServiceDefinition()
        Dim dtServiceDefinition As DataTable

        Try
            If (ddlflightdesig.SelectedIndex <> -1) Then

                If (ddlserviceType.SelectedIndex <> -1) Then

                    dtServiceDefinition = go_Bo.Get_ServiceDefinition("", ddlserviceType.SelectedValue.ToString.Trim, ddlflightdesig.SelectedValue.ToString.Trim)
                    gvServicedefinition.DataSource = dtServiceDefinition
                    gvServicedefinition.DataBind()



                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub gvServiceDefinition_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvServicedefinition.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Session("DatabaseUpgrading") = "0" Then
                Dim imgView As ImageButton = CType(e.Row.Cells(7).FindControl("imgBtnEdit"), ImageButton)
                imgView.ImageUrl = "~/Images/erase.gif"
                imgView.ToolTip = "View"

            End If
        End If
    End Sub
    Protected Sub gvServiceDefinition_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvServicedefinition.RowEditing
        Dim service_definition_cl_Id As String = ""
        Dim flight_no_desig_ref As String = ""
        Dim ServiceDefinitionID As Integer = 0

        Try
            ServiceDefinitionID = Convert.ToInt32(CType(gvServicedefinition.Rows(e.NewEditIndex).Cells(2).FindControl("HidServiceDefinitionID"), HiddenField).Value)
            service_definition_cl_Id = ddlserviceType.SelectedValue.ToString.Trim
            flight_no_desig_ref = ddlflightdesig.SelectedValue.ToString.Trim
            Response.Redirect("ServiceDefinitionEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&SID=" & ServiceDefinitionID & "&SDID=" & service_definition_cl_Id & "&FDID=" & flight_no_desig_ref)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing Service Definition : " & ex.ToString & ". ');", True)

        End Try
    End Sub
    Private Sub GetVersionNo()

        Dim dtFlightDesig As DataTable

        Try
            dtFlightDesig = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtFlightDesig.Rows.Count > 0 Then
                hidVersionNo.Value = dtFlightDesig.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub ddlflightdesig_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlflightdesig.SelectedIndexChanged
        GetServiceType()
    End Sub
    Protected Sub ddlserviceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlserviceType.SelectedIndexChanged
        GetServiceDefinition()

    End Sub

   
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Response.Redirect("ServiceDefinitionEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&SID=0" & "&SDID=" & ddlserviceType.SelectedValue.ToString.Trim & "&FDID=" & ddlflightdesig.SelectedValue.ToString.Trim)
    End Sub
End Class