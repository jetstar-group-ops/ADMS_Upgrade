Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data
Imports System.Data.Common
Imports System.Configuration
Imports System.Net
Imports System.Reflection
Imports System.IO
Imports System.Globalization
Imports System.Data.SqlClient

Public Class DAL_Manager


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

    Public Function Is_Database_Upgrade_Running() As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Is_Database_Upgrade_Running"
            Dim GetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddOutParameter(GetCommand, "@IsDatabaseUpgradeRunning", DbType.Int16, 4)

            db.ExecuteNonQuery(GetCommand)

            If GetCommand.Parameters("@IsDatabaseUpgradeRunning").Value.ToString() <> String.Empty Then
                intResult = Convert.ToInt16(GetCommand.Parameters("@IsDatabaseUpgradeRunning").Value)
            End If


        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function

    Public Sub Start_Database_Upgrade_Session()
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Try
            Dim sqlCommand As String = DbUserName & "Start_Database_Upgrade_Session"
            Dim GetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.ExecuteNonQuery(GetCommand)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Sub Cancel_Database_Upgrade_Session()
        Dim db As Database = DatabaseFactory.CreateDatabase()

        Try
            Dim sqlCommand As String = DbUserName & "Cancel_Database_Upgrade_Session"
            Dim GetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.ExecuteNonQuery(GetCommand)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

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

    Public Function GetDBTableData(ByVal strTableId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsDBTableData As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Table_Records"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)

            dsDBTableData = db.ExecuteDataSet(dbGetCommand)

            Return dsDBTableData.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Function DeleteAircraft(ByVal strAircraftId As String, ByVal intVersionNo As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_Aircraft"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "AircraftId", DbType.String, strAircraftId)
            db.AddInParameter(DeleteCommand, "@VersionNo", DbType.Int32, intVersionNo)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function



    Public Function CreateAircraft(ByVal strAircraftId As String, _
                ByVal strModelName As String, _
                ByVal strRefChordOrigin As String, _
                ByVal strRefChordLength As String, _
                ByVal strRefStation As String, _
                ByVal strIUEquConstC As String, _
                ByVal strIUEquConstK As String, _
                ByVal strMinOpWeightAdj As String, _
                ByVal strMaxOpWeightAdj As String, _
                ByVal strMinOpIUAdj As String, _
                ByVal strMaxOpIUAdj As String, _
                ByVal intVersionNo As Integer, _
                ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_AirCraft"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            db.AddInParameter(InsertUpdateCommand, "@Model_name", DbType.String, strModelName)

            If strRefChordOrigin.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_origin", DbType.Decimal, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_origin", DbType.Decimal, Convert.ToDecimal(strRefChordOrigin))
            End If

            If strRefChordLength.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_length", DbType.Decimal, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_length", DbType.Decimal, Convert.ToDecimal(strRefChordLength))
            End If

            If strRefStation.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Ref_station", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Ref_station", DbType.Int32, Convert.ToInt32(strRefStation))
            End If

            If strIUEquConstC.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_C", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_C", DbType.Int32, Convert.ToInt32(strIUEquConstC))
            End If

            If strIUEquConstK.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_K", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_K", DbType.Int32, Convert.ToInt32(strIUEquConstK))
            End If

            If strMinOpWeightAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Min_op_weight_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Min_op_weight_adjustment", DbType.Int32, Convert.ToInt32(strMinOpWeightAdj))
            End If

            If strMaxOpWeightAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Max_op_weight_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Max_op_weight_adjustment", DbType.Int32, Convert.ToInt32(strMaxOpWeightAdj))
            End If

            If strMinOpIUAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Min_op_IU_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Min_op_IU_adjustment", DbType.Int32, Convert.ToInt32(strMinOpIUAdj))
            End If

            If strMaxOpIUAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Max_op_IU_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Max_op_IU_adjustment", DbType.Int32, Convert.ToInt32(strMaxOpIUAdj))
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, strCreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function



    Public Function UpdateAircraft(ByVal strAircraftId As String, _
                ByVal strModelName As String, _
                ByVal strRefChordOrigin As String, _
                ByVal strRefChordLength As String, _
                ByVal strRefStation As String, _
                ByVal strIUEquConstC As String, _
                ByVal strIUEquConstK As String, _
                ByVal strMinOpWeightAdj As String, _
                ByVal strMaxOpWeightAdj As String, _
                ByVal strMinOpIUAdj As String, _
                ByVal strMaxOpIUAdj As String, _
                ByVal intVersionNo As Integer, _
                ByVal strUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try

            Dim sqlCommand As String = DbUserName & "Update_AirCraft"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            db.AddInParameter(InsertUpdateCommand, "@Model_name", DbType.String, strModelName)

            If strRefChordOrigin.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_origin", DbType.Decimal, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_origin", DbType.Decimal, Convert.ToDecimal(strRefChordOrigin))
            End If

            If strRefChordLength.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_length", DbType.Decimal, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Ref_chord_length", DbType.Decimal, Convert.ToDecimal(strRefChordLength))
            End If

            If strRefStation.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Ref_station", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Ref_station", DbType.Int32, Convert.ToInt32(strRefStation))
            End If

            If strIUEquConstC.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_C", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_C", DbType.Int32, Convert.ToInt32(strIUEquConstC))
            End If

            If strIUEquConstK.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_K", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@IU_equ_const_K", DbType.Int32, Convert.ToInt32(strIUEquConstK))
            End If

            If strMinOpWeightAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Min_op_weight_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Min_op_weight_adjustment", DbType.Int32, Convert.ToInt32(strMinOpWeightAdj))
            End If

            If strMaxOpWeightAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Max_op_weight_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Max_op_weight_adjustment", DbType.Int32, Convert.ToInt32(strMaxOpWeightAdj))
            End If

            If strMinOpIUAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Min_op_IU_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Min_op_IU_adjustment", DbType.Int32, Convert.ToInt32(strMinOpIUAdj))
            End If

            If strMaxOpIUAdj.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Max_op_IU_adjustment", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Max_op_IU_adjustment", DbType.Int32, Convert.ToInt32(strMaxOpIUAdj))
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function


    Public Function UpdateAircraftConfig(ByVal strAircraftId As String, _
                ByVal intACId As String, _
                ByVal strTankRef As String, _
                ByVal strAirconfig As String, _
                ByVal strFuelTank As String, _
                ByVal intVersionNo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try

            Dim sqlCommand As String = DbUserName & "Update_AirCraft_Config"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            If intACId = 0 Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Config_Id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Config_Id", DbType.Int32, intACId)
            End If

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_id", DbType.String, Convert.ToString(strAircraftId))
            End If

            If strTankRef.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@tank_ref_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@tank_ref_cl_id", DbType.String, Convert.ToString(strTankRef))
            End If

            If strAirconfig.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@air_config_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@air_config_cl_id", DbType.String, Convert.ToString(strAirconfig))
            End If

            If strFuelTank.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@fuel_in_tank_allowed", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@fuel_in_tank_allowed", DbType.String, Convert.ToString(strFuelTank))
            End If



            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function Get_Aircrafts(ByVal strAircraftId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAircrafts As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Aircrafts"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            dsAircrafts = db.ExecuteDataSet(dbGetCommand)

            Return dsAircrafts.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Get_Aircraft_Config(ByVal strTableId As String, ByVal strAircraftId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAirConfig As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Aircraft_Config"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)
            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            dsAirConfig = db.ExecuteDataSet(dbGetCommand)

            Return dsAirConfig.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function


#Region "Registration"

    Public Function Get_Registration(ByVal strAircraftId As String, ByVal RegistrationID As Int32) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAircrafts As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Registration"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            If RegistrationID.Equals(0) Then
                db.AddInParameter(dbGetCommand, "@Registration_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Registration_Id", DbType.String, RegistrationID)
            End If

            dsAircrafts = db.ExecuteDataSet(dbGetCommand)

            Return dsAircrafts.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CreateRegistration(ByVal Registration_Id As Integer, _
             ByVal Registration_Number As String, _
             ByVal Aircraft_Id As String, _
             ByVal MSN As String, _
             ByVal Seat_Configuration As String, _
             ByVal Load_Data_Sheet_Sef As String, _
             ByVal Basic_Weight As Int32, _
             ByVal Basic_Arm As Int32, _
             ByVal Subfleet_Id As String, _
             ByVal intVersionNo As Integer, _
             ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Registration"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@Registration_Id", DbType.Int32, Registration_Id)
            db.AddInParameter(InsertUpdateCommand, "@Registration_Number", DbType.String, Registration_Number)

            If Aircraft_Id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, Aircraft_Id)
            End If

            db.AddInParameter(InsertUpdateCommand, "@MSN", DbType.Int32, MSN)

            If Seat_Configuration.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Seat_Configuration", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Seat_Configuration", DbType.Int32, Convert.ToInt32(Seat_Configuration))
            End If

            If Load_Data_Sheet_Sef.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Load_Data_Sheet_Sef", DbType.Decimal, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Load_Data_Sheet_Sef", DbType.Decimal, Convert.ToDecimal(Load_Data_Sheet_Sef))
            End If



            db.AddInParameter(InsertUpdateCommand, "@Basic_Weight", DbType.Int32, Basic_Weight)
            db.AddInParameter(InsertUpdateCommand, "@Basic_Arm", DbType.Int32, Basic_Arm)

            If Subfleet_Id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, Subfleet_Id)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, strCreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function UpdateRegistration(ByVal Registration_Id As Integer, _
            ByVal Registration_Number As String, _
            ByVal Aircraft_Id As String, _
            ByVal MSN As String, _
            ByVal Seat_Configuration As String, _
            ByVal Load_Data_Sheet_Sef As String, _
            ByVal Basic_Weight As Int32, _
            ByVal Basic_Arm As Int32, _
            ByVal Subfleet_Id As String, _
            ByVal intVersionNo As Integer, _
            ByVal strUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Update_Registration"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@Registration_Id", DbType.Int32, Registration_Id)
            db.AddInParameter(InsertUpdateCommand, "@Registration_Number", DbType.String, Registration_Number)

            If Aircraft_Id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, Aircraft_Id)
            End If

            db.AddInParameter(InsertUpdateCommand, "@MSN", DbType.Int32, MSN)

            If Seat_Configuration.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Seat_Configuration", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Seat_Configuration", DbType.Int32, Convert.ToInt32(Seat_Configuration))
            End If

            If Load_Data_Sheet_Sef.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Load_Data_Sheet_Sef", DbType.Decimal, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Load_Data_Sheet_Sef", DbType.Decimal, Convert.ToDecimal(Load_Data_Sheet_Sef))
            End If



            db.AddInParameter(InsertUpdateCommand, "@Basic_Weight", DbType.Int32, Basic_Weight)
            db.AddInParameter(InsertUpdateCommand, "@Basic_Arm", DbType.Int32, Basic_Arm)

            If Subfleet_Id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, Subfleet_Id)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

    Public Function DeleteRegistration(ByVal RegistrationId As Int32, ByVal intVersionNo As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_Registration"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "@strRegistrationId", DbType.Int32, RegistrationId)
            db.AddInParameter(DeleteCommand, "@VersionNo", DbType.Int32, intVersionNo)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function

#End Region



#Region "Subfleet"
    Public Function CreateSubfleet(ByVal strSubfleetId As String, _
           ByVal strAircraftId As String, _
           ByVal strMaxTaxiWeight As String, _
           ByVal strMaxTakeoffWeight As String, _
           ByVal strMaxLandingWeight As String, _
           ByVal strMaxZeroFuelWeight As String, _
           ByVal strFlightDeckWeight As String, _
           ByVal strCabinCrewWeight As String, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Subfleet"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, strSubfleetId)
            db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)

            If strMaxTaxiWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_taxi_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_taxi_weight", DbType.Int32, Convert.ToDecimal(strMaxTaxiWeight))
            End If

            If strMaxTakeoffWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_takeoff_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_takeoff_weight", DbType.Int32, Convert.ToDecimal(strMaxTakeoffWeight))
            End If

            If strMaxLandingWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_landing_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_landing_weight", DbType.Int32, Convert.ToInt32(strMaxLandingWeight))
            End If

            If strMaxZeroFuelWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_zero_fuel_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_zero_fuel_weight", DbType.Int32, Convert.ToInt32(strMaxZeroFuelWeight))
            End If

            If strFlightDeckWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@flight_deck_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@flight_deck_weight", DbType.Int32, Convert.ToInt32(strFlightDeckWeight))
            End If

            If strCabinCrewWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@cabin_crew_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@cabin_crew_weight", DbType.Int32, Convert.ToInt32(strCabinCrewWeight))
            End If





            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, strCreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function UpdateSubfleet(ByVal strSubfleetId As String, _
              ByVal strAircraftId As String, _
              ByVal strMaxTaxiWeight As String, _
              ByVal strMaxTakeoffWeight As String, _
              ByVal strMaxLandingWeight As String, _
              ByVal strMaxZeroFuelWeight As String, _
              ByVal strFlightDeckWeight As String, _
              ByVal strCabinCrewWeight As String, _
              ByVal intVersionNo As Integer, _
              ByVal strUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Update_Subfleet"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, strSubfleetId)
            db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)

            If strMaxTaxiWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_taxi_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_taxi_weight", DbType.Int32, Convert.ToDecimal(strMaxTaxiWeight))
            End If

            If strMaxTakeoffWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_takeoff_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_takeoff_weight", DbType.Int32, Convert.ToDecimal(strMaxTakeoffWeight))
            End If

            If strMaxLandingWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_landing_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_landing_weight", DbType.Int32, Convert.ToInt32(strMaxLandingWeight))
            End If

            If strMaxZeroFuelWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@max_zero_fuel_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@max_zero_fuel_weight", DbType.Int32, Convert.ToInt32(strMaxZeroFuelWeight))
            End If

            If strFlightDeckWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@flight_deck_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@flight_deck_weight", DbType.Int32, Convert.ToInt32(strFlightDeckWeight))
            End If

            If strCabinCrewWeight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@cabin_crew_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@cabin_crew_weight", DbType.Int32, Convert.ToInt32(strCabinCrewWeight))
            End If





            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function Get_Subfleet(ByVal strAircraftId As String, ByVal strSubfleetId As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsSubfleet As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Subfleet"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strSubfleetId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@subfleet_Id", DbType.String, strSubfleetId)
            End If

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            dsSubfleet = db.ExecuteDataSet(dbGetCommand)

            Return dsSubfleet.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function DeleteSubfleet(ByVal strSubfleetId As String, ByVal intVersionNo As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_Subfleet"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "@strSubfleetId", DbType.String, strSubfleetId)
            db.AddInParameter(DeleteCommand, "@VersionNo", DbType.Int32, intVersionNo)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function
#End Region

#Region "Galley Arms"

    Public Function Get_GalleyArms(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strChoiceID As String, ByVal strSubFleetID As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAirConfig As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_GalleyArms"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)
            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If
            If strChoiceID.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@ChoiceId", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@ChoiceId", DbType.String, strChoiceID)
            End If
            If strSubFleetID.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@SubfleetIDId", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@SubfleetIDId", DbType.String, strSubFleetID)
            End If



            dsAirConfig = db.ExecuteDataSet(dbGetCommand)

            Return dsAirConfig.Tables(0)

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Create_Update_GalleyArms(ByVal Crew_Galley_Arm_ID As Int64, ByVal crew_galley_desig_cl_id As String, ByVal strChoiceID As String, _
                                    ByVal arm As Decimal, ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Int32


        Try
            Dim sqlCommand As String = DbUserName & "Create_GalleyArms"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            If (Crew_Galley_Arm_ID = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@crew_galley_arm_id", DbType.Int64, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@crew_galley_arm_id", DbType.Int64, Crew_Galley_Arm_ID)
            End If

            If crew_galley_desig_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@crew_galley_desig_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@crew_galley_desig_cl_id", DbType.String, crew_galley_desig_cl_id)
            End If

            If strChoiceID.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@arm_type", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@arm_type", DbType.String, strChoiceID)
            End If

            db.AddInParameter(InsertUpdateCommand, "@arm", DbType.Decimal, arm)

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            If strSubFleetID.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, strSubFleetID)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, VersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, LastUpdatedBy)


            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))
            Return intResult

        Catch ex As Exception
            Throw ex
        End Try

    End Function


