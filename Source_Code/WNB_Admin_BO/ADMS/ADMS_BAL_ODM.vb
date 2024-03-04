Imports System.Net
Imports System.Configuration
Imports System.IO
Imports WNB_Admin_DAL

Public Class ADMS_BAL_ODM

    Public Function Get_Obstacles(ByVal psUserId As String, ByVal intObstacleId As Integer, _
                                  ByVal psICAO As String, ByVal intRwyId As String, _
                                  ByVal psRwyMod As String) As DataSet

        Dim ResultRows As DataSet = Nothing
        Dim objDal As New ADMS_DAL_ODM

        Try
            ResultRows = objDal.Get_Obstacles(psUserId, intObstacleId, psICAO, intRwyId, psRwyMod)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

        Return ResultRows

    End Function

    Public Function CreateUpdate_Obstacles(ByVal psUserId As String, ByVal intObstacleId As Integer, _
                    ByVal psICAO As String, ByVal psRwyId As String, _
                     ByVal psRwyMod As String, ByVal PodsObstacleDetails As DataSet) As Integer

        Dim intResult As Integer = 0
        Dim objDal As New ADMS_DAL_ODM

        Try
            intResult = objDal.CreateUpdate_Obstacles(psUserId, intObstacleId, _
                            psICAO, psRwyId, psRwyMod, PodsObstacleDetails)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

        Return intResult

    End Function

    Public Function Delete_Obstacle(ByVal psUserId As String, ByVal intObstacleId As Integer, _
                        ByVal psCAO As String, ByVal psRwyId As String, _
                        ByVal psRwyMod As String) As Integer

        Dim intResult As Integer = 0
        Dim objDal As New ADMS_DAL_ODM

        Try
            intResult = objDal.Delete_Obstacle(psUserId, intObstacleId, psCAO, psRwyId, psRwyMod)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

        Return intResult

    End Function


