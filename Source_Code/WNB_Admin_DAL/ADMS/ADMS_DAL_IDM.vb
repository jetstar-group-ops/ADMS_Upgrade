
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO

Public Class ADMS_DAL_IDM

    Public Function GetAllIntersections(ByVal psUserId As String, _
                                        ByVal psIcao As String, _
                                        ByVal psRwyId As String, _
                                        ByVal psRwyMod As String) As DataTable

        Dim loDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim loIntersections As DataSet = Nothing
        Dim lsSqlCommand As String = "Get_AllIntersections"
        Dim loDbGetCommand As DbCommand = loDB.GetStoredProcCommand(lsSqlCommand)

        loDB.AddInParameter(loDbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        loDB.AddInParameter(loDbGetCommand, "Icao", DbType.String, _
                         IIf(psIcao = "", DBNull.Value, psIcao))

        loDB.AddInParameter(loDbGetCommand, "RwyId", DbType.String, _
                         IIf(psRwyId = "", DBNull.Value, psRwyId))

        loDB.AddInParameter(loDbGetCommand, "RwyMod", DbType.String, _
                         IIf(psRwyMod = "", DBNull.Value, psRwyMod))

        loIntersections = loDB.ExecuteDataSet(loDbGetCommand)

        Return loIntersections.Tables(0)

    End Function

    Public Function GetIntersectionDetails(ByVal psUserId As String, _
                                     ByVal psIcao As String, _
                                     ByVal psRwyId As String, _
                                     ByVal psRwyMod As String, _
                                     ByVal piIntersectionId As Integer) As DataSet

        Dim loDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim loIntersectionDetails As DataSet = Nothing
        Dim lsSqlCommand As String = "Get_Intersection_Details"
        Dim loDbGetCommand As DbCommand = loDB.GetStoredProcCommand(lsSqlCommand)

        loDB.AddInParameter(loDbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        loDB.AddInParameter(loDbGetCommand, "Icao", DbType.String, _
                         IIf(psIcao = "", DBNull.Value, psIcao))

        loDB.AddInParameter(loDbGetCommand, "RwyId", DbType.String, _
                         IIf(psRwyId = "", DBNull.Value, psRwyId))

        loDB.AddInParameter(loDbGetCommand, "RwyMod", DbType.String, _
                         IIf(psRwyMod = "", DBNull.Value, psRwyMod))

        loDB.AddInParameter(loDbGetCommand, "Intersection_Id", DbType.Int32, IIf(piIntersectionId = 0, DBNull.Value, piIntersectionId))

        loIntersectionDetails = loDB.ExecuteDataSet(loDbGetCommand)

        Return loIntersectionDetails

    End Function

    Public Function CreateUpdateIntersection(ByVal poIntersectionDetails As DataSet, _
                                     ByVal psUserId As String, _
                                     ByVal psIcao As String, _
                                      ByVal psRwyId As String, _
                                        ByVal psRwyMod As String, _
                                        ByVal psIntersectionId As String) As Integer

        Dim intResult As Integer = 0
        Try
            Dim loDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim sqlCommand As String = "CreateUpdate_Intersection"
            Dim InsertDeleteCommand As DbCommand = loDB.GetStoredProcCommand(sqlCommand)

            loDB.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, psUserId)
            loDB.AddInParameter(InsertDeleteCommand, "@Icao", DbType.String, IIf(psIcao = "", DBNull.Value, psIcao))

            loDB.AddInParameter(InsertDeleteCommand, "@RwyId", DbType.String, _
                       IIf(psRwyId = "", DBNull.Value, psRwyId))

            loDB.AddInParameter(InsertDeleteCommand, "@RwyMod", DbType.String, _
                             IIf(psRwyMod = "", DBNull.Value, psRwyMod))

            loDB.AddInParameter(InsertDeleteCommand, "@Intersection_Id", DbType.Int32, _
                            IIf(psIntersectionId = "", DBNull.Value, psIntersectionId))

            loDB.AddInParameter(InsertDeleteCommand, "@IntersectionDetails", DbType.Xml, poIntersectionDetails.GetXml)
            loDB.AddOutParameter(InsertDeleteCommand, "@intResult", DbType.Int16, 4)

            loDB.ExecuteNonQuery(InsertDeleteCommand)

            If InsertDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(InsertDeleteCommand.Parameters("@intResult").Value)
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function DeleteIntersection(ByVal psUserId As String, _
                                     ByVal psIcao As String, _
                                       ByVal psRwyId As String, _
                                        ByVal psRwyMod As String, _
                                        ByVal piIntersectionId As Integer) As Integer

        Dim intResult As Integer = 0
        Try
            Dim loDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim sqlCommand As String = "Delete_Intersection"
            Dim InsertDeleteCommand As DbCommand = loDB.GetStoredProcCommand(sqlCommand)

            loDB.AddInParameter(InsertDeleteCommand, "@UserId", DbType.String, psUserId)
            loDB.AddInParameter(InsertDeleteCommand, "@Icao", DbType.String, IIf(psIcao = "", DBNull.Value, psIcao))

            loDB.AddInParameter(InsertDeleteCommand, "@RwyId", DbType.String, _
                     IIf(psRwyId = "", DBNull.Value, psRwyId))

            loDB.AddInParameter(InsertDeleteCommand, "@RwyMod", DbType.String, _
                             IIf(psRwyMod = "", DBNull.Value, psRwyMod))

            loDB.AddInParameter(InsertDeleteCommand, "@Intersection_Id", DbType.Int32, piIntersectionId)

            loDB.AddOutParameter(InsertDeleteCommand, "@intResult", DbType.Int16, 4)

            loDB.ExecuteNonQuery(InsertDeleteCommand)

            If InsertDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(InsertDeleteCommand.Parameters("@intResult").Value)
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

End Class
