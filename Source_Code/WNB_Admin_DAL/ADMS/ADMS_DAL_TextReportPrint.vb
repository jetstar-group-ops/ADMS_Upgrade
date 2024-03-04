Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO

Public Class ADMS_DAL_TextReportPrint

    Public Function GetTextReportPrint(ByVal psUserId As String, _
                                 ByVal psIcao As String, _
                                 ByVal psRwyId As String, _
                                 ByVal psRwyMod As String, _
                                 ByVal psAirlineCode As String) As DataSet

        Dim loDatabase As Database = DatabaseFactory.CreateDatabase("WNBConnectionString")
        Dim loChangeRequestReportDS As DataSet = Nothing
        Dim lsSqlCommand As String = "Get_TextPrint_Report"
        Dim lodbGetCommand As DbCommand = loDatabase.GetStoredProcCommand(lsSqlCommand)

        loDatabase.AddInParameter(lodbGetCommand, "UserId", DbType.String, IIf(psUserId = "", DBNull.Value, psUserId))

        loDatabase.AddInParameter(lodbGetCommand, "@Icao", DbType.String, IIf(psIcao = "", DBNull.Value, psIcao))
        loDatabase.AddInParameter(lodbGetCommand, "@RwyId", DbType.String, IIf(psRwyId = "", DBNull.Value, psRwyId))
        loDatabase.AddInParameter(lodbGetCommand, "@RwyMod", DbType.String, IIf(psRwyMod = "", DBNull.Value, psRwyMod))
        loDatabase.AddInParameter(lodbGetCommand, "@AirlineCode", DbType.String, IIf(psAirlineCode = "", DBNull.Value, psAirlineCode))

        loChangeRequestReportDS = loDatabase.ExecuteDataSet(lodbGetCommand)
        Return loChangeRequestReportDS

    End Function

End Class
