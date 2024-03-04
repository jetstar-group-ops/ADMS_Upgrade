Public Partial Class UnderFloorConfiguration
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Sub fleet Details."
                    Response.Redirect("../Home.aspx?Message=" & ls_Message)
                    Exit Sub
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If
                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If



                GetAircrafts()


                If Session("DatabaseUpgrading") = "0" Then
                    DisableControls()
                End If

                'btnAdd.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If

    End Sub

    Protected Sub ddlAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraft.SelectedIndexChanged
        If ddlAircraft.SelectedIndex <> -1 Then
            GetAircraftsConfig()
        End If
    End Sub

    Protected Sub ddlAircarftConfig_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircarftConfig.SelectedIndexChanged
        If ddlAircarftConfig.SelectedIndex <> -1 Then
            GetULDDefault()
        End If
    End Sub

    Protected Sub ddlULDConfiguration_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlULDConfiguration.SelectedIndexChanged
        If ddlAircarftConfig.SelectedIndex <> -1 Then
            GetULDConfigurationDetails()
        End If
    End Sub

    Private Sub GetULDConfigurationDetails()

        Dim dtULDDetails As DataTable
        Try
            dtULDDetails = go_Bo.Get_ULDConfiguration(hidTableId.Value, ddlAircraft.SelectedValue.Trim, 0, ddlAircarftConfig.SelectedValue.Trim, ddlULDConfiguration.SelectedValue.Trim)
            gvPosition.DataSource = dtULDDetails
            gvPosition.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetAircrafts()

        Dim dtAircrafts As DataTable

        Try
            dtAircrafts = go_Bo.Get_Aircrafts("")

            ddlAircraft.DataSource = dtAircrafts
            ddlAircraft.DataBind()

            ddlAircraft.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetAircraftsConfig()

        Dim dtAircraftsConfig As DataTable

        Try
            dtAircraftsConfig = go_Bo.GetAircraftConfigDetails(ddlAircraft.SelectedValue.Trim, "")

            ddlAircarftConfig.DataSource = dtAircraftsConfig
            ddlAircarftConfig.DataBind()

            ddlAircarftConfig.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetULDDefault()

        Dim dtULDConfig As DataTable

        Try
            dtULDConfig = go_Bo.GetULDConfigDefaultDetails(ddlAircraft.SelectedValue.Trim, ddlAircarftConfig.SelectedValue.Trim)

            ddlULDConfiguration.DataSource = dtULDConfig
            ddlULDConfiguration.DataBind()
            ddlULDConfiguration.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DisableControls()

    End Sub


    Protected Sub gvPosition_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvPosition.RowEditing
        Dim strAircraftId As String = ""
        Dim ConfigID As Integer
        Dim hidVersion As Integer

        Try

            strAircraftId = ddlAircraft.SelectedValue.Trim
            ConfigID = Convert.ToInt32(CType(gvPosition.Rows(e.NewEditIndex).Cells(2).FindControl("hidConfigID"), HiddenField).Value)
            hidVersion = Convert.ToInt32(CType(gvPosition.Rows(e.NewEditIndex).Cells(2).FindControl("hidVersion"), HiddenField).Value)
            Response.Redirect("UlDConfigurationEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&AID=" _
                              & strAircraftId & "&ID=" & ConfigID & "&CID=" & ddlAircarftConfig.SelectedValue.Trim & _
                              "&UID=" & ddlULDConfiguration.SelectedValue & "&VID=" & hidVersion)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while editing the Aircraft : " & ex.ToString & ". ');", True)

        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click

        Try
            If (ddlAircraft.SelectedIndex <> -1 And ddlAircarftConfig.SelectedIndex <> -1 And ddlULDConfiguration.SelectedIndex <> -1) Then
                Response.Redirect("ULDConfigurationEdit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value & "&AID=" & ddlAircraft.SelectedValue.Trim & "&ID=" & "&CID=" & ddlAircarftConfig.SelectedValue.Trim & "&UID=" & ddlULDConfiguration.SelectedValue & "&VID=0")
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Please Select All Manditory Fileds.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while adding Record : " & ex.ToString & ". ');", True)

        End Try

    End Sub
End Class