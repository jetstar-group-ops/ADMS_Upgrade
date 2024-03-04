Imports System.IO
Imports System.Data
Imports System.Configuration
Partial Public Class ViewLoadSheet
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


            btnViewLoadSheet.Attributes.Add("onClick", "return ValidateControls('VIEWLOADSHEET');")
        End If
        ScriptManager.RegisterStartupScript(Me, GetType(System.String), "onload", _
         "SetDivSize();", True)
    End Sub

    Private Sub btnViewLoadSheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewLoadSheet.Click
        If Not IsValidate("VIEWLOADSHEET") Then
            Exit Sub
        End If

        Dim strLogsheetFilePath As String = ConfigurationSettings.AppSettings("LogsheetFilePath")
        Dim strLogsheetFolderStructure As String = ConfigurationSettings.AppSettings("LogsheetFolderStructure")
        Dim strLogsheetDateFolderFormat As String = ConfigurationSettings.AppSettings("LogsheetDateFolderFormat")
        Dim strLogsheetYearFolderFormat As String = ConfigurationSettings.AppSettings("LogsheetYearFolderFormat")
        Dim strLogsheetMonthFolderFormat As String = ConfigurationSettings.AppSettings("LogsheetMonthFolderFormat")
        Dim strDate As String = ""
        Dim strYearFolderName As String = ""
        Dim strMonthFolderName As String = ""
        Dim strDateFolderName As String = ""
        Dim strDirectoryPath As String = ""
        Dim strDestinationFile As String = ""
        Dim strDestinationFolder As String = ""
        Dim strDestinationFolderName As String = ""
        Dim strAppRootPath As String = ""
        Dim dtLoadSheetFiles As New DataTable
        Dim dtmDate As DateTime
        Dim loSpourceDir As IO.DirectoryInfo
        Dim lo_SourceFiles As IO.FileInfo()



        Try

            strAppRootPath = Request.MapPath(Request.ApplicationPath)

            dtmDate = Date.Parse(txtDate.Text.ToString.Trim())
            strYearFolderName = dtmDate.ToString(strLogsheetYearFolderFormat)
            strMonthFolderName = dtmDate.ToString(strLogsheetMonthFolderFormat)
            strDateFolderName = dtmDate.ToString(strLogsheetDateFolderFormat)

            DeleteLoadSheet(strAppRootPath & "\Temp\LoadSheet\")

            If strLogsheetFolderStructure.ToString.Trim.Equals("Year/Month/Date") Then
                strDirectoryPath = strLogsheetFilePath & strYearFolderName & "\" & strMonthFolderName & "\" & strDateFolderName & "\"
                strDestinationFolder = strYearFolderName & "_" & strMonthFolderName & "_" & strDateFolderName & "_"

            ElseIf strLogsheetFolderStructure.ToString.Trim.Equals("Year/Month") Then
                strDirectoryPath = strLogsheetFilePath & strYearFolderName & "\" & strMonthFolderName & "\"
                strDestinationFolder = strYearFolderName & "_" & strMonthFolderName & "_"

            ElseIf strLogsheetFolderStructure.ToString.Trim.Equals("Month/Date") Then
                strDirectoryPath = strLogsheetFilePath & strMonthFolderName & "\" & strDateFolderName & "\"
                strDestinationFolder = strMonthFolderName & "_" & strDateFolderName & "_"

            ElseIf strLogsheetFolderStructure.ToString.Trim.Equals("Year/Date") Then
                strDirectoryPath = strLogsheetFilePath & strYearFolderName & "\" & strDateFolderName & "\"
                strDestinationFolder = strYearFolderName & "_" & strDateFolderName & "_"

            Else
                strDirectoryPath = strLogsheetFilePath & strDateFolderName & "\"
                strDestinationFolder = strDateFolderName & "_"

            End If



            strDestinationFolderName = strDestinationFolder


            

            loSpourceDir = New IO.DirectoryInfo(strDirectoryPath)


            dtLoadSheetFiles.Columns.Add("CreatedDate", Type.GetType("System.String"))
            dtLoadSheetFiles.Columns.Add("WriteTime", Type.GetType("System.String"))
            dtLoadSheetFiles.Columns.Add("FileName", Type.GetType("System.String"))
            dtLoadSheetFiles.Columns.Add("GeneratedFile", Type.GetType("System.String"))
            dtLoadSheetFiles.Columns.Add("GeneratedFileName", Type.GetType("System.String"))

            dtLoadSheetFiles.AcceptChanges()

            If loSpourceDir.Exists Then
                lo_SourceFiles = loSpourceDir.GetFiles("*.*", SearchOption.AllDirectories)

                If lo_SourceFiles.Length > 0 Then



                    strDestinationFolder = strAppRootPath & "\Temp\LoadSheet\" & Session("UserId") & "_" & strDestinationFolderName
                    

                    For Each loSourceFile In lo_SourceFiles

                        strDestinationFile = strDestinationFolder & loSourceFile.Name

                        File.Copy(loSourceFile.FullName, strDestinationFile, True)

                        Dim drLoadSheetRow As DataRow
                        drLoadSheetRow = dtLoadSheetFiles.NewRow
                        drLoadSheetRow("CreatedDate") = loSourceFile.CreationTimeUtc
                        drLoadSheetRow("WriteTime") = loSourceFile.LastWriteTimeUtc
                        drLoadSheetRow("FileName") = loSourceFile.FullName
                        drLoadSheetRow("GeneratedFile") = "..\Temp\LoadSheet\" & Session("UserId") & "_" & strDestinationFolderName & loSourceFile.Name
                        drLoadSheetRow("GeneratedFileName") = loSourceFile.Name
                        dtLoadSheetFiles.Rows.Add(drLoadSheetRow)
                        dtLoadSheetFiles.AcceptChanges()

                    Next

                    gvViewLoadSheet.DataSource = dtLoadSheetFiles
                    gvViewLoadSheet.DataBind()
                Else
                    gvViewLoadSheet.DataSource = dtLoadSheetFiles
                    gvViewLoadSheet.DataBind()

                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                        "alert('Load Sheet not available for selected date.');", True)
                End If
            Else
                gvViewLoadSheet.DataSource = dtLoadSheetFiles
                gvViewLoadSheet.DataBind()

                ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", _
                    "alert('Load Sheet not available for selected date.');", True)
            End If



        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert(' " & ex.Message.ToString().Replace("'", "") & ".');", True)
        End Try
    End Sub



    Private Sub DeleteLoadSheet(ByVal strDirectoryName As String)
        Dim loDirectory As IO.DirectoryInfo
        Dim loFiles As IO.FileInfo()

        Try
            loDirectory = New IO.DirectoryInfo(strDirectoryName)
            loFiles = loDirectory.GetFiles()

            If loFiles.Length > 0 Then
                For Each loFile In loFiles
                    If loFile.Name.StartsWith(Session("UserId") & "_") Then
                        loFile.Delete()
                    End If
                Next
            End If


        Catch ex As Exception
            Throw ex
        Finally
            loDirectory = Nothing
            loFiles = Nothing
        End Try
    End Sub
    Private Sub DeleteDirectoryInfo(ByVal strDirectoryName As String)

        Dim loDirectory As IO.DirectoryInfo
        Dim loSubDirectorys As IO.DirectoryInfo()
        Dim loFiles As IO.FileInfo()

        Try
            loDirectory = New IO.DirectoryInfo(strDirectoryName)
            If loDirectory.Exists Then

                loSubDirectorys = loDirectory.GetDirectories()

                If loSubDirectorys.Length > 0 Then
                    For Each loSubDirectory In loSubDirectorys
                        DeleteDirectoryInfo(loSubDirectory.FullName)
                    Next
                    loDirectory.Delete()
                Else
                    loFiles = loDirectory.GetFiles()
                    If loFiles.Length > 0 Then
                        For Each loFile In loFiles
                            loFile.Delete()
                        Next
                    End If
                    loDirectory.Delete()
                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try
        
    End Sub

    Private Function IsValidate(ByVal strCmd As String) As Boolean
        Dim bReturn As Boolean = True
        Dim strErrorMessage As New Text.StringBuilder

        Try
            If Not strCmd Is Nothing AndAlso (String.Equals(strCmd, "VIEWLOADSHEET")) Then

                If Not txtDate.Text Is Nothing AndAlso String.Equals(txtDate.Text.Trim(), String.Empty) Then
                    strErrorMessage.Append("\n - Required Load Sheet Date.")
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
End Class