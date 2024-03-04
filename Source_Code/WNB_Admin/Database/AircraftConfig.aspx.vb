Public Partial Class AircraftConfig
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()
    Private WithEvents ItemTemplateColumn As ItemTemplate

    Private Sub AircraftConfig_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        CreateTemplateColumn()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim ls_Message As String = ""
            Try
                If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.AircraftConfig, _
                                   "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

                    ls_Message = "You don't have permission to Update Aircraft Configuration Details."
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
                GetAirConfigDetail()

                BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

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

            ddlAircraft.DataSource = dtAircrafts
            ddlAircraft.DataBind()

            ddlAircraft.Items.Insert(0, New ListItem("", ""))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub GetAirConfigDetail()

        Dim dtAirConfig As DataTable

        Try
            dtAirConfig = go_Bo.Get_Aircraft_Config(hidTableId.Value.ToString.Trim, ddlAircraft.SelectedValue.ToString.Trim)

            gvAirConfig.DataSource = dtAirConfig
            gvAirConfig.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ddlAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAircraft.SelectedIndexChanged
        GetAirConfigDetail()
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        Dim dtAirConfig = New DataTable()
        Dim ddlFuelTank As DropDownList
        Dim drAirConfig As DataRow
        Dim HidVersion As New HiddenField
        Dim strTankRef As String = ""
        Dim strAirconfig As String = ""
        Dim strFuelTank As String = ""
        Dim intACId As Integer = 0
        Dim intVersionNO As Integer = 0

        dtAirConfig.Columns.Add("aircraft_config_id", System.Type.GetType("System.String"))
        dtAirConfig.Columns.Add("tank_ref_cl_id", System.Type.GetType("System.String"))
        dtAirConfig.Columns.Add("air_config_cl_id", System.Type.GetType("System.String"))
        dtAirConfig.Columns.Add("fuel_in_tank_allowed", System.Type.GetType("System.String"))
        dtAirConfig.Columns.Add("VersionNo", System.Type.GetType("System.Int32"))



        For intRCnt As Integer = 0 To gvAirConfig.Rows.Count - 1
            strTankRef = gvAirConfig.Rows(intRCnt).Cells(0).Text.ToString.Trim
            'HidVersion = gvAirConfig.Rows(intRCnt).Cells(1).Text.ToString.Trim
            HidVersion.Value = 0
            HidVersion.Value = CType(gvAirConfig.Rows(intRCnt).Cells(1).Controls(1), HiddenField).Value
            For intCnt As Integer = 2 To gvAirConfig.Rows(intRCnt).Cells.Count - 1

                strAirconfig = CType(gvAirConfig.HeaderRow.Cells(intCnt).Controls(0), Literal).Text.ToString.Trim
                ddlFuelTank = CType(gvAirConfig.Rows(intRCnt).Cells(intCnt).Controls(0), DropDownList)
                strFuelTank = ddlFuelTank.SelectedValue.ToString.Trim

                intACId = intACId + 1

                drAirConfig = dtAirConfig.NewRow

                drAirConfig("aircraft_config_id") = intACId
                drAirConfig("tank_ref_cl_id") = strTankRef
                drAirConfig("air_config_cl_id") = strAirconfig
                drAirConfig("fuel_in_tank_allowed") = strFuelTank
                drAirConfig("VersionNo") = HidVersion.Value

                dtAirConfig.Rows.Add(drAirConfig)

            Next
        Next

        Dim intResult As Integer = 0

        For Each row As DataRow In dtAirConfig.Rows
            Try
                'intResult = go_Bo.UpdateAircraftConfig(ddlAircraft.SelectedItem.Value.ToString(), _
                '   row(0).ToString(), row(1).ToString(), _
                '    row(2).ToString(), row(3).ToString(), _
                '     Convert.ToInt32(row(4).ToString()))


                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                       "alert(' Successfully updated the Aircraft Config .');", True)


                'If intResult = 1 Then
                '    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                '       "alert(' Successfully updated the Aircraft Config .');", True)



                'ElseIf intResult = 2 Then
                '    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                '        "alert('Aircraft details does not exists.');", True)
                'Else
                '    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                '       "alert('Error occured while updating the Aircraft.');", True)
                'End If


            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured while Adding\updating the Aircraft : " & ex.ToString & ". ');", True)

            End Try
        Next



    End Sub

    Sub gvAirConfig_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header Then
            For intCnt As Integer = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(intCnt).HorizontalAlign = HorizontalAlign.Left
            Next
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            For intCnt As Integer = 2 To e.Row.Cells.Count - 1
                e.Row.Cells(intCnt).HorizontalAlign = HorizontalAlign.Left
                If intCnt > 0 And e.Row.Cells(intCnt).Controls.Count <> 0 Then
                    CType(e.Row.Cells(intCnt).Controls(0), DropDownList).Width = Unit.Pixel(70)
                End If
            Next

        End If

    End Sub


    Private Sub CreateTemplateColumn()

        Dim dtAirConfig As DataTable

        Try
            dtAirConfig = go_Bo.Get_Aircraft_Config(hidTableId.Value.ToString.Trim, ddlAircraft.SelectedValue.ToString.Trim)

            For intCnt As Integer = 1 To dtAirConfig.Columns.Count - 2
                Dim TemplatedColumn As New TemplateField()
                ItemTemplateColumn = New ItemTemplate(dtAirConfig.Columns(intCnt).ColumnName.ToString, ListItemType.Header, False)
                TemplatedColumn.HeaderTemplate = ItemTemplateColumn
                ItemTemplateColumn = New ItemTemplate(dtAirConfig.Columns(intCnt).ColumnName.ToString, ListItemType.Item, False)
                TemplatedColumn.ItemTemplate = ItemTemplateColumn
                gvAirConfig.Columns.Add(TemplatedColumn)
            Next

        Catch ex As Exception
            Throw ex
        End Try

        
    End Sub

    'Protected Sub CheckboxColumn_CheckBoxCheckedChanged(ByVal sender As CheckBox, _
    '                                                      ByVal e As System.EventArgs) _
    '                                                      Handles CheckboxColumn.CheckBoxCheckedChanged
    '    Dim CurrentCheckBox = DirectCast(sender, CheckBox)
    '    Response.Write(CurrentCheckBox.Checked)
    'End Sub



End Class