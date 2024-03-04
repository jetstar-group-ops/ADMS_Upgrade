
Imports System.IO
Imports System.Data
Imports System.Net

Partial Public Class APNPushRequest
    Inherits System.Web.UI.Page
    Private go_Bo As New WNB_Admin_BO.WNB_Admin_BO()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then

            'Dim ls_Message As String = ""

            Server.ScriptTimeout = 900000000
            SendNotificationNew()
            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.WNB_FULL_ACCESS, "", 0) = False Then

                lsMessage = "You don't have permission on Weight and Balance System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            btnAPNPushRequest.Attributes.Add("onClick", "return ValidateControls('SENDREQUEST');")

        End If

        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub

    Private Sub SendNotificationNew()

        Try

            Dim client As New WebClient()
            Dim dsResult As New DataSet
            dsResult = go_Bo.APNPushRequest()

            CmbDevice.DataSource = dsResult
            CmbDevice.DataTextField = "IPAD_UDID"
            CmbDevice.DataValueField = "Notification_Token_Number"
            CmbDevice.DataBind()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub FillDevice()
        Try

        Catch ex As Exception

        End Try
    End Sub


    Private Sub btnAPNPushRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAPNPushRequest.Click
        'Clear the grid results

        grdAPNResults.DataSource = Nothing
        grdAPNResults.DataBind()

        'End Grid clear

        If Not IsValidate("SENDREQUEST") Then
            Exit Sub
        End If



        Dim dsResult As DataSet = Nothing
        Dim intResult As Integer = 5
        Dim intBadge As Integer = 0
        Dim strMessage As String = ""
        Dim intAPNRequestId As Integer = 0
        Dim dtRequestDetail As DataTable = Nothing
        'Dim bSandBox As Boolean
        Dim intPushResult As Integer = 0
        Dim bSuccess As Boolean = True

        Dim drAPNRequest As DataRow

        'Dim strCertificatePath As String = ConfigurationSettings.AppSettings("CertificatePath")
        'Dim strPassword As String = ConfigurationSettings.AppSettings("CertificatePassword")
        ' bSandBox = Convert.ToBoolean(ConfigurationSettings.AppSettings("SandBox"))

        Try
            dsResult = go_Bo.APNTableSchema()
            If Not dsResult Is Nothing Then
                dtRequestDetail = dsResult.Tables(0).Copy
                intAPNRequestId = Convert.ToInt32(dsResult.Tables(1).Rows(0)("APNRequestID"))
            End If

            dsResult = Nothing

            If chkBadge.Checked = True Then
                intBadge = 1
            End If

            'code for sending msg to single device Start
            If (rdCheckSingle.Checked = True) Then

                Try
                    intPushResult = SendNotificationViaPhp(txtNotification.Text.ToString.Trim, intBadge.ToString, CmbDevice.SelectedValue.Trim())

                    If intPushResult = 1 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                            "alert('Successfully performed APN Push Request.');", True)

                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Reset", _
                           "ResetValue('All');", True)

                        txtNotification.Text = String.Empty
                        chkBadge.Checked = False

                    Else
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                                "alert('Error Occured While Processing APN Push Request.');", True)
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Reset", _
                           "ResetValue('Single');", True)

                    End If
                Catch ex As Exception
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                                "alert('Error Occured While Processing APN Push Request.');", True)
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Reset", _
                          "ResetValue('Single');", True)


                End Try
                intResult = go_Bo.CreateAPNRequestMaster(intAPNRequestId, txtNotification.Text.ToString.Trim, System.DateTime.Now, intBadge, Session("UserId"), dtRequestDetail)


                Return
            End If
            'End


            dsResult = go_Bo.APNPushRequest()


            If Not dsResult Is Nothing Then


                For intCnt As Integer = 0 To dsResult.Tables(0).Rows.Count - 1

                    drAPNRequest = dtRequestDetail.NewRow
                    drAPNRequest("APN_Request_ID") = intAPNRequestId
                    drAPNRequest("IPAD_UDID") = dsResult.Tables(0).Rows(intCnt)("IPAD_UDID").ToString.Trim
                    drAPNRequest("IsDisabled") = 0
                    drAPNRequest("Notification_Token_Number") = dsResult.Tables(0).Rows(intCnt)("Notification_Token_Number").ToString.Trim

                    Try
                        intPushResult = SendNotificationViaPhp(txtNotification.Text.ToString.Trim, intBadge.ToString, dsResult.Tables(0).Rows(intCnt)("Notification_Token_Number").ToString.Trim)


                        If intPushResult = 0 Then
                            bSuccess = False
                            strMessage = "Failed"
                        Else
                            strMessage = "Pass"
                        End If

                    Catch ex As Exception
                        strMessage = "Failed"
                    End Try

                    drAPNRequest("Message") = strMessage
                    dtRequestDetail.Rows.Add(drAPNRequest)
                    dtRequestDetail.AcceptChanges()

                Next



                If bSuccess = False Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "MessageAll", _
                           "alert('APN Push Request have not been Successfully performed for all Ipad.');", True)

                Else
                    If intPushResult = 1 Then
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "MessageAll", _
                            "alert('Successfully performed APN Push Request.');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "MessageAll", _
                                "alert('Error Occured While Processing APN Push Request.');", True)

                    End If
                End If

                intResult = go_Bo.CreateAPNRequestMaster(intAPNRequestId, txtNotification.Text.ToString.Trim, System.DateTime.Now, intBadge, Session("UserId"), dtRequestDetail)

                dtRequestDetail.Columns.Remove("APN_Request_ID")

                grdAPNResults.DataSource = dtRequestDetail.DefaultView
                grdAPNResults.DataBind()

                txtNotification.Text = String.Empty
                chkBadge.Checked = False


            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Error Occured While Processing APN Push Request. Error Details :'  " & ex.Message.ToString().Replace("'", "") & ".);", True)


        End Try

    End Sub

    Private Function SendNotificationViaPhp(ByVal psMsg As String, ByVal psBadge As String, ByVal psDeviceToken As String) As Integer

        Dim WebReq As HttpWebRequest
        Dim IpadUrl As String = ""
        Dim WebRes As HttpWebResponse
        Dim intResult As Integer = 0

        Dim strURL As String = ConfigurationSettings.AppSettings("APNPushReqURL")
        'Dim strPassword As String = ConfigurationSettings.AppSettings("APNPushNCPassword")
        'Dim strUserId As String = ConfigurationSettings.AppSettings("APNPushNCUserId")
        'Dim strDomain As String = ConfigurationSettings.AppSettings("APNPushNCDomain")





        Try

            strURL = strURL & "message=" & psMsg & "&badge=" & psBadge & "&devicetoken=" & psDeviceToken
            WebReq = WebRequest.Create(strURL)



            WebReq.Timeout = WebReq.Timeout * 20
            WebRes = CType(WebReq.GetResponse(), HttpWebResponse)



            If WebRes.StatusCode = HttpStatusCode.OK Then
                'Or WebRes.StatusCode = HttpStatusCode.Accepted Then
                intResult = 1
            End If

            WebRes.Close()


        Catch ex As Exception
            intResult = 0
            Throw New Exception(ex.ToString)
        Finally
            WebReq = Nothing
            WebRes = Nothing
        End Try

        Return intResult

    End Function



    Private Function IsValidate(ByVal strCmd As String) As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder

        Try
            If Not strCmd Is Nothing AndAlso (String.Equals(strCmd, "SENDREQUEST")) Then

                If Not txtNotification.Text Is Nothing AndAlso String.Equals(txtNotification.Text.Trim(), String.Empty) Then
                    strErrorMessage.Append("\n - Required Notification Message.")
                    bReturn = False
                End If

            End If

            If Not bReturn Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strErrorMessage.ToString.Trim() + "');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('Error occured while validating Controls details.');", True)
        End Try

        Return bReturn
    End Function

    Public Function proxyBypass() As WebProxy
        Dim ts4Webproxy As WebProxy
        '-----------Proxy------------------
        If System.Configuration.ConfigurationManager.AppSettings("WebProxyName") <> "" Then

            Dim ByPassUrls() As String
            ByPassUrls = System.Configuration.ConfigurationManager.AppSettings("ByPassUrls").Split(";")

            ts4Webproxy = New WebProxy(System.Configuration.ConfigurationManager.AppSettings("WebProxyName"), _
                     Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("WebProxyPortNumber")))

            ts4Webproxy.Credentials = New NetworkCredential(ConfigurationManager.AppSettings("NetworkCredentialUserid"), _
                                        ConfigurationManager.AppSettings("NetworkCredentialPassword"), _
                                    ConfigurationManager.AppSettings("NetworkCredentialDomain"))
        End If
        '-----------Proxy------------------
        Return ts4Webproxy

    End Function


End Class