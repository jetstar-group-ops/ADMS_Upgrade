Imports WNB_Admin_DAL

Public Class ADMS_BAL_CRR

    Dim GOADMS_DAL_CRR As New ADMS_DAL_CRR

    Public Function GetChangeRequestReport(ByVal psUserId As String, _
                                  ByVal psStartDate As String, _
                                  ByVal psEndDate As String) As DataSet

        Return GOADMS_DAL_CRR.GetChangeRequestReport(psUserId, psStartDate, psEndDate)

    End Function

End Class
