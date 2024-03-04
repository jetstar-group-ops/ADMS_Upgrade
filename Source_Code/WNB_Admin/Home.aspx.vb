Public Partial Class Home1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserId") & "" = "" Then
            Response.Redirect("Login.aspx")
        End If
        If Not IsPostBack Then
            If Request.QueryString("Message") <> "" Then
                trMsg.Attributes.Add("style", "display:inline;height:35Px;")
                lblMessage.Text = Request.QueryString("Message").ToString()
            End If

            'Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()
            'Dim dsDBDetail As DataTable
            'Dim intResult As Integer

            'Try
            '    intResult = objBo.Is_Database_Upgrade_Running()
            '    Session("DatabaseUpgrading") = intResult

            '    If intResult = 1 Then
            '        btnPublishDB.Enabled = True
            '        btnDBUpgradeCancel.Enabled = True
            '        btnDBUpgrade.Enabled = False
            '    Else
            '        btnPublishDB.Enabled = False
            '        btnDBUpgradeCancel.Enabled = False
            '        btnDBUpgrade.Enabled = True
            '    End If

            '    dsDBDetail = objBo.GetDBDetails()
            '    If Not dsDBDetail Is Nothing Then
            '        If dsDBDetail.Rows.Count > 0 Then
            '            lblCurrDBVarVal.Text = dsDBDetail.Rows(0)("CurrDBVar").ToString()
            '            lblPubDateVal.Text = Date.Parse(dsDBDetail.Rows(0)("PublishedDate").ToString()).ToString("dd-MMM-yyyy")
            '            lblPubByVal.Text = dsDBDetail.Rows(0)("PublishedBy").ToString()
            '        End If
            '    End If

            'Catch ex As Exception
            '    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while getting weight and Balance Database Details : " & ex.Message.ToString & ".');", True)
            'End Try
        End If

    End Sub

    'Protected Sub btnDBUpgrade_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDBUpgrade.Click
    '    Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()

    '    Try
    '        objBo.Start_Database_Upgrade_Session()
    '        btnPublishDB.Enabled = True
    '        btnDBUpgradeCancel.Enabled = True
    '        btnDBUpgrade.Enabled = False
    '        Session("DatabaseUpgrading") = "1"

    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while starting Database Upgrade Session .');", True)
    '    End Try
    'End Sub

    'Protected Sub btnDBUpgradeCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDBUpgradeCancel.Click
    '    Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()

    '    Try
    '        objBo.Cancel_Database_Upgrade_Session()
    '        btnPublishDB.Enabled = False
    '        btnDBUpgradeCancel.Enabled = False
    '        btnDBUpgrade.Enabled = True
    '        Session("DatabaseUpgrading") = "0"

    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while canceling Database Upgrade Session.');", True)
    '    End Try
    'End Sub
End Class