Imports WNB_Admin_DAL

Public Class ADMS_BAL_APTCategory

    Dim goADMS_DAL_CT As New ADMS_DAL_APTCategory

    Public Function GetCategories(ByVal psUserId As String) As DataTable
        Return goADMS_DAL_CT.GetCategories(psUserId)
    End Function

    Public Function CreateCategory(ByVal strCategoryId As String, _
                                        ByVal strCategory As String _
                                  , ByVal psUserId As String) As Integer
        Return goADMS_DAL_CT.CreateCategory(strCategoryId, strCategory, psUserId)
    End Function

    Public Function UpdateCategory(ByVal strCategoryId As String, _
                                        ByVal strCategory As String _
                                 , ByVal psUserId As String) As Integer
        Return goADMS_DAL_CT.UpdateCategory(strCategoryId, strCategory, psUserId)
    End Function

    Public Function DeleteCategory(ByVal strSelectedCat As String, _
                                   ByVal psUserId As String) As Integer

        Return goADMS_DAL_CT.DeleteCategory(strSelectedCat, psUserId)

    End Function

End Class
