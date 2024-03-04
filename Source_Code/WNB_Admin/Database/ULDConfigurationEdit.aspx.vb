Public Partial Class ULDConfigurationEdit
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



                If Not Request.QueryString("AID") Is Nothing Then
                    lblAircraft.Text = Request.QueryString("AID")

                End If
                If Not Request.QueryString("CID") Is Nothing Then
                    lblAircarftConfig.Text = Request.QueryString("CID")

                End If
                If Not Request.QueryString("UID") Is Nothing Then
                    lblULDConfiguration.Text = Request.QueryString("UID")

                End If
                If Not Request.QueryString("VID") Is Nothing Then
                    hidversion.Value = Request.QueryString("VID")

                End If


                GetULDRef()
                GetULDPosition()

                If Not Request.QueryString("ID") Is Nothing Then
                    hidID.Value = Request.QueryString("ID")
                    GetPostionDetails()
                End If

               
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Error occured during page load: " & ex.ToString & ".');", True)
            End Try
        End If

    End Sub

    Private Sub GetULDRef()
        Try
            Dim dtULDref As DataTable
            dtULDref = go_Bo.Get_ULDDefiniton("26", lblAircraft.Text.Trim)
            ddlUldRef.DataSource = dtULDref.DefaultView
            ddlUldRef.DataBind()
            ddlUldRef.Items.Add(New ListItem("", ""))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub GetULDPosition()
        Try
            Dim dtULDConfiguratoin As DataTable
            dtULDConfiguratoin = go_Bo.Get_ULDPosition("4", lblAircraft.Text, 0)
            ddlUldPosition.DataSource = dtULDConfiguratoin.DefaultView
            ddlUldPosition.DataBind()
            ddlUldPosition.Items.Add(New ListItem("", ""))
        Catch ex As Exception

        End Try
    End Sub


    Private Sub GetPostionDetails()
        Try
            Dim dtPositionDetails As DataTable
            dtPositionDetails = go_Bo.Get_ULDConfiguration(hidTableId.Value, lblAircraft.Text.Trim, Convert.ToInt32(hidID.Value), lblAircarftConfig.Text, lblULDConfiguration.Text)
            If (dtPositionDetails.Rows.Count > 0) Then
                ddlUldPosition.Text = dtPositionDetails.Rows(0)("pos_ref1")
                ddlUldRef.Text = dtPositionDetails.Rows(0)("uld_conf_cl_id")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnsave.Click
        Try

            Dim intresult As Integer = 1
            If (hidID.Value = "") Then
                hidID.Value = "0"
            End If
            'intresult = go_Bo.CreateUpdateAirCraftPostion(Convert.ToInt32(hidID.Value), lblAircarftConfig.Text.Trim, txtPosition.Text, _
            '                                             lblULDConfiguration.Text.Trim, TxtMaxLoad.Text.Trim, TxtPosArm.Text.Trim, lblAircraft.Text, _
            '                                             hidversion.Value, "Admin")
            If intresult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Reocord Saved Successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                  "alert('Error occured while Adding a new Choice.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while Saving Record : " & ex.ToString & ". ');", True)

        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try
            Response.Redirect("UnderFloorConfiguration.aspx?FID=" & hidFunctionId.Value & "&TID=" & hidTableId.Value)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Error occured while Cancel : " & ex.ToString & ". ');", True)

        End Try

    End Sub
End Class