Imports WNB_Admin_DAL

Public Class ADMS_BAL_AptMaxExp

    Dim goADMS_DAL_AMaxExp As New ADMS_DAL_AptMaxExp
    Public Function GetAptMaxExp(ByVal psUserId As String) As DataTable
        Return goADMS_DAL_AMaxExp.GetAptMaxExp(psUserId)
    End Function

    Public Function CreateAptMaxExp(ByVal strCode As String, _
                                        ByVal strDesc As String _
                                  , ByVal psUserId As String) As Integer
        Return goADMS_DAL_AMaxExp.CreateAptMaxExp(strCode, strDesc, psUserId)
    End Function

    Public Function UpdateAptMaxExp(ByVal strCode As String, _
                                        ByVal strDesc As String _
                                 , ByVal psUserId As String) As Integer
        Return goADMS_DAL_AMaxExp.UpdateAptMaxExp(strCode, strDesc, psUserId)
    End Function

    Public Function DeleteAptMaxExp(ByVal strSelectedCat As String, _
                                   ByVal psUserId As String) As Integer

        Return goADMS_DAL_AMaxExp.DeleteAptMaxExp(strSelectedCat, psUserId)

    End Function

End Class
