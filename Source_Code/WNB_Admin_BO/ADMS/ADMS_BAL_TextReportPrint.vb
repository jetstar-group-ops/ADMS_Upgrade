
Imports WNB_Admin_DAL

Public Class ADMS_BAL_TextReportPrint

    Dim GOADMS_DAL_TextReportPrint As New ADMS_DAL_TextReportPrint

    Public Function GetChangeRequestReport(ByVal psUserId As String, _
                                 ByVal psIcao As String, _
                                 ByVal psRwyId As String, _
                                 ByVal psRwyMod As String, _
                                 ByVal psAirlineCode As String) As DataSet

        Return GOADMS_DAL_TextReportPrint.GetTextReportPrint(psUserId, psIcao, psRwyId, psRwyMod, psAirlineCode)

    End Function


End Class
