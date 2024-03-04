Imports WNB_Admin_BO

Partial Public Class ADMS_Home
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
                Request.Browser.Adapters.Clear()
            End If

            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.ADMS, "", 0) = False Then

                lsMessage = "You don't have permission on Airport Database Management System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            If Me.IsPostBack = False Then

                CreateMenus()

                Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
                Dim lsNextAiracCode As String = ""
                Dim lsCurrentAiracCycleStartDate As String = ""
                Dim lsCurrentAiracCycleEndDate As String = ""
                Dim lsNextAiracCycleStartDate As String = ""
                Dim loAirac As ADMS_BAL_Data_Checks.Date_Results

                loADMS_BAL_Data_Checks.GetAIRAC_Cycle_Data(Date.Now.AddDays(27).ToString("dd/MM/yyyy"), _
                        lsNextAiracCode, lsCurrentAiracCycleStartDate, _
                        lsCurrentAiracCycleEndDate, lsNextAiracCycleStartDate)

                LblUserId.Text = Session("UserId") & ""
                LblNextARICCycle.Text = lsNextAiracCode
                'LblCommencing.Text = Date.Parse(lsNextAiracCycleStartDate).ToString("dd/MM/yyyy")
                loAirac = loADMS_BAL_Data_Checks.Get_Cycle_Dates(lsNextAiracCode)
                LblCommencing.Text = loAirac.StartDate.ToString("dd/MM/yyyy")

                'loADMS_BAL_Data_Checks.GetAIRAC_Cycle_Data(Date.Now.ToString("dd/MM/yyyy"), _
                '     lsCurrentAiracCode, lsCurrentAiracCycleStartDate, _
                '     lsCurrentAiracCycleEndDate, lsNextAiracCycleStartDate)

                Session("CurrentAiracCycle") = lsNextAiracCode

                lblVersion.Text = System.Configuration.ConfigurationManager.AppSettings("ADMSVersion")
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Error while getting system details. \n" & _
                    ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub CreateMenus()

        Dim loFileMenu As New MenuItem
        Dim loDataMenu As New MenuItem
        Dim loSysParameters As New MenuItem
        Dim loAptCategory As New MenuItem
        Dim loAptMaxCodes As New MenuItem
        Dim loDBExportCodes As New MenuItem
        Dim loCheckMenu As New MenuItem
        Dim loReportMenu As New MenuItem
        Dim loExportMenu As New MenuItem
        Dim loHelpMenu As New MenuItem
        Dim loLogout As New MenuItem
        'Dim loExitMenu As New MenuItem

        Dim loExportDataForBoeingOptDatabase As New MenuItem
        Dim loArchiveAirportManager As New MenuItem
        Dim loExportAirportManager As New MenuItem
        Dim loExportAirportMax As New MenuItem



        'loExitMenu.Text = "Exist"
        'loExitMenu.NavigateUrl = "../Home.aspx"
        loFileMenu.Text = "File"
        'loFileMenu.ChildItems.Add(loExitMenu)
        '-----------------------------------------------

        Dim loSelectairport As New MenuItem
        Dim loAddAirport As New MenuItem
        Dim loGoogleEarth As New MenuItem

        loDataMenu.Text = "Data"

        loSelectairport.Text = "Select Airport"
        loSelectairport.NavigateUrl = "ADMS_AirportDM.aspx"

        loAddAirport.Text = "Add Airport"
        loAddAirport.NavigateUrl = "ADMS_AirportDM.aspx?ICAO=-1"

        loGoogleEarth.Text = "Show All Airports on Google Earth"
        loGoogleEarth.NavigateUrl = "ADMS_AirportDM.aspx?GE=1"

        loSysParameters.Text = "System Parameters"
        loSysParameters.NavigateUrl = "ADMS_ParameterValues.aspx"

        loDataMenu.ChildItems.Add(loSelectairport)
        loDataMenu.ChildItems.Add(loAddAirport)
        loDataMenu.ChildItems.Add(loGoogleEarth)
        loDataMenu.ChildItems.Add(loSysParameters)

        loAptCategory.Text = "Airport Category"
        loAptCategory.NavigateUrl = "ADMS_APTCategory.aspx"
        loDataMenu.ChildItems.Add(loAptCategory)

        loAptMaxCodes.Text = "Airport Max Codes"
        loAptMaxCodes.NavigateUrl = "ADMS_AptMaxExp.aspx"
        loDataMenu.ChildItems.Add(loAptMaxCodes)

        loDBExportCodes.Text = "Database Export Codes"
        loDBExportCodes.NavigateUrl = "ADMS_DatabaseExp.aspx"
        loDataMenu.ChildItems.Add(loDBExportCodes)
        '---------------------------------------------

        Dim loCheckAllAirportsMenu As New MenuItem

        loCheckAllAirportsMenu.Text = "Check All Airport"
        loCheckAllAirportsMenu.NavigateUrl = "ADMS_DataCheckReport.aspx?ModuleId=A&ICAO="
        loCheckMenu.Text = "Check"
        loCheckMenu.ChildItems.Add(loCheckAllAirportsMenu)
        '--------------------------------------------

        Dim loChangedRecords As New MenuItem

        loChangedRecords.Text = "Changed Records"
        loChangedRecords.NavigateUrl = "ADMS_CRR.aspx"

        loReportMenu.Text = "Reports"
        loReportMenu.ChildItems.Add(loChangedRecords)
        '------------------------------------------------

        loExportMenu.Text = "Export"
        loHelpMenu.Text = "Help"
        loLogout.Text = "Logout"
        loLogout.NavigateUrl = "~/Login.aspx"


        '-------------------------------------------------
        'loExportAirportMax.Text = "Archive Master Database"
        'loExportAirportMax.NavigateUrl = ""

        loArchiveAirportManager.Text = "Update Airport Max"
        loArchiveAirportManager.NavigateUrl = "ADMS_Airport_Max.aspx"

        loExportAirportManager.Text = "Export Airport Manager"
        loExportAirportManager.NavigateUrl = "ADMS_Export_Airport_Manager.aspx"

        loExportDataForBoeingOptDatabase.Text = "Export OPT Database 4.0"
        loExportDataForBoeingOptDatabase.NavigateUrl = "ADMS_Export_Data_For_Boeing_OPT.aspx"


        loExportMenu.ChildItems.Add(loArchiveAirportManager)
        loExportMenu.ChildItems.Add(loExportAirportManager)
        loExportMenu.ChildItems.Add(loExportDataForBoeingOptDatabase)
        'loExportMenu.ChildItems.Add(loExportAirportMax)

        '-----------------------------------


        MnuMaiin.Items.Add(loFileMenu)
        MnuMaiin.Items.Add(loDataMenu)
        MnuMaiin.Items.Add(loCheckMenu)
        MnuMaiin.Items.Add(loReportMenu)
        MnuMaiin.Items.Add(loExportMenu)
        MnuMaiin.Items.Add(loHelpMenu)
        MnuMaiin.Items.Add(loLogout)



    End Sub

    Private Sub BtnSelectAirport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSelectAirport.Click
        Try
            Response.Redirect("ADMS_AirportDM.aspx")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while selecting airport. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub btnShowallAirportsOnGoogleEarth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowallAirportsOnGoogleEarth.Click
        Try
            Response.Redirect("ADMS_AirportDM.aspx?GE=1")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                  "Message", "alert('Error while selecting airport. \n" & _
                  ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    'Private Sub LnkBtnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnLogout.Click
    '    Try
    '        Session("UserId") = Nothing
    '        Session.Abandon()
    '        Response.Redirect("~/Login.aspx")

    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
    '             "Message", "alert('Error while loggin out of ADMS. \n" & _
    '             ex.Message.Replace("'", "") & "');", True)
    '    End Try
    'End Sub
End Class