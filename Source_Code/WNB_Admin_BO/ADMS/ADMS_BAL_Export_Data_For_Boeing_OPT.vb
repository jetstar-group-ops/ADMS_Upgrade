Imports WNB_Admin_DAL

Public Class ADMS_BAL_Export_Data_For_Boeing_OPT

    Dim goADMS_DAL_Export_Data_For_Boeing_OPT As New ADMS_DAL_Export_Data_For_Boeing_OPT

    Public Sub ExportDataForBoeingOptDatabase(ByVal psUserId As String, _
            ByVal poBoeingOptData As DataSet, _
            ByVal psExportedDbFile As String, _
            ByVal psEmptyBoeingOptSqlieDbFile As String, _
            ByVal PsAiracCode As String)

        goADMS_DAL_Export_Data_For_Boeing_OPT.ExportDataForBoeingOptDatabase(psUserId, _
                            poBoeingOptData, psExportedDbFile, psEmptyBoeingOptSqlieDbFile, PsAiracCode)

    End Sub

    Public Function GetDataForBoeingOptDatabaseExport(ByVal psUserId As String, _
                    ByVal psActive As String) As DataSet
        Return goADMS_DAL_Export_Data_For_Boeing_OPT.GetDataForBoeingOptDatabaseExport(psUserId, psActive)
    End Function

End Class