#End Region

#Region "Zone Definition"

    Public Function Get_ZoneDefinition(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strSubFleetID As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsZoneDefinition As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_ZoneDefinition"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)
            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            If strSubFleetID.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@SubFleetID", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@SubFleetID", DbType.String, strSubFleetID)
            End If



            dsZoneDefinition = db.ExecuteDataSet(dbGetCommand)

            Return dsZoneDefinition.Tables(0)

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Create_Update_ZoneDefination(ByVal zone_definition_id As Int64, ByVal designation_id As String, ByVal arm As Decimal, _
                                     ByVal max_capacity As Int32, ByVal first_row_number As Int32, ByVal last_row_number As Int32, ByVal Description As String, _
                                     ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Int32


        Try
            Dim sqlCommand As String = DbUserName & "Create_Update_ZoneDefination"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            If (zone_definition_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@zone_definition_id", DbType.Int64, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@zone_definition_id", DbType.Int64, zone_definition_id)
            End If

            If designation_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@designation_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@designation_id", DbType.String, designation_id)
            End If

            db.AddInParameter(InsertUpdateCommand, "@arm", DbType.Decimal, arm)


            db.AddInParameter(InsertUpdateCommand, "@max_capacity", DbType.Int32, max_capacity)
            db.AddInParameter(InsertUpdateCommand, "@first_row_number", DbType.Int32, first_row_number)
            db.AddInParameter(InsertUpdateCommand, "@last_row_number", DbType.Int32, last_row_number)

            If Description.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Description", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Description", DbType.String, Description)
            End If





            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            If strSubFleetID.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, strSubFleetID)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, VersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, LastUpdatedBy)


            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))
            Return intResult

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "AirCraft Adjustment"

    Public Function Get_Aircraft_Config_Adjustments(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strSubFleetID As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAirCraftConfigAdj As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_AirCraftConfigAdjustments"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)
            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            If strSubFleetID.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@SubFleetID", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@SubFleetID", DbType.String, strSubFleetID)
            End If



            dsAirCraftConfigAdj = db.ExecuteDataSet(dbGetCommand)

            Return dsAirCraftConfigAdj.Tables(0)

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Function Create_Update_AirCraftConfigAdj(ByVal air_conf_adjust_id As Int64, ByVal reference_cl_id As String, ByVal is_enabled As String, ByVal empty_weight As Int32, _
                                    ByVal arm As Decimal, ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Int32


        Try
            Dim sqlCommand As String = DbUserName & "Create_Update_Aircraft_Config_Adj"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            If (air_conf_adjust_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@air_conf_adjust_id", DbType.Int64, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@air_conf_adjust_id", DbType.Int64, air_conf_adjust_id)
            End If

            If reference_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@reference_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@reference_cl_id", DbType.String, reference_cl_id)
            End If


            If is_enabled.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_enabled", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@is_enabled", DbType.String, is_enabled)
            End If

            db.AddInParameter(InsertUpdateCommand, "@empty_weight", DbType.Int32, empty_weight)


            db.AddInParameter(InsertUpdateCommand, "@arm", DbType.Decimal, arm)



            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            If strSubFleetID.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, strSubFleetID)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, VersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, LastUpdatedBy)


            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))
            Return intResult

        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region


