'using System.Web.SessionState;
Imports System.Collections
Imports WNB_Admin_DAL

Public Class ADMS_BAL_ADM


    Dim goADMS_DAL_ADM As New ADMS_DAL_ADM

    Public Function GetAllAirports(ByVal psUserId As String, _
                                   ByVal psIcao As String) As DataTable
        Return goADMS_DAL_ADM.GetAllAirports(psUserId, psIcao)
    End Function

    Public Function GetAirportDetails(ByVal psUserId As String, _
                                     ByVal psIcao As String) As DataSet

        Return goADMS_DAL_ADM.GetAirportDetails(psUserId, psIcao)

    End Function

    Public Function CreateUpdateAirport(ByVal poAirportDetails As DataSet, _
                                     ByVal psUserId As String, _
                                     ByVal psIcao As String) As Integer

        Return goADMS_DAL_ADM.CreateUpdateAirport(poAirportDetails, psUserId, psIcao)

    End Function

    Public Function DeleteAirport(ByVal psUserId As String, _
                                    ByVal psIcao As String) As Integer

        Return goADMS_DAL_ADM.DeleteAirport(psUserId, psIcao)

    End Function

    'ADDED BY SWAMI ON 05-1-2024'
    Public Function GetAllAirportsByStatus(ByVal psUserId As String, _
                                   ByVal psIcao As String, ByVal psActive As Integer) As DataTable
        Return goADMS_DAL_ADM.GetAllAirportsByStatus(psUserId, psIcao, psActive)
    End Function

