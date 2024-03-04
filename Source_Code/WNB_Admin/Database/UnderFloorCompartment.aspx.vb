Partial Public Class UnderFloorCompartment
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
    Private Sub DisableControls()

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim intResult As Integer = 0
        Dim strAircraftId As String = ""
        Dim strSubfleetId As String = ""
        Dim intVersionNo As Integer = 0
        Dim isEnabled As String = ""
        Dim UldCompLID As System.Int64 = 0
        Dim PosRef1 As Integer = 0
        Dim PosRef2 As Integer = 0
        Dim PosRef3 As Integer = 0
        Dim MaxLoad As Integer = 0

        Try



            Dim Designation As String

            For intRCnt As Integer = 0 To gvUnderfloor.Rows.Count - 1

                Designation = gvUnderfloor.Rows(intRCnt).Cells(0).Text

                If (CType(gvUnderfloor.Rows(intRCnt).Cells(1).FindControl("txtMaxCptLoad"), TextBox).Text <> "") Then
                    MaxLoad = Convert.ToInt32(CType(gvUnderfloor.Rows(intRCnt).Cells(1).FindControl("txtMaxCptLoad"), TextBox).Text)
                End If


                If (CType(gvUnderfloor.Rows(intRCnt).Cells(2).FindControl("ddlRef1"), DropDownList).Text <> "") Then
                    PosRef1 = Convert.ToInt32(CType(gvUnderfloor.Rows(intRCnt).Cells(2).FindControl("ddlRef1"), DropDownList).Text)
                    If (PosRef1 = 0) Then
                        PosRef1 = Nothing

                    End If
                End If

                If (CType(gvUnderfloor.Rows(intRCnt).Cells(3).FindControl("ddlRef3"), DropDownList).Text <> "") Then
                    PosRef2 = Convert.ToInt32(CType(gvUnderfloor.Rows(intRCnt).Cells(3).FindControl("ddlRef3"), DropDownList).Text)
                    If (PosRef2 = 0) Then
                        PosRef2 = Nothing

                    End If
                End If

                If (CType(gvUnderfloor.Rows(intRCnt).Cells(4).FindControl("ddlRef3"), DropDownList).Text <> "") Then
                    PosRef3 = Convert.ToInt32(CType(gvUnderfloor.Rows(intRCnt).Cells(4).FindControl("ddlRef3"), DropDownList).Text)
                    If (PosRef3 = 0) Then
                        PosRef3 = Nothing

                    End If
                End If

                If (CType(gvUnderfloor.Rows(intRCnt).Cells(5).FindControl("hidVersion"), HiddenField).Value <> "") Then
                    intVersionNo = CInt(CType(gvUnderfloor.Rows(intRCnt).Cells(5).FindControl("hidVersion"), HiddenField).Value)
                End If
                
                If (CType(gvUnderfloor.Rows(intRCnt).Cells(5).FindControl("hidUndCompID"), HiddenField).Value <> "") Then
                    UldCompLID = Convert.ToInt64(CType(gvUnderfloor.Rows(intRCnt).Cells(5).FindControl("hidUndCompID"), HiddenField).Value)
                End If
                If (intVersionNo = 0) Then
                    intVersionNo = -1
                End If

                intResult = go_Bo.CreateUpdateUnderFloorComp(UldCompLID, Designation, MaxLoad, PosRef1, PosRef2, PosRef3, ddlAircraft.SelectedValue.Trim, intVersionNo, "Admin")

            Next


            If intResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert(' Successfully updated the Underfloor Compartment.');", True)

            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Underfloor Compartment does not exists.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert('Error occured while updating the Underfloor Compartment.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating Underfloor Compartment : " & ex.ToString & ". ');", True)

        End Try
    End Sub

    Protected Sub ddlAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAircraft.SelectedIndexChanged
        Try
            GetUnderFloorDetails()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetUnderFloorDetails()
        Try
            Dim dtUnderFloorDetails As DataTable

            If (ddlAircraft.SelectedIndex <> -1) Then
                dtUnderFloorDetails = go_Bo.Get_UnderFloor_Comp(hidTableId.Value.Trim, ddlAircraft.SelectedValue.Trim)
                gvUnderfloor.DataSource = dtUnderFloorDetails.DefaultView
                gvUnderfloor.DataBind()

            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvUnderfloor_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUnderfloor.RowCreated
        Try
            Dim dtPosition As DataTable
            Dim ddlPos As DropDownList

            dtPosition = go_Bo.Get_ULDPosition("4", ddlAircraft.SelectedValue.Trim, 0)

            If (Not dtPosition Is Nothing) Then


                Dim Dtrow As DataRow
                Dtrow = dtPosition.NewRow()
                Dtrow("Pos_Name") = 0
                dtPosition.Rows.Add(Dtrow)


            End If

            ddlPos = CType(e.Row.FindControl("ddlRef1"), DropDownList)
            ddlPos.DataSource = dtPosition
            ddlPos.DataBind()

            ddlPos = CType(e.Row.FindControl("ddlRef2"), DropDownList)
            ddlPos.DataSource = dtPosition
            ddlPos.DataBind()

            ddlPos = CType(e.Row.FindControl("ddlRef3"), DropDownList)
            ddlPos.DataSource = dtPosition
            ddlPos.DataBind()

        Catch ex As Exception

        End Try
    End Sub
End Class