Imports System.Text
Imports System.IO

Public Class ADMS_BAL_KML

    Public Function CreateAllAirports_KMLFile(ByVal psUserId As String, ByVal poAirportsTable As DataTable, _
                                    ByVal psStyleFilePath As String, ByVal psKMLOutputFilePath As String) As String
        Try
            Dim sb As New StringBuilder
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim loADMS_BAL_APTCategory As New ADMS_BAL_APTCategory
            Dim dtAptCategrories, dtAptFilteredCategory As DataTable

            dtAptCategrories = loADMS_BAL_APTCategory.GetCategories(psUserId)

            sb.Append("<?xml version=" & ControlChars.Quote & "1.0" & ControlChars.Quote & " encoding=" & ControlChars.Quote & _
                         "UTF-8" & ControlChars.Quote & "?>" & ControlChars.NewLine & "")
            sb.Append("<kml xmlns=" & ControlChars.Quote & "http://earth.google.com/kml/2.0" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
            sb.Append("<Document xmlns:xlink=" & ControlChars.Quote & "http://www.w3/org/1999/xlink" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
            sb.Append("<name>All_Ports</name>" & ControlChars.NewLine & "")

            sb.Append("<LookAt>" & ControlChars.NewLine & "")
            sb.Append("<longitude>144.8433</longitude>" & ControlChars.NewLine & "")
            sb.Append("<latitude>-37.6733</latitude>" & ControlChars.NewLine & "")
            sb.Append("<range>538663.00000000000000000</range>" & ControlChars.NewLine & "")
            sb.Append("<tilt>0.00000000000000000</tilt>" & ControlChars.NewLine & "")
            sb.Append("<heading>0.00000000000000000</heading>" & ControlChars.NewLine & "")
            sb.Append("</LookAt>" & ControlChars.NewLine & "")

            sb.Append(getAirportStyle(psStyleFilePath))
            'sb.Append(getAllAirportStyle())

            For I = 0 To poAirportsTable.Rows.Count - 1

                dtAptFilteredCategory = New DataTable
                sb.Append("<Placemark>" & ControlChars.NewLine & "")
                sb.Append("<name>" & poAirportsTable.Rows(I)("ICAO") & "</name>" & ControlChars.NewLine & "")

                dtAptCategrories.DefaultView.RowFilter = "CatId='" & poAirportsTable.Rows(I)("Category") & "'"
                dtAptFilteredCategory = dtAptCategrories.DefaultView.ToTable()

                sb.Append("<description>" & poAirportsTable.Rows(I)("IATA") & ControlChars.NewLine & _
                            poAirportsTable.Rows(I)("Name") & ControlChars.NewLine & _
                            poAirportsTable.Rows(I)("City") & ControlChars.NewLine & _
                            poAirportsTable.Rows(I)("Country") & ControlChars.NewLine & _
                            "Elevation: " & poAirportsTable.Rows(I)("Elevation") & " ft" & ControlChars.NewLine & _
                           IIf(dtAptFilteredCategory.Rows.Count > 0, dtAptFilteredCategory.Rows(0)("CatDesc"), "") & _
                         "</description>" & ControlChars.NewLine & "")
                sb.Append("<styleUrl>#Port-" & poAirportsTable.Rows(I)("Category") & "</styleUrl>" & ControlChars.NewLine & "")
                sb.Append("<Point>" & ControlChars.NewLine & "")
                sb.Append("<coordinates> " & loADMS_BAL_Data_Checks.Coord_StringToReal(poAirportsTable.Rows(I)("LonDir"), poAirportsTable.Rows(I)("LonDeg"), _
                                                                                       poAirportsTable.Rows(I)("LonMin"), poAirportsTable.Rows(I)("LonSec")) & _
                         "," & loADMS_BAL_Data_Checks.Coord_StringToReal(poAirportsTable.Rows(I)("LatDir"), poAirportsTable.Rows(I)("LatDeg"), _
                                                                                       poAirportsTable.Rows(I)("LatMin"), poAirportsTable.Rows(I)("LatSec")))
                sb.Append("</coordinates>" & ControlChars.NewLine & "")
                sb.Append("</Point>" & ControlChars.NewLine & "")
                sb.Append("</Placemark>" & ControlChars.NewLine & "")

                dtAptFilteredCategory = Nothing
            Next
            sb.Append("</Document>" & ControlChars.NewLine & "")
            sb.Append("</kml>" & ControlChars.NewLine & "")

            Dim FILE_PATH As String = psKMLOutputFilePath & "AllAirports_" & psUserId & "_" & Date.Now.ToString("MM_dd_yyyy_HHmmss") & ".KML"
            'Dim FILE_PATH As String = "AllAirports_" & psUserId & "_" & Date.Now.ToString("MM_dd_yyyy_HHmmss") & ".KML"

            generateKMLFile(FILE_PATH, sb)

            loADMS_BAL_Data_Checks = Nothing
            loADMS_BAL_APTCategory = Nothing

            Return FILE_PATH

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getAirportStyle(ByVal lpStylePath As String) As String
        Try
            Dim lsStyle As String

            lsStyle = File.ReadAllText(lpStylePath).ToString()

            Return lsStyle

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CreateAirport_KMLFile(ByVal psUserId As String, ByVal poAirportTable As DataTable, _
                                  ByVal psStyleFilePath As String, ByVal psKMLOutputFilePath As String) As String
        Try
            Dim sb As New StringBuilder
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim ldsRunway As DataSet
            Dim ldtRecipRwy As DataTable
            Dim liTora As Integer
            Dim liResaLd As Integer
            Dim liDeltaFieldLenth As Integer
            Dim liDisplayTora As Integer
            Dim lsRecipRwyId As String = ""
            Dim lbRunwayActive As Boolean
            Dim lsngRecipLat, lsngRecipLon, lsngLat, lsngLon As Single
            Dim lbValidLat As Boolean
            Dim lbValidLon As Boolean

            'Landing Thresholds Variables
            Dim lbRunwayHasReciprocalRunway As Boolean
            Dim lsReciprocalRunwayId As String
            Dim loReciprocalRunwayTable As DataTable
            Dim LatNumA, LonNumA, LatRecipRwyNumB, LonRecipRwyNumB, Height As Single
            Dim lbValidLatNumA, lbValidLonNumA, lbValidLatNumB, lbValidLonNumB As Boolean
            Dim Answers_AtoB As ADMS_BAL_Data_Checks.AtoB_Results
            'Dim lsngLatLandThreshold, lsngLonLandThreshold As Single
            Dim LandingThreshold(0, 1) As Decimal

            Dim loADMS_BAL_APTCategory As New ADMS_BAL_APTCategory
            Dim dtAptCategrories, dtAptFilteredCategory As DataTable


            'creation of kml document
            sb.Append("<?xml version=" & ControlChars.Quote & "1.0" & ControlChars.Quote & " encoding=" & ControlChars.Quote & _
                         "UTF-8" & ControlChars.Quote & "?>" & ControlChars.NewLine & "")
            sb.Append("<kml xmlns=" & ControlChars.Quote & "http://earth.google.com/kml/2.0" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
            sb.Append("<Document xmlns:xlink=" & ControlChars.Quote & "http://www.w3/org/1999/xlink" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
            'creation of look at
            sb.Append("<LookAt>" & ControlChars.NewLine & "")
            sb.Append("<longitude>" & loADMS_BAL_Data_Checks.Coord_StringToReal(poAirportTable.Rows(0)("LonDir"), _
                                                                                poAirportTable.Rows(0)("LonDeg"), _
                                                                                poAirportTable.Rows(0)("LonMin"), _
                                                                                poAirportTable.Rows(0)("LonSec")) & _
                                                                                "</longitude>" & ControlChars.NewLine & "")


            sb.Append("<latitude>" & loADMS_BAL_Data_Checks.Coord_StringToReal(poAirportTable.Rows(0)("LatDir"), _
                                                                                poAirportTable.Rows(0)("LatDeg"), _
                                                                                poAirportTable.Rows(0)("LatMin"), _
                                                                                poAirportTable.Rows(0)("LatSec")) & _
                                                                                "</latitude>" & ControlChars.NewLine & "")

            sb.Append("<range>3700.00000000000000000</range>" & ControlChars.NewLine & "")
            sb.Append("<tilt>0.00000000000000000</tilt>" & ControlChars.NewLine & "")
            sb.Append("<heading>0.00000000000000000</heading>" & ControlChars.NewLine & "")
            sb.Append("</LookAt>" & ControlChars.NewLine & "")

            sb.Append(getAirportStyle(psStyleFilePath))

            'Airport Details

            dtAptCategrories = loADMS_BAL_APTCategory.GetCategories(psUserId)

            dtAptCategrories.DefaultView.RowFilter = "CatId='" & poAirportTable.Rows(0)("Category") & "'"
            dtAptFilteredCategory = dtAptCategrories.DefaultView.ToTable()


            sb.Append("<Placemark>" & ControlChars.NewLine & "")
            sb.Append("<name>" & poAirportTable.Rows(0)("ICAO") & "  " & poAirportTable.Rows(0)("Elevation") & " ft")
            sb.Append("</name>" & ControlChars.NewLine & "")
            sb.Append("<description>" & poAirportTable.Rows(0)("IATA") & "" & ControlChars.NewLine & _
                                        poAirportTable.Rows(0)("Name") & "" & ControlChars.NewLine & _
                                        poAirportTable.Rows(0)("City") & "" & ControlChars.NewLine & _
                                        poAirportTable.Rows(0)("Country") & "" & ControlChars.NewLine & _
                                        poAirportTable.Rows(0)("Elevation") & " ft" & ControlChars.NewLine & _
                                        IIf(dtAptFilteredCategory.Rows.Count > 0, dtAptFilteredCategory.Rows(0)("CatDesc"), ""))
            sb.Append("</description>" & ControlChars.NewLine & "")
            sb.Append("<styleUrl>#ARP</styleUrl>" & ControlChars.NewLine & "")
            sb.Append("<Point>" & ControlChars.NewLine & "<coordinates>" & _
                       loADMS_BAL_Data_Checks.Coord_StringToReal(poAirportTable.Rows(0)("LonDir"), _
                                                                                poAirportTable.Rows(0)("LonDeg"), _
                                                                                poAirportTable.Rows(0)("LonMin"), _
                                                                                poAirportTable.Rows(0)("LonSec")) & "," & _
                       loADMS_BAL_Data_Checks.Coord_StringToReal(poAirportTable.Rows(0)("LatDir"), _
                                                                                poAirportTable.Rows(0)("LatDeg"), _
                                                                                poAirportTable.Rows(0)("LatMin"), _
                                                                                poAirportTable.Rows(0)("LatSec")))
            sb.Append("</coordinates></Point>" & ControlChars.NewLine & _
           "</Placemark>" & ControlChars.NewLine & "")

            ldsRunway = loADMS_BAL_RDM.GetRunWay(psUserId, poAirportTable.Rows(0)("ICAO"), "", "")

            'Linestring between two end points
            Dim query = From q In (From p In ldsRunway.Tables(0).AsEnumerable() Select New With {.RwyId = p("RwyId")}) Select q.RwyId Distinct
            'Dim DistinctRwyId() As String = query.ToArray()
            Dim DistinctRwyIdLst As New ArrayList(query.ToArray())
            ldtRecipRwy = ldsRunway.Tables(0)
Runways:
            For X = 0 To DistinctRwyIdLst.Count - 1
                Dim lsRnwyId As String = DistinctRwyIdLst(X)
                lsRecipRwyId = loADMS_BAL_Data_Checks.GetRecipocalRunway(lsRnwyId)
                If lsRecipRwyId <> "" Then
                    sb.Append("<Placemark>" & ControlChars.NewLine & "")
                    sb.Append("<name>Runway</name>")
                    sb.Append("<description>" & lsRnwyId & "/" & lsRecipRwyId & "</description>")

                    Dim strRwyDetails = From ldtRecipRwyDetail In ldtRecipRwy _
                                        Select lsRwyId = ldtRecipRwyDetail("RwyId"), _
                                               lsLatDir = ldtRecipRwyDetail("LatDir"), _
                                               lsLatDeg = ldtRecipRwyDetail("LatDeg"), _
                                               lsLatMin = ldtRecipRwyDetail("LatMin"), _
                                               lsLatSec = ldtRecipRwyDetail("LatSec"), _
                                               lsLonDir = ldtRecipRwyDetail("LonDir"), _
                                               lsLonDeg = ldtRecipRwyDetail("LonDeg"), _
                                               lsLonMin = ldtRecipRwyDetail("LonMin"), _
                                               lsLonSec = ldtRecipRwyDetail("LonSec"), _
                                               lsActive = ldtRecipRwyDetail("Active")

                    Dim loRwyResultRow = From RecipRwyRow In strRwyDetails _
                                      Where RecipRwyRow.lsRwyId = lsRecipRwyId

                    Dim loRecipRwyResultRow = From RecipRwyRow In strRwyDetails _
                                      Where RecipRwyRow.lsRwyId = lsRnwyId

                    For Each loRwy In loRwyResultRow
                        lsngLat = loADMS_BAL_Data_Checks.GetDecimalDegree(loRwy.lsLatDir, loRwy.lsLatDeg, loRwy.lsLatMin, loRwy.lsLatSec, lbValidLat)
                        lsngLon = loADMS_BAL_Data_Checks.GetDecimalDegree(loRwy.lsLonDir, loRwy.lsLonDeg, loRwy.lsLonMin, loRwy.lsLonSec, lbValidLon)
                        lbRunwayActive = Convert.ToBoolean(loRwy.lsActive)
                    Next

                    For Each loRR In loRecipRwyResultRow
                        lsngRecipLat = loADMS_BAL_Data_Checks.GetDecimalDegree(loRR.lsLatDir, loRR.lsLatDeg, loRR.lsLatMin, loRR.lsLatSec, lbValidLat)
                        lsngRecipLon = loADMS_BAL_Data_Checks.GetDecimalDegree(loRR.lsLonDir, loRR.lsLonDeg, loRR.lsLonMin, loRR.lsLonSec, lbValidLon)
                    Next

                    If lbRunwayActive = True Then
                        sb.Append("<styleUrl>#RunwayRed</styleUrl>" & ControlChars.NewLine & "")
                    Else
                        sb.Append("<styleUrl>#RunwayOrange</styleUrl>" & ControlChars.NewLine & "")
                    End If


                    sb.Append("<LineString>" & ControlChars.NewLine & "<coordinates>")
                    sb.Append(lsngLon & "," & lsngLat & "," & "0" & ControlChars.NewLine)
                    sb.Append(lsngRecipLon & "," & lsngRecipLat & "," & "0" & ControlChars.NewLine)

                    sb.Append("</coordinates>" & ControlChars.NewLine & _
                     "</LineString>" & ControlChars.NewLine & _
                     "</Placemark>" & ControlChars.NewLine & "")

                    DistinctRwyIdLst.Remove(lsRnwyId)
                    DistinctRwyIdLst.Remove(lsRecipRwyId)

                    If DistinctRwyIdLst.Count >= 0 Then
                        GoTo Runways
                    End If
                End If
            Next

            'Runway End Points
            If Not ldsRunway Is Nothing Then
                If ldsRunway.Tables(0).Rows.Count > 0 Then
                    For I = 0 To ldsRunway.Tables(0).Rows.Count - 1
                        sb.Append("<Placemark>")
                        sb.Append(" <name>" & ldsRunway.Tables(0).Rows(I)("RwyId") & _
                                  " " & ldsRunway.Tables(0).Rows(I)("ElevStartTORA") & " ft" & "</name>")

                        sb.Append("<description>" & ControlChars.NewLine & "")
                        sb.Append("<table width=" & ControlChars.Quote & "300" & ControlChars.Quote & ">")
                        sb.Append("<tr><td>Rwy " & ldsRunway.Tables(0).Rows(I)("RwyId") & "</td><td> " & _
                                  ldsRunway.Tables(0).Rows(I)("RwyMod") & "</td></tr>")
                        sb.Append("<tr><td>TORA</td><td>TODA</td><td>ASDA</td><td>LDA</td></tr>")
                        sb.Append("<tr><td>" & ldsRunway.Tables(0).Rows(I)("TORA") & _
                                  " m</td><td>" & ldsRunway.Tables(0).Rows(I)("TODA") & _
                                  " m</td><td>" & ldsRunway.Tables(0).Rows(I)("ASDA") & _
                                  " m</td><td>" & ldsRunway.Tables(0).Rows(I)("LDA") & " m</td></tr>")

                        sb.Append("<tr><td>Width</td>")
                        sb.Append("<td>" & ldsRunway.Tables(0).Rows(I)("Width") & " m</td>")
                        sb.Append("<td>Entry</td>")
                        sb.Append("<td>" & ldsRunway.Tables(0).Rows(I)("LineUpAngle") & "</td></tr>")

                        sb.Append("<tr><td>Slope</td>")
                        sb.Append("<td>" & Math.Round(ldsRunway.Tables(0).Rows(I)("Slope"), 2) & "</td>")
                        sb.Append("<td>Up</td>")
                        sb.Append("<td></td></tr>")

                        sb.Append("<tr><td>MagDir</td>")
                        sb.Append("<td>" & ldsRunway.Tables(0).Rows(I)("MagHdg") & "</td></tr>")
                        sb.Append("</table><p></p><p></p></description><styleUrl>#RwyEnd</styleUrl>")
                        sb.Append("<Point>")
                        sb.Append("<coordinates> " & loADMS_BAL_Data_Checks.Coord_StringToReal(ldsRunway.Tables(0).Rows(I)("LonDir"), _
                                                                                               ldsRunway.Tables(0).Rows(I)("LonDeg"), _
                                                                                               ldsRunway.Tables(0).Rows(I)("LonMin"), _
                                                                                               ldsRunway.Tables(0).Rows(I)("LonSec")) & _
                                               "," & loADMS_BAL_Data_Checks.Coord_StringToReal(ldsRunway.Tables(0).Rows(I)("LatDir"), _
                                                                                               ldsRunway.Tables(0).Rows(I)("LatDeg"), _
                                                                                               ldsRunway.Tables(0).Rows(I)("LatMin"), _
                                                                                               ldsRunway.Tables(0).Rows(I)("LatSec")))
                        sb.Append("</coordinates>" & ControlChars.NewLine & "")
                        sb.Append("</Point>" & ControlChars.NewLine & "")
                        sb.Append("</Placemark>" & ControlChars.NewLine & "")

                        'Start: Landing Thresholds Calculation---------------------
                        lbRunwayHasReciprocalRunway = False
                        lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(ldsRunway.Tables(0).Rows(I)("RwyId"))

                        loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, ldsRunway.Tables(0).Rows(I)("ICAO"), _
                            lsReciprocalRunwayId, ldsRunway.Tables(0).Rows(I)("RwyMod")).Tables(0).Copy
                        If loReciprocalRunwayTable.Rows.Count > 0 Then
                            lbRunwayHasReciprocalRunway = True
                        End If

                        If Convert.ToInt32(ldsRunway.Tables(0).Rows(I)("DispThr")) > 0 And _
                                        lbRunwayHasReciprocalRunway = True Then

                            LatNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(ldsRunway.Tables(0).Rows(I)("LatDir"), _
                                      ldsRunway.Tables(0).Rows(I)("LatDeg"), ldsRunway.Tables(0).Rows(I)("LatMin"), _
                                      ldsRunway.Tables(0).Rows(I)("LatSec"), lbValidLatNumA)

                            LonNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(ldsRunway.Tables(0).Rows(I)("LonDir"), _
                                    ldsRunway.Tables(0).Rows(I)("LonDeg"), ldsRunway.Tables(0).Rows(I)("LonMin"), _
                                   ldsRunway.Tables(0).Rows(I)("LonSec"), lbValidLonNumA)


                            LatRecipRwyNumB = loADMS_BAL_Data_Checks.GetDecimalDegree(loReciprocalRunwayTable.Rows(0)("LatDir"), _
                                   loReciprocalRunwayTable.Rows(0)("LatDeg"), loReciprocalRunwayTable.Rows(0)("LatMin"), _
                                   loReciprocalRunwayTable.Rows(0)("LatSec"), lbValidLatNumB)

                            LonRecipRwyNumB = loADMS_BAL_Data_Checks.GetDecimalDegree(loReciprocalRunwayTable.Rows(0)("LonDir"), _
                                    loReciprocalRunwayTable.Rows(0)("LonDeg"), loReciprocalRunwayTable.Rows(0)("LonMin"), _
                                    loReciprocalRunwayTable.Rows(0)("LonSec"), lbValidLonNumB)

                            Height = 0

                            If lbValidLatNumA = True And lbValidLonNumA = True And lbValidLatNumB = True And lbValidLonNumB = True Then
                                Answers_AtoB = loADMS_BAL_Data_Checks.Great_CircleAtoB(LatNumA, LonNumA, LatRecipRwyNumB, LonRecipRwyNumB, Height)

                                'Answers_BfromA = loADMS_BAL_Data_Checks.Great_CircleBfromA(LatNumA, LonNumA, Answers_AtoB.TrackA, _
                                '                               Convert.ToInt32(ldsRunway.Tables(0).Rows(I)("DispThr")), Height)

                                'Answers_BfromA = loADMS_BAL_Data_Checks.Great_CircleBfromA(LatNumA, LonNumA, Answers_AtoB.TrackA, _
                                '                                  Answers_AtoB.Rho, Height)
                                LandingThreshold = loADMS_BAL_Data_Checks.ReturnCoords(LatNumA, LonNumA, Answers_AtoB.TrackA, _
                                                            Convert.ToInt32(ldsRunway.Tables(0).Rows(I)("DispThr")), Height)


                                sb.Append("<Placemark>" & ControlChars.NewLine & "")
                                sb.Append("<name>" & ldsRunway.Tables(0).Rows(I)("RwyId") & _
                                          " Thr</name>" & ControlChars.NewLine & "")
                                sb.Append("<description>" & ControlChars.NewLine & _
                                          "<table width=" & ControlChars.Quote & "200" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
                                sb.Append("<tr><td>Rwy " & ldsRunway.Tables(0).Rows(I)("RwyId") & "</td>" & ControlChars.NewLine & "")
                                sb.Append("<td>" & ldsRunway.Tables(0).Rows(I)("RwyMod") & "</td>" & ControlChars.NewLine & "</tr>")
                                sb.Append("<tr>" & ControlChars.NewLine & "<td>LDA</td>" & ControlChars.NewLine & "")
                                sb.Append("<td>" & ldsRunway.Tables(0).Rows(I)("LDA") & " m</td>" & ControlChars.NewLine & "</tr>")
                                sb.Append("<tr>" & ControlChars.NewLine & "<td>Disp</td>" & ControlChars.NewLine & "")
                                sb.Append("<td>" & ldsRunway.Tables(0).Rows(I)("DispThr") & _
                                          " m</td>" & ControlChars.NewLine & "</tr>" & _
                                          ControlChars.NewLine & "</table>" & ControlChars.NewLine & "")

                                sb.Append("<p></p>" & ControlChars.NewLine & _
                                          "</description>" & ControlChars.NewLine & _
                                          "<styleUrl>#DispThr</styleUrl>" & ControlChars.NewLine & "")
                                sb.Append("<Point>" & ControlChars.NewLine & _
                                          "<coordinates>" & ControlChars.NewLine & _
                                          "" & LandingThreshold(0, 0) & "," & LandingThreshold(0, 1) & "")
                                sb.Append("</coordinates>" & ControlChars.NewLine & "</Point>")
                                sb.Append("</Placemark>" & ControlChars.NewLine & "")
                                'Start: Landing Thresholds Calculation---------------------
                            End If
                        End If
                    Next
                End If
            End If




            'Runway Inetersections
            If Not ldsRunway Is Nothing Then
                If ldsRunway.Tables(1).Rows.Count > 0 Then
                    For J = 0 To ldsRunway.Tables(1).Rows.Count - 1
                        sb.Append("<Placemark>" & ControlChars.NewLine & "")
                        sb.Append("<name>" & ldsRunway.Tables(1).Rows(J)("Ident") & "  " & ldsRunway.Tables(1).Rows(J)("ElevStartTORA") & " ft")
                        sb.Append("</name>" & ControlChars.NewLine & "")
                        sb.Append("<description>" & ControlChars.NewLine & _
                                  "<table width=" & ControlChars.Quote & "200" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
                        sb.Append("<tr><td>" & ldsRunway.Tables(1).Rows(J)("Ident") & "</td>" & ControlChars.NewLine & "")
                        sb.Append("<td>Rwy " & ldsRunway.Tables(1).Rows(J)("RwyId") & "</td>" & ControlChars.NewLine & "")
                        sb.Append("<td>" & ldsRunway.Tables(1).Rows(J)("RwyMod") & "</td>" & ControlChars.NewLine & "</tr>")
                        sb.Append("<tr><td>TORA</td>" & ControlChars.NewLine & "")

                        liTora = Convert.ToInt32(ldsRunway.Tables(1).Rows(J)("TORA"))
                        liResaLd = Convert.ToInt32(ldsRunway.Tables(1).Rows(J)("ResaLd"))
                        liDeltaFieldLenth = Convert.ToInt32(ldsRunway.Tables(1).Rows(J)("DeltaFieldLength"))

                        liDisplayTora = liTora - liResaLd - liDeltaFieldLenth

                        sb.Append("<td>" & liDisplayTora & " m</td></tr>" & ControlChars.NewLine & "")
                        sb.Append("<tr><td>Disp</td>" & ControlChars.NewLine & "")
                        sb.Append("<td>" & ldsRunway.Tables(1).Rows(J)("DeltaFieldLength") & " m</td></tr>" & ControlChars.NewLine & "")
                        sb.Append("<tr><td>Entry</td>" & ControlChars.NewLine & "")
                        sb.Append("<td>" & ldsRunway.Tables(1).Rows(J)("LineUpAngle") & "</td></tr></table>" & ControlChars.NewLine & "")
                        sb.Append("<p>" & ldsRunway.Tables(1).Rows(J)("Comment") & "</p></description>" & ControlChars.NewLine & "")

                        sb.Append("<styleUrl>#Intersection</styleUrl>" & ControlChars.NewLine & "")
                        sb.Append("<Point>" & ControlChars.NewLine & "")

                        sb.Append("<coordinates> " & loADMS_BAL_Data_Checks.Coord_StringToReal(ldsRunway.Tables(1).Rows(J)("LonDir"), _
                                                                                                  ldsRunway.Tables(1).Rows(J)("LonDeg"), _
                                                                                                  ldsRunway.Tables(1).Rows(J)("LonMin"), _
                                                                                                  ldsRunway.Tables(1).Rows(J)("LonSec")) & _
                                                  "," & loADMS_BAL_Data_Checks.Coord_StringToReal(ldsRunway.Tables(1).Rows(J)("LatDir"), _
                                                                                                  ldsRunway.Tables(1).Rows(J)("LatDeg"), _
                                                                                                  ldsRunway.Tables(1).Rows(J)("LatMin"), _
                                                                                                  ldsRunway.Tables(1).Rows(J)("LatSec")))
                        sb.Append("</coordinates>" & ControlChars.NewLine & "</Point>" & ControlChars.NewLine & "")
                        sb.Append("</Placemark>" & ControlChars.NewLine & "")
                    Next
                End If
            End If



            sb.Append("</Document></kml>")

            Dim FILE_PATH As String = psKMLOutputFilePath & "Airport_" & poAirportTable.Rows(0)("IATA") & _
                        "_" & psUserId & "_" & Date.Now.ToString("MM_dd_yyyy_HHmmss") & ".KML"
            generateKMLFile(FILE_PATH, sb)

            dtAptFilteredCategory = Nothing
            loADMS_BAL_APTCategory = Nothing
            loADMS_BAL_Data_Checks = Nothing
            loADMS_BAL_RDM = Nothing

            Return FILE_PATH
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CreateObstacleSplay_KMLFile(ByVal psUserId As String, ByVal poRunway As DataSet, _
                               ByVal psStyleFilePath As String, ByVal psKMLOutputFilePath As String) As String
        Try
            Dim sb As New StringBuilder
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim LatNumA, LonNumA, LatRecipRwyNumB, LonRecipRwyNumB, Height As Single
            Dim lbValidLatNumA, lbValidLonNumA, lbValidLatNumB, lbValidLonNumB As Boolean
            Dim SplayPoints(,) As Decimal
            Dim ObstaclePoints(,) As Decimal
            Dim lbRunwayHasReciprocalRunway As Boolean
            Dim loReciprocalRunwayTable As DataTable
            Dim lsReciprocalRunwayId As String
            Dim Answers_AtoB As ADMS_BAL_Data_Checks.AtoB_Results
            Dim liDispToPlusToda As Integer
            Dim liDistancePlusDispTo As Integer


            sb.Append("<?xml version=" & ControlChars.Quote & "1.0" & ControlChars.Quote & " encoding=" & ControlChars.Quote & _
                         "UTF-8" & ControlChars.Quote & "?>" & ControlChars.NewLine & "")
            sb.Append("<kml xmlns=" & ControlChars.Quote & "http://earth.google.com/kml/2.0" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
            sb.Append("<Document xmlns:xlink=" & ControlChars.Quote & "http://www.w3/org/1999/xlink" & ControlChars.Quote & ">" & ControlChars.NewLine & "")
            sb.Append("<name>Splay</name>" & ControlChars.NewLine & "")

            lbRunwayHasReciprocalRunway = False
            lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(poRunway.Tables(0).Rows(0)("RwyId"))

            loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, poRunway.Tables(0).Rows(0)("ICAO"), _
                lsReciprocalRunwayId, "").Tables(0).Copy
            If loReciprocalRunwayTable.Rows.Count > 0 Then
                lbRunwayHasReciprocalRunway = True
            End If



            sb.Append("<LookAt>" & ControlChars.NewLine & "")
            sb.Append("<longitude>" & loADMS_BAL_Data_Checks.Coord_StringToReal(loReciprocalRunwayTable.Rows(0)("LonDir"), _
                                                                               loReciprocalRunwayTable.Rows(0)("LonDeg"), _
                                                                               loReciprocalRunwayTable.Rows(0)("LonMin"), _
                                                                               loReciprocalRunwayTable.Rows(0)("LonSec")) & _
                                                                               "</longitude>" & ControlChars.NewLine & "")


            sb.Append("<latitude>" & loADMS_BAL_Data_Checks.Coord_StringToReal(loReciprocalRunwayTable.Rows(0)("LatDir"), _
                                                                                loReciprocalRunwayTable.Rows(0)("LatDeg"), _
                                                                                loReciprocalRunwayTable.Rows(0)("LatMin"), _
                                                                                loReciprocalRunwayTable.Rows(0)("LatSec")) & _
                                                                                "</latitude>" & ControlChars.NewLine & "")
            sb.Append("<range>" & (2 * Convert.ToInt32(poRunway.Tables(0).Rows(0)("TODA"))) & "</range>" & ControlChars.NewLine & "")
            sb.Append("<tilt>0.00000000000000000</tilt>" & ControlChars.NewLine & "")
            sb.Append("<heading>0.00000000000000000</heading>" & ControlChars.NewLine & "")
            sb.Append("</LookAt>" & ControlChars.NewLine & "")

            sb.Append(getAirportStyle(psStyleFilePath))

            sb.Append("<Placemark>" & ControlChars.NewLine & "<name>Line Ident</name>" & ControlChars.NewLine & _
                      "<description>Line Name</description>" & ControlChars.NewLine & "")
            sb.Append("<styleUrl>#CL</styleUrl>" & ControlChars.NewLine & "<LineString>" & ControlChars.NewLine & "")


            liDispToPlusToda = Convert.ToInt32(poRunway.Tables(0).Rows(0)("DispTo")) + Convert.ToInt32(poRunway.Tables(0).Rows(0)("TODA"))

           
            If lbRunwayHasReciprocalRunway = True Then

                LatNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(poRunway.Tables(0).Rows(0)("LatDir"), _
                                          poRunway.Tables(0).Rows(0)("LatDeg"), poRunway.Tables(0).Rows(0)("LatMin"), _
                                          poRunway.Tables(0).Rows(0)("LatSec"), lbValidLatNumA)

                LonNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(poRunway.Tables(0).Rows(0)("LonDir"), _
                        poRunway.Tables(0).Rows(0)("LonDeg"), poRunway.Tables(0).Rows(0)("LonMin"), _
                       poRunway.Tables(0).Rows(0)("LonSec"), lbValidLonNumA)


                LatRecipRwyNumB = loADMS_BAL_Data_Checks.GetDecimalDegree(loReciprocalRunwayTable.Rows(0)("LatDir"), _
                       loReciprocalRunwayTable.Rows(0)("LatDeg"), loReciprocalRunwayTable.Rows(0)("LatMin"), _
                       loReciprocalRunwayTable.Rows(0)("LatSec"), lbValidLatNumB)

                LonRecipRwyNumB = loADMS_BAL_Data_Checks.GetDecimalDegree(loReciprocalRunwayTable.Rows(0)("LonDir"), _
                        loReciprocalRunwayTable.Rows(0)("LonDeg"), loReciprocalRunwayTable.Rows(0)("LonMin"), _
                        loReciprocalRunwayTable.Rows(0)("LonSec"), lbValidLonNumB)

                Height = 0

                If lbValidLatNumA = True And lbValidLonNumA = True And lbValidLatNumB = True And lbValidLonNumB = True Then
                    Answers_AtoB = loADMS_BAL_Data_Checks.Great_CircleAtoB(LatNumA, LonNumA, LatRecipRwyNumB, LonRecipRwyNumB, Height)

                    SplayPoints = loADMS_BAL_Data_Checks.ReturnSplay(LatNumA, LonNumA, Answers_AtoB.TrackA, liDispToPlusToda)

                    'BC And BF points
                    sb.Append("<coordinates>" & SplayPoints(0, 0) & "," & SplayPoints(0, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("" & SplayPoints(1, 0) & "," & SplayPoints(1, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("</coordinates>" & ControlChars.NewLine & "</LineString>" & ControlChars.NewLine & _
                              "</Placemark>" & ControlChars.NewLine & "")

                    'FL, SL, BL, BR, SR, FR points
                    sb.Append("<Placemark>" & ControlChars.NewLine & _
                              "<name>Splay Ident</name>" & ControlChars.NewLine & _
                              "<description>Splay Name</description>" & _
                              ControlChars.NewLine & "<styleUrl>#TakeOffSplay</styleUrl>" & _
                              ControlChars.NewLine & "<LineString>" & ControlChars.NewLine & "")

                    sb.Append("<coordinates>" & SplayPoints(2, 0) & "," & SplayPoints(2, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("" & SplayPoints(3, 0) & "," & SplayPoints(3, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("" & SplayPoints(4, 0) & "," & SplayPoints(4, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("" & SplayPoints(5, 0) & "," & SplayPoints(5, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("" & SplayPoints(6, 0) & "," & SplayPoints(6, 1) & ",0" & ControlChars.NewLine & "")
                    sb.Append("" & SplayPoints(7, 0) & "," & SplayPoints(7, 1) & ",0" & ControlChars.NewLine & "")

                    sb.Append("" & ControlChars.NewLine & "</coordinates>" & _
                              ControlChars.NewLine & "</LineString>" & ControlChars.NewLine & _
                              "</Placemark>" & ControlChars.NewLine & "")

                End If
            End If

            If Not poRunway.Tables(2) Is Nothing Then
                If poRunway.Tables(2).Rows.Count > 0 Then
                    For I = 0 To poRunway.Tables(2).Rows.Count - 1
                        sb.Append("<Placemark>" & ControlChars.NewLine & _
                              "<name>" & poRunway.Tables(2).Rows(I)("Elevation") & "</name>" & ControlChars.NewLine & "")

                        sb.Append("<description>" & ControlChars.NewLine & _
                                  "<table width=" & ControlChars.Quote & "300" & ControlChars.Quote & ">" & _
                                  ControlChars.NewLine & "<tr>" & ControlChars.NewLine & "<td>" & ControlChars.NewLine & "")

                        sb.Append("<P>Runway " & poRunway.Tables(0).Rows(0)("RwyId") & " " & _
                                  poRunway.Tables(0).Rows(0)("RwyMod") & "</P>" & ControlChars.NewLine & "")
                        sb.Append("<P>Obstacle " & poRunway.Tables(2).Rows(I)("Distance") & " m from SOT</P>" & ControlChars.NewLine & "")
                        sb.Append("<P>" & poRunway.Tables(2).Rows(I)("ObsRef") & "</P>" & ControlChars.NewLine & "")
                        sb.Append("<br>" & poRunway.Tables(2).Rows(I)("Comment") & "</br>" & ControlChars.NewLine & "")
                        sb.Append("</td>" & ControlChars.NewLine & _
                                  "</tr>" & ControlChars.NewLine & _
                                  "</table>" & ControlChars.NewLine & _
                                  "</description>" & ControlChars.NewLine & "")
                        sb.Append("<styleUrl>#ObstacleSquare</styleUrl>" & ControlChars.NewLine & "")
                        sb.Append("<Point>" & ControlChars.NewLine & "")

                        If lbRunwayHasReciprocalRunway = True Then

                            liDistancePlusDispTo = Convert.ToInt32(poRunway.Tables(2).Rows(I)("Distance")) + Convert.ToInt32(poRunway.Tables(0).Rows(0)("DispTo"))

                            ObstaclePoints = loADMS_BAL_Data_Checks.ReturnCoords(LatNumA, LonNumA, Answers_AtoB.TrackA, _
                                                            liDistancePlusDispTo, poRunway.Tables(2).Rows(I)("LatOffSet"))
                            sb.Append("<coordinates>" & ObstaclePoints(0, 0) & "," & ObstaclePoints(0, 1) & "</coordinates>" & ControlChars.NewLine & "")
                        End If
                        sb.Append("</Point>" & ControlChars.NewLine & "")
                        sb.Append("</Placemark>")

                    Next
                End If
            End If
            sb.Append("<Placemark>" & ControlChars.NewLine & "<name>Procedures</name>" & _
                      ControlChars.NewLine & "<description>" & ControlChars.NewLine & _
                      "<table width=" & ControlChars.Quote & "250" & ControlChars.Quote & ">" & ControlChars.NewLine & _
                      "<tr>" & ControlChars.NewLine & "")

            sb.Append("<td>Takeoff Procedures -  Rwy  " & poRunway.Tables(0).Rows(0)("RwyId") & "  " & _
                          poRunway.Tables(0).Rows(0)("RwyMod") & "</td>" & _
                          ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")



            If IsDBNull(poRunway.Tables(0).Rows(0)("AEProc")) = False Then
                If poRunway.Tables(0).Rows(0)("AEProc") <> "" Then
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>All Takeoffs</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>" & poRunway.Tables(0).Rows(0)("AEProc") & "</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                End If
            Else
                sb.Append("<tr>" & ControlChars.NewLine & "<td>All Takeoffs</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                sb.Append("<tr>" & ControlChars.NewLine & "<td> </td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
            End If
            If IsDBNull(poRunway.Tables(0).Rows(0)("EOProc")) = False Then
                If poRunway.Tables(0).Rows(0)("EOProc") <> "" Then
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>Engine-out</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>" & poRunway.Tables(0).Rows(0)("EOProc") & "</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                End If
            Else
                sb.Append("<tr>" & ControlChars.NewLine & "<td>Engine-out</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                sb.Append("<tr>" & ControlChars.NewLine & "<td> </td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
            End If

            If IsDBNull(poRunway.Tables(0).Rows(0)("PostProc")) = False Then
                If poRunway.Tables(0).Rows(0)("PostProc") <> "" Then
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>Post Takeoff</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>" & poRunway.Tables(0).Rows(0)("PostProc") & "</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                End If
            Else
                sb.Append("<tr>" & ControlChars.NewLine & "<td>Post Takeoff</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                sb.Append("<tr>" & ControlChars.NewLine & "<td> </td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
            End If
            If IsDBNull(poRunway.Tables(0).Rows(0)("Comment")) = False Then
                If poRunway.Tables(0).Rows(0)("Comment") <> "" Then
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>Comment</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                    sb.Append("<tr>" & ControlChars.NewLine & "<td>" & poRunway.Tables(0).Rows(0)("Comment") & "</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                End If
            Else
                sb.Append("<tr>" & ControlChars.NewLine & "<td>Comment</td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
                sb.Append("<tr>" & ControlChars.NewLine & "<td> </td>" & ControlChars.NewLine & "</tr>" & ControlChars.NewLine & "")
            End If

            sb.Append("</table>" & ControlChars.NewLine & _
                      "</description>" & ControlChars.NewLine & _
                      "<styleUrl>#EOP</styleUrl>" & ControlChars.NewLine & _
                      "<Point>" & ControlChars.NewLine & "")
            sb.Append("<coordinates>" & LonRecipRwyNumB & "," & LatRecipRwyNumB)

            sb.Append("</coordinates>" & ControlChars.NewLine & "</Point>" & _
                      ControlChars.NewLine & "</Placemark>" & ControlChars.NewLine & "")
            sb.Append("</Document>" & ControlChars.NewLine & "</kml>" & ControlChars.NewLine & "")

            Dim FILE_PATH As String = psKMLOutputFilePath & "SplayObstacle_" & poRunway.Tables(0).Rows(0)("ICAO") & _
                        "_" & psUserId & "_" & Date.Now.ToString("MM_dd_yyyy_HHmmss") & ".KML"

            generateKMLFile(FILE_PATH, sb)
            Return FILE_PATH

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Sub generateKMLFile(ByVal FILE_PATH As String, ByVal sb As StringBuilder)
        Try
            If File.Exists(FILE_PATH) = False Then
                File.Create(FILE_PATH).Dispose()
            Else
                System.IO.File.Delete(FILE_PATH)
            End If
            Dim objWriter As New System.IO.StreamWriter(FILE_PATH, True)
            objWriter.WriteLine(sb.ToString())
            objWriter.Close()
            objWriter = Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