#Region "Choice_List"
    Public Function CreateChoice_List(ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Choice_List"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            db.AddInParameter(InsertUpdateCommand, "@Choice_list_id", DbType.String, strchoice_list_id)


            If strdescription.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, strdescription)
            End If

            If is_active.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "is_active", DbType.Int32, is_active)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, strCreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function DeleteChoice_List(ByVal strChoice_list_Id As String, ByVal intVersionNo As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_Choice_List"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "@Choice_list_Id", DbType.String, strChoice_list_Id)
            db.AddInParameter(DeleteCommand, "@VersionNo", DbType.Int32, intVersionNo)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function
    Public Function UpdateChoice_List(ByVal strChoice_list_id As String, _
            ByVal strdescription As String, _
            ByVal is_active As Integer, _
            ByVal intVersionNo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Update_Choice_List"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@Choice_list_id", DbType.String, strChoice_list_id)

            If strdescription.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, strdescription)
            End If

            If is_active.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, is_active)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function Get_ChoiceList(ByVal strChoice_list_Id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoicelist As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_Choice_List"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strChoice_list_Id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Choice_list_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Choice_list_Id", DbType.String, strChoice_list_Id)
            End If

            dsChoicelist = db.ExecuteDataSet(dbGetCommand)

            Return dsChoicelist.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "Choices"
    Public Function CreateChoices(ByVal straircraft_id As String, ByVal strchoices_id As String, ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_default As Integer, ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Choices"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@aircraft_id", DbType.String, straircraft_id)
            db.AddInParameter(InsertUpdateCommand, "@Choices_id", DbType.String, strchoices_id)

            If strchoice_list_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@choice_list_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@choice_list_id", DbType.String, strchoice_list_id)
            End If

            If strdescription.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, strdescription)
            End If

            If is_default.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_default", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@is_default", DbType.Int32, is_default)
            End If

            If is_active.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, is_active)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, strCreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function UpdateChoices(ByVal strchoices_id As String, ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_default As Integer, ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Update_Choices"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@Choices_id", DbType.String, strchoices_id)

            If strchoice_list_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@choice_list_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@choice_list_id", DbType.String, strchoice_list_id)
            End If

            If strdescription.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@description", DbType.String, strdescription)
            End If

            If is_default.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_default", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "is_default", DbType.Int32, is_default)
            End If

            If is_active.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@is_active", DbType.Int32, is_active)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function Get_Choices(ByVal straircraft_id As String, ByVal strchoices_id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_Choices"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If straircraft_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, straircraft_id)
            End If

            If strchoices_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@choices_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@choices_id", DbType.String, strchoices_id)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function DeleteChoices(ByVal strChoices_Id As String, ByVal intVersionNo As Integer) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Delete_Choices"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(DeleteCommand, "@Choices_Id", DbType.String, strChoices_Id)
            db.AddInParameter(DeleteCommand, "@VersionNo", DbType.Int32, intVersionNo)
            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try
        Return intResult
    End Function
