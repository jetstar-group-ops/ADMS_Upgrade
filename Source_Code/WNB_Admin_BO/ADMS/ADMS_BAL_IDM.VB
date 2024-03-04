Imports System.Net
Imports System.Configuration
Imports System.IO
Imports WNB_Admin_DAL

Public Class ADMS_BAL_IDM

#Region "CONSTANTS "
    Const Deg_Rad = Math.PI / 180
    Const Nm_m = 1852.0
    Const Nm_km = 1.852
    Const Ft_m = 0.3048
    Const Ft_km = Ft_m / 1000
    Const Ft_Nm = Ft_km / Nm_km
#End Region

    Dim goADMS_DAL_IDM As New ADMS_DAL_IDM

    Public Function GetAllIntersections(ByVal psUserId As String, _
                                       ByVal psIcao As String, _
                                       ByVal psRwyId As String, _
                                       ByVal psRwyMod As String) As DataTable

        Return goADMS_DAL_IDM.GetAllIntersections(psUserId, psIcao, psRwyId, psRwyMod)

    End Function

    Public Function GetIntersectionDetails(ByVal psUserId As String, _
                                     ByVal psIcao As String, _
                                     ByVal psRwyId As String, _
                                     ByVal psRwyMod As String, _
                                     ByVal piIntersectionId As Integer) As DataSet

        Return goADMS_DAL_IDM.GetIntersectionDetails(psUserId, psIcao, psRwyId, psRwyMod, piIntersectionId)

    End Function

    Public Function CreateUpdateIntersection(ByVal poIntersectionDetails As DataSet, _
                                    ByVal psUserId As String, _
                                    ByVal psIcao As String, _
                                     ByVal psRwyId As String, _
                                       ByVal psRwyMod As String, _
                                       ByVal psIntersectionId As String) As Integer

        Return goADMS_DAL_IDM.CreateUpdateIntersection(poIntersectionDetails, psUserId, psIcao, _
                                                       psRwyId, psRwyMod, psIntersectionId)

    End Function

    Public Function DeleteIntersection(ByVal psUserId As String, _
                                    ByVal psIcao As String, _
                                      ByVal psRwyId As String, _
                                       ByVal psRwyMod As String, _
                                       ByVal piIntersectionId As Integer) As Integer

        Return goADMS_DAL_IDM.DeleteIntersection(psUserId, psIcao, psRwyId, psRwyMod, piIntersectionId)

    End Function

