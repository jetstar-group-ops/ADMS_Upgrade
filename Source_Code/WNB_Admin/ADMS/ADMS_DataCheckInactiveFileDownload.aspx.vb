Imports System.Configuration.ConfigurationManager
Imports System.Security.Principal

Partial Public Class ADMS_DataCheckInactiveFileDownload
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
            Dim path As String = Session("InactiveFilePath")

            ImpersonateUser(gs_DomainAccount, gs_Domain, gs_Pwd)

            Dim file As System.IO.FileInfo = New System.IO.FileInfo(path) '-- if the file exists on the server  
            If file.Exists Then 'set appropriate headers 
                Dim fileName As String = file.Name + Session("UserId") + ".txt"
                file.CopyTo(Server.MapPath(Request.ApplicationPath + "\Temp\" + fileName), True)
                Response.Clear()
                Response.ContentType = "text/plain"
                Response.AppendHeader("Content-Disposition", "attachment; filename=" & file.Name & ".TXT")
                Response.TransmitFile(path)
                Response.Flush()
                Response.End()
            End If



            UndoImpersonation()

        Catch ex As Exception
            Throw ex
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