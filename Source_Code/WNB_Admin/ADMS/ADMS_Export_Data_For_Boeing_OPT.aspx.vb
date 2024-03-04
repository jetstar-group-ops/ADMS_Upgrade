Imports System.Configuration.ConfigurationManager
Imports System.Security.Principal
Imports WNB_Admin_BO
Imports System.IO

Partial Public Class ADMS_Export_Data_For_Boeing_OPT
    Inherits System.Web.UI.Page

#Region "Impersonation Constants"
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
#End Region

    Dim gs_DomainAccount As String = AppSettings("ADMS_DomainAccount")
    Dim gs_Pwd As String = AppSettings("ADMS_Pwd")
    Dim gs_Domain As String = AppSettings("ADMS_Domain")
    Dim strLogFile As String = AppSettings("ExportLog")


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
                File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> Page load event ", Environment.NewLine, Date.Today, Session("UserId")))
                LblEmptyBoeingOptSqlieDbFile.Text = AppSettings("EmptyBoeingOptSqlieDbFile") & ""

                Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
                Dim lsAiracCode As String = ""
                Dim lsCurrentAiracCycleStartDate As String = ""
                Dim lsCurrentAiracCycleEndDate As String = ""
                Dim lsNextAiracCycleStartDate As String = ""

                loADMS_BAL_Data_Checks.GetAIRAC_Cycle_Data(Date.Now.AddDays(27).ToString("dd/MM/yyyy"), _
                        lsAiracCode, lsCurrentAiracCycleStartDate, _
                        lsCurrentAiracCycleEndDate, lsNextAiracCycleStartDate)

                LblExportedOptDatabaseFile.Text = AppSettings("ExportedBoeingOptSqlieDbFileLocation") & _
                        "\" & Session("AirlineCode") & "_Airport_" & lsAiracCode & ".sdb"
                File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> Page load GetAIRAC_Cycle_Data completed ", Environment.NewLine, Date.Now, Session("UserId")))
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                   "Message", "alert('Error while exporting Boeing OPT data. \n" & _
                   ex.Message.Replace("'", "") & "');", True)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> Page load: " & ex.ToString(), Environment.NewLine, Date.Now, Session("UserId")))
        End Try

    End Sub

    Private Sub BtnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExport.Click

        Try
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}  -> Export Started", Environment.NewLine, Date.Now, Session("UserId")))
            Dim loBoeingOptData As DataSet
            Dim loADMS_BAL_Export_Data_For_Boeing_OPT As New ADMS_BAL_Export_Data_For_Boeing_OPT
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim lsAiracCode As String = ""
            Dim lsCurrentAiracCycleStartDate As String = ""
            Dim lsCurrentAiracCycleEndDate As String = ""
            Dim lsNextAiracCycleStartDate As String = ""

            loADMS_BAL_Data_Checks.GetAIRAC_Cycle_Data(Date.Now.AddDays(27).ToString("dd/MM/yyyy"), _
                    lsAiracCode, lsCurrentAiracCycleStartDate, _
                    lsCurrentAiracCycleEndDate, lsNextAiracCycleStartDate)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}  -> GetAIRAC_Cycle_Data Completed", Environment.NewLine, Date.Now, Session("UserId")))

            loBoeingOptData = loADMS_BAL_Export_Data_For_Boeing_OPT.GetDataForBoeingOptDatabaseExport(Session("UserId"), _
                        IIf(ChkExportInActiveRecords.Checked, "", "1"))
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> GetDataForBoeingOptDatabaseExport Completed", Environment.NewLine, Date.Now, Session("UserId")))
            'ModifyAirportData(loBoeingOptData)
            ModifyRunwayData(loBoeingOptData)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> ModifyRunwayData Completed", Environment.NewLine, Date.Now, Session("UserId")))
            'ModifyObstecleData(loBoeingOptData)
            ModifyIntersectionData(loBoeingOptData)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> ModifyIntersectionData Completed", Environment.NewLine, Date.Now, Session("UserId")))

            ImpersonateUser(gs_DomainAccount, gs_Domain, gs_Pwd)
            loADMS_BAL_Export_Data_For_Boeing_OPT.ExportDataForBoeingOptDatabase(Session("UserId"), _
                        loBoeingOptData, LblExportedOptDatabaseFile.Text, _
                        LblEmptyBoeingOptSqlieDbFile.Text, lsAiracCode)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> ExportDataForBoeingOptDatabase Completed", Environment.NewLine, Date.Now, Session("UserId")))
            UndoImpersonation()

            If Not loBoeingOptData Is Nothing Then
                LblExportDataSummary.Text = "Export completed<Br>" & loBoeingOptData.Tables("AptInfo").Rows.Count & ": Airports<Br>" & _
                                          loBoeingOptData.Tables("RwyInfo").Rows.Count & ": Runways<Br>" & _
                                          loBoeingOptData.Tables("ObstInfo").Rows.Count & ": Obstacles<Br>" & _
                                          loBoeingOptData.Tables("IntersectInfo").Rows.Count & ": Intersection<Br>" & _
                                          "were exported at " & Date.Now
            End If


            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
              "Message", "alert('Successfully exported the data." & "');", True)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> Export Completed", Environment.NewLine, Date.Now, Session("UserId")))
        Catch ex As Exception
            UndoImpersonation()
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while exporting Boeing OPT data. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
            File.AppendAllText(strLogFile, String.Format("{0}{1:dd-MMM-yyyy HH:mm:ss}-{2}   -> Export button click: " & ex.ToString(), Environment.NewLine, Date.Now, Session("UserId")))
        End Try

    End Sub

    Private Sub ModifyAirportData(ByRef PoBoeingOptData As DataSet)

    End Sub

    Private Sub ModifyRunwayData(ByRef PoBoeingOptData As DataSet)

        'update LatStartTORA and LongStartTORA filed
        Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
        Dim liLatDecimalValue As Single
        Dim lbIsValidValue As Boolean = False

        For I = 0 To PoBoeingOptData.Tables("RwyInfo").Rows.Count - 1

            liLatDecimalValue = loADMS_BAL_Data_Checks.GetDecimalDegree(PoBoeingOptData.Tables("RwyInfo").Rows(I)("LatDir"), _
                    PoBoeingOptData.Tables("RwyInfo").Rows(I)("LatDeg"), _
                    PoBoeingOptData.Tables("RwyInfo").Rows(I)("LatMin"), _
                    PoBoeingOptData.Tables("RwyInfo").Rows(I)("LatSec"), lbIsValidValue)

            PoBoeingOptData.Tables("RwyInfo").Rows(I)("LatStartTORA") = liLatDecimalValue

            liLatDecimalValue = loADMS_BAL_Data_Checks.GetDecimalDegree(PoBoeingOptData.Tables("RwyInfo").Rows(I)("LonDir"), _
                   PoBoeingOptData.Tables("RwyInfo").Rows(I)("LonDeg"), _
                   PoBoeingOptData.Tables("RwyInfo").Rows(I)("LonMin"), _
                   PoBoeingOptData.Tables("RwyInfo").Rows(I)("LonSec"), lbIsValidValue)

            PoBoeingOptData.Tables("RwyInfo").Rows(I)("LongStartTORA") = liLatDecimalValue

        Next
        PoBoeingOptData.Tables("RwyInfo").AcceptChanges()
    End Sub

    Private Sub ModifyObstecleData(ByRef PoBoeingOptData As DataSet)

    End Sub

    Private Sub ModifyIntersectionData(ByRef PoBoeingOptData As DataSet)
        'update LatStartTORA and LongStartTORA field
        Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
        Dim liLatDecimalValue As Single
        Dim lbIsValidValue As Boolean = False

        For I = 0 To PoBoeingOptData.Tables("IntersectInfo").Rows.Count - 1

            liLatDecimalValue = loADMS_BAL_Data_Checks.GetDecimalDegree(PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LatDir"), _
                    PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LatDeg"), _
                    PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LatMin"), _
                    PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LatSec"), lbIsValidValue)

            PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LatStartTORA") = liLatDecimalValue

            liLatDecimalValue = loADMS_BAL_Data_Checks.GetDecimalDegree(PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LonDir"), _
                   PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LonDeg"), _
                   PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LonMin"), _
                   PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LonSec"), lbIsValidValue)

            PoBoeingOptData.Tables("IntersectInfo").Rows(I)("LongStartTORA") = liLatDecimalValue


        Next
        PoBoeingOptData.Tables("IntersectInfo").AcceptChanges()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("ADMS_Home.aspx")
    End Sub
End Class