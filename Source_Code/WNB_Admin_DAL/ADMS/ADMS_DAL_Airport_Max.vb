Imports System
Imports System.Data
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.IO
Imports System.Data.OleDb

Public Class ADMS_DAL_Airport_Max

    Public Function ExportDataToMDB(ByVal psUserId As String, _
                                    ByVal poDsArptRwyObsData As DataSet, _
                                    ByVal lsDataSourcePathWithExtension As String)
        Try
            Dim dbProvider = "PROVIDER=Microsoft.Jet.OLEDB.4.0;"
            Dim dbSource = "Data Source= " & lsDataSourcePathWithExtension & ""
            Dim dbProviderSource As String
            dbProviderSource = dbProvider + dbSource

            Dim con = New OleDb.OleDbConnection(dbProviderSource)
            Dim cmd As New OleDb.OleDbCommand()
            con.Open()

            'Deleting data from all tables
            DeleteAirportRunwayObstacleTbl(dbProviderSource, con, cmd)

            'Exporting Airport Table
            ExportDataForAiports(psUserId, poDsArptRwyObsData.Tables(0), con, cmd)
            'Exporting Runway Table
            ExportDataForRunways(psUserId, poDsArptRwyObsData.Tables(1), con, cmd)
            'Exporting Obstacle Table
            ExportDataForObstacle(psUserId, poDsArptRwyObsData.Tables(2), con, cmd)
            'Exporting Intesection Table
            ExportDataForRunways(psUserId, poDsArptRwyObsData.Tables(3), con, cmd)
            'Exporting Obstacles Table Again
            ExportDataForObstacle(psUserId, poDsArptRwyObsData.Tables(4), con, cmd)

            con.Close()


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ExportDataForAiports(ByVal psUserId As String, _
                                         ByVal poAirportMDBTable As DataTable, _
                                         ByVal con As OleDb.OleDbConnection, _
                                         ByVal cmd As OleDb.OleDbCommand) As Boolean
        Try


            If Not poAirportMDBTable Is Nothing Then
                If poAirportMDBTable.Rows.Count > 0 Then
                    For I = 0 To poAirportMDBTable.Rows.Count - 1

                        Dim sql = "INSERT INTO tblAirport(fldICAOcd, fldATAcd, fldstnName, " & _
                                  "fldarptName, fldarptElev, fldarptmmemo, fldarptLong, fldarptLat, " & _
                                  "fldMagDev, fldUpdated,fldInactive) " & _
                                  "VALUES ('" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldICAOcd")) & "'," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldATAcd")) & "'," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldstnName")) & "'," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldarptName")) & "'," & _
                                           "" & getFieldValue(poAirportMDBTable.Rows(I)("fldarptElev")) & "," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldarptmmemo")) & "'," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldarptLong")) & "'," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldarptLat")) & "'," & _
                                           "'" & getStringFieldValues(poAirportMDBTable.Rows(I)("fldMagDev")) & "'," & _
                                           "" & poAirportMDBTable.Rows(I)("fldUpdated") & "," & _
                                           "" & poAirportMDBTable.Rows(I)("fldInactive") & "" & _
                                         ")"
                        cmd = New OleDb.OleDbCommand(sql, con)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ExportDataForRunways(ByVal psUserId As String, _
                                         ByVal poRunwayMDBTable As DataTable, _
                                         ByVal con As OleDb.OleDbConnection, _
                                         ByVal cmd As OleDb.OleDbCommand) As Boolean
        Try

            If Not poRunwayMDBTable Is Nothing Then
                If poRunwayMDBTable.Rows.Count > 0 Then
                    For I = 0 To poRunwayMDBTable.Rows.Count - 1

                        Dim sql = "INSERT INTO tblRUNWAY(fldICAOcd,fldRWYno,fldRwyHdg,fldRwyIntersect,fldNotam," & _
                           "fldRwySpecial,fldATACd,fldRWYID,fldRwyBRE,fldRwyLOE,fldRwyPkE,fldRwyPKD,fldRwyTOD," & _
                           "fldRwyClwy,fldRwySTPwy,fldTOSlope,fldTOobsn,fldTONote,fldLDLen,fldLDslope,fldGSLen," & _
                           "fldGSslope,fldRwyUpdate,fldRwyUpdSrc,fldRwyTpdDt,fldRwyInt,fldLndNote,fldAlignOpt,fldAlignDt," & _
                           "fldDisttoBoundary,fldTurn,fldTurnProcedure,fldPathWidth,fldSurface,fldStrength,fldRwyWidth," & _
                           "fldrwyshldr,fldRwyRefCode,fldRwyNotam,fldRwyACFTapp,fldILSCAt,fldUpdated,fldInactive," & _
                           "flddelBRdist,flddelLODist,fldextobdist,fldextobhgt,fldextobsg,flddelAppdist,flddelStpdist) " & _
                           "VALUES ('" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldICAOcd")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRWYno")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyHdg")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyIntersect")) & "'," & _
                                    "" & poRunwayMDBTable.Rows(I)("fldNotam") & "," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwySpecial")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldATACd")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRWYID")) & "'," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyBRE")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyLOE")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyPkE")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyPKD")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyTOD")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyClwy")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwySTPwy")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldTOSlope")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldTOobsn")) & "," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldTONote")) & "'," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldLDLen")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldLDslope")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldGSLen")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldGSslope")) & "," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyUpdate")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyUpdSrc")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyTpdDt")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyInt")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldLndNote")) & "'," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldAlignOpt")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldAlignDt")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldDisttoBoundary")) & "," & _
                                    "" & poRunwayMDBTable.Rows(I)("fldTurn") & "," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldTurnProcedure")) & "'," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldPathWidth")) & "," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldSurface")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldStrength")) & "'," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldRwyWidth")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldrwyshldr")) & "," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyRefCode")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyNotam")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldRwyACFTapp")) & "'," & _
                                    "'" & getStringFieldValues(poRunwayMDBTable.Rows(I)("fldILSCAt")) & "'," & _
                                    "" & poRunwayMDBTable.Rows(I)("fldUpdated") & "," & _
                                    "" & poRunwayMDBTable.Rows(I)("fldInactive") & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("flddelBRdist")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("flddelLODist")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldextobdist")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("fldextobhgt")) & "," & _
                                    "" & poRunwayMDBTable.Rows(I)("fldextobsg") & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("flddelAppdist")) & "," & _
                                    "" & getFieldValue(poRunwayMDBTable.Rows(I)("flddelStpdist")) & "" & _
                                    ")"

                        cmd = New OleDb.OleDbCommand(sql, con)
                        cmd.ExecuteNonQuery()

                    Next
                End If
            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ExportDataForObstacle(ByVal psUserId As String, _
                                          ByVal poObstaclesMDBTable As DataTable, _
                                          ByVal con As OleDb.OleDbConnection, _
                                          ByVal cmd As OleDb.OleDbCommand) As Boolean
        Try

            If Not poObstaclesMDBTable Is Nothing Then
                If poObstaclesMDBTable.Rows.Count > 0 Then
                    For I = 0 To poObstaclesMDBTable.Rows.Count - 1

                        Dim sql = "INSERT INTO tblOBSTACLE(fldRwyID,fldOBSName,fldOBSDist,fldOBSHT," & _
                                  "fldOBSDEV,fldOBsDevLR,fldOBSref,fldOBSSRC,fldOBSDT,fldOBsaddht,fldOBSNetGross) " & _
                                  "VALUES ('" & getStringFieldValues(poObstaclesMDBTable.Rows(I)("fldRwyID")) & "'," & _
                                           "'" & getStringFieldValues(poObstaclesMDBTable.Rows(I)("fldOBSName")) & "'," & _
                                           "" & getFieldValue(poObstaclesMDBTable.Rows(I)("fldOBSDist")) & "," & _
                                           "" & getFieldValue(poObstaclesMDBTable.Rows(I)("fldOBSHT")) & "," & _
                                           "" & getFieldValue(poObstaclesMDBTable.Rows(I)("fldOBSDEV")) & "," & _
                                           "'" & getStringFieldValues(poObstaclesMDBTable.Rows(I)("fldOBsDevLR")) & "'," & _
                                           "'" & getStringFieldValues(poObstaclesMDBTable.Rows(I)("fldOBSref")) & "'," & _
                                           "'" & getStringFieldValues(poObstaclesMDBTable.Rows(I)("fldOBSSRC")) & "'," & _
                                           "'" & poObstaclesMDBTable.Rows(I)("fldOBSDT") & "'," & _
                                           "" & getFieldValue(poObstaclesMDBTable.Rows(I)("fldOBsaddht")) & "," & _
                                           "" & poObstaclesMDBTable.Rows(I)("fldOBSNetGross") & "" & _
                                         ")"
                        cmd = New OleDb.OleDbCommand(sql, con)
                        cmd.ExecuteNonQuery()
                    Next
                End If
            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function DeleteAirportRunwayObstacleTbl(ByVal DataSourceFilePath As String, _
                                                   ByVal con As OleDb.OleDbConnection, _
                                                   ByVal cmd As OleDb.OleDbCommand)
        Try

            Dim sqlDelete = "delete from tblAirport"
            cmd = New OleDb.OleDbCommand(sqlDelete, con)
            cmd.ExecuteNonQuery()

            sqlDelete = "delete from tblRUNWAY"
            cmd = New OleDb.OleDbCommand(sqlDelete, con)
            cmd.ExecuteNonQuery()


            sqlDelete = "delete from tblOBSTACLE"
            cmd = New OleDb.OleDbCommand(sqlDelete, con)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getStringFieldValues(ByVal psValue As String) As String
        Try
            Return (psValue.Trim & "").Replace("'", "")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function getFieldValue(ByVal psValue As String) As String

        Try
            If psValue.Trim & "" = "" Then
                Return "NULL"
            Else
                Return psValue.Trim()
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class


