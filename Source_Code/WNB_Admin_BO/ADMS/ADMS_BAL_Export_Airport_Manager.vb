Imports System.Text
Imports System.IO

Public Class ADMS_BAL_Export_Airport_Manager

#Region "CONSTANTS "
    Const Deg_Rad = Math.PI / 180
    Const Nm_m = 1852.0
    Const Nm_km = 1.852
    Const Ft_m = 0.3048
    Const Ft_km = Ft_m / 1000
    Const Ft_Nm = Ft_km / Nm_km
#End Region

    Dim liTotalObstaclesExported, liTotalRunwaysExported, piTotalIntstnExported, liTtlSumOfObs As Integer

    Shared QFUId As String
    Shared ModId As String
    Shared Space As String
    Shared ShkId As String

    Public Function ExportAirportManagerFile(ByVal psUserId As String, _
                                             ByVal psAirlineCode As String, _
                                             ByVal psFilePathToExport As String, _
                                             ByRef psSummary As String, _
                                             ByVal lbExportInactiveRecords As Boolean) As String
        Try
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim loAiports As DataTable
            Dim sbEAM As New StringBuilder
            Dim lsExportedTextFileName As String
            Dim loMagVar_Results As ADMS_BAL_Data_Checks.MagVar_Results
            Dim loMagVariationInDegree As Single
            Dim lsAiracCode As String = ""
            Dim lsCurrentAiracCycleStartDate As String = ""
            Dim lsCurrentAiracCycleEndDate As String = ""
            Dim lsNextAiracCycleStartDate As String = ""
            Dim lsMagVariation As StringBuilder
            Dim larrMagSec() As String
            Dim liNoOfAirportsExported As Integer
            Dim lsRnwySummary As String
            Dim loADMS_BAL_RDM As ADMS_BAL_RDM
            Dim ldsRunwayDetails As DataSet
            Dim dtRunwayValidate As DataTable

            loADMS_BAL_Data_Checks.GetAIRAC_Cycle_Data(Date.Now.AddDays(27).ToString("dd/MM/yyyy"), _
                    lsAiracCode, lsCurrentAiracCycleStartDate, _
                    lsCurrentAiracCycleEndDate, lsNextAiracCycleStartDate)

            lsExportedTextFileName = psAirlineCode & "_AptMgr_" & lsAiracCode & ""

            loAiports = loADMS_BAL_ADM.GetAllAirportsByStatus(psUserId, "", lbExportInactiveRecords)

            If Not loAiports Is Nothing Then
                If loAiports.Rows.Count > 0 Then

                    sbEAM.Append("Version=1;" & ControlChars.NewLine)

                    liNoOfAirportsExported = 0
                    For I = 0 To loAiports.Rows.Count - 1

                        If lbExportInactiveRecords = False And Convert.ToBoolean(loAiports.Rows(I)("Active")) = False Then
                            Continue For
                        End If

                        'Validating airport has atleast one active runway or ExpApp should be A or J-------------
                        ' If lbExportInactiveRecords = False Then --chnage

                        loADMS_BAL_RDM = New ADMS_BAL_RDM
                        ldsRunwayDetails = loADMS_BAL_RDM.GetRunWay(psUserId, loAiports.Rows(I)("ICAO"), "", "")
                        If Not ldsRunwayDetails Is Nothing Then
                            If ldsRunwayDetails.Tables(0).Rows.Count > 0 Then

                                ' ldsRunwayDetails.Tables(0).DefaultView.RowFilter = "Active='True' AND (ExpApp='J' OR ExpApp='U' OR ExpApp='A')"
                                ldsRunwayDetails.Tables(0).DefaultView.RowFilter = "(ExpApp='J' OR ExpApp='U' OR ExpApp='A')"
                                dtRunwayValidate = ldsRunwayDetails.Tables(0).DefaultView.ToTable()

                                If Not (dtRunwayValidate.Rows.Count > 0) Then
                                    Continue For
                                End If
                            End If
                        End If

                        loADMS_BAL_RDM = Nothing
                        ldsRunwayDetails = Nothing
                        'End If
                        '----------------------------------------------------------------------------------------


                        liNoOfAirportsExported = liNoOfAirportsExported + 1
                        sbEAM.Append("Airport;" & ControlChars.NewLine)
                        sbEAM.Append("  Name=" & loAiports.Rows(I)("City") & ";" & ControlChars.NewLine)
                        sbEAM.Append("  City=" & loAiports.Rows(I)("Name") & ";" & ControlChars.NewLine)
                        sbEAM.Append("  State=" & loAiports.Rows(I)("Country") & ";" & ControlChars.NewLine)
                        sbEAM.Append("  ICAO=" & loAiports.Rows(I)("ICAO") & ";" & ControlChars.NewLine)
                        sbEAM.Append("  IATA=" & loAiports.Rows(I)("IATA") & ";" & ControlChars.NewLine)
                        sbEAM.Append("  Latitude=00000000N;" & ControlChars.NewLine)
                        sbEAM.Append("  Longitude=000000000E;" & ControlChars.NewLine)
                        sbEAM.Append("  Elevation=" & Convert.ToInt32(loAiports.Rows(I)("Elevation")) * Ft_m & ";" & ControlChars.NewLine)

                        'Change in specification document for this comment.
                        'loMagVar_Results = loADMS_BAL_Data_Checks.Get_ADM_MagVarDataCheck(loAiports.Rows(I)("LatDir"), _
                        '         loAiports.Rows(I)("LatDeg"), loAiports.Rows(I)("LatMin"), _
                        '         loAiports.Rows(I)("LatSec"), loAiports.Rows(I)("LonDir"), _
                        '         loAiports.Rows(I)("LonDeg"), loAiports.Rows(I)("LonMin"), _
                        '         loAiports.Rows(I)("LonSec"), 0, _
                        '         Date.Now.Year)

                        'If loMagVar_Results.Err <> 0 Then
                        '    loMagVariationInDegree = 0
                        'Else
                        '    loMagVariationInDegree = loMagVar_Results.Dek
                        'End If
                        lsMagVariation = New StringBuilder
                        lsMagVariation.Append("")

                        If Convert.ToString(loAiports.Rows(I)("MagDeg")).Length = 2 Then
                            lsMagVariation.Append(loAiports.Rows(I)("MagDeg"))
                        ElseIf Convert.ToString(loAiports.Rows(I)("MagDeg")).Length = 1 Then
                            lsMagVariation.Append("0" & loAiports.Rows(I)("MagDeg"))
                        Else
                            lsMagVariation.Append(loAiports.Rows(I)("MagDeg"))
                        End If

                        If Convert.ToString(loAiports.Rows(I)("MagMin")).Length = 2 Then
                            lsMagVariation.Append(loAiports.Rows(I)("MagMin"))
                        ElseIf Convert.ToString(loAiports.Rows(I)("MagMin")).Length = 1 Then
                            lsMagVariation.Append("0" & loAiports.Rows(I)("MagMin"))
                        Else
                            lsMagVariation.Append(loAiports.Rows(I)("MagMin"))
                        End If

                        larrMagSec = Convert.ToString(loAiports.Rows(I)("MagSec")).Split(".")

                        If larrMagSec.Length > 1 Then
                            If Convert.ToString(Convert.ToInt32(larrMagSec(0))).Length < 2 Then
                                lsMagVariation.Append("0" & Convert.ToInt32(larrMagSec(0)))
                            ElseIf Convert.ToString(Convert.ToInt32(larrMagSec(0))).Length = 2 Then
                                lsMagVariation.Append(Convert.ToInt32(larrMagSec(0)))
                            Else
                                lsMagVariation.Append(Convert.ToInt32(larrMagSec(0)))
                            End If

                            If Convert.ToString(Convert.ToInt32(larrMagSec(1))).Length < 2 Then
                                lsMagVariation.Append(Convert.ToInt32(larrMagSec(1)) & "0")
                            ElseIf Convert.ToString(Convert.ToInt32(larrMagSec(0))).Length = 2 Then
                                lsMagVariation.Append(Convert.ToInt32(larrMagSec(1)))
                            End If
                        End If

                        lsMagVariation.Append(loAiports.Rows(I)("MagDir"))

                        sbEAM.Append("  MagneticVariation=" & lsMagVariation.ToString() & ";" & ControlChars.NewLine)
                        lsMagVariation = Nothing

                        sbEAM.Append("  Comments=;" & ControlChars.NewLine)
                        sbEAM.Append("  LastUpdate=;" & ControlChars.NewLine)

                        sbEAM.Append(getRunwayBlock(psUserId, loAiports.Rows(I)("ICAO"), lsRnwySummary, lbExportInactiveRecords))

                        sbEAM.Append("End;" & ControlChars.NewLine)


                    Next

                    GenerateAirportManagerTextFile(psFilePathToExport & lsExportedTextFileName, sbEAM)

                End If
            End If
            loAiports = Nothing
            psSummary = psSummary & liNoOfAirportsExported & ": Airports <BR>" & lsRnwySummary & "" & _
                        " were exported at " & Date.Now

            Return psFilePathToExport & lsExportedTextFileName
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getRunwayBlock(ByVal psUserId As String, ByVal psICAO As String, ByRef psRwySummary As String, ByVal lbExportInactiveRecords As Boolean) As String
        Try
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim ldsRunwayDetails As DataSet
            Dim sbRunway As New StringBuilder
            Dim DistinctRwyIdLst As New ArrayList()
            Dim lsGrpIdentMod As String = ""
            Dim lsRecipRwyId As String = ""
            Dim lsSelectedRwyId As String
            Dim lsFilterWhereRwyId As String
            Dim lsGroupId, lsPreviousGroupId As String
            Dim lsTotalCommentString, AEStr, EOStr, PostStr, CmtStr As String
            Dim laIteamfrmList As String()
            Dim lsngRwySlopeForIntersection As Single
            Dim dtRunway As DataTable
            Dim dtObstacles As DataTable
            Dim dtIntersection As DataTable
            Dim lsSharklet As String = System.Configuration.ConfigurationManager.AppSettings("ExportedAirportMaxSharplet")

            psRwySummary = ""

            ldsRunwayDetails = loADMS_BAL_RDM.GetRunWay(psUserId, psICAO, "", "")

            If Not ldsRunwayDetails Is Nothing Then
                If ldsRunwayDetails.Tables(0).Rows.Count > 0 Then
                    For J = 0 To ldsRunwayDetails.Tables(0).Rows.Count - 1
                        If Convert.ToInt32(ldsRunwayDetails.Tables(0).Rows(J)("RwyId").ToString.Substring(0, 2)) < 19 Then

                            If ldsRunwayDetails.Tables(0).Rows(J)("RwyId").ToString().Length = 2 Then
                                lsGrpIdentMod = ldsRunwayDetails.Tables(0).Rows(J)("RwyId") & " /" & ldsRunwayDetails.Tables(0).Rows(J)("RwyId") & _
                                             " " & ldsRunwayDetails.Tables(0).Rows(J)("RwyMod")
                            Else
                                lsGrpIdentMod = ldsRunwayDetails.Tables(0).Rows(J)("RwyId") & "/" & ldsRunwayDetails.Tables(0).Rows(J)("RwyId") & _
                                             "" & ldsRunwayDetails.Tables(0).Rows(J)("RwyMod")
                            End If
                        Else
                            lsRecipRwyId = loADMS_BAL_Data_Checks.GetRecipocalRunway(ldsRunwayDetails.Tables(0).Rows(J)("RwyId"))

                            If lsRecipRwyId.Length = 2 Then
                                lsGrpIdentMod = lsRecipRwyId & " /" & ldsRunwayDetails.Tables(0).Rows(J)("RwyId") & _
                                             " " & ldsRunwayDetails.Tables(0).Rows(J)("RwyMod")
                            Else
                                lsGrpIdentMod = lsRecipRwyId & "/" & ldsRunwayDetails.Tables(0).Rows(J)("RwyId") & _
                                             "" & ldsRunwayDetails.Tables(0).Rows(J)("RwyMod")
                            End If
                        End If
                        DistinctRwyIdLst.Add(lsGrpIdentMod)
                    Next
                End If
            End If

            DistinctRwyIdLst.Sort()

            For K = 0 To DistinctRwyIdLst.Count - 1

                lsSelectedRwyId = ""
                lsSelectedRwyId = DistinctRwyIdLst(K)

                Dim laSlash As String()
                Dim lsFilteredRwyMod As String
                laSlash = lsSelectedRwyId.Split("/")

                lsGroupId = laSlash(0).Trim()

                If laSlash(0).Trim.Length = 2 Then
                    lsFilterWhereRwyId = laSlash(1).Trim().Substring(0, 2)
                    lsFilteredRwyMod = laSlash(1).Trim().Substring(2, laSlash(1).ToString().Length - 2)
                ElseIf laSlash(0).Trim.Length = 3 Then
                    lsFilterWhereRwyId = laSlash(1).Trim().Substring(0, 3)
                    lsFilteredRwyMod = laSlash(1).Trim().Substring(3, laSlash(1).ToString().Length - 3)
                End If

                ldsRunwayDetails.Tables(0).DefaultView.RowFilter = "RwyId='" & lsFilterWhereRwyId.Trim() & _
                                                                 "' AND RwyMod='" & lsFilteredRwyMod.Trim() & "'"

                dtRunway = ldsRunwayDetails.Tables(0).DefaultView.ToTable()

                If dtRunway.Rows.Count > 0 Then

                    'Mukund Start: 02.01.2015 added when Fred asked as bug
                    If lbExportInactiveRecords = False And Convert.ToBoolean(dtRunway.Rows(0)("Active")) = False Then
                        Continue For
                    End If
                    'Mukund End: 02.01.2015 added when Fred asked as bug

                    If Not (Convert.ToString(dtRunway.Rows(0)("ExpApp")) = "J" Or Convert.ToString(dtRunway.Rows(0)("ExpApp")) = "A" Or Convert.ToString(dtRunway.Rows(0)("ExpApp")) = "U") Then 'added by swami on 22-02-2024'


                        'Mukund: start 15.12.2014 this is for NZAA; as there is only runway it was failing in End; statement.
                        If K = (DistinctRwyIdLst.Count - 1) Then
                            sbRunway.Append("  End;" & ControlChars.NewLine)
                        End If
                        'Mukund: end 15.12.2014
                        Continue For
                    End If

                    If lsGroupId <> lsPreviousGroupId Then

                        sbRunway.Append("  Runway;" & ControlChars.NewLine)
                        sbRunway.Append("    MagneticHeading=" & dtRunway.Rows(0)("MagHdg") & ";" & ControlChars.NewLine)
                        sbRunway.Append("    MagneticHeadingDate=;" & ControlChars.NewLine)
                        sbRunway.Append("    Strength=;" & ControlChars.NewLine)
                        sbRunway.Append("    MaxLength=" & dtRunway.Rows(0)("TORA") & ";" & ControlChars.NewLine)
                        sbRunway.Append("    Width=" & dtRunway.Rows(0)("Width") & ";" & ControlChars.NewLine)
                        sbRunway.Append("    Shoulder=" & Convert.ToInt32(dtRunway.Rows(0)("Shoulder")) / 2.0 & ";" & ControlChars.NewLine)
                        sbRunway.Append("    Comments=;" & ControlChars.NewLine)
                        sbRunway.Append("    LastUpdate=;" & ControlChars.NewLine)
                    End If

                    sbRunway.Append("    QFU;" & ControlChars.NewLine)

                    liTotalRunwaysExported = liTotalRunwaysExported + 1

                    'CHANGED REQUEST. Ver.R
                    'If Convert.ToString(dtRunway.Rows(0)("RwyId")).Length = 2 Then
                    '    sbRunway.Append("      Ident=" & dtRunway.Rows(0)("RwyId") & _
                    '                      "  " & dtRunway.Rows(0)("RwyMod") & ";" & ControlChars.NewLine)
                    'Else
                    '    sbRunway.Append("      Ident=" & dtRunway.Rows(0)("RwyId") & _
                    '                     " " & dtRunway.Rows(0)("RwyMod") & ";" & ControlChars.NewLine)
                    'End If
                    Dim strRunwayId As String = ""

                    QFUId = ""
                    ModId = ""
                    Space = ""
                    ShkId = ""

                    strRunwayId = Get_RunwayId(dtRunway.Rows(0)("ICAO").ToString(), dtRunway.Rows(0)("RwyId").ToString(), dtRunway.Rows(0)("RwyMod").ToString(), lsSharklet, ldsRunwayDetails.Tables(0))

                    sbRunway.Append("      Ident=" & strRunwayId & ";" & ControlChars.NewLine)

                    sbRunway.Append("      ASDA=" & dtRunway.Rows(0)("ASDA") & ";" & ControlChars.NewLine)

                    'If Convert.ToString(dtRunway.Rows(0)("RwyMod")).ToUpper = Convert.ToString("std").ToUpper Then
                    '    sbRunway.Append("      LDA=" & dtRunway.Rows(0)("LDA") & ";" & ControlChars.NewLine)
                    'ElseIf dtRunway.Rows(0)("RwyMod") <> "std" Then
                    '    sbRunway.Append("      LDA=0;" & ControlChars.NewLine)
                    'End If

                    'If Convert.ToString(dtRunway.Rows(0)("RwyMod")).ToUpper <> Convert.ToString(lsSharklet).ToUpper Then
                    '    sbRunway.Append("      LDA=" & dtRunway.Rows(0)("LDA") & ";" & ControlChars.NewLine)
                    'ElseIf Convert.ToString(dtRunway.Rows(0)("RwyMod")).ToUpper = Convert.ToString(lsSharklet).ToUpper Then
                    '    sbRunway.Append("      LDA=1;" & ControlChars.NewLine)
                    'End If

                    'As per Rev. R
                    sbRunway.Append("      LDA=" & dtRunway.Rows(0)("LDA") & ";" & ControlChars.NewLine)
                    '------
                    sbRunway.Append("      TODA=" & dtRunway.Rows(0)("TODA") & ";" & ControlChars.NewLine)
                    sbRunway.Append("      TORA=" & dtRunway.Rows(0)("TORA") & ";" & ControlChars.NewLine)

                    If dtRunway.Rows(0)("SlopeDir") = "Up" Then
                        lsngRwySlopeForIntersection = dtRunway.Rows(0)("Slope")
                        sbRunway.Append("      Slope=" & dtRunway.Rows(0)("Slope") & ";" & ControlChars.NewLine)
                    ElseIf dtRunway.Rows(0)("SlopeDir") = "Dn" Then
                        lsngRwySlopeForIntersection = Convert.ToSingle(dtRunway.Rows(0)("Slope")) * -1
                        sbRunway.Append("      Slope=" & Convert.ToSingle(dtRunway.Rows(0)("Slope")) * -1 & ";" & ControlChars.NewLine)

                    End If

                    sbRunway.Append("      EntryAngle=" & dtRunway.Rows(0)("LineUpAngle") & ";" & ControlChars.NewLine)
                    sbRunway.Append("      TakeoffShift=0;" & ControlChars.NewLine)
                    sbRunway.Append("      ThresholdElevation=" & Convert.ToInt32(dtRunway.Rows(0)("ElevStartTORA")) * Ft_m & ";" & ControlChars.NewLine)
                    sbRunway.Append("      ThresholdLatitude=00000000N;" & ControlChars.NewLine)
                    sbRunway.Append("      ThresholdLongitude=000000000E;" & ControlChars.NewLine)
                    sbRunway.Append("      ApproachSlope=" & dtRunway.Rows(0)("GaGrad") & ";" & ControlChars.NewLine)
                    sbRunway.Append("      IncrementGAHeight=0.0;" & ControlChars.NewLine)
                    sbRunway.Append("      GroovedPFCSurfaceTO=False;" & ControlChars.NewLine)
                    sbRunway.Append("      GroovedPFCStopway=False;" & ControlChars.NewLine)
                    sbRunway.Append("      GroovedPFCSurfaceLD=False;" & ControlChars.NewLine)
                    sbRunway.Append("      RunwayPavement=1;" & ControlChars.NewLine)
                    sbRunway.Append("      Comments=;" & ControlChars.NewLine)

                    lsTotalCommentString = ""

                    If IsDBNull(dtRunway.Rows(0)("AEProc")) = True Then
                        AEStr = ""
                    ElseIf dtRunway.Rows(0)("AEProc") = "" Then
                        AEStr = ""
                    ElseIf IsDBNull(dtRunway.Rows(0)("AEProc")) = False Then
                        If dtRunway.Rows(0)("AEProc") <> "" Then
                            AEStr = "All Engines:" & vbCrLf & dtRunway.Rows(0)("AEProc") & vbCrLf
                        End If
                    End If

                    If IsDBNull(dtRunway.Rows(0)("EOProc")) = True Then
                        EOStr = ""
                    ElseIf dtRunway.Rows(0)("EOProc") = "" Then
                        EOStr = ""
                    ElseIf IsDBNull(dtRunway.Rows(0)("EOProc")) = False Then
                        If dtRunway.Rows(0)("EOProc") <> "" Then
                            EOStr = "EOP:" & vbCrLf & dtRunway.Rows(0)("EOProc") & vbCrLf
                        End If
                    End If

                    If IsDBNull(dtRunway.Rows(0)("PostProc")) = True Then
                        PostStr = ""
                    ElseIf dtRunway.Rows(0)("PostProc") = "" Then
                        PostStr = ""
                    ElseIf IsDBNull(dtRunway.Rows(0)("PostProc")) = False Then
                        If dtRunway.Rows(0)("PostProc") <> "" Then
                            PostStr = "Post T/O:" & vbCrLf & dtRunway.Rows(0)("PostProc") & vbCrLf
                        End If
                    End If

                    If IsDBNull(dtRunway.Rows(0)("Comment")) = True Then
                        CmtStr = ""
                    ElseIf dtRunway.Rows(0)("Comment") = "" Then
                        CmtStr = ""
                    ElseIf IsDBNull(dtRunway.Rows(0)("Comment")) = False Then
                        If dtRunway.Rows(0)("Comment") <> "" Then
                            CmtStr = "Comment:" & vbCrLf & dtRunway.Rows(0)("Comment") & vbCrLf
                        End If
                    End If

                    lsTotalCommentString = AEStr & EOStr & PostStr & CmtStr
                    sbRunway.Append("      TOComments=" & lsTotalCommentString & ";" & ControlChars.NewLine)
                    sbRunway.Append("      LDComments=" & dtRunway.Rows(0)("LandComment") & ";" & ControlChars.NewLine)
                    sbRunway.Append("      LastUpdate=;" & ControlChars.NewLine)




                    ldsRunwayDetails.Tables(2).DefaultView.RowFilter = "ICAO='" & dtRunway.Rows(0)("ICAO") & _
                                                                     "' AND RwyId='" & dtRunway.Rows(0)("RwyId") & _
                                                                     "' AND RwyMod='" & dtRunway.Rows(0)("RwyMod") & "'"

                    ldsRunwayDetails.Tables(1).DefaultView.RowFilter = "ICAO='" & dtRunway.Rows(0)("ICAO") & _
                                                               "' AND RwyId='" & dtRunway.Rows(0)("RwyId") & _
                                                               "' AND RwyMod='" & dtRunway.Rows(0)("RwyMod") & "'"

                    dtObstacles = ldsRunwayDetails.Tables(2).DefaultView.ToTable()
                    dtIntersection = ldsRunwayDetails.Tables(1).DefaultView.ToTable()

                    sbRunway.Append(getObstacleBlock(dtObstacles, 0, liTotalObstaclesExported, lbExportInactiveRecords))

                    liTtlSumOfObs = liTtlSumOfObs + liTotalObstaclesExported
                    sbRunway.Append("    End;" & ControlChars.NewLine)

                    sbRunway.Append(getIntersectionBlock(dtIntersection, _
                                                         dtObstacles, _
                                                         dtRunway.Rows(0)("ASDA"), _
                                                         dtRunway.Rows(0)("TODA"), _
                                                         dtRunway.Rows(0)("TORA"), _
                                                         lsngRwySlopeForIntersection, _
                                                         lsTotalCommentString, _
                                                         IIf(IsDBNull(dtRunway.Rows(0)("LandComment")), "", dtRunway.Rows(0)("LandComment")), _
                                                         piTotalIntstnExported, _
                                                         dtRunway, _
                                                        lsSharklet, _
                                                        lbExportInactiveRecords))


                    dtIntersection = Nothing
                    dtObstacles = Nothing

                    If K = (DistinctRwyIdLst.Count - 1) Then
                        sbRunway.Append("  End;" & ControlChars.NewLine)
                    Else
                        Dim nextItem As String
                        Dim nextItemArr As String()
                        nextItemArr = DistinctRwyIdLst(K + 1).ToString().Split("/")
                        nextItem = nextItemArr(0).Trim()

                        If nextItem <> lsGroupId Then
                            sbRunway.Append("  End;" & ControlChars.NewLine)
                        End If
                        nextItemArr = Nothing
                    End If

                    lsPreviousGroupId = lsGroupId

                End If

            Next
            psRwySummary = psRwySummary + " " & (liTotalRunwaysExported + piTotalIntstnExported) & ": QFUs <Br>" & liTtlSumOfObs & ": Obstacles <Br>"
            'intTotalObstaclesExported
            'liTotalRunwaysExported

            Return sbRunway.ToString()


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getObstacleBlock(ByVal poObstacles As DataTable, _
                                      ByVal intDltFieldLength As Integer, _
                                      ByRef intTotalObstaclesExported As Integer, _
                                      ByVal lbExportInactiveRecords As Boolean) As String
        Try
            Dim sbObstacle As New StringBuilder

            If Not poObstacles Is Nothing Then
                intTotalObstaclesExported = poObstacles.Rows.Count
                If poObstacles.Rows.Count > 0 Then

                    For I = 0 To poObstacles.Rows.Count - 1

                        'Mukund Start: 02.01.2015 added when Fred asked as bug
                        If lbExportInactiveRecords = False And Convert.ToBoolean(poObstacles.Rows(I)("Active")) = False Then
                            Continue For
                        End If
                        'Mukund End: 02.01.2015 added when Fred asked as bug

                        sbObstacle.Append("      Obstacle;" & ControlChars.NewLine)
                        sbObstacle.Append("        Distance=" & Convert.ToInt32(poObstacles.Rows(I)("Distance")) - intDltFieldLength & _
                                                        ";" & ControlChars.NewLine)
                        sbObstacle.Append("        Elevation=" & Convert.ToInt32(poObstacles.Rows(I)("Elevation")) * Ft_m & _
                                                      ";" & ControlChars.NewLine)
                        sbObstacle.Append("        LateralDistance=" & Convert.ToInt32(poObstacles.Rows(I)("LatOffSet")) & ";" & ControlChars.NewLine)
                        sbObstacle.Append("        Nature=;" & ControlChars.NewLine)
                        sbObstacle.Append("        Comments=;" & ControlChars.NewLine)
                        sbObstacle.Append("        LastUpdate=;" & ControlChars.NewLine)
                        sbObstacle.Append("      End;" & ControlChars.NewLine)

                    Next
                End If
            End If

            Return sbObstacle.ToString()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getIntersectionBlock(ByVal poIntersection As DataTable, _
                                          ByVal poObstacle As DataTable, _
                                          ByVal poRwyASDA As Integer, _
                                          ByVal poRwyTODA As Integer, _
                                          ByVal poRwyTORA As Integer, _
                                          ByVal poRwyRSlop As Single, _
                                          ByVal lsTotalCommentString As String, _
                                          ByVal lsLandComment As String, _
                                          ByRef piTotalIntstnExported As Integer, _
                                          ByVal podtRunways As DataTable, _
                                          ByVal lsSharklet As String, _
                                          ByVal lbExportInactiveRecords As Boolean) As String
        Try
            Dim sbIntersection As New StringBuilder

            If Not poIntersection Is Nothing Then
                If poIntersection.Rows.Count > 0 Then
                    For I = 0 To poIntersection.Rows.Count - 1

                        'Mukund Start: 02.01.2015 added when Fred asked as bug
                        If lbExportInactiveRecords = False And Convert.ToBoolean(poIntersection.Rows(I)("Active")) = False Then
                            Continue For
                        End If
                        'Mukund End: 02.01.2015 added when Fred asked as bug

                        sbIntersection.Append("    QFU;" & ControlChars.NewLine)
                        piTotalIntstnExported = piTotalIntstnExported + 1

                        'To add one more extra space before RwyMod
                        'If Convert.ToString(poIntersection.Rows(I)("RwyId")).Length = 2 Then
                        '    sbIntersection.Append("      Ident=" & poIntersection.Rows(I)("RwyId") & _
                        '                                     "  " & poIntersection.Rows(I)("RwyMod") & _
                        '                                     " " & poIntersection.Rows(I)("Ident") & _
                        '                                     ";" & ControlChars.NewLine)

                        'Else
                        '    sbIntersection.Append("      Ident=" & poIntersection.Rows(I)("RwyId") & _
                        '                                     " " & poIntersection.Rows(I)("RwyMod") & _
                        '                                     " " & poIntersection.Rows(I)("Ident") & _
                        '                                     ";" & ControlChars.NewLine)
                        'End If
                        'As per Rev.R
                        Dim strRunwayIdIntersection As String = ""
                        'strRunwayIdIntersection = Get_RunwayIdForIntersection(podtRunways.Rows(0)("ICAO"), podtRunways.Rows(0)("RwyId").ToString(), podtRunways.Rows(0)("RwyMod"), lsSharklet, podtRunways, poIntersection.Rows(I)("Ident"))
                        strRunwayIdIntersection = Trim(QFUId + ModId + Space + poIntersection.Rows(I)("Ident") + " " + ShkId)
                        '---
                        sbIntersection.Append("      Ident=" & strRunwayIdIntersection & ";" & ControlChars.NewLine)

                        sbIntersection.Append("      ASDA=" & poRwyASDA - Convert.ToInt32(poIntersection.Rows(I)("DeltaFieldLength")) & _
                                                                ";" & ControlChars.NewLine)
                        sbIntersection.Append("      LDA=1;" & ControlChars.NewLine)
                        sbIntersection.Append("      TODA=" & poRwyTODA - Convert.ToInt32(poIntersection.Rows(I)("DeltaFieldLength")) & _
                                                                ";" & ControlChars.NewLine)
                        sbIntersection.Append("      TORA=" & poRwyTORA - Convert.ToInt32(poIntersection.Rows(I)("DeltaFieldLength")) & _
                                                                ";" & ControlChars.NewLine)
                        sbIntersection.Append("      Slope=" & poRwyRSlop & ";" & ControlChars.NewLine)
                        sbIntersection.Append("      EntryAngle=" & poIntersection.Rows(I)("LineUpAngle") & ";" & ControlChars.NewLine)
                        sbIntersection.Append("      TakeoffShift=0;" & ControlChars.NewLine)

                        sbIntersection.Append("      ThresholdElevation=" & Convert.ToInt32(poIntersection.Rows(I)("ElevStartTORA")) * Ft_m & _
                                                                ";" & ControlChars.NewLine)
                        sbIntersection.Append("      ThresholdLatitude=00000000N;" & ControlChars.NewLine)
                        sbIntersection.Append("      ThresholdLongitude=000000000E;" & ControlChars.NewLine)
                        sbIntersection.Append("      ApproachSlope=;" & ControlChars.NewLine)
                        sbIntersection.Append("      IncrementGAHeight=0;" & ControlChars.NewLine)
                        sbIntersection.Append("      GroovedPFCSurfaceTO=False;" & ControlChars.NewLine)
                        sbIntersection.Append("      GroovedPFCStopway=False;" & ControlChars.NewLine)
                        sbIntersection.Append("      GroovedPFCSurfaceLD=False;" & ControlChars.NewLine)
                        sbIntersection.Append("      RunwayPavement=1;" & ControlChars.NewLine)
                        sbIntersection.Append("      Comments=;" & ControlChars.NewLine)
                        sbIntersection.Append("      TOComments=" & lsTotalCommentString & ";" & ControlChars.NewLine)
                        sbIntersection.Append("      LDComments=" & lsLandComment & ";" & ControlChars.NewLine)
                        sbIntersection.Append("      LastUpdate=;" & ControlChars.NewLine)

                        sbIntersection.Append(getObstacleBlock(poObstacle, Convert.ToInt32(poIntersection.Rows(I)("DeltaFieldLength")), liTotalObstaclesExported, lbExportInactiveRecords))
                        liTtlSumOfObs = liTtlSumOfObs + liTotalObstaclesExported

                        sbIntersection.Append("    End;" & ControlChars.NewLine)
                    Next
                End If
            End If

            Return sbIntersection.ToString()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function Get_RunwayId(ByVal strCurrentRwyICAO As String, _
                                  ByVal strCurrentRwyId As String, _
                                  ByVal strCurrentRwyMod As String, _
                                  ByVal strSharklet As String, _
                                  ByVal dtRwys As DataTable) As String
        Try
           

            Dim p As Integer


            Dim OtherMod As String
            Dim OtherModId As String
            Dim q As Integer
            Dim strResultRunwayId As String

            Dim dtRwyMod As New DataTable

            Dim AllTag As String = "All"
            Dim NonTag As String = "Non"
            ' Make provision for the values of AllTag and NonTAg to be changed in the future 

            QFUId = strCurrentRwyId + " "
            If Len(QFUId) = 3 Then
                QFUId = QFUId + " "
            End If

            If UCase(strCurrentRwyMod) = "STD" Then
                ModId = ""
            Else
                ModId = strCurrentRwyMod
            End If

            p = strCurrentRwyMod.IndexOf(strSharklet)

            If p > -1 Then
                ModId = Trim(Left(strCurrentRwyMod, p))
                ShkId = strSharklet
            End If

            If p = -1 Then
                'SQL Query

                dtRwys.DefaultView.RowFilter = "ICAO='" & strCurrentRwyICAO & "' AND RwyId='" & strCurrentRwyId & "'"
                dtRwyMod = dtRwys.DefaultView.ToTable()

                ShkId = AllTag

                If Not dtRwyMod Is Nothing Then
                    If dtRwyMod.Rows.Count > 0 Then
                        For intCnter = 0 To dtRwyMod.Rows.Count - 1
                            OtherMod = dtRwyMod.Rows(intCnter)(2).ToString()  'from current record in SQL results set 
                            q = OtherMod.IndexOf(strSharklet)
                            If q > -1 Then
                                OtherModId = Left(OtherMod, q)
                            Else
                                OtherModId = OtherMod
                            End If
                            OtherModId = Trim(OtherModId)
                            If ((q > -1) And (OtherModId = ModId)) Then
                                ShkId = NonTag
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            If ModId = "" Then Space = "" Else Space = " "

            strResultRunwayId = Trim(QFUId + ModId + Space + ShkId)

            Return strResultRunwayId
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function Get_RunwayIdForIntersection(ByVal strCurrentRwyICAO As String, _
                                                ByVal strCurrentRwyId As String, _
                                                ByVal strCurrentRwyMod As String, _
                                                ByVal strSharklet As String, _
                                                ByVal dtRwys As DataTable, _
                                                ByVal strIntrstnIdent As String) As String
        Try
            Dim QFUId As String
            Dim p As Integer
            Dim ShkId As String
            Dim ModId As String
            Dim OtherMod As String
            Dim OtherModId As String
            Dim q As Integer
            Dim strResultRunwayId As String
            Dim Space As String
            Dim dtRwyMod As New DataTable

            Dim AllTag As String = "All"
            Dim NonTag As String = "Non"
            ' Make provision for the values of AllTag and NonTAg to be changed in the future 

            QFUId = strCurrentRwyId + " "
            If Len(QFUId) = 3 Then
                QFUId = QFUId + " "
            End If

            If UCase(strCurrentRwyMod) = "STD" Then
                ModId = ""
            Else
                ModId = strCurrentRwyMod
            End If

            p = strCurrentRwyMod.IndexOf(strSharklet)

            If p > -1 Then
                ModId = Trim(Left(strCurrentRwyMod, p))
                ShkId = strSharklet
            End If
            If p = -1 Then
                'SQL Query

                dtRwys.DefaultView.RowFilter = "ICAO='" & strCurrentRwyICAO & "' AND RwyId='" & strCurrentRwyId & "'"
                dtRwyMod = dtRwys.DefaultView.ToTable()

                ShkId = AllTag

                If Not dtRwyMod Is Nothing Then
                    If dtRwyMod.Rows.Count > 0 Then
                        For intCnter = 0 To dtRwyMod.Rows.Count - 1
                            OtherMod = dtRwyMod.Rows(intCnter)(2).ToString()  'from current record in SQL results set 
                            q = OtherMod.IndexOf(strSharklet)
                            If q > -1 Then
                                OtherModId = Left(OtherMod, q)
                            Else
                                OtherModId = OtherMod
                            End If
                            OtherModId = Trim(OtherModId)
                            If ((q > -1) And (OtherModId = ModId)) Then
                                ShkId = NonTag
                                Exit For
                            End If
                        Next
                    End If
                End If



            End If

            If ModId = "" Then Space = "" Else Space = " "

            strResultRunwayId = Trim(QFUId + ModId + Space + strIntrstnIdent + " " + ShkId)

            Return strResultRunwayId
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Private Function GenerateAirportManagerTextFile(ByVal FILE_PATH As String, ByVal sb As StringBuilder) As String
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
    End Function


End Class
