Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO

Public Class ADMS_DAL_ADM

    Public Function GetAllAirports(ByVal psUserId As String, _
                                   ByVal psIcao As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_AllAirports"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        db.AddInParameter(dbGetCommand, "Icao", DbType.String, _
                         IIf(psIcao = "", DBNull.Value, psIcao))

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)
        Return ReportGroupsDS.Tables(0)

    End Function

    Public Function GetAirportCategories(ByVal psUserId As String, _
                                    ByVal psCatId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_Airport_Category_Details"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        db.AddInParameter(dbGetCommand, "Icao", DbType.String, _
                          IIf(psCatId = "", DBNull.Value, psCatId))

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)
        Return ReportGroupsDS.Tables(0)

    End Function

    Public Function GetAirportDetails(ByVal psUserId As String, _
                                      ByVal psIcao As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_Airport_Details"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        db.AddInParameter(dbGetCommand, "Icao", DbType.String, _
                          IIf(psIcao = "", DBNull.Value, psIcao))

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)
        Return ReportGroupsDS

    End Function

    Public Function CreateUpdateAirport(ByVal poAirportDetails As DataSet, _
                                      ByVal psUserId As String, _
                                      ByVal psIcao As String) As Integer

        Dim intResult As Integer = 0
        Try
            Dim objDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim sqlCommand As String = "CreateUpdate_Airport"
            Dim InsertDeleteCommand As DbCommand = objDB.GetStoredProcCommand(sqlCommand)

            objDB.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, psUserId)
            objDB.AddInParameter(InsertDeleteCommand, "@Icao", DbType.String, IIf(psIcao = "", DBNull.Value, psIcao))
            objDB.AddInParameter(InsertDeleteCommand, "@AirportDetails", DbType.Xml, poAirportDetails.GetXml)
            objDB.AddOutParameter(InsertDeleteCommand, "@intResult", DbType.Int16, 4)

            objDB.ExecuteNonQuery(InsertDeleteCommand)

            If InsertDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(InsertDeleteCommand.Parameters("@intResult").Value)
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function DeleteAirport(ByVal psUserId As String, _
                                      ByVal psIcao As String) As Integer

        Dim intResult As Integer = 0
        Try
            Dim objDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim sqlCommand As String = "Delete_Airport"
            Dim InsertDeleteCommand As DbCommand = objDB.GetStoredProcCommand(sqlCommand)

            objDB.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, psUserId)
            objDB.AddInParameter(InsertDeleteCommand, "@Icao", DbType.String, IIf(psIcao = "", DBNull.Value, psIcao))
            objDB.AddOutParameter(InsertDeleteCommand, "@intResult", DbType.Int16, 4)

            objDB.ExecuteNonQuery(InsertDeleteCommand)

            If InsertDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(InsertDeleteCommand.Parameters("@intResult").Value)
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    'added by swami on 22-02-2024'
    Public Function GetAllAirportsByStatus(ByVal psUserId As String, _
                                   ByVal psIcao As String, ByVal psActive As Integer) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "Get_AllAirportsByStatus"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        'db.AddInParameter(dbGetCommand, "Icao", DbType.String, _
        '                 IIf(psIcao = "", DBNull.Value, psIcao))

        db.AddInParameter(dbGetCommand, "Active", DbType.Int32, psActive)


        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)
        Return ReportGroupsDS.Tables(0)
    End Function

End Class
