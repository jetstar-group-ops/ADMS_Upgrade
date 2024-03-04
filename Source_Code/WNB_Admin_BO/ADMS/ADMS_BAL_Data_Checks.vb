Imports System.IO
Imports System.Text

Public Class ADMS_BAL_Data_Checks

#Region "CONSTANTS "
    ''/* constants */

    Const DisCL = 30 * Nm_m ' 30 NM as per EAG standard splay 
    Const SplayWidth = 900 ' Half splay width ~ metre 
    Const BaseWidth = 90 ' Splay base half width ~ metre 
    Const EOPOffset = 400  'Distance by which the EOP placemark is positioned back from the end of the runway !metre 
    Const Nm_m = 1852.0
    Const Nm_km = 1.852



#End Region


#Region "AIRAC cycle"


    Structure Date_Results
        Dim StartDate As Date
        Dim EndDate As Date
    End Structure



    '***************************************************************************
    '*  CYCLES                                                                 *
    '*  Functions/procedures related to calculating AIRAC cycle dates          *
    '*  F.H. Orford - March, 1992    Updated to Delphi April 2003              * 
    '*  Converted to VB May 2014                                               *
    '***************************************************************************


    Private Function Get_Cycle(ByVal DateIn As Date) As String
        ' function to return the AIRAC cycle within which a given date lies 
        ' Valid for dates after 06/03/2014 

        Const Base_Date As String = "06/03/2014" 'The date for the start of a previous cycle
        Const Base_Num As Integer = 3            'The cycle number for the Base_Date
        Dim J As Date
        Dim Yeer As Integer
        Dim Year As Integer
        Dim Num As Integer
        Dim DeltaDays As Long
        Dim YeerStr As String
        Dim NumStr As String
        Dim Cykle As String

        If DateIn < Base_Date Then
            Return "9999"
        End If

        J = DateTime.Parse(Base_Date)
        Yeer = DatePart("yyyy", J)
        Num = Base_Num

        ' Calculate each cycle since base cycle
        Do
            Num = Num + 1
            J = DateAdd(DateInterval.Day, 28, J)
            Year = DatePart("yyyy", J)
            If Year > Yeer Then Num = 1
            Yeer = Year
            DeltaDays = DateDiff(DateInterval.Day, J, DateIn)
        Loop Until DeltaDays < 28
        ' Build Cycle string
        Yeer = Yeer Mod 100
        YeerStr = Convert.ToString(Yeer)
        If Len(YeerStr) = 1 Then YeerStr = "0" + YeerStr
        NumStr = Convert.ToString(Num)
        If Len(NumStr) = 1 Then NumStr = "0" + NumStr
        Cykle = YeerStr + NumStr
        Return Cykle
    End Function


    Public Function Get_Cycle_Dates(ByVal Cykle As String) As Date_Results

        ' Procedure to return start date for a given AIRAC cycle '

        Const Base_Date As String = "06/03/2014" ' The date for the start of a previous cycle

        Dim NextCycle As String
        Dim StartDate As Date
        Dim EndDate As Date
        Dim Results As Date_Results

        ' Calculate start date
        StartDate = DateTime.Parse(Base_Date)
        Do
            StartDate = DateAdd(DateInterval.Day, 28, StartDate)
            NextCycle = Get_Cycle(StartDate)
        Loop Until NextCycle = Cykle

        ' Calculate end date
        EndDate = DateAdd(DateInterval.Day, 27, StartDate)

        ' Results
        Results.StartDate = StartDate
        Results.EndDate = EndDate
        Return Results

    End Function  ' Get_Cycle_Dates

    Public Sub GetAIRAC_Cycle_Data(ByVal psMyDate As Date, _
                                        ByRef psAIRAC_code As String, _
                                        ByRef psCurrent_AIRAC_Cycle_Start_Date As String, _
                                        ByRef psCurrent_AIRAC_Cycle_End_Date As String, _
                                        ByRef psNext_AIRAC_Cycle_Commence_Date As String)

        Dim Dait As Date
        Dim Dates As Date_Results

        ' Get the AIRAC cycle that this date is in
        Dait = DateTime.Parse(psMyDate)
        psAIRAC_code = Get_Cycle(Dait)

        ' Get the start and end dates for this cycle

        Dates = Get_Cycle_Dates(psAIRAC_code)

        psCurrent_AIRAC_Cycle_Start_Date = Dates.StartDate
        psCurrent_AIRAC_Cycle_End_Date = Dates.EndDate

        psNext_AIRAC_Cycle_Commence_Date = Date.Parse(psCurrent_AIRAC_Cycle_End_Date).AddDays(1)

    End Sub


    'Private Sub btCalcCycle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCalcCycle.Click

    '    Dim Dait As Date
    '    Dim Dates As Date_Results

    '    ' Get the AIRAC cycle that this date is in
    '    Dait = DateTime.Parse(tbDate.Text)
    '    tbCycle.Text = Get_Cycle(Dait)

    '    ' Get the start and end dates for this cycle

    '    Dates = Get_Cycle_Dates(tbCycle.Text)

    '    tbStart.Text = Dates.StartDate
    '    tbEnd.Text = Dates.EndDate

    'End Sub

#End Region


