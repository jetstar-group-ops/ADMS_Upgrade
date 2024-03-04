Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Public Class ADMS_DAL_DatabaseExp
    ''' <summary>
    ''' Get DatabaseExp
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDatabaseExp(ByVal psUserId As String) As DataTable

        Dim loDatabase As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim loReportGroupsDS As DataSet = Nothing
        Dim lsCommand As String = "[Get_DatabaseExp]"
        Dim loDbGetCommand As DbCommand = loDatabase.GetStoredProcCommand(lsCommand)

        loDatabase.AddInParameter(loDbGetCommand, "@UserId", DbType.String, psUserId)

        loReportGroupsDS = loDatabase.ExecuteDataSet(loDbGetCommand)
        Return loReportGroupsDS.Tables(0)

    End Function
    ''' <summary>
    ''' Add Database Exp
    ''' </summary>
    ''' <param name="psCode"></param>
    ''' <param name="psDesc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateDatabaseExp(ByVal psCode As String, _
                                  ByVal psDesc As String _
                                  , ByVal psUserId As String) As Integer

        Dim liResult As Integer = 0
        Try
            Dim loDatabase As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim loSqlCommand As String = "Add_DatabaseExp"
            Dim loUpdateCommand As DbCommand = loDatabase.GetStoredProcCommand(loSqlCommand)

            loDatabase.AddInParameter(loUpdateCommand, "@UserId", DbType.String, psUserId)


            loDatabase.AddInParameter(loUpdateCommand, "@Code", DbType.String, psCode)

            loDatabase.AddInParameter(loUpdateCommand, "@Desc", DbType.String, psDesc)


            loDatabase.AddOutParameter(loUpdateCommand, "@intResult", DbType.Int16, 4)

            loDatabase.ExecuteNonQuery(loUpdateCommand)

            If loUpdateCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                liResult = Convert.ToInt32(loUpdateCommand.Parameters("@intResult").Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return liResult
    End Function
    ''' <summary>
    ''' Update Category 
    ''' </summary>
    ''' <param name="strCategoryId"></param>
    ''' <param name="strCategory"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDatabaseExp(ByVal psCode As String, _
                                      ByVal psDesc As String _
                                     , ByVal psUserId As String) As Integer

        Dim liResult As Integer = 0
        Try
            Dim loDatabase As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim loSqlCommand As String = "Update_DatabaseExp"
            Dim loUpdateCommand As DbCommand = loDatabase.GetStoredProcCommand(loSqlCommand)

            loDatabase.AddInParameter(loUpdateCommand, "@UserId", DbType.String, psUserId)
            loDatabase.AddInParameter(loUpdateCommand, "@Code", DbType.String, psCode)

            loDatabase.AddInParameter(loUpdateCommand, "@Desc", DbType.String, psDesc)


            loDatabase.AddOutParameter(loUpdateCommand, "@intResult", DbType.Int16, 4)

            loDatabase.ExecuteNonQuery(loUpdateCommand)

            If loUpdateCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                liResult = Convert.ToInt32(loUpdateCommand.Parameters("@intResult").Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return liResult
    End Function
    ''' <summary>
    ''' Delete Database Exp
    ''' </summary>
    ''' <param name="strSelectedCat"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteDatabaseExp(ByVal psSelectedCode As String, _
                                     ByVal psUserId As String) As Integer

        Dim liResult As Integer = 0
        Try
            Dim objDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim loSqlCommand As String = "Delete_DatabaseExp"
            Dim loDeleteCommand As DbCommand = objDB.GetStoredProcCommand(loSqlCommand)

            objDB.AddInParameter(loDeleteCommand, "@UserId", DbType.String, psUserId)
            objDB.AddInParameter(loDeleteCommand, "@Code", DbType.String, psSelectedCode)

            objDB.AddOutParameter(loDeleteCommand, "@intResult", DbType.Int16, 4)

            objDB.ExecuteNonQuery(loDeleteCommand)

            If loDeleteCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                liResult = Convert.ToInt32(loDeleteCommand.Parameters("@intResult").Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return liResult
    End Function
End Class
