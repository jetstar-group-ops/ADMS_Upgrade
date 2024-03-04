Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO
Imports JQCommon
Imports System.Globalization
Imports System.Data.SqlClient


Public Class ADMS_DAL_ODM

    Public Function Get_Obstacles(ByVal psUserId As String, ByVal intObstacleId As Integer, _
                ByVal psICAO As String, ByVal psRwyId As String, ByVal psRwyMod As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_Obstacles"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbGetCommand, "@UserId", DbType.String, psUserId)
        db.AddInParameter(dbGetCommand, "@ObstacleId", DbType.Int32, IIf(intObstacleId = 0, DBNull.Value, intObstacleId))
        db.AddInParameter(dbGetCommand, "@ICAO", DbType.String, IIf(psICAO = "", DBNull.Value, psICAO))
        db.AddInParameter(dbGetCommand, "@RwyId", DbType.String, IIf(psRwyId = "", DBNull.Value, psRwyId))
        db.AddInParameter(dbGetCommand, "@RwyMod", DbType.String, IIf(psRwyMod = "", DBNull.Value, psRwyMod))

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)

        Return ReportGroupsDS
    End Function

    Public Function CreateUpdate_Obstacles(ByVal strUserId As String, ByVal intObstacleId As Integer, ByVal strICAO As String, ByVal strRwyId As String, _
                                        ByVal RwyMod As String, ByVal dsRunwayDetails As DataSet) As Integer

        Dim intResult As Integer = 0
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
            Dim sqlCommand As String = "CreateUpdate_Obstacles"
            Dim InsertDeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, strUserId)
            db.AddInParameter(InsertDeleteCommand, "@Obstacle_Id", DbType.String, IIf(intObstacleId = 0, DBNull.Value, intObstacleId))
            db.AddInParameter(InsertDeleteCommand, "@ICAO", DbType.String, strICAO)
            db.AddInParameter(InsertDeleteCommand, "@RwyId", DbType.String, strRwyId)
            db.AddInParameter(InsertDeleteCommand, "@RwyMod", DbType.String, RwyMod)
            db.AddInParameter(InsertDeleteCommand, "@ObstacleDetails", DbType.Xml, dsRunwayDetails.GetXml)
            db.AddOutParameter(InsertDeleteCommand, "@intResult", DbType.Int16, 1)

            db.ExecuteNonQuery(InsertDeleteCommand)

            If InsertDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(InsertDeleteCommand.Parameters("@intResult").Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return intResult
    End Function

    Public Function Delete_Obstacle(ByVal strUserId As String, ByVal intObstacleId As Integer, ByVal strICAO As String, ByVal strRwyId As String, _
                                      ByVal RwyMod As String) As Integer

        Dim intResult As Integer = 0
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
            Dim sqlCommand As String = "Delete_Obstacle"
            Dim InsertDeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, strUserId)
            db.AddInParameter(InsertDeleteCommand, "@Obstacle_id", DbType.Int16, intObstacleId)
            db.AddInParameter(InsertDeleteCommand, "@ICAO", DbType.String, strICAO)
            db.AddInParameter(InsertDeleteCommand, "@RwyId", DbType.String, strRwyId)
            db.AddInParameter(InsertDeleteCommand, "@RwyMod", DbType.String, RwyMod)
            db.AddOutParameter(InsertDeleteCommand, "@intResult", DbType.Int16, 1)

            db.ExecuteNonQuery(InsertDeleteCommand)

            If InsertDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(InsertDeleteCommand.Parameters("@intResult").Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return intResult
    End Function

End Class
