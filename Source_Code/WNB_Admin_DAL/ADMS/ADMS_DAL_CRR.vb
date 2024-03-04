Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO

Public Class ADMS_DAL_CRR

    Public Function GetChangeRequestReport(ByVal psUserId As String, _
                                  ByVal psStartDate As String, _
                                  ByVal psEndDate As String) As DataSet

        Dim loDatabase As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim loChangeRequestReportDS As DataSet = Nothing
        Dim lsSqlCommand As String = "Get_Change_Record_Report"
        Dim lodbGetCommand As DbCommand = loDatabase.GetStoredProcCommand(lsSqlCommand)

        loDatabase.AddInParameter(lodbGetCommand, "UserId", DbType.String, _
                          IIf(psUserId = "", DBNull.Value, psUserId))

        loDatabase.AddInParameter(lodbGetCommand, "StartDate", DbType.DateTime, Date.Parse(psStartDate))
        loDatabase.AddInParameter(lodbGetCommand, "EndDate", DbType.DateTime, Date.Parse(psEndDate))

        loChangeRequestReportDS = loDatabase.ExecuteDataSet(lodbGetCommand)
        Return loChangeRequestReportDS

    End Function

End Class
