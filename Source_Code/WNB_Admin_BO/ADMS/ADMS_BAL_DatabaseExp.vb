Imports WNB_Admin_DAL

Public Class ADMS_BAL_DatabaseExp

    Dim goADMS_DAL_DbExp As New ADMS_DAL_DatabaseExp
    Public Function GetDbExport(ByVal psUserId As String) As DataTable
        Return goADMS_DAL_DbExp.GetDatabaseExp(psUserId)
    End Function

    Public Function CreateDbExport(ByVal strCode As String, _
                                        ByVal strDesc As String _
                                  , ByVal psUserId As String) As Integer
        Return goADMS_DAL_DbExp.CreateDatabaseExp(strCode, strDesc, psUserId)
    End Function

    Public Function UpdateDbExport(ByVal strCode As String, _
                                        ByVal strDesc As String _
                                 , ByVal psUserId As String) As Integer
        Return goADMS_DAL_DbExp.UpdateDatabaseExp(strCode, strDesc, psUserId)
    End Function

    Public Function DeleteDbExport(ByVal strSelectedCat As String, _
                                   ByVal psUserId As String) As Integer

        Return goADMS_DAL_DbExp.DeleteDatabaseExp(strSelectedCat, psUserId)

    End Function

End Class