#Region "Data Check"

    Private Function IsAirportHasActiveRunways(ByVal psIcao As String, _
                            ByVal poRunways As DataTable) As Boolean

        Dim loMyRunways As DataTable

        'poRunways.DefaultView.RowFilter = "ICAO='" & psIcao & "' AND Active = 1"
        poRunways.DefaultView.RowFilter = "ICAO='" & psIcao & "'"
        loMyRunways = poRunways.DefaultView.ToTable

        If loMyRunways.Rows.Count = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Function AirportDataCheck(ByVal psIcao As String, _
                                     ByVal pbValidateRunways As Boolean, _
                                     ByVal psUserId As String, _
                                     ByVal pbValidateObstacles As Boolean, _
            ByVal pbValidateIntersections As Boolean, _
            ByRef psAirlineCode As String) As DataTable

        Dim loParameterTable As DataTable
        Dim loADMS_BO_Common As New ADMS_BAL_Common
        Dim loAirportsDataTable As DataTable
        Dim loDataChecksResultTable As DataTable
        Dim lsElevation As String
        Dim lsMaxAptElev As String
        Dim lsMinAptElev As String
        Dim lsErrDescription As String = ""
        Dim lsModuleId As String = "A"
        Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
        Dim loRunway As DataSet
        Dim ldMagVariationDegree As Single
        Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
        Dim liMaxMagVar, liMinMagVar As Single
        Dim ldLatitudeInDecDegree, ldLongitudeInDecDegree As Single
        Dim liMinLat, liMaxLat, liMinLon, liMaxLon As Single
        Dim pbIsValidLatDecimalDegreeValue, pbIsValidLonDecimalDegreeValue, pbIsValidAPMagVarDecimalDegreeValue As Boolean
        Dim lbValidCalculatedMagVar, lbValidCodedMagVar As Boolean
        Dim dsRunways As DataSet
        Dim liMaxMagErr As Single
        Dim liMaxNameLength, liMaxCityLength As Integer

        loAirportsDataTable = GetAllAirports(psUserId, psIcao)

        If Not loAirportsDataTable Is Nothing Then
            If loAirportsDataTable.Rows.Count > 0 Then
                psAirlineCode = Convert.ToString(loAirportsDataTable.Rows(0)(1))
            End If

        End If

        loParameterTable = loADMS_BO_Common.GetParameterTable(psUserId)
        loDataChecksResultTable = loADMS_BO_Common.GetEmptyDataChecksResultTable()

        lsMaxAptElev = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxAptElev")
        lsMinAptElev = loADMS_BO_Common.GetParameterValue(loParameterTable, "MinAptElev")
        liMaxMagVar = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxMagVar")
        liMinMagVar = loADMS_BO_Common.GetParameterValue(loParameterTable, "MinMagVar")
        liMinMagVar = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxMagErr")
        liMinLat = loADMS_BO_Common.GetParameterValue(loParameterTable, "MinLat")
        liMaxLat = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxLat")
        liMinLon = loADMS_BO_Common.GetParameterValue(loParameterTable, "MinLon")
        liMaxLon = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxLon")
        liMaxMagErr = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxMagErr")
        liMaxNameLength = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxNameLength")
        liMaxCityLength = loADMS_BO_Common.GetParameterValue(loParameterTable, "MaxCityLength")


        'loRunway = Nothing
        Dim lbIsAirportHasActiveRunway As Boolean = False
        For I = 0 To loAirportsDataTable.Rows.Count - 1

            loRunway = Nothing
            If I = 0 Then
                lsErrDescription = ""

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    "Error: ", "", "", "", "", 0, lsErrDescription, lsModuleId)
            End If

            'Runway Data check Point no. 1
            ' Runway Data check: 1. Check that the airport has data for at least one runway. If not, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If loRunway Is Nothing Then
                loRunway = loADMS_BAL_RDM.GetRunWay(psUserId, loAirportsDataTable.Rows(I)("ICAO"), "", "")
            End If

            lbIsAirportHasActiveRunway = IsAirportHasActiveRunways(loAirportsDataTable.Rows(I)("ICAO"), _
                                                                   loRunway.Tables(0))
            If lbIsAirportHasActiveRunway = False Then
                lsErrDescription = "No Runways in database for this airport."

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If


            'If Not loRunway Is Nothing Then
            '    If loRunway.Tables(0).Rows.Count = 0 Then
            '        lsErrDescription = "Airport " & loAirportsDataTable.Rows(I)("ICAO") & " should have atleast one Runway."

            '        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
            '            loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
            '            "", "", "", 0, lsErrDescription, lsModuleId)
            '    End If
            '    loRunway = Nothing

            'End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '2. If the airport elevation is greater than MaxAptElev or less than MinAptElev, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            lsElevation = loAirportsDataTable.Rows(I)("Elevation") & ""


            If Not (Val(lsElevation) > lsMinAptElev And Val(lsElevation) < lsMaxAptElev) Then
                lsErrDescription = "Airport elevation " & lsElevation & " out of range " & lsMinAptElev & " to " & lsMaxAptElev

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '3. validate latitude values
            'loADMS_BO_Common.ValidateLatitude(loDataChecksResultTable, _
            '    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("City"), _
            '    "", "", "", loAirportsDataTable.Rows(I)("LatDir"), loAirportsDataTable.Rows(I)("LatDeg"), _
            '    loAirportsDataTable.Rows(I)("LatMin"), loAirportsDataTable.Rows(I)("LatSec"), _
            '    loParameterTable, lsModuleId)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '' validate longitude values
            'loADMS_BO_Common.ValidateLongitude(loDataChecksResultTable, _
            '    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("City"), _
            '    "", "", "", loAirportsDataTable.Rows(I)("LonDir"), loAirportsDataTable.Rows(I)("LonDeg"), _
            '    loAirportsDataTable.Rows(I)("LonMin"), loAirportsDataTable.Rows(I)("LonSec"), _
            '    loParameterTable, lsModuleId)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '' validate magnetic values
            'loADMS_BO_Common.ValidateLongitude(loDataChecksResultTable, _
            '    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("City"), _
            '    "", "", "", loAirportsDataTable.Rows(I)("MagDir"), loAirportsDataTable.Rows(I)("MagDeg"), _
            '    loAirportsDataTable.Rows(I)("MagMin"), loAirportsDataTable.Rows(I)("MagSec"), _
            '    loParameterTable, lsModuleId)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ldLatitudeInDecDegree = loADMS_BAL_Data_Checks.GetDecimalDegree(loAirportsDataTable.Rows(I)("LatDir"), _
               loAirportsDataTable.Rows(I)("LatDeg"), loAirportsDataTable.Rows(I)("LatMin"), _
               loAirportsDataTable.Rows(I)("LatSec"), pbIsValidLatDecimalDegreeValue)

            ldLongitudeInDecDegree = loADMS_BAL_Data_Checks.GetDecimalDegree(loAirportsDataTable.Rows(I)("LonDir"), _
             loAirportsDataTable.Rows(I)("LonDeg"), loAirportsDataTable.Rows(I)("LonMin"), _
             loAirportsDataTable.Rows(I)("LonSec"), pbIsValidLonDecimalDegreeValue)

            If pbIsValidLatDecimalDegreeValue = False Then
                'If pbIsValidLatDecimalDegreeValue = True Then
                '    If Not (ldLatitudeInDecDegree > liMinLat And ldLatitudeInDecDegree < liMaxLat) Then
                '        lsErrDescription = "Airport Latitude reference point is out of range values." & _
                '                            "Range Values are from (LatMin)" & liMinLat & " to (LatMax) " & liMaxLat & ""

                '        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                '            loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                '            "", "", "", 0, lsErrDescription, lsModuleId)
                '    End If
                'Else
                lsErrDescription = "Invalid latitude " & loAirportsDataTable.Rows(I)("LatDir") & _
                " " & loAirportsDataTable.Rows(I)("LatDeg") & " " & loAirportsDataTable.Rows(I)("LatMin") & _
                " " & loAirportsDataTable.Rows(I)("LatSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If

            If pbIsValidLonDecimalDegreeValue = False Then
                '  If pbIsValidLonDecimalDegreeValue = True Then
                '    If Not (ldLongitudeInDecDegree > liMinLon And ldLongitudeInDecDegree < liMaxLon) Then
                '        lsErrDescription = "Airport Longitude reference point is out of range values." & _
                '                            "Range Values are from (LonMin)" & liMinLon & " to (LonMax) " & liMaxLon & ""

                '        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                '            loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                '            "", "", "", 0, lsErrDescription, lsModuleId)
                '    End If
                'Else
                lsErrDescription = "Invalid longitude " & loAirportsDataTable.Rows(I)("LonDir") & _
               " " & loAirportsDataTable.Rows(I)("LonDeg") & " " & loAirportsDataTable.Rows(I)("LonMin") & _
               " " & loAirportsDataTable.Rows(I)("LonSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '4. If this coded magnetic variation (in degrees) is greater than 
            ' MaxMagVar or less than MinMagVar, log an error.

            ldMagVariationDegree = loADMS_BAL_Data_Checks.GetDecimalDegree(loAirportsDataTable.Rows(I)("MagDir"), _
                loAirportsDataTable.Rows(I)("MagDeg"), loAirportsDataTable.Rows(I)("MagMin"), _
                loAirportsDataTable.Rows(I)("MagSec"), pbIsValidAPMagVarDecimalDegreeValue)

            If pbIsValidAPMagVarDecimalDegreeValue = False Then
                'If pbIsValidAPMagVarDecimalDegreeValue = True Then
                '    If Not ldMagVariationDegree > liMinMagVar And ldMagVariationDegree < liMaxMagVar Then
                '        lsErrDescription = "Invalid Mag Var " & loAirportsDataTable.Rows(I)("MagDir") & " " & _
                '         loAirportsDataTable.Rows(I)("MagDeg") & " " & loAirportsDataTable.Rows(I)("MagMin") & " " & _
                '        loAirportsDataTable.Rows(I)("MagSec")

                '        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                '            loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                '            "", "", "", 0, lsErrDescription, lsModuleId)
                '    End If
                'Else
                lsErrDescription = "Invalid Mag Var " & loAirportsDataTable.Rows(I)("MagDir") & _
                " " & loAirportsDataTable.Rows(I)("MagDeg") & " " & loAirportsDataTable.Rows(I)("MagMin") & _
                " " & loAirportsDataTable.Rows(I)("MagSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If


            '4 CONTUTINUE... Using the procedure MagVar, calculate the magnetic variation 
            'using the coordinates of the airport reference point. 
            'If the calculated magnetic variation (in degrees) varies 
            'from the coded magnetic variation (in degrees) by more than MaxMagErr, log an error.

            Dim loMagVar_Results As ADMS_BAL_Data_Checks.MagVar_Results

            Dim loCodedMagVarInDegree As Single
            Dim loMagVariationInDegree As Single

            loCodedMagVarInDegree = loADMS_BAL_Data_Checks.GetDecimalDegree(loAirportsDataTable.Rows(I)("MagDir"), _
                                                                            loAirportsDataTable.Rows(I)("MagDeg"), _
                                                                            loAirportsDataTable.Rows(I)("MagMin"), _
                                                                            loAirportsDataTable.Rows(I)("MagMin"), _
                                                                            lbValidCodedMagVar)

            If lbValidCodedMagVar = False Then
                lsErrDescription = "Invalid Coded Mag Var in degress " & loCodedMagVarInDegree

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If

            loMagVar_Results = loADMS_BAL_Data_Checks.Get_ADM_MagVarDataCheck(loAirportsDataTable.Rows(I)("LatDir"), _
                     loAirportsDataTable.Rows(I)("LatDeg"), loAirportsDataTable.Rows(I)("LatMin"), _
                     loAirportsDataTable.Rows(I)("LatSec"), loAirportsDataTable.Rows(I)("LonDir"), _
                     loAirportsDataTable.Rows(I)("LonDeg"), loAirportsDataTable.Rows(I)("LonMin"), _
                     loAirportsDataTable.Rows(I)("LonSec"), 0, _
                     Date.Now.Year)

            If loMagVar_Results.Err <> 0 Then

                'Commneted below line bcoz User doesnt required message that is Get_ADM_MagVarDataCheck()->MagVar() method throwing
                'lsErrDescription = loMagVar_Results.ErrStr

                'loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                '    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                '    "", "", "", 0, lsErrDescription, lsModuleId)
            Else

                loMagVariationInDegree = loMagVar_Results.Dek

                If lbValidCalculatedMagVar = False Then
                    If Math.Abs(loMagVariationInDegree - loCodedMagVarInDegree) > liMaxMagErr Then
                        lsErrDescription = "Calculated Mag Var " & loMagVariationInDegree & " differs from coded MagVar" & _
                        loCodedMagVarInDegree & " by more than " & liMaxMagErr

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                            loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                            "", "", "", 0, lsErrDescription, lsModuleId)
                    End If
                Else
                    lsErrDescription = "Invalid Calculated Mag Var in degress " & loMagVariationInDegree

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                        loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                        "", "", "", 0, lsErrDescription, lsModuleId)
                End If

            End If

            '5.  Add check parameter MaxNameLength and initially set to 30.
            'If the length of Airport.Name is greater than MaxNameLength log an error.
            If Convert.ToString(loAirportsDataTable.Rows(I)("Name")).Length > liMaxNameLength Then

                lsErrDescription = "Airport Name Length greater than " & liMaxNameLength & " characters"

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If


            '6 Add check parameter MaxCityLength and initially set to 25. 
            'If the length of Airport.City is greater than MaxCityLength log an error.

            If Convert.ToString(loAirportsDataTable.Rows(I)("City")).Length > liMaxCityLength Then

                lsErrDescription = "Airport City Length greater than " & liMaxCityLength & " characters"

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), _
                    "", "", "", 0, lsErrDescription, lsModuleId)
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Next

        If pbValidateRunways = True Then
            loADMS_BAL_RDM.RunwayDataCheck(loDataChecksResultTable, psIcao, "", "", _
                    pbValidateObstacles, pbValidateIntersections, loParameterTable, psUserId)
        End If

        'Mukund 09.Nov.2014: Start - To sort resulted error messages
        Dim loDataCRSorted As New DataTable
        loDataChecksResultTable.DefaultView.Sort = "ICAO ASC"
        loDataCRSorted = loDataChecksResultTable.DefaultView.ToTable()
        loDataChecksResultTable = loDataCRSorted
        loDataCRSorted = Nothing
        'Mukund 09.Nov.2014: End


        'To Check Inactive Records
        For I = 0 To loAirportsDataTable.Rows.Count - 1
            If I = 0 Then
                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    "   ", "", "", "", "", 0, "", lsModuleId)

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    "  ", "", "", "", "", 0, "", lsModuleId)

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    "    ", "", "", "", "", 0, "", lsModuleId)

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                    "Inactive Records:", "", "", "", "", 0, "", lsModuleId)
            End If
            If Convert.ToBoolean(loAirportsDataTable.Rows(I)("Active")) = False Then
                loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                  loAirportsDataTable.Rows(I)("ICAO"), loAirportsDataTable.Rows(I)("CITY"), "", "", "", 0, "", lsModuleId)
            End If
        Next

        dsRunways = loADMS_BAL_RDM.GetRunWay(psUserId, psIcao, "", "")

        'To show Inactive Runways, Inersections and Obstacles
        If Not dsRunways Is Nothing Then
            If dsRunways.Tables(0).Rows.Count > 0 Then
                For J = 0 To dsRunways.Tables(0).Rows.Count - 1
                    If Convert.ToBoolean(dsRunways.Tables(0).Rows(J)("Active")) = False Then
                        lsErrDescription = ""
                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                        dsRunways.Tables(0).Rows(J)("ICAO"), dsRunways.Tables(0).Rows(J)("CITY"), _
                        dsRunways.Tables(0).Rows(J)("RwyId"), dsRunways.Tables(0).Rows(J)("RwyMod"), _
                        "", 0, lsErrDescription, lsModuleId)
                    End If
                Next
            End If

            If dsRunways.Tables(1).Rows.Count > 0 Then
                For J = 0 To dsRunways.Tables(1).Rows.Count - 1
                    If Convert.ToBoolean(dsRunways.Tables(1).Rows(J)("Active")) = False Then
                        lsErrDescription = ""
                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                        dsRunways.Tables(1).Rows(J)("ICAO"), dsRunways.Tables(1).Rows(J)("CITY"), _
                        dsRunways.Tables(1).Rows(J)("RwyId"), dsRunways.Tables(1).Rows(J)("RwyMod"), _
                        dsRunways.Tables(1).Rows(J)("Ident"), 0, lsErrDescription, lsModuleId)
                    End If
                Next
            End If

            If dsRunways.Tables(2).Rows.Count > 0 Then
                For J = 0 To dsRunways.Tables(2).Rows.Count - 1
                    If Convert.ToBoolean(dsRunways.Tables(2).Rows(J)("Active")) = False Then
                        lsErrDescription = ""
                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(loDataChecksResultTable, _
                        dsRunways.Tables(2).Rows(J)("ICAO"), dsRunways.Tables(2).Rows(J)("CITY"), _
                        dsRunways.Tables(2).Rows(J)("RwyId"), dsRunways.Tables(2).Rows(J)("RwyMod"), _
                        dsRunways.Tables(2).Rows(J)("Distance"), 0, lsErrDescription, lsModuleId)
                    End If
                Next
            End If

        End If

        dsRunways = Nothing
        loADMS_BO_Common = Nothing
        loADMS_BAL_RDM = Nothing
        loADMS_BAL_Data_Checks = Nothing
        loAirportsDataTable = Nothing


        Return loDataChecksResultTable
    End Function
#End Region


End Class
