Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO

Public Class ADMS_DAL_Common

    Public Function GetParameterTable(ByVal psUserId As String) As DataTable

        Dim loDB As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim loParamettersDataSet As DataSet = Nothing
        Dim lsSqlCommand As String = "Get_GetParameterTable"
        Dim loDbGetCommand As DbCommand = loDB.GetStoredProcCommand(lsSqlCommand)

        loDB.AddInParameter(loDbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        loParamettersDataSet = loDB.ExecuteDataSet(loDbGetCommand)

        Return loParamettersDataSet.Tables(0)

    End Function

End Class