#Region "Data Checks"

    Public Sub IntersectionDataCheck(ByRef poDataChecksResultTable As DataTable, _
           ByVal psIcao As String, ByVal psRwyId As String, _
           ByVal psRwyMod As String, _
           ByVal poParameterTable As DataTable, _
           ByVal psUserId As String)

        Dim loIntersectionTable As DataTable
        Dim loADMS_BAL_IDM As New ADMS_BAL_IDM
        Dim loADMS_BAL_ODM As New ADMS_BAL_ODM
        Dim loADMS_BO_Common As New ADMS_BAL_Common
        Dim lsModuleId As String = "I"
        Dim lsIdent As String
        Dim liMaxIntIdent, liTora, liDeltaFieldLength, liDispTo As Integer
        Dim liMinLen, liMaxLen, liMaxDispErr As Integer
        Dim lsErrDescription As String = ""
        Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
        Dim Answers As ADMS_BAL_Data_Checks.AtoB_Results
        Dim Answers_Recip As ADMS_BAL_Data_Checks.AtoB_Results
        Dim LatNumA, LonNumA, LatNumB_Intsec, LonNumB_Intsec, Height As Single
        Dim LatNumR, LonNumR As Single
        Dim lsMaxAptRwyDis As Single
        Dim lbRunwayHasReciprocalRunway As Boolean = False
        Dim lsReciprocalRunwayId As String
        Dim loReciprocalRunwayTable As DataTable
        Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
        Dim liElevStartTORA_RecipRwy, liElevStartTORA, liElevStartTORA_Rwy As Integer
        Dim lsngCalcIntersectionSlope, lsngCalcRunwaySlope, lsngDiffOfIntRwySlop, lsngMaxDeltaSlope As Single
        Dim lsngDiffOfEleOfRwyNInter, lsngMaxDeltaInElev As Single
        Dim liResaTo, liZeroLength As Integer
        Dim lbValidateLatNumA, lbValidateLonNumA As Boolean
        Dim lbValidateLatNumB_Intsec, lbValidateLonNumB_Intsec As Boolean
        Dim lbValidLatNumR, lbValidLonNumR As Boolean


        If poDataChecksResultTable Is Nothing Then
            poDataChecksResultTable = loADMS_BO_Common.GetEmptyDataChecksResultTable
        End If

        If poParameterTable Is Nothing Then
            poParameterTable = loADMS_BO_Common.GetParameterTable(psUserId)
        End If

        loIntersectionTable = GetAllIntersections(psUserId, psIcao, psRwyId, psRwyMod)

        liMaxIntIdent = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxIntIdent")
        lsMaxAptRwyDis = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxAptRwyDis")
        liMaxLen = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxLen")
        liMinLen = loADMS_BO_Common.GetParameterValue(poParameterTable, "MinLen")
        liMaxDispErr = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDispErr")
        lsngMaxDeltaSlope = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDeltaSlope")
        lsngMaxDeltaInElev = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDeltaInElev")
        liZeroLength = loADMS_BO_Common.GetParameterValue(poParameterTable, "ZeroLength")

        For I = 0 To loIntersectionTable.Rows.Count - 1

            'CHANGED--1. If the length of ‘Ident’ is greater then ‘MaxIntIdent’, then raise an error message.

            '1.If the length of ‘RwyMod’ + ‘Ident’ is greater then ‘MaxIntIdent - 1’ then raise an error message
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'THIS CHECK IS COMMENTED AS PER SPECIFICATION DOCUMENT REV.S
            'lsIdent = loIntersectionTable.Rows(I)("RwyMod") + loIntersectionTable.Rows(I)("Ident") & ""
            'If Len(lsIdent) > (liMaxIntIdent - 1) Then
            '    'lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
            '    lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
            '   " Intersection Ident  " & lsIdent & " exceeds " & (liMaxIntIdent - 1) & " characters"

            '    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
            '       loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
            '       loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
            '       "", 0, lsErrDescription, lsModuleId)
            'End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '2. Check that the values for LatDir, LatDeg, LatMin, LatSec, LonDir, LonDeg, 
            'LonMin and LonSec for this Intersection, represent a valid set of earth coordinates. If not, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' validate latitude values
            'loADMS_BO_Common.ValidateLatitude(poDataChecksResultTable, _
            '    loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("City"), _
            '    loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
            '    "", loIntersectionTable.Rows(I)("LatDir"), loIntersectionTable.Rows(I)("LatDeg"), _
            '    loIntersectionTable.Rows(I)("LatMin"), loIntersectionTable.Rows(I)("LatSec"), _
            '    poParameterTable, lsModuleId)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '' validate longitude values
            'loADMS_BO_Common.ValidateLatitude(poDataChecksResultTable, _
            '    loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("City"), _
            '    loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
            '    "", loIntersectionTable.Rows(I)("LonDir"), loIntersectionTable.Rows(I)("LonDeg"), _
            '    loIntersectionTable.Rows(I)("LonMin"), loIntersectionTable.Rows(I)("LonSec"), _
            '    poParameterTable, lsModuleId)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '3. Calculate the Great Circle distance from this intersection position to the Airport 
            'Reference Point using procedure ‘Great_CircleAtoB’. If the distance is 
            'greater than ‘MaxAptRwyDis’, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            LatNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LatDirA"), _
                   loIntersectionTable.Rows(I)("LatDegA"), loIntersectionTable.Rows(I)("LatMinA"), _
                   loIntersectionTable.Rows(I)("LatSecA"), lbValidateLatNumA)

            LonNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LonDirA"), _
                    loIntersectionTable.Rows(I)("LonDegA"), loIntersectionTable.Rows(I)("LonMinA"), _
                    loIntersectionTable.Rows(I)("LonSecA"), lbValidateLonNumA)

            LatNumB_Intsec = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LatDir"), _
                   loIntersectionTable.Rows(I)("LatDeg"), loIntersectionTable.Rows(I)("LatMin"), _
                   loIntersectionTable.Rows(I)("LatSec"), lbValidateLatNumB_Intsec)

            LonNumB_Intsec = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LonDir"), _
                    loIntersectionTable.Rows(I)("LonDeg"), loIntersectionTable.Rows(I)("LonMin"), _
                    loIntersectionTable.Rows(I)("LonSec"), lbValidateLonNumB_Intsec)

            Height = 0

            '2. Check that the values for LatDir, LatDeg, LatMin, LatSec, LonDir, LonDeg, 
            'LonMin and LonSec for this Intersection, represent a valid set of earth coordinates. If not, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbValidateLatNumB_Intsec = False Then
                ' lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                 " Invalid latitude " & loIntersectionTable.Rows(I)("LatDir") & " " & loIntersectionTable.Rows(I)("LatDeg") & _
                 " " & loIntersectionTable.Rows(I)("LatMin") & " " & loIntersectionTable.Rows(I)("LatSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                   loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            If lbValidateLonNumB_Intsec = False Then
                ' lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                     " Invalid longitude " & loIntersectionTable.Rows(I)("LonDir") & " " & loIntersectionTable.Rows(I)("LonDeg") & _
                     " " & loIntersectionTable.Rows(I)("LonMin") & " " & loIntersectionTable.Rows(I)("LonSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                   loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If lbValidateLatNumB_Intsec = True And lbValidateLonNumB_Intsec = True Then

                Answers = loADMS_BAL_Data_Checks.Great_CircleAtoB(LatNumA, LonNumA, LatNumB_Intsec, LonNumB_Intsec, Height)

                If Answers.Rho > lsMaxAptRwyDis Then
                    '  lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                    lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                   " Calculated distance from intersection to ARP " & Math.Round(Answers.Rho) & " exceeds " & Math.Round(lsMaxAptRwyDis) & " m"

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                       loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                       loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                       "", 0, lsErrDescription, lsModuleId)
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '4. If the TORA for this runway minus DeltaFieldLength (ie Remaining Runway) is is less than MinLen or greater then MaxLen then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liTora = loIntersectionTable.Rows(I)("TORA")
            liDeltaFieldLength = loIntersectionTable.Rows(I)("DeltaFieldLength")

            If Not ((liTora - liDeltaFieldLength) > liMinLen And (liTora - liDeltaFieldLength) < liMaxLen) Then
                ' lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                   " Remaining Runway Length " & liTora - liDeltaFieldLength & " out of range " & liMinLen & " to" & liMaxLen

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                   loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '5. If a reciprocal runway exists, using procedure ‘Great_CircleAtoB, calculate the Great Circle distance 
            'between the beginning of this runway and the position of the Intersection
            'If DispTO for this runway plus DeltaFieldLength minus the 
            ' calculated distance is greater than MaxDispErr, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            lbRunwayHasReciprocalRunway = False
            lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(loIntersectionTable.Rows(I)("RwyId"))
            loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, loIntersectionTable.Rows(I)("ICAO"), _
                lsReciprocalRunwayId, "").Tables(0).Copy
            If loReciprocalRunwayTable.Rows.Count > 0 Then
                lbRunwayHasReciprocalRunway = True
            End If

            If lbRunwayHasReciprocalRunway = True Then

                LatNumR = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LatDirR"), _
                   loIntersectionTable.Rows(I)("LatDegR"), loIntersectionTable.Rows(I)("LatMinR"), _
                   loIntersectionTable.Rows(I)("LatSecR"), lbValidLatNumR)

                LonNumR = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LonDirR"), _
                        loIntersectionTable.Rows(I)("LonDegR"), loIntersectionTable.Rows(I)("LonMinR"), _
                        loIntersectionTable.Rows(I)("LonSecR"), lbValidLonNumR)

                LatNumB_Intsec = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LatDir"), _
                       loIntersectionTable.Rows(I)("LatDeg"), loIntersectionTable.Rows(I)("LatMin"), _
                       loIntersectionTable.Rows(I)("LatSec"), lbValidateLatNumB_Intsec)

                LonNumB_Intsec = loADMS_BAL_Data_Checks.GetDecimalDegree(loIntersectionTable.Rows(I)("LonDir"), _
                        loIntersectionTable.Rows(I)("LonDeg"), loIntersectionTable.Rows(I)("LonMin"), _
                        loIntersectionTable.Rows(I)("LonSec"), lbValidateLonNumB_Intsec)

                Height = 0

                If lbValidateLatNumB_Intsec = True And lbValidateLonNumB_Intsec = True Then
                    Answers_Recip = loADMS_BAL_Data_Checks.Great_CircleAtoB(LatNumR, LonNumR, LatNumB_Intsec, LonNumB_Intsec, Height)

                    liDispTo = loIntersectionTable.Rows(I)("DispTo") & ""

                    If Math.Abs((liDispTo + liDeltaFieldLength - (Answers_Recip.Rho * Nm_m))) > liMaxDispErr Then
                        ' lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                        lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                        " Coded DispTO " & liDispTo & " plus Delta FL " & liDeltaFieldLength & _
                        " differs from calculated displacement " & Math.Round((Answers_Recip.Rho * Nm_m)) & " by more than " & liMaxDispErr

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                                          loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                                          loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                                          "", 0, lsErrDescription, lsModuleId)
                    End If
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '6. If a reciprocal runway exists calculate and TORA is greater than ZeroLength, calculate the intersection slope as follows:
            'CalcIntSlope = (Elevation at the beginning of the reciprocal runway – 
            '       Elevation at this Intersection)/(TORA - DeltaFieldLength) / 0.3048 * 100.0.
            'If the difference between CalcIntSlope and the calculated runway slope CalcRwySlope is greater than MaxDeltaSlope then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbRunwayHasReciprocalRunway = True Then
                If liTora > liZeroLength Then
                    liElevStartTORA_RecipRwy = loReciprocalRunwayTable.Rows(0)("ElevStartTORA")
                    liElevStartTORA = loIntersectionTable.Rows(I)("ElevStartTORA")
                    lsngCalcIntersectionSlope = (liElevStartTORA_RecipRwy - liElevStartTORA) / (liTora - liDeltaFieldLength) * 0.3048 * 100.0

                    liElevStartTORA_Rwy = loIntersectionTable.Rows(I)("ElevStartTORA_Rwy")
                    liResaTo = loIntersectionTable.Rows(I)("ResaTo")
                    lsngCalcRunwaySlope = (liElevStartTORA_RecipRwy - liElevStartTORA_Rwy) / (liDispTo + liTora + liResaTo) * 0.3048 * 100.0

                    lsngDiffOfIntRwySlop = lsngCalcIntersectionSlope - lsngCalcRunwaySlope

                    If lsngDiffOfIntRwySlop < 0 Then
                        lsngDiffOfIntRwySlop = lsngDiffOfIntRwySlop * -1
                    End If

                    If lsngDiffOfIntRwySlop > lsngMaxDeltaSlope Then
                        'lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                        lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                         " Difference between calculated slope to intersection  " & lsngCalcIntersectionSlope & _
                         " and calculated runway slope  " & lsngCalcRunwaySlope & _
                         " more than " & lsngMaxDeltaSlope

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                                          loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                                          loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                                          "", 0, lsErrDescription, lsModuleId)
                    End If
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '7. If the difference between the elevation at the beginning of the runway and 
            'the elevation at the intersection is greater than MaxDeltaInElev’, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            lsngDiffOfEleOfRwyNInter = liElevStartTORA_Rwy - liElevStartTORA

            If lsngDiffOfEleOfRwyNInter < 0 Then
                lsngDiffOfEleOfRwyNInter = lsngDiffOfEleOfRwyNInter * -1
            End If
            If lsngDiffOfEleOfRwyNInter > lsngMaxDeltaInElev Then
                'lsErrDescription = loIntersectionTable.Rows(I)("RwyId") & " " & loIntersectionTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loIntersectionTable.Rows(I)("Ident") & ":" & _
                     " Difference between Elevation at Start of TORA " & liElevStartTORA_Rwy & _
                     " and Intersection Elevation  " & liElevStartTORA & _
                     " more than " & lsngMaxDeltaInElev

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                                  loIntersectionTable.Rows(I)("ICAO"), loIntersectionTable.Rows(I)("CITY"), _
                                  loIntersectionTable.Rows(I)("RwyId"), loIntersectionTable.Rows(I)("RwyMod"), _
                                  "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Next


        loADMS_BAL_IDM = Nothing
        loADMS_BAL_ODM = Nothing
        loADMS_BO_Common = Nothing
        loADMS_BAL_RDM = Nothing
        loADMS_BAL_Data_Checks = Nothing


    End Sub

#End Region

End Class
