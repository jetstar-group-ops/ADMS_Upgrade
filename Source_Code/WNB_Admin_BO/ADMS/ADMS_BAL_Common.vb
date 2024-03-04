Imports WNB_Admin_DAL

Public Class ADMS_BAL_Common

    Public Function GetParameterTable(ByVal psUserId As String) As DataTable
        Dim loADMS_DAL_Common As New ADMS_DAL_Common

        Return loADMS_DAL_Common.GetParameterTable(psUserId)

    End Function

    Public Function GetParameterValue(ByRef poParameterTable As DataTable, _
                                      ByVal psParameterName As String) As String
        Dim loMyTable As DataTable
        Dim lsParameterValue As String = ""

        poParameterTable.DefaultView.RowFilter = "ParameterName = '" & psParameterName & "'"
        loMyTable = poParameterTable.DefaultView.ToTable

        If loMyTable.Rows.Count >= 1 Then
            lsParameterValue = loMyTable.Rows(0)("LimitValue") & ""
        End If

        Return lsParameterValue

    End Function

    Public Function GetEmptyDataChecksResultTable() As DataTable

        Dim loDataChecksResultTable As New DataTable

        loDataChecksResultTable.Columns.Add("ICAO", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("City", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("RwyId", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("RwyMod", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("IntersectionOrObstacleData", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("DcStatus", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("ErrDescription", System.Type.GetType("System.String"))
        loDataChecksResultTable.Columns.Add("ModuleId", System.Type.GetType("System.String"))

        Return loDataChecksResultTable

    End Function

    Public Sub AddRowIntoDataChecksResultTable(ByRef poDataChecksResultTable As DataTable, _
            ByVal psICAO As String, ByVal psCity As String, ByVal psRwyId As String, ByVal psRwyMod As String, _
            ByVal psIntersectionOrObstacleData As String, ByVal psDcStatus As String, _
            ByVal psErrDescription As String, ByVal psModuleId As String)

        Dim loDataRow As DataRow

        loDataRow = poDataChecksResultTable.NewRow

        loDataRow("ICAO") = psICAO
        loDataRow("City") = psCity
        loDataRow("RwyId") = psRwyId
        loDataRow("RwyMod") = psRwyMod
        loDataRow("IntersectionOrObstacleData") = psIntersectionOrObstacleData
        loDataRow("DcStatus") = psDcStatus
        loDataRow("ErrDescription") = psErrDescription
        loDataRow("ModuleId") = psModuleId

        poDataChecksResultTable.Rows.Add(loDataRow)
        poDataChecksResultTable.AcceptChanges()

    End Sub

    Public Function ValidateLatitude(ByRef poDataChecksResultTable As DataTable, _
         ByVal psIcao As String, ByVal psCity As String, ByVal psRwyId As String, _
         ByVal psRwyMod As String, ByVal psARIO_Data As String, _
         ByVal psDirection As String, ByVal psDegree As String, _
         ByVal psMin As String, ByVal psSec As String, _
         ByVal poParameterTable As DataTable, ByVal psModuleId As String) As Boolean

        Dim lbResult As Boolean = True
        Dim lsErrDescription As String

        If Not (psDirection = "N" Or psDirection = "S") Then
            lbResult = False
            lsErrDescription = "Invalid Latitude direction " & psDirection & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
                psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If

        If Not (Val(psDegree) >= 0 And Val(psDegree) <= 90) Then
            lbResult = False
            lsErrDescription = "Invalid Latitude degree " & psDegree & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
               psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If

        If Not (Val(psMin) >= 0 And Val(psMin) <= 59) Then
            lbResult = False
            lsErrDescription = "Invalid Latitude minutes  " & psMin & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
               psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If

        If Not (Val(psSec) >= 0 And Val(psSec) <= 59.99) Then
            lbResult = False
            lsErrDescription = "Invalid Latitude seconds  " & psSec & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
               psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If


        If lbResult = False Then

        End If

        Return lbResult
    End Function

    Public Function ValidateLongitude(ByRef poDataChecksResultTable As DataTable, _
        ByVal psIcao As String, ByVal psCity As String, ByVal psRwyId As String, _
        ByVal psRwyMod As String, ByVal psARIO_Data As String, _
        ByVal psDirection As String, ByVal psDegree As String, _
        ByVal psMin As String, ByVal psSec As String, _
        ByVal poParameterTable As DataTable, ByVal psModuleId As String) As Boolean

        Dim lbResult As Boolean = True
        Dim lsErrDescription As String

        If Not (psDirection = "E" Or psDirection = "W") Then
            lbResult = False
            lsErrDescription = "Invalid longitude direction " & psDirection & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
                psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If

        If Not (Val(psDegree) >= 0 And Val(psDegree) <= 180) Then
            lbResult = False
            lsErrDescription = "Invalid longitude degree " & psDegree & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
               psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If

        If Not (Val(psMin) >= 0 And Val(psMin) <= 59) Then
            lbResult = False
            lsErrDescription = "Invalid longitude minutes  " & psMin & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
               psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If

        If Not (Val(psSec) >= 0 And Val(psSec) <= 59.99) Then
            lbResult = False
            lsErrDescription = "Invalid longitude seconds  " & psSec & "."
            AddRowIntoDataChecksResultTable(poDataChecksResultTable, psIcao, psCity, _
               psRwyId, psRwyMod, psARIO_Data, IIf(lbResult = True, 1, 0), lsErrDescription, psModuleId)
        End If


        If lbResult = False Then

        End If

        Return lbResult
    End Function

    Public Function GetEmptyAirportDataTableToExportInMDB() As DataTable

        Dim loAirportResultTable As New DataTable

        loAirportResultTable.Columns.Add("fldICAOcd", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldATAcd", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldstnName", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldarptName", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldarptElev", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldarptmmemo", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldarptLong", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldarptLat", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldMagDev", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldUpdated", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldInactive", System.Type.GetType("System.String"))


        Return loAirportResultTable

    End Function

    Public Function GetEmptyRunwayDataTableToExportInMDB() As DataTable

        Dim loDataRunwayResultTable As New DataTable

        loDataRunwayResultTable.Columns.Add("fldICAOcd", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRWYno", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyHdg", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyIntersect", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldNotam", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwySpecial", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldATACd", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRWYID", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyBRE", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyLOE", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyPkE", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyPKD", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyTOD", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyClwy", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwySTPwy", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldTOSlope", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldTOobsn", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldTONote", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldLDLen", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldLDslope", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldGSLen", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldGSslope", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyUpdate", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyUpdSrc", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyTpdDt", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyInt", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldLndNote", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldAlignOpt", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldAlignDt", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldDisttoBoundary", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldTurn", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldTurnProcedure", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldPathWidth", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldSurface", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldStrength", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyWidth", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldrwyshldr", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyRefCode", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyNotam", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldRwyACFTapp", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldILSCAt", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldUpdated", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldInactive", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("flddelBRdist", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("flddelLODist", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldextobdist", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldextobhgt", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("fldextobsg", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("flddelAppdist", System.Type.GetType("System.String"))
        loDataRunwayResultTable.Columns.Add("flddelStpdist", System.Type.GetType("System.String"))


        Return loDataRunwayResultTable

    End Function

    Public Function GetEmptyObstacleDataTableToExportInMDB() As DataTable

        Dim loAirportResultTable As New DataTable

        loAirportResultTable.Columns.Add("fldRwyID", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSName", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSDist", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSHT", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSDEV", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBsDevLR", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSref", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSSRC", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSDT", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBsaddht", System.Type.GetType("System.String"))
        loAirportResultTable.Columns.Add("fldOBSNetGross", System.Type.GetType("System.String"))


        Return loAirportResultTable

    End Function


End Class
