
Imports System.Text
Imports System.IO
Imports WNB_Admin_DAL

Public Class ADMS_BAL_Airport_Max

#Region "CONSTANTS "
    Const Deg_Rad = Math.PI / 180
    Const Nm_m = 1852.0
    Const Nm_km = 1.852
    Const Ft_m = 0.3048
    Const Ft_km = Ft_m / 1000
    Const Ft_Nm = Ft_km / Nm_km
#End Region


    Public Function ExportAirportDataInMDB(ByVal psUserId As String, _
                                           ByVal psSharklet As String, _
                                           ByVal lsDataSourcePathWithExtension As String, _
                                                   ByRef psSummaryInfo As String) As String

        'ByVal psExportInactiveRcrds As Boolean, _

        Try
            Dim loADMS_BAL_Common As New ADMS_BAL_Common
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loADMS_DAL_Airport_Max As New ADMS_DAL_Airport_Max
            Dim loAiportTableForMDB As DataTable
            Dim loRunwayTableForMDB As DataTable
            Dim loObstacleTableForMDB_Rwy As DataTable
            Dim loObstacleTableForMDB_Intesection As DataTable
            Dim loIntersectionTableForMDB As DataTable
            Dim loAirports As DataTable
            Dim loRunways As DataSet
            Dim ldsAirportRunwayObstacles As New DataSet
            Dim lsNoReciprocalRwy As New StringBuilder
            Dim liTotalAirports, liTotalRwys, liTotalObstacles As Integer

            lsNoReciprocalRwy.Append("")

            'AIRPORTS Data processsing
            loAirports = loADMS_BAL_ADM.GetAllAirports(psUserId, "")
            loAiportTableForMDB = loADMS_BAL_Common.GetEmptyAirportDataTableToExportInMDB()

            Dim loDataRow As DataRow


            If Not loAirports Is Nothing Then
                If loAirports.Rows.Count > 0 Then
                    For I = 0 To loAirports.Rows.Count - 1

                        'If (loAirports.Rows(I)("Active") = False And psExportInactiveRcrds = True) Then
                        '    Continue For
                        'End If
                        liTotalAirports = liTotalAirports + 1

                        loDataRow = loAiportTableForMDB.NewRow
                        loDataRow("fldICAOcd") = loAirports.Rows(I)("ICAO")
                        loDataRow("fldATAcd") = loAirports.Rows(I)("IATA")
                        loDataRow("fldstnName") = loAirports.Rows(I)("City")
                        loDataRow("fldarptName") = loAirports.Rows(I)("Name")
                        loDataRow("fldarptElev") = loAirports.Rows(I)("Elevation")
                        loDataRow("fldarptmmemo") = " "
                        loDataRow("fldarptLong") = "000000000E"
                        loDataRow("fldarptLat") = "00000000S"
                        loDataRow("fldMagDev") = "000"
                        loDataRow("fldUpdated") = "True"
                        loDataRow("fldInactive") = IIf(Convert.ToBoolean(loAirports.Rows(I)("Active")), False, True)

                        loAiportTableForMDB.Rows.Add(loDataRow)
                        loAiportTableForMDB.AcceptChanges()

                    Next
                End If
            End If

            ldsAirportRunwayObstacles.Tables.Add(loAiportTableForMDB)
            ldsAirportRunwayObstacles.AcceptChanges()

            'RUNWAYS Data processsing
            loRunwayTableForMDB = loADMS_BAL_Common.GetEmptyRunwayDataTableToExportInMDB()
            loObstacleTableForMDB_Rwy = loADMS_BAL_Common.GetEmptyObstacleDataTableToExportInMDB()
            loObstacleTableForMDB_Intesection = loADMS_BAL_Common.GetEmptyObstacleDataTableToExportInMDB()

            Dim Special As String
            Dim FldRwyNo As String
            Dim FldRwyId As String
            Dim RwyApplic As String
            Dim SpecApplic As String = ""
            Dim p As Integer
            Dim RSlope As Single
            Dim lsTotalCommentString, AEStr, EOStr, PostStr, CmtStr As String
            Dim LineupKey As Integer

            Dim lbRunwayHasReciprocalRunway As Boolean = False
            Dim lsReciprocalRunwayId As String
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim loReciprocalRunwayTable As DataTable
            Dim dtObstacles, dtNoOfObstacles As DataTable


            If Not loAirports Is Nothing Then
                If loAirports.Rows.Count > 0 Then
                    For I = 0 To loAirports.Rows.Count - 1

                        loRunways = loADMS_BAL_RDM.GetRunWay(psUserId, loAirports.Rows(I)("ICAO"), "", "")

                        If Not loRunways Is Nothing Then
                            If loRunways.Tables(0).Rows.Count > 0 Then

                                For J = 0 To loRunways.Tables(0).Rows.Count - 1

                                    'If (loRunways.Tables(0).Rows(J)("Active") = False And psExportInactiveRcrds = False) Then
                                    '    Continue For
                                    'End If

                                    ''---------Commented as per new spec document (JQ-AirportDatabase-Design-RevK)---------------------
                                    'If loRunways.Tables(0).Rows(J)("RwyMod") = "Std" Then
                                    '    Special = ""
                                    'Else
                                    '    p = loRunways.Tables(0).Rows(J)("RwyMod").ToString().IndexOf(psSharklet)
                                    '    If p > -1 Then
                                    '        Special = Left(loRunways.Tables(0).Rows(J)("RwyMod"), p - 2)
                                    '    Else
                                    '        Special = loRunways.Tables(0).Rows(J)("RwyMod")
                                    '    End If
                                    'End If
                                    'If Special <> "" Then
                                    '    Special = "/" + Special
                                    'End If


                                    'If IsDBNull(loRunways.Tables(0).Rows(J)("AcftApp")) Then
                                    '    SpecApplic = Special
                                    'ElseIf loRunways.Tables(0).Rows(J)("AcftApp") <> "" Then
                                    '    SpecApplic = Special + ">" + loRunways.Tables(0).Rows(J)("AcftApp")
                                    'Else
                                    '    SpecApplic = Special
                                    'End If


                                    'FldRwyNo = loRunways.Tables(0).Rows(J)("RwyId") + SpecApplic
                                    'FldRwyId = loRunways.Tables(0).Rows(J)("ICAO") + FldRwyNo
                                    '------------------------------------------------------------

                                    If IsDBNull(loRunways.Tables(0).Rows(J)("AcftApp")) Then
                                        RwyApplic = ""
                                    ElseIf loRunways.Tables(0).Rows(J)("AcftApp") <> "" Then
                                        RwyApplic = loRunways.Tables(0).Rows(J)("AcftApp")
                                    Else
                                        RwyApplic = ""
                                    End If

                                    If loRunways.Tables(0).Rows(J)("RwyMod") = "Std" Then
                                        Special = ""
                                    Else
                                        p = Convert.ToString(loRunways.Tables(0).Rows(J)("RwyMod")).IndexOf(psSharklet)
                                        If p > -1 Then
                                            Special = Left(loRunways.Tables(0).Rows(J)("RwyMod"), p)
                                            RwyApplic = "S"
                                        Else
                                            Special = loRunways.Tables(0).Rows(J)("RwyMod")
                                        End If
                                    End If

                                    If Special <> "" Then
                                        Special = "/" + Special
                                    End If
                                    If RwyApplic <> "" Then
                                        SpecApplic = Special + ">" + RwyApplic
                                    Else
                                        SpecApplic = Special
                                    End If

                                    If Special <> "" Then
                                        If Special.Substring(0, 1) = "/" Then
                                            Special = Special.Substring(1, Special.ToString().Length - 1)
                                        End If
                                    End If

                                    FldRwyNo = loRunways.Tables(0).Rows(J)("RwyId") + SpecApplic
                                    FldRwyId = loRunways.Tables(0).Rows(J)("ICAO") + FldRwyNo


                                    'Get Reciprocal Runway

                                    lbRunwayHasReciprocalRunway = False
                                    lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(loRunways.Tables(0).Rows(J)("RwyId"))


                                    'loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, loRunways.Tables(0).Rows(J)("ICAO"), _
                                    '                        lsReciprocalRunwayId, "").Tables(0).Copy 'added by swami on 26-02-2024
                                    loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, loRunways.Tables(0).Rows(J)("ICAO"), _
                                                            lsReciprocalRunwayId, loRunways.Tables(0).Rows(J)("RwyMod")).Tables(0).Copy



                                    If loReciprocalRunwayTable.Rows.Count > 0 Then
                                        lbRunwayHasReciprocalRunway = True
                                    End If

                                    If lbRunwayHasReciprocalRunway = False Then
                                        lsNoReciprocalRwy.Append(loRunways.Tables(0).Rows(J)("ICAO") & " " & _
                                                            loRunways.Tables(0).Rows(J)("RwyId") & " has no reciprocal runway." & "<BR>")
                                        Continue For
                                    End If


                                    liTotalRwys = liTotalRwys + 1

                                    loDataRow = loRunwayTableForMDB.NewRow
                                    loDataRow("fldICAOcd") = loRunways.Tables(0).Rows(J)("ICAO")
                                    loDataRow("fldRWYno") = FldRwyNo
                                    loDataRow("fldRwyHdg") = loRunways.Tables(0).Rows(J)("RwyId")
                                    loDataRow("fldRwyIntersect") = ""
                                    loDataRow("fldNotam") = "False"
                                    loDataRow("fldRwySpecial") = Special
                                    loDataRow("fldATACd") = loAirports.Rows(I)("IATA")
                                    loDataRow("fldRWYID") = FldRwyId
                                    loDataRow("fldRwyBRE") = IIf(IsDBNull(loRunways.Tables(0).Rows(J)("ElevStartTORA")), "0", loRunways.Tables(0).Rows(J)("ElevStartTORA"))

                                    If loReciprocalRunwayTable.Rows.Count = 0 Then
                                        loDataRow("fldRwyLOE") = ""
                                    Else
                                        loDataRow("fldRwyLOE") = IIf(IsDBNull(loReciprocalRunwayTable.Rows(0)("ElevStartTORA")), "0", loReciprocalRunwayTable.Rows(0)("ElevStartTORA"))
                                    End If

                                    loDataRow("fldRwyPkE") = "-10000"
                                    loDataRow("fldRwyPKD") = "-10000"

                                    loDataRow("fldRwyTOD") = Convert.ToDecimal(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("TORA")), "0", loRunways.Tables(0).Rows(J)("TORA"))) / Ft_m
                                    loDataRow("fldRwyClwy") = (Convert.ToInt32(loRunways.Tables(0).Rows(J)("TODA")) - Convert.ToInt32(loRunways.Tables(0).Rows(J)("TORA"))) / Ft_m
                                    loDataRow("fldRwySTPwy") = (Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("ASDA")), "0", loRunways.Tables(0).Rows(J)("ASDA"))) - Convert.ToInt32(loRunways.Tables(0).Rows(J)("TORA"))) / Ft_m

                                    If loRunways.Tables(0).Rows(J)("SlopeDir") = "Up" Then
                                        RSlope = Convert.ToSingle(loRunways.Tables(0).Rows(J)("Slope"))
                                    Else
                                        RSlope = Convert.ToSingle(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("Slope")), 0, Convert.ToSingle(loRunways.Tables(0).Rows(J)("Slope")))) * -1
                                    End If

                                    loDataRow("fldTOSlope") = RSlope / 100.0

                                    dtNoOfObstacles = New DataTable
                                    loRunways.Tables(2).DefaultView.RowFilter = "ICAO='" & loRunways.Tables(0).Rows(J)("ICAO") & _
                                                        "' AND RwyId='" & loRunways.Tables(0).Rows(J)("RwyId") & _
                                                        "' AND RwyMod='" & loRunways.Tables(0).Rows(J)("RwyMod") & "'"

                                    dtNoOfObstacles = loRunways.Tables(2).DefaultView.ToTable

                                    If Not dtNoOfObstacles Is Nothing Then
                                        loDataRow("fldTOobsn") = dtNoOfObstacles.Rows.Count
                                    Else
                                        loDataRow("fldTOobsn") = 0
                                    End If
                                    dtNoOfObstacles = Nothing

                                    lsTotalCommentString = ""

                                    If IsDBNull(loRunways.Tables(0).Rows(J)("AEProc")) = True Then
                                        AEStr = ""
                                    ElseIf loRunways.Tables(0).Rows(J)("AEProc") = "" Then
                                        AEStr = ""
                                    ElseIf IsDBNull(loRunways.Tables(0).Rows(J)("AEProc")) = False Then
                                        If loRunways.Tables(0).Rows(J)("AEProc") <> "" Then
                                            'AEStr = "All Engines:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("AEProc")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                            AEStr = "All Engines:" & vbCrLf & loRunways.Tables(0).Rows(J)("AEProc") & vbCrLf
                                        End If
                                    End If

                                    If IsDBNull(loRunways.Tables(0).Rows(J)("EOProc")) = True Then
                                        EOStr = ""
                                    ElseIf loRunways.Tables(0).Rows(J)("EOProc") = "" Then
                                        EOStr = ""
                                    ElseIf IsDBNull(loRunways.Tables(0).Rows(J)("EOProc")) = False Then
                                        If loRunways.Tables(0).Rows(J)("EOProc") <> "" Then
                                            'EOStr = "EOP:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("EOProc")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                            EOStr = "EOP:" & vbCrLf & loRunways.Tables(0).Rows(J)("EOProc") & vbCrLf
                                        End If
                                    End If

                                    If IsDBNull(loRunways.Tables(0).Rows(J)("PostProc")) = True Then
                                        PostStr = ""
                                    ElseIf loRunways.Tables(0).Rows(J)("PostProc") = "" Then
                                        PostStr = ""
                                    ElseIf IsDBNull(loRunways.Tables(0).Rows(J)("PostProc")) = False Then
                                        If loRunways.Tables(0).Rows(J)("PostProc") <> "" Then
                                            'PostStr = "Post T/O:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("PostProc")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                            PostStr = "Post T/O:" & vbCrLf & loRunways.Tables(0).Rows(J)("PostProc") & vbCrLf
                                        End If
                                    End If

                                    If IsDBNull(loRunways.Tables(0).Rows(J)("Comment")) = True Then
                                        CmtStr = ""
                                    ElseIf loRunways.Tables(0).Rows(J)("Comment") = "" Then
                                        CmtStr = ""
                                    ElseIf IsDBNull(loRunways.Tables(0).Rows(J)("Comment")) = False Then
                                        If loRunways.Tables(0).Rows(J)("Comment") <> "" Then
                                            'CmtStr = "Comment:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("Comment")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                            CmtStr = "Comment:" & vbCrLf & Convert.ToString(loRunways.Tables(0).Rows(J)("Comment")).Replace(vbLf, vbCrLf) & vbCrLf
                                        End If
                                    End If

                                    lsTotalCommentString = AEStr & EOStr & PostStr & CmtStr


                                    loDataRow("fldTONote") = lsTotalCommentString
                                    loDataRow("fldLDLen") = Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("LDA")), 0, loRunways.Tables(0).Rows(J)("LDA"))) / Ft_m
                                    loDataRow("fldLDslope") = RSlope / 100.0
                                    loDataRow("fldGSLen") = "0"
                                    loDataRow("fldGSslope") = "0.0"
                                    loDataRow("fldRwyUpdate") = loRunways.Tables(0).Rows(J)("ChangeDateTime")
                                    loDataRow("fldRwyUpdSrc") = " "
                                    loDataRow("fldRwyTpdDt") = loRunways.Tables(0).Rows(J)("ChangeDateTime")
                                    loDataRow("fldRwyInt") = loRunways.Tables(0).Rows(J)("ChangeUser").ToString().Substring(0, 3)
                                    loDataRow("fldLndNote") = " "

                                    If loRunways.Tables(0).Rows(J)("LineUpAngle") = "180" Then
                                        LineupKey = 2
                                    ElseIf loRunways.Tables(0).Rows(J)("LineUpAngle") = "90" Then
                                        LineupKey = 1
                                    Else
                                        LineupKey = 0
                                    End If

                                    loDataRow("fldAlignOpt") = LineupKey
                                    loDataRow("fldAlignDt") = "0"
                                    loDataRow("fldDisttoBoundary") = "0"
                                    loDataRow("fldTurn") = "False"
                                    loDataRow("fldTurnProcedure") = " "
                                    loDataRow("fldPathWidth") = "3"
                                    loDataRow("fldSurface") = "Hard Surface"
                                    loDataRow("fldStrength") = "UKN"
                                    loDataRow("fldRwyWidth") = Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("Width")), 0, loRunways.Tables(0).Rows(J)("Width"))) / Ft_m
                                    loDataRow("fldrwyshldr") = Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("Shoulder")), 0, loRunways.Tables(0).Rows(J)("Shoulder"))) / Ft_m
                                    loDataRow("fldRwyRefCode") = " "
                                    loDataRow("fldRwyNotam") = " "
                                    loDataRow("fldRwyACFTapp") = IIf(IsDBNull(loRunways.Tables(0).Rows(J)("AcftApp")), "", loRunways.Tables(0).Rows(J)("AcftApp"))
                                    loDataRow("fldILSCAt") = " "
                                    loDataRow("fldUpdated") = "True"
                                    loDataRow("fldInactive") = IIf(Convert.ToBoolean(loRunways.Tables(0).Rows(J)("Active")), False, True)
                                    loDataRow("flddelBRdist") = "0"
                                    loDataRow("flddelLODist") = "0"
                                    loDataRow("fldextobdist") = "0"
                                    loDataRow("fldextobhgt") = "0"
                                    loDataRow("fldextobsg") = "False"
                                    loDataRow("flddelAppdist") = "0"
                                    loDataRow("flddelStpdist") = "0"

                                    loRunwayTableForMDB.Rows.Add(loDataRow)
                                    loRunwayTableForMDB.AcceptChanges()

                                    'Obstacle Data processing
                                    Dim LR As String

                                    If Not loRunways.Tables(2) Is Nothing Then
                                        If loRunways.Tables(2).Rows.Count > 0 Then
                                            dtObstacles = New DataTable
                                            loRunways.Tables(2).DefaultView.RowFilter = "RwyId='" & loRunways.Tables(0).Rows(J)("RwyId") & _
                                                                "' And RwyMod='" & loRunways.Tables(0).Rows(J)("RwyMod") & "'"
                                            dtObstacles = loRunways.Tables(2).DefaultView.ToTable

                                            For K = 0 To dtObstacles.Rows.Count - 1

                                                'If (dtObstacles.Rows(K)("Active") = False And psExportInactiveRcrds = False) Then
                                                '    Continue For
                                                'End If
                                                liTotalObstacles = liTotalObstacles + 1

                                                loDataRow = loObstacleTableForMDB_Rwy.NewRow
                                                loDataRow("fldRwyID") = FldRwyId

                                                If IsDBNull(dtObstacles.Rows(K)("Comment")) = True Then
                                                    loDataRow("fldOBSName") = ""
                                                Else
                                                    If dtObstacles.Rows(K)("Comment").ToString().Length > 20 Then
                                                        loDataRow("fldOBSName") = dtObstacles.Rows(K)("Comment").ToString().Substring(0, 19)
                                                    Else
                                                        loDataRow("fldOBSName") = dtObstacles.Rows(K)("Comment")
                                                    End If
                                                End If


                                                loDataRow("fldOBSDist") = (Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("Distance")), 0, dtObstacles.Rows(K)("Distance"))) - Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(J)("TORA")), 0, loRunways.Tables(0).Rows(J)("TORA")))) / Ft_m

                                                If loReciprocalRunwayTable.Rows.Count = 0 Then
                                                    loDataRow("fldOBSHT") = (Convert.ToInt32(dtObstacles.Rows(K)("Elevation")) - 0)
                                                Else
                                                    loDataRow("fldOBSHT") = (Convert.ToInt32(dtObstacles.Rows(K)("Elevation")) - Convert.ToInt32(IIf(IsDBNull(loReciprocalRunwayTable.Rows(0)("ElevStartTORA")), 0, loReciprocalRunwayTable.Rows(0)("ElevStartTORA"))))
                                                End If

                                                loDataRow("fldOBSDEV") = (Math.Abs(Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("LatOffSet")), 0, dtObstacles.Rows(K)("LatOffSet")))) / Ft_m)

                                                If Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("LatOffSet")), 0, dtObstacles.Rows(K)("LatOffSet"))) < 0 Then
                                                    LR = "L"
                                                ElseIf Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("LatOffSet")), 0, dtObstacles.Rows(K)("LatOffSet"))) > 0 Then
                                                    LR = "R"
                                                Else
                                                    LR = "C"
                                                End If

                                                loDataRow("fldOBsDevLR") = LR
                                                loDataRow("fldOBSref") = "0"
                                                loDataRow("fldOBSSRC") = IIf(IsDBNull(dtObstacles.Rows(K)("ObsRef")), "", dtObstacles.Rows(K)("ObsRef"))
                                                loDataRow("fldOBSDT") = dtObstacles.Rows(K)("ChangeDateTime")
                                                loDataRow("fldOBsaddht") = "0"
                                                loDataRow("fldOBSNetGross") = "False"

                                                loObstacleTableForMDB_Rwy.Rows.Add(loDataRow)
                                                loObstacleTableForMDB_Rwy.AcceptChanges()

                                            Next
                                        End If
                                    End If

                                Next
                            End If
                        End If
                    Next
                End If
            End If

            ' Adding Runways
            ldsAirportRunwayObstacles.Tables.Add(loRunwayTableForMDB)
            ldsAirportRunwayObstacles.AcceptChanges()

            ' Adding OBSTACLES
            ldsAirportRunwayObstacles.Tables.Add(loObstacleTableForMDB_Rwy)
            ldsAirportRunwayObstacles.AcceptChanges()


            loIntersectionTableForMDB = loADMS_BAL_Common.GetEmptyRunwayDataTableToExportInMDB()

            ' Preparing INTERSECTION Data in Runway table
            Dim dtIntersections As DataTable

            If Not loAirports Is Nothing Then
                If loAirports.Rows.Count > 0 Then
                    For AptCounter = 0 To loAirports.Rows.Count - 1

                        loRunways = loADMS_BAL_RDM.GetRunWay(psUserId, loAirports.Rows(AptCounter)("ICAO"), "", "")

                        If Not loRunways Is Nothing Then
                            If loRunways.Tables(0).Rows.Count > 0 Then

                                For RwyCounter = 0 To loRunways.Tables(0).Rows.Count - 1

                                    'If (loRunways.Tables(0).Rows(RwyCounter)("Active") = False And psExportInactiveRcrds = False) Then
                                    '    Continue For
                                    'End If

                                    dtIntersections = New DataTable
                                    loRunways.Tables(1).DefaultView.RowFilter = "RwyId='" & loRunways.Tables(0).Rows(RwyCounter)("RwyId") & _
                                                                "' And RwyMod='" & loRunways.Tables(0).Rows(RwyCounter)("RwyMod") & "'"
                                    dtIntersections = loRunways.Tables(1).DefaultView.ToTable

                                    ''---------Commented as per new spec document (JQ-AirportDatabase-Design-RevK)---------------------

                                    'If loRunways.Tables(0).Rows(RwyCounter)("RwyMod") = "Std" Then
                                    '    Special = ""
                                    'Else
                                    '    p = loRunways.Tables(0).Rows(RwyCounter)("RwyMod").ToString().IndexOf(psSharklet)
                                    '    If p > -1 Then
                                    '        Special = Left(loRunways.Tables(0).Rows(RwyCounter)("RwyMod"), p - 2)
                                    '    Else
                                    '        Special = loRunways.Tables(0).Rows(RwyCounter)("RwyMod")
                                    '    End If
                                    'End If
                                    'If Special <> "" Then
                                    '    Special = "/" + Special
                                    'End If

                                    'If IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("AcftApp")) Then
                                    '    SpecApplic = Special
                                    'ElseIf loRunways.Tables(0).Rows(RwyCounter)("AcftApp") <> "" Then
                                    '    SpecApplic = Special + ">" + loRunways.Tables(0).Rows(RwyCounter)("AcftApp")
                                    'Else
                                    '    SpecApplic = Special
                                    'End If
                                    '----------------------------------------------------------------------------------
                                    If IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("AcftApp")) Then
                                        RwyApplic = ""
                                    ElseIf loRunways.Tables(0).Rows(RwyCounter)("AcftApp") <> "" Then
                                        RwyApplic = loRunways.Tables(0).Rows(RwyCounter)("AcftApp")
                                    Else
                                        RwyApplic = ""
                                    End If

                                    If loRunways.Tables(0).Rows(RwyCounter)("RwyMod") = "Std" Then
                                        Special = ""
                                    Else
                                        p = Convert.ToString(loRunways.Tables(0).Rows(RwyCounter)("RwyMod")).IndexOf(psSharklet)
                                        If p > -1 Then
                                            Special = Left(loRunways.Tables(0).Rows(RwyCounter)("RwyMod"), p)
                                            RwyApplic = "S"
                                        Else
                                            Special = loRunways.Tables(0).Rows(RwyCounter)("RwyMod")
                                        End If
                                    End If

                                    If Special <> "" Then
                                        Special = "/" + Special
                                    End If
                                    If RwyApplic <> "" Then
                                        SpecApplic = Special + ">" + RwyApplic
                                    Else
                                        SpecApplic = Special
                                    End If

                                    If Special <> "" Then
                                        If Special.Substring(0, 1) = "/" Then
                                            Special = Special.Substring(1, Special.ToString().Length - 1)
                                        End If
                                    End If


                                    lbRunwayHasReciprocalRunway = False
                                    lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(loRunways.Tables(0).Rows(RwyCounter)("RwyId"))
                                    loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, loRunways.Tables(0).Rows(RwyCounter)("ICAO"), _
                                                            lsReciprocalRunwayId, "").Tables(0).Copy
                                    If loReciprocalRunwayTable.Rows.Count > 0 Then
                                        lbRunwayHasReciprocalRunway = True
                                    End If


                                    If lbRunwayHasReciprocalRunway = False Then
                                        'lsNoReciprocalRwy.Append(loRunways.Tables(0).Rows(RwyCounter)("ICAO") & " " & _
                                        '                    loRunways.Tables(0).Rows(RwyCounter)("RwyId") & " has no reciprocal runway." & "<BR>")
                                        Continue For
                                    End If


                                    For intstnCounter = 0 To dtIntersections.Rows.Count - 1

                                        'If (dtIntersections.Rows(intstnCounter)("Active") = False And psExportInactiveRcrds = False) Then
                                        '    Continue For
                                        'End If

                                        liTotalRwys = liTotalRwys + 1

                                        FldRwyNo = loRunways.Tables(0).Rows(RwyCounter)("RwyId") + "+" + dtIntersections.Rows(intstnCounter)("Ident") + SpecApplic
                                        FldRwyId = loRunways.Tables(0).Rows(RwyCounter)("ICAO") + FldRwyNo


                                        loDataRow = loIntersectionTableForMDB.NewRow
                                        loDataRow("fldICAOcd") = loRunways.Tables(0).Rows(RwyCounter)("ICAO")
                                        loDataRow("fldRWYno") = FldRwyNo
                                        loDataRow("fldRwyHdg") = loRunways.Tables(0).Rows(RwyCounter)("RwyId")
                                        loDataRow("fldRwyIntersect") = dtIntersections.Rows(intstnCounter)("Ident")
                                        loDataRow("fldNotam") = "False"
                                        loDataRow("fldRwySpecial") = Special
                                        loDataRow("fldATACd") = loAirports.Rows(AptCounter)("IATA")
                                        loDataRow("fldRWYID") = FldRwyId
                                        loDataRow("fldRwyBRE") = IIf(IsDBNull(dtIntersections.Rows(intstnCounter)("ElevStartTORA")), 0, dtIntersections.Rows(intstnCounter)("ElevStartTORA"))

                                        If loReciprocalRunwayTable.Rows.Count = 0 Then
                                            loDataRow("fldRwyLOE") = "0"
                                        Else
                                            loDataRow("fldRwyLOE") = IIf(IsDBNull(loReciprocalRunwayTable.Rows(0)("ElevStartTORA")), 0, loReciprocalRunwayTable.Rows(0)("ElevStartTORA"))
                                        End If

                                        loDataRow("fldRwyPkE") = "-10000"
                                        loDataRow("fldRwyPKD") = "-10000"
                                        loDataRow("fldRwyTOD") = Convert.ToDecimal(IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("TORA")), 0, (Convert.ToInt32(loRunways.Tables(0).Rows(RwyCounter)("TORA")) - Convert.ToInt32(dtIntersections.Rows(intstnCounter)("DeltaFieldLength"))))) / Ft_m
                                        loDataRow("fldRwyClwy") = (Convert.ToInt32(loRunways.Tables(0).Rows(RwyCounter)("TODA")) - Convert.ToInt32(loRunways.Tables(0).Rows(RwyCounter)("TORA"))) / Ft_m
                                        loDataRow("fldRwySTPwy") = (Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("ASDA")), 0, loRunways.Tables(0).Rows(RwyCounter)("ASDA"))) - Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("TORA")), 0, loRunways.Tables(0).Rows(RwyCounter)("TORA")))) / Ft_m

                                        If loRunways.Tables(0).Rows(RwyCounter)("SlopeDir") = "Up" Then
                                            RSlope = Convert.ToSingle(loRunways.Tables(0).Rows(RwyCounter)("Slope"))
                                        Else
                                            RSlope = Convert.ToSingle(Convert.ToSingle(loRunways.Tables(0).Rows(RwyCounter)("Slope"))) * -1
                                        End If

                                        loDataRow("fldTOSlope") = RSlope / 100.0


                                        dtNoOfObstacles = New DataTable
                                        loRunways.Tables(2).DefaultView.RowFilter = "ICAO='" & loRunways.Tables(0).Rows(RwyCounter)("ICAO") & _
                                                            "' AND RwyId='" & loRunways.Tables(0).Rows(RwyCounter)("RwyId") & _
                                                            "' AND RwyMod='" & loRunways.Tables(0).Rows(RwyCounter)("RwyMod") & "'"

                                        dtNoOfObstacles = loRunways.Tables(2).DefaultView.ToTable

                                        If Not dtNoOfObstacles Is Nothing Then
                                            loDataRow("fldTOobsn") = dtNoOfObstacles.Rows.Count
                                        Else
                                            loDataRow("fldTOobsn") = 0
                                        End If
                                        dtNoOfObstacles = Nothing

                                        lsTotalCommentString = ""

                                        If IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("AEProc")) = True Then
                                            AEStr = ""
                                        ElseIf loRunways.Tables(0).Rows(RwyCounter)("AEProc") = "" Then
                                            AEStr = ""
                                        ElseIf IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("AEProc")) = False Then
                                            If loRunways.Tables(0).Rows(RwyCounter)("AEProc") <> "" Then
                                                'AEStr = "All Engines:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("AEProc")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                                AEStr = "All Engines:" & vbCrLf & loRunways.Tables(0).Rows(RwyCounter)("AEProc") & vbCrLf
                                            End If
                                        End If

                                        If IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("EOProc")) = True Then
                                            EOStr = ""
                                        ElseIf loRunways.Tables(0).Rows(RwyCounter)("EOProc") = "" Then
                                            EOStr = ""
                                        ElseIf IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("EOProc")) = False Then
                                            If loRunways.Tables(0).Rows(RwyCounter)("EOProc") <> "" Then
                                                'EOStr = "EOP:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("EOProc")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                                EOStr = "EOP:" & vbCrLf & loRunways.Tables(0).Rows(RwyCounter)("EOProc") & vbCrLf
                                            End If
                                        End If

                                        If IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("PostProc")) = True Then
                                            PostStr = ""
                                        ElseIf loRunways.Tables(0).Rows(RwyCounter)("PostProc") = "" Then
                                            PostStr = ""
                                        ElseIf IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("PostProc")) = False Then
                                            If loRunways.Tables(0).Rows(RwyCounter)("PostProc") <> "" Then
                                                'PostStr = "Post T/O:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("PostProc")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                                PostStr = "Post T/O:" & vbCrLf & loRunways.Tables(0).Rows(RwyCounter)("PostProc") & vbCrLf
                                            End If
                                        End If

                                        If IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("Comment")) = True Then
                                            CmtStr = ""
                                        ElseIf loRunways.Tables(0).Rows(RwyCounter)("Comment") = "" Then
                                            CmtStr = ""
                                        ElseIf IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("Comment")) = False Then
                                            If loRunways.Tables(0).Rows(RwyCounter)("Comment") <> "" Then
                                                'CmtStr = "Comment:" & vbCr & vbLf & Convert.ToString(loRunways.Tables(0).Rows(J)("Comment")).Replace(vbLf, vbNewLine) & vbCr & vbLf & vbCr & vbLf
                                                CmtStr = "Comment:" & vbCrLf & Convert.ToString(loRunways.Tables(0).Rows(RwyCounter)("Comment")).Replace(vbLf, vbCrLf) & vbCrLf
                                            End If
                                        End If

                                        lsTotalCommentString = AEStr & EOStr & PostStr & CmtStr


                                        loDataRow("fldTONote") = lsTotalCommentString
                                        loDataRow("fldLDLen") = "0"
                                        loDataRow("fldLDslope") = RSlope / 100.0
                                        loDataRow("fldGSLen") = "0"
                                        loDataRow("fldGSslope") = "0.0"
                                        loDataRow("fldRwyUpdate") = loRunways.Tables(0).Rows(RwyCounter)("ChangeDateTime")
                                        loDataRow("fldRwyUpdSrc") = " "
                                        loDataRow("fldRwyTpdDt") = loRunways.Tables(0).Rows(RwyCounter)("ChangeDateTime")
                                        loDataRow("fldRwyInt") = loRunways.Tables(0).Rows(RwyCounter)("ChangeUser").ToString().Substring(0, 3)
                                        loDataRow("fldLndNote") = " "

                                        If dtIntersections.Rows(intstnCounter)("LineUpAngle") = "180" Then
                                            LineupKey = 2
                                        ElseIf dtIntersections.Rows(intstnCounter)("LineUpAngle") = "90" Then
                                            LineupKey = 1
                                        Else
                                            LineupKey = 0
                                        End If

                                        loDataRow("fldAlignOpt") = LineupKey
                                        loDataRow("fldAlignDt") = "0"
                                        loDataRow("fldDisttoBoundary") = "0"
                                        loDataRow("fldTurn") = False
                                        loDataRow("fldTurnProcedure") = " "
                                        loDataRow("fldPathWidth") = "3"
                                        loDataRow("fldSurface") = "Hard Surface"
                                        loDataRow("fldStrength") = "UKN"
                                        loDataRow("fldRwyWidth") = Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("Width")), 0, loRunways.Tables(0).Rows(RwyCounter)("Width"))) / Ft_m
                                        loDataRow("fldrwyshldr") = Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("Shoulder")), 0, loRunways.Tables(0).Rows(RwyCounter)("Shoulder"))) / Ft_m
                                        loDataRow("fldRwyRefCode") = " "
                                        loDataRow("fldRwyNotam") = " "
                                        loDataRow("fldRwyACFTapp") = IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("AcftApp")), "", loRunways.Tables(0).Rows(RwyCounter)("AcftApp"))
                                        loDataRow("fldILSCAt") = " "
                                        loDataRow("fldUpdated") = "True"
                                        loDataRow("fldInactive") = IIf(Convert.ToBoolean(loRunways.Tables(0).Rows(RwyCounter)("Active")), False, True)
                                        loDataRow("flddelBRdist") = "0"
                                        loDataRow("flddelLODist") = "0"
                                        loDataRow("fldextobdist") = "0"
                                        loDataRow("fldextobhgt") = "0"
                                        loDataRow("fldextobsg") = "False"
                                        loDataRow("flddelAppdist") = "0"
                                        loDataRow("flddelStpdist") = "0"

                                        loIntersectionTableForMDB.Rows.Add(loDataRow)
                                        loIntersectionTableForMDB.AcceptChanges()

                                        'Obstacle Data processing
                                        Dim LR As String

                                        If Not loRunways.Tables(2) Is Nothing Then
                                            If loRunways.Tables(2).Rows.Count > 0 Then
                                                dtObstacles = New DataTable
                                                loRunways.Tables(2).DefaultView.RowFilter = "RwyId='" & loRunways.Tables(0).Rows(RwyCounter)("RwyId") & _
                                                                    "' And RwyMod='" & loRunways.Tables(0).Rows(RwyCounter)("RwyMod") & "'"
                                                dtObstacles = loRunways.Tables(2).DefaultView.ToTable

                                                For K = 0 To dtObstacles.Rows.Count - 1

                                                    'If (dtObstacles.Rows(K)("Active") = False And psExportInactiveRcrds = True) Then
                                                    '    Continue For
                                                    'End If

                                                    liTotalObstacles = liTotalObstacles + 1

                                                    loDataRow = loObstacleTableForMDB_Intesection.NewRow
                                                    loDataRow("fldRwyID") = FldRwyId

                                                    'loDataRow("fldOBSName") = IIf(IsDBNull(dtObstacles.Rows(K)("Comment")), "", dtObstacles.Rows(K)("Comment"))
                                                    If IsDBNull(dtObstacles.Rows(K)("Comment")) = True Then
                                                        loDataRow("fldOBSName") = ""
                                                    Else
                                                        If dtObstacles.Rows(K)("Comment").ToString().Length > 20 Then
                                                            loDataRow("fldOBSName") = dtObstacles.Rows(K)("Comment").ToString().Substring(0, 19)
                                                        Else
                                                            loDataRow("fldOBSName") = dtObstacles.Rows(K)("Comment")
                                                        End If
                                                    End If

                                                    loDataRow("fldOBSDist") = (Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("Distance")), 0, dtObstacles.Rows(K)("Distance"))) - Convert.ToInt32(IIf(IsDBNull(loRunways.Tables(0).Rows(RwyCounter)("TORA")), 0, loRunways.Tables(0).Rows(RwyCounter)("TORA")))) / Ft_m

                                                    If loReciprocalRunwayTable.Rows.Count = 0 Then
                                                        loDataRow("fldOBSHT") = (Convert.ToInt32(dtObstacles.Rows(K)("Elevation")) - 0)
                                                    Else
                                                        loDataRow("fldOBSHT") = (Convert.ToInt32(dtObstacles.Rows(K)("Elevation")) - Convert.ToInt32(IIf(IsDBNull(loReciprocalRunwayTable.Rows(0)("ElevStartTORA")), 0, loReciprocalRunwayTable.Rows(0)("ElevStartTORA"))))
                                                    End If

                                                    loDataRow("fldOBSDEV") = (Math.Abs(Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("LatOffSet")), 0, dtObstacles.Rows(K)("LatOffSet")))) / Ft_m)

                                                    If Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("LatOffSet")), 0, dtObstacles.Rows(K)("LatOffSet"))) < 0 Then
                                                        LR = "L"
                                                    ElseIf Convert.ToInt32(IIf(IsDBNull(dtObstacles.Rows(K)("LatOffSet")), 0, dtObstacles.Rows(K)("LatOffSet"))) > 0 Then
                                                        LR = "R"
                                                    Else
                                                        LR = "C"
                                                    End If

                                                    loDataRow("fldOBsDevLR") = LR
                                                    loDataRow("fldOBSref") = "0"
                                                    loDataRow("fldOBSSRC") = IIf(IsDBNull(dtObstacles.Rows(K)("ObsRef")), "", dtObstacles.Rows(K)("ObsRef"))
                                                    loDataRow("fldOBSDT") = dtObstacles.Rows(K)("ChangeDateTime")
                                                    loDataRow("fldOBsaddht") = "0"
                                                    loDataRow("fldOBSNetGross") = "False"

                                                    loObstacleTableForMDB_Intesection.Rows.Add(loDataRow)
                                                    loObstacleTableForMDB_Intesection.AcceptChanges()

                                                Next
                                            End If
                                        End If ' END OF Obstacle LOOP
                                    Next ' END OF INTERSECTION LOOP
                                    dtIntersections = Nothing
                                Next
                            End If
                        End If
                    Next
                End If
            End If

            ' Adding Intersections
            ldsAirportRunwayObstacles.Tables.Add(loIntersectionTableForMDB)
            ldsAirportRunwayObstacles.AcceptChanges()


            ' Adding Obstacles while adding intersections
            ldsAirportRunwayObstacles.Tables.Add(loObstacleTableForMDB_Intesection)
            ldsAirportRunwayObstacles.AcceptChanges()

            loADMS_DAL_Airport_Max.ExportDataToMDB(psUserId, ldsAirportRunwayObstacles, lsDataSourcePathWithExtension)


            psSummaryInfo = "<Br>" & liTotalAirports & ": Airports<Br>" & _
                             liTotalRwys & ": Runways<Br>" & liTotalObstacles & _
                             ": Obstacles<Br>were exported at " & Date.Now

            Return lsNoReciprocalRwy.ToString()

        Catch ex As Exception
            Throw ex
        End Try
    End Function



End Class
