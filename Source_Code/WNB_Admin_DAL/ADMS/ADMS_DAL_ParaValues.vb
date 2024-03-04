
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common

Public Class ADMS_DAL_ParaValues

    Public Function GetParameters(ByVal psUserId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim ReportGroupsDS As DataSet = Nothing
        Dim sqlCommand As String = "[Get_Parameter]"
        Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

        db.AddInParameter(dbGetCommand, "@UserId", DbType.String, psUserId)

        ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)
        Return ReportGroupsDS.Tables(0)

    End Function


    ''' <summary>
    ''' Update Parameter Value
    ''' </summary>
    ''' <param name="sngParmeterValue"></param>
    ''' <param name="strParameterName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateParameter(ByVal sngParmeterValue As Single, _
                                      ByVal strParameterName As String _
                                  , ByVal psUserId As String) As Integer

        Dim intResult As Integer = 0
        Try
            Dim objDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")

            Dim sqlCommand As String = "Update_Parameter"
            Dim UpdateCommand As DbCommand = objDB.GetStoredProcCommand(sqlCommand)

            objDB.AddInParameter(UpdateCommand, "@UserId", DbType.String, psUserId)
            objDB.AddInParameter(UpdateCommand, "@ParameterName", DbType.String, strParameterName)

            objDB.AddInParameter(UpdateCommand, "@LimitValue", DbType.Single, sngParmeterValue)


            objDB.AddOutParameter(UpdateCommand, "@intResult", DbType.Int16, 4)

            objDB.ExecuteNonQuery(UpdateCommand)

            If UpdateCommand.Parameters("@intResult").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt32(UpdateCommand.Parameters("@intResult").Value)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function
End Class
