Public Partial Class ServiceData
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.Aircraft, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Add and Update Service Data Details."
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
            dtServiceType = go_Bo.Get_ChoiceListByChoicelistID("", "SERVICE_TYPE")

            ddlserviceType.DataSource = dtServiceType
            ddlserviceType.DataBind()

            ddlserviceType.Items.Insert(0, New ListItem("", ""))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetServiceData()
        Dim dtServiceData As DataTable

        Try
            If (ddlAirCraftId.SelectedIndex <> -1) Then

                If (ddlServiceType.SelectedIndex <> -1) Then

                    dtServiceData = go_Bo.Get_ServiceData("", ddlServiceType.SelectedValue.ToString.Trim, ddlAirCraftId.SelectedValue.ToString.Trim)
                    gvServiceData.DataSource = dtServiceData
                    gvServiceData.DataBind()



                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ddlAirCraftId_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAirCraftId.SelectedIndexChanged
        GetServiceType()
    End Sub

    Protected Sub ddlServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlServiceType.SelectedIndexChanged
        GetServiceData()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim intResult As Integer = 0

        Dim intVersionNo As Integer = 0
        Dim weight As System.Int32 = 0
        Dim aircraftID As String = ""
        Dim occupies_seat As String = ""
        Dim type_choicelistid As String = ""
        Dim serviceDataId As Int32 = 0

        Try

            Dim category_choicelistid As String

            For intRCnt As Integer = 0 To gvServiceData.Rows.Count - 1

                category_choicelistid = gvServiceData.Rows(intRCnt).Cells(0).Text

                'strAircraftId = gvULDDefinition.Rows(intRCnt).Cells(0).Text

                If (CType(gvServiceData.Rows(intRCnt).Cells(1).FindControl("Txtweight"), TextBox).Text <> "") Then
                    weight = Convert.ToInt32(CType(gvServiceData.Rows(intRCnt).Cells(1).FindControl("Txtweight"), TextBox).Text)
                End If
                If (CType(gvServiceData.Rows(intRCnt).Cells(2).FindControl("ddloccupiesSeat"), DropDownList).Text <> "") Then
                    occupies_seat = (CType(gvServiceData.Rows(intRCnt).Cells(2).FindControl("ddloccupiesSeat"), DropDownList).Text)
                End If

                type_choicelistid = ddlServiceType.Text

                aircraftID = ddlAirCraftId.Text

                If (CType(gvServiceData.Rows(intRCnt).Cells(3).FindControl("hidServiceDataID"), HiddenField).Value <> "") Then
                    serviceDataId = (CType(gvServiceData.Rows(intRCnt).Cells(3).FindControl("hidServiceDataID"), HiddenField).Value)
                End If

                If (CType(gvServiceData.Rows(intRCnt).Cells(4).FindControl("hidVersionNo"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvServiceData.Rows(intRCnt).Cells(4).FindControl("hidVersionNo"), HiddenField).Value)
                End If

                
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If
                'Add/Update the ULD Definition
                intResult = go_Bo.Create_Update_ServiceData(serviceDataId, type_choicelistid, weight, occupies_seat, category_choicelistid, aircraftID, intVersionNo)

            Next


            If intResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert(' Successfully updated the Service Data.');", True)

            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Service Data does not exists.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert('Error occured while updating the Service Data.');", True)
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Service Date : " & ex.ToString & ". ');", True)

        End Try
    End Sub

End Class