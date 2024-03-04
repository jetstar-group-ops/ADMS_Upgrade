
Imports System.IO
Imports System.Data
Partial Public Class DBValidation
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.WNB_FULL_ACCESS, "", 0) = False Then

                lsMessage = "You don't have permission on Weight and Balance System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            btnExport.Enabled = False

            'Start: new code added by ajay on 12-feb-15
            Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()
            Dim dsDBDetail As DataTable
            Dim intResult As Integer

            Try
                intResult = objBo.Is_Database_Upgrade_Running()
                Session("DatabaseUpgrading") = intResult

                If intResult = 1 Then
                    btnPublishDB.Enabled = True
                    btnDBUpgradeCancel.Enabled = True
                    btnDBUpgrade.Enabled = False
                Else
                    btnPublishDB.Enabled = False
                    btnDBUpgradeCancel.Enabled = False
                    btnDBUpgrade.Enabled = True
                End If

                dsDBDetail = objBo.GetDBDetails()
                If Not dsDBDetail Is Nothing Then
                    If dsDBDetail.Rows.Count > 0 Then
                        lblCurrDBVarVal.Text = dsDBDetail.Rows(0)("CurrDBVar").ToString()
                        lblPubDateVal.Text = Date.Parse(dsDBDetail.Rows(0)("PublishedDate").ToString()).ToString("dd-MMM-yyyy")
                        lblPubByVal.Text = dsDBDetail.Rows(0)("PublishedBy").ToString()
                    End If
                End If

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while getting weight and Balance Database Details : " & ex.Message.ToString & ".');", True)
            End Try

            'end

        End If
        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub

    Protected Sub btnSanityChecks_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSanityChecks.Click
        Dim dsResult As DataSet = Nothing
        Dim intResult As Integer = 5


        Try

            dsResult = go_Bo.DatabaseSanityChecks()

            If Not dsResult Is Nothing Then

                If dsResult.Tables.Count > 1 Then

                    If dsResult.Tables(0).Rows.Count > 0 Then
                        intResult = Convert.ToInt16(dsResult.Tables(0).Rows(0)(0))
                    End If

                    BindGridViewData(dsResult.Tables(1))
                End If

            End If


            If intResult = 1 Then
                btnExport.Enabled = True
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('Successfully performed database sanity checks and all the validations are passed.');", True)
            ElseIf intResult = 0 Then
                btnExport.Enabled = True
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Successfully performed database sanity checks and not all the validations are passed; see full details in below table.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error Occured While performing database sanity checks.');", True)

            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Error Occured While performing database sanity checks. Error Details :'  " & ex.Message.ToString().Replace("'", "") & ".);", True)

        End Try
    End Sub

    Private Sub BindGridViewData(ByVal dtResult As DataTable)

        Try
            gvSanityCheck.DataSource = dtResult
            gvSanityCheck.DataBind()

            dgRptHeader.DataSource = dtResult
            dgRptHeader.DataBind()


            gvSanityCheck.EditIndex = -1

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Export Datagrid data into Excel file using Response.AddHeader and Response.ContentType
    ''' </summary>   
    ''' <remarks>No Parameter</remarks>
    Private Sub ExportGridToExcel()
        Try
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment;filename=CPR_Establishment.xls")
            Response.ContentType = "application/vnd.ms-excel"
            ' Remove the charset from the Content-Type header.
            Response.Charset = ""
            ' Turn off the view state.
            Me.EnableViewState = False

            Dim tw As New System.IO.StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)

            ' Get the HTML for the control.
            dgRptHeader.RenderControl(hw)
            ' Write the HTML back to the browser.
            Response.Write(tw.ToString())
            ' End the response.
            Response.End()
        Catch ex As Exception
            'Throw ex
        End Try
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        ExportGridToExcel()
    End Sub

    Protected Sub btnDBUpgrade_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDBUpgrade.Click
        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()

        Try
            objBo.Start_Database_Upgrade_Session()
            btnPublishDB.Enabled = True
            btnDBUpgradeCancel.Enabled = True
            btnDBUpgrade.Enabled = False
            Session("DatabaseUpgrading") = "1"

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while starting Database Upgrade Session .');", True)
        End Try
    End Sub

    Protected Sub btnDBUpgradeCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDBUpgradeCancel.Click
        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()

        Try
            objBo.Cancel_Database_Upgrade_Session()
            btnPublishDB.Enabled = False
            btnDBUpgradeCancel.Enabled = False
            btnDBUpgrade.Enabled = True
            Session("DatabaseUpgrading") = "0"

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while canceling Database Upgrade Session.');", True)
        End Try
    End Sub

End Class