#Region "Data Checks"

    Public Sub ObstacleDataCheck(ByRef poDataChecksResultTable As DataTable, _
           ByVal psIcao As String, ByVal psRwyId As String, _
           ByVal psRwyMod As String, _
           ByVal poParameterTable As DataTable, _
           ByVal psUserId As String)


        Dim loObstaclesTable As DataTable
        Dim loADMS_BAL_IDM As New ADMS_BAL_IDM
        Dim loADMS_BAL_ODM As New ADMS_BAL_ODM
        Dim loADMS_BO_Common As New ADMS_BAL_Common
        Dim lsModuleId As String = "O"
        Dim lsErrDescription As String = ""
        Dim liDistance, liMaxObsDis, liTODA As Integer
        Dim lbRunwayHasReciprocalRunway As Boolean = False
        Dim lsReciprocalRunwayId As String
        Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
        Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
        Dim loReciprocalRunwayTable As DataTable
        Dim liElevStartTORA_RecipRwy, liElevStartTORA_Rwy, liElevation, liMaxObsHt As Integer
        Dim dtObstacles As DataTable
        Dim intPrevElevation As Integer
        Dim lsPrevICAO, lsPrevRwyId, lsPrevRwyMod As String


        If poDataChecksResultTable Is Nothing Then
            poDataChecksResultTable = loADMS_BO_Common.GetEmptyDataChecksResultTable
        End If

        If poParameterTable Is Nothing Then
            poParameterTable = loADMS_BO_Common.GetParameterTable(psUserId)
        End If

        loObstaclesTable = Get_Obstacles(psUserId, 0, psIcao, psRwyId, psRwyMod).Tables(0).Copy
        liMaxObsDis = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxObsDis")
        liMaxObsHt = loADMS_BO_Common.GetParameterValue(poParameterTable, "MaxObsHt")



        For I = 0 To loObstaclesTable.Rows.Count - 1

            '1. If the number of obstacles for this runway exceeds ‘MaxNumObs’, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'This point is implemented in Runway Data Checks 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '2. If the obstacle Distance is greater than MaxObsDis then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liDistance = loObstaclesTable.Rows(I)("Distance") & ""
            If liDistance > liMaxObsDis Then
                'lsErrDescription = loObstaclesTable.Rows(I)("RwyId") & " " & loObstaclesTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loObstaclesTable.Rows(I)("Distance") & ":" & _
                " Obstacle Distance " & liDistance & " is greater than " & liMaxObsDis

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loObstaclesTable.Rows(I)("ICAO"), loObstaclesTable.Rows(I)("CITY"), _
                   loObstaclesTable.Rows(I)("RwyId"), loObstaclesTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)

            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '3. If the obstacle Distance is less than the TODA for this runway, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liTODA = loObstaclesTable.Rows(I)("TODA") & ""

            If liDistance < liTODA Then
                ' lsErrDescription = loObstaclesTable.Rows(I)("RwyId") & " " & loObstaclesTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loObstaclesTable.Rows(I)("Distance") & ":" & _
                   " Obstacle at " & liDistance & " is within the TODA " & liTODA

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loObstaclesTable.Rows(I)("ICAO"), loObstaclesTable.Rows(I)("CITY"), _
                   loObstaclesTable.Rows(I)("RwyId"), loObstaclesTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'IS IT REQUIRED AS IT IS NOT IN REQUIREMENT DOCUMENT
            '' validate latitude values
            'loADMS_BO_Common.ValidateLatitude(poDataChecksResultTable, _
            '    loObstaclesTable.Rows(I)("ICAO"), loObstaclesTable.Rows(I)("City"), _
            '    loObstaclesTable.Rows(I)("RwyId"), loObstaclesTable.Rows(I)("RwyMod"), _
            '    "", loObstaclesTable.Rows(I)("LatDir"), loObstaclesTable.Rows(I)("LatDeg"), _
            '    loObstaclesTable.Rows(I)("LatMin"), loObstaclesTable.Rows(I)("LatSec"), _
            '    poParameterTable, lsModuleId)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '' validate longitude values
            'loADMS_BO_Common.ValidateLatitude(poDataChecksResultTable, _
            '    loObstaclesTable.Rows(I)("ICAO"), loObstaclesTable.Rows(I)("City"), _
            '    loObstaclesTable.Rows(I)("RwyId"), loObstaclesTable.Rows(I)("RwyMod"), _
            '    "", loObstaclesTable.Rows(I)("LonDir"), loObstaclesTable.Rows(I)("LonDeg"), _
            '    loObstaclesTable.Rows(I)("LonMin"), loObstaclesTable.Rows(I)("LonSec"), _
            '    poParameterTable, lsModuleId)
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '4. If a reciprocal runway exists, obtain the elevation at the beginning of the reciprocal runway. 
            'If the obstacle Elevation is less then the elevation at the beginning of 
            'the reciprocal runway, then log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            lbRunwayHasReciprocalRunway = False
            lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(loObstaclesTable.Rows(I)("RwyId"))
            loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(psUserId, loObstaclesTable.Rows(I)("ICAO"), _
                lsReciprocalRunwayId, "").Tables(0).Copy
            If loReciprocalRunwayTable.Rows.Count > 0 Then
                lbRunwayHasReciprocalRunway = True
            End If

            If lbRunwayHasReciprocalRunway = True Then
                liElevStartTORA_RecipRwy = loReciprocalRunwayTable.Rows(0)("ElevStartTORA")
                liElevation = loObstaclesTable.Rows(I)("Elevation")

                If liElevation < liElevStartTORA_RecipRwy Then
                    '  lsErrDescription = loObstaclesTable.Rows(I)("RwyId") & " " & loObstaclesTable.Rows(I)("RwyMod") & _
                    lsErrDescription = " " & loObstaclesTable.Rows(I)("Distance") & ":" & _
                   " Obstacle Elevation " & liElevation & " below End of Runway Elevation " & liElevStartTORA_RecipRwy

                    loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                       loObstaclesTable.Rows(I)("ICAO"), loObstaclesTable.Rows(I)("CITY"), _
                       loObstaclesTable.Rows(I)("RwyId"), loObstaclesTable.Rows(I)("RwyMod"), _
                       "", 0, lsErrDescription, lsModuleId)
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '5. If the obstacle Elevation minus the elevation at the 
            'beginning of the runway is greater than MaxObsHt, log an error.
            'Sort the obstacles on increasing distance from the beginning of the runway. 
            'If there is more than one obstacle compare the elevation of each obstacle (except the first one) 
            'with the elevation of the preceding one. If an obstacle is lower than the preceding obstacle, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            liElevStartTORA_Rwy = loObstaclesTable.Rows(I)("ElevStartTORA_Rwy")

            If (liElevation - liElevStartTORA_Rwy) > liMaxObsHt Then
                'lsErrDescription = loObstaclesTable.Rows(I)("RwyId") & " " & loObstaclesTable.Rows(I)("RwyMod") & _
                lsErrDescription = " " & loObstaclesTable.Rows(I)("Distance") & ":" & _
                " Obstacle Elevation " & liElevation & " exceeds Start of Runway Elevation by more than " & liMaxObsHt

                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                   loObstaclesTable.Rows(I)("ICAO"), loObstaclesTable.Rows(I)("CITY"), _
                   loObstaclesTable.Rows(I)("RwyId"), loObstaclesTable.Rows(I)("RwyMod"), _
                   "", 0, lsErrDescription, lsModuleId)
            End If


            '5. REMAINING PART IS HERE-----------------------------
            'Sort the obstacles on increasing distance from the beginning of the runway. 
            'If there is more than one obstacle compare the elevation of each obstacle (except the first one) 
            'with the elevation of the preceding one. If an obstacle is lower than the preceding obstacle, log an error.
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Check here whether Distance is sorted or not, For sorting only it has taken out of above loop.


            If lsPrevRwyId <> loObstaclesTable.Rows(I)("RwyId") Then

                loObstaclesTable.DefaultView.RowFilter = "ICAO='" & loObstaclesTable.Rows(I)("ICAO") & "' AND RwyId='" & _
                                                                    loObstaclesTable.Rows(I)("RwyId") & "' AND RwyMod='" & _
                                                                    loObstaclesTable.Rows(I)("RwyMod") & "'"
                loObstaclesTable.DefaultView.Sort = "Distance ASC"

                dtObstacles = loObstaclesTable.DefaultView.ToTable

                For J = 0 To dtObstacles.Rows.Count - 1
                    If J > 0 Then
                        If Not (Convert.ToInt32(dtObstacles.Rows(J).Item("Elevation")) > intPrevElevation) Then
                            'lsErrDescription = dtObstacles.Rows(J)("RwyId") & " " & loObstaclesTable.Rows(J)("RwyMod") & _
                            lsErrDescription = " " & dtObstacles.Rows(J)("Distance") & ":" & _
                                " Obstacle Elevation " & dtObstacles.Rows(J)("Elevation") & _
                                " is less than previous Obstacle Elevation  " & intPrevElevation

                            loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                               dtObstacles.Rows(J)("ICAO"), dtObstacles.Rows(J)("CITY"), _
                               dtObstacles.Rows(J)("RwyId"), dtObstacles.Rows(J)("RwyMod"), _
                               "", 0, lsErrDescription, lsModuleId)
                        End If
                    End If
                    intPrevElevation = dtObstacles.Rows(J).Item("Elevation")
                Next


            ElseIf lsPrevRwyId = loObstaclesTable.Rows(I)("RwyId") Then
                If lsPrevRwyMod <> loObstaclesTable.Rows(I)("RwyMod") Then

                    loObstaclesTable.DefaultView.RowFilter = "ICAO='" & loObstaclesTable.Rows(I)("ICAO") & "' AND RwyId='" & _
                                                                        loObstaclesTable.Rows(I)("RwyId") & "' AND RwyMod='" & _
                                                                        loObstaclesTable.Rows(I)("RwyMod") & "'"
                    loObstaclesTable.DefaultView.Sort = "Distance ASC"

                    dtObstacles = loObstaclesTable.DefaultView.ToTable

                    For J = 0 To dtObstacles.Rows.Count - 1
                        If J > 0 Then
                            If Not (Convert.ToInt32(dtObstacles.Rows(J).Item("Elevation")) > intPrevElevation) Then
                                ' lsErrDescription = dtObstacles.Rows(J)("RwyId") & " " & loObstaclesTable.Rows(J)("RwyMod") & _
                                lsErrDescription = " " & dtObstacles.Rows(J)("Distance") & ":" & _
                                    " Obstacle Elevation " & dtObstacles.Rows(J)("Elevation") & _
                                    " is less than previous Obstacle Elevation  " & intPrevElevation

                                loADMS_BO_Common.AddRowIntoDataChecksResultTable(poDataChecksResultTable, _
                                   dtObstacles.Rows(J)("ICAO"), dtObstacles.Rows(J)("CITY"), _
                                   dtObstacles.Rows(J)("RwyId"), dtObstacles.Rows(J)("RwyMod"), _
                                   "", 0, lsErrDescription, lsModuleId)
                            End If
                        End If
                        intPrevElevation = dtObstacles.Rows(J).Item("Elevation")
                    Next
                End If

            End If

            'lsPrevICAO = loObstaclesTable.Rows(I)("ICAO")
            lsPrevRwyId = loObstaclesTable.Rows(I)("RwyId")
            lsPrevRwyMod = loObstaclesTable.Rows(I)("RwyMod")
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Next

        '5. REMAINING PART IS HERE-----------------------------
        'Sort the obstacles on increasing distance from the beginning of the runway. 
        'If there is more than one obstacle compare the elevation of each obstacle (except the first one) 
        'with the elevation of the preceding one. If an obstacle is lower than the preceding obstacle, log an error.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Check here whether Distance is sorted or not, For sorting only it has taken out of above loop.



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        loADMS_BAL_IDM = Nothing
        loADMS_BAL_ODM = Nothing
        loADMS_BO_Common = Nothing
        loADMS_BAL_Data_Checks = Nothing


    End Sub

#End Region

End Class
