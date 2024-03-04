Public Class Enums
    Public Enum Functionalities
        'These Enums will be compared with report_id(column) of tb_reports.
        SystemAdministration = 1
        UserAdministration = 2
        RoleAdministration = 3
        Aircraft = 4
        AircraftConfig = 5
        AnalyticalReport = 12
        Administration = 11
        'These Enums will be compared with report_id(column) of tb_reports.
        ADMS = 30
        ADMSReadOnlyRights = 31
        WNB_FULL_ACCESS = 32
    End Enum

    Public Enum UserTypes
        SystemUser = 1
        UserCreatedUser = 0
    End Enum

    Public Enum RoleTypes
        SystemRole = 1
        UserCreatedRole = 0
    End Enum
End Class
