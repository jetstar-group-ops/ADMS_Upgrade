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

Public Class ADMS_DAL_RDM

    Public Function Get_Runway(ByVal strUserId As String, ByVal strICAO As String, ByVal intRwyId As String, ByVal RwyMod As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_Runway"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        db.AddInParameter(dbGetCommand, "@UserId", DbType.String, strUserId)
        db.AddInParameter(dbGetCommand, "@ICAO", DbType.String, IIf(strICAO = "", DBNull.Value, strICAO))
        db.AddInParameter(dbGetCommand, "@RwyId", DbType.String, IIf(intRwyId = "", DBNull.Value, intRwyId))
        db.AddInParameter(dbGetCommand, "@RwyMod", DbType.String, IIf(RwyMod = "", DBNull.Value, RwyMod))

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)

        Return ReportGroupsDS
    End Function


    Public Function CreateUpdate_Runway(ByVal strUserId As String, ByVal strICAO As String, ByVal strRwyId As String, _
                                        ByVal RwyMod As String, ByVal dsRunwayDetails As DataSet) As Integer

        Dim intResult As Integer = 0
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
            Dim sqlCommand As String = "CreateUpdate_Runway"
            Dim InsertDeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, strUserId)
            db.AddInParameter(InsertDeleteCommand, "@ICAO", DbType.String, strICAO)
            db.AddInParameter(InsertDeleteCommand, "@RwyId", DbType.String, strRwyId)
            db.AddInParameter(InsertDeleteCommand, "@RwyMod", DbType.String, RwyMod)
            db.AddInParameter(InsertDeleteCommand, "@RunwayDetails", DbType.Xml, dsRunwayDetails.GetXml)
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


    Public Function Delete_Runway(ByVal strUserId As String, ByVal strICAO As String, ByVal strRwyId As String, _
                                        ByVal RwyMod As String) As Integer

        Dim intResult As Integer = 0
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
            Dim sqlCommand As String = "Delete_Runway"
            Dim InsertDeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, strUserId)
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
