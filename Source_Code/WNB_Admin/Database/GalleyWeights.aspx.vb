Public Partial Class GalleyWeights
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Galley Weights."
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
    Private Sub GetServiceType()
        Dim dtServiceType As DataTable
        Try
            dtServiceType = go_Bo.Get_ChoiceListByChoicelistID("", "GALLEY_DESIG")

            ddlServiceId.DataSource = dtServiceType
            ddlServiceId.DataBind()

            ddlServiceId.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetGalleyWeight()
        Dim dtgalleyWeight As DataTable
       
        Try
            If (ddlAirCraftId.SelectedIndex <> -1) Then

                If (ddlSublfeet.SelectedIndex <> -1) Then

                    'dtgalleyWeight = go_Bo.Get_GalleyWeight(ddlSublfeet.SelectedValue.ToString.Trim, ddlServiceId.SelectedValue.ToString.Trim)
                    'gvGalleyWeight.DataSource = dtgalleyWeight
                    'gvGalleyWeight.DataBind()



                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub ddlAirCraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAirCraftId.SelectedIndexChanged
        GetSubFleet()
        GetServiceType()
    End Sub

    Protected Sub ddlServiceId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlServiceId.SelectedIndexChanged
        GetGalleyWeight()
    End Sub
End Class