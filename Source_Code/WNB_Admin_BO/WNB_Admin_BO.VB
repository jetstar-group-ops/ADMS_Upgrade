Public Class WNB_Admin_BO

#Region "--Variables--"

    Private lo_StructureFile As String
    
#End Region

#Region "--Properties--"

    Public Property StructureFile() As String
        Get
            Return lo_StructureFile
        End Get
        Set(ByVal value As String)
            lo_StructureFile = value
        End Set

    End Property


#End Region
    Public Function GetFunctionalities(ByVal ParentFunctionalityId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.GetFunctionalities(ParentFunctionalityId)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetReportsForFunctionality(ByVal FunctionalityId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.GetReportsForFunctionality(FunctionalityId)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try


    End Function

    Public Function GetUserDetails(ByVal ps_UserId As String, _
                                  ByVal ps_Password As String) As DataTable

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.GetUserDetails(ps_UserId, ps_Password)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function ChangePwd(ByVal ps_UserId As String, ByVal ps_NewPassword As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.ChangePwd(ps_UserId, ps_NewPassword)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function IsUserHasPermission(ByVal UserId As String, _
                                          ByVal FunctionalityId As Integer, _
                                          ByVal LocationId As String, _
                                          ByVal ParentFunctionality As Integer) As Boolean

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.IsUserHasPermission(UserId, FunctionalityId, LocationId, ParentFunctionality)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try
    End Function

    Public Function GetUsers() As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.GetUsers()
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try
    End Function

    Public Function GetRoles() As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.GetRoles()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try
    End Function

    Public Function GetStations() As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.GetStations()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetUserProfile(ByVal ps_UserId As String, ByVal UserSNO As Integer) As DataTable

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.GetUserProfile(ps_UserId, UserSNO)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function CreateUpdateUserProfile(ByVal UserId As String, _
                                      ByVal LocationId As String, _
                                      ByVal Password As String, _
                                      ByVal IsDisabled As Integer, _
                                      ByVal RoleIds As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateUpdateUserProfile(UserId, LocationId, Password, _
                            IsDisabled, RoleIds)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function GetPagesFunctionalities(ByVal ParentFunctionalityId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.GetPagesFunctionalities(ParentFunctionalityId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function GetRoleFunctionalities(ByVal RoleId As Integer) As DataTable

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.GetRoleFunctionalities(RoleId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetRoleFunctionalityLocations(ByVal RoleId As Integer, _
                                         ByVal FunctionalityId As Integer) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.GetRoleFunctionalityLocations(RoleId, FunctionalityId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function IsRoleNameExists(ByVal RoleName As String, ByVal RoleId As Int32)

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.IsRoleNameExists(RoleName, RoleId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function CreateRole(ByVal RoleName As String, _
                        ByVal RoleDescription As String, _
                        ByVal RoleFunctionalities As String, _
                        ByVal CreatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.CreateRole(RoleName, RoleDescription, RoleFunctionalities, CreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try
    End Function

    Public Function UpdateRole(ByVal RoleId As Integer, ByVal RoleName As String, _
                        ByVal RoleDescription As String, _
                        ByVal RoleFunctionalities As String, _
                        ByVal ModifyBy As String) As Integer


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.UpdateRole(RoleId, RoleName, RoleDescription, RoleFunctionalities, _
                          ModifyBy)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function DeleteRole(ByVal intRoleId As Integer) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteRole(intRoleId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function DeleteUser(ByVal intUserSNO As Integer, ByVal strUserId As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteUser(intUserSNO, strUserId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetDBDetails() As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.GetDBDetails()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function GetDBTableData(ByVal strTableId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.GetDBTableData(strTableId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function









    Public Function Is_Database_Upgrade_Running() As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            Return objWNBService.Is_Database_Upgrade_Running()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Sub Start_Database_Upgrade_Session()
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            objWNBService.Start_Database_Upgrade_Session()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Sub


    Public Sub Cancel_Database_Upgrade_Session()
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try
            objWNBService.Cancel_Database_Upgrade_Session()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Sub



    Public Function Get_Aircraft_Config(ByVal strTableId As String, ByVal strAircraftId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_Aircraft_Config(strTableId, strAircraftId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

#Region "Common Methods"

    Public Function Get_ChoiceListByChoicelistID(ByVal straircraftId As String, ByVal strchoice_list_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ChoiceListByChoicelistID(straircraftId, strchoice_list_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetAircraftConfigDetails(ByVal straircraftId As String, ByVal Choice_list_Id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.GetAircraftConfigDetails(straircraftId, Choice_list_Id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetULDConfigDefaultDetails(ByVal straircraftId As String, ByVal air_conf_cl_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.GetULDConfigDefaultDetails(straircraftId, air_conf_cl_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

#End Region

#Region "Aircraft"

    Public Function Get_Aircrafts(ByVal strAircraftId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_Aircrafts(strAircraftId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

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


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateAircraft(strAircraftId, strModelName, _
                 strRefChordOrigin, strRefChordLength, strRefStation, _
                 strIUEquConstC, strIUEquConstK, strMinOpWeightAdj, _
                 strMaxOpWeightAdj, strMinOpIUAdj, strMaxOpIUAdj, _
                 intVersionNo, strCreatedBy)


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

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


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.UpdateAircraft(strAircraftId, strModelName, _
                 strRefChordOrigin, strRefChordLength, strRefStation, _
                 strIUEquConstC, strIUEquConstK, strMinOpWeightAdj, _
                 strMaxOpWeightAdj, strMinOpIUAdj, strMaxOpIUAdj, _
                 intVersionNo, strUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function UpdateAircraftConfig(ByVal strAircraftId As String, _
             ByVal intACId As String, _
             ByVal strTankRef As String, _
             ByVal strAirconfig As String, _
             ByVal strFuelTank As String, _
             ByVal intVersionNo As Integer) As Integer


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.UpdateAircraftConfig(strAircraftId, intACId, _
                 strTankRef, strAirconfig, strFuelTank, _
                 intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function DeleteAircraft(ByVal strAircraftId As String, ByVal intVersionNo As Integer) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteAircraft(strAircraftId, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "Registration"


    Public Function Get_Registration(ByVal strAircraftId As String, ByVal RegistrationID As Int32) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_Registration(strAircraftId, RegistrationID)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function



    Public Function CreateRegistration(ByVal Registration_Id As Int32, _
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

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateRegistration(Registration_Id, Registration_Number, _
                 Aircraft_Id, MSN, Seat_Configuration, _
                 Load_Data_Sheet_Sef, Basic_Weight, Basic_Arm, _
                 Subfleet_Id, _
                 intVersionNo, strCreatedBy)


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function UpdateRegistration(ByVal Registration_Id As Int32, _
            ByVal Registration_Number As String, _
            ByVal Aircraft_Id As String, _
            ByVal MSN As String, _
            ByVal Seat_Configuration As String, _
            ByVal Load_Data_Sheet_Sef As String, _
            ByVal Basic_Weight As Int32, _
            ByVal Basic_Arm As Int32, _
            ByVal Subfleet_Id As String, _
            ByVal intVersionNo As Integer, _
            ByVal strCUpdatedBy As String) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.UpdateRegistration(Registration_Id, Registration_Number, _
                 Aircraft_Id, MSN, Seat_Configuration, _
                 Load_Data_Sheet_Sef, Basic_Weight, Basic_Arm, _
                 Subfleet_Id, intVersionNo, strCUpdatedBy)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function DeleteRegistration(ByVal RegistrationId As Int32, ByVal intVersionNo As Integer) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteRegistration(RegistrationId, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

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


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateSubfleet(strSubfleetId, strAircraftId, _
                strMaxTaxiWeight, strMaxTakeoffWeight, strMaxLandingWeight, _
                strMaxZeroFuelWeight, strFlightDeckWeight, strCabinCrewWeight, _
                intVersionNo, strCreatedBy)



        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

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


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.UpdateSubfleet(strSubfleetId, strAircraftId, _
                strMaxTaxiWeight, strMaxTakeoffWeight, strMaxLandingWeight, _
                strMaxZeroFuelWeight, strFlightDeckWeight, strCabinCrewWeight, _
                intVersionNo, strUpdatedBy)



        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function Get_Subfleet(ByVal strAircraftId As String, ByVal strSubfleetId As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_Subfleet(strSubfleetId, strAircraftId)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function DeleteSubfleet(ByVal strSubfleetId As String, ByVal intVersionNo As Integer) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteSubfleet(strSubfleetId, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

#End Region
#Region "Galley Arms"
    Public Function Get_GalleyArms(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strChoiceID As String, ByVal strSubFleetID As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_GalleyArms(strTableId, strAircraftId, strChoiceID, strSubFleetID)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Create_Update_GalleyArms(ByVal Crew_Galley_Arm_ID As Int64, ByVal crew_galley_desig_cl_id As String, ByVal strChoiceID As String, _
                                    ByVal arm As Decimal, ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_GalleyArms(Crew_Galley_Arm_ID, crew_galley_desig_cl_id, strChoiceID, arm, strAircraftId, VersionNo, _
                                                    strSubFleetID, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "Zone Definition"
    Public Function Get_ZoneDefinition(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strSubFleetID As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ZoneDefinition(strTableId, strAircraftId, strSubFleetID)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function Create_Update_ZoneDefination(ByVal zone_definition_id As Int64, ByVal designation_id As String, ByVal arm As Decimal, _
                                     ByVal max_capacity As Int32, ByVal first_row_number As Int32, ByVal last_row_number As Int32, ByVal Description As String, _
                                     ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_ZoneDefination(zone_definition_id, designation_id, arm, max_capacity, first_row_number, last_row_number, Description, strAircraftId, VersionNo, _
                                                    strSubFleetID, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "AirCraft Adjustment"

    Public Function Get_Aircraft_Config_Adjustments(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strSubFleetID As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_Aircraft_Config_Adjustments(strTableId, strAircraftId, strSubFleetID)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Create_Update_AirCraftConfigAdj(ByVal air_conf_adjust_id As Int64, ByVal reference_cl_id As String, ByVal is_enabled As String, ByVal empty_weight As Int32, _
                                    ByVal arm As Decimal, ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_AirCraftConfigAdj(air_conf_adjust_id, reference_cl_id, is_enabled, empty_weight, arm, strAircraftId, VersionNo, _
                                                  strSubFleetID, LastUpdatedBy)


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

#End Region


#Region "Choice_List"
    Public Function CreateChoice_List(ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateChoice_List(strchoice_list_id, strdescription, _
                 is_active, _
                 intVersionNo, strCreatedBy)



        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function DeleteChoice_List(ByVal strChoice_list_Id As String, ByVal intVersionNo As Integer) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteChoice_List(strChoice_list_Id, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function UpdateChoice_List(ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_active As Integer, _
           ByVal intVersionNo As Integer) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.UpdateChoice_List(strchoice_list_id, strdescription, _
                 is_active, _
                 intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function Get_ChoiceList(ByVal strChoice_list_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ChoiceList(strChoice_list_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "Choices"
    Public Function CreateChoices(ByVal straircraft_id As String, ByVal strchoices_id As String, ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_default As Integer, ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer


        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateChoices(straircraft_id, strchoices_id, strchoice_list_id, _
            strdescription, _
            is_default, is_active, _
            intVersionNo, _
            strCreatedBy)



        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function UpdateChoices(ByVal strchoices_id As String, ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_default As Integer, ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.UpdateChoices(strchoices_id, strchoice_list_id, _
            strdescription, _
            is_default, is_active, _
            intVersionNo, _
            strCreatedBy)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Get_Choices(ByVal straircraft_id As String, ByVal strchoices_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_Choices(straircraft_id, strchoices_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function DeleteChoices(ByVal strChoices_Id As String, ByVal intVersionNo As Integer) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.DeleteChoices(strChoices_Id, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "ULDDefinition"
    Public Function Get_ULDDefiniton(ByVal strTabeleId As Integer, ByVal straircraft_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ULDDefiniton(strTabeleId, straircraft_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function Create_Update_ULDDefinition(ByVal struld_definition_id As Integer, ByVal struld_cl_id As String, _
           ByVal allow_cargo As Boolean, _
           ByVal allow_bags As Boolean, ByVal strtare_weight As Integer, _
           ByVal straircraft_id As String, _
           ByVal intVersionNo As Integer) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_ULDDefinition(struld_definition_id, struld_cl_id, _
            allow_cargo, _
            allow_bags, strtare_weight, _
            straircraft_id, _
            intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "Operational Limit"
    Public Function Create_Update_OprLimit(ByVal operational_limits_id As Integer, ByVal aircraft_config_cl_id As String, _
           ByVal op_limit_type_cl_id As String, _
           ByVal weight As Integer, ByVal MAC As Integer, _
           ByVal Subfleet_Id As String, _
           ByVal intVersionNo As Integer) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_OprLimit(operational_limits_id, aircraft_config_cl_id, _
            op_limit_type_cl_id, _
            weight, MAC, _
            Subfleet_Id, _
            intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function Get_OperationalLimit(ByVal strTableId As Integer, ByVal straircraftconfig_cl_id As String, ByVal op_limit_cl_id As String, ByVal strsubfleet_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_OperationalLimit(strTableId, straircraftconfig_cl_id, op_limit_cl_id, strsubfleet_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "Service Definition"
    Public Function Create_Update_ServiceDefinition(ByVal service_definition_id As Integer, ByVal service_defintion_cl_id As String, _
            ByVal flight_no_desig_ref As String, _
            ByVal start_flight_number As Integer, ByVal end_flight_number As Integer, _
            ByVal intVersionNo As Integer) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_ServiceDefinition(service_definition_id, service_defintion_cl_id, _
            flight_no_desig_ref, _
            start_flight_number, end_flight_number, _
            intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function Get_ServiceDefinition(ByVal service_definition_id As String, ByVal service_definition_cl_id As String, ByVal flight_no_desig_ref As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ServiceDefinition(service_definition_id, service_definition_cl_id, flight_no_desig_ref)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "Service Data"
    Public Function Create_Update_ServiceData(ByVal servicedata_id As Integer, ByVal type_choicelistid As String, _
         ByVal weight As Integer, _
         ByVal occupies_seat As String, ByVal category_choicelistid As String, _
         ByVal aircraft_id As String, ByVal intVersionNo As Integer) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.Create_Update_ServiceData(servicedata_id, type_choicelistid, _
            weight, occupies_seat, category_choicelistid, _
            aircraft_id, intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
    Public Function Get_ServiceData(ByVal servicedata_id As String, ByVal type_choicelistid As String, ByVal aircraftid As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ServiceData(servicedata_id, type_choicelistid, aircraftid)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "Galley Weight"
    Public Function Get_GalleyWeight(ByVal subfleet_id As String, ByVal desig_cl_id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_GalleyWeight(subfleet_id, desig_cl_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region
#Region "Under floor comp"
    Public Function Get_UnderFloor_Comp(ByVal strTableId As Integer, ByVal Aircraft_Id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_UnderFloor_Comp(strTableId, Aircraft_Id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function CreateUpdateUnderFloorComp(ByVal underfloor_comp_id As Int32, ByVal Comp_cl_id As String, _
                                               ByVal max_cpt_load As Integer, _
                                               ByVal pos_ref1 As Integer, ByVal pos_ref2 As Integer, ByVal pos_ref3 As Integer, _
                                               ByVal strAircraftId As String, ByVal Version As Int32, _
                                               ByVal LastUpdatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateUpdateUnderFloorComp(underfloor_comp_id, Comp_cl_id, max_cpt_load, pos_ref1, pos_ref2, pos_ref3, strAircraftId, Version, LastUpdatedBy)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "Under floor"
    Public Function Get_UnderFloor(ByVal strTableId As Integer, ByVal Aircraft_Id As String) As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_UnderFloor(strTableId, Aircraft_Id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function CreateUpdateUnderFloor(ByVal underfloor_hold_id As Int32, ByVal hold_cl_id As String, _
                                               ByVal Max_hold_Load As Integer, _
                                               ByVal underfloor_comp_id1 As String, ByVal underfloor_comp_id2 As String, _
                                               ByVal strAircraftId As String, ByVal Version As Int32, _
                                               ByVal LastUpdatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateUpdateUnderFloor(underfloor_hold_id, hold_cl_id, Max_hold_Load, underfloor_comp_id1, underfloor_comp_id2, strAircraftId, Version, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "ULD-Position"

    Public Function Get_ULDPosition(ByVal strTableId As String, ByVal strAircraftId As String, ByVal PosID As System.Int32) As DataTable

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ULDPosition(strTableId, strAircraftId, PosID)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function CreateUpdateAirCraftPostion(ByVal aircraft_config_pos_id As Int32, ByVal aircraft_conf_cl_id As String, _
                                                ByVal Pos_name As String, ByVal uld_ref_cl_id As String, ByVal Max_Pos_Load As Integer, _
                                                ByVal PosArm As System.Int32, ByVal strAircraftId As String, ByVal Version As Int32, _
                                                ByVal LastUpdatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            Return objWNBService.CreateUpdateAirCraftPostion(aircraft_config_pos_id, aircraft_conf_cl_id, Pos_name, uld_ref_cl_id, Max_Pos_Load, PosArm, strAircraftId, Version, LastUpdatedBy)
        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

#End Region


#Region "ULD Configuration"

    Public Function Get_ULDConfiguration(ByVal strTableId As String, ByVal strAircraftId As String, ByVal ConfigID As System.Int32, _
                                   ByVal AirConfigID As String, ByVal ULDConfigID As String) As DataTable

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_ULDConfiguration(strTableId, strAircraftId, ConfigID, AirConfigID, ULDConfigID)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "Analytical Reports"
    Public Function Get_IPADUDID() As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_IPADUDID()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Get_IPADVersionNo() As DataTable
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_IPADVersionNo()

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Get_IPADDBVersionHistory(ByVal IpadUD_id As String, ByVal IsExcludeLIPad As Integer, ByVal strDBVerNo As String, ByVal strExcludeDBVerNo As String) As DataSet
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_IPADDBVersionHistory(IpadUD_id, IsExcludeLIPad, strDBVerNo, strExcludeDBVerNo)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function GetIPadDBVersionHistoryReport(ByVal ps_IpadUD_id As String, ByVal IsExcludeLIPad As Integer, ByVal strDBVerNo As String, ByVal strExcludeDBVerNo As String) As DataSet


        Dim dsIPadDBVersionHistory As DataSet = Nothing
        Dim dsVersionHistory As DataSet = Nothing


        Try
            'Read Datastructure of output dataset from XML file
            dsIPadDBVersionHistory = New DataSet()
            dsIPadDBVersionHistory.ReadXmlSchema(lo_StructureFile)
            dsIPadDBVersionHistory.Tables(0).Columns("Update_DT").DataType = System.Type.GetType("System.DateTime")
            dsIPadDBVersionHistory.AcceptChanges()

            dsVersionHistory = Get_IPADDBVersionHistory(ps_IpadUD_id, IsExcludeLIPad, strDBVerNo, strExcludeDBVerNo)

            Dim dtVersionHistory As New DataTable
            dsVersionHistory.Tables(0).DefaultView.Sort = "IPAD_UDID"
            dtVersionHistory = dsVersionHistory.Tables(0).DefaultView.ToTable.Copy

            dsIPadDBVersionHistory.Tables.RemoveAt(0)
            dsIPadDBVersionHistory.Tables.Add(dtVersionHistory)


        Catch ex As Exception
            Throw ex
        End Try

        Return dsIPadDBVersionHistory

    End Function

    Public Function Get_IPADDetails(ByVal IpadUD_id As String) As DataSet
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Get_IPADDetails(IpadUD_id)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Update_IPADDetails(ByVal IpadUD_id As String, _
                ByVal EmpNo As String, _
                ByVal IsDisabled As Integer, _
                ByVal intVersionNo As Integer, _
                ByVal strUpdatedBy As String) As Integer
        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Update_IPADDetails(IpadUD_id, EmpNo, IsDisabled, intVersionNo, strUpdatedBy)


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function Push_Pre_Production_Data_To_Production(ByVal strCreatedBy As String) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.Push_Pre_Production_Data_To_Production(strCreatedBy)


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function DatabaseSanityChecks() As DataSet

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.DatabaseSanityChecks()


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function APNPushRequest() As DataSet

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.APNPushRequest()


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function

    Public Function APNTableSchema() As DataSet

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.APNTableSchema()


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function


    Public Function CreateAPNRequestMaster(ByVal intAPNRequestId As Integer, _
                                     ByVal strAPNMessage As String, _
                                     ByVal dtmAPNRequestDate As DateTime, _
                                     ByVal intBadgeCount As Integer, _
                                     ByVal strUserID As String, _
                                     ByVal dtAPNRequestDetail As DataTable) As Integer

        Dim objWNBService As New WNBWebService.WNB_Admin_WS
        Try

            Return objWNBService.CreateAPNRequestMaster(intAPNRequestId, strAPNMessage, dtmAPNRequestDate, _
                            intBadgeCount, strUserID, dtAPNRequestDetail)


        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try

    End Function
#End Region

#Region "Home"

    Public Sub Publish_Database(ByVal strDbVersion As String, ByVal strPublishedBy As String)
        Dim objWNBService As New WNBWebService.WNB_Admin_WS

        Try

            objWNBService.Publish_Database(strDbVersion, strPublishedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objWNBService = Nothing
        End Try
    End Sub

#End Region

End Class
