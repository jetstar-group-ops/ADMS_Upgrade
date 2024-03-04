Imports System.Configuration.ConfigurationManager
Imports System.Security.Principal
Imports WNB_Admin_BO

Partial Public Class ADMS_Export_Airport_Manager
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
                LblExportFilePath.Text = System.Configuration.ConfigurationManager.AppSettings("ExportedAirportManagerTextFilePath") & ""
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while getting export airport manager data. \n" & _
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


    Private Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        Try
            Dim loADMS_BAL_Export_Airport_Manager As New ADMS_BAL_Export_Airport_Manager
            Dim lsResultPath As String
            Dim lsExportedOutputFilePath As String
            Dim lsSummary As String = ""

            lsExportedOutputFilePath = System.Configuration.ConfigurationManager.AppSettings("ExportedAirportManagerTextFilePath")

            ImpersonateUser(gs_DomainAccount, gs_Domain, gs_Pwd)

            lsResultPath = loADMS_BAL_Export_Airport_Manager.ExportAirportManagerFile(Session("UserId"), _
                                                                         Session("AirlineCode"), _
                                                                         LblExportFilePath.Text, _
                                                                         lsSummary, _
                                                                         ChkExportInActiveRecords.Checked)

            LblExportDataSummary.Text = " Export Completed: <Br>" & lsSummary
            Session("AirportManagerFilePath") = lsResultPath

            Dim file As System.IO.FileInfo = New System.IO.FileInfo(lsResultPath) '-- if the file exists on the server  
            If file.Exists Then 'set appropriate headers 
                'Dim fileName As String = Session("UserId") + file.Name '+ ".csv"
                'file.CopyTo(Server.MapPath(Request.ApplicationPath + "\Temp\" + fileName), True)
                'Response.Clear()
                'Response.ContentType = "text/plain"
                'Response.AppendHeader("Content-Disposition", "attachment; filename=" & fileName & "")
                'Response.TransmitFile("..\Temp\" + fileName)
                'Response.Flush()
                'Response.End()

                'ClientScript.RegisterStartupScript(Me.GetType(), "Download", "window.Open('http://localhost:57854/ADMS/AirportManagerFileDownload.aspx'")

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "popup", "<script language=javascript>window.open('AirportManagerFileDownload.aspx','','width=300px,height=200px')</script>")
            End If
            UndoImpersonation()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                             "Message", "alert('Error while exporting airport manager data. \n" & _
                             ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Try
            Response.Redirect("ADMS_Home.aspx")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Error while closing airport manager data. \n" & _
                            ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub
End Class