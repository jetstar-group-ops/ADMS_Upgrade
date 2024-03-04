
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO
Imports System.Globalization

Public Class WNB_Admin_DAL

#Region "--Variables--"
    Private DbUserName As String = ConfigurationSettings.AppSettings("DbOwnerForWNB") & "."

#End Region


    Public Function GetFunctionalities(ByVal ParentFunctionalityId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StationsDS As DataSet = Nothing

        Try

            Dim sqlCommand As String = DbUserName & "Get_Functionalities"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "ParentFunctionalityId", DbType.Int64, _
                    IIf(ParentFunctionalityId = "", DBNull.Value, ParentFunctionalityId))


            StationsDS = db.ExecuteDataSet(dbGetCommand)

            Return StationsDS.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GetReportsForFunctionality(ByVal FunctionalityId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim ReportGroupsDS As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Reports_Functionalities"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "FunctionalityId", DbType.Int32, _
                              IIf(FunctionalityId = "", System.DBNull.Value, FunctionalityId))

            ReportGroupsDS = db.ExecuteDataSet(dbGetCommand)

            Return ReportGroupsDS.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GetUserDetails(ByVal ps_UserId As String, _
                                   ByVal ps_Password As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StationsDS As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_User_Details"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "UserId", DbType.String, ps_UserId)
            db.AddInParameter(dbGetCommand, "Password", DbType.String, ps_Password)
            StationsDS = db.ExecuteDataSet(dbGetCommand)

            Return StationsDS.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Function ChangePwd(ByVal psUserId As String, ByVal NewPwd As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer = 0
        Try

            Dim sqlCommand As String = DbUserName & "Change_PWD"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@UserID", DbType.String, psUserId)
            db.AddInParameter(InsertUpdateCommand, "@NewPwd", DbType.String, NewPwd)

            db.ExecuteNonQuery(InsertUpdateCommand)

            intResult = 1

        Catch ex As Exception
            intResult = 0
            Throw ex
        End Try

        Return intResult

    End Function


    Public Function IsUserHasPermission(ByVal UserId As String, _
                                      ByVal FunctionalityId As Integer, _
                                      ByVal LocationId As String, _
                                      ByVal ParentFunctionality As Integer) As Boolean
        Dim lo_UserRoles As DataTable
        Dim lo_RoleFunctionalityLocations As DataTable
        Dim FunctionResult As Boolean = False
        Dim Result As Boolean = False

        lo_UserRoles = GetUserProfile(UserId, 0)

        For I = 0 To lo_UserRoles.Rows.Count - 1

            If lo_UserRoles.Rows(I)("Role_Id") IsNot DBNull.Value Then

                Result = IsRoleHasFunctionality(lo_UserRoles.Rows(I)("Role_Id"), _
                                   WNB_Common.Enums.Functionalities.SystemAdministration)
                If Result = True Then
                    FunctionResult = True
                    Exit For
                End If

                Result = IsRoleHasFunctionality(lo_UserRoles.Rows(I)("Role_Id"), FunctionalityId)

                If Result = True Then
                    If LocationId = "" Then
                        FunctionResult = True
                        Exit For
                    Else
                        'if given location is user's base location
                        If LocationId = lo_UserRoles.Rows(I)("Location_Id") Then
                            FunctionResult = True
                            Exit For
                        Else
                            lo_RoleFunctionalityLocations = _
                            GetRoleFunctionalityLocations(lo_UserRoles.Rows(I)("Role_Id"), FunctionalityId)

                            For J = 0 To lo_RoleFunctionalityLocations.Rows.Count - 1
                                If lo_RoleFunctionalityLocations.Rows(J)("Location_Id") = LocationId Then
                                    FunctionResult = True
                                    Exit For
                                End If
                            Next
                        End If

                    End If

                Else
                    Result = IsRoleHasFunctionality(lo_UserRoles.Rows(I)("Role_Id"), ParentFunctionality)
                    If Result = True Then
                        FunctionResult = True
                        Exit For
                    Else
                        Result = IsRoleHasFunctionality(lo_UserRoles.Rows(I)("Role_Id"), _
                                    WNB_Common.Enums.Functionalities.SystemAdministration)
                        If Result = True Then
                            FunctionResult = True
                            Exit For
                        End If
                    End If
                End If

                If FunctionResult = True Then Exit For
            End If
        Next

        Return FunctionResult
    End Function

    Public Function GetUserProfile(ByVal ps_UserId As String, ByVal UserSNO As Integer) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StationsDS As DataSet = Nothing
        Try
            Dim sqlCommand As String = DbUserName & "Get_User_Profile"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "UserId", DbType.String, ps_UserId)
            db.AddInParameter(dbGetCommand, "UserSNO", DbType.String, UserSNO)

            StationsDS = db.ExecuteDataSet(dbGetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return StationsDS.Tables(0)

    End Function

    Public Function IsRoleHasFunctionality(ByVal roleId As Integer, _
                                           ByVal FunctionalityId As Integer) As Boolean

        Dim loroleFunctionalities As DataTable
        Dim Result As Boolean = False

        Try
            loroleFunctionalities = GetRoleFunctionalities(roleId)

            For I = 0 To loroleFunctionalities.Rows.Count - 1
                If loroleFunctionalities.Rows(I)("Functionality_Id") = FunctionalityId Then
                    Result = True
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try

        Return Result

    End Function

    Public Function GetRoleFunctionalities(ByVal RoleId As Integer) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RoleFunctionalitiesDS As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Role_Functionalities"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "RoleId", DbType.Int64, RoleId)

            RoleFunctionalitiesDS = db.ExecuteDataSet(dbGetCommand)
        Catch ex As Exception
            Throw ex
        End Try

        Return RoleFunctionalitiesDS.Tables(0)

    End Function

    Public Function GetRoleFunctionalityLocations(ByVal RoleId As Integer, _
                                         ByVal FunctionalityId As Integer) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RoleFunctionalitiesDS As DataSet = Nothing

        Try

            Dim sqlCommand As String = DbUserName & "Get_Role_Functionality_Locations"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "RoleId", DbType.Int64, RoleId)
            db.AddInParameter(dbGetCommand, "FunctionalityId", DbType.Int64, FunctionalityId)

            RoleFunctionalitiesDS = db.ExecuteDataSet(dbGetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return RoleFunctionalitiesDS.Tables(0)

    End Function


    Public Function GetUsers() As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StationsDS As DataSet = Nothing

        Try

            Dim sqlCommand As String = DbUserName & "Get_Users"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            StationsDS = db.ExecuteDataSet(dbGetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return StationsDS.Tables(0)

    End Function


    Public Function GetRoles() As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RolesDS As DataSet = Nothing
        Try

            Dim sqlCommand As String = DbUserName & "Get_Roles"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            RolesDS = db.ExecuteDataSet(dbGetCommand)
        Catch ex As Exception
            Throw ex
        End Try

        Return RolesDS.Tables(0)

    End Function


    Public Function GetStations() As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StationsDS As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Stations"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            StationsDS = db.ExecuteDataSet(dbGetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return StationsDS.Tables(0)

    End Function

    Public Function CreateUpdateUserProfile(ByVal UserId As String, _
                                      ByVal LocationId As String, _
                                      ByVal Password As String, _
                                      ByVal IsDisabled As Integer, _
                                      ByVal RoleIds As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer = 0

        Try

            Dim sqlCommand As String = DbUserName & "Create_Update_User_Profile"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "UserId", DbType.String, UserId)
            db.AddInParameter(InsertUpdateCommand, "LocationId", DbType.String, LocationId)
            db.AddInParameter(InsertUpdateCommand, "Password", DbType.String, Password)
            db.AddInParameter(InsertUpdateCommand, "IsDisabled", DbType.String, IsDisabled)
            db.AddInParameter(InsertUpdateCommand, "RoleIds", DbType.String, _
                              IIf(RoleIds = "", System.DBNull.Value, RoleIds))

            db.ExecuteNonQuery(InsertUpdateCommand)

            intResult = 1
        Catch ex As Exception
            intResult = 0
            Throw ex
        End Try

        Return intResult

    End Function


    Public Function GetPagesFunctionalities(ByVal ParentFunctionalityId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim StationsDS As DataSet = Nothing
        Try

            Dim sqlCommand As String = DbUserName & "Get_Pages_Functionalities"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            StationsDS = db.ExecuteDataSet(dbGetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return StationsDS.Tables(0)

    End Function

    Public Function IsRoleNameExists(ByVal RoleName As String, ByVal RoleId As Integer) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim RoleNameExistCount As Integer

        Try


            Dim sqlCommand As String = DbUserName & "IsRoleNameExists"
            Dim IsExistCheckCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(IsExistCheckCommand, "@RoleId", DbType.Int32, RoleId)
            db.AddInParameter(IsExistCheckCommand, "@RoleName", DbType.String, RoleName)
            db.AddOutParameter(IsExistCheckCommand, "@Count", DbType.String, 5)

            db.ExecuteNonQuery(IsExistCheckCommand)

            RoleNameExistCount = Convert.ToInt32(IsExistCheckCommand.Parameters("@Count").Value)

            If RoleNameExistCount > 0 Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Function CreateRole(ByVal RoleName As String, _
                     ByVal RoleDescription As String, _
                     ByVal RoleFunctionalities As String, _
                     ByVal CreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Role"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "RoleName", DbType.String, RoleName)
            db.AddInParameter(InsertUpdateCommand, "RoleDescription", DbType.String, RoleDescription)
            db.AddInParameter(InsertUpdateCommand, "FunctionalitiesIDs", DbType.String, IIf(RoleFunctionalities = "", DBNull.Value, RoleFunctionalities))
            db.AddInParameter(InsertUpdateCommand, "CreatedBy", DbType.String, CreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function UpdateRole(ByVal RoleId As Integer, ByVal RoleName As String, _
                        ByVal RoleDescription As String, _
                        ByVal RoleFunctionalities As String, _
                        ByVal ModifyBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try

            Dim sqlCommand As String = DbUserName & "Update_Role"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "RoleId", DbType.String, RoleId)
            db.AddInParameter(InsertUpdateCommand, "RoleName", DbType.String, RoleName)
            db.AddInParameter(InsertUpdateCommand, "RoleDescription", DbType.String, RoleDescription)
            db.AddInParameter(InsertUpdateCommand, "FunctionalitiesIDs", DbType.String, IIf(RoleFunctionalities = "", DBNull.Value, RoleFunctionalities))
            db.AddInParameter(InsertUpdateCommand, "UpdatedBy", DbType.String, ModifyBy)


            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function DeleteRole(ByVal intRoleId As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_Role"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "RoleId", DbType.Int32, intRoleId)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function

    Public Function DeleteUser(ByVal intUserSNO As Integer, ByVal strUserId As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_User"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "UserId", DbType.String, strUserId)
            db.AddInParameter(DeleteCommand, "UserSNO", DbType.Int32, intUserSNO)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function

    Public Function GetDBDetails() As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsDBDetails As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Latest_DB_Details"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            dsDBDetails = db.ExecuteDataSet(dbGetCommand)

            Return dsDBDetails.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
End Class
