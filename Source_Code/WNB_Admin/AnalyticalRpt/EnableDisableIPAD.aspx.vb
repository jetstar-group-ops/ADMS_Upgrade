
Imports System.IO
Imports System.Data
Partial Public Class EnableDisableIPAD
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


            'Dim ls_Message As String = ""

            'If go_Bo.IsUserHasPermission(Session("UserId"), WNB_Common.Enums.Functionalities.EnableDisableIPAD, _
            '                      "", WNB_Common.Enums.Functionalities.SystemAdministration) = False Then

            '    ls_Message = "You don't have permission to Enable/Disable IPAD."
            '    Response.Redirect("../Home.aspx?Message=" & ls_Message)
            '    Exit Sub
            'End If

            FillDDLIPADUDID(ddlIpadUDId, "")
            BindGridViewData()

        End If
        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub

    Private Sub FillDDLIPADUDID(ByRef ddlControl As System.Web.UI.WebControls.DropDownList, ByVal strFirstItem As String)

        Dim dtIPADUDIDs As DataTable
        Try

            dtIPADUDIDs = go_Bo.Get_IPADUDID()

            ddlControl.DataSource = dtIPADUDIDs
            ddlControl.DataBind()
            ddlControl.Items.Insert(0, New ListItem(strFirstItem, ""))

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while populating IPAD UDID Dropdownlist.');", True)
        End Try
    End Sub

    Private Sub BindGridViewData()
        Dim dsIPADDetails As New DataSet


        Try
            dsIPADDetails = go_Bo.Get_IPADDetails(ddlIpadUDId.SelectedValue.ToString.Trim)

            gvIPADDetails.DataSource = dsIPADDetails
            gvIPADDetails.DataBind()

            gvIPADDetails.EditIndex = -1

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dsIPADDetails As New DataSet


        Try
            BindGridViewData()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub gvIPADDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvIPADDetails.RowCommand

        Dim strArray As String()
        strArray = e.CommandArgument.ToString().Split(",").ToArray()

        Dim objTempButton As Button
        Dim gvRow As GridViewRow
        Dim chkIsDisable As CheckBox
        Dim IsDisable As Integer
        Dim VersionNo As Integer
        objTempButton = DirectCast(e.CommandSource, Button)
        gvRow = DirectCast(objTempButton.NamingContainer, GridViewRow)

        chkIsDisable = CType(gvRow.Cells(7).FindControl("chkDisable"), CheckBox)

        IsDisable = Convert.ToInt32(chkIsDisable.Checked)
        VersionNo = Convert.ToInt32(strArray(2).ToString.Trim)
        UpdateRecord(strArray(0), strArray(1), IsDisable, VersionNo)
    End Sub

    Private Sub UpdateRecord(ByVal IpadID As String, ByVal Empno As String, ByVal Isdisbled As Integer, ByVal VersionNo As Integer)
        Try
            Dim intResult As Integer = 0


            intResult = go_Bo.Update_IPADDetails(IpadID, Empno, Isdisbled, VersionNo, Session("UserId").ToString.Trim)
            If intResult = 1 Then
                BindGridViewData()

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
               "alert('IPAD details have been updated successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                "alert('Error occured while updating IPAD details.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Error Occured While updating IPAD details.');", True)

        End Try
    End Sub
End Class