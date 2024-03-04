Public Partial Class ADMS_Master
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Me.IsPostBack = False Then
        '    CreateMenus()
        'End If
    End Sub

    'Private Sub CreateMenus()

    '    Dim loFileMenu As New MenuItem
    '    Dim loDataMenu As New MenuItem
    '    Dim loSysParameters As New MenuItem
    '    Dim loAptCategory As New MenuItem
    '    Dim loCheckMenu As New MenuItem
    '    Dim loReportMenu As New MenuItem
    '    Dim loExportMenu As New MenuItem
    '    Dim loHelpMenu As New MenuItem
    '    Dim loExitMenu As New MenuItem

    '    Dim loExportDataForBoeingOptDatabase As New MenuItem
    '    Dim loArchiveAirportManager As New MenuItem
    '    Dim loExportAirportManager As New MenuItem
    '    Dim loExportOPTDatabase3pt5 As New MenuItem



    '    loExitMenu.Text = "Exist"
    '    loExitMenu.NavigateUrl = "../Home.aspx"
    '    loFileMenu.Text = "File"
    '    loFileMenu.ChildItems.Add(loExitMenu)
    '    '-----------------------------------------------

    '    Dim loAirportMenu As New MenuItem

    '    loAirportMenu.Text = "Airport"
    '    loAirportMenu.NavigateUrl = "ADMS_AirportDM.aspx"
    '    loDataMenu.Text = "Data"
    '    loDataMenu.ChildItems.Add(loAirportMenu)

    '    loSysParameters.Text = "System Parameters"
    '    loSysParameters.NavigateUrl = "ADMS_ParameterValues.aspx"
    '    loDataMenu.ChildItems.Add(loSysParameters)

    '    loAptCategory.Text = "Airport Category"
    '    loAptCategory.NavigateUrl = "ADMS_APTCategory.aspx"
    '    loDataMenu.ChildItems.Add(loAptCategory)
    '    '---------------------------------------------

    '    Dim loCheckAllAirportsMenu As New MenuItem

    '    loCheckAllAirportsMenu.Text = "Check All Airport"
    '    loCheckAllAirportsMenu.NavigateUrl = "ADMS_AirportDM.aspx"
    '    loCheckMenu.Text = "Check"
    '    loCheckMenu.ChildItems.Add(loCheckAllAirportsMenu)
    '    '--------------------------------------------

    '    Dim loChangedRecords As New MenuItem

    '    loChangedRecords.Text = "Changed Records"
    '    loChangedRecords.NavigateUrl = "ADMS_CRR.aspx"

    '    loReportMenu.Text = "Reports"
    '    loReportMenu.ChildItems.Add(loChangedRecords)
    '    '------------------------------------------------

    '    loExportMenu.Text = "Export"
    '    loHelpMenu.Text = "Help"

    '    '-------------------------------------------------

    '    loArchiveAirportManager.Text = "Archive Master Database"
    '    loArchiveAirportManager.NavigateUrl = ""

    '    loExportAirportManager.Text = "Export Airport Manager"
    '    loExportAirportManager.NavigateUrl = ""

    '    loExportDataForBoeingOptDatabase.Text = "Export OPT Database 4.0"
    '    loExportDataForBoeingOptDatabase.NavigateUrl = "ADMS_Export_Data_For_Boeing_OPT.aspx"

    '    loExportOPTDatabase3pt5.Text = "Export OPT Database 3.5"
    '    loExportOPTDatabase3pt5.NavigateUrl = ""

    '    loExportMenu.ChildItems.Add(loArchiveAirportManager)
    '    loExportMenu.ChildItems.Add(loExportAirportManager)
    '    loExportMenu.ChildItems.Add(loExportDataForBoeingOptDatabase)
    '    loExportMenu.ChildItems.Add(loExportOPTDatabase3pt5)

    '    '-----------------------------------


    '    MnuMaiin.Items.Add(loFileMenu)
    '    MnuMaiin.Items.Add(loDataMenu)
    '    MnuMaiin.Items.Add(loCheckMenu)
    '    MnuMaiin.Items.Add(loReportMenu)
    '    MnuMaiin.Items.Add(loExportMenu)
    '    MnuMaiin.Items.Add(loHelpMenu)



    'End Sub
   
    
   
    
    'Protected Sub LnkBtnLogout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LnkBtnLogout.Click
    '    Session("UserId") = Nothing

    '    Session.Abandon()
    '    'LblUserDetails.Text = ""
    '    Response.Redirect("~/Login.aspx")
    'End Sub
End Class