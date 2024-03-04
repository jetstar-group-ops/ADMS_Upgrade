Imports System.IO
Imports System.Security.Principal
Imports System.Configuration.ConfigurationManager
Imports WNB_Admin_BO

Partial Public Class ADMS_DataCheckReport
    Inherits System.Web.UI.Page

#Region "Impersonation"


    Private LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private LOGON32_PROVIDER_DEFAULT As Integer = 0

    Private ImpersonationContext As WindowsImpersonationContext

    Declare Function LogonUserA Lib "advapi32.dll" (ByVal lpszUsername As String, _
                            ByVal lpszDomain As String, _
                            ByVal lpszPassword As String, _
                            ByVal dwLogonType As Integer, _
                            ByVal dwLogonProvider As Integer, _
                            ByRef phToken As IntPtr) As Integer


    Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
                            ByVal ExistingTokenHandle As IntPtr, _
                            ByVal ImpersonationLevel As Integer, _
                            ByRef DuplicateTokenHandle As IntPtr) As Integer

    Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long

    Dim gs_DomainAccount As String = AppSettings("ADMS_DomainAccount")
    Dim gs_Pwd As String = AppSettings("ADMS_Pwd")
    Dim gs_Domain As String = AppSettings("ADMS_Domain")

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.ADMS, "", 0) = False Then

                lsMessage = "You don't have permission on Airport Database Management System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            If Me.IsPostBack = False Then

                Dim loADMS_BAL_ADM As ADMS_BAL_ADM
                Dim loADMS_BAL_RDM As ADMS_BAL_RDM
                Dim lsIcao As String = ""
                Dim lsRwyId As String = ""
                Dim lsRwyMod As String = ""
                Dim lsModuleId As String = ""
                Dim loDataCheckReport As DataTable
                Dim psAirlineCode As String = ""

                lsIcao = Request("ICAO")
                'lsRwyId = Request("RwyId")
                'lsRwyMod = Request("RwyMod")
                lsModuleId = Request("ModuleId")

                If lsModuleId = "A" Then
                    loADMS_BAL_ADM = New ADMS_BAL_ADM
                    'TxtSearchAirportId.Text = lsIcao
                    loDataCheckReport = loADMS_BAL_ADM.AirportDataCheck(lsIcao, True, Session("UserId"), True, True, psAirlineCode)

                ElseIf lsModuleId = "R" Then
                    loADMS_BAL_RDM = New ADMS_BAL_RDM
                    loADMS_BAL_RDM.RunwayDataCheck(loDataCheckReport, lsIcao, lsRwyId, _
                        lsRwyMod, True, True, Nothing, Session("UserId"))
                End If


                DgrdDataCheckReport.DataSource = loDataCheckReport
                DgrdDataCheckReport.DataBind()


                LblErrorFilePath.Text = System.Configuration.ConfigurationManager.AppSettings("ExportedDataCheckErrorTextFilePath") & ""
                LblInactiveFilePath.Text = System.Configuration.ConfigurationManager.AppSettings("ExportedDataCheckInactiveTextFilePath") & ""


                Dim sbErrorResult = New StringBuilder
                Dim sbInactiveResult = New StringBuilder
                Dim intInactiveRecordCheck As Integer = 0
                Dim lsErrorOutputFilePath As String = LblErrorFilePath.Text & psAirlineCode & "_CheckAll_" & Session("CurrentAiracCycle")
                Dim lsInactiveOutputFilePath As String = LblInactiveFilePath.Text & psAirlineCode & "_Inactive_" & Session("CurrentAiracCycle")


                sbErrorResult.Append("Airport Data Check File -     Jetstar     - Cycle " & Session("CurrentAiracCycle").ToString() & "")
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append("Run by " & Session("UserId") & " at " & Date.Now.ToString("dd/MM/yyyy HH:mm:ss") & "")
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append(Environment.NewLine)

                sbInactiveResult.Append("Airport Data Inactive Records -     Jetstar     - Cycle " & Session("CurrentAiracCycle").ToString() & "")
                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append("Run by " & Session("UserId") & " at " & Date.Now.ToString("dd/MM/yyyy HH:mm:ss") & "")
                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append(Environment.NewLine)

                For i = 0 To loDataCheckReport.Rows.Count - 1
                    For c = 0 To loDataCheckReport.Columns.Count - 1
                        If c = 0 And Convert.ToString(loDataCheckReport.Rows(i)(c)).Trim() = "" Then
                            Exit For
                        Else
                            If intInactiveRecordCheck = 0 Then
                                If Convert.ToString(loDataCheckReport.Rows(i)(c)).Trim() <> "Inactive Records:" Then
                                    If c = 5 Or c = 7 Then
                                        Continue For
                                    Else
                                        sbErrorResult.Append(loDataCheckReport.Rows(i)(c).ToString() & " ")
                                    End If
                                End If
                            Else
                                If c <= 4 Then
                                    sbInactiveResult.Append(loDataCheckReport.Rows(i)(c).ToString() & " ")
                                End If
                            End If

                            If Convert.ToString(loDataCheckReport.Rows(i)(c)).Trim() = "Error:" Then
                                Exit For
                            End If
                            If Convert.ToString(loDataCheckReport.Rows(i)(c)).Trim() = "Inactive Records:" Then
                                sbInactiveResult.Append(loDataCheckReport.Rows(i)(c).ToString() & " ")
                                intInactiveRecordCheck = 1
                                Exit For
                            End If
                        End If

                    Next
                    If intInactiveRecordCheck = 0 Then
                        sbErrorResult.AppendLine()
                    Else
                        sbInactiveResult.AppendLine()
                    End If
                Next
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append("End of Airport Data Check File - Cycle " & Session("CurrentAiracCycle").ToString() & "")
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append("Completed by " & Session("UserId") & " at " & Date.Now.ToString("dd/MM/yyyy HH:mm:ss") & "")
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append(Environment.NewLine)
                sbErrorResult.Append("Checked: ______________________________________   Date: ______________________")

                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append("End of Airport Data Inactive Records - Cycle " & Session("CurrentAiracCycle").ToString() & "")
                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append("Completed by " & Session("UserId") & " at " & Date.Now.ToString("dd/MM/yyyy HH:mm:ss") & "")
                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append(Environment.NewLine)
                sbInactiveResult.Append("Checked: ______________________________________   Date: ______________________")

                ImpersonateUser(gs_DomainAccount, gs_Domain, gs_Pwd)

                Dim strWriter As StreamWriter
                strWriter = New StreamWriter(lsErrorOutputFilePath, False)
                strWriter.WriteLine(sbErrorResult.ToString)
                strWriter.Close()

                strWriter = New StreamWriter(lsInactiveOutputFilePath, False)
                strWriter.WriteLine(sbInactiveResult.ToString)
                strWriter.Close()

                UndoImpersonation()

                Dim fileError As System.IO.FileInfo = New System.IO.FileInfo(lsErrorOutputFilePath) '-- if the file exists on the server  
                Dim fileInacitve As System.IO.FileInfo = New System.IO.FileInfo(lsInactiveOutputFilePath) '-- if the file exists on the server  

                Session("ErrorFilePath") = lsErrorOutputFilePath
                Session("InactiveFilePath") = lsInactiveOutputFilePath
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Error while generating data check report. \n" & _
                    ex.Message.Replace("'", "") & "');", True)
        End Try


    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Try
            If Request("ICAO") = "" Then
                Response.Redirect("ADMS_Home.aspx")
            Else
                Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & Request("ICAO"))
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while closing data check report. \n" & _
                   ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Protected Sub btnExportErrors_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportErrors.Click
        Try
            Dim lsErrorOutputFilePath As String = Server.MapPath("~/Temp/JQ_CheckAll_") '& Session("AiracCycle").ToString() & "/")
            Dim fileError As System.IO.FileInfo = New System.IO.FileInfo(lsErrorOutputFilePath) '-- if the file exists on the server  

            If fileError.Exists Then 'set appropriate headers 
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "popup", "<script language=javascript>window.open('ADMS_DataCheckErrorFileDownload.aspx','','width=300px,height=200px')</script>")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                 "Message", "alert('Error while Exporting Error data check report. \n" & _
                 ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnExportInactiveRecords_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportInactiveRecords.Click
        Try
            Dim lsInactiveOutputFilePath As String = Server.MapPath("~/Temp/JQ_Disabled_")
            Dim fileInacitve As System.IO.FileInfo = New System.IO.FileInfo(lsInactiveOutputFilePath)

            If fileInacitve.Exists Then 'set appropriate headers 
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "popup", "<script language=javascript>window.open('ADMS_DataCheckInactiveFileDownload.aspx','','width=300px,height=200px')</script>")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                 "Message", "alert('Error while Exporting disable data check report. \n" & _
                 ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Function ImpersonateUser(ByVal userName As String, _
    ByVal domain As String, ByVal password As String) As Boolean

        Dim tempWindowsIdentity As WindowsIdentity
        Dim token As IntPtr = IntPtr.Zero
        Dim tokenDuplicate As IntPtr = IntPtr.Zero

        ImpersonateUser = False

        If RevertToSelf() Then
            If LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, token) <> 0 Then
                If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                    tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                    ImpersonationContext = tempWindowsIdentity.Impersonate()
                    If Not ImpersonationContext Is Nothing Then
                        ImpersonateUser = True
                    End If
                End If
            End If
        End If

        If Not tokenDuplicate.Equals(IntPtr.Zero) Then
            CloseHandle(tokenDuplicate)
        End If
        If Not token.Equals(IntPtr.Zero) Then
            CloseHandle(token)
        End If

    End Function

    Private Sub UndoImpersonation()
        Try
            If Not ImpersonationContext Is Nothing Then
                ImpersonationContext.Undo()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                              "Message", "alert('Error while exporting airport manager data. \n" & _
                              ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

End Class