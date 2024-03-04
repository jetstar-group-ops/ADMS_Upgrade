
Imports System.Net
Imports System.Configuration
Imports System.IO
Imports WNB_Admin_DAL

Public Class ADMS_BAL_RDM

#Region "CONSTANTS "
    Const Deg_Rad = Math.PI / 180
    Const Nm_m = 1852.0
    Const Nm_km = 1.852
    Const Ft_m = 0.3048
    Const Ft_km = Ft_m / 1000
    Const Ft_Nm = Ft_km / Nm_km
#End Region


#Region "DB function"

    Public Function GetRunWay(ByVal strUserId As String, ByVal strICAO As String, ByVal intRwyId As String, ByVal RwyMod As String) As DataSet

        Dim ResultRows As DataSet = Nothing
        Dim objDal As New ADMS_DAL_RDM()

        Try
            ResultRows = objDal.Get_Runway(strUserId, strICAO, intRwyId, RwyMod)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

        Return ResultRows

    End Function

    Public Function CreateUpdate_Runway(ByVal strUserId As String, ByVal strICAO As String, ByVal strRwyId As String, _
                     ByVal RwyMod As String, ByVal dsRunwayDetails As DataSet) As Integer

        Dim intResult As Integer = 0
        Dim objDal As New ADMS_DAL_RDM()

        Try
            intResult = objDal.CreateUpdate_Runway(strUserId, strICAO, strRwyId, RwyMod, dsRunwayDetails)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

        Return intResult

    End Function

    Public Function Delete_Runway(ByVal strUserId As String, ByVal strICAO As String, ByVal strRwyId As String, _
                    ByVal RwyMod As String) As Integer

        Dim intResult As Integer = 0
        Dim objDal As New ADMS_DAL_RDM()

        Try
            intResult = objDal.Delete_Runway(strUserId, strICAO, strRwyId, RwyMod)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

        Return intResult

    End Function

#End Region

#Region "Data Check"

    Public Sub RunwayDataCheck(ByRef poDataChecksResultTable As DataTable, _
            ByVal psIcao As String, ByVal psRwyId As String, _
            ByVal psRwyMod As String, ByVal pbValidateObstacles As Boolean, _
            ByVal pbValidateIntersections As Boolean, _
            ByVal poParameterTable As DataTable, ByVal psUserId As String)

        Dim loRunWaysTable As DataTable
        Dim loADMS_BAL_IDM As New ADMS_BAL_IDM
        Dim loADMS_BAL_ODM As New ADMS_BAL_ODM
        Dim loADMS_BO_Common As New ADMS_BAL_Common
        Dim lsModuleId As String = "R"
        Dim lsErrDescription As String = ""
        Dim liTORA As Integer
        Dim liTODA As Integer
        Dim liASDA As Integer
        Dim liLDA As Integer
        Dim liMaxLen As Integer
        Dim liMinLen As Integer
        Dim liMaxSwy As Integer
        Dim liMaxCwy As Integer
        Dim liMaxDispThr As Integer
        Dim liDispThr As Integer
        Dim liDispTo As Integer
        Dim liMaxDispTO As Integer
        Dim liResaTo As Integer
        Dim liMaxRESA As Integer
        Dim liResaLd As Integer
        Dim liGaGrad As Integer
        Dim liMaxGaGrad As Integer
        Dim liWidth, liWidthRecip As Integer
        Dim liMaxWidth As Integer
        Dim liMinWidth As Integer
        Dim liElevStartTORA As Integer
        Dim liMaxAptElev As Integer
        Dim liMinAptElev As Integer
        Dim liMagHdg As Integer
        Dim liMaxMagHdg As Integer
        Dim liMinMagHdg As Integer
        Dim liMaxshoulder As Integer
        Dim liShoulder, liShoulderRecip As Integer
        Dim lbRunwayHasReciprocalRunway As Boolean = False
        Dim lsReciprocalRunwayId As String
        Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
        Dim loReciprocalRunwayTable As DataTable
        Dim LatNumA, LonNumA, LatNumB, LonNumB, Height As Single
        Dim LatNumARwy, LonNumARwy, LatNumBRecpRwy, LonNumBRecpRwy, HeightRecipRwy As Single
        Dim liMaxTORAErr, liZeroLength, liMaxLDAErr, liMaxDeltaElev As Integer
        Dim lsngSlope, lsngMinSlope, lsngMaxSlope, lsngCalcRwySlope, lsngActualRwySlope, lsngMaxDeltaSlope As Single
        Dim liElevStartTORA_Recip, liMaxNumObs, liElevation_Airport As Integer
        Dim lsSlopeDir As String
        Dim loMagVariationForAirportInDegree, lsngDiffOfCalRwySlopNSlope, lsngDiffOfEleStartToraNEleOfAirport As Single
        Dim loObstacle As DataSet
        Dim lbValidLatNumA, lbValidLonNumA, lbValidLatNumB, lbValidLonNumB As Boolean
        Dim lbValidateLatNumBRecpRwy, lbValidateLonNumBRecpRwy, lbValidateAirportMagVar As Boolean

        Dim Answers As ADMS_BAL_Data_Checks.AtoB_Results
        Dim AnswersRecpRwy As ADMS_BAL_Data_Checks.AtoB_Results
        Dim lsMaxAptRwyDis, liMaxHdgErr, lsngCalMagHeading, lsngDiffBtwMagHgdNCalMagHeading As Single
        Dim liMaxIntIdent As Integer

        If poDataChecksResultTable Is Nothing Then
            poDataChecksResultTable = loADMS_BO_Common.GetEmptyDataChecksResultTable
        End If

        If poParameterTable Is Nothing Then
            poParameterTable = loADMS_BO_Common.GetParameterTable(psUserId)
        End If

        loRunWaysTable = GetRunWay(psUserId, psIcao, psRwyId, psRwyMod).Tables(0).Copy

        lsMaxAptRwyDis = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxAptRwyDis")
        liMaxLen = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxLen")
        liMinLen = loADMS_BO_Common.GetParameterValue(poParameterTable, "MinLen")
        liMaxCwy = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxCwy")
        liMaxSwy = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxSwy")
        liMaxDispThr = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDispThr")
        liMaxDispTO = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDispTO")
        liMaxRESA = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxRESA")
        liMaxGaGrad = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxGaGrad")
        liMaxWidth = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxWidth")
        liMinWidth = loADMS_BO_Common.GetParameterValue(poParameterTable, "MinWidth")
        liMaxshoulder = loADMS_BO_Common.GetParameterValue(poParameterTable, "Maxshoulder")
        liMaxAptElev = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxAptElev")
        liMinAptElev = loADMS_BO_Common.GetParameterValue(poParameterTable, "MinAptElev")
        liMaxMagHdg = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxMagHdg")
        liMinMagHdg = loADMS_BO_Common.GetParameterValue(poParameterTable, "MinMagHdg")
        liMaxTORAErr = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxTORAErr")
        liZeroLength = loADMS_BO_Common.GetParameterValue(poParameterTable, "ZeroLength")
        liMaxLDAErr = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxLDAErr")
        lsngMinSlope = loADMS_BO_Common.GetParameterValue(poParameterTable, "MinSlope")
        lsngMaxSlope = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxSlope")
        lsngMaxDeltaSlope = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDeltaSlope")
        liMaxHdgErr = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxHdgErr")
        liMaxNumObs = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxNumObs")
        liMaxDeltaElev = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxDeltaElev")
        liMaxIntIdent = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxIntIdent")



        For I = 0 To loRunWaysTable.Rows.Count - 1

            'OBSTACLE Data check Point no. 1
            ' Obstacle Data check: 1. If the number of obstacles for this runway exceeds ‘MaxNumObs’, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            loObstacle = New DataSet
            loObstacle = loADMS_BAL_ODM.Get_Obstacles(psUserId, 0, loRunWaysTable.Rows(I)("ICAO"), _
                                                      loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"))
            If Not loObstacle Is Nothing Then
                If loObstacle.Tables(0).Rows.Count > liMaxNumObs Then
                    'loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & _
                    lsErrDescription = " Number of obstacles " & loObstacle.Tables(0).Rows.Count & " is greater than " & liMaxNumObs & "."

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                    loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                    loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                    "", 0, lsErrDescription, lsModuleId)
                End If
                loObstacle = Nothing

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '1. Check that the airport has data for at least one runway. If not, log an error.
            ''Above point is implemented in Airport Data Check
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'This point is implemented in Airport Data Checks
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '2. See ‘Appendix 7, (8) Latitude and Longitude’ for background information.
            'Check that the values for LatDir, LatDeg, LatMin, LatSec, LonDir, LonDeg, LonMin and 
            'LonSec for this runway, represent a valid set of earth coordinates. If not, log an error.

            '' validate latitude values
            'loADMS_BO_Common.ValidateLatitude(poDataChecksResultTable, _
            '    loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("City"), _
            '    loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
            '    "", loRunWaysTable.Rows(I)("LatDir"), loRunWaysTable.Rows(I)("LatDeg"), _
            '    loRunWaysTable.Rows(I)("LatMin"), loRunWaysTable.Rows(I)("LatSec"), _
            '    poParameterTable, lsModuleId)

            '' validate longitude values
            'loADMS_BO_Common.ValidateLatitude(poDataChecksResultTable, _
            '    loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("City"), _
            '    loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
            '    "", loRunWaysTable.Rows(I)("LonDir"), loRunWaysTable.Rows(I)("LonDeg"), _
            '    loRunWaysTable.Rows(I)("LonMin"), loRunWaysTable.Rows(I)("LonSec"), _
            '    poParameterTable, lsModuleId)



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '3: .See() 'Appendix 7, (2) Reciprocal Runway’ for background information.
            'Using the function RecipRwy, get the runway identifier for the reciprocal runway. 
            'Set a flag to indicate whether a reciprocal runway exists

            lbRunwayHasReciprocalRunway = False
            lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(loRunWaysTable.Rows(I)("RwyId"))

            loReciprocalRunwayTable = GetRunWay(psUserId, loRunWaysTable.Rows(I)("ICAO"), _
                lsReciprocalRunwayId, "").Tables(0).Copy

            If loReciprocalRunwayTable.Rows.Count > 1 Then
                Dim dtRecipRwy As New DataTable

                loReciprocalRunwayTable.DefaultView.RowFilter = "RwyMod='std'"
                dtRecipRwy = loReciprocalRunwayTable.DefaultView.ToTable()

                If dtRecipRwy.Rows.Count = 0 Then
                    loReciprocalRunwayTable = New DataTable
                    loReciprocalRunwayTable = GetRunWay(psUserId, loRunWaysTable.Rows(I)("ICAO"), _
                        lsReciprocalRunwayId, loRunWaysTable.Rows(I)("RwyMod")).Tables(0).Copy
                Else

                    loReciprocalRunwayTable = New DataTable
                    loReciprocalRunwayTable = dtRecipRwy

                End If

                dtRecipRwy = Nothing

            End If

            If loReciprocalRunwayTable.Rows.Count > 0 Then
                lbRunwayHasReciprocalRunway = True
            Else
                lsErrDescription = "Reciprocal runway does not exist."

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                   loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)

                'YMML Melbourne 16  Std     :  Reciprocal runway does not exist
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '4 Deleted
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '5: .See() 'Appendix 7, (3) Great_CircleAtoB’ for background information.
            'Calculate the Great Circle distance from the beginning of the runway 
            'to the airport reference point using procedure ‘Great_CircleAtoB’. 
            'If the distance is greater than ‘MaxAptRwyDis’, log an error.

            LatNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(loRunWaysTable.Rows(I)("LatDirA"), _
                    loRunWaysTable.Rows(I)("LatDegA"), loRunWaysTable.Rows(I)("LatMinA"), _
                    loRunWaysTable.Rows(I)("LatSecA"), lbValidLatNumA)

            LonNumA = loADMS_BAL_Data_Checks.GetDecimalDegree(loRunWaysTable.Rows(I)("LonDirA"), _
                    loRunWaysTable.Rows(I)("LonDegA"), loRunWaysTable.Rows(I)("LonMinA"), _
                    loRunWaysTable.Rows(I)("LonSecA"), lbValidLonNumA)

            LatNumB = loADMS_BAL_Data_Checks.GetDecimalDegree(loRunWaysTable.Rows(I)("LatDir"), _
                   loRunWaysTable.Rows(I)("LatDeg"), loRunWaysTable.Rows(I)("LatMin"), _
                   loRunWaysTable.Rows(I)("LatSec"), lbValidLatNumB)

            LonNumB = loADMS_BAL_Data_Checks.GetDecimalDegree(loRunWaysTable.Rows(I)("LonDir"), _
                    loRunWaysTable.Rows(I)("LonDeg"), loRunWaysTable.Rows(I)("LonMin"), _
                    loRunWaysTable.Rows(I)("LonSec"), lbValidLonNumB)

            Height = 0

            'Data Check 2.
            '2. See ‘Appendix 7, (8) Latitude and Longitude’ for background information.
            'Check that the values for LatDir, LatDeg, LatMin, LatSec, LonDir, LonDeg, LonMin and 
            'LonSec for this runway, represent a valid set of earth coordinates. If not, log an error.

            If lbValidLatNumB = False Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = "Invalid latitude " & loRunWaysTable.Rows(I)("LatDir") & " " & loRunWaysTable.Rows(I)("LatDeg") & _
                " " & loRunWaysTable.Rows(I)("LatMin") & " " & loRunWaysTable.Rows(I)("LatSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                   loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            If lbValidLonNumB = False Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = "Invalid longitude " & loRunWaysTable.Rows(I)("LonDir") & " " & loRunWaysTable.Rows(I)("LonDeg") & _
                      " " & loRunWaysTable.Rows(I)("LonMin") & " " & loRunWaysTable.Rows(I)("LonSec")

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                   loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If

            If lbValidLatNumB = True And lbValidLonNumB = True Then

                Answers = loADMS_BAL_Data_Checks.Great_CircleAtoB(LatNumA, LonNumA, LatNumB, LonNumB, Height)

                If Answers.Rho > lsMaxAptRwyDis Then
                    'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                    lsErrDescription = "Calculated distance from start of runway to ARP " & Answers.Rho & " exceeds " & lsMaxAptRwyDis & " m"

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                       loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                       loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                       "", 0, lsErrDescription, lsModuleId)
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '6. If the TORA is greater than ZeroLength and if the TORA is greater than MaxLen or less than MinLen log an error.
            liTORA = loRunWaysTable.Rows(I)("TORA") & ""

            If liTORA > liZeroLength And (Val(liTORA) < liMinLen Or Val(liTORA) > liMaxLen) Then
                ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " TORA " & liTORA & " out of range " & liMinLen & " to " & liMaxLen

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                   loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '7.If the TODA is greater than ZeroLength and if the TODA is greater than MaxLen or less than MinLen log an error.
            liTODA = loRunWaysTable.Rows(I)("TODA") & ""

            If liTODA > liZeroLength And (Val(liTODA) < liMinLen Or Val(liTODA) > liMaxLen) Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " TODA " & liTODA & " out of range " & liMinLen & " to " & liMaxLen

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                    loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                    loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                    "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '8.If the ASDA is greater than ZeroLength and if the ASDA is greater than MaxLen or less than MinLen log an error.
            liASDA = loRunWaysTable.Rows(I)("ASDA") & ""

            If liASDA > liZeroLength And (Val(liASDA) < liMinLen Or Val(liASDA) > liMaxLen) Then
                ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " ASDA " & liASDA & " out of range " & liMinLen & " to " & liMaxLen

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                   loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '9.If the LDA is greater than ZeroLength and if the LDA is greater than MaxLen or less than MinLen log an error.
            liLDA = loRunWaysTable.Rows(I)("LDA") & ""

            If liLDA > liZeroLength And (Val(liLDA) < liMinLen Or Val(liLDA) > liMaxLen) Then
                '  lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " LDA " & liLDA & " out of range " & liMinLen & " to " & liMaxLen

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                    loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                    loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                    "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '10. If ASDA is less than TORA, log an error
            If liASDA < liTORA Then
                ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " ASDA " & liASDA & " is less than TORA " & liTORA

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '11. If ASDA minus TORA i.e. Stopway is greater than MaxSwy then log an error.
            If (liASDA - liTORA) > liMaxSwy Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Stopway " & liASDA - liTORA & " is greater than " & liMaxSwy

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '12. If TODA minus TORA i.e. Clearway is greater than MaxCwy then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If (liTODA - liTORA) > liMaxCwy Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Clearway " & liTODA - liTORA & " is greater than " & liMaxCwy

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '13. If DispThr is greater than MaxDispThr then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liDispThr = loRunWaysTable.Rows(I)("DispThr") & ""

            If liDispThr > liMaxDispThr Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Disp Thr " & liDispThr & " out of range 0 to " & liMaxDispThr

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '14. If DispTO is greater than MaxDispTO then log an error
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liDispTo = loRunWaysTable.Rows(I)("DispTO") & ""
            If liDispTo > liMaxDispTO Then
                ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Disp T/O " & liDispTo & " out of range 0 to " & liMaxDispTO

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '15. If ResaTo is greater than MaxRESA then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liResaTo = loRunWaysTable.Rows(I)("ResaTo") & ""
            If liResaTo > liMaxRESA Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Takeoff RESA " & liResaTo & " out of range 0 to " & liMaxRESA

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '16. If ResaLd is greater than MaxRESA then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liResaLd = loRunWaysTable.Rows(I)("ResaLd") & ""
            If liResaLd > liMaxRESA Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Landing RESA " & liResaLd & " out of range 0 to " & liMaxRESA

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '17. If a reciprocal runway exists, calculate the Great Circle distance between the beginning 
            'of this runway and the beginning of the reciprocal runway.
            '     If DispTO plus TORA plus ResaTO minus the calculated distance is 
            'greater than MaxTORAErr and the TORA is greater than ZeroLength, log an error.
            '     If DispThr plus LDA plus ResaLd minus the calculated distance is greater 
            'than MaxLDAErr and the LDA is greater than ZeroLength, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbRunwayHasReciprocalRunway = True Then

                LatNumARwy = loADMS_BAL_Data_Checks.GetDecimalDegree(loRunWaysTable.Rows(I)("LatDir"), _
                    loRunWaysTable.Rows(I)("LatDeg"), loRunWaysTable.Rows(I)("LatMin"), _
                    loRunWaysTable.Rows(I)("LatSec"), lbValidLatNumA)

                LonNumARwy = loADMS_BAL_Data_Checks.GetDecimalDegree(loRunWaysTable.Rows(I)("LonDir"), _
                        loRunWaysTable.Rows(I)("LonDeg"), loRunWaysTable.Rows(I)("LonMin"), _
                        loRunWaysTable.Rows(I)("LonSec"), lbValidLatNumA)

                LatNumBRecpRwy = loADMS_BAL_Data_Checks.GetDecimalDegree(loReciprocalRunwayTable.Rows(0)("LatDir"), _
                       loReciprocalRunwayTable.Rows(0)("LatDeg"), loReciprocalRunwayTable.Rows(0)("LatMin"), _
                       loReciprocalRunwayTable.Rows(0)("LatSec"), lbValidateLatNumBRecpRwy)

                LonNumBRecpRwy = loADMS_BAL_Data_Checks.GetDecimalDegree(loReciprocalRunwayTable.Rows(0)("LonDir"), _
                        loReciprocalRunwayTable.Rows(0)("LonDeg"), loReciprocalRunwayTable.Rows(0)("LonMin"), _
                        loReciprocalRunwayTable.Rows(0)("LonSec"), lbValidateLonNumBRecpRwy)

                HeightRecipRwy = 0

                If lbValidateLatNumBRecpRwy = True And lbValidateLonNumBRecpRwy = True Then
                    AnswersRecpRwy = loADMS_BAL_Data_Checks.Great_CircleAtoB(LatNumARwy, LonNumARwy, LatNumBRecpRwy, LonNumBRecpRwy, HeightRecipRwy)

                    If ((Math.Abs((liDispTo + liTORA + liResaTo) - (AnswersRecpRwy.Rho * Nm_m))) > liMaxTORAErr) And liTORA > liZeroLength Then
                        'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                        lsErrDescription = " Coded DispTO " & liDispTo & " plus TORA " & liTORA & " plus Takeoff RESA " & liResaLd & _
                        " differs from calculated runway length " & Math.Round((AnswersRecpRwy.Rho * Nm_m)) & " by more than " & liMaxTORAErr


                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                           loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                           loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                           "", 0, lsErrDescription, lsModuleId)
                    End If

                    If ((Math.Abs((liDispThr + liLDA + liResaLd) - (AnswersRecpRwy.Rho * Nm_m))) > liMaxLDAErr) And liLDA > liZeroLength Then
                        ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                        lsErrDescription = " Coded DispThr " & liDispThr & " plus LDA " & liLDA & " plus Landing RESA " & liResaLd & _
                            " differs from calculated runway length " & Math.Round((AnswersRecpRwy.Rho * Nm_m)) & " by more than " & liMaxTORAErr

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                           loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                           loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                           "", 0, lsErrDescription, lsModuleId)
                    End If
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '18. See ‘Appendix 7, (7) Runway Slope’ for background information.
            'If SlopeNum is greater than MaxSlope or less than MinSlope then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            lsngSlope = loRunWaysTable.Rows(I)("Slope")
            If (lsngSlope < lsngMinSlope Or lsngSlope > lsngMaxSlope) Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Slope " & lsngSlope & " out of range " & lsngMinSlope & " to " & lsngMaxSlope

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '19. If a reciprocal runway exists, calculate the runway slope as follows:
            'CalcRwySlope = (Elevation at the beginning of the reciprocal runway 
            '           – Elevation at the beginning of this runway)/(DispTO + TORA + ResaTORA) / 0.3048 * 100.0.
            'If the difference between SlopeNum and the calculated slope CalcRwySlope is greater than MaxDeltaSlope then log an error.
            'If the calculated slope is less than zero and SlopeDir is not equal to ‘Dn’ then log an error.
            'If the calculated slope is greater than zero and SlopeDir is not equal to ‘Up’ then log an error
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbRunwayHasReciprocalRunway = True Then
                If liTORA > liZeroLength Then
                    liElevStartTORA_Recip = loReciprocalRunwayTable.Rows(0)("ElevStartTORA")
                    liElevStartTORA = loRunWaysTable.Rows(I)("ElevStartTORA")
                    lsngCalcRwySlope = (liElevStartTORA_Recip - liElevStartTORA) / (liDispTo + liTORA + liResaTo) * 0.3048 * 100.0

                    lsngActualRwySlope = lsngCalcRwySlope
                    lsngCalcRwySlope = Math.Abs(lsngCalcRwySlope)

                    lsngDiffOfCalRwySlopNSlope = lsngCalcRwySlope - lsngSlope
                    If lsngDiffOfCalRwySlopNSlope < 0 Then
                        lsngDiffOfCalRwySlopNSlope = lsngDiffOfCalRwySlopNSlope * -1
                    End If

                    If lsngDiffOfCalRwySlopNSlope > lsngMaxDeltaSlope Then
                        'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                        lsErrDescription = " Calculated Slope " & lsngCalcRwySlope & " differs from coded slope " & lsngSlope & _
                        " by more than " & lsngMaxDeltaSlope

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                             loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                             loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                             "", 0, lsErrDescription, lsModuleId)
                    End If

                    lsSlopeDir = loRunWaysTable.Rows(I)("SlopeDir")
                    If lsngActualRwySlope < 0 And lsSlopeDir <> "Dn" Then
                        ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                        lsErrDescription = " Calculated Slope Direction " & lsngActualRwySlope & " differs from coded slope direction - Up"

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                             loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                             loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                             "", 0, lsErrDescription, lsModuleId)
                    End If
                    If lsngActualRwySlope > 0 And lsSlopeDir <> "Up" Then
                        'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                        lsErrDescription = " Calculated Slope Direction " & lsngActualRwySlope & " differs from coded slope direction - Dn"

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                             loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                             loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                             "", 0, lsErrDescription, lsModuleId)
                    End If
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '20. If GaGrad is greater than MaxGaGrad, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liGaGrad = loRunWaysTable.Rows(I)("GaGrad") & ""
            If liGaGrad > liMaxGaGrad Then
                ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Go around gradient " & liGaGrad & " out of range 0.0 to " & liMaxGaGrad

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '21. If Width is less than MinWidth or greater than MaxWidth, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liWidth = loRunWaysTable.Rows(I)("Width") & ""
            If (liWidth < liMinWidth Or liWidth > liMaxWidth) Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Width " & liWidth & " out of range " & liMinWidth & " to " & liMaxWidth

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '22. If a reciprocal runway exists and the Width of the reciprocal runway 
            ' is not equal to the Width of this runway, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbRunwayHasReciprocalRunway = True Then
                liWidthRecip = loReciprocalRunwayTable.Rows(0)("Width")

                If liWidth <> liWidthRecip Then
                    ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                    lsErrDescription = " Runway width " & liWidth & " is different to that of reciprocal runway "

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                         loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                         loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                         "", 0, lsErrDescription, lsModuleId)
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '23. If Shoulder is less than zero or greater than MaxShoulder, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liShoulder = loRunWaysTable.Rows(I)("Shoulder") & ""
            If liShoulder < 0 Or liShoulder > liMaxshoulder Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Shoulder " & liShoulder & " is out of range 0 to " & liMaxshoulder

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '24. If a reciprocal runway exists and the Shoulder of the reciprocal 
            'runway is not equal to the Shoulder of this runway, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbRunwayHasReciprocalRunway = True Then
                liShoulderRecip = loReciprocalRunwayTable.Rows(0)("Shoulder") & ""

                If liShoulder <> liShoulderRecip Then
                    ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                    lsErrDescription = " Shoulder width " & liShoulder & " is different to that of reciprocal runway"

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                         loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                         loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                         "", 0, lsErrDescription, lsModuleId)
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '25. If ElevStartTORA is less than MinAptElev or greater than MaxAptElev, then log an error.
            'If the difference between ElevStartTORA and the elevation of the Airport 
            'Reference Point is greater than MaxDeltaElev, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If (liElevStartTORA < liMinAptElev Or liElevStartTORA > liMaxAptElev) Then
                '  lsErrDescription =lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Elevation at Start of TORA " & liElevStartTORA & " out of range " & liMinAptElev & " to " & liMaxAptElev

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            liElevation_Airport = loRunWaysTable.Rows(I)("Elevation")
            lsngDiffOfEleStartToraNEleOfAirport = liElevStartTORA - liElevation_Airport
            If lsngDiffOfEleStartToraNEleOfAirport < 0 Then
                lsngDiffOfEleStartToraNEleOfAirport = lsngDiffOfEleStartToraNEleOfAirport * -1
            End If
            If lsngDiffOfEleStartToraNEleOfAirport > liMaxDeltaElev Then
                ' lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Difference between Elevation at Start of TORA " & liElevStartTORA & _
                 " and Airport Elevation " & liElevation_Airport & " more than " & liMaxDeltaElev

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '26. If MagHdg is less than MinMagHdg or greater than MaxMagHdg, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liMagHdg = loRunWaysTable.Rows(I)("MagHdg") & ""

            If (liMagHdg < liMinMagHdg Or liMagHdg > liMaxMagHdg) Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " Magnetic Heading " & liMagHdg & " out of range " & liMinMagHdg & " to " & liMaxMagHdg

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '27: See() 'Appendix 7, (3) Great_CircleAtoB’ and ‘(5) TrueToMag’ for background information.
            'If a reciprocal runway exists, calculate the Great Circle true heading between the beginning of this
            ' runway and the beginning of the reciprocal runway.
            'Covert the calculated true heading to a calculated magnetic heading, using the function ‘TrueToMag’ and the 
            'magnetic variation calculated for the Airport reference Point.
            'If the difference between MagHdg (on Runway Screen) and the calculated magnetic heading is greater than MaxHdgErr, then log an error.
            'Great Circle true heading = AnswersRecpRwy.TrackA (2nd Output value of MagVar function)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If lbRunwayHasReciprocalRunway = True Then

                Dim loMagVar_Results As ADMS_BAL_Data_Checks.MagVar_Results

                loMagVar_Results = loADMS_BAL_Data_Checks.Get_ADM_MagVarDataCheck(loRunWaysTable.Rows(I)("LatDirA"), _
                         loRunWaysTable.Rows(I)("LatDegA"), loRunWaysTable.Rows(I)("LatMinA"), _
                         loRunWaysTable.Rows(I)("LatSecA"), loRunWaysTable.Rows(I)("LonDirA"), _
                         loRunWaysTable.Rows(I)("LonDegA"), loRunWaysTable.Rows(I)("LonMinA"), _
                         loRunWaysTable.Rows(I)("LonSecA"), 0, _
                         Date.Now.Year)

                If loMagVar_Results.Err <> 0 Then
                    'Commneted below line bcoz User doesnt required message that is Get_ADM_MagVarDataCheck()->MagVar() method throwing
                    'lsErrDescription = loMagVar_Results.ErrStr

                    'loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                    '     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                    '     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                    '     "", 0, lsErrDescription, lsModuleId)
                Else
                    Dim MVar As Single
                    Dim tbMagDir As String
                    Dim Deg As Single
                    Dim Min As Single

                    MVar = loMagVar_Results.Dek

                    If MVar < 0.0 Then
                        tbMagDir = "W"
                    Else
                        tbMagDir = "E"
                    End If
                    MVar = Math.Abs(MVar)
                    Deg = Math.Truncate(MVar)
                    MVar = (MVar - Deg) * 60.0
                    Min = Math.Truncate(MVar)
                    MVar = (MVar - Min) * 60.0

                    Dim tsMagDeg As String
                    Dim tsMagMin As String
                    Dim tsMagSec As String

                    tsMagDeg = Convert.ToString(Deg)
                    tsMagMin = Convert.ToString(Min)
                    tsMagSec = Convert.ToString(MVar)

                    loMagVariationForAirportInDegree = loADMS_BAL_Data_Checks.GetDecimalDegree(tbMagDir, tsMagDeg, tsMagMin, tsMagSec, lbValidateAirportMagVar)

                End If

                If lbValidateAirportMagVar = True Then
                    ' TrueToMag() is to calculate the GreatCircleDistanceAtoB
                    lsngCalMagHeading = loADMS_BAL_Data_Checks.TrueToMag(AnswersRecpRwy.TrackA, loMagVariationForAirportInDegree)

                    lsngDiffBtwMagHgdNCalMagHeading = (liMagHdg - lsngCalMagHeading)
                    If lsngDiffBtwMagHgdNCalMagHeading < 0 Then
                        lsngDiffBtwMagHgdNCalMagHeading = lsngDiffBtwMagHgdNCalMagHeading * -1
                    End If

                    lsngDiffBtwMagHgdNCalMagHeading = Math.Min(lsngDiffBtwMagHgdNCalMagHeading, (360 - lsngDiffBtwMagHgdNCalMagHeading))

                    If lsngDiffBtwMagHgdNCalMagHeading > liMaxHdgErr Then
                        'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                        lsErrDescription = " Calculated runway heading " & lsngCalMagHeading & " differs from coded heading " & _
                       liMagHdg & " by more than  " & liMaxHdgErr

                        loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                             loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                             loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                             "", 0, lsErrDescription, lsModuleId)
                    End If
                End If
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '28. If TODA is less than TORA log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If liTODA < liTORA Then
                'lsErrDescription = loRunWaysTable.Rows(I)("RwyId") & " " & loRunWaysTable.Rows(I)("RwyMod") & ":" & _
                lsErrDescription = " TODA " & liASDA & " is less than TORA " & liTORA

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '29. Check number of rows of runway comments as follows.''''''''''''''''''''''
            'Build the string variable TotalCommentString as defined in – Airport Manager
            'If the number of rows in TotalCommentString is greater than 9 then log an error message.

            'loRunWaysTable.Rows(I)("ElevStartTORA")
            Dim lsTotalCommentString, AEStr, EOStr, PostStr, CmtStr As String

            lsTotalCommentString = ""

            If IsDBNull(loRunWaysTable.Rows(I)("AEProc")) = True Then
                AEStr = ""
            ElseIf loRunWaysTable.Rows(I)("AEProc") = "" Then
                AEStr = ""
            ElseIf IsDBNull(loRunWaysTable.Rows(I)("AEProc")) = False Then
                If loRunWaysTable.Rows(I)("AEProc") <> "" Then
                    AEStr = "All Engines:" & vbCrLf & loRunWaysTable.Rows(I)("AEProc")
                End If
            End If

            If IsDBNull(loRunWaysTable.Rows(I)("EOProc")) = True Then
                EOStr = ""
            ElseIf loRunWaysTable.Rows(I)("EOProc") = "" Then
                EOStr = ""
            ElseIf IsDBNull(loRunWaysTable.Rows(I)("EOProc")) = False Then
                If loRunWaysTable.Rows(I)("EOProc") <> "" Then
                    EOStr = vbCrLf & "EOP:" & vbCrLf & loRunWaysTable.Rows(I)("EOProc")
                End If
            End If

            If IsDBNull(loRunWaysTable.Rows(I)("PostProc")) = True Then
                PostStr = ""
            ElseIf loRunWaysTable.Rows(I)("PostProc") = "" Then
                PostStr = ""
            ElseIf IsDBNull(loRunWaysTable.Rows(I)("PostProc")) = False Then
                If loRunWaysTable.Rows(I)("PostProc") <> "" Then
                    PostStr = vbCrLf & "Post T/O:" & vbCrLf & loRunWaysTable.Rows(I)("PostProc")
                End If
            End If

            If IsDBNull(loRunWaysTable.Rows(I)("Comment")) = True Then
                CmtStr = ""
            ElseIf loRunWaysTable.Rows(I)("Comment") = "" Then
                CmtStr = ""
            ElseIf IsDBNull(loRunWaysTable.Rows(I)("Comment")) = False Then
                If loRunWaysTable.Rows(I)("Comment") <> "" Then
                    CmtStr = vbCrLf & "Comment:" & vbCrLf & loRunWaysTable.Rows(I)("Comment")
                End If
            End If

            lsTotalCommentString = AEStr & EOStr & PostStr & CmtStr

            Dim strTtlCommNoOfLines() As String = lsTotalCommentString.Split(vbLf)
            Dim intNoOfLines As Integer = 0
            If strTtlCommNoOfLines.Count > 0 Then
                For iCmtCnter = 0 To strTtlCommNoOfLines.Count - 1
                    intNoOfLines = intNoOfLines + 1
                Next
            End If

            If intNoOfLines > 9 Then
                lsErrDescription = " Number of rows of airport comments is greater than 9 – AirportMax limit."

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '30. If the length of ‘RwyMod’ is greater then ‘MaxIntIdent’ then raise an error message.
            'For example.
            'YMML Melbourne      :  34  E12345:  Length of RwyMod exceeds 5 characters 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Len(Convert.ToString(loRunWaysTable.Rows(I)("RwyMod")).Trim()) > liMaxIntIdent Then
                lsErrDescription = " Length of RwyMod exceeds " & liMaxIntIdent & " characters."

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                     loRunWaysTable.Rows(I)("ICAO"), loRunWaysTable.Rows(I)("CITY"), _
                     loRunWaysTable.Rows(I)("RwyId"), loRunWaysTable.Rows(I)("RwyMod"), _
                     "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



        Next

        If pbValidateIntersections = True Then
            loADMS_BAL_IDM.IntersectionDataCheck(poDataChecksResultTable, _
                    psIcao, psRwyId, psRwyMod, poParameterTable, psUserId)
        End If

        If pbValidateObstacles = True Then
            loADMS_BAL_ODM.ObstacleDataCheck(poDataChecksResultTable, _
                   psIcao, psRwyId, psRwyMod, poParameterTable, psUserId)
        End If

        loADMS_BAL_IDM = Nothing
        loADMS_BAL_ODM = Nothing
        loADMS_BO_Common = Nothing


    End Sub



#End Region


End Class
