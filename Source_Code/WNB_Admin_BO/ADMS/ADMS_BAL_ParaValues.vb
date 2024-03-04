Imports WNB_Admin_DAL

Public Class ADMS_BAL_ParaValues

    Dim goADMS_DAL_PV As New ADMS_DAL_ParaValues

    Public Function GetParameters(ByVal psUserId As String) As DataTable
        Return goADMS_DAL_PV.GetParameters(psUserId)
    End Function

    Public Function UpdateParameter(ByVal strParameterName As String, _
                                         ByVal sngSelectedValue As Single _
                                  , ByVal psUserId As String) As Integer
        Return goADMS_DAL_PV.UpdateParameter(sngSelectedValue, strParameterName, psUserId)
    End Function

End Class