#End Region

#Region "ULDDefinition"
    Public Function Get_ULDDefiniton(ByVal strTableId As Integer, ByVal straircraft_id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_ULDDefinition"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strTableId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, strTableId)
            End If

            If straircraft_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, straircraft_id)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function Create_Update_ULDDefinition(ByVal struld_definition_id As Integer, ByVal struld_cl_id As String, _
           ByVal allow_cargo As Boolean, _
           ByVal allow_bags As Boolean, ByVal strtare_weight As Integer, _
           ByVal straircraft_id As String, _
           ByVal intVersionNo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Update_ULDDefinition"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If (struld_definition_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@uld_definition_id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@uld_definition_id", DbType.Int32, struld_definition_id)
            End If

            If struld_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@uld_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@uld_cl_id", DbType.String, struld_cl_id)
            End If

            If allow_cargo.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@allow_cargo", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@allow_cargo", DbType.Int32, allow_cargo)
            End If

            If allow_bags.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@allow_bags", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@allow_bags", DbType.Int32, allow_bags)
            End If

            If strtare_weight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@tare_weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@tare_weight", DbType.Int32, strtare_weight)
            End If

            If straircraft_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@aircraft_id", DbType.String, straircraft_id)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
#End Region

#Region "Operational Limit"
    Public Function Create_Update_OprLimit(ByVal operational_limits_id As Integer, ByVal aircraft_config_cl_id As String, _
           ByVal op_limit_type_cl_id As String, _
           ByVal weight As Integer, ByVal MAC As Integer, _
           ByVal Subfleet_Id As String, _
           ByVal intVersionNo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_Update_OprLimit"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If (operational_limits_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@operational_limits_id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@operational_limits_id", DbType.Int32, operational_limits_id)
            End If

            If aircraft_config_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@aircraft_config_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@aircraft_config_cl_id", DbType.String, aircraft_config_cl_id)
            End If

            If op_limit_type_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@op_limit_type_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@op_limit_type_cl_id", DbType.String, op_limit_type_cl_id)
            End If

            If weight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@weight", DbType.Int32, weight)
            End If

            If MAC.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@MAC", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@MAC", DbType.Int32, MAC)
            End If

            If Subfleet_Id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Subfleet_Id", DbType.String, Subfleet_Id)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function Get_OperationalLimit(ByVal strTableId As Integer, ByVal straircraftconfig_cl_id As String, ByVal op_limit_cl_id As String, ByVal strsubfleet_id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_OperationalLimit"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strTableId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, strTableId)
            End If

            If straircraftconfig_cl_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@aircraft_config_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@aircraft_config_cl_id", DbType.String, straircraftconfig_cl_id)
            End If

            If op_limit_cl_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@op_limit_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@op_limit_cl_id", DbType.String, op_limit_cl_id)
            End If

            If strsubfleet_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@subfleetId", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@subfleetId", DbType.String, strsubfleet_id)
            End If


            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "Service Definitions"
    Public Function Create_Update_ServiceDefinition(ByVal service_definition_id As Integer, ByVal service_defintion_cl_id As String, _
          ByVal flight_no_desig_ref As String, _
          ByVal start_flight_number As Integer, ByVal end_flight_number As Integer, _
          ByVal intVersionNo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_update_ServiceDefinition"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If (service_definition_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@service_definition_id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@service_definition_id", DbType.Int32, service_definition_id)
            End If

            If service_defintion_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@service_defintion_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@service_defintion_cl_id", DbType.String, service_defintion_cl_id)
            End If

            If flight_no_desig_ref.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@flight_no_desig_ref", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@flight_no_desig_ref", DbType.String, flight_no_desig_ref)
            End If

            If start_flight_number.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@start_flight_number", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@start_flight_number", DbType.Int32, start_flight_number)
            End If

            If end_flight_number.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@end_flight_number", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@end_flight_number", DbType.Int32, end_flight_number)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function Get_ChoiceListByChoicelistID(ByVal straircraftId As String, ByVal strchoice_list_id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_ChoiceList"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If straircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, straircraftId)
            End If

            If strchoice_list_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@choice_list_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@choice_list_id", DbType.String, strchoice_list_id)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Function Get_ServiceDefinition(ByVal service_definition_id As String, ByVal service_definition_cl_id As String, ByVal flight_no_desig_ref As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_ServiceDefinition"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If service_definition_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@service_definition_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@service_definition_id", DbType.String, service_definition_id)
            End If

            If service_definition_cl_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@service_definition_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@service_definition_cl_id", DbType.String, service_definition_cl_id)
            End If

            If flight_no_desig_ref.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@flight_no_desig_ref", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@flight_no_desig_ref", DbType.String, flight_no_desig_ref)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "ULD-Position"

    Public Function Get_ULDPosition(ByVal strTableId As String, ByVal strAircraftId As String, ByVal PosID As System.Int32) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAirConfig As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_ULDPosition"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)
            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If
            If PosID = 0 Then
                db.AddInParameter(dbGetCommand, "@AirCraft_Config_Pos_Id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@AirCraft_Config_Pos_Id", DbType.Int32, PosID)
            End If

            dsAirConfig = db.ExecuteDataSet(dbGetCommand)

            Return dsAirConfig.Tables(0)

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CreateUpdateAirCraftPostion(ByVal aircraft_config_pos_id As Int32, ByVal aircraft_conf_cl_id As String, _
                                                ByVal Pos_name As String, ByVal uld_ref_cl_id As String, ByVal Max_Pos_Load As Integer, _
                                                ByVal PosArm As System.Int32, ByVal strAircraftId As String, ByVal Version As Int32, _
                                                ByVal LastUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Int32 = 0

        Try
            Dim sqlCommand As String = DbUserName & "Create_update_AirCraftPostion"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            If (aircraft_config_pos_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@aircraft_config_pos_id", DbType.Int32, System.DBNull.Value)
            Else

                db.AddInParameter(InsertUpdateCommand, "@aircraft_config_pos_id", DbType.Int32, aircraft_config_pos_id)

            End If

            If aircraft_conf_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@aircraft_conf_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@aircraft_conf_cl_id", DbType.String, aircraft_conf_cl_id)
            End If

            If Pos_name.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Pos_name", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Pos_name", DbType.String, Pos_name)
            End If
            If uld_ref_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@uld_ref_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@uld_ref_cl_id", DbType.String, uld_ref_cl_id)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Max_Pos_Load", DbType.Int32, Max_Pos_Load)

            db.AddInParameter(InsertUpdateCommand, "@Pos_Arm", DbType.Int32, PosArm)

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, Version)
            intResult = Convert.ToInt32(db.ExecuteScalar(InsertUpdateCommand))

            Return intResult

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "Common Functions"

    Public Function GetAircraftConfigDetails(ByVal straircraftId As String, ByVal Choice_list_Id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "GetAircraftConfigDetails"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If straircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, straircraftId)
            End If

            If Choice_list_Id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@choice_list_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Choice_list_Id", DbType.String, Choice_list_Id)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function GetULDConfigDefaultDetails(ByVal straircraftId As String, ByVal air_conf_cl_id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "GetULDConfigDefaultDetails"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If straircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_id", DbType.String, straircraftId)
            End If

            If air_conf_cl_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@air_conf_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@air_conf_cl_id", DbType.String, air_conf_cl_id)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "Underfloor Comp"
    Public Function Get_UnderFloor_Comp(ByVal strTableId As Integer, ByVal Aircraft_Id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsUnderFloorComp As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_UnderFloor_Comp"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strTableId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, strTableId)
            End If


            If Aircraft_Id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, Aircraft_Id)
            End If




            dsUnderFloorComp = db.ExecuteDataSet(dbGetCommand)

            Return dsUnderFloorComp.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CreateUpdateUnderFloorComp(ByVal underfloor_comp_id As Int32, ByVal Comp_cl_id As String, _
                                               ByVal max_cpt_load As Integer, _
                                               ByVal pos_ref1 As Integer, ByVal pos_ref2 As Integer, ByVal pos_ref3 As Integer, _
                                               ByVal strAircraftId As String, ByVal Version As Int32, _
                                               ByVal LastUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Int32 = 0

        Try
            Dim sqlCommand As String = DbUserName & "Create_update_UnderFloorComp"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            If (underfloor_comp_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@underfloor_comp_id", DbType.Int32, System.DBNull.Value)
            Else

                db.AddInParameter(InsertUpdateCommand, "@underfloor_comp_id", DbType.Int32, underfloor_comp_id)

            End If

            If Comp_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Comp_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Comp_cl_id", DbType.String, Comp_cl_id)
            End If


            db.AddInParameter(InsertUpdateCommand, "@max_cpt_load", DbType.Int32, max_cpt_load)

            db.AddInParameter(InsertUpdateCommand, "@pos_ref1", DbType.Int32, pos_ref1)
            db.AddInParameter(InsertUpdateCommand, "@pos_ref2", DbType.Int32, pos_ref2)
            db.AddInParameter(InsertUpdateCommand, "@pos_ref3", DbType.Int32, pos_ref3)

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, Version)
            intResult = Convert.ToInt32(db.ExecuteScalar(InsertUpdateCommand))

            Return intResult

        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region


#Region "Underfloor "
    Public Function Get_UnderFloor(ByVal strTableId As Integer, ByVal Aircraft_Id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsUnderFloorComp As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_UnderFloor"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            If strTableId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@TableId", DbType.Int32, strTableId)
            End If


            If Aircraft_Id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, Aircraft_Id)
            End If




            dsUnderFloorComp = db.ExecuteDataSet(dbGetCommand)

            Return dsUnderFloorComp.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function CreateUpdateUnderFloor(ByVal underfloor_hold_id As Int32, ByVal hold_cl_id As String, _
                                               ByVal Max_hold_Load As Integer, _
                                               ByVal underfloor_comp_id1 As String, ByVal underfloor_comp_id2 As String, _
                                               ByVal strAircraftId As String, ByVal Version As Int32, _
                                               ByVal LastUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Int32 = 0

        Try
            Dim sqlCommand As String = DbUserName & "Create_update_underfloor"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            If (underfloor_hold_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@underfloor_hold_id", DbType.Int32, System.DBNull.Value)
            Else

                db.AddInParameter(InsertUpdateCommand, "@underfloor_hold_id", DbType.Int32, underfloor_hold_id)

            End If

            If hold_cl_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@hold_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@hold_cl_id", DbType.String, hold_cl_id)
            End If


            db.AddInParameter(InsertUpdateCommand, "@Max_hold_Load", DbType.Int32, Max_hold_Load)

            If underfloor_comp_id1.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@underfloor_comp_id1", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@underfloor_comp_id1", DbType.String, underfloor_comp_id1)
            End If

            If underfloor_comp_id2.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@underfloor_comp_id2", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@underfloor_comp_id2", DbType.String, underfloor_comp_id2)
            End If

            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, Version)
            intResult = Convert.ToInt32(db.ExecuteScalar(InsertUpdateCommand))

            Return intResult

        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "ULD Configuration"

    Public Function Get_ULDConfiguration(ByVal strTableId As String, ByVal strAircraftId As String, ByVal ConfigID As System.Int32, _
                                   ByVal AirConfigID As String, ByVal ULDConfigID As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsAirConfig As DataSet = Nothing

        Try
            Dim sqlCommand As String = DbUserName & "Get_ULDConfiguration"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(dbGetCommand, "@TableId", DbType.String, strTableId)
            If strAircraftId.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@Aircraft_Id", DbType.String, strAircraftId)
            End If
            If ConfigID = 0 Then
                db.AddInParameter(dbGetCommand, "@uld_conf_id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@uld_conf_id", DbType.Int32, ConfigID)
            End If

            If AirConfigID.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@air_conf_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@air_conf_cl_id", DbType.String, AirConfigID)
            End If

            If ULDConfigID.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@uld_conf_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@uld_conf_cl_id", DbType.String, ULDConfigID)
            End If


            dsAirConfig = db.ExecuteDataSet(dbGetCommand)

            Return dsAirConfig.Tables(0)

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "Service Data"
    Public Function Create_Update_ServiceData(ByVal servicedata_id As Integer, ByVal type_choicelistid As String, _
         ByVal weight As Integer, _
         ByVal occupies_seat As String, ByVal category_choicelistid As String, _
         ByVal aircraft_id As String, ByVal intVersionNo As Integer) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer
        Try


            Dim sqlCommand As String = DbUserName & "Create_update_ServiceData"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)



            If (servicedata_id = 0) Then
                db.AddInParameter(InsertUpdateCommand, "@servicedata_id", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@servicedata_id", DbType.Int32, servicedata_id)
            End If

            If type_choicelistid.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@type_choicelistid", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@type_choicelistid", DbType.String, type_choicelistid)
            End If

            If weight.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@weight", DbType.Int32, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@weight", DbType.Int32, weight)
            End If

            If occupies_seat.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@occupies_seat", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@occupies_seat", DbType.String, occupies_seat)
            End If

            If category_choicelistid.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@category_choicelistid", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@category_choicelistid", DbType.String, category_choicelistid)
            End If

            If aircraft_id.Equals(String.Empty) Then
                db.AddInParameter(InsertUpdateCommand, "@aircraft_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(InsertUpdateCommand, "@aircraft_id", DbType.String, aircraft_id)
            End If

            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            'db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function
    Public Function Get_ServiceData(ByVal servicedata_id As String, ByVal type_choicelistid As String, ByVal aircraftid As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_ServiceData"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)



            If servicedata_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@servicedata_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@servicedata_id", DbType.String, servicedata_id)
            End If

            If type_choicelistid.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@type_choicelistid", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@type_choicelistid", DbType.String, type_choicelistid)
            End If

            If aircraftid.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@aircraftid", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@aircraftid", DbType.String, aircraftid)
            End If

            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region
#Region "Galley Weight"
    Public Function Get_GalleyWeight(ByVal subfleet_id As String, ByVal desig_cl_id As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsChoices As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_GalleyWeight"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)



            If subfleet_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@subfleet_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@subfleet_id", DbType.String, subfleet_id)
            End If

            If desig_cl_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@designation_cl_id", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@designation_cl_id", DbType.String, desig_cl_id)
            End If



            dsChoices = db.ExecuteDataSet(dbGetCommand)

            Return dsChoices.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region


#Region "Home"

    Public Function Publish_Database(ByVal strDbVersion As String, ByVal strPublishedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try
            Dim sqlCommand As String = DbUserName & "Publish_Database"
            Dim DeleteCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(DeleteCommand, "DatabaseVersionNo", DbType.String, strDbVersion)
            db.AddInParameter(DeleteCommand, "UserId", DbType.String, strPublishedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(DeleteCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

#End Region

#Region "Analytical Report"
    Public Function Get_IPADUDID() As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsUDID As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_IPAD_UDID"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            dsUDID = db.ExecuteDataSet(dbGetCommand)

            Return dsUDID.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Get_IPADDBVersionHistory(ByVal IpadUD_id As String, ByVal IsExcludeLIPad As Integer, ByVal strDBVerNo As String, ByVal strExcludeDBVerNo As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsDBVerHis As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_IPAD_DB_VER_History"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)



            If IpadUD_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@IPADUDID", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@IPADUDID", DbType.String, IpadUD_id)
            End If

            db.AddInParameter(dbGetCommand, "@IsExcludeLIPad", DbType.Int16, IsExcludeLIPad)

            If strDBVerNo.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@DBVerNo", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@DBVerNo", DbType.String, strDBVerNo)
            End If

            If strExcludeDBVerNo.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@ExcludeDBVerNo", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@ExcludeDBVerNo", DbType.String, strExcludeDBVerNo)
            End If

            dsDBVerHis = db.ExecuteDataSet(dbGetCommand)

            Return dsDBVerHis
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Function Get_IPADDetails(ByVal IpadUD_id As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsDBVerHis As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_IPAD_Details"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)



            If IpadUD_id.Equals(String.Empty) Then
                db.AddInParameter(dbGetCommand, "@IPADUDID", DbType.String, System.DBNull.Value)
            Else
                db.AddInParameter(dbGetCommand, "@IPADUDID", DbType.String, IpadUD_id)
            End If




            dsDBVerHis = db.ExecuteDataSet(dbGetCommand)

            Return dsDBVerHis
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Get_IPADVersionNo() As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsDBVerHis As DataSet = Nothing
        'straircraft_id = "A321"
        Try
            Dim sqlCommand As String = DbUserName & "Get_IPAD_VersionNo"
            Dim dbGetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            dsDBVerHis = db.ExecuteDataSet(dbGetCommand)

            Return dsDBVerHis.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function Update_IPADDetails(ByVal IpadUD_id As String, _
                ByVal EmpNo As String, _
                ByVal IsDisabled As Integer, _
                ByVal intVersionNo As Integer, _
                ByVal strUpdatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try

            Dim sqlCommand As String = DbUserName & "Update_IPAD_Details"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)

            db.AddInParameter(InsertUpdateCommand, "@IPADUDID", DbType.String, IpadUD_id)
            db.AddInParameter(InsertUpdateCommand, "@EMP_No", DbType.String, EmpNo)
            db.AddInParameter(InsertUpdateCommand, "@IsDisabled", DbType.Int16, IsDisabled)
            db.AddInParameter(InsertUpdateCommand, "@Version_No", DbType.Int32, intVersionNo)
            db.AddInParameter(InsertUpdateCommand, "@UpdatedBy", DbType.String, strUpdatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function


    Public Function Push_Pre_Production_Data_To_Production(ByVal strCreatedBy As String) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim intResult As Integer

        Try

            Dim sqlCommand As String = DbUserName & "Push_Pre_Production_Data_To_Production"
            Dim InsertUpdateCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(InsertUpdateCommand, "@CreatedBy", DbType.String, strCreatedBy)

            intResult = Convert.ToInt16(db.ExecuteScalar(InsertUpdateCommand))

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function


    Public Function DatabaseSanityChecks() As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsResult As DataSet

        Try

            Dim sqlCommand As String = DbUserName & "usp_Database_Sanity_Checks"
            Dim GetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            dsResult = db.ExecuteDataSet(GetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return dsResult

    End Function


    Public Function APNPushRequest() As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsResult As DataSet

        Try

            Dim sqlCommand As String = DbUserName & "Get_APN_Details"
            Dim GetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            dsResult = db.ExecuteDataSet(GetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return dsResult

    End Function

    Public Function APNTableSchema() As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dsResult As DataSet

        Try

            Dim sqlCommand As String = DbUserName & "Get_APN_Request_Details_Schema"
            Dim GetCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)


            dsResult = db.ExecuteDataSet(GetCommand)

        Catch ex As Exception
            Throw ex
        End Try

        Return dsResult

    End Function


    Public Function CreateAPNRequestMaster(ByVal intAPNRequestId As Integer, _
                                     ByVal strAPNMessage As String, _
                                     ByVal dtmAPNRequestDate As DateTime, _
                                     ByVal intBadgeCount As Integer, _
                                     ByVal strUserID As String, _
                                     ByVal dtAPNRequestDetail As DataTable) As Integer

        Dim objDB As Database = DatabaseFactory.CreateDatabase()
        Dim lo_Connection As SqlConnection
        Dim intResult As Integer = 0

        Try

            Dim sqlCommand As String = DbUserName & "Create_APN_Request_Master"
            Dim InsertUpdateCommand As DbCommand = objDB.GetStoredProcCommand(sqlCommand)

            objDB.AddInParameter(InsertUpdateCommand, "@intAPNRequestId", DbType.Int32, intAPNRequestId)
            objDB.AddInParameter(InsertUpdateCommand, "@vAPNMessage", DbType.String, strAPNMessage)
            objDB.AddInParameter(InsertUpdateCommand, "@dtAPNRequestDate", DbType.DateTime, dtmAPNRequestDate)
            objDB.AddInParameter(InsertUpdateCommand, "@intBadgeCount", DbType.Int16, intBadgeCount)
            objDB.AddInParameter(InsertUpdateCommand, "@vUserID", DbType.String, strUserID)

            intResult = Convert.ToInt16(objDB.ExecuteScalar(InsertUpdateCommand))

            If intResult = 1 Then
                lo_Connection = New SqlConnection(objDB.ConnectionString)

                lo_Connection.Open()

                Dim bc = New System.Data.SqlClient.SqlBulkCopy(lo_Connection, SqlBulkCopyOptions.TableLock, Nothing)
                bc.BulkCopyTimeout = 2000
                bc.BatchSize = dtAPNRequestDetail.Rows.Count
                bc.DestinationTableName = "Tbl_Sys_APN_Request_Details"
                bc.WriteToServer(dtAPNRequestDetail)
                bc.Close()
                intResult = 1
            End If

        Catch ex As Exception
            Throw ex
        End Try

        Return intResult

    End Function

#End Region
End Class

