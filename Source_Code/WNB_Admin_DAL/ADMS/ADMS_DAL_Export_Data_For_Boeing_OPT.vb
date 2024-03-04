Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Text
Imports Finisar.SQLite
Imports System.Diagnostics
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO
Imports System.Data.SQLite

Public Class ADMS_DAL_Export_Data_For_Boeing_OPT

    Public Function AddAirport()
        Dim SQLconnect As New SQLiteConnection()
        Dim SQLcommand As SQLiteCommand

        SQLconnect.ConnectionString = "Data Source=Populated_SQL_Light_DB_OPTAirportMaster_V4.0.sdb;"
        SQLconnect.Open()
        SQLcommand = SQLconnect.CreateCommand
        'Insert Record into Foo
        SQLcommand.CommandText = "INSERT INTO foo (title, description) VALUES ('This is a title', 'This is a Description')"
        'Update Last Created Record in Foo
        SQLcommand.CommandText = "UPDATE foo SET title = 'New Title', description = 'New Description' WHERE id = last_insert_rowid()"
        'Delete Last Created Record from Foo
        SQLcommand.CommandText = "DELETE FROM foo WHERE id = last_insert_rowid()"
        SQLcommand.ExecuteNonQuery()
        SQLcommand.Dispose()
        SQLconnect.Close()
    End Function

    Public Sub ExportDataForBoeingOptDatabase(ByVal psUserId As String, _
                ByVal poBoeingOptData As DataSet, _
                ByVal psExportedDbFile As String, _
                ByVal psEmptyBoeingOptSqlieDbFile As String, _
                ByVal PsAiracCode As String)

        Dim I As Integer
        Dim SQLconnect As New SQLiteConnection()
        Dim SQLcommand As SQLiteCommand
        Dim lsMySql As String

        Try
            'If System.IO.File.Exists(psExportedDbFile) = True Then
            '    System.IO.File.Delete(psExportedDbFile)
            'End If

            System.IO.File.Copy(psEmptyBoeingOptSqlieDbFile, psExportedDbFile, True)
            SQLconnect.ConnectionString = "Data Source=" & psExportedDbFile & "; Version=3;" 'Populated_SQL_Light_DB_OPTAirportMaster_V4.0.sdb;"
            SQLconnect.Open()
            SQLcommand = SQLconnect.CreateCommand

            For I = 0 To poBoeingOptData.Tables("AptInfo").Rows.Count - 1

                lsMySql = "INSERT INTO AptInfo (Code,IATA,Name,City,Country,Elevation,RwyMeasType," & _
                    "DistUnit,HtUnit,ObDistRef,ObHtRef,CrDate,UpdDate,Comment,MagVar,Lat,Long ) VALUES (" & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("CODE") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("IATA") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("NAME") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("CITY") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("COUNTRY") & "") & "'," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("Elevation") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("RwyMeasType") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("DistUnit") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("HtUnit") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("ObDistRef") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("ObHtRef") & "") & "," & _
                "'" & Date.Parse(getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("CrDate") & "")).ToString("dd-MM-yyyy HH:mm") & "'," & _
                "'" & Date.Parse(getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("UpdDate") & "")).ToString("dd-MM-yyyy HH:mm") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("Comment") & "") & "'," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("MagVar") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("lat") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("AptInfo").Rows(I)("Long") & "") & " ) "

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next

            For I = 0 To poBoeingOptData.Tables("RwyInfo").Rows.Count - 1
                lsMySql = "INSERT INTO RwyInfo(Code,ID,TORA,TODA,ASDA,XLDA,Width,Surface,SlopeTOD," & _
                    "SlopeASD,SlopeLDA,CrDate,UpdDate,CrTime,UpTime,Comment,eoLabel," & _
                    "lineupAngle,elevStartTORA,elevEndTORA,LatStartTORA,LongStartTORA," & _
                    "LatEndTORA,LongEndTORA,MagHdg,gaLabel,mfrh, PCN, useForTakeoff, useForLanding) VALUES (" & _
                  "'" & getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("CODE") & "") & "'," & _
                  "'" & getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("ID") & "") & "'," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("TORA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("TODA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("ASDA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("XLDA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("Width") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("Surface") & "") & "," & _
                   "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("SlopeTOD") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("SlopeASD") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("SlopeLDA") & "") & "," & _
                  "'" & Date.Parse(getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("CrDate") & "")).ToString("dd-MM-yyyy HH:mm") & "'," & _
                  "'" & Date.Parse(getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("UpdDate") & "")).ToString("dd-MM-yyyy HH:mm") & "'," & _
                  "'" & Date.Parse(getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("CrTime") & "")).ToString("HH:mm") & "'," & _
                  "'" & Date.Parse(getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("UpTime") & "")).ToString("HH:mm") & "'," & _
                  "'" & getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("Comment") & "") & "'," & _
                   "'" & getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("MagHdgeoLabel") & "") & "'," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("lineupAngle") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("elevStartTORA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("elevEndTORA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("LatStartTORA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("LongStartTORA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("LatEndTORA") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("LongEndTORA") & "") & "," & _
                   "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("MagHdg") & "") & "," & _
                  "'" & getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("gaLabel") & "") & "'," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("mfrh") & "") & "," & _
                  "'" & getStringFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("PCN") & "") & "'," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("useForTakeoff") & "") & "," & _
                  "" & getFieldValue(poBoeingOptData.Tables("RwyInfo").Rows(I)("useForLanding") & "") & ")"

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next

            For I = 0 To poBoeingOptData.Tables("IntersectInfo").Rows.Count - 1

                lsMySql = "INSERT INTO IntersectInfo(Code,ID,Name,deltaFL,deltaRef," & _
                "elevStartTORA,LatStartTORA,LongStartTORA,lineupAngle,slopeTOD,slopeASD,Comment) VALUES (" & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("CODE") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("ID") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("Name") & "") & "'," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("deltaFL") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("deltaRef") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("elevStartTORA") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("LatStartTORA") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("LongStartTORA") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("lineupAngle") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("slopeTOD") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("slopeASD") & "") & "," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("IntersectInfo").Rows(I)("Comment") & "") & "')"

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next

            For I = 0 To poBoeingOptData.Tables("ObstInfo").Rows.Count - 1

                lsMySql = "INSERT INTO ObstInfo(Code,ID,ProcedureID,Number,Dist,Ht,LatOffSet) VALUES (" & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("ObstInfo").Rows(I)("CODE") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("ObstInfo").Rows(I)("ID") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("ObstInfo").Rows(I)("ProcedureID") & "") & " '," & _
                "" & I + 1 & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("ObstInfo").Rows(I)("Dist") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("ObstInfo").Rows(I)("Ht") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("ObstInfo").Rows(I)("LatOffSet") & "") & ")"

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next


            For I = 0 To poBoeingOptData.Tables("EOProced").Rows.Count - 1

                lsMySql = "INSERT INTO EOProced(Code,ID,ProcedureID,EOProc,AcType,FlapConfig) VALUES (" & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("EOProced").Rows(I)("CODE") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("EOProced").Rows(I)("ID") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("EOProced").Rows(I)("ProcedureID") & "") & " '," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("EOProced").Rows(I)("EOProc") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("EOProced").Rows(I)("AcType") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("EOProced").Rows(I)("FlapConfig") & "") & "')"

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next

            For I = 0 To poBoeingOptData.Tables("GAProced").Rows.Count - 1

                lsMySql = "INSERT INTO GAProced(Code,ID,ProcedureID,gaProc,gaGradient,DecisionHt," & _
                    "DeltaHt,AcType,FlapConfig) VALUES (" & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("CODE") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("ID") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("ProcedureID") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("gaProc") & "") & "'," & _
                "" & getFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("gaGradient") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("DecisionHt") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("DeltaHt") & "") & "," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("AcType") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("GAProced").Rows(I)("FlapConfig") & "") & "')"

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next

            For I = 0 To poBoeingOptData.Tables("databaseConstants").Rows.Count - 1

                lsMySql = "INSERT INTO databaseConstants(displayAirportComments,displayRunwayComments," & _
                "airportversionID,assembleDate,versionNum,StartDate,EndDate,StartTime,EndTime) VALUES (" & _
                "" & getFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("displayAirportComments") & "") & "," & _
                "" & getFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("displayRunwayComments") & "") & "," & _
                "'" & PsAiracCode & "" & getStringFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("airportversionID")) & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("assembleDate") & "") & "'," & _
                "" & getFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("versionNum") & "") & "," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("StartDate") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("EndDate") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("StartTime") & "") & "'," & _
                "'" & getStringFieldValue(poBoeingOptData.Tables("databaseConstants").Rows(I)("EndTime") & "") & "')"

                SQLcommand.CommandText = lsMySql
                SQLcommand.ExecuteNonQuery()
            Next


            SQLcommand.Dispose()
            SQLconnect.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function getStringFieldValue(ByVal psValue As String) As String

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

    Public Function GetDataForBoeingOptDatabaseExport(ByVal psUserId As String, _
                        ByVal psActive As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_Data_For_BoeingOptDatabaseExport"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        Dim lsTableName As String = ""

        db.AddInParameter(dbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        db.AddInParameter(dbGetCommand, "Active", DbType.Int16, _
                          IIf(psActive = "", DBNull.Value, psActive))

        db.AddOutParameter(dbGetCommand, "TableNames", DbType.String, 3000)

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)

        lsTableName = db.GetParameterValue(dbGetCommand, "TableNames").ToString()

        For I = 0 To lsTableName.Split(",").Length - 1
            ReportGroupsDS.Tables(I).TableName = lsTableName.Split(",")(I)
            ReportGroupsDS.AcceptChanges()
        Next

        Return ReportGroupsDS

    End Function

End Class
