Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WNB_Admin_WS
    Inherits System.Web.Services.WebService

    <WebMethod()> _
      Public Function GetFunctionalities(ByVal ParentFunctionalityId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try
            Return objDal.GetFunctionalities(ParentFunctionalityId)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetReportsForFunctionality(ByVal FunctionalityId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try
            Return objDal.GetReportsForFunctionality(FunctionalityId)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try


    End Function

    <WebMethod()> _
    Public Function GetUserDetails(ByVal ps_UserId As String, _
                                  ByVal ps_Password As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try
            Return objDal.GetUserDetails(ps_UserId, ps_Password)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function ChangePwd(ByVal ps_UserId As String, ByVal ps_NewPassword As String) As Integer

        Dim objDal As New DAL_Manager()

        Try
            Return objDal.ChangePwd(ps_UserId, ps_NewPassword)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function IsUserHasPermission(ByVal UserId As String, _
                                          ByVal FunctionalityId As Integer, _
                                          ByVal LocationId As String, _
                                          ByVal ParentFunctionality As Integer) As Boolean

        Dim objDal As New DAL_Manager()

        Try
            Return objDal.IsUserHasPermission(UserId, FunctionalityId, LocationId, ParentFunctionality)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
    Public Function GetUsers() As DataTable
        Dim objDal As New DAL_Manager()

        Try
            Return objDal.GetUsers()
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetRoles() As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetRoles()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
    Public Function GetStations() As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetStations()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetUserProfile(ByVal ps_UserId As String, ByVal UserSNO As Integer) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetUserProfile(ps_UserId, UserSNO)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function CreateUpdateUserProfile(ByVal UserId As String, _
                                      ByVal LocationId As String, _
                                      ByVal Password As String, _
                                      ByVal IsDisabled As Integer, _
                                      ByVal RoleIds As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateUpdateUserProfile(UserId, LocationId, Password, _
                            IsDisabled, RoleIds)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetPagesFunctionalities(ByVal ParentFunctionalityId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetPagesFunctionalities(ParentFunctionalityId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
    Public Function GetRoleFunctionalities(ByVal RoleId As Integer) As DataTable

        Dim objDal As New DAL_Manager()

        Try
            Return objDal.GetRoleFunctionalities(RoleId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetRoleFunctionalityLocations(ByVal RoleId As Integer, _
                                         ByVal FunctionalityId As Integer) As DataTable
        Dim objDal As New DAL_Manager()

        Try
            Return objDal.GetRoleFunctionalityLocations(RoleId, FunctionalityId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function IsRoleNameExists(ByVal RoleName As String, ByVal RoleId As Int32)

        Dim objDal As New DAL_Manager()

        Try
            Return objDal.IsRoleNameExists(RoleName, RoleId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function CreateRole(ByVal RoleName As String, _
                        ByVal RoleDescription As String, _
                        ByVal RoleFunctionalities As String, _
                        ByVal CreatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try
            Return objDal.CreateRole(RoleName, RoleDescription, RoleFunctionalities, CreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function UpdateRole(ByVal RoleId As Integer, ByVal RoleName As String, _
                        ByVal RoleDescription As String, _
                        ByVal RoleFunctionalities As String, _
                        ByVal ModifyBy As String) As Integer


        Dim objDal As New DAL_Manager()

        Try
            Return objDal.UpdateRole(RoleId, RoleName, RoleDescription, RoleFunctionalities, _
                          ModifyBy)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function DeleteRole(ByVal intRoleId As Integer) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteRole(intRoleId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function DeleteUser(ByVal intUserSNO As Integer, ByVal strUserId As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteUser(intUserSNO, strUserId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetDBDetails() As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetDBDetails()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function GetDBTableData(ByVal strTableId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetDBTableData(strTableId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function


    <WebMethod()> _
   Public Function DeleteAircraft(ByVal strAircraftId As String, ByVal intVersionNo As Integer) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteAircraft(strAircraftId, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
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

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateAircraft(strAircraftId, strModelName, _
                 strRefChordOrigin, strRefChordLength, strRefStation, _
                 strIUEquConstC, strIUEquConstK, strMinOpWeightAdj, _
                 strMaxOpWeightAdj, strMinOpIUAdj, strMaxOpIUAdj, _
                 intVersionNo, strCreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function





    <WebMethod()> _
  Public Function Is_Database_Upgrade_Running() As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Is_Database_Upgrade_Running()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function


    <WebMethod()> _
  Public Sub Start_Database_Upgrade_Session()

        Dim objDal As New DAL_Manager()

        Try
            objDal.Start_Database_Upgrade_Session()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Sub

    <WebMethod()> _
  Public Sub Cancel_Database_Upgrade_Session()

        Dim objDal As New DAL_Manager()

        Try
            objDal.Cancel_Database_Upgrade_Session()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Sub

    <WebMethod()> _
    Public Function Get_Aircrafts(ByVal strAircraftId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_Aircrafts(strAircraftId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function


    <WebMethod()> _
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

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.UpdateAircraft(strAircraftId, strModelName, _
                 strRefChordOrigin, strRefChordLength, strRefStation, _
                 strIUEquConstC, strIUEquConstK, strMinOpWeightAdj, _
                 strMaxOpWeightAdj, strMinOpIUAdj, strMaxOpIUAdj, _
                 intVersionNo, strUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
        Public Function UpdateAircraftConfig(ByVal strAircraftId As String, _
                   ByVal intACId As String, _
                   ByVal strTankRef As String, _
                   ByVal strAirconfig As String, _
                   ByVal strFuelTank As String, _
                   ByVal intVersionNo As Integer) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.UpdateAircraftConfig(strAircraftId, intACId, strTankRef, strAirconfig, strFuelTank, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function Get_Aircraft_Config(ByVal strTableId As String, ByVal strAircraftId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_Aircraft_Config(strTableId, strAircraftId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

#Region "Registration"

    <WebMethod()> _
  Public Function Get_Registration(ByVal strAircraftId As String, ByVal RegistrationID As Int32) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_Registration(strAircraftId, RegistrationID)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
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
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateRegistration(Registration_Id, Registration_Number, _
                 Aircraft_Id, MSN, Seat_Configuration, _
                 Load_Data_Sheet_Sef, Basic_Weight, Basic_Arm, _
                 Subfleet_Id, _
                 intVersionNo, strCreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
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
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.UpdateRegistration(Registration_Id, Registration_Number, _
                 Aircraft_Id, MSN, Seat_Configuration, _
                 Load_Data_Sheet_Sef, Basic_Weight, Basic_Arm, _
                 Subfleet_Id, intVersionNo, strCUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
 Public Function DeleteRegistration(ByVal RegistrationId As Int32, ByVal intVersionNo As Integer) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteRegistration(RegistrationId, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

#End Region

#Region "Subfleet"
    <WebMethod()> _
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

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateSubfleet(strSubfleetId, strAircraftId, _
                 strMaxTaxiWeight, strMaxTakeoffWeight, strMaxLandingWeight, _
                 strMaxZeroFuelWeight, strFlightDeckWeight, strCabinCrewWeight, _
                 intVersionNo, strCreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
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

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.UpdateSubfleet(strSubfleetId, strAircraftId, _
                 strMaxTaxiWeight, strMaxTakeoffWeight, strMaxLandingWeight, _
                 strMaxZeroFuelWeight, strFlightDeckWeight, strCabinCrewWeight, _
                 intVersionNo, strUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
    Public Function Get_Subfleet(ByVal strAircraftId As String, ByVal strSubfleetId As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_Subfleet(strSubfleetId, strAircraftId)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
    <WebMethod()> _
   Public Function DeleteSubfleet(ByVal strSubfleetId As String, ByVal intVersionNo As Integer) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteSubfleet(strSubfleetId, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

#End Region


#Region "Galley Arms"

    <WebMethod()> _
    Public Function Get_GalleyArms(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strChoiceID As String, ByVal strSubFleetID As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_GalleyArms(strTableId, strAircraftId, strChoiceID, strSubFleetID)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function


    <WebMethod()> _
    Public Function Create_Update_GalleyArms(ByVal Crew_Galley_Arm_ID As Int64, ByVal crew_galley_desig_cl_id As String, ByVal strChoiceID As String, _
                                    ByVal arm As Decimal, ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_GalleyArms(Crew_Galley_Arm_ID, crew_galley_desig_cl_id, strChoiceID, arm, strAircraftId, VersionNo, _
                                                    strSubFleetID, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function


#End Region

#Region "Zone Definition"
    <WebMethod()> _
    Public Function Get_ZoneDefinition(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strSubFleetID As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ZoneDefinition(strTableId, strAircraftId, strSubFleetID)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function Create_Update_ZoneDefination(ByVal zone_definition_id As Int64, ByVal designation_id As String, ByVal arm As Decimal, _
                                     ByVal max_capacity As Int32, ByVal first_row_number As Int32, ByVal last_row_number As Int32, ByVal Description As String, _
                                     ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_ZoneDefination(zone_definition_id, designation_id, arm, max_capacity, first_row_number, last_row_number, Description, strAircraftId, VersionNo, _
                                                    strSubFleetID, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

#End Region

#Region "AirCraft Adjustment"
    <WebMethod()> _
   Public Function Get_Aircraft_Config_Adjustments(ByVal strTableId As String, ByVal strAircraftId As String, ByVal strSubFleetID As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_Aircraft_Config_Adjustments(strTableId, strAircraftId, strSubFleetID)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function Create_Update_AirCraftConfigAdj(ByVal air_conf_adjust_id As Int64, ByVal reference_cl_id As String, ByVal is_enabled As String, ByVal empty_weight As Int32, _
                                    ByVal arm As Decimal, ByVal strAircraftId As String, ByVal VersionNo As Int32, _
                                   ByVal strSubFleetID As String, ByVal LastUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_AirCraftConfigAdj(air_conf_adjust_id, reference_cl_id, is_enabled, empty_weight, arm, strAircraftId, VersionNo, _
                                                    strSubFleetID, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
#End Region


#Region "Choice_List"
    <WebMethod()> _
Public Function CreateChoice_List(ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateChoice_List(strchoice_list_id, strdescription, _
                 is_active, _
                 intVersionNo, strCreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
 Public Function DeleteChoice_List(ByVal strChoice_list_Id As String, ByVal intVersionNo As Integer) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteChoice_List(strChoice_list_Id, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
Public Function UpdateChoice_List(ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_active As Integer, _
           ByVal intVersionNo As Integer) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.UpdateChoice_List(strchoice_list_id, strdescription, _
                 is_active, _
                 intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
    Public Function Get_ChoiceList(ByVal strChoice_list_id As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ChoiceList(strChoice_list_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
#End Region
#Region "Choices"
    <WebMethod()> _
Public Function CreateChoices(ByVal straircraft_id As String, ByVal strchoices_id As String, ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_default As Integer, ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateChoices(straircraft_id, strchoices_id, strchoice_list_id, _
            strdescription, _
            is_default, is_active, _
            intVersionNo, _
            strCreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
Public Function UpdateChoices(ByVal strchoices_id As String, ByVal strchoice_list_id As String, _
           ByVal strdescription As String, _
           ByVal is_default As Integer, ByVal is_active As Integer, _
           ByVal intVersionNo As Integer, _
           ByVal strCreatedBy As String) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.UpdateChoices(strchoices_id, strchoice_list_id, _
            strdescription, _
            is_default, is_active, _
            intVersionNo, _
            strCreatedBy)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
    Public Function Get_Choices(ByVal straircraft_id As String, ByVal strchoices_id As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_Choices(straircraft_id, strchoices_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
    <WebMethod()> _
 Public Function DeleteChoices(ByVal strChoices_Id As String, ByVal intVersionNo As Integer) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DeleteChoices(strChoices_Id, intVersionNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
#End Region
#Region "ULDDefinition"
    <WebMethod()> _
   Public Function Get_ULDDefiniton(ByVal strTabeleId As Integer, ByVal straircraft_id As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ULDDefiniton(strTabeleId, straircraft_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
    <WebMethod()> _
Public Function Create_Update_ULDDefinition(ByVal struld_definition_id As Integer, ByVal struld_cl_id As String, _
           ByVal allow_cargo As Boolean, _
           ByVal allow_bags As Boolean, ByVal strtare_weight As Integer, _
           ByVal straircraft_id As String, _
           ByVal intVersionNo As Integer) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_ULDDefinition(struld_definition_id, struld_cl_id, _
            allow_cargo, _
            allow_bags, strtare_weight, _
            straircraft_id, _
            intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
#End Region
#Region "Operational Limit"
    <WebMethod()> _
    Public Function Create_Update_OprLimit(ByVal operational_limits_id As Integer, ByVal aircraft_config_cl_id As String, _
           ByVal op_limit_type_cl_id As String, _
           ByVal weight As Integer, ByVal MAC As Integer, _
           ByVal Subfleet_Id As String, _
           ByVal intVersionNo As Integer) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_OprLimit(operational_limits_id, aircraft_config_cl_id, _
            op_limit_type_cl_id, _
            weight, MAC, _
            Subfleet_Id, _
            intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
  Public Function Get_OperationalLimit(ByVal strTableId As Integer, ByVal straircraftconfig_cl_id As String, ByVal op_limit_cl_id As String, ByVal strsubfleet_id As String) As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_OperationalLimit(strTableId, straircraftconfig_cl_id, op_limit_cl_id, strsubfleet_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
#End Region



#Region "ULD-Position"
    <WebMethod()> _
    Public Function Get_ULDPosition(ByVal strTableId As String, ByVal strAircraftId As String, ByVal PosID As System.Int32) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ULDPosition(strTableId, strAircraftId, PosID)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
   Public Function CreateUpdateAirCraftPostion(ByVal aircraft_config_pos_id As Int32, ByVal aircraft_conf_cl_id As String, _
                                                ByVal Pos_name As String, ByVal uld_ref_cl_id As String, ByVal Max_Pos_Load As Integer, _
                                                ByVal PosArm As System.Int32, ByVal strAircraftId As String, ByVal Version As Int32, _
                                                ByVal LastUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateUpdateAirCraftPostion(aircraft_config_pos_id, aircraft_conf_cl_id, Pos_name, uld_ref_cl_id, Max_Pos_Load, PosArm, strAircraftId, Version, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
#End Region

#Region "Under floor comp"
    <WebMethod()> _
    Public Function Get_UnderFloor_Comp(ByVal strTableId As Integer, ByVal Aircraft_Id As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_UnderFloor_Comp(strTableId, Aircraft_Id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
   Public Function CreateUpdateUnderFloorComp(ByVal underfloor_comp_id As Int32, ByVal Comp_cl_id As String, _
                                               ByVal max_cpt_load As Integer, _
                                               ByVal pos_ref1 As Integer, ByVal pos_ref2 As Integer, ByVal pos_ref3 As Integer, _
                                               ByVal strAircraftId As String, ByVal Version As Int32, _
                                               ByVal LastUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateUpdateUnderFloorComp(underfloor_comp_id, Comp_cl_id, max_cpt_load, pos_ref1, pos_ref2, pos_ref3, strAircraftId, Version, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
#End Region

#Region "Common Functions"

    <WebMethod()> _
 Public Function GetAircraftConfigDetails(ByVal straircraftId As String, ByVal Choice_list_Id As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetAircraftConfigDetails(straircraftId, Choice_list_Id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
 Public Function GetULDConfigDefaultDetails(ByVal straircraftId As String, ByVal air_conf_cl_id As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.GetULDConfigDefaultDetails(straircraftId, air_conf_cl_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
#End Region

#Region "Under floor "
    <WebMethod()> _
    Public Function Get_UnderFloor(ByVal strTableId As Integer, ByVal Aircraft_Id As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_UnderFloor(strTableId, Aircraft_Id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
   Public Function CreateUpdateUnderFloor(ByVal underfloor_hold_id As Int32, ByVal hold_cl_id As String, _
                                               ByVal Max_hold_Load As Integer, _
                                               ByVal underfloor_comp_id1 As String, ByVal underfloor_comp_id2 As String, _
                                               ByVal strAircraftId As String, ByVal Version As Int32, _
                                               ByVal LastUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.CreateUpdateUnderFloor(underfloor_hold_id, hold_cl_id, Max_hold_Load, underfloor_comp_id1, underfloor_comp_id2, strAircraftId, Version, LastUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
#End Region


#Region "Service Definitions"
    <WebMethod()> _
   Public Function Create_Update_ServiceDefinition(ByVal service_definition_id As Integer, ByVal service_defintion_cl_id As String, _
          ByVal flight_no_desig_ref As String, _
          ByVal start_flight_number As Integer, ByVal end_flight_number As Integer, _
          ByVal intVersionNo As Integer) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_ServiceDefinition(service_definition_id, service_defintion_cl_id, _
            flight_no_desig_ref, _
            start_flight_number, end_flight_number, _
            intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
 Public Function Get_ChoiceListByChoicelistID(ByVal straircraftId As String, ByVal strchoice_list_id As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ChoiceListByChoicelistID(straircraftId, strchoice_list_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
    <WebMethod()> _
Public Function Get_ServiceDefinition(ByVal service_definition_id As String, ByVal service_definition_cl_id As String, ByVal flight_no_desig_ref As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ServiceDefinition(service_definition_id, service_definition_cl_id, flight_no_desig_ref)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
#End Region
#Region "Service Data"
    <WebMethod()> _
  Public Function Create_Update_ServiceData(ByVal servicedata_id As Integer, ByVal type_choicelistid As String, _
         ByVal weight As Integer, _
         ByVal occupies_seat As String, ByVal category_choicelistid As String, _
         ByVal aircraft_id As String, ByVal intVersionNo As Integer) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Create_Update_ServiceData(servicedata_id, type_choicelistid, _
            weight, occupies_seat, category_choicelistid, _
            aircraft_id, intVersionNo)
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function
    <WebMethod()> _
Public Function Get_ServiceData(ByVal servicedata_id As String, ByVal type_choicelistid As String, ByVal aircraftid As String) As DataTable

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ServiceData(servicedata_id, type_choicelistid, aircraftid)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
#End Region
#Region "Galley Weight"
    <WebMethod()> _
Public Function Get_GalleyWeight(ByVal subfleet_id As String, ByVal desig_cl_id As String) As DataTable


        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_GalleyWeight(subfleet_id, desig_cl_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function
#End Region

#Region "Under Floor Configuration"

    <WebMethod()> _
    Public Function Get_ULDConfiguration(ByVal strTableId As String, ByVal strAircraftId As String, ByVal ConfigID As System.Int32, _
                                   ByVal AirConfigID As String, ByVal ULDConfigID As String) As DataTable


        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_ULDConfiguration(strTableId, strAircraftId, ConfigID, AirConfigID, ULDConfigID)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

#End Region

#Region "Analytical Reports"
    <WebMethod()> _
   Public Function Get_IPADUDID() As DataTable


        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_IPADUDID()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
   Public Function Get_IPADDBVersionHistory(ByVal IpadUD_id As String, ByVal IsExcludeLIPad As Integer, ByVal strDBVerNo As String, ByVal strExcludeDBVerNo As String) As DataSet


        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_IPADDBVersionHistory(IpadUD_id, IsExcludeLIPad, strDBVerNo, strExcludeDBVerNo)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
   Public Function Get_IPADDetails(ByVal IpadUD_id As String) As DataSet


        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_IPADDetails(IpadUD_id)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

    <WebMethod()> _
    Public Function Get_IPADVersionNo() As DataTable
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Get_IPADVersionNo()

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function



    <WebMethod()> _
    Public Function Update_IPADDetails(ByVal IpadUD_id As String, _
                ByVal EmpNo As String, _
                ByVal IsDisabled As Integer, _
                ByVal intVersionNo As Integer, _
                ByVal strUpdatedBy As String) As Integer

        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Update_IPADDetails(IpadUD_id, EmpNo, IsDisabled, intVersionNo, strUpdatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
    Public Function Push_Pre_Production_Data_To_Production(ByVal strCreatedBy As String) As Integer
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.Push_Pre_Production_Data_To_Production(strCreatedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
    Public Function DatabaseSanityChecks() As DataSet
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.DatabaseSanityChecks()
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
  Public Function APNPushRequest() As DataSet
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.APNPushRequest()
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function

    <WebMethod()> _
Public Function APNTableSchema() As DataSet
        Dim objDal As New DAL_Manager()

        Try

            Return objDal.APNTableSchema()
        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try
    End Function


    <WebMethod()> _
    Public Function CreateAPNRequestMaster(ByVal intAPNRequestId As Integer, _
                                      ByVal strAPNMessage As String, _
                                      ByVal dtmAPNRequestDate As DateTime, _
                                      ByVal intBadgeCount As Integer, _
                                      ByVal strUserID As String, _
                                      ByVal dtAPNRequestDetail As DataTable) As Integer

        Dim objDal As New DAL_Manager()




        Try

            Return objDal.CreateAPNRequestMaster(intAPNRequestId, strAPNMessage, dtmAPNRequestDate, _
                            intBadgeCount, strUserID, dtAPNRequestDetail)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Function

#End Region



#Region "Home"
    <WebMethod()> _
 Public Sub Publish_Database(ByVal strDbVersion As String, ByVal strPublishedBy As String)

        Dim objDal As New DAL_Manager()

        Try
            objDal.Publish_Database(strDbVersion, strPublishedBy)

        Catch ex As Exception
            Throw ex
        Finally
            objDal = Nothing
        End Try

    End Sub

#End Region

End Class

