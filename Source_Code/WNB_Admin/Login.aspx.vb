Imports System.Data
Imports System.IO
Imports System.Configuration.ConfigurationManager
Partial Public Class Login1
    Inherits System.Web.UI.Page


    Private Sub Login1_Authenticate(ByVal sender As Object, _
                                   ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) _
                                   Handles Login1.Authenticate

        '******* Remember me Functionality *****************

        Dim securityCookie As New HttpCookie("SecurityCookie")

        If Login1.RememberMeSet Then
            Dim persistDays As Int32 = 0
            persistDays = Convert.ToInt32(AppSettings("RemebermeDays").ToString())

            'By Default for Seven Days
            Dim regex As New Regex("<(.|" & vbLf & ")+?>", RegexOptions.IgnoreCase Or RegexOptions.Multiline)
            Dim strTxtUserName As String = regex.Replace(Login1.UserName, String.Empty)
            Dim strTxtPassword As String = regex.Replace(Login1.Password, String.Empty)

            securityCookie("UserName") = strTxtUserName
            securityCookie("Password") = strTxtPassword
            securityCookie.Expires = DateTime.Now.AddDays(persistDays)
        Else
            securityCookie("UserName") = String.Empty
            securityCookie("Password") = String.Empty
            securityCookie.Expires = DateTime.Now.AddMinutes(1)

        End If

        Response.Cookies.Add(securityCookie)
        '******* Remember me Functionality *****************

        Dim objBo As New WNB_Admin_BO.WNB_Admin_BO()
        Dim dtUserDetails As DataTable
        Dim ls_FunctionalityIDs As String = ""

        Try
            dtUserDetails = objBo.GetUserDetails(Login1.UserName, Login1.Password)

            If dtUserDetails.Rows.Count = 0 Then
                e.Authenticated = False
                Login1.FailureText = "Invalid User ID or Password."
                Login1.Focus()
            Else
                If dtUserDetails.Rows(0)("IsDisabled") IsNot System.DBNull.Value Then
                    If dtUserDetails.Rows(0)("IsDisabled") = "1" Then
                        e.Authenticated = False
                        Login1.FailureText = "Your account is currently disabled, please contact to JFIDS System Administrators."
                        Login1.Focus()
                    Else
                        e.Authenticated = True
                    End If
                Else
                    e.Authenticated = True
                End If
            End If

            If e.Authenticated = True Then

                Session("UserLocationId") = dtUserDetails.Rows(0)("Location_Id") & ""
                Session("UserLocationName") = dtUserDetails.Rows(0)("Location_Name") & ""
                Session("UserID") = dtUserDetails.Rows(0)("User_ID")

                'Entry for ADMS 14.01.2015
                Session("AirlineCode") = dtUserDetails.Rows(0)("AirlineCode") & ""
                'Entry for ADMS 14.01.2015

                Login1.DestinationPageUrl = "Home.aspx"

               
            End If
        Catch ex As Exception

            LogException(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub LogException(ByVal strException As String)
        
        If (AppSettings("ScriptDisplay").ToString() <> String.Empty) Then
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), "Message", "alert('" + strException.ToString() + "');", True)
        End If


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim objMasterPage As MasterPage
        objMasterPage = Master

        If IsPostBack = False Then
            Session("UserId") = Nothing
            Session.Abandon()
        End If
        
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not IsPostBack Then
            If Request.Cookies("SecurityCookie") IsNot Nothing Then
                Dim cookie As HttpCookie = Request.Cookies.[Get]("SecurityCookie")

                Dim strUserName As String = cookie("UserName").ToString()
                If Not strUserName.Equals(String.Empty) Then
                    Login1.UserName = strUserName
                End If
                Dim strPassword As String = cookie("PassWord").ToString()
                If Not strPassword.Equals(String.Empty) Then
                    Dim tb As TextBox = DirectCast(Login1.FindControl("Password"), TextBox)
                    tb.Attributes("Value") = strPassword
                    Login1.RememberMeSet = True
                End If
            End If
        End If
        Response.Cache.SetNoStore()
    End Sub
End Class