#Region "Other Common Functions"

    Public Structure AtoB_Results
        Dim Rho As Single ' distance
        Dim TrackA As Single ' angle
        Dim TrackB As Single ' angle
    End Structure


    Structure BfromA_Results
        Dim ThyB As Single
        Dim LamdaB As Single
        Dim TrackB As Single
    End Structure

    Structure MagVar_Results
        Dim Dek, Dip, Ti, Gv As Single
        Dim Err As Integer
        Dim ErrStr As String
    End Structure

    Function Coord_StringToReal(ByVal Dir As String, ByVal Deg As Integer, ByVal Min As Integer, ByVal Sec As Single) As Single

        Dim GoodValue As Boolean = True
        Dim CoordNum As Decimal

        'Check input values

        Dir = UCase(Dir)
        If Dir <> "N" And Dir <> "S" And Dir <> "E" And Dir <> "W" Then GoodValue = False
        If (Deg < 0 Or Deg > 90) And (Dir = "N" Or Dir = "S") Then GoodValue = False
        If (Deg < 0 Or Deg > 180) And (Dir = "E" Or Dir = "W") Then GoodValue = False
        If Min < 0 Or Min > 59 Then GoodValue = False
        If Sec < 0.0 Or Not (Sec < 60.0) Then GoodValue = False

        If Not (GoodValue) Then
            Return 999.99
        End If

        CoordNum = Deg + Min / 60.0 + Sec / 3600.0


        If (CoordNum < 0.0 Or CoordNum > 90.0) And (Dir = "N" Or Dir = "S") Then GoodValue = False
        If (CoordNum < 0.0 Or CoordNum > 180.0) And (Dir = "E" Or Dir = "W") Then GoodValue = False

        If Dir = "S" Then CoordNum = CoordNum * -1.0
        If Dir = "W" Then CoordNum = CoordNum * -1.0

        If Not (GoodValue) Then
            Return 999.99
        Else
            Return CoordNum
        End If

    End Function

    'Public Function Coord_StringToReal(ByVal Dir As String, ByVal Deg As Integer, ByVal Min As Integer, ByVal Sec As Single) As Single

    '    Dim GoodValue As Boolean = True
    '    Dim CoordNum As Single

    '    'Check input values

    '    Dir = UCase(Dir)
    '    If Dir <> "N" And Dir <> "S" And Dir <> "E" And Dir <> "W" Then GoodValue = False
    '    If (Deg < 0 Or Deg > 59) And (Dir = "N" Or Dir = "S") Then GoodValue = False
    '    If (Deg < 0 Or Deg > 179) And (Dir = "E" Or Dir = "W") Then GoodValue = False
    '    If Min < 0 Or Min > 59 Then GoodValue = False
    '    If Sec < 0.0 Or Not (Sec < 60.0) Then GoodValue = False

    '    If Not (GoodValue) Then
    '        Return 999.99
    '    End If

    '    CoordNum = Deg + Min / 60.0 + Sec / 3600.0

    '    If Dir = "S" Then CoordNum = CoordNum * -1.0
    '    If Dir = "W" Then CoordNum = CoordNum * -1.0

    '    Return CoordNum
    'End Function


    Function MagVar(ByVal Alt As Single, ByVal Glat As Single, ByVal Glon As Single, ByVal Year As Single) As MagVar_Results

        '{*******************************************************************************}
        '{     Subroutine Geomag (Geomagnetic Field Computation)                         }
        '{*******************************************************************************}
        '{                                                                               }
        '{ Converted to VB - F. H. Orford - May 2014                                     }
        '{                                                                               }
        '{ This Delphi procedure is based on the following FORTRAN sub-routine           }
        '{ The original sub-routine loaded the Gauss Coefficients from a file on each run}
        '{ The Delphi version has them embedded as constants.                            }
        '{ To update the co-efficients, run the Delphi program LoadWMMp.exe, referencing }
        '{ the new '*.COF" file. This will generate the corresponding Delphi constants   }
        '{ Epoch, CMast and CdMast. The constants should be copied into this procedure   }
        '{ and the program recompiled.                                                   }
        '
        '{ Updated to Epoch 2005  in March 2006                                          }
        '
        '{ Updated to Epoch 2010  in January 2011                                        }
        '{ Data obtained from www.ngdc.noaa.gov/geomag/models.shtml using the link       }
        '{ using the link 'Download the current DoD World Magnetic Model (WMM2010)'      }

        '     Geomag Is A National Imagery And Mapping Agency (Nima) Standard
        '     Product.  It Is Covered Under Dma Military Specification:
        '     Mil-W-89500 (1993).
        '
        '     For Information On The Use And Applicability Of This Product,
        '     Contact Dma At The Following Address:
        '
        '                     Director
        '                     National Imagery And Mapping Agency/Headquarters
        '                     Attn: Code P33
        '                     12310 Sunrise Varlley Dr.
        '                     Reston, Va 20191-3449
        '                     Attn: Mr. Dick Martino
        '
        '                     Phone:   (703) 264-3002
        '                     Fax:     (703) 264-3133
        '                     Email:   Martinod@Nima.Mil
        '
        '  ***********************************************************************
        '
        '     Programmed By:  John M. Quinn  7/19/90
        '                     Fleet Products Division, Code N342
        '                     Naval Oceanographic Office  (Navoceano)
        '                     Stennis Space Center (Ssc), Ms
        '                     Phone:   Com:  (601) 688-5828
        '                              Dsn:        485-5828
        '                              Fax:  (601) 688-5221
        '
        '     Now At:         Geomagnetics Group
        '                     U.S. Geological Survey   Ms 966
        '                     Federal Center
        '                     Denver, Co 80225-0046
        '
        '                     Phone: (303) 273-8475
        '                     Fax:   (303) 273-8600
        '                     Email: Quinn@Ghtmail.Cr.Usgs.Gov
        '                     Web:   Http://Geomag.Usgs.Gov
        '
        '  ***********************************************************************
        '
        '     Purpose:  This Routine Computes The Declination (Dec),
        '               Inclination (Dip), Total Intensity (Ti) And
        '               Grid Variation (Gv - Polar Regions Only, Referenced
        '               To Grid North Of A Stereographic Projection) Of The
        '               Earth's Magnetic Field In Geodetic Coordinates
        '               From The Coefficients Of The Current Official
        '               Department Of Defense (Dod) Spherical Harmonic World
        '               Magnetic Model (Wmm-95).  The Wmm Series Of Models Is
        '               Updated Every 5 Years On January 1'st Of Those Years
        '               Which Are Divisible By 5 (I.E. 1980, 1985, 1990 Etc.)
        '               By The U.S. Geological Survey (Usgs) In Cooperation
        '               With The British Geological Survey (Bgs).  The Model
        '               Is Based On Geomagnetic Survey Measurements From
        '               Aircraft, Satellite And Geomagnetic Observatories.
        '
        '  ***********************************************************************
        '
        '     Model:  The Wmm Series Geomagnetic Models Are Composed
        '             Of Two Parts:  The Main Field Model, Which Is
        '             Valid At The Base Epoch Of The Current Model And
        '             A Secular Variation Model, Which Accounts For Slow
        '             Temporal Variations In The Main Geomagnetic Field
        '             From The Base Epoch To A Maximum Of 5 Years Beyond
        '             The Base Epoch.  For Example, The Base Epoch Of
        '             The Wmm-2000 Model Is 2000.0.  This Model Is Therefore
        '             Considered Valid Between 2000.0 And 2005.0. The
        '             Computed Magnetic Parameters Are Referenced To The
        '             Wgs-84 Ellipsoid.
        '
        '  ***********************************************************************
        '
        '     Accuracy:  In Ocean Areas At The Earth's Surface Over The
        '                Entire 5 Year Life Of A Degree And Order 12
        '                Spherical Harmonic Model Such As Wmm-95, The Estimated
        '                Maximum Rms Errors For The Various Magnetic Components
        '                Are:
        '
        '                Dec  -   0.5 Degrees
        '                Dip  -   0.5 Degrees
        '                Ti   - 280.0 Nanoteslas (Nt)
        '                Gv   -   0.5 Degrees
        '
        '                Other Magnetic Components That Can Be Derived From
        '                These Four By Simple Trigonometric Relations Will
        '                Have The Following Approximate Errors Over Ocean Areas:
        '
        '                X    - 140 Nt (North)
        '                Y    - 140 Nt (East)
        '                Z    - 200 Nt (Vertical) Positive Is Down
        '                H    - 200 Nt (Horizontal)
        '
        '                Over Land The Maximum Rms Errors Are Expected To Be
        '                Higher, Although The Rms Errors For Dec, Dip, And Gv
        '                Are Still Estimated To Be Less Than 1.0 Degree, For
        '                The Entire 5-Year Life Of The Model At The Earth's
        '                Surface.  The Other Component Errors Over Land Are
        '                More Difficult To Estimate And So Are Not Given.
        '
        '                The Accuracy At Any Given Time For All Of These
        '                Geomagnetic Parameters Depends On The Geomagnetic
        '                Latitude.  The Errors Are Least From The Equator To
        '                Mid-Latitudes And Greatest Near The Magnetic Poles.
        '
        '                It Is Very Important To Note That A Degree And
        '                Order 12 Model, Such As Wmm-2000, Describes Only
        '                The Long Wavelength Spatial Magnetic Fluctuations
        '                Due To Earth's Core.  Not Included In The Wmm Series
        '                Models Are Intermediate And Short Wavelength
        '                Spatial Fluctuations Of The Geomagnetic Field
        '                Which Originate In The Earth's Mantle And Crust.
        '                Consequently, Isolated Angular Errors At Various
        '                Positions On The Surface (Primarily Over Land, In
        '                Continental Margins And Over Oceanic Seamounts,
        '                Ridges And Trenches) Of Several Degrees May Be
        '                Expected. Also Not Included In The Model Are
        '                Nonsecular Temporal Fluctuations Of The Geomagnetic
        '                Field Of Magnetospheric And Ionospheric Origin.
        '                During Magnetic Storms, Temporal Fluctuations Can
        '                Cause Substantial Deviations Of The Geomagnetic
        '                Field From Model Values.  In Arctic And Antarctic
        '                Regions, As Well As In Equatorial Regions, Deviations
        '                From Model Values Are Both Frequent And Persistent.
        '
        '                If The Required Declination Accuracy Is More
        '                Stringent Than The Wmm Series Of Models Provide, Then
        '                The User Is Advised To Request Special (Regional Or
        '                Local) Surveys Be Performed And Models Prepared.
        '                Requests Of This Nature Should Be Made To Nima
        '                At The Address Above.
        '
        '  ***********************************************************************
        '
        '     Usage:  This Routine Is Broken Up Into Two Parts:
        '
        '             A) An Initialization Module, Which Is Called Only
        '                Once At The Beginning Of The Main (Calling)
        '                Program
        '             B) A Processing Module, Which Computes The Magnetic
        '                Field Parameters For Each Specified Geodetic
        '                Position (Altitude, Latitude, Longitude) And Time
        '
        '             Initialization Is Made Via A Single Call To The Main
        '             Entry Point (Geomag), While Subsequent Processing
        '             Calls Are Made Through The Second Entry Point (Geomg1).
        '             One Call To The Processing Module Is Required For Each
        '             Position And Time.
        '
        '             The Variable Maxdeg In The Initialization Call Is The
        '             Maximum Degree To Which The Spherical Harmonic Model
        '             Is To Be Computed.  It Must Be Specified By The User
        '             In The Calling Routine.  Normally It Is 12 But It May
        '             Be Set Less Than 12 To Increase Computational Speed At
        '             The Expense Of Reduced Accuracy.
        '
        '             The Pc Version Of This Subroutine Must Be Compiled
        '             With A Fortran 77 Compatible Compiler Such As The
        '             Microsoft Optimizing Fortran Compiler Version 4.1
        '             Or Later.
        '
        '
        '  **********************************************************************
        '
        '     References:
        '
        '       John M. Quinn, David J. Kerridge And David R. Barraclough,
        '            World Magnetic Charts For 1985 - Spherical Harmonic
        '            Models Of The Geomagnetic Field And Its Secular
        '            Variation, Geophys. J. R. Astr. Soc. (1986) 87,
        '            Pp 1143-1157
        '
        '       Defense Mapping Agency Technical Report, Tr 8350.2:
        '            Department Of Defense World Geodetic System 1984,
        '            Sept. 30 (1987)
        '
        '       Joseph C. Cain, Et Al.; A Proposed Model For The
        '            International Geomagnetic Reference Field - 1965,
        '            J. Geomag. And Geoelect. Vol. 19, No. 4, Pp 335-355
        '            (1967) (See Appendix)
        '
        '       Alfred J. Zmuda, World Magnetic Survey 1957-1969,
        '            International Association Of Geomagnetism And
        '            Aeronomy (Iaga) Bulletin #28, Pp 186-188 (1971)
        '
        '       John M. Quinn, Rachel J. Coleman, Michael R. Peck, And
        '            Stephen E. Lauber; The Joint Us/Uk 1990 Epoch
        '            World Magnetic Model, Technical Report No. 304,
        '            Naval Oceanographic Office (1991)
        '
        '       John M. Quinn, Rachel J. Coleman, Donald L. Shiel, And
        '            John M. Nigro; The Joint Us/Uk 1995 Epoch World
        '            Magnetic Model, Technical Report No. 314, Naval
        '            Oceanographic Office (1995)
        '
        '            Susan Amcmillan, David R. Barraclough, John M. Quinn, And
        '            Rachel J. Coleman;  The 1995 Revision Of The Joint Us/Uk
        '            Geomagnetic Field Models - I. Secular Variation, Journal Of
        '            Geomagnetism And Geoelectricity, Vol. 49, Pp. 229-243
        '            (1997)
        '
        '            John M. Quinn, Rachel J. Coelman, Susam Macmillan, And
        '            David R. Barraclough;  The 1995 Revision Of The Joint
        '            Us/Uk Geomagnetic Field Models: Ii. Main Field,Journal Of
        '            Geomagnetism And Geoelectricity, Vol. 49, Pp. 245 - 261
        '            (1997)
        '
        '  ***********************************************************************
        '
        '     Parameter Descriptions:
        '
        '       A      - Semimajor Axis Of Wgs-84 Ellipsoid (Km)
        '       B      - Semiminor Axis Of Wgs-84 Ellipsoid (Km)
        '       Re     - Mean Radius Of Iau-66 Ellipsoid (Km)
        '       Snorm  - Schmidt Normalization Factors
        '       C      - Gauss Coefficients Of Main Geomagnetic Model (Nt)
        '       Cd     - Gauss Coefficients Of Secular Geomagnetic Model (Nt/Yr)
        '       Tc     - Time Adjusted Geomagnetic Gauss Coefficients (Nt)
        '       Otime  - Time On Previous Call To Geomag (Yrs)
        '       Oalt   - Geodetic Altitude On Previous Call To Geomag (Yrs)
        '       Olat   - Geodetic Latitude On Previous Call To Geomag (Deg.)
        '       Olon   - Geodetic Longitude On Previous Call To Geomag (Deg.)
        '       Time   - Computation Time (Yrs)                        (Input)
        '                (Eg. 1 July 1995 = 1995.500)
        '       Alt    - Geodetic Altitude (Km)                        (Input)
        '       Glat   - Geodetic Latitude (Deg.)                      (Input)
        '       Glon   - Geodetic Longitude (Deg.)                     (Input)
        '       Epoch  - Base Time Of Geomagnetic Model (Yrs)
        '       Dtr    - Degree To Radian Conversion
        '       Sp(M)  - Sine Of (M*Spherical Coord. Longitude)
        '       Cp(M)  - Math.Cosine Of (M*Spherical Coord. Longitude)
        '       St     - Sine Of (Spherical Coord. Latitude)
        '       Ct     - Math.Cosine Of (Spherical Coord. Latitude)
        '       R      - Spherical Coordinate Radial Position (Km)
        '       Ca     - Math.Cosine Of Spherical To Geodetic Vector Rotation Angle
        '       Sa     - Sine Of Spherical To Geodetic Vector Rotation Angle
        '       Br     - Radial Component Of Geomagnetic Field (Nt)
        '       Bt     - Theta Component Of Geomagnetic Field (Nt)
        '       Bp     - Phi Component Of Geomagnetic Field (Nt)
        '       P(N,M) - Associated Legendre Polynomials (Unnormalized)
        '       Pp(N)  - Associated Legendre Polynomials For M=1 (Unnormalized)
        '       Dp(N,M)- Theta Derivative Of P(N,M) (Unnormalized)
        '       Bx     - North Geomagnetic Component (Nt)
        '       By     - East Geomagnetic Component (Nt)
        '       Bz     - Vertically Down Geomagnetic Component (Nt)
        '       Bh     - Horizontal Geomagnetic Component (Nt)
        '       Dek    - Geomagnetic Declination (Deg.)                (Output)
        '                  East=Positive Angles
        '                  West=Negative Angles
        '       Dip    - Geomagnetic Inclination (Deg.)                (Output)
        '                  Down=Positive Angles
        '                    Up=Negative Angles
        '       Ti     - Geomagnetic Total Intensity (Nt)              (Output)
        '       Gv     - Geomagnetic Grid Variation (Deg.)             (Output)
        '                Referenced To Grid North
        '                Grid North Referenced To 0 Meridian
        '                Of A Polar Stereographic Projection
        '                (Arctic/Antarctic Only)
        '       Maxdeg - Maximum Degree Of Spherical Harmonic Model    (Input)
        '       Moxord - Maximum Order Of Spherical Harmonic Model
        '
        '
        '  ***********************************************************************
        '
        '
        '     Note:  This Version Of Geomag Uses A Wmm Series Geomagnetic
        '            Fiels Model Referenced To The Wgs-84 Gravity Model
        '            Ellipsoid
        '
        '  { Initialization Module }

        Const MaxOrd As Integer = 12
        Const A As Decimal = 6378.137
        Const B As Decimal = 6356.7523142
        Const Re As Decimal = 6371.2

        ''Below entries are for year 2009-2014
        'Const Epoch As Single = 2010.0
        'Dim CMast(,) As Decimal = { _
        '   {0.0, 4944.4, -2707.7, -160.2, 286.4, 44.6, -20.8, -57.9, 11.0, -20.5, 2.8, 0.2, -0.9}, _
        '   {-29496.6, -1586.3, -576.1, 251.9, -211.2, 188.9, 44.1, -21.1, -20.0, 11.5, -0.1, 1.7, 0.3}, _
        '   {-2396.6, 3026.1, 1668.6, -536.6, 164.3, -118.2, 61.5, 6.5, 11.9, 12.8, 4.7, -0.6, 2.1}, _
        '   {1340.1, -2326.2, 1231.9, 634.0, -309.1, 0.0, -66.3, 24.9, -17.4, -7.2, 4.4, -1.8, -2.5}, _
        '   {912.6, 808.9, 166.7, -357.1, 89.4, 100.9, 3.1, 7.0, 16.7, -7.4, -7.2, 0.9, 0.5}, _
        '   {-230.9, 357.2, 200.3, -141.1, -163.0, -7.8, 55.0, -27.7, 7.0, 8.0, -1.0, -0.4, 0.6}, _
        '   {72.8, 68.6, 76.0, -141.4, -22.8, 13.2, -77.9, -3.3, -10.8, 2.1, -3.9, -2.5, 0.0}, _
        '   {80.5, -75.1, -4.7, 45.3, 13.9, 10.4, 1.7, 4.9, 1.7, -6.1, -2.0, -1.3, 0.1}, _
        '   {24.4, 8.1, -14.5, -5.6, -19.3, 11.5, 10.9, -14.1, -3.7, 7.0, -2.0, -2.1, 0.3}, _
        '   {5.4, 9.4, 3.4, -5.2, 3.1, -12.4, -0.7, 8.4, -8.5, -10.1, -8.3, -1.9, -0.9}, _
        '   {-2.0, -6.3, 0.9, -1.1, -0.2, 2.5, -0.3, 2.2, 3.1, -1.0, -2.8, -1.8, -0.2}, _
        '   {3.0, -1.5, -2.1, 1.7, -0.5, 0.5, -0.8, 0.4, 1.8, 0.1, 0.7, 3.8, 0.9}, _
        '   {-2.2, -0.2, 0.3, 1.0, -0.6, 0.9, -0.1, 0.5, -0.4, -0.4, 0.2, -0.8, 0.0}}

        'Dim CdMast(,) As Decimal = { _
        '   {0.0, -25.9, -22.5, 7.3, 1.1, 0.4, -0.2, 0.7, -0.1, 0.0, 0.1, 0.0, 0.0}, _
        '   {11.6, 16.5, -11.8, -3.9, 2.7, 1.8, -2.1, 0.3, 0.2, -0.2, -0.1, 0.1, 0.0}, _
        '   {-12.1, -4.4, 1.9, -2.6, 3.9, 1.2, -0.4, -0.1, 0.4, 0.0, 0.0, 0.0, 0.0}, _
        '   {0.4, -4.1, -2.9, -7.7, -0.8, 4.0, -0.6, -0.1, 0.4, -0.1, -0.1, 0.1, 0.0}, _
        '   {-1.8, 2.3, -8.7, 4.6, -2.1, -0.6, 0.5, -0.8, 0.1, 0.1, -0.1, 0.0, 0.0}, _
        '   {-1.0, 0.6, -1.8, -1.0, 0.9, 1.0, 0.9, -0.3, -0.1, 0.0, 0.0, 0.1, 0.1}, _
        '   {-0.2, -0.2, -0.1, 2.0, -1.7, -0.3, 1.7, 0.3, 0.4, -0.2, -0.1, 0.0, 0.0}, _
        '   {0.1, -0.1, -0.6, 1.3, 0.4, 0.3, -0.7, 0.6, 0.3, 0.3, -0.2, -0.1, 0.0}, _
        '   {-0.1, 0.1, -0.6, 0.2, -0.2, 0.3, 0.3, -0.6, 0.2, 0.2, 0.0, -0.1, 0.0}, _
        '   {0.0, -0.1, 0.0, 0.3, -0.4, -0.3, 0.1, -0.1, -0.4, -0.2, -0.1, 0.0, 0.0}, _
        '   {0.0, 0.0, -0.1, 0.2, 0.0, -0.1, -0.2, 0.0, -0.1, -0.2, -0.2, -0.1, 0.0}, _
        '   {0.0, 0.0, 0.0, 0.1, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.1, 0.0, 0.0}, _
        '   {0.0, 0.0, 0.1, 0.1, -0.1, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.1, 0.1}}

        ''Below entries are for year 2015-2020
        Const Epoch As Single = 2015.0
        Dim CMast(,) As Decimal = { _
        {0.0, 4796.2, -2845.6, -115.3, 283.4, 47.4, -20.7, -54.1, 10.2, -21.6, 3.3, -0.1, -1.0}, _
        {-29438.5, -1501.1, -642.0, 245.0, -188.6, 196.9, 33.2, -19.4, -18.1, 10.8, -0.3, 2.1, 0.5}, _
        {-2445.3, 3012.5, 1676.6, -538.3, 180.9, -119.4, 58.8, 5.6, 13.2, 11.7, 4.6, -0.7, 1.8}, _
        {1351.1, -2352.3, 1225.6, 581.9, -329.5, 16.1, -66.5, 24.4, -14.6, -6.8, 4.4, -1.1, -2.2}, _
        {907.2, 813.7, 120.3, -335.0, 70.3, 100.1, 7.3, 3.3, 16.2, -6.9, -7.9, 0.7, 0.3}, _
        {-232.6, 360.1, 192.4, -141.0, -157.4, 4.3, 62.5, -27.5, 5.7, 7.8, -0.6, -0.2, 0.7}, _
        {69.5, 67.4, 72.8, -129.8, -29.0, 13.2, -70.9, -2.3, -9.1, 1.0, -4.1, -2.1, -0.1}, _
        {81.6, -76.1, -6.8, 51.9, 15.0, 9.3, -2.8, 6.7, 2.2, -3.9, -2.8, -1.5, 0.3}, _
        {24.0, 8.6, -16.9, -3.2, -20.6, 13.3, 11.7, -16.0, -2.0, 8.5, -1.1, -2.5, 0.2}, _
        {5.4, 8.8, 3.1, -3.1, 0.6, -13.3, -0.1, 8.7, -9.1, -10.5, -8.7, -2.0, -0.9}, _
        {-1.9, -6.5, 0.2, 0.6, -0.6, 1.7, -0.7, 2.1, 2.3, -1.8, -3.6, -2.3, -0.2}, _
        {3.1, -1.5, -2.3, 2.1, -0.9, 0.6, -0.7, 0.2, 1.7, -0.2, 0.4, 3.5, 0.7}, _
        {-2.0, -0.3, 0.4, 1.3, -0.9, 0.9, 0.1, 0.5, -0.4, -0.4, 0.2, -0.9, 0.0}}

        Dim CdMast(,) As Decimal = { _
            {0.0, -26.8, -27.1, 8.4, -0.6, 0.4, 0.0, 0.7, -0.3, -0.2, 0.1, 0.0, 0.0}, _
            {10.7, 17.9, -13.3, -0.4, 5.3, 1.6, -2.2, 0.5, 0.3, -0.1, -0.1, 0.1, 0.0}, _
            {-8.6, -3.3, 2.4, 2.3, 3.0, -1.1, -0.7, -0.2, 0.3, -0.2, 0.0, 0.0, -0.1}, _
            {3.1, -6.2, -0.4, -10.4, -5.3, 3.3, 0.1, -0.1, 0.6, 0.1, 0.0, 0.1, 0.0}, _
            {-0.4, 0.8, -9.2, 4.0, -4.2, 0.1, 1.0, -0.7, -0.1, 0.1, -0.2, 0.0, 0.0}, _
            {-0.2, 0.1, -1.4, 0.0, 1.3, 3.8, 1.3, 0.1, -0.2, 0.0, 0.1, 0.0, 0.0}, _
            {-0.5, -0.2, -0.6, 2.4, -1.1, 0.3, 1.5, 0.1, 0.3, -0.2, -0.1, 0.1, 0.0}, _
            {0.2, -0.2, -0.4, 1.3, 0.2, -0.4, -0.9, 0.3, 0.0, 0.4, -0.2, 0.0, 0.0}, _
            {0.0, 0.1, -0.5, 0.5, -0.2, 0.4, 0.2, -0.4, 0.3, 0.3, 0.1, -0.1, 0.0}, _
            {0.0, -0.1, -0.1, 0.4, -0.5, -0.2, 0.1, 0.0, -0.2, -0.1, -0.1, 0.0, 0.0}, _
            {0.0, 0.0, -0.1, 0.3, -0.1, -0.1, -0.1, 0.0, -0.2, -0.1, -0.2, -0.1, 0.0}, _
            {0.0, 0.0, -0.1, 0.1, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.1, -0.1, 0.0}, _
            {0.1, 0.0, 0.0, 0.1, -0.1, 0.0, 0.1, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0}}


        Dim C(12, 12), Cd(12, 12), Tc(12, 12), Dp(12, 12), Snorm(12, 12), K(12, 12) As Decimal

        Dim Sp(12), Cp(12), Fn(12), Fm(12), Pp(12) As Decimal

        Dim Dtr, A2, B2, C2, A4, B4, C4, Flnmj, Dt, Rlon, Rlat, Srlon, Srlat, Crlon, Crlat, Srlon2, Srlat2, Crlon2, Crlat2, Q, Q1, Q2, Ct, St, R2, R, D, Ca, Sa, Aor, Ar, Br, Bt, Bp, Bpp, Par, Temp1, Temp2, Parp, Bx, By, Bz, Bh As Decimal

        Dim J, N, M As Integer

        Dim Dek, Dip, Ti, Gv As Single
        Dim Err As Integer
        Dim ErrStr As String = ""

        Dim Results As MagVar_Results

        Err = 0
        If ((Alt < -0.16) Or (Alt > 8.0)) Then
            Err = 1
            ErrStr = "Altitude is not in the range of -0.16 to 8.0 km"
        End If
        If Math.Abs(Glat) > 89.0 Then
            Err = 2
            ErrStr = "Latitude greater than 180 degrees"
        End If
        If Math.Abs(Glon) > 180.0 Then
            Err = 3
            ErrStr = "Longitude greater than 180 degrees"
        End If
        If ((Year < (Epoch - 5.0)) Or (Year > (Epoch + 5.0))) Then
            Err = 4
            ErrStr = "Current year is not valid for Epoch " & Convert.ToString(Epoch) & " data. Mag Var not checked." & _
                      " Refer to Procedure MagVar in Unit FpNav for Epoch update process."
        End If
        If Err > 0 Then
            Results.Err = Err
            Results.ErrStr = ErrStr
            Return Results
        End If

        Dtr = Math.PI / 180.0
        Sp(0) = 0.0
        Cp(0) = 1.0
        Snorm(0, 0) = 1.0
        Pp(0) = 1.0
        Dp(0, 0) = 0.0

        C = CMast
        Cd = CdMast

        A2 = A * A
        B2 = B * B
        C2 = A2 - B2
        A4 = A2 * A2
        B4 = B2 * B2
        C4 = A4 - B4

        '  Convert Schmidt Normalized Gauss Coefficients To Unnormalized '
        Snorm(0, 0) = 1.0

        For N = 1 To MaxOrd
            Snorm(N, 0) = Snorm(N - 1, 0) * (2 * N - 1) / N
            J = 2
            For M = 0 To N
                K(N, M) = ((N - 1) * (N - 1) - M * M) / ((2 * N - 1) * (2 * N - 3))
                If (M > 0) Then
                    Flnmj = ((N - M + 1) * J) / (N + M)
                    Snorm(N, M) = Snorm(N, M - 1) * Math.Sqrt(Flnmj)
                    J = 1
                    C(M - 1, N) = Snorm(N, M) * C(M - 1, N)
                    Cd(M - 1, N) = Snorm(N, M) * Cd(M - 1, N)
                End If
                C(N, M) = Snorm(N, M) * C(N, M)
                Cd(N, M) = Snorm(N, M) * Cd(N, M)
            Next ' M
            Fn(N) = N + 1
            Fm(N) = N
        Next ' N

        K(1, 1) = 0.0

        '   Processing Module '

        Dt = Year - Epoch
        Rlon = Glon * Dtr
        Rlat = Glat * Dtr
        Srlon = Math.Sin(Rlon)
        Srlat = Math.Sin(Rlat)
        Crlon = Math.Cos(Rlon)
        Crlat = Math.Cos(Rlat)
        Srlon2 = Srlon ^ 2
        Srlat2 = Srlat ^ 2
        Crlon2 = Crlon ^ 2
        Crlat2 = Crlat ^ 2
        Sp(1) = Srlon
        Cp(1) = Crlon

        '   Convert From Geodetic Coords. To Spherical Coords '
        Q = Math.Sqrt(A2 - C2 * Srlat2)
        Q1 = Alt * Q
        Q2 = ((Q1 + A2) / (Q1 + B2)) ^ 2
        Ct = Srlat / Math.Sqrt(Q2 * Crlat2 + Srlat2)
        St = Math.Sqrt(1.0 - Ct * Ct)
        R2 = Alt * Alt + 2.0 * Q1 + (A4 - C4 * Srlat2) / (Q * Q)
        R = Math.Sqrt(R2)
        D = Math.Sqrt(A2 * Crlat2 + B2 * Srlat2)
        Ca = (Alt + D) / R
        Sa = C2 * Crlat * Srlat / (R * D)

        For M = 2 To MaxOrd
            Sp(M) = Sp(1) * Cp(M - 1) + Cp(1) * Sp(M - 1)
            Cp(M) = Cp(1) * Cp(M - 1) - Sp(1) * Sp(M - 1)
        Next

        Aor = Re / R
        Ar = Aor * Aor
        Br = 0.0
        Bt = 0.0
        Bp = 0.0
        Bpp = 0.0

        For N = 1 To MaxOrd
            Ar = Ar * Aor

            For M = 0 To N

                ' Compute Unnormalized Associated Legendre Polynomials '
                ' And Derivatives Via Recursion Relations              '

                If (N = M) Then
                    Snorm(N, M) = St * Snorm(N - 1, M - 1)
                    Dp(N, M) = St * Dp(N - 1, M - 1) + Ct * Snorm(N - 1, M - 1)
                ElseIf ((N = 1) And (M = 0)) Then
                    Snorm(N, M) = Ct * Snorm(N - 1, M)
                    Dp(N, M) = Ct * Dp(N - 1, M) - St * Snorm(N - 1, M)
                ElseIf ((N > 1) And (N <> M)) Then
                    If (M > (N - 2)) Then Snorm(N - 2, M) = 0.0
                    If (M > (N - 2)) Then Dp(N - 2, M) = 0.0
                    Snorm(N, M) = Ct * Snorm(N - 1, M) - K(N, M) * Snorm(N - 2, M)
                    Dp(N, M) = Ct * Dp(N - 1, M) - St * Snorm(N - 1, M) - K(N, M) * Dp(N - 2, M)
                End If

                '         { Time Adjust The Gauss Coefficients }
                Tc(N, M) = C(N, M) + Dt * Cd(N, M)

                If (M <> 0) Then
                    Tc(M - 1, N) = C(M - 1, N) + Dt * Cd(M - 1, N)
                End If

                '         { Accumulate Terms Of The Spherical Harmonic Expansions }
                Par = Ar * Snorm(N, M)

                If (M = 0) Then
                    Temp1 = Tc(N, M) * Cp(M)
                    Temp2 = Tc(N, M) * Sp(M)
                Else
                    Temp1 = Tc(N, M) * Cp(M) + Tc(M - 1, N) * Sp(M)
                    Temp2 = Tc(N, M) * Sp(M) - Tc(M - 1, N) * Cp(M)
                End If

                Bt = Bt - Ar * Temp1 * Dp(N, M)
                Bp = Bp + Fm(M) * Temp2 * Par
                Br = Br + Fn(N) * Temp1 * Par

                '         { Special Case:  North/South Geographic Poles }
                If ((St = 0.0) And (M = 1)) Then
                    If (N = 1) Then
                        Pp(N) = Pp(N - 1)
                    Else
                        Pp(N) = Ct * Pp(N - 1) - K(N, M) * Pp(N - 2)
                    End If
                    Parp = Ar * Pp(N)
                    Bpp = Bpp + Fm(M) * Temp2 * Parp
                End If

            Next ' for M = 0 to N

        Next ' for N = 1 to Maxord 


        If (St = 0.0) Then
            Bp = Bpp
        Else
            Bp = Bp / St
        End If

        '  {  Rotate Magnetic Vector Components From Spherical To }
        '  { Geodetic Coordinates                                 }
        Bx = -Bt * Ca - Br * Sa
        By = Bp
        Bz = Bt * Sa - Br * Ca

        '  { Compute Declination (Dec), Inclination (Dip) And }
        '  { Total Intensity (Ti)                             }
        Bh = Math.Sqrt(Bx * Bx + By * By)
        Ti = Math.Sqrt(Bh * Bh + Bz * Bz)
        Dek = Math.Atan(By / Bx) / Dtr
        Dip = Math.Atan(Bz / Bh) / Dtr

        '  { Compute Magnetic Grid Variation If The Current   }
        '  { Geodetic Position Is In The Arctic Or Antarctic  }
        '  { (I.E. Glat > +55 Degrees Or Glat < -55 Degrees)  }
        '  { Otherwise, Set Magnetic Grid Variation To -999.0 }

        Gv = -999.0

        If Math.Abs(Glat) >= 55.0 Then

            If ((Glat > 0.0) And (Glon >= 0.0)) Then
                Gv = Dek - Glon
            End If


            If ((Glat > 0.0) And (Glon < 0.0)) Then
                Gv = Dek + Math.Abs(Glon)
            End If

            If ((Glat < 0.0) And (Glon < 0.0)) Then
                Gv = Dek + Glon
            End If

            If ((Glat < 0.0) And (Glon < 0.0)) Then
                Gv = Dek - Math.Abs(Glon)
            End If

            If Gv > 180.0 Then
                Gv = Gv - 360.0
            End If

            If Gv < -180.0 Then
                Gv = Gv + 360.0
            End If

        End If


        Results.Dek = Dek
        Results.Dip = Dip
        Results.Ti = Ti
        Results.Gv = Gv
        Results.Err = Err
        Results.ErrStr = ErrStr
        Return Results


    End Function 'MagVar



    '-----------------------------------------------------------------------------

    Public Function Great_CircleAtoB(ByVal ThyA As Single, ByVal LamdaA As Single, _
                ByVal ThyB As Single, ByVal LamdaB As Single, ByVal Ht As Single) As AtoB_Results


        '******************************************************************************
        '* Procedure to calculate Great Circle track and distance between points      *
        '* "A" and "B" on the WGS84 Spheroid.                                         *
        '*                                                                            *
        '*  Reference :- Sadono E. M., General Non-Iterative Solutions of the Inverse *
        '*               and Direct Geodetic Problems.                                *
        '*                                                                            *
        '*      Input :-                                                              *
        '*              ThyA   -  Origin      Latitude  ~ deg ( North positive )      *
        '*              ThyB   -  Destination Latitude  ~ deg ( North positive )      *
        '*              LamdaB -  Destination Longitude ~ deg ( East  positive )      *
        '*              Ht     -  Altitude ~ feet                                     *
        '*      Output :-                                                             *
        '*              Rho    -  Distance ~ NM                                       *
        '*              TrackA -  Track out of origin    ~ deg                        *
        '*              TrackB -  Track into destination ~ deg                        *
        '*                                                                            *
        '*  Converted from Delphi - F H Orford - May 2014                             *                                             *
        '******************************************************************************

        Const f As Single = 1 / 298.25722210088     ' Flattening
        Const Nm_km As Single = 1.852
        Const Ft_km As Single = 0.3048 / 1000.0
        Const Semi_Major_Axis As Single = 6378.137   ' km
        Const Deg_Rad As Single = Math.PI / 180.0

        Dim a, BetaA, BetaB, dThy, Theta, Omega, Psi As Decimal

        Dim Rho, TrackA, TrackB As Single

        Dim Results As AtoB_Results


        ' Cheat at the poles !
        If ThyA < -89.999999999 Then
            ThyA = -89.999999999
        End If
        If ThyA > 89.999999999 Then
            ThyA = 89.999999999
        End If
        If ThyB < -89.999999999 Then
            ThyB = -89.999999999
        End If
        If ThyB > 89.999999999 Then
            ThyB = 89.999999999
        End If

        a = (Semi_Major_Axis + Ht * Ft_km) / Nm_km
        BetaA = Math.Atan(Math.Tan(ThyA * Deg_Rad) * (1.0 - f))
        BetaB = Math.Atan(Math.Tan(ThyB * Deg_Rad) * (1.0 - f))
        dThy = LamdaB - LamdaA
        If dThy > 180 Then
            dThy = dThy - 360.0
        End If
        If dThy < -180 Then
            dThy = dThy + 360.0
        End If
        dThy = dThy * Deg_Rad
        Theta = Math.Acos(Math.Sin(BetaA) * Math.Sin(BetaB) + Math.Cos(BetaA) * Math.Cos(BetaB) * Math.Cos(dThy)) '  -(1)
        If Theta < 0.000000001 Then
            Theta = 0.000000001
        End If
        Rho = a * (Theta - f / 2 * ((Math.Sin(BetaA) ^ 2 + Math.Sin(BetaB) ^ 2) * (Theta + Math.Cos(Theta) * Math.Sin(Theta)) - (2 * Math.Sin(BetaA) * Math.Sin(BetaB)) * (Theta * Math.Cos(Theta) + Math.Sin(Theta))) / Math.Sin(Theta) ^ 2)
        '                                                                     ' -(2)
        If dThy <> 0.0 Then

            Try
                Psi = (Math.Sin(BetaB) - Math.Sin(BetaA) * Math.Cos(Theta)) / (Math.Cos(BetaA) * Math.Sin(Theta))    ' -(3)
                If Psi > 0.999999999 Then Psi = 0.999999999
                Psi = Math.Acos(Psi)    ' -(3)

                TrackA = Psi / Deg_Rad
                If dThy < 0.0 Then TrackA = 360.0 - TrackA
            Catch e As ArithmeticException
                '           on EInvalidOp do
                TrackA = 0.0
            End Try

            Try
                Omega = (Math.Sin(BetaA) - Math.Sin(BetaB) * Math.Cos(Theta)) / (Math.Cos(BetaB) * Math.Sin(Theta)) '-(4)
                If Omega > 0.999999999 Then
                    Omega = 0.999999999
                End If

                Omega = Math.Acos(Omega)   ' -(4)

                TrackB = Omega / Deg_Rad
                If dThy < 0.0 Then
                    TrackB = 180.0 + TrackB
                Else
                    TrackB = 180.0 - TrackB
                End If

            Catch
                '          on EInvalidOp do
                TrackB = 0.0
            End Try

        Else
            If ThyB > ThyA Then        ' Constant longitude}
                TrackA = 0.0
                TrackB = 0.0
            Else
                TrackA = 180.0
                TrackB = 180.0
            End If
        End If

        ' Results
        Results.Rho = Rho
        Results.TrackA = TrackA
        Results.TrackB = TrackB

        Return Results

    End Function ' Great_CircleAtoB


    Function Great_CircleBfromA(ByVal ThyA As Single, ByVal LamdaA As Single, ByVal TrackA As Single, ByVal Rho As Single, ByVal Ht As Single) As BfromA_Results


        ' Procedure to calculate the co-ordinates of a point B on a Great Circle track at a given
        '  track and distance from a given point A, based on the WGS84 Spheroid.
        '
        '  Reference :- Sadono E. M., General Non-Iterative Solutions of the Inverse
        '               and Direct Geodetic Problems.
        '
        '      Input :-
        '              ThyA   -  Origin      Latitude  ~ deg ( North positive )
        '              LamdaA -  Origin      Longitude ~ deg ( East  positive )
        '              TrackA -  Track out of origin    ~ deg
        '              Rho    -  Distance ~ NM
        '              Ht     -  Altitude ~ feet
        '      Output :-
        '              ThyB   -  Destination Latitude  ~ deg ( North positive )
        '              LamdaB -  Destination Longitude ~ deg ( East  positive )
        '              TrackB -  Track into destination ~ deg

        Const f As Single = 1 / 298.25722210088     ' Flattening
        Const Nm_km As Single = 1.852
        Const Ft_km As Single = 0.3048 / 1000.0
        Const Semi_Major_Axis As Single = 6378.137   ' km
        Const Deg_Rad As Single = Math.PI / 180.0

        Dim AY, A, B, AC, BB, DS, BA, AR, C As Decimal

        Dim ThyB As Single
        Dim LamdaB As Single
        Dim TrackB As Single
        Dim Neg As Boolean
        Dim Results As BfromA_Results

        AY = (Ht * Ft_km + Semi_Major_Axis) / Nm_km
        TrackA = TrackA * Deg_Rad
        A = ThyA * Deg_Rad
        B = LamdaA * Deg_Rad
        BA = Math.Tan(A) * (1.0 - f)
        BA = Math.Atan(BA)
        AC = Rho / (AY * (1 - f / 2.0))

        Do
            AR = AC
            BB = Math.Sin(BA) * Math.Cos(AR) + Math.Cos(BA) * Math.Sin(AR) * Math.Cos(TrackA)
            BB = Math.Asin(BB)

            DS = Math.Sin(BA) * Math.Sin(BA) + Math.Sin(BB) * Math.Sin(BB)
            DS = DS * (AR + Math.Cos(AR) * Math.Sin(AR))
            DS = DS - (2.0 * Math.Sin(BA) * Math.Sin(BB)) * (AR * Math.Cos(AR) + Math.Sin(AR))
            DS = DS * (f / 2.0) / (Math.Sin(AR) * Math.Sin(AR))
            DS = (AR - DS) * AY
            AC = AR * Rho / DS
        Loop Until Math.Abs(Rho - DS) < 0.0005


        Try
            C = (Math.Cos(AR) - Math.Sin(BB) * Math.Sin(BA)) / (Math.Cos(BB) * Math.Cos(BA))

            If C < 0.0 Then
                Neg = True
            End If

            C = Math.Abs(C)

            If C < 1.0E-17 Then
                C = 1.0E-17
            End If

            If Neg = True Then
                C = C * -1.0
            End If

            If C > 1.0 Then
                C = 1.0
            End If

            C = Math.Acos(C)

            If TrackA > Math.PI Then
                C = C * -1
            End If

        Catch e As ArithmeticException
            '        on E:EMathError do
            '        ShowMessage('ThyA ' + FloatToStr(ThyA) + 'LamdaA ' +  FloatToStr(LamdaA) + 'TrackA ' +  FloatToStr(TrackA) + 'Rho ' + FloatToStr(Rho))
        End Try

        TrackB = Math.Sin(BA) - Math.Sin(BB) * Math.Cos(AR)
        TrackB = TrackB / (Math.Cos(BB) * Math.Sin(AR))
        If TrackB > 0.99999999999 Then
            TrackB = 0.99999999999
        End If

        If TrackB < -1.0 Then
            TrackB = -1.0
        End If

        TrackB = Math.Acos(TrackB)
        TrackB = TrackB / Deg_Rad

        If C >= 0.0 Then
            TrackB = 180.0 - TrackB
        Else
            TrackB = 180.0 + TrackB
        End If

        LamdaB = (B + C) / Deg_Rad

        If LamdaB > 180.0 Then
            LamdaB = LamdaB - 360.0
        End If

        If LamdaB < -180.0 Then
            LamdaB = LamdaB + 360.0
        End If


        ThyB = Math.Atan(Math.Tan(BB) / (1.0 - f))
        ThyB = ThyB / Deg_Rad

        Results.ThyB = ThyB
        Results.LamdaB = LamdaB
        Results.TrackB = TrackB

        Return Results

    End Function  ' Great_CircleBfromA

    Function RecipRwy(ByVal RwyIn As String) As String
        'Return reciprical runway identifier }

        ' Input is "nnM"
        ' nn is runway identifier number
        ' M is ident modifier "L", "C" or "R"

        'Output is same identifier as input

        Dim DirStr As String
        Dim DirVal As Integer
        Dim LatType As String
        Dim RwyOut As String

        DirStr = RwyIn.Substring(0, 2)
        DirVal = Convert.ToInt32(DirStr)
        DirVal = DirVal + 18

        If DirVal > 36 Then
            DirVal = DirVal - 36
        End If

        DirStr = Convert.ToString(DirVal)

        If Len(DirStr) < 2 Then
            DirStr = "0" & DirStr
        End If

        If Len(RwyIn) > 2 Then
            LatType = UCase(RwyIn.Substring(2, 1))
            If LatType = "L" Then
                LatType = "R"
            ElseIf LatType = "R" Then
                LatType = "L"
            End If
        Else
            LatType = ""
        End If

        RwyOut = DirStr & LatType

        Return RwyOut

    End Function ' RecipRwy



    Function TrueToMag(ByVal Dir As Single, ByVal MagVar As Single) As Single
        ' Function to convert a true direction to a magnetic direction '

        'Input
        '   Dir    - True heading
        '   MagVar - Magnetic Variation
        '                  East=Positive Value
        '                  West=Negative Value

        'Output
        '          - Magnetic Heading

        Dir = Dir - MagVar
        If Dir > 360.0 Then
            Dir = Dir - 360.0
        End If
        If Dir <= 0.0 Then
            Dir = Dir + 360.0
        End If
        Return Dir
    End Function   ' TrueToMag



    'Private Sub btCalcAB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCalcAB.Click

    '    Dim LatNumA As Single
    '    Dim LonNumA As Single
    '    Dim LatNumB As Single
    '    Dim LonNumB As Single
    '    Dim Height As Single

    '    Dim Answers As AtoB_Results

    '    LatNumA = Coord_StringToReal(tbLatDirA.Text, tbLatDegA.Text, tbLatMinA.Text, tbLatSecA.Text)
    '    LonNumA = Coord_StringToReal(tbLonDirA.Text, tbLonDegA.Text, tbLonMinA.Text, tbLonSecA.Text)
    '    LatNumB = Coord_StringToReal(tbLatDirB.Text, tbLatDegB.Text, tbLatMinB.Text, tbLatSecB.Text)
    '    LonNumB = Coord_StringToReal(tbLonDirB.Text, tbLonDegB.Text, tbLonMinB.Text, tbLonSecB.Text)
    '    Height = tbHeight.Text

    '    Answers = Great_CircleAtoB(LatNumA, LonNumA, LatNumB, LonNumB, Height)

    '    tbRho.Text = Answers.Rho
    '    tbTrackA.Text = Answers.TrackA
    '    tbTrackB.Text = Answers.TrackB


    'End Sub




    'Private Sub btCalcMagVar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCalcMagVar.Click
    '    Dim LatNumA As Single
    '    Dim LonNumA As Single
    '    Dim Height As Single
    '    Dim Yeer As Single
    '    Dim Dir As String = ""
    '    Dim Deg As Single
    '    Dim Min As Single
    '    Dim MVar As Single
    '    Dim Answers As MagVar_Results

    '    LatNumA = Coord_StringToReal(tbLatDirC.Text, tbLatDegC.Text, tbLatMinC.Text, tbLatSecC.Text)
    '    LonNumA = Coord_StringToReal(tbLonDirC.Text, tbLonDegC.Text, tbLonMinC.Text, tbLonSecC.Text)
    '    Height = Convert.ToSingle(tbHeight.Text)
    '    Yeer = Convert.ToSingle(tbYeer.Text)

    '    Answers = MagVar(Height, LatNumA, LonNumA, Yeer)

    '    MVar = Answers.Dek

    '    If MVar < 0.0 Then
    '        tbMagDir.Text = "W"
    '    Else
    '        tbMagDir.Text = "E"
    '    End If
    '    MVar = Math.Abs(MVar)
    '    Deg = Math.Truncate(MVar)
    '    MVar = (MVar - Deg) * 60.0
    '    Min = Math.Truncate(MVar)
    '    MVar = (MVar - Min) * 60.0

    '    tbMagDeg.Text = Convert.ToString(Deg)
    '    tbMagMin.Text = Convert.ToString(Min)
    '    tbMagSec.Text = Convert.ToString(MVar)



    '    'If Answers.Dek > 0 Then
    '    'Dir = " E"
    '    'End If
    '    'If Answers.Dek < 0 Then
    '    'Dir = " W"
    '    'End If

    '    'tbMagDir.Text = Dir
    '    'tbMagVar.Text = Convert.ToString(Math.Abs(Answers.Dek))

    'End Sub



    'Private Sub CalcBA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalcBA.Click

    '    Dim LatNumA As Single
    '    Dim LonNumA As Single
    '    Dim Height As Single
    '    Dim TrackA As Single
    '    Dim Rho As Single

    '    Dim LatNumBB As Single
    '    Dim LonNumBB As Single

    '    Dim Deg As Integer
    '    Dim Min As Integer

    '    Dim Answers As BfromA_Results

    '    LatNumA = Coord_StringToReal(tbLatDirAA.Text, tbLatDegAA.Text, tbLatMinAA.Text, tbLatSecAA.Text)
    '    LonNumA = Coord_StringToReal(tbLonDirAA.Text, tbLonDegAA.Text, tbLonMinAA.Text, tbLonSecAA.Text)

    '    Height = tbHeight.Text
    '    TrackA = Convert.ToSingle(tbTrackAA.Text)
    '    Rho = Convert.ToSingle(tbRhoAA.Text)

    '    Answers = Great_CircleBfromA(LatNumA, LonNumA, TrackA, Rho, Height)
    '    LatNumBB = Answers.ThyB
    '    LonNumBB = Answers.LamdaB

    '    If LatNumBB < 0.0 Then
    '        tbLatDirBB.Text = "S"
    '    Else
    '        tbLatDirBB.Text = "N"
    '    End If
    '    LatNumBB = Math.Abs(LatNumBB)
    '    Deg = Math.Truncate(LatNumBB)
    '    LatNumBB = (LatNumBB - Deg) * 60.0
    '    Min = Math.Truncate(LatNumBB)
    '    LatNumBB = (LatNumBB - Min) * 60.0
    '    tbLatDegBB.Text = Convert.ToString(Deg)
    '    tbLatMinBB.Text = Convert.ToString(Min)
    '    tbLatSecBB.Text = Convert.ToString(LatNumBB)

    '    If LonNumBB < 0.0 Then
    '        tbLonDirBB.Text = "W"
    '    Else
    '        tbLonDirBB.Text = "E"
    '    End If
    '    LonNumBB = Math.Abs(LonNumBB)
    '    Deg = Math.Truncate(LonNumBB)
    '    LonNumBB = (LonNumBB - Deg) * 60.0
    '    Min = Math.Truncate(LonNumBB)
    '    LonNumBB = (LonNumBB - Min) * 60.0
    '    tbLonDegBB.Text = Convert.ToString(Deg)
    '    tbLonMinBB.Text = Convert.ToString(Min)
    '    tbLonSecBB.Text = Convert.ToString(LonNumBB)

    '    tbTrackBB.Text = Answers.TrackB


    'End Sub


    'Private Sub btRecipRwy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btRecipRwy.Click
    '    tbRecip.Text = RecipRwy(tbRwy.Text)
    'End Sub


    'Private Sub btCalcHdg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btCalcHdg.Click
    '    Dim Hdg As Single
    '    Dim MVar As Single

    '    Hdg = Convert.ToSingle(tbTrueHdg.Text)
    '    MVar = Coord_StringToReal(tbMagDir.Text, tbMagDeg.Text, tbMagMin.Text, tbMagSec.Text)

    '    Hdg = TrueToMag(Hdg, MVar)

    '    tbMagHdg.Text = Convert.ToString(Hdg)
    'End Sub



    'To calculate the coordinates of the displaced landing threshold.
    Function ReturnCoords(ByVal LatNumS As Single, ByVal LonNumS As Single, ByVal BrgCL As Single, ByVal Dis As Single, ByVal OffSet As Single) As Decimal(,)

        ' Calculate the co-ordinates of a point X which is at a bearing, distance and offset '
        'from a starting point S                                                             '

        'Input
        '   LatNumS - the latitude of the starting point - decimal degrees
        '   LonNumS -  the longitude of the starting point - decimal degrees
        '   BrgCL - the bearing of a centreline from the starting point - decimal degrees
        '   Dis  - the distance along the centreline to abeam point X - decimal metres
        '   Offset - the distance perpendicular to the centreline to the point X - decimal metres

        'Output
        '   a two dimensional array conatining the latitude and longitude of point X in decimal degrees
        '   (0,0) - longitude
        '   (0,1) - latitude

        Dim PosCl As BfromA_Results
        Dim PosX As BfromA_Results
        Dim Ht As Single = 0.0
        Dim Koords(0, 1) As Decimal
        Dim ObsOffAng As Decimal

        ' Calculate position projected to centre line  '
        PosCl = Great_CircleBfromA(LatNumS, LonNumS, BrgCL, Dis / Nm_m, Ht)

        ' Calculate coordinates if Offset is greater than 0 '
        If Math.Abs(OffSet) > 0 Then

            If OffSet > 0.0 Then
                ObsOffAng = 90.0
            Else
                ObsOffAng = -90.0
            End If

            PosX = Great_CircleBfromA(PosCl.ThyB, PosCl.LamdaB, IncBrg(BrgCL, ObsOffAng), Math.Abs(OffSet / Nm_m), Ht)
            Koords(0, 0) = PosX.LamdaB
            Koords(0, 1) = PosX.ThyB
        Else
            Koords(0, 0) = PosCl.LamdaB
            Koords(0, 1) = PosCl.ThyB
        End If
        Return Koords

    End Function 'ReturnCoords 


    Function IncBrg(ByVal Base As Single, ByVal Inc As Single) As Single

        'Function to increment a bearing by a given amount '

        ' Input
        '       Base - the starting bearing in degrees
        '       Inc  - the amount in degreessby which the bearing is incremented (positive of negative)
        ' Output
        '       The resultant bearing in degrees

        Dim Result As Single
        Result = Base + Inc
        If Result > 360.0 Then
            Result = Result - 360.0
        End If
        If Result < 0.0 Then
            Result = Result + 360.0
        End If
        Return Result
    End Function ' IncBrg



    Function ReturnSplay(ByVal LatNumS As Single, ByVal LonNumS As Single, ByVal CLHdg As Single, ByVal DisS As Single) As Decimal(,)

        ' Calculate the co-ordinates of a series of points which represent the extended centre-line '
        ' and obstacle splay for a given runway                                                     '

        'Input
        '   LatNumS - the latitude of the beginning of the runway - decimal degrees
        '   LonNumS -  the longitude of the beginning of the runway - decimal degrees
        '   CLHdg - the bearing of the runway centreline from the beginning of the runway - decimal degrees
        '   DisS  - the distance along the centreline from the beginning of the runway - decimal metres

        'Output
        '   a two dimensional array containing the latitude and longitude of the points which
        '   represent the extended centre-line and obstacle splay
        '   Refer to Design Document, Appendix 5 - Runway Centreline and Splay Geometry,
        '   Diagram 7, for definition of the points

        '   (x,0) - longitude
        '   (x,1) - latitude

        '       where x=0 is Point BC
        '       where x=1 is Point BF
        '       where x=2 is Point FL
        '       where x=3 is Point SL
        '       where x=4 is Point BL
        '       where x=5 is Point BR
        '       where x=6 is Point SR
        '       where x=7 is Point FR

        Dim Points(7, 1) As Decimal
        Dim Temp(0, 1) As Decimal
        Dim Alpha As Decimal
        Dim AlphaD As Decimal
        Dim LenS As Decimal

        ' Calculate position of point BC
        Temp = ReturnCoords(LatNumS, LonNumS, CLHdg, DisS, 0.0)
        Points(0, 0) = Temp(0, 0)
        Points(0, 1) = Temp(0, 1)

        ' Calculate position of point BF
        Temp = ReturnCoords(Points(0, 1), Points(0, 0), CLHdg, DisCL, 0.0)
        Points(1, 0) = Temp(0, 0)
        Points(1, 1) = Temp(0, 1)

        ' Calculate position of point BR
        Temp = ReturnCoords(LatNumS, LonNumS, CLHdg, DisS, 90.0)
        Points(5, 0) = Temp(0, 0)
        Points(5, 1) = Temp(0, 1)

        ' Calculate position of point BL
        Temp = ReturnCoords(LatNumS, LonNumS, CLHdg, DisS, -90.0)
        Points(4, 0) = Temp(0, 0)
        Points(4, 1) = Temp(0, 1)

        Alpha = Math.Atan(1 / 8)
        LenS = (SplayWidth - BaseWidth) / Math.Sin(Alpha)
        AlphaD = Alpha / Math.PI * 180.0

        ' Calculate position of point SR
        Temp = ReturnCoords(Points(5, 1), Points(5, 0), IncBrg(CLHdg, AlphaD), LenS, 0.0)
        Points(6, 0) = Temp(0, 0)
        Points(6, 1) = Temp(0, 1)

        ' Calculate position of point SL
        Temp = ReturnCoords(Points(4, 1), Points(4, 0), IncBrg(CLHdg, -AlphaD), LenS, 0.0)
        Points(3, 0) = Temp(0, 0)
        Points(3, 1) = Temp(0, 1)

        ' Calculate position of point FR
        Temp = ReturnCoords(Points(6, 1), Points(6, 0), CLHdg, DisCL - LenS, 0.0)
        Points(7, 0) = Temp(0, 0)
        Points(7, 1) = Temp(0, 1)

        ' Calculate position of point FL
        Temp = ReturnCoords(Points(3, 1), Points(3, 0), CLHdg, DisCL - LenS, 0.0)
        Points(2, 0) = Temp(0, 0)
        Points(2, 1) = Temp(0, 1)

        Return Points
    End Function  ' ReturnSplay


#End Region

#Region "My Public Other Common Functions"

    Public Function GetDecimalDegree(ByVal Dir As String, ByVal Deg As Integer, _
                                   ByVal Min As Integer, ByVal Sec As Single, ByRef pbIsValidValue As Boolean) As Single
        Dim liDecimalDegree As Single

        liDecimalDegree = Coord_StringToReal(Dir, Deg, Min, Sec)
        If liDecimalDegree = Convert.ToSingle(999.99) Then
            pbIsValidValue = False
        Else
            pbIsValidValue = True
        End If

        Return liDecimalDegree
    End Function

    Public Function Get_ADM_MagVarDataCheck(ByVal tbLatDirC As String, _
                                            ByVal tbLatDegC As String, _
                                            ByVal tbLatMinC As String, _
                                            ByVal tbLatSecC As String, _
                                            ByVal tbLonDirC As String, _
                                            ByVal tbLonDegC As String, _
                                            ByVal tbLonMinC As String, _
                                            ByVal tbLonSecC As String, _
                                            ByVal tbHeight As String, _
                                            ByVal tbYeer As String) As MagVar_Results
        Dim LatNumA As Single
        Dim LonNumA As Single
        Dim Height As Single
        Dim Yeer As Single
        Dim Dir As String = ""
        Dim Deg As Single
        Dim Min As Single
        Dim MVar As Single
        Dim Answers As MagVar_Results
        Dim tbMagDir As String

        LatNumA = Coord_StringToReal(tbLatDirC, tbLatDegC, tbLatMinC, tbLatSecC)
        LonNumA = Coord_StringToReal(tbLonDirC, tbLonDegC, tbLonMinC, tbLonSecC)
        Height = Convert.ToSingle(tbHeight)
        Yeer = Convert.ToSingle(tbYeer)

        Answers = MagVar(Height, LatNumA, LonNumA, Yeer)

        MVar = Answers.Dek

        If MVar < 0.0 Then
            tbMagDir = "W"
        Else
            tbMagDir = "E"
        End If
        MVar = Math.Abs(MVar)
        Deg = Math.Truncate(MVar)
        MVar = (MVar - Deg) * 60.0
        Min = Math.Truncate(MVar)
        MVar = (MVar - Min) * 60.0

        Dim tbMagDeg As String
        Dim tbMagMin As String
        Dim tbMagSec As String

        tbMagDeg = Convert.ToString(Deg)
        tbMagMin = Convert.ToString(Min)
        tbMagSec = Convert.ToString(MVar)

        Return Answers


    End Function

    Public Function GetRecipocalRunway(ByVal psRwyId As String) As String
        Return RecipRwy(psRwyId)
    End Function

#End Region


End Class
