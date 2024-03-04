Imports System.Web.UI.DataVisualization
Imports WNB_Admin_BO


Partial Public Class ADMS_ObstacleGraph
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim loBO As New WNB_Admin_BO.WNB_Admin_BO
            Dim lsMessage As String = ""

            If loBO.IsUserHasPermission(Session("UserId"), _
                WNB_Common.Enums.Functionalities.ADMS, "", 0) = False Then

                lsMessage = "You don't have permission on Airport Database Management System."
                Response.Redirect("../Home.aspx?Message=" & lsMessage)
                Exit Sub
            End If

            If IsPostBack = False Then

                If Request("ICAO") <> String.Empty And Request("RwyId") <> String.Empty And Request("RwyMod") <> String.Empty Then
                    lblICAO.Text = Request("ICAO")
                    lblRwyId.Text = Request("RwyId")
                    lblRwyMod.Text = Request("RwyMod")
                    lblAirlineCode.Text = Session("Airlinecode")

                    PrepareGraphValues()

                End If
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                      "Message", "alert('Error while Ploting Obstacle Graph. \n" & _
                      ex.Message.Replace("'", "") & "');", True)
        End Try

    End Sub

    Public Sub PrepareGraphValues()
        Try
            Dim loADMS_BAL_RDM As New ADMS_BAL_RDM
            Dim loADMS_BAL_ADM As New ADMS_BAL_ADM
            Dim loADMS_BAL_Data_Checks As New ADMS_BAL_Data_Checks
            Dim loRunwyTable, loAirportDetails As DataSet
            Dim lbRunwayHasReciprocalRunway As Boolean
            Dim lsReciprocalRunwayId As String
            Dim loReciprocalRunwayTable As DataTable
            Dim liXMaxRangeValue As Integer
            Dim liYMaxRangeValue As Integer
            Dim liRwyTora As Integer
            Dim liRwyElevStartTora As Integer
            Dim lsObstaclePlotName As String = ""

            loRunwyTable = loADMS_BAL_RDM.GetRunWay(Session("UserId"), lblICAO.Text, lblRwyId.Text, lblRwyMod.Text)

            If Not loRunwyTable Is Nothing Then
                If loRunwyTable.Tables(0).Rows.Count > 0 Then

                    loAirportDetails = loADMS_BAL_ADM.GetAirportDetails(Session("UserId"), lblICAO.Text)
                    If Not loAirportDetails Is Nothing Then
                        If loAirportDetails.Tables(0).Rows.Count > 0 Then
                            lsObstaclePlotName = loAirportDetails.Tables(0).Rows(0)("Name") & _
                            " (" & loAirportDetails.Tables(0).Rows(0)("ICAO") & ")" & " - " & "Runway " & _
                             loRunwyTable.Tables(0).Rows(0)("RwyId") & " " & loRunwyTable.Tables(0).Rows(0)("RwyMod")
                        End If
                    End If

                    'Calculations --------------------------------
                    liRwyTora = Convert.ToInt32(loRunwyTable.Tables(0).Rows(0)("TORA"))
                    liRwyElevStartTora = Convert.ToInt32(loRunwyTable.Tables(0).Rows(0)("ElevStartTORA"))

                    lbRunwayHasReciprocalRunway = False
                    lsReciprocalRunwayId = loADMS_BAL_Data_Checks.GetRecipocalRunway(loRunwyTable.Tables(0).Rows(0)("RwyId"))
                    loReciprocalRunwayTable = loADMS_BAL_RDM.GetRunWay(Session("UserId"), loRunwyTable.Tables(0).Rows(0)("ICAO"), _
                        lsReciprocalRunwayId, loRunwyTable.Tables(0).Rows(0)("RwyMod")).Tables(0).Copy
                    If loReciprocalRunwayTable.Rows.Count > 0 Then
                        lbRunwayHasReciprocalRunway = True
                    End If

                    If loRunwyTable.Tables(2).Rows.Count > 0 Then



                        Dim tblForMaxTable = loRunwyTable.Tables(2).AsEnumerable()
                        Dim liMaxDistance = tblForMaxTable.Max(Function(r) r.Field(Of Int32)("Distance"))
                        Dim liMaxElevation = tblForMaxTable.Max(Function(r) r.Field(Of Int32)("Elevation"))

                        If liRwyTora > liMaxDistance Then
                            liXMaxRangeValue = liRwyTora
                        Else
                            liXMaxRangeValue = liMaxDistance
                        End If
                        If liRwyElevStartTora > liMaxElevation Then
                            liYMaxRangeValue = liRwyElevStartTora
                        Else
                            liYMaxRangeValue = liMaxElevation
                        End If

                        'liXMaxRangeValue = getRoundedMaxValue(liXMaxRangeValue)
                        'liYMaxRangeValue = getRoundedMaxValue(liYMaxRangeValue)

                        liXMaxRangeValue = liXMaxRangeValue + 1000
                        liYMaxRangeValue = liYMaxRangeValue + 100

                        'Calculations --------------------------------

                        Dim fontStyle As New System.Drawing.Font("Verdana", 10, Drawing.FontStyle.Bold)

                        ChartObstaclePlot.Titles.Add(lsObstaclePlotName)
                        ChartObstaclePlot.Titles.Item(0).ForeColor = Drawing.Color.Blue
                        ChartObstaclePlot.Titles.Item(0).Font = fontStyle

                        Dim liStartOfYAxis As Integer
                        If liRwyElevStartTora > 600 Then
                            liStartOfYAxis = liRwyElevStartTora - 100
                            liStartOfYAxis = getRoundedMaxValue(liStartOfYAxis)
                        End If
                        'Dim liStartOfYAxis As Integer
                        'If liRwyTora > 600 Then
                        '    liStartOfYAxis = liRwyTora - 100
                        '    liStartOfYAxis = getRoundedMaxValue(liStartOfYAxis)
                        'End If


                        ChartObstaclePlot.ChartAreas.Add("CA1")
                        ChartObstaclePlot.ChartAreas("CA1").AxisX.Minimum = 0
                        ChartObstaclePlot.ChartAreas("CA1").AxisX.Maximum = liXMaxRangeValue

                        ChartObstaclePlot.ChartAreas("CA1").AxisY.Minimum = liStartOfYAxis
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.Maximum = liYMaxRangeValue

                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.Minimum = 0
                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.Maximum = liXMaxRangeValue

                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.Minimum = liStartOfYAxis
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.Maximum = liYMaxRangeValue

                        ChartObstaclePlot.ChartAreas("CA1").AxisX.Interval = 1000
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.Interval = 100

                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.Interval = 1000
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.Interval = 100

                        ChartObstaclePlot.ChartAreas("CA1").AxisX.IsReversed = False
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.IsReversed = False
                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.IsReversed = False
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.IsReversed = True


                        ChartObstaclePlot.ChartAreas("CA1").BorderDashStyle = Charting.ChartDashStyle.Solid

                        Dim x As New System.Drawing.Font("Verdana", 10)

                        ChartObstaclePlot.ChartAreas("CA1").AxisX.Title = "Distance From Start of Takeoff ~m"
                        ChartObstaclePlot.ChartAreas("CA1").AxisX.TitleFont = x

                        ChartObstaclePlot.ChartAreas("CA1").AxisY.Title = "Elevation (AMSL) ~ft"
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.TitleFont = x

                        ChartObstaclePlot.ChartAreas("CA1").AxisX.IsStartedFromZero = True
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.IsStartedFromZero = True
                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.IsStartedFromZero = True
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.IsStartedFromZero = True
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.MajorTickMark.Enabled = False

                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.LabelStyle.ForeColor = Drawing.Color.White
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.LabelStyle.ForeColor = Drawing.Color.White



                        ChartObstaclePlot.ChartAreas("CA1").AxisX.MajorGrid.LineDashStyle = Charting.ChartDashStyle.Dot
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.MajorGrid.LineDashStyle = Charting.ChartDashStyle.Dot
                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.MajorGrid.LineDashStyle = Charting.ChartDashStyle.Dot
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.MajorGrid.LineDashStyle = Charting.ChartDashStyle.Dot

                        ChartObstaclePlot.ChartAreas("CA1").AxisX.MinorGrid.LineDashStyle = Charting.ChartDashStyle.Dot
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.MinorGrid.LineDashStyle = Charting.ChartDashStyle.Dot
                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.MinorGrid.LineDashStyle = Charting.ChartDashStyle.Dot
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.MinorGrid.LineDashStyle = Charting.ChartDashStyle.Dot

                        ChartObstaclePlot.ChartAreas("CA1").AxisX.MajorGrid.LineColor = Drawing.Color.Silver
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.MajorGrid.LineColor = Drawing.Color.Silver
                        ChartObstaclePlot.ChartAreas("CA1").AxisX.MinorGrid.LineColor = Drawing.Color.Silver
                        ChartObstaclePlot.ChartAreas("CA1").AxisY.MinorGrid.LineColor = Drawing.Color.Silver

                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.MajorGrid.Enabled = False
                        ChartObstaclePlot.ChartAreas("CA1").AxisX2.MinorGrid.Enabled = False
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.MajorGrid.Enabled = False
                        ChartObstaclePlot.ChartAreas("CA1").AxisY2.MinorGrid.Enabled = False

                        ChartObstaclePlot.Series.Add("S1")

                        If loRunwyTable.Tables(2).Rows.Count > 0 Then
                            For I = 0 To loRunwyTable.Tables(2).Rows.Count - 1
                                If Convert.ToBoolean(loRunwyTable.Tables(2).Rows(I)("Active")) = False Then
                                    Dim lsSeries As String = ""

                                    lsSeries = "S1" & Convert.ToString(I)
                                    ChartObstaclePlot.Series.Add(lsSeries)
                                    ChartObstaclePlot.Series(lsSeries).Points.AddXY(loRunwyTable.Tables(2).Rows(I)("Distance"), 0)
                                    ChartObstaclePlot.Series(lsSeries).Points.AddXY(loRunwyTable.Tables(2).Rows(I)("Distance"), loRunwyTable.Tables(2).Rows(I)("Elevation"))
                                    ChartObstaclePlot.Series(lsSeries).Color = Drawing.Color.Green
                                    ChartObstaclePlot.Series(lsSeries)("PointWidth") = 0.02

                                Else
                                    ChartObstaclePlot.Series("S1").Points.AddXY(loRunwyTable.Tables(2).Rows(I)("Distance"), 0)
                                    ChartObstaclePlot.Series("S1").Points.AddXY(loRunwyTable.Tables(2).Rows(I)("Distance"), loRunwyTable.Tables(2).Rows(I)("Elevation"))
                                    ChartObstaclePlot.Series("S1").Color = Drawing.Color.Red
                                    ChartObstaclePlot.Series("S1")("PointWidth") = 0.01

                                End If

                            Next
                        End If


                        ChartObstaclePlot.Series.Add("S2")
                        ChartObstaclePlot.Series("S2").XAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Secondary
                        ChartObstaclePlot.Series("S2").YAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Secondary
                        ChartObstaclePlot.Series("S2").ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line
                        ChartObstaclePlot.Series("S2").Color = Drawing.Color.Blue
                        ChartObstaclePlot.Series("S2")("PointWidth") = 0.02


                        'Calculations --------------------------------
                        If lbRunwayHasReciprocalRunway = True Then

                            Dim liYtoRound As Integer = liYMaxRangeValue - Convert.ToInt32(loReciprocalRunwayTable.Rows(0)("ElevStartTORA")) + liStartOfYAxis

                            ChartObstaclePlot.Series("S2").Points.AddXY(0, liYMaxRangeValue - Convert.ToInt32(loRunwyTable.Tables(0).Rows(0)("ElevStartTORA")) + liStartOfYAxis)
                            ChartObstaclePlot.Series("S2").Points.AddXY(Convert.ToInt32(loRunwyTable.Tables(0).Rows(0)("TORA")), liYMaxRangeValue - Convert.ToInt32(loReciprocalRunwayTable.Rows(0)("ElevStartTORA")) + liStartOfYAxis)
                        End If

                        If liYMaxRangeValue > 1500 Then
                            ChartObstaclePlot.Height = 800
                            ChartObstaclePlot.Width = 700
                        End If

                        If liXMaxRangeValue > 1500 Then
                            ChartObstaclePlot.Height = 800
                            ChartObstaclePlot.Width = 1350
                        End If
                        'Calculations --------------------------------

                        ChartObstaclePlot.Palette = System.Web.UI.DataVisualization.Charting.ChartColorPalette.Fire
                        ChartObstaclePlot.DataBind()

                        fontStyle = Nothing
                        x = Nothing

                    End If
                End If
            End If

            loADMS_BAL_RDM = Nothing
            loADMS_BAL_ADM = Nothing
            loADMS_BAL_Data_Checks = Nothing
            loRunwyTable = Nothing
            loAirportDetails = Nothing



        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function getRoundedMaxValue(ByVal psValue As String) As Integer
        Try
            Dim liBal, liDiff, liQuo, liValue As Integer

            liValue = Convert.ToInt32(psValue)

            If psValue.Length = 6 Then
                liQuo = Math.DivRem(liValue, 5000, liBal)
                'liDiff = 5000 - liBal
                liValue = liValue - liBal

            ElseIf psValue.Length = 5 Then
                liQuo = Math.DivRem(liValue, 1000, liBal)
                'liDiff = 1000 - liBal
                liValue = liValue - liBal

            ElseIf psValue.Length = 4 Then
                liQuo = Math.DivRem(liValue, 100, liBal)
                'liDiff = 100 - liBal
                liValue = liValue - liBal

            ElseIf psValue.Length = 3 Then
                liQuo = Math.DivRem(liValue, 10, liBal)
                'liDiff = 10 - liBal
                liValue = liValue - liBal

            End If

            Return liValue

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub btnShowObstacle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowObstacle.Click
        Try
            Response.Redirect("ADMS_ObstacleDM.aspx?ICAO=" & lblICAO.Text & "&RwyId=" & lblRwyId.Text & "&RwyMod=" & lblRwyMod.Text)

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, GetType(System.String), _
                    "Message", "alert('Error showing Obstacle Page. \n" & _
                    ex.Message.Replace("'", "") & "');", True)
        End Try
    End Sub
End Class