Imports System.Configuration.ConfigurationManager
Imports System.Security.Principal
Imports WNB_Admin_BO


Partial Public Class ADMS_Airport_Max
    Inherits System.Web.UI.Page
#Region "Impersonation Declarations"

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

            If IsPostBack = False Then
                LblExportFilePath.Text = System.Configuration.ConfigurationManager.AppSettings("ExportAirportMaxMDBFilePath")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Error while loading Airport Max information. \n" & _
                    ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub


    Protected Sub BtnStartUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnStartUpdate.Click
        Try
            Dim loADMS_BAL_Airport_Max As New ADMS_BAL_Airport_Max
            Dim lsNoReciprocalRwy As String = ""
            Dim lsSharklet As String = System.Configuration.ConfigurationManager.AppSettings("ExportedAirportMaxSharplet")
            Dim ExportAirportDataInMDBFilePath As String
            Dim lsSummaryInfo As String = ""

            If ValidateData() = False Then
                Exit Sub
            End If

            ExportAirportDataInMDBFilePath = System.Configuration.ConfigurationManager.AppSettings("ExportAirportMaxMDBFilePath") & _
                                             txtMdbFileName.Text.Trim() & ".LIB"

            ' ImpersonateUser(gs_DomainAccount, gs_Domain, gs_Pwd)

            Dim file As System.IO.FileInfo = New System.IO.FileInfo(ExportAirportDataInMDBFilePath)

            If file.Exists = False Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('" & txtMdbFileName.Text.Trim() & ".LIB" & " File not found at mentioned location.\n" & "');", True)
                Exit Sub
            End If

            'LblExportDataSummary.Text = "Complete File Path: " & ControlChars.Quote & ExportAirportDataInMDBFilePath & ControlChars.Quote

            lsNoReciprocalRwy = loADMS_BAL_Airport_Max.ExportAirportDataInMDB(Session("UserId"), lsSharklet, _
                                ExportAirportDataInMDBFilePath, _
                                lsSummaryInfo)
            'ChkExportInActiveRecords.Checked,

            'UndoImpersonation()

            loADMS_BAL_Airport_Max = Nothing
            LblExportDataSummary.Text = "Data successfully exported.<Br><Br>Export Summary:<Br>" & lsSummaryInfo & _
                                        "<Br><Br>" & Convert.ToString(lsNoReciprocalRwy)

            'ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
            '     "Message", "alert('Data successfully exported. \n" & lsNoReciprocalRwy & "');", True)
            loADMS_BAL_Airport_Max = Nothing

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while exporting Airport Max data \n" & _
                   ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Function validateControl() As Boolean
        Try
            If txtMdbFileName.Text = "" Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidateData() As Boolean

        Dim lsErrMsg As String = ""
        Dim lbResult As Boolean = True

        If txtMdbFileName.Text.Trim = "" Then
            lsErrMsg = "Please enter file name."
            GoTo ReturnResult
        End If


ReturnResult:

        If lsErrMsg <> "" Then
            lbResult = False
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
              "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)
        End If

        Return lbResult

    End Function


    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Try
            Response.Redirect("ADMS_home.aspx")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while Closing Airport Max data \n" & _
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
            Throw ex
        End Try

    End Sub


End Class