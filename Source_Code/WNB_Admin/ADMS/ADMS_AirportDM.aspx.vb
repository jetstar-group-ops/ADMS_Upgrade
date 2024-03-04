Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports WNB_Admin_BO

'Imports System.IO
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
'Imports iTextSharp.text.html
'Imports iTextSharp.text.html.simpleparser

Partial Public Class ADMS_AirportDM
    Inherits System.Web.UI.Page

    Private goAirports As DataTable

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
            'Checking Readonly Rights
            If Convert.ToString(Session("UserId")).ToUpper() <> Convert.ToString("Admin").ToUpper Then
                If loBO.IsUserHasPermission(Session("UserId"), _
                        WNB_Common.Enums.Functionalities.ADMSReadOnlyRights, "", 0) = True Then
                    ReadOnlyRightsToControls()
                End If
            End If





            If Me.IsPostBack = False Then
                Dim loADMS_BAL_APTCategory As New ADMS_BAL_APTCategory
                Dim loAirportCategories As DataTable

                loAirportCategories = loADMS_BAL_APTCategory.GetCategories(Session("UserId"))
                LstCategory.Items.Clear()

                For I = 0 To loAirportCategories.Rows.Count - 1
                    LstCategory.Items.Add(loAirportCategories.Rows(I)("CatDesc"))
                    LstCategory.Items(LstCategory.Items.Count - 1).Value = loAirportCategories.Rows(I)("CatId")
                Next

                DivAirportDetails.Attributes.Add("style", "display:none;")
                If Request("ICAO") & "" <> "" And Request("ICAO") & "" <> "-1" Then
                    TxtSearchAirportId.Text = Request("ICAO") & ""
                    BtnSelectAirport_Click(sender, e)
                End If

                'TxtCity.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                'TxtCountry.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                'TxtName.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                TxtIcao.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")
                TxtIata.Attributes.Add("OnKeyPress", " return AllowAlphaNumeric(this);")

                TxtElevation.Attributes.Add("OnKeyPress", " return AllowNegativeValue(this);")

                TxtLatDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLatSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")

                TxtLonDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLonMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtLonSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")

                TxtMagDeg.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtMagMin.Attributes.Add("OnKeyPress", " return AllowNumericOnly(this);")
                TxtMagSec.Attributes.Add("OnKeyPress", " return AllowNumericOnlyWithDecimal(this);")

                If Request("ICAO") = "-1" Then
                    ' request to create a new airport
                    BtnCreateAirport_Click(sender, e)
                    BtnSelectAirport.Visible = False
                    TxtSearchAirportId.Visible = False
                    btnDataCheck.Enabled = False
                    btnDelete.Enabled = False
                    BtnClose.Visible = False
                    btnPrint.Enabled = False
                    BtnUpdate.Text = "Add"
                End If

                If Request("GE") = "1" Then
                    btnGoogleEarth_Click(sender, e)
                    BtnSelectAirport.Visible = False
                    TxtSearchAirportId.Visible = False
                End If

                TxtSearchAirportId.Focus()
                BtnClose.Visible = True

                If BtnSelectAirport.Visible = True Then
                    LblTitle.Text = "Select Airport"
                Else
                    LblTitle.Text = "Airport Details"
                End If
                If LblTitle.Text = "Airport Details" Then
                    BtnClose.Visible = False
                End If
            End If


        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                     "Message", "alert('Error while getting airports details. \n" & _
                     ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub ReadOnlyRightsToControls()
        Try
            BtnAddRunway.Enabled = False
            BtnUpdate.Enabled = False
            btnDelete.Enabled = False
            BtnCreateAirport.Enabled = False
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function GetAndShowAirport(ByVal psIcao As String) As Boolean
        Try


            Dim loAirportDetails As DataSet
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM

            loAirportDetails = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), psIcao)

            If loAirportDetails.Tables(0).Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Airport not found! please enter a valid ICAO or IATA code of the airport." & "');", True)
                Return False
            End If

            DgrdAirportRunways.DataSource = loAirportDetails.Tables(1)
            DgrdAirportRunways.DataBind()
            ShowAirportDetails(loAirportDetails.Tables(0))

            Return True
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Sub ShowAirportDetails(ByVal poAirportDetails As DataTable)

        If poAirportDetails.Rows.Count > 0 Then

            TxtIcao.Text = poAirportDetails.Rows(0)("ICAO") & ""
            HdnIcao.Value = TxtIcao.Text
            TxtIcao.Enabled = False

            TxtIata.Text = poAirportDetails.Rows(0)("IATA") & ""
            TxtName.Text = poAirportDetails.Rows(0)("Name") & ""
            TxtCity.Text = poAirportDetails.Rows(0)("City") & ""
            TxtCountry.Text = poAirportDetails.Rows(0)("Country") & ""
            TxtElevation.Text = poAirportDetails.Rows(0)("Elevation") & ""

            LstMagDir.SelectedIndex = -1
            If LstMagDir.Items.FindByValue(poAirportDetails.Rows(0)("MagDir") & "") IsNot Nothing Then
                LstMagDir.Items.FindByValue(poAirportDetails.Rows(0)("MagDir") & "").Selected = True
            End If

            TxtMagDeg.Text = poAirportDetails.Rows(0)("MagDeg") & ""
            TxtMagMin.Text = poAirportDetails.Rows(0)("MagMin") & ""
            TxtMagSec.Text = Math.Round(Convert.ToDecimal(poAirportDetails.Rows(0)("MagSec")), 2) & ""

            LstLatDir.SelectedIndex = -1
            If LstLatDir.Items.FindByValue(poAirportDetails.Rows(0)("LatDir") & "") IsNot Nothing Then
                LstLatDir.Items.FindByValue(poAirportDetails.Rows(0)("LatDir") & "").Selected = True
            End If

            TxtLatDeg.Text = poAirportDetails.Rows(0)("LatDeg") & ""
            TxtLatMin.Text = poAirportDetails.Rows(0)("LatMin") & ""
            TxtLatSec.Text = Math.Round(Convert.ToDecimal(poAirportDetails.Rows(0)("LatSec")), 2) & ""

            LstLonDir.SelectedIndex = -1
            If LstLonDir.Items.FindByValue(poAirportDetails.Rows(0)("LonDir") & "") IsNot Nothing Then
                LstLonDir.Items.FindByValue(poAirportDetails.Rows(0)("LonDir") & "").Selected = True
            End If

            TxtLonDeg.Text = poAirportDetails.Rows(0)("LonDeg") & ""
            TxtLonMin.Text = poAirportDetails.Rows(0)("LonMin") & ""
            TxtLonSec.Text = Math.Round(Convert.ToDecimal(poAirportDetails.Rows(0)("LonSec")), 2) & ""

            If poAirportDetails.Rows(0)("Active") & "" <> "" Then
                ChkActive.Checked = poAirportDetails.Rows(0)("Active") 'IIf(Val(poAirportDetails.Rows(0)("Active") & "") = 1, True, False)
            End If

            LstCategory.SelectedIndex = -1
            If LstCategory.Items.FindByValue(poAirportDetails.Rows(0)("Category") & "") IsNot Nothing Then
                LstCategory.Items.FindByValue(poAirportDetails.Rows(0)("Category") & "").Selected = True
            End If

            LblUpdatedBy.Text = poAirportDetails.Rows(0)("ChangeUser") & ""
            If poAirportDetails.Rows(0)("ChangeDateTime") & "" <> "" Then
                LblUpdatedOn.Text = Date.Parse(poAirportDetails.Rows(0)("ChangeDateTime")).ToString("dd-MM-yyyy HH:mm")
            End If

            TxtComments.Text = poAirportDetails.Rows(0)("Comment") & ""
        End If
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Try
            ' MakeFormInNavigationMode()
            DivAirportDetails.Attributes.Add("style", "display:none;")
            Response.Redirect("ADMS_Home.aspx")
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                 "Message", "alert('Error while cancelling the edit operation.\n" & ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Protected Sub BtnSelectAirport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSelectAirport.Click
        Try
            If TxtSearchAirportId.Text.Trim = "" Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Please enter either ICAO or IATA code of the airport." & "');", True)
                Exit Sub
            End If
            If GetAndShowAirport(TxtSearchAirportId.Text) = True Then
                DivAirportDetails.Attributes.Add("style", "display:inline;")
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                '          "window.setTimeout('open_Airport_Details_box()',500);", True)
                BtnSelectAirport.Visible = False
                BtnClose.Visible = False
                TxtSearchAirportId.Visible = False
                LblTitle.Text = "Airport Details"
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while getting airport details.\n" & ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub MakeFormInEntryMode()

        'PnlAirportDetails.Enabled = True
        'PnlAirportDetails.Visible = True
        'DgrdAirports.Enabled = False
        'BtnCreateAirport.Enabled = False
        'BtnSelectAirport.Enabled = False

    End Sub

    Private Sub MakeFormInNavigationMode()

        'PnlAirportDetails.Enabled = False
        'PnlAirportDetails.Visible = False
        'DgrdAirports.Enabled = True
        'BtnCreateAirport.Enabled = True
        'BtnSelectAirport.Enabled = True

    End Sub

    Private Sub BtnCreateAirport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCreateAirport.Click
        HdnIcao.Value = ""
        ClearFormForNewAirport()
        TxtIcao.Enabled = True
        BtnAddRunway.Enabled = False
        btnGoogleEarth_SingleAirport.Enabled = False
        DgrdAirportRunways.DataSource = Nothing
        DgrdAirportRunways.DataBind()

        DivAirportDetails.Attributes.Add("style", "display:inline;")
    End Sub

    Private Sub ClearFormForNewAirport()

        TxtIcao.Text = ""
        TxtIata.Text = ""
        TxtName.Text = ""
        TxtCity.Text = ""
        TxtCountry.Text = ""
        TxtElevation.Text = ""

        LstMagDir.SelectedIndex = 0

        TxtMagDeg.Text = ""
        TxtMagMin.Text = ""
        TxtMagSec.Text = ""

        LstLatDir.SelectedIndex = 0

        TxtLatDeg.Text = ""
        TxtLatMin.Text = ""
        TxtLatSec.Text = ""

        LstLonDir.SelectedIndex = 0

        TxtLonDeg.Text = ""
        TxtLonMin.Text = ""
        TxtLonSec.Text = ""

        ChkActive.Checked = False

        LstCategory.SelectedIndex = 0

        LblUpdatedBy.Text = ""
        LblUpdatedOn.Text = ""

        TxtComments.Text = ""
    End Sub

    Private Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Try
            If ValidateData() = False Then
                'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
                '       "window.setTimeout('open_Airport_Details_box()',500);", True)
                Exit Sub
            End If

            Dim loAirportDetails As DataSet
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim loMyNewAirportDetails As New DataSet
            Dim liResult As Integer

            loAirportDetails = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), "-1")
            loMyNewAirportDetails.Tables.Add(loAirportDetails.Tables(0).Copy)
            loMyNewAirportDetails.Tables(0).Columns("ChangeDateTime").DataType = System.Type.GetType("System.String")
            loAirportDetails.Dispose()

            loMyNewAirportDetails.Tables(0).Rows.Clear()

            Dim loDataRow As DataRow

            loDataRow = loMyNewAirportDetails.Tables(0).NewRow
            loMyNewAirportDetails.Tables(0).Rows.Add(loDataRow)
            loMyNewAirportDetails.Tables(0).AcceptChanges()

            PolukateAirportData(loMyNewAirportDetails.Tables(0))

            loAirportDetails = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), loMyNewAirportDetails.Tables(0).Rows(0)("ICAO"))
            If Not loAirportDetails Is Nothing Then
                If loAirportDetails.Tables(0).Rows.Count > 0 And TxtIcao.Enabled = True Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('This Airport entry is already exist." & "');", True)
                    Exit Sub
                End If
            End If
            loAirportDetails = Nothing

            liResult = loADMS_BAL_ADM.CreateUpdateAirport(loMyNewAirportDetails, Session("UserId"), IIf(TxtIcao.Enabled = False, TxtIcao.Text, ""))
            If liResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully updated the airport details." & "');", True)

                If Request("ICAO") = "-1" Then
                    Response.Redirect("ADMS_AirportDM.aspx?ICAO=" & TxtIcao.Text, True)
                End If

            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to updated the airport details; please contact support team." & "');", True)
            End If
            BtnSelectAirport_Click(sender, e)
            'DivAirportDetails.Attributes.Add("style", "display:none;")
            'Response.Redirect("ADMS_Home.aspx")
        Catch ex As Exception
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleScript", _
            '            "window.setTimeout('open_Airport_Details_box()',500);", True)
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while updating airport details.\n" & _
                ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Private Sub PolukateAirportData(ByRef poAirportDetails As DataTable)
        poAirportDetails.Rows(0)("ICAO") = TxtIcao.Text
        poAirportDetails.Rows(0)("IATA") = TxtIata.Text
        poAirportDetails.Rows(0)("Name") = TxtName.Text
        poAirportDetails.Rows(0)("City") = TxtCity.Text
        poAirportDetails.Rows(0)("Country") = TxtCountry.Text
        poAirportDetails.Rows(0)("Elevation") = IIf(TxtElevation.Text = "", 0, TxtElevation.Text)

        poAirportDetails.Rows(0)("MagDir") = LstMagDir.SelectedValue
        poAirportDetails.Rows(0)("MagDeg") = TxtMagDeg.Text
        poAirportDetails.Rows(0)("MagMin") = TxtMagMin.Text
        poAirportDetails.Rows(0)("MagSec") = TxtMagSec.Text

        poAirportDetails.Rows(0)("LatDir") = LstLatDir.SelectedValue
        poAirportDetails.Rows(0)("LatDeg") = TxtLatDeg.Text
        poAirportDetails.Rows(0)("LatMin") = TxtLatMin.Text
        poAirportDetails.Rows(0)("LatSec") = TxtLatSec.Text

        poAirportDetails.Rows(0)("LonDir") = LstLonDir.SelectedValue
        poAirportDetails.Rows(0)("LonDeg") = TxtLonDeg.Text
        poAirportDetails.Rows(0)("LonMin") = TxtLonMin.Text
        poAirportDetails.Rows(0)("LonSec") = TxtLonSec.Text

        poAirportDetails.Rows(0)("Active") = IIf(ChkActive.Checked, 1, 0)
        poAirportDetails.Rows(0)("Category") = LstCategory.SelectedValue
        poAirportDetails.Rows(0)("ChangeUser") = Session("UserId")
        poAirportDetails.Rows(0)("ChangeDateTime") = Date.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")

        poAirportDetails.Rows(0)("Comment") = TxtComments.Text

        'LblUpdatedOn.Text = Date.Now.ToString("dd-MM-yyyy HH:mm")
        'LblUpdatedBy.Text = Session("UserId")

    End Sub

    Private Function ValidateData() As Boolean

        Dim lsErrMsg As String = ""
        Dim lbResult As Boolean = True

        If TxtIcao.Text.Trim.Length <> 4 Then
            lsErrMsg = "Please enter 4 charector ICAO code for the airport."
            GoTo ReturnResult
        End If

        If TxtIata.Text.Trim.Length <> 3 Then
            lsErrMsg = "Please enter 3 charector IATA code for the airport."
            GoTo ReturnResult
        End If

        If ChkActive.Checked = True Then

            If TxtName.Text.Trim = "" Then
                lsErrMsg = "Please enter name for the airport."
                GoTo ReturnResult
            End If

            If TxtCity.Text.Trim = "" Then
                lsErrMsg = "Please enter city name for the airport."
                GoTo ReturnResult
            End If

            If TxtCountry.Text.Trim = "" Then
                lsErrMsg = "Please enter country name for the airport."
                GoTo ReturnResult
            End If

            If TxtElevation.Text.Trim = "" Then
                lsErrMsg = "Please enter Elevation value for the airport."
                GoTo ReturnResult
            End If

            'validation of Magnetic values
            If TxtMagDeg.Text.Trim = "" Then
                lsErrMsg = "Please enter Magnetic Variation (Degree) for the airport."
                GoTo ReturnResult
            End If
            If TxtMagMin.Text.Trim = "" Then
                lsErrMsg = "Please enter Magnetic Variation (MIN) for the airport."
                GoTo ReturnResult
            End If
            If TxtMagSec.Text.Trim = "" Then
                lsErrMsg = "Please enter Magnetic Variation (SEC) for the airport."
                GoTo ReturnResult
            End If

            'validation of Latitude values
            If TxtLatDeg.Text.Trim = "" Then
                lsErrMsg = "Please enter Latitude value (Degree) for the airport."
                GoTo ReturnResult
            End If
            If TxtLatMin.Text.Trim = "" Then
                lsErrMsg = "Please enter Latitude value (MIN) for the airport."
                GoTo ReturnResult
            End If
            If TxtLatSec.Text.Trim = "" Then
                lsErrMsg = "Please enter Latitude value (SEC) for the airport."
                GoTo ReturnResult
            End If

            'validation of Longitude values
            If TxtLonDeg.Text.Trim = "" Then
                lsErrMsg = "Please enter Longitude value (Degree) for the airport."
                GoTo ReturnResult
            End If
            If TxtLonMin.Text.Trim = "" Then
                lsErrMsg = "Please enter Longitude value (MIN) for the airport."
                GoTo ReturnResult
            End If
            If TxtLonSec.Text.Trim = "" Then
                lsErrMsg = "Please enter Longitude value (SEC) for the airport."
                GoTo ReturnResult
            End If

        End If

ReturnResult:

        If lsErrMsg <> "" Then
            lbResult = False
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
              "Message", "alert('" & lsErrMsg.Replace("'", "") & "');", True)
        End If

        Return lbResult

    End Function

    Private Sub BtnAddRunway_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAddRunway.Click
        Response.Redirect("ADMS_RunwayDM.aspx?ICAO=" & TxtIcao.Text)
    End Sub

    'Private Sub DgrdAirports_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DgrdAirports.ItemDataBound
    '    If e.Item.ItemIndex = -1 Then Exit Sub

    '    Dim loBtnCreateAirport As System.Web.UI.WebControls.Button

    '    loBtnCreateAirport = e.Item.FindControl("BtnCheckAirport")

    '    loBtnCreateAirport.Attributes.Add("onClick", "return ShowDataCheckReport('" & _
    '                goAirports.Rows(e.Item.ItemIndex)("ICAO") & "');")
    'End Sub

    Protected Sub btnGoogleEarth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGoogleEarthAllAirports.Click
        Try
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim loADMS_BAL_KML As New ADMS_BAL_KML
            Dim lsKMLFileOutputfilePath As String = ""
            Dim lsKMLInputStyles As String = Server.MapPath("~/ADMS/Airport_KML_Styles.txt") 'Request.MapPath("\ADMS\Airport_KML_Styles.txt")
            Dim lsKMLOutputFilePath As String = Server.MapPath("~/Temp/KMLFiles\") 'Request.MapPath("\Temp\KMLFiles\")

            If Not System.IO.Directory.Exists(lsKMLOutputFilePath) Then
                System.IO.Directory.CreateDirectory(lsKMLOutputFilePath)
            End If

            goAirports = loADMS_BAL_ADM.GetAllAirports(Session("UserId") & "", "")
            lsKMLFileOutputfilePath = loADMS_BAL_KML.CreateAllAirports_KMLFile(Session("UserId"), goAirports, lsKMLInputStyles, lsKMLOutputFilePath)

            loADMS_BAL_ADM = Nothing
            loADMS_BAL_KML = Nothing

            Dim file As System.IO.FileInfo = New System.IO.FileInfo(lsKMLFileOutputfilePath)

            If file.Exists Then
                'Response.ClearHeaders()
                'Response.AppendHeader("Content-Encoding", "none;")
                Response.Redirect("../Temp/KMLfiles/" & file.Name, False)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                     "Message", "alert('Error while Generating KML file. Contact to Administrator. \n" & _
                     ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Protected Sub btnGoogleEarth_SingleAirport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGoogleEarth_SingleAirport.Click
        Try
            Dim psIcao As String = HdnIcao.Value
            Dim loAirportDetails As DataSet
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim lsKMLFileOutputfilePath As String = ""
            Dim loADMS_BAL_KML As New ADMS_BAL_KML
            Dim lsKMLOutputFilePath As String = Server.MapPath("~/Temp/KMLFiles/")
            Dim lsKMLInputStyles As String = Server.MapPath("~/ADMS/Airport_KML_Styles.txt")

            If Not System.IO.Directory.Exists(lsKMLOutputFilePath) Then
                System.IO.Directory.CreateDirectory(lsKMLOutputFilePath)
            End If

            loAirportDetails = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), psIcao)
            If loAirportDetails.Tables(0).Rows.Count = 0 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Airport not found! please enter a valid ICAO or IATA code of the airport." & "');", True)
                Exit Sub
            End If

            lsKMLFileOutputfilePath = loADMS_BAL_KML.CreateAirport_KMLFile(Session("UserId"), loAirportDetails.Tables(0), _
                                                                           lsKMLInputStyles, lsKMLOutputFilePath)

            Dim file As System.IO.FileInfo = New System.IO.FileInfo(lsKMLFileOutputfilePath)

            If file.Exists Then
                Response.Redirect("../Temp/KMLfiles/" & file.Name, False)
            End If

            loADMS_BAL_KML = Nothing
            loADMS_BAL_ADM = Nothing
            file = Nothing

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                     "Message", "alert('Error while getting airports details. \n" & _
                     ex.Message.Replace("'", "") & "');", True)

        End Try
    End Sub



    Protected Sub BtnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnClose.Click
        Response.Redirect("ADMS_home.aspx")
    End Sub


    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click

        Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
        Dim liResult As Integer
        Dim loAirportDetails As DataSet
        Try
            'Validation to check runways are present before deletion
            loAirportDetails = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), TxtIcao.Text)
            If Not loAirportDetails Is Nothing Then
                If loAirportDetails.Tables(1).Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                            "Message", "alert('Please delete Runway(s) of this airport first." & "');", True)
                    Exit Sub
                End If
            End If
            loAirportDetails = Nothing
            '-----------------------

            liResult = loADMS_BAL_ADM.DeleteAirport(Session("UserId"), HdnIcao.Value)
            If liResult = 1 Then
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Successfully deleted the airport details." & "');", True)
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Failed to delete airport details; please contact support team." & "');", True)
            End If
            loADMS_BAL_ADM = Nothing
            Response.Redirect("ADMS_Home.aspx")

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                "Message", "alert('Error while deleting airport details. \n" & _
                    ex.Message.Replace("'", "") & "');", True)
            Exit Sub
        End Try

    End Sub

    Protected Sub btnDataCheck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDataCheck.Click
        Try
            Response.Redirect("ADMS_DataCheckReport.aspx?ICAO=" & HdnIcao.Value & "&ModuleId=A", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
               "Message", "alert('Error while checking runway data.\n" & _
      ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Response.Redirect("ADMS_TextReportPrint.aspx?ICAO=" & HdnIcao.Value & "&AirlineCode=" & Session("AirliineCode"))

            ' GenerateReport()
            'Response.Redirect(HdnExportFileName.Value)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
              "Message", "alert('Error while exporting to print the report.\n" & _
     ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub
    Public Sub GenerateReport()
        Try
            Dim cryRpt As New ReportDocument
            Dim myLogonInfo As New TableLogOnInfo()

            Dim DbUserName As String
            Dim DbUserPwd As String
            Dim DbName As String
            Dim DbServerName As String
            'Dim rptPath As String

            'rptPath = Request.MapPath(Request.ApplicationPath)
            cryRpt.Load(Request.MapPath("..\ADMS\TextFileReport.rpt"))
            DbServerName = System.Configuration.ConfigurationManager.AppSettings("DbServerName")
            DbName = System.Configuration.ConfigurationManager.AppSettings("DbName")
            DbUserName = System.Configuration.ConfigurationManager.AppSettings("DbUserName")
            DbUserPwd = System.Configuration.ConfigurationManager.AppSettings("DbUserPwd")

            'cryRpt.SetDatabaseLogon(DbUserName, DbUserPwd)
            For Each myTable In cryRpt.Database.Tables
                myLogonInfo = myTable.LogOnInfo
                myLogonInfo.ConnectionInfo.ServerName = DbServerName
                myLogonInfo.ConnectionInfo.DatabaseName = DbName
                If DbUserName = "" Then
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = True
                Else
                    myLogonInfo.ConnectionInfo.UserID = DbUserName
                    myLogonInfo.ConnectionInfo.Password = DbUserPwd
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = False
                End If
                myTable.ApplyLogOnInfo(myLogonInfo)
            Next myTable

            cryRpt.SetParameterValue("ICAO", TxtIcao.Text)
            cryRpt.SetParameterValue("RwyId", "")
            cryRpt.SetParameterValue("RwyMod", "")
            cryRpt.SetParameterValue("AirlineCode", "")

            'cryRpt.Subreports(0).Load(Request.MapPath("..\ADMS\ObstacleSubReport.rpt"))
            For Each myTable In cryRpt.Subreports(1).Database.Tables
                myLogonInfo = myTable.LogOnInfo
                myLogonInfo.ConnectionInfo.ServerName = DbServerName
                myLogonInfo.ConnectionInfo.DatabaseName = DbName
                If DbUserName = "" Then
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = True
                Else
                    myLogonInfo.ConnectionInfo.UserID = DbUserName
                    myLogonInfo.ConnectionInfo.Password = DbUserPwd
                    myLogonInfo.ConnectionInfo.IntegratedSecurity = False
                End If
                myTable.ApplyLogOnInfo(myLogonInfo)
            Next myTable

            'CRV_TextReportPrint.ReportSource = cryRpt

            Dim loTextFileReport As New TextFileReport

            loTextFileReport.SetParameterValue("ICAO", TxtIcao.Text)
            loTextFileReport.SetParameterValue("RwyId", "")
            loTextFileReport.SetParameterValue("RwyMod", "")
            loTextFileReport.SetParameterValue("AirlineCode", "")

            Dim lsExportFileName As String = ""
            lsExportFileName = Request.MapPath(Request.ApplicationPath) & "\Temp\" & _
                Session("UserId") & "_PrintStd.PDF"

            cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, lsExportFileName)

            HdnExportFileName.Value = "..\Temp\" & _
                Session("UserId") & "_PrintStd.PDF"

            '---------- Testing to open PDF outof explorer--------------
            'Response.ContentType = "application/pdf"
            'Response.AddHeader("content-disposition", "attachment;filename=" & HdnExportFileName.Value & "")
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)

            'Dim sw As New StringWriter()
            'Dim hw As New HtmlTextWriter(sw)

            ''write header and crew basic data
            ''WriteHeaderAndCrewBasicDetails(hw, loCrewBasicData)


            'hw.WriteLine("Emp No: " & "<BR />")
            'hw.WriteLine("<BR />")
            'hw.WriteLine("<Table border='2'>")
            'hw.WriteLine("<TR>")
            'hw.WriteLine("<TD>AJAY")
            'hw.WriteLine("</TD>")
            'hw.WriteLine("</TR>")
            'hw.WriteLine("</Table>")

            'hw.WriteLine("<BR />")

            'Dim sr As New StringReader(sw.ToString())
            'Dim pdfDoc As New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0.0F)
            'Dim htmlparser As New HTMLWorker(pdfDoc)

            'PdfWriter.GetInstance(pdfDoc, Response.OutputStream)

            'pdfDoc.Open()

            'htmlparser.Parse(sr)

            'pdfDoc.Close()
            'Response.Write(pdfDoc)
            'Response.End()


        Catch ex As Exception
            'Throw ex
        End Try
    End Sub



End Class