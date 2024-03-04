Public Partial Class OperationalLimitEdit
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

                If Not Request.QueryString("FID") Is Nothing Then
                    hidFunctionId.Value = Request.QueryString("FID").ToString()
                End If

                If Not Request.QueryString("TID") Is Nothing Then
                    hidTableId.Value = Request.QueryString("TID").ToString()
                End If

                GetVersionNo()

                If Not Request.QueryString("ACID") Is Nothing Then
                    HidAirconfigcl.Value = Request.QueryString("ACID")

                End If

                If Not Request.QueryString("OPID") Is Nothing Then
                    HidoplimitID.Value = Request.QueryString("OPID")

                End If

                If Not Request.QueryString("OPLID") Is Nothing Then
                    HidoplID.Value = Request.QueryString("OPLID")

                End If


                If Not Request.QueryString("SFID") Is Nothing Then
                    HidSubfleetID.Value = Request.QueryString("SFID")

                Else
                    BtnSave.Text = "ADD"
                    ' ClearControls()
                End If
                GetOperationalLimit()
                If Session("DatabaseUpgrading") = "0" Then
                    'DisableControls()
                End If

                'BtnSave.Attributes.Add("onClick", "return ValidateControls('UPDATE');")

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If
    End Sub
    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancel.Click
        Response.Redirect("OperationalLimit.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
    End Sub
    Private Sub GetVersionNo()

        Dim dtOperationalLimit As DataTable

        Try
            dtOperationalLimit = go_Bo.GetDBTableData(hidTableId.Value.ToString.Trim)

            If dtOperationalLimit.Rows.Count > 0 Then
                hidVersionNo.Value = dtOperationalLimit.Rows(0)("Version_No").ToString.Trim
            Else
                hidVersionNo.Value = "0"
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub GetOperationalLimit()
        Dim dtOperationalLimit As DataTable
        Try
            If (HidoplimitID.Value = "FORWARD") Then
                dtOperationalLimit = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, HidAirconfigcl.Value.Trim, "FORWARD", HidSubfleetID.Value.Trim)
                If (Not dtOperationalLimit Is Nothing And dtOperationalLimit.Rows.Count > 0) Then
                    txtweight.Text = dtOperationalLimit.Rows(0)("weight")
                    txtmac.Text = dtOperationalLimit.Rows(0)("MAC")
                    hidVersionNo.Value = dtOperationalLimit.Rows(0)("Version_no")
                End If

            ElseIf (HidoplimitID.Value = "FORWARDZERO") Then
                dtOperationalLimit = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, HidAirconfigcl.Value.Trim, "FORWARDZERO", HidSubfleetID.Value.Trim)
                If (Not dtOperationalLimit Is Nothing And dtOperationalLimit.Rows.Count > 0) Then
                    txtweight.Text = dtOperationalLimit.Rows(0)("weight")
                    txtmac.Text = dtOperationalLimit.Rows(0)("MAC")
                    hidVersionNo.Value = dtOperationalLimit.Rows(0)("Version_no")
                End If

            ElseIf (HidoplimitID.Value = "AFTLIMIT") Then
                dtOperationalLimit = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, HidAirconfigcl.Value.Trim, "AFTLIMIT", HidSubfleetID.Value.Trim)
                If (Not dtOperationalLimit Is Nothing And dtOperationalLimit.Rows.Count > 0) Then
                    txtweight.Text = dtOperationalLimit.Rows(0)("weight")
                    txtmac.Text = dtOperationalLimit.Rows(0)("MAC")
                    hidVersionNo.Value = dtOperationalLimit.Rows(0)("Version_no")
                End If

            ElseIf (HidoplimitID.Value = "AFTZERO") Then
                dtOperationalLimit = go_Bo.Get_OperationalLimit(hidTableId.Value.Trim, HidAirconfigcl.Value.Trim, "AFTZERO", HidSubfleetID.Value.Trim)
                If (Not dtOperationalLimit Is Nothing And dtOperationalLimit.Rows.Count > 0) Then
                    txtweight.Text = dtOperationalLimit.Rows(0)("weight")
                    txtmac.Text = dtOperationalLimit.Rows(0)("MAC")
                    hidVersionNo.Value = dtOperationalLimit.Rows(0)("Version_no")
                End If
            End If
        Catch ex As Exception


        End Try
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSave.Click
        Dim intResult As Integer = 0
        Dim oplimitID As Integer = 0
        Dim intVersionNo As Integer = 0
        Dim airconfigclID As String = ""
        Dim oplimitclID As String = ""
        Dim subfleetID As String = ""

        Dim MAC As Integer = 0
        Try

            Dim weight As Integer = 0

            'For intRCnt As Integer = 0 To gvEdit.Rows.Count - 1


            'Add/Update the Operational Limit
            intResult = go_Bo.Create_Update_OprLimit(HidoplID.Value, HidAirconfigcl.Value, HidoplimitID.Value, txtweight.Text.Trim, txtmac.Text.Trim, HidSubfleetID.Value, hidVersionNo.Value)




            If intResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert(' Successfully updated the Operational Limit.');", True)

            ElseIf intResult = 2 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Operational Limit does not exists.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                   "alert('Error occured while updating the Operational Limit.');", True)
            End If






        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while Adding\updating the Operational Limit: " & ex.ToString & ". ');", True)

        End Try
    End Sub

End Class