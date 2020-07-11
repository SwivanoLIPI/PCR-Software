Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Data.OleDb
Imports System.Linq

Public Class Form1
    Dim brs As Integer
    Dim myPort As Array
    Dim baris As Integer
    Dim iterasi As Integer
    Dim TrendLine As New System.Windows.Forms.DataVisualization.Charting.Series("TrendLine")
    Dim vi As Integer
    Dim Rand As New Random
    Dim curve As Integer
    Dim FileName As String
    Dim header As String
    Dim tipeA As Integer = 3
    Dim l As ListViewItem
    Dim P_Stack As String
    Dim N As Integer
    Dim x As String
    Dim Tc As String
    Dim P_H2O As String
    Dim LV As ListView
    Dim q As Integer
    Dim j_o2 As String
    Dim z As Integer
    Dim v As String
    Dim c1 As String
    Dim c2 As String
    Dim c3 As String
    Dim c4 As String
    Dim Delimiter As String
    Dim sw As StreamWriter
    Dim legend As String
    Dim sfDialog As New SaveFileDialog
    Dim h As String
    Dim w As String
    Dim k As Integer
    Dim Mysqlcon As MySqlConnection
    Dim COMMAND As MySqlCommand
    Delegate Sub SetTextCallBack(ByVal [text] As String)

    Private Sub Button29_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button29.Click
        Try
            If ListView2.Items.Count > 0 And ComboBox8.Text = "Single Curve" Or ListView7.Columns.Count = 31 Then
                If Not Application.OpenForms().OfType(Of Form5).Any Then
                    MsgBox("Run for single curve failed ! " & (vbCrLf) & (vbCrLf) & "Choose these option to retry :" & (vbCrLf) & (vbCrLf) & "1. Clear all previous data using ""Clear All"" button" & (vbCrLf) & (vbCrLf) & "2. Press button ""Clear and Run Single Curve""." & (vbCrLf) & (vbCrLf) & "3. Change Graph mode or curve number that will use")
                    Button29.Enabled = False
                    Exit Sub
                    End
                End If
            End If
            For i = 1 To ListView4.Items.Count
                If ComboBox1.Text = ListView4.Items(i - 1).SubItems(0).Text Then
                    If Not Application.OpenForms().OfType(Of Form5).Any Then
                        MsgBox("Run for single curve fail! Try to clear previous data, press button clear then run, or change mode and curve")
                        Button29.Enabled = False
                        Exit Sub
                        End
                    End If
                End If
            Next
            Button29.Enabled = False
            Button3.Enabled = False
            If Chart1.Series(0).Points.Count <= 1 Then
                Chart1.Series(0).Points.Clear()
            End If
            If ComboBox8.Text = "Single Curve" And ListView2.Items.Count > 0 Then
                If Not Application.OpenForms().OfType(Of Form5).Any Then
                    MsgBox("Error in updating graphics. Try using button clear then run or choose mode graph Multi Curve!")
                    Button29.Enabled = True
                    Button3.Enabled = True
                    Exit Sub
                End If
            End If
            If ComboBox33.Text = "" Then
                ComboBox33.BackColor = Color.Red
                MsgBox("Choose Mode First")
                ComboBox33.BackColor = Color.White
                Exit Sub
            Else
                ComboBox33.BackColor = Color.White
            End If
            If ComboBox1.Text = "Curve 2" Then
                curve = 1
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 3" Then
                curve = 2
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 4" Then
                curve = 3
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 5" Then
                curve = 4
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 6" Then
                curve = 5
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 7" Then
                curve = 6
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 8" Then
                curve = 7
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 9" Then
                curve = 8
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            ElseIf ComboBox1.Text = "Curve 10" Then
                curve = 9
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            Else
                curve = 0
                TextBox9.Text = ComboBox33.Text & " (" & curve + 1 & ")" + " VS Oxygen Permeatation"
            End If
            If Not TextBox7.Text = "" Then
                c1 = CDec(TextBox27.Text)
            End If
            If Not TextBox4.Text = "" Then
                c2 = CDec(TextBox4.Text)
            End If
            If Not TextBox7.Text = "" Then
                c3 = CDec(TextBox7.Text)
            End If
            If Not TextBox28.Text = "" Then
                c4 = CDec(TextBox28.Text)
            End If
            TextBox35.Text = ""
            TextBox34.Text = ""
            TextBox30.Text = ""
            TextBox45.Text = ""
            TextBox46.Text = 3
            ListView2.Items.Clear()
            Chart1.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
            Chart1.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
            Chart1.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
            Chart1.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
            TextBox9.Text = ComboBox33.Text + " VS Oxygen Permeatation"
            Chart1.Titles.Clear()
            Chart1.Titles.Add(TextBox9.Text)
            If ComboBox33.Text = "P_O2 Feed Surface Membrane" Then
                PO2FeedSurfaceMembrane("P_O2 feed surface membrane (Bar)")
            ElseIf ComboBox33.Text = "Membrane Characteristic Thickness" Then
                MembraneCharacteristicThickness("Membrane Characteristic Thickness(um)")
            ElseIf ComboBox33.Text = "Effective Area" Then
                EffectiveArea("Effective Area (cm^2)")
            ElseIf ComboBox33.Text = "% O2 in Effluent" Then
                O2inEffluent("% O2 in Effluent (%)")
            ElseIf ComboBox33.Text = "% N2 in Effluent" Then
                N2inEffluent("% N2 in Effluent (%)")
            ElseIf ComboBox33.Text = "Temperature" Then
                Temperatures("Temperature (Celcius)")
            ElseIf ComboBox33.Text = "Ionic Conductivity" Then
                IonicConductivity("Ionic Conductivity of Material (S.cm-1)")
            ElseIf ComboBox33.Text = "Thickness" Then
                Thickness("Thickness (cm)")
            ElseIf ComboBox33.Text = "Total O2 Chem Potential Diff" Then
                TotalO2ChemPotentialDiff("Total O2 Chem Potential Diff (J/mol)")
            ElseIf ComboBox33.Text = "Electronic Conductivity" Then
                ElectronicConductivity("Electronic Conductivity (S.cm-1)")
            Else
                OxygenDebit("Oxygen Debit(mol/s)")
            End If
            Dim m As String
            m = CDec(1 / (10 ^ TextBox46.Text))
            m = m.Replace("1", "E0")
            m = m.ToString
            If Format(CDbl(ListView2.Items(0).SubItems(2).Text), m.ToString) = Format(CDbl(ListView2.Items(1).SubItems(2).Text), m.ToString) Then
                Do
                    TextBox46.Text = TextBox46.Text + 1
                    m = CDec(1 / (10 ^ TextBox46.Text))
                    m = m.Replace("1", "E0")
                    m = m.ToString
                Loop While Format(CDbl(ListView2.Items(0).SubItems(2).Text), m.ToString) = Format(CDbl(ListView2.Items(1).SubItems(2).Text), m.ToString)
            End If
            Button29.Enabled = True
            Button3.Enabled = True
            Button2.PerformClick()
        Catch ex As Exception
            MsgBox("Error in updating graphic")
            Exit Sub
        End Try
    End Sub
    Private Sub OxygenDebit(ByVal v As String)
        Try
            Dim X_N2 As String = TextBox3.Text
            Dim X_O2 As String = TextBox18.Text
            Dim A As String = TextBox14.Text
            Dim F As String = TextBox2.Text
            Dim R As String = TextBox1.Text
            N = ((CStr(TextBox98.Text) - CStr(TextBox97.Text)) / CStr(TextBox96.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Oxygen Debit (mol/s)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Permeatation Oxygen (mol.s-1.cm-2)"
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    v = CStr(baris * (TextBox96.Text))
                    ListView2.Items(baris - 1).SubItems(1).Text = CDbl(v)
                    j_o2 = CDbl((v * (X_O2 - ((21 / 78) * X_N2) * (28 / 32) ^ 0.5)) / (A * 22.4 * 60 * 1000000000))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = CDbl(j_o2)
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = CStr(ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text)
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    ListView2.Items(baris - 1).EnsureVisible()
                    ListView2.Items(baris - 1).BackColor = Color.LightBlue
                    ListView2.Items(baris - 1).ForeColor = Color.Black
                    If baris > 1 Then
                        ListView2.Items(baris - 1).BackColor = Color.LightBlue
                        ListView2.Items(baris - 1).ForeColor = Color.Black
                        ListView2.Items(baris - 2).BackColor = Color.White
                        ListView2.Items(baris - 2).ForeColor = Color.Black
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text.ToString))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Line
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                Next
            Next
            Chart1.Invalidate()
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Public Sub wait(ByVal Dt As Double)
        Dim IDay As Double = Date.Now.DayOfYear
        Dim CDay As Double
        Dim ITime As Double = Date.Now.TimeOfDay.TotalSeconds
        Dim CTime As Double
        Dim DiffDay As Double
        Try
            Do
                Application.DoEvents()
                CDay = Date.Now.DayOfYear
                CTime = Date.Now.TimeOfDay.TotalSeconds
                DiffDay = CDay - IDay
                CTime = CTime + 86400 * DiffDay
                If CTime >= ITime + Dt Then Exit Do
            Loop
        Catch e As Exception
        End Try
    End Sub
    Sub PO2FeedSurfaceMembrane(ByVal P_o2 As String)
        Dim P_o2n As String = TextBox23.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim S_i As String = TextBox93.Text
        Dim Lo As String = TextBox24.Text
        Dim T As String = TextBox12.Text
        Try
            TextBox35.Text = ""
            TextBox34.Text = ""
            TextBox7.Text = ""
            TextBox27.Text = ""
            TextBox28.Text = ""
            TextBox4.Text = ""
            TextBox30.Text = ""
            TextBox45.Text = ""
            ListView2.Items.Clear()
            N = ((CDec(TextBox106.Text) - CDec(TextBox105.Text)) / CDec(TextBox104.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    P_o2 = (baris) * CDec(TextBox104.Text) + TextBox105.Text
                    ListView2.Items(baris - 1).SubItems(1).Text = P_o2
                    j_o2 = CDec(((S_i * R * T) / (4 * Lo ^ 2 * F ^ 2)) * Math.Log(P_o2 / P_o2n))
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.ChartAreas("ChartArea1").AxisX.ScaleView.Size = Double.NaN
                        Chart1.ChartAreas("ChartArea1").AxisY.ScaleView.Size = Double.NaN
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "P_O2 feed surface membrane (Bar)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Permeatation Oxygen (ml.min-1.cm-2)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title.ToLowerInvariant()
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub EffectiveArea(ByVal A As String)
        Dim X_N2 As String = TextBox3.Text
        Dim X_O2 As String = TextBox18.Text
        Dim v As String = TextBox95.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox17.Text) - CDec(TextBox16.Text)) / CDec(TextBox15.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    A = baris * CDec(TextBox15.Text) + TextBox16.Text
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(1).Text = A
                    j_o2 = CDec(v * (X_O2 - ((21 / 78) * X_N2) * (28 / 32) ^ 0.5)) / (A * 22.4 * 60 * 1000000000)
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Effective Area (cm^2)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "oxygen Permeatation (mol.s-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub O2inEffluent(ByVal X_O2 As String)
        Dim X_N2 As String = TextBox3.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox21.Text) - CDec(TextBox20.Text)) / CDec(TextBox19.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    X_O2 = baris * CDec(TextBox19.Text) + TextBox20.Text
                    ListView2.Items(baris - 1).SubItems(1).Text = X_O2
                    j_o2 = CDec(v * (X_O2 - ((21 / 78) * X_N2) * (28 / 32) ^ 0.5)) / (A * 22.4 * 60 * 1000000000)
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "% O2 in Effluent (%)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (mol.s-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub N2inEffluent(ByVal X_N2 As String)
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox22.Text) - CDec(TextBox6.Text)) / CDec(TextBox5.Text)) + 1
            For Me.baris = 1 To CInt(N)
                ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    X_N2 = baris * CDec(TextBox5.Text) + TextBox6.Text
                    ListView2.Items(baris - 1).SubItems(1).Text = X_N2
                    j_o2 = CDec(v * (X_O2 - ((21 / 78) * X_N2) * (28 / 32) ^ 0.5)) / (A * 22.4 * 60 * 1000000000)
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "% N2 in Effluent (%)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (mol.s-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub Temperatures(ByVal T As String)
        Dim X_N2 As String = TextBox3.Text
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Dim P_o2 As String = TextBox10.Text
        Dim P_o2n As String = TextBox23.Text
        Dim S_i As String = TextBox93.Text
        Dim Lo As String = TextBox24.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox11.Text) - CDec(TextBox8.Text)) / CDec(TextBox13.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    T = baris * CDec(TextBox13.Text) + TextBox8.Text
                    ListView2.Items(baris - 1).SubItems(1).Text = T
                    j_o2 = CDec(((S_i * R * T) / (4 * Lo ^ 2 * F ^ 2)) * Math.Log(P_o2 / P_o2n))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Temperature (Celcius)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (ml.min-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub IonicConductivity(ByVal S_i As String)
        Dim X_N2 As String = TextBox3.Text
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Dim T As String = TextBox12.Text
        Dim P_o2 As String = TextBox10.Text
        Dim P_o2n As String = TextBox23.Text
        Dim Lo As String = TextBox24.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox102.Text) - CDec(TextBox101.Text)) / CDec(TextBox100.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    S_i = baris * CDec(TextBox100.Text) + TextBox101.Text
                    ListView2.Items(baris - 1).SubItems(1).Text = S_i
                    j_o2 = CDec(((S_i * R * T) / (4 * Lo ^ 2 * F ^ 2)) * Math.Log(P_o2 / P_o2n))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Ionic Conductivity of Material (S.cm-1)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (ml.min-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub Thickness(ByVal Lo As String)
        Dim X_N2 As String = TextBox3.Text
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Dim T As String = TextBox12.Text
        Dim P_o2 As String = TextBox10.Text
        Dim P_o2n As String = TextBox23.Text
        Dim S_i As String = TextBox93.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox94.Text) - CDec(TextBox26.Text)) / CDec(TextBox25.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    Lo = baris * CDec(TextBox25.Text) + TextBox26.Text
                    ListView2.Items(baris - 1).SubItems(1).Text = Lo
                    j_o2 = CDec(((S_i * R * T) / (4 * Lo ^ 2 * F ^ 2)) * Math.Log(P_o2 / P_o2n))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Thickness (cm)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (ml.min-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub TotalO2ChemPotentialDiff(ByVal u As String)
        Dim X_N2 As String = TextBox3.Text
        Dim s_e As String = TextBox33.Text
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Dim T As String = TextBox12.Text
        Dim P_o2 As String = TextBox10.Text
        Dim P_o2n As String = TextBox23.Text
        Dim Lo As String = TextBox24.Text
        Dim Lc As String = TextBox40.Text
        Dim aplh As String = TextBox44.Text
        Dim S_i As String = TextBox93.Text
        Dim Lcte As String
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox38.Text) - CDec(TextBox37.Text)) / CDec(TextBox36.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    u = baris * CDec(TextBox36.Text) + TextBox38.Text
                    Lcte = Lo * (1 + (aplh * T))
                    ListView2.Items(baris - 1).SubItems(1).Text = u
                    j_o2 = CDec(((1 / (1 + (2 * Lc / Lcte))) * (1 / (16 * F ^ 2)) * (S_i * s_e / S_i + s_e) * (u / Lcte)))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If

                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Total O2 Chem Potential Diff (J/mol)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (ml.min-1.cm-2)"
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub ElectronicConductivity(ByVal s_e As String)
        Dim X_N2 As String = TextBox3.Text
        Dim u As String = TextBox39.Text
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Dim T As String = TextBox12.Text
        Dim P_o2 As String = TextBox10.Text
        Dim P_o2n As String = TextBox23.Text
        Dim Lo As String = TextBox24.Text
        Dim Lc As String = TextBox40.Text
        Dim aplh As String = TextBox44.Text
        Dim S_i As String = TextBox93.Text
        Dim Lcte As String
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox32.Text) - CDec(TextBox31.Text)) / CDec(TextBox29.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    s_e = baris * CDec(TextBox29.Text) + TextBox31.Text
                    Lcte = Lo * (1 + (aplh * T))
                    ListView2.Items(baris - 1).SubItems(1).Text = s_e
                    j_o2 = CDec(((1 / (1 + (2 * Lc / Lcte))) * (1 / (16 * F ^ 2)) * (S_i * s_e / S_i + s_e) * (u / Lcte)))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If
                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Electronic Conductivity (S.cm-1)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (ml.min-1.cm-2)"
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Sub MembraneCharacteristicThickness(ByVal Lc As String)
        Dim X_N2 As String = TextBox3.Text
        Dim u As String = TextBox39.Text
        Dim X_O2 As String = TextBox18.Text
        Dim A As String = TextBox14.Text
        Dim F As String = TextBox2.Text
        Dim R As String = TextBox1.Text
        Dim v As String = TextBox95.Text
        Dim T As String = TextBox12.Text
        Dim P_o2 As String = TextBox10.Text
        Dim P_o2n As String = TextBox23.Text
        Dim Lo As String = TextBox24.Text
        Dim Lcte As String
        Dim aplh As String = TextBox44.Text
        Dim S_i As String = TextBox93.Text
        Dim s_e As String = TextBox33.Text
        Try
            ListView2.Items.Clear()
            N = ((CDec(TextBox43.Text) - CDec(TextBox42.Text)) / CDec(TextBox41.Text)) + 1
            For Me.baris = 1 To CInt(N)
                l = Me.ListView2.Items.Add("")
                For j As Integer = 1 To Me.ListView2.Columns.Count
                    l.SubItems.Add("")
                Next
                For Me.iterasi = 2 To tipeA
                    ListView2.Columns(1).Text = Chart1.ChartAreas("ChartArea1").AxisX.Title
                    ListView2.Items(baris - 1).SubItems(0).Text = baris
                    Lc = (baris * CDec(TextBox41.Text)) + TextBox42.Text
                    Lcte = Lo * (1 + (aplh * T))
                    ListView2.Items(baris - 1).SubItems(1).Text = Lc
                    j_o2 = CDec(((1 / (1 + (2 * (Lc / 10000) / Lcte))) * (1 / (16 * F ^ 2)) * (S_i * s_e / S_i + s_e) * (u / Lcte)))
                    ListView2.Items(CInt(baris - 1)).SubItems(2).Text = j_o2
                    If baris > 1 Then
                        ListView2.Items(baris - 1).SubItems(3).Text = ListView2.Items(baris - 2).SubItems(2).Text - ListView2.Items(baris - 1).SubItems(2).Text
                    Else
                        ListView2.Items(baris - 1).SubItems(3).Text = 0
                    End If
                    TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text))
                    TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text))
                    If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    Else
                        TextBox35.Text = Format(CDbl(TextBox35.Text), "0.0000E0")
                        TextBox45.Text = Format(CDbl(TextBox45.Text), "0.0000E0")
                        TextBox35.BackColor = Color.LightGreen
                        TextBox45.BackColor = Color.LightGreen
                    End If

                    If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                        TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    Else
                        TextBox34.Text = Format(CDbl(TextBox34.Text), "0.0000E0")
                        TextBox30.Text = Format(CDbl(TextBox30.Text), "0.0000E0")
                        TextBox34.BackColor = Color.Yellow
                        TextBox30.BackColor = Color.Yellow
                    End If
                    If ComboBox12.Text = "Real Time" Then
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        wait(0.001)
                    End If
                    TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
                    TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
                    TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
                    TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
                    TextBox28.BackColor = Color.LightBlue
                    TextBox4.BackColor = Color.LightBlue
                    TextBox27.BackColor = Color.GreenYellow
                    TextBox7.BackColor = Color.GreenYellow
                    Chart1.ChartAreas("ChartArea1").AxisX.Title = "Membrane Characteristic Thickness(um)"
                    Chart1.ChartAreas("ChartArea1").AxisY.Title = "Oxygen Permeatation (ml.min-1.cm-2)"
                Next
            Next
            GroupBox3.Enabled = True
        Catch ex As Exception
            MsgBox("Error in updating graphic. Try using 2d graphics")
            Exit Sub
        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            curve = curve + 1
            TabControl1.SelectedIndex = 1
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button31_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button31.Click
        Dim saveFileDialog1 As New SaveFileDialog()
        Try
            saveFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF (*.emf)|*.emf|PNG (*.png)|*.png|SVG (*.svg)|*.svg|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif"
            saveFileDialog1.FilterIndex = 2
            saveFileDialog1.RestoreDirectory = True
            If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                Dim format As ChartImageFormat = ChartImageFormat.Bmp
                If saveFileDialog1.FileName.EndsWith("bmp") Then
                    format = ChartImageFormat.Bmp
                Else
                    If saveFileDialog1.FileName.EndsWith("jpg") Then
                        format = ChartImageFormat.Jpeg
                    Else
                        If saveFileDialog1.FileName.EndsWith("emf") Then
                            format = ChartImageFormat.Emf
                        Else
                            If saveFileDialog1.FileName.EndsWith("gif") Then
                                format = ChartImageFormat.Gif
                            Else
                                If saveFileDialog1.FileName.EndsWith("png") Then
                                    format = ChartImageFormat.Png
                                Else
                                    If saveFileDialog1.FileName.EndsWith("tif") Then
                                        format = ChartImageFormat.Tiff
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Chart1.SaveImage(saveFileDialog1.FileName, format)
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button32.Click
        Dim m As String
        m = CDec(1 / (10 ^ TextBox46.Text))
        m = m.Replace("1", "E0")
        m = m.ToString
        Try
            If ComboBox8.Text = "Multi Curve" Then
                For i = 0 To ListView4.Items.Count - 1
                    Chart1.Series(CInt(i)).IsVisibleInLegend = True
                    Chart1.Series(CInt(i)).LegendText = ComboBox33.Text & "(" & i & ")"
                    Chart1.Series(CInt(i)).IsVisibleInLegend = True
                    For g = 1 To ListView2.Items.Count
                        If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox35.Text), m.ToString) Then
                            ListView2.Items(g - 1).BackColor = Color.LightGreen
                            Chart1.Series(CInt(i)).Points(g - 1).Label = "max(" & i + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                            Chart1.Series(CInt(i)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                            Chart1.Series(CInt(i)).Points(g - 1).LabelForeColor = Color.LightBlue
                            Chart1.Series(CInt(i)).Points(g - 1).MarkerSize = 10
                            Chart1.Series(CInt(i)).Points(g - 1).MarkerColor = Color.LightGreen
                            Chart1.Series(CInt(i)).IsVisibleInLegend = True
                            Chart1.Series(CInt(i)).LegendText = ComboBox33.Text & "(" & i & ")"
                        End If
                    Next g
                    For g = 1 To ListView2.Items.Count
                        If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                            ListView2.Items(g - 1).BackColor = Color.Yellow
                            Chart1.Series(CInt(i)).Points(g - 1).Label = "min(" & i + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                            Chart1.Series(CInt(i)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                            Chart1.Series(CInt(i)).Points(g - 1).LabelForeColor = Color.LightBlue
                            Chart1.Series(CInt(i)).Points(g - 1).MarkerSize = 10
                            Chart1.Series(CInt(i)).Points(g - 1).MarkerColor = Color.Yellow
                            Chart1.Series(CInt(i)).IsVisibleInLegend = True
                            Chart1.Series(CInt(i)).LegendText = ComboBox33.Text & "(" & i & ")"
                        End If
                    Next g
                    Chart1.Series(CInt(i)).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes
                    Chart1.Series(CInt(i)).SmartLabelStyle.IsMarkerOverlappingAllowed = True
                    Chart1.Series(CInt(i)).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Right
                Next
            Else
                For g = 1 To ListView2.Items.Count
                    If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox35.Text), m.ToString) Then
                        ListView2.Items(g - 1).BackColor = Color.LightGreen
                        Chart1.Series(CInt(curve)).Points(g - 1).Label = "max(" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                        Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                        Chart1.Series(CInt(curve)).Points(g - 1).LabelForeColor = Color.Blue
                        Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                        Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.LightGreen
                        Chart1.Series(CInt(curve)).IsVisibleInLegend = True
                        Chart1.Series(CInt(curve)).LegendText = ComboBox33.Text & "(" & curve & ")"
                    End If
                Next g
                For g = 1 To ListView2.Items.Count
                    If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                        ListView2.Items(g - 1).BackColor = Color.Yellow
                        Chart1.Series(CInt(curve)).Points(g - 1).Label = "min(" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                        Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                        Chart1.Series(CInt(curve)).Points(g - 1).LabelForeColor = Color.Blue
                        Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                        Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.Yellow
                        Chart1.Series(CInt(curve)).IsVisibleInLegend = True
                        Chart1.Series(CInt(curve)).LegendText = ComboBox33.Text & "(" & curve & ")"
                    End If
                Next g
                Chart1.Series(CInt(curve)).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes
                Chart1.Series(CInt(curve)).SmartLabelStyle.IsMarkerOverlappingAllowed = True
                Chart1.Series(CInt(curve)).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Right
            End If
        Catch q As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button19.Click
        Try
            With Chart2.ChartAreas(0)
                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.NotSet
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button33_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button33.Click
        Try
            If ComboBox8.Text = "Multi Curve" Then
                For i = 1 To ListView4.Items.Count
                    For g = 1 To ListView2.Items.Count
                        Chart1.Series(CInt(i - 1)).Points(g - 1).Label = ""
                        Chart1.Series(CInt(i - 1)).Points(g - 1).MarkerSize = 7
                    Next g
                Next
            Else
                For g = 1 To ListView2.Items.Count
                    Chart1.Series(CInt(curve)).Points(g - 1).Label = ""
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 7
                Next g
            End If
            Me.Chart1.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
            Me.Chart1.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
            Me.Chart1.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
            Me.Chart1.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
            TextBox67.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
            TextBox68.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
            TextBox66.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
            TextBox65.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Form1_FormClosing_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If Application.OpenForms().OfType(Of Form2).Any Then
                Form2.Close()
            ElseIf Application.OpenForms().OfType(Of Form3).Any Then
                Form3.Close()
            ElseIf Application.OpenForms().OfType(Of Form4).Any Then
                Form4.Close()
            ElseIf Application.OpenForms().OfType(Of Form5).Any Then
                Form5.Close()
            ElseIf Application.OpenForms().OfType(Of Form6).Any Then
                Form6.Close()
            ElseIf Application.OpenForms().OfType(Of Form7).Any Then
                Form7.Close()
            ElseIf Application.OpenForms().OfType(Of Form8).Any Then
                Form8.Close()
            ElseIf Application.OpenForms().OfType(Of Form9).Any Then
                Form9.Close()
            ElseIf Application.OpenForms().OfType(Of Dialog1).Any Then
                Dialog1.Close()
            ElseIf Application.OpenForms().OfType(Of Dialog2).Any Then
                Dialog2.Close()
            ElseIf Application.OpenForms().OfType(Of LoginForm1).Any Then
                LoginForm1.Close()
            ElseIf Application.OpenForms().OfType(Of SplashScreen1).Any Then
                SplashScreen1.Close()
            ElseIf Application.OpenForms().OfType(Of Form10).Any Then
                Form10.Close()
            End If
            Button53.PerformClick()
            SerialPort1.Close()
            Exit Sub
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Timer3.Stop()
            Timer1.Stop()
            Timer2.Stop()
            Timer4.Stop()
            Timer5.Stop()
            Timer6.Stop()
            GroupBox18.Enabled = False
            BtnScanPort.PerformClick()
            If My.Computer.Clock.LocalTime.Year = 2020 And My.Computer.Clock.LocalTime.Month = 3 And My.Computer.Clock.LocalTime.Day = 30 Then
                Dialog1.Show()
                Me.Visible = False
                Exit Sub
                Me.Visible = True
                Me.Show()
                Me.BringToFront()
            End If
            TextBox61.Visible = False
            Button68.Enabled = False
            Button66.Enabled = False
            Button46.Enabled = False
            Button47.Enabled = False
            If ListView4.Items.Count > 1 Then
                ComboBox8.Text = "Multi Curve"
            End If
            If ComboBox8.Text = "Multi Curve" Then
                Button68.Enabled = False
                Button66.Enabled = False
                Form7.Show()
            Else
                Label66.ForeColor = Color.Transparent
                Label66.BackColor = Color.Transparent
                ListView5.BackColor = Color.Black
                ComboBox9.Enabled = False
                ComboBox10.Enabled = False
                ComboBox11.Enabled = False
                ComboBox15.Enabled = False
                TextBox69.Text = "Disabled"
                TextBox70.Text = "Disabled"
                TextBox71.Text = "Disabled"
                TextBox69.Enabled = False
                TextBox70.Enabled = False
                TextBox71.Enabled = False
                Label50.Enabled = False
                Label51.Enabled = False
                Label52.Enabled = False
                Label13.Enabled = False
                Button57.Enabled = False
                Button51.Enabled = False
                Button52.Enabled = False
                ListView5.Enabled = False
                Form5.Hide()
            End If
            TextBox47.Visible = False
            'TextBox62.Enabled = False
            'TextBox63.Enabled = False
            Chart1.Series(0).Points.Clear()
            Timer1.Stop()
            Label104.Text = "Interval (s)"
            'TextBox59.Enabled = True
            TextBox60.Enabled = True
            Chart1.Series(0).Points.AddXY(0, 0)
            Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
            If ComboBox14.Text = "Dash" Then
                With Chart1.ChartAreas(0)
                    .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                End With
            Else
                With Chart1.ChartAreas(0)
                    .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                End With
            End If
            Me.Chart1.ChartAreas("ChartArea1").AxisX.Maximum = 10
            Me.Chart1.ChartAreas("ChartArea1").AxisX.Minimum = 0
            Me.Chart1.ChartAreas("ChartArea1").AxisY.Minimum = 0
            Me.Chart1.ChartAreas("ChartArea1").AxisY.Maximum = 10
            Me.Chart1.ChartAreas("ChartArea1").AxisY.LabelStyle.Format = "0.00E0"
            Me.Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Format = "N2"
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub ComboBox33_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox33.SelectedIndexChanged
        Try
            ComboBox33.BackColor = Color.White
            TextBox35.Text = ""
            TextBox34.Text = ""
            TextBox7.Text = ""
            TextBox27.Text = ""
            TextBox28.Text = ""
            TextBox4.Text = ""
            TextBox30.Text = ""
            TextBox45.Text = ""
            c1 = ""
            c2 = ""
            c3 = ""
            c4 = ""
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs, ByVal ParamArray fieldWidths() As Integer) Handles Button8.Click
        Try
            Dim SaveFile As New SaveFileDialog
            SaveFile.FileName = ""
            SaveFile.Filter = "Text Files (*.txt)|*.txt"
            SaveFile.Title = "Save"
            SaveFile.ShowDialog()
            Dim Write As New System.IO.StreamWriter(SaveFile.FileName)
            Dim col As ColumnHeader
            Dim columnnames As String = ""
            For Each col In ListView2.Columns
                If String.IsNullOrEmpty(columnnames) Then
                    columnnames = col.Text
                Else
                    columnnames &= "|" & col.Text
                End If
            Next
            Write.Write(columnnames & vbCrLf)
            For Me.baris = 1 To ListView2.Items.Count
                Write.Write(ListView2.Items(baris - 1).SubItems(0).Text & "|" & ListView2.Items(baris - 1).SubItems(1).Text & "|" & ListView2.Items(baris - 1).SubItems(2).Text & "|" & ListView2.Items(baris - 1).SubItems(3).Text & vbCrLf)
            Next baris
            Write.Close()
        Catch p As Exception
            Exit Sub
        End Try
    End Sub
    Public Function ExportListViewToCSV(ByVal filename As String, ByVal lv As ListView) As Boolean
        Try
            Dim os As New StreamWriter(filename)
            For i As Integer = 0 To lv.Columns.Count - 1
                os.Write("""" & lv.Columns(i).Text.Replace("""", """""") & """,")
            Next
            os.WriteLine()
            For i As Integer = 0 To lv.Items.Count - 1
                For j As Integer = 0 To lv.Columns.Count - 1
                    os.Write("""" & lv.Items(i).SubItems(j).Text.Replace("""", """""") + """,")
                Next
                os.WriteLine()
            Next
            os.Close()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Try
            Button29.Enabled = True
            ComboBox1.Text = "Curve 1"
            ListView5.Items.Clear()
            ListView7.Items.Clear()
            ListView7.Columns.Clear()
            ListView7.Columns.Add("No.", 30, HorizontalAlignment.Center)
            Form5.ListView7.Items.Clear()
            Form5.ListView1.Items.Clear()
            TextBox72.Text = 0
            ComboBox10.Text = "Curve 1"
            Form6.Button34.PerformClick()
            ListView2.Items.Clear()
            TextBox30.Text = ""
            TextBox45.Text = ""
            TextBox28.Text = ""
            TextBox7.Text = ""
            TextBox27.Text = ""
            TextBox4.Text = ""
            TextBox34.Text = ""
            TextBox35.Text = ""
            ListView7.Items.Clear()
            For i = 1 To 10
                Chart1.Series(i - 1).Points.Clear()
                Chart1.Series(i - 1).Name = "Curve " & i
            Next
            Chart1.Series(0).Points.AddXY(0, 0)
            Chart1.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
            TextBox47.Text = ""
            Button50.PerformClick()
            Button52.PerformClick()
            Button13.PerformClick()
            Button57.PerformClick()
            Chart1.ChartAreas(0).AxisX.Title = ""
            Chart1.ChartAreas(0).AxisY.Title = ""
            Chart1.Titles.Clear()
            wait(0.1)
            ListView5.Enabled = False
            ListView5.BackColor = Color.Gray
            Button68.Enabled = False
            Button66.Enabled = False
            Button51.Enabled = False
            Button57.Enabled = False
            Button52.Enabled = False
        Catch ef As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Chart1.Series(CInt(curve)).Points.Clear()
            Dim m As String
            m = CDec(1 / (10 ^ TextBox46.Text))
            m = m.Replace("1", "E0")
            m = m.ToString
            If Format(CDbl(ListView2.Items(0).SubItems(2).Text), m.ToString) = Format(CDbl(ListView2.Items(1).SubItems(2).Text), m.ToString) Then
                Do
                    TextBox46.Text = TextBox46.Text + 1
                    m = CDec(1 / (10 ^ TextBox46.Text))
                    m = m.Replace("1", "E0")
                    m = m.ToString
                Loop While Format(CDbl(ListView2.Items(0).SubItems(2).Text), m.ToString) = Format(CDbl(ListView2.Items(1).SubItems(2).Text), m.ToString)
            End If
            For Me.baris = 1 To ListView2.Items.Count
                ListView2.Items(baris - 1).SubItems(0).BackColor = Color.White
                ListView2.Items(baris - 1).SubItems(1).BackColor = Color.White
                ListView2.Items(baris - 1).SubItems(2).BackColor = Color.White
                ListView2.Items(baris - 1).SubItems(3).BackColor = Color.White
                TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text), m)
                TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text), m)
                TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text), m)
                TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text), m)
                Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                If ComboBox17.Text = "Point" Then
                    Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                ElseIf ComboBox17.Text = "Area" Then
                    Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                ElseIf ComboBox17.Text = "Fast Line" Then
                    Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Else
                    Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                End If
                If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                    TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), m.ToString)
                    TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), m.ToString)
                    TextBox35.BackColor = Color.LightGreen
                    TextBox45.BackColor = Color.LightGreen
                Else
                    TextBox35.Text = Format(CDbl(TextBox35.Text), m.ToString)
                    TextBox45.Text = Format(CDbl(TextBox45.Text), m.ToString)
                    TextBox35.BackColor = Color.LightGreen
                    TextBox45.BackColor = Color.LightGreen
                End If
                If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                    TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), m.ToString)
                    TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), m.ToString)
                    TextBox34.BackColor = Color.Yellow
                    TextBox30.BackColor = Color.Yellow
                Else
                    TextBox34.Text = Format(CDbl(TextBox34.Text), m.ToString)
                    TextBox30.Text = Format(CDbl(TextBox30.Text), m.ToString)
                    TextBox34.BackColor = Color.Yellow
                    TextBox30.BackColor = Color.Yellow
                End If
                If ComboBox14.Text = "Dash" Then
                    With Chart1.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    End With
                Else
                    With Chart1.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    End With
                End If
            Next
            Me.Chart1.ChartAreas("ChartArea1").AxisY.LabelStyle.Format = m.ToString
            Me.Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Format = "N2"
            For g = 1 To ListView2.Items.Count
                If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox35.Text), m.ToString) Then
                    ListView2.Items(g - 1).BackColor = Color.LightGreen
                    Chart1.Series(CInt(curve)).Points(g - 1).Label = "max (" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                    Chart1.Series(CInt(curve)).Points(CInt(ListView2.Items.Count - 1) / 2).Label = ComboBox33.Text & " (" & curve + 1 & ")"
                    Chart1.Series(CInt(curve)).Points(CInt(ListView2.Items.Count - 1) / 2).Font = New System.Drawing.Font("Consolas", 7.0F)
                    Chart1.Series(CInt(curve)).Points(CInt(ListView2.Items.Count - 1) / 2).LabelForeColor = Color.Gray
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.LightGreen
                    Chart1.Series(CInt(curve)).Points(g - 1).LabelForeColor = Color.Blue
                    Chart1.Series(CInt(curve)).Points(g - 1).Font = New System.Drawing.Font("Arial", 6, FontStyle.Italic)
                End If
                If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                    ListView2.Items(g - 1).BackColor = Color.Yellow
                    Chart1.Series(CInt(curve)).Points(g - 1).Label = "min (" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.Yellow
                    Chart1.Series(CInt(curve)).Points(g - 1).LabelForeColor = Color.Blue
                    Chart1.Series(CInt(curve)).Points(g - 1).Font = New System.Drawing.Font("Arial", 6, FontStyle.Italic)
                End If
            Next g
            TextBox28.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
            TextBox28.BackColor = Color.LightBlue
            TextBox4.BackColor = Color.LightBlue
            TextBox27.BackColor = Color.GreenYellow
            TextBox7.BackColor = Color.GreenYellow
            TextBox7.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
            TextBox4.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
            TextBox27.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
            Chart1.Series(CInt(curve)).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
            Chart1.Series(CInt(curve)).SmartLabelStyle.IsMarkerOverlappingAllowed = vbFalse
            Chart1.Series(CInt(curve)).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Top
            Chart1.Series(curve).Name = ComboBox33.Text & " (" & curve + 1 & ")"
            Chart1.ChartAreas("ChartArea1").AxisX.IsLabelAutoFit = True
            Chart1.ChartAreas("ChartArea1").AxisY.IsLabelAutoFit = True
            l = Me.ListView4.Items.Add(ComboBox1.Text)
            l.SubItems.Add((ComboBox33.Text))
            l.SubItems.Add((TextBox35.Text))
            l.SubItems.Add((TextBox45.Text))
            l.SubItems.Add(TextBox34.Text)
            l.SubItems.Add((TextBox30.Text))
            l.SubItems.Add(TextBox27.Text + CDec(TextBox27.Text - TextBox4.Text) / 4)
            l.SubItems.Add(TextBox28.Text + CDec(TextBox28.Text - TextBox7.Text) / 4)
            l.SubItems.Add(TextBox4.Text - CDec(TextBox27.Text - TextBox4.Text) / 4)
            l.SubItems.Add(TextBox7.Text - CDec(TextBox28.Text - TextBox7.Text) / 4)
            Chart1.ChartAreas("ChartArea1").AxisY.Maximum = ListView4.Items(ListView4.Items.Count - 1).SubItems(7).Text
            Chart1.ChartAreas("ChartArea1").AxisY.Minimum = ListView4.Items(ListView4.Items.Count - 1).SubItems(9).Text
            Chart1.ChartAreas("ChartArea1").AxisX.Maximum = ListView4.Items(ListView4.Items.Count - 1).SubItems(6).Text
            Chart1.ChartAreas("ChartArea1").AxisX.Minimum = ListView4.Items(ListView4.Items.Count - 1).SubItems(8).Text
            If TextBox66.Text = "" Then
                TextBox66.Text = ListView4.Items(0).SubItems(6).Text
                TextBox67.Text = ListView4.Items(0).SubItems(7).Text
                TextBox68.Text = ListView4.Items(0).SubItems(9).Text
                TextBox65.Text = ListView4.Items(0).SubItems(8).Text
            Else
                For br As Integer = 1 To ListView4.Items.Count
                    If (ListView4.Items(br - 1).SubItems(7).Text) > (TextBox67.Text) Then
                        TextBox67.Text = (ListView4.Items(br - 1).SubItems(7).Text)
                    Else
                        TextBox67.Text = TextBox67.Text
                    End If
                    If ListView4.Items(br - 1).SubItems(6).Text > TextBox66.Text Then
                        TextBox66.Text = ListView4.Items(br - 1).SubItems(6).Text
                    Else
                        TextBox66.Text = TextBox66.Text
                    End If
                    If (ListView4.Items(br - 1).SubItems(9).Text) < CDec(TextBox68.Text) Then
                        TextBox68.Text = ListView4.Items(br - 1).SubItems(9).Text
                    Else
                        TextBox68.Text = TextBox68.Text
                    End If

                    If ListView4.Items(br - 1).SubItems(8).Text < TextBox65.Text Then
                        TextBox65.Text = (ListView4.Items(br - 1).SubItems(8).Text)
                    Else
                        TextBox65.Text = TextBox65.Text
                    End If
                Next
                Chart1.ChartAreas("ChartArea1").AxisY.Maximum = ListView4.Items(ListView4.Items.Count - 1).SubItems(7).Text
                Chart1.ChartAreas("ChartArea1").AxisY.Minimum = ListView4.Items(ListView4.Items.Count - 1).SubItems(9).Text
                Chart1.ChartAreas("ChartArea1").AxisX.Maximum = ListView4.Items(ListView4.Items.Count - 1).SubItems(6).Text
                Chart1.ChartAreas("ChartArea1").AxisX.Minimum = ListView4.Items(ListView4.Items.Count - 1).SubItems(8).Text
            End If
            ListView7.Columns.Add(ColumnHeader5.Text & " (" & curve + 1 & ")", 100, HorizontalAlignment.Center)
            ListView7.Columns.Add(ColumnHeader6.Text & " (" & curve + 1 & ")", 120, HorizontalAlignment.Center)
            ListView7.Columns.Add(ColumnHeader10.Text & " (" & curve + 1 & ")", 120, HorizontalAlignment.Center)
            Form5.ListView7.Columns.Add(ColumnHeader5.Text & " (" & curve + 1 & ")", 100, HorizontalAlignment.Center)
            Form5.ListView7.Columns.Add(ColumnHeader6.Text & " (" & curve + 1 & ")", 120, HorizontalAlignment.Center)
            Form5.ListView7.Columns.Add(ColumnHeader10.Text & " (" & curve + 1 & ")", 120, HorizontalAlignment.Center)
            For baris As Integer = CInt(0 + Form5.ListView1.Items.Count) To Me.ListView4.Items.Count - 1
                l = Form5.ListView1.Items.Add(ListView4.Items(baris).SubItems(0).Text)
                For j = 1 To Me.ListView4.Columns.Count - 1
                    l.SubItems.Add(ListView4.Items(baris).SubItems(j).Text)
                Next
            Next
            For Me.baris = CInt(1) To CInt(ListView2.Items.Count - Form5.ListView7.Items.Count + 1)
                l = Me.ListView7.Items.Add("")
                For j As Integer = CInt(2 + CInt(TextBox72.Text)) To CInt(ListView2.Items.Count + TextBox72.Text)
                    l.SubItems.Add("")
                Next
            Next
            For Me.baris = CInt(1) To CInt(ListView2.Items.Count - Form5.ListView7.Items.Count + 1)
                l = Form5.ListView7.Items.Add("")
                For j As Integer = CInt(2 + CInt(TextBox72.Text)) To CInt(ListView2.Items.Count + TextBox72.Text)
                    l.SubItems.Add("")
                Next
            Next
            For Me.baris = 1 To CInt(ListView2.Items.Count)
                ListView7.Items(baris - 1).SubItems(0).Text = ListView2.Items(baris - 1).SubItems(0).Text
                ListView7.Items(baris - 1).SubItems(1 + CInt(TextBox72.Text)).Text = ListView2.Items(baris - 1).SubItems(1).Text
                ListView7.Items(baris - 1).SubItems(2 + CInt(TextBox72.Text)).Text = ListView2.Items(baris - 1).SubItems(2).Text
                ListView7.Items(baris - 1).SubItems(3 + CInt(TextBox72.Text)).Text = ListView2.Items(baris - 1).SubItems(3).Text
            Next
            For Me.baris = 1 To CInt(ListView2.Items.Count)
                Form5.ListView7.Items(baris - 1).SubItems(0).Text = ListView2.Items(baris - 1).SubItems(0).Text
                Form5.ListView7.Items(baris - 1).SubItems(1 + CInt(TextBox72.Text)).Text = ListView2.Items(baris - 1).SubItems(1).Text
                Form5.ListView7.Items(baris - 1).SubItems(2 + CInt(TextBox72.Text)).Text = ListView2.Items(baris - 1).SubItems(2).Text
                Form5.ListView7.Items(baris - 1).SubItems(3 + CInt(TextBox72.Text)).Text = ListView2.Items(baris - 1).SubItems(3).Text
            Next
            TextBox72.Text = TextBox72.Text + 3
            If Form5.ListView7.Items(baris - 1).SubItems(0).Text = "" Then
                Form5.ListView7.Items(baris - 1).Remove()
            End If
            For i = 1 To ListView7.Items.Count
                For j = 1 To ListView7.Columns.Count
                    If ListView7.Items(i - 1).SubItems(j - 1).Text = "" Then
                        ListView7.Items(i - 1).Remove()
                    End If
                Next
            Next
            Form9.Close()
        Catch ex As Exception
            Exit Sub
        End Try
        MsgBox("Finish simulation")
    End Sub
    Private Sub triggres(ByVal m As String)
        Try
            TextBox46.Text = TextBox46.Text + 1
            m = CDec(1 / (10 ^ TextBox46.Text))
            m = m.Replace("1", "E0")
            m = m.ToString
            Dim d As Integer
            d = 0
            For g = 1 To ListView2.Items.Count
                If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                    d = d + 1
                    ListView2.Items(g - 1).BackColor = Color.Yellow
                    Chart1.Series(CInt(curve)).Points(g - 1).Label = "min(" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                    Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.Yellow
                End If
            Next g
            Dim c As Integer = 0
            For g = 2 To ListView2.Items.Count
                If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) And Format(CDbl(ListView2.Items(g - 2).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                    c = g
                    Exit For
                End If
            Next
            For i = 1 To d
                If Format(CDbl(ListView2.Items(c - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) And Format(CDbl(ListView2.Items(c - 2).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                    TextBox46.Text = TextBox46.Text + 1
                    m = CDec(1 / (10 ^ TextBox46.Text))
                    m = m.Replace("1", "E0")
                    m = m.ToString
                    For Me.baris = 1 To ListView2.Items.Count
                        ListView2.Items(baris - 1).SubItems(0).BackColor = Color.White
                        ListView2.Items(baris - 1).SubItems(1).BackColor = Color.White
                        ListView2.Items(baris - 1).SubItems(2).BackColor = Color.White
                        ListView2.Items(baris - 1).SubItems(3).BackColor = Color.White
                        TextBox35.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text), m)
                        TextBox34.Text = Format(CDbl(ListView2.Items(0).SubItems(2).Text), m)
                        TextBox45.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text), m)
                        TextBox30.Text = Format(CDbl(ListView2.Items(0).SubItems(1).Text), m)
                        Chart1.Series(CInt(curve)).Points.AddXY(CDec(ListView2.Items(baris - 1).SubItems(1).Text.ToString), CDec(ListView2.Items(baris - 1).SubItems(2).Text))
                        If ComboBox17.Text = "Point" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox17.Text = "Area" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox17.Text = "Fast Line" Then
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart1.Series(CInt(curve)).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        If Format(CDec(TextBox35.Text)) < Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                            TextBox35.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), m.ToString)
                            TextBox45.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), m.ToString)
                            TextBox35.BackColor = Color.LightGreen
                            TextBox45.BackColor = Color.LightGreen
                        Else
                            TextBox35.Text = Format(CDbl(TextBox35.Text), m.ToString)
                            TextBox45.Text = Format(CDbl(TextBox45.Text), m.ToString)
                            TextBox35.BackColor = Color.LightGreen
                            TextBox45.BackColor = Color.LightGreen
                        End If
                        If Format(CDec(TextBox34.Text)) > Format(CDec(ListView2.Items(baris - 1).SubItems(2).Text)) Then
                            TextBox34.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(2).Text), m.ToString)
                            TextBox30.Text = Format(CDbl(ListView2.Items(baris - 1).SubItems(1).Text), m.ToString)
                            TextBox34.BackColor = Color.Yellow
                            TextBox30.BackColor = Color.Yellow
                        Else
                            TextBox34.Text = Format(CDbl(TextBox34.Text), m.ToString)
                            TextBox30.Text = Format(CDbl(TextBox30.Text), m.ToString)
                            TextBox34.BackColor = Color.Yellow
                            TextBox30.BackColor = Color.Yellow
                        End If
                        If ComboBox14.Text = "Dash" Then
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart1.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                    Next
                    Me.Chart1.ChartAreas("ChartArea1").AxisX.Maximum = CDec(ListView2.Items(ListView2.Items.Count - 1).SubItems(1).Text.ToString) + Math.Abs(CDec(ListView2.Items(ListView2.Items.Count - 1).SubItems(1).Text.ToString) - CDec(ListView2.Items(0).SubItems(1).Text.ToString)) / 5
                    TextBox27.Text = (CDec(ListView2.Items(ListView2.Items.Count - 1).SubItems(1).Text.ToString) + Math.Abs(CDec(ListView2.Items(ListView2.Items.Count - 1).SubItems(1).Text.ToString) - CDec(ListView2.Items(0).SubItems(1).Text.ToString)) / 5)
                    Me.Chart1.ChartAreas("ChartArea1").AxisX.Minimum = CDec(ListView2.Items(0).SubItems(1).Text.ToString) - Math.Abs(CDec(TextBox45.Text) - CDec(TextBox30.Text)) / 5
                    TextBox4.Text = (CDec(ListView2.Items(0).SubItems(1).Text.ToString) - Math.Abs(CDec(ListView2.Items(ListView2.Items.Count - 1).SubItems(1).Text.ToString) - CDec(ListView2.Items(0).SubItems(1).Text.ToString)) / 5)
                    Me.Chart1.ChartAreas("ChartArea1").AxisY.Minimum = CDec(TextBox34.Text) - Math.Abs(CDec(TextBox35.Text) - CDec(TextBox34.Text)) / 5
                    TextBox7.Text = (CDec(TextBox34.Text) - Math.Abs(CDec(TextBox35.Text) - CDec(TextBox34.Text)) / 5)
                    Me.Chart1.ChartAreas("ChartArea1").AxisY.Maximum = CDec(TextBox35.Text) + Math.Abs(CDec(TextBox35.Text) - CDec(TextBox34.Text)) / 5
                    TextBox28.Text = (CDec(TextBox35.Text) + Math.Abs(CDec(TextBox35.Text) - CDec(TextBox34.Text)) / 5)
                    Me.Chart1.ChartAreas("ChartArea1").AxisY.LabelStyle.Format = m.ToString
                    Me.Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Format = "N2"
                    For g = 1 To ListView2.Items.Count
                        If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox35.Text), m.ToString) Then
                            ListView2.Items(g - 1).BackColor = Color.LightGreen
                            Chart1.Series(CInt(curve)).Points(g - 1).Label = "max(" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                            Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                            Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                            Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.LightGreen
                        End If
                    Next g
                    d = 0
                    For g = 1 To ListView2.Items.Count
                        If Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString) = Format(CDbl(TextBox34.Text), m.ToString) Then
                            d = d + 1
                            ListView2.Items(g - 1).BackColor = Color.Yellow
                            Chart1.Series(CInt(curve)).Points(g - 1).Label = "min(" & curve + 1 & ") = " + Format(CDbl(ListView2.Items(g - 1).SubItems(2).Text), m.ToString)
                            Chart1.Series(CInt(curve)).Points(g - 1).MarkerStyle = MarkerStyle.Circle
                            Chart1.Series(CInt(curve)).Points(g - 1).MarkerSize = 10
                            Chart1.Series(CInt(curve)).Points(g - 1).MarkerColor = Color.Yellow
                        End If
                    Next g
                Else
                    Exit For
                End If
            Next
            Chart1.Series(CInt(curve)).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes
            Chart1.Series(CInt(curve)).SmartLabelStyle.IsMarkerOverlappingAllowed = True
            Chart1.Series(CInt(curve)).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Right
            TextBox46.Text = TextBox46.Text + 1
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button3_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            Button29.Enabled = True
            ListView5.Items.Clear()
            Button3.Enabled = False
            TextBox72.Text = 0
            ListView7.Items.Clear()
            For i As Integer = 2 To ListView7.Columns.Count
                ListView7.Columns.Clear()
            Next
            ListView7.Columns.Add("No.", 30, HorizontalAlignment.Center)
            Form5.ListView7.Items.Clear()
            ListView2.Items.Clear()
            Button50.PerformClick()
            Chart1.Series(0).Points.Clear()
            Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
            Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
            Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
            Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
            TextBox67.Text = ""
            TextBox66.Text = ""
            TextBox65.Text = ""
            TextBox68.Text = ""
            TextBox47.Text = ""
            TextBox4.Text = ""
            TextBox27.Text = ""
            TextBox28.Text = ""
            TextBox34.Text = ""
            TextBox30.Text = ""
            TextBox45.Text = ""
            TextBox27.Text = ""
            TextBox46.Text = ""
            TextBox35.Text = ""
            TextBox7.Text = ""
            legend = ""
            For i = 0 To 9
                Me.Chart1.Series(i).Points.Clear()
                Me.Chart1.Series(i).Name = "Curve " & i + 1
            Next i
            Button29.PerformClick()
        Catch ex As Exception
            Exit Sub
        End Try
        Button3.Enabled = True
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            Form4.Show()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub BtnScanPort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnScanPort.Click
        Try
            CmbScanPort.Items.Clear()
            Dim myPort As Array
            Dim i As Integer
            myPort = IO.Ports.SerialPort.GetPortNames
            CmbScanPort.Items.AddRange(myPort)
            i = CmbScanPort.Items.Count
            i = i - i
            Try
                CmbScanPort.SelectedIndex = i
            Catch ex As Exception
                Dim result As DialogResult
                result = MessageBox.Show("Com Port not detected", "Warning!!!", MessageBoxButtons.OK)
                CmbScanPort.Text = ""
                CmbScanPort.Items.Clear()
                Call Form1_Load(Me, e)
            End Try
            Button28.Enabled = True
            CmbScanPort.DroppedDown = True
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub Button28_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button28.Click
        'Try
        If CmbScanPort.Text = "" Then
            MsgBox("You have to choose Port before measuring!")
            Exit Sub
        End If
        If CmbBaud.Text = "" Then
            MsgBox("You have to choose Baud rate before measuring!")
            Exit Sub
        End If

        'If ComboBox2.Text = "Gate Time" Then
        'Timer1.Interval = Val(TextBox60.Text) * 1000
        ' Timer1.Start()
        ' Else
        If ComboBox3.Text = "Monitoring" Then
            Timer7.Interval = TextBox60.Text * 1000
            Timer7.Start()
        Else
            Timer1.Interval = 1000
            Timer1.Start()
        End If
        


        'MsgBox("ok")
        ' End If
        'Catch ex As Exception
        'Exit Sub
        'End Try
    End Sub
    Dim DisplaySeriesTrendLine As Boolean = False
    Private Const MAX_RECURSIVE_CALLS As Integer = 1000000000
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' Try

        Dim i As Integer = ListView1.Items.Count + 1
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            SerialPort1.Open()
            SerialPort1.ReadLine.ToString()
            ' Button14.PerformClick()
            ' wait(1000)
            Dim ka = CStr(SerialPort1.ReadLine.ToString)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)

            'wait(1)

            'Denaturation prepare
            If IsNumeric(ka) = True Then
                Label118.Text = ka
                ListView1.Items.Add(ListView1.Items.Count + 1)
                ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                ListView1.Items(i - 1).SubItems.Add(ka)
                

                If i <= 1 And ComboBox3.Text = "Run All Process" Then 'memberikan label proses pertama denaturasi
                    ListView1.Items(i - 1).SubItems.Add("Process to Denatruration")
                    Button14.PerformClick() 'heater on
                    ComboBox3.Text = "Process to Denaturation"
                    ComboBox3.BackColor = Color.White
                ElseIf i <= 1 And ComboBox3.Text = "Denaturation" Then 'memberikan label proses pertama denaturasi
                    ListView1.Items(i - 1).SubItems.Add("Process to Denatruration")
                    Button14.PerformClick() 'heater on
                    ComboBox3.Text = "Process to Denaturation"
                    ComboBox3.BackColor = Color.White
                    'Denaturation start
                ElseIf ka >= TextBox48.Text And ka <= TextBox59.Text And i >= 2 And ListView1.Items(i - 2).SubItems(3).Text = "Process to Denaturation" Then
                    ComboBox3.Text = "Denaturation"
                    Button5.PerformClick()
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.Red
                    ListView1.Items(i - 1).BackColor = Color.Red
                ElseIf Me.ComboBox3.Text = "Denaturaion" Then
                    Timer1.Stop()
                    wait(1000)
                    MsgBox("fan on")
                    Button5.PerformClick()
                    Timer1.Start()


                    'steady


                    'Annealing start
                ElseIf ka >= TextBox58.Text And ka <= TextBox62.Text And ListView1.Items(i - 2).SubItems(3).Text = "Denaturation" Then
                    ComboBox3.Text = "Annealing"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.Orange
                    ListView1.Items(i - 1).BackColor = Color.Red
                    If ComboBox3.Text = "Annealing" Then
                        Button14.PerformClick()
                    End If
                    'steady
                    ' Button35.PerformClick()
                    'wait(60000)
                    'heating
                    Button14.PerformClick()
                    'Extension start
                ElseIf ka >= TextBox63.Text And ka <= TextBox73.Text And ListView1.Items(i - 2).SubItems(3).Text = "Annealing" Then
                    ComboBox3.Text = "Extension"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.Yellow
                    ListView1.Items(i - 1).BackColor = Color.Yellow
                    If ComboBox3.Text = "Extension" Then
                        Button14.PerformClick()
                    End If
                    'steady
                    ' Button35.PerformClick()
                    ' wait(60000)
                    Button5.PerformClick()

                    'finishing
                ElseIf ka >= TextBox74.Text And ka <= TextBox75.Text And ListView1.Items(i - 2).SubItems(3).Text = "Extension" Then
                    ComboBox3.Text = "Finishing"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.YellowGreen
                    ListView1.Items(i - 1).BackColor = Color.Red
                    If ComboBox3.Text = "Finishing" Then
                        Button5.PerformClick()
                        ' wait(10000)
                        Timer1.Stop()
                        Button53.PerformClick()
                    End If

                    'steady
                    ' Button35.PerformClick()

                    ' SerialPort1.Close()
                    ' Button26.PerformClick()
                    Exit Sub
                Else
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                End If
                Button87.PerformClick()

                'push button





                'Graphic setting
                If ComboBox4.Text = "Point" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                ElseIf ComboBox4.Text = "Area" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                ElseIf ComboBox4.Text = "Fast Line" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Else
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                End If
                Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                ListView1.Items(i - 1).EnsureVisible()
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                If ComboBox5.Text = "Red" Then
                    Chart2.Series(0).Color = Color.Red
                ElseIf ComboBox5.Text = "Green" Then
                    Chart2.Series(0).Color = Color.Green
                ElseIf ComboBox5.Text = "Blue" Then
                    Chart2.Series(0).Color = Color.Blue
                Else
                    Chart2.Series(0).Color = Color.Brown
                End If
                If ComboBox6.Text = "Dash" Then
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    End With
                Else
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    End With
                End If
                Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
            End If
        End If
        ' Dim X_N2 As String = TextBox3.Text
        ' Dim A As String = TextBox14.Text
        ' Dim F As String = TextBox2.Text
        ' Dim R As String = TextBox1.Text
        ' Dim v As String = TextBox95.Text

        'Catch s As Exception
        ' MsgBox("Data stopped!")
        ' Exit Sub
        ' End Try
        ' MsgBox(ka)

        ' wait(1000)
        ' End If

    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Try
            Button26.PerformClick()
            TextBox47.Text = ""
            legend = ""
            SerialPort1.Close()
            TextBox50.Text = 0
            TextBox51.Text = 0
            TextBox52.Text = 0
            TextBox53.Text = 0
            TextBox54.Text = ""
            TextBox55.Text = ""
            TextBox56.Text = 0
            TextBox57.Text = 0
            ProgressBar1.Value = 0
            Label106.Text = "Standby"
            Me.Chart2.Series(0).Points.Clear()
            ListView1.Items.Clear()
            'ListView3.Items.Clear()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
        Try
            GroupBox17.Visible = True
            GroupBox17.Enabled = True
            SerialPort1.Close()
            Timer1.Stop()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button27_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button27.Click
        Try
            Dim SaveFile As New SaveFileDialog
            SaveFile.FileName = ""
            SaveFile.Filter = "Text Files (*.txt)|*.txt"
            SaveFile.Title = "Save"
            SaveFile.ShowDialog()
            Dim Write As New System.IO.StreamWriter(SaveFile.FileName)
            Dim col As ColumnHeader
            Dim columnnames As String = ""
            For Each col In ListView1.Columns
                If String.IsNullOrEmpty(columnnames) Then
                    columnnames = col.Text
                Else
                    columnnames &= "|" & col.Text
                End If
            Next
            Write.Write(columnnames & vbCrLf)
            For Me.baris = 1 To ListView1.Items.Count - 1
                Write.Write(ListView1.Items(baris - 1).SubItems(0).Text & "|" & ListView1.Items(baris - 1).SubItems(1).Text & "|" & ListView1.Items(baris - 1).SubItems(2).Text & "|" & ListView1.Items(baris - 1).SubItems(3).Text & vbCrLf)
            Next baris
            Write.Close()
        Catch d As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button25_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button25.Click
        Try
            Chart2.ChartAreas(0).AxisY.ScaleView.Size = Math.Abs(CDec(TextBox55.Text) - CDec(TextBox54.Text)) * 2
            Chart2.ChartAreas(0).AxisX.ScaleView.Size = Math.Abs(CDec(ListView1.Items(ListView1.Items.Count).SubItems(2).Text.ToString) - CDec(ListView1.Items(0).SubItems(2).Text.ToString)) * 2
            Chart2.ChartAreas(0).AxisX.ScrollBar.Enabled = True
            Chart2.ChartAreas(0).AxisY.ScrollBar.Enabled = True
            Chart2.ChartAreas(0).CursorX.IsUserEnabled = True
            Chart2.ChartAreas(0).CursorY.IsUserEnabled = True
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Try
            TextBox49.Text = ComboBox3.Text
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button24_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button24.Click
        Try
            Chart2.ChartAreas(0).AxisY.ScaleView.Size = Math.Abs(CDec(TextBox55.Text) - CDec(TextBox54.Text)) / 2
            Chart2.ChartAreas(0).AxisX.ScaleView.Size = Math.Abs(CDec(ListView1.Items(ListView1.Items.Count).SubItems(2).Text.ToString) - CDec(ListView1.Items(0).SubItems(2).Text.ToString)) / 2
            Chart2.ChartAreas(0).AxisX.ScrollBar.Enabled = True
            Chart2.ChartAreas(0).AxisY.ScrollBar.Enabled = True
            Chart2.ChartAreas(0).CursorX.IsUserEnabled = True
            Chart2.ChartAreas(0).CursorY.IsUserEnabled = True
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button23.Click
        Try
            With Chart2.ChartAreas(0)
                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button22_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button22.Click
        Try
            With Chart2.ChartAreas(0)
                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button20.Click
        Dim i As Integer
        Try
            Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
            If TextBox55.Text = "" Then
                TextBox55.Text = ListView1.Items(0).SubItems(2).Text
            Else
                If CDec(ListView1.Items(i - 1).SubItems(2).Text.ToString) > CDec(TextBox55.Text) And Not ListView1.Items(i - 1).SubItems(2).Text.ToString = "" And Not TextBox55.Text = "" Then
                    TextBox55.Text = Format(CDec(ListView1.Items(i - 1).SubItems(2).Text.ToString), "00.00")
                    ListView1.Items(i - 1).BackColor = Color.Yellow
                    Chart2.ChartAreas("ChartArea1").AxisY.Maximum = CDec(TextBox55.Text) + (TextBox57.Text / 8)
                    Chart2.Series(0).Points(i - 1).Label = "max(" & Label100.Text + 1 & ")= " + CDec(ListView1.Items(i - 1).SubItems(2).Text)
                    Chart2.Series(0).Points(i - 1).MarkerStyle = MarkerStyle.Circle
                    Chart2.Series(0).Points(i - 1).MarkerSize = 10
                    Chart2.Series(0).Points(i - 1).MarkerColor = Color.Yellow
                    Label100.Text = Val(Label100.Text) + 1
                Else
                    TextBox55.Text = CDec(TextBox55.Text)
                    Chart2.ChartAreas("ChartArea1").AxisY.Maximum = CDec(TextBox55.Text) + CDec(TextBox57.Text / 8)
                End If
            End If
            If TextBox54.Text = "" Then
                TextBox54.Text = ListView1.Items(0).SubItems(2).Text
            Else
                If CDec(ListView1.Items(i - 1).SubItems(2).Text.ToString) < CDec(TextBox54.Text) Then
                    TextBox54.Text = Format(CDec(ListView1.Items(i - 1).SubItems(2).Text.ToString), "00.00")
                    Chart2.ChartAreas("ChartArea1").AxisY.Minimum = CDec(TextBox54.Text) - (TextBox57.Text / 8)
                    ListView1.Items(i - 1).BackColor = Color.LightGreen
                    Chart2.Series(0).Points(i - 1).Label = "min(" & Label101.Text + 1 & ")= " + (ListView1.Items(i - 1).SubItems(2).Text)
                    Chart2.Series(0).Points(i - 1).MarkerStyle = MarkerStyle.Circle
                    Chart2.Series(0).Points(i - 1).MarkerSize = 10
                    Chart2.Series(0).Points(i - 1).MarkerColor = Color.LightGreen
                    Label101.Text = Val(Label101.Text) + 1
                Else
                    TextBox54.Text = CDec(TextBox54.Text)
                    Chart2.ChartAreas("ChartArea1").AxisY.Minimum = CDec(TextBox54.Text) - (TextBox57.Text / 8)
                End If
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button17.Click
        Try
            With Chart2.ChartAreas(0)
                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.NotSet
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Try
            With Chart2.ChartAreas(0)
                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.NotSet
                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.NotSet
            End With
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        Try
            For g = 1 To ListView1.Items.Count
                Chart2.Series(CInt(curve)).Points(g - 1).Label = ""
                Chart2.Series(CInt(curve)).Points(g - 1).MarkerSize = 0
            Next g
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Try
            Dim saveFileDialog1 As New SaveFileDialog()
            saveFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF (*.emf)|*.emf|PNG (*.png)|*.png|SVG (*.svg)|*.svg|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif"
            saveFileDialog1.FilterIndex = 2
            saveFileDialog1.RestoreDirectory = True
            If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                Dim format As ChartImageFormat = ChartImageFormat.Bmp
                If saveFileDialog1.FileName.EndsWith("bmp") Then
                    format = ChartImageFormat.Bmp
                Else
                    If saveFileDialog1.FileName.EndsWith("jpg") Then
                        format = ChartImageFormat.Jpeg
                    Else
                        If saveFileDialog1.FileName.EndsWith("emf") Then
                            format = ChartImageFormat.Emf
                        Else
                            If saveFileDialog1.FileName.EndsWith("gif") Then
                                format = ChartImageFormat.Gif
                            Else
                                If saveFileDialog1.FileName.EndsWith("png") Then
                                    format = ChartImageFormat.Png
                                Else
                                    If saveFileDialog1.FileName.EndsWith("tif") Then
                                        format = ChartImageFormat.Tiff
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Chart2.SaveImage(saveFileDialog1.FileName, format)
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Try
            Chart2.Series(0).Points.Clear()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button30.Click
        Try
            TabControl1.SelectedIndex = 2
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button39.Click
        Try
            TabControl1.SelectedIndex = 1
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button40.Click
        Try
            TabControl1.SelectedIndex = 2
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button42_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button42.Click
        Try
            TabControl1.SelectedIndex = 3
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub TextBox9_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox9.TextChanged
        Try
            TextBox9.Height =
             TextRenderer.MeasureText(
                 TextBox9.Text,
                 TextBox9.Font,
                 New Size(TextBox9.ClientSize.Width, 1000),
                 TextFormatFlags.WordBreak
             ).Height
            TextBox9.AutoSize = True
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub ComboBox8_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox8.SelectedIndexChanged
        Try
            ComboBox9.Items.Clear()
            ComboBox9.Items.Add("Effective Area")
            ComboBox9.Items.Add("% N2 in Effluent")
            ComboBox9.Items.Add("% O2 in Effluent")
            ComboBox9.Items.Add("P_O2 Feed Surface Membrane")
            ComboBox9.Items.Add("Membrane Characteristic Thickness")
            ComboBox9.Items.Add("Temperature")
            ComboBox9.Items.Add("Ionic Conductivity")
            ComboBox9.Items.Add("Thickness")
            ComboBox9.Items.Add("Total O2 Chem Potential Diff")
            ComboBox9.Items.Add("Electronic Conductivity")
            ComboBox9.Items.Add("Oxygen Debit(mol/s)")
            ComboBox9.Items.Add("Farad Contant")
            ComboBox9.Items.Add("Gas constant")
            ComboBox9.Items.Add("P_O2 PSM")
            ComboBox9.Items.Add("CTE efc")
            If ComboBox8.Text = "Multi Curve" And Form5.ListView3.Items.Count < 1 Then
                Form7.Show()
            ElseIf ComboBox8.Text = "Multi Curve" And ListView2.Items.Count < 1 Then
                Form7.Show()
            ElseIf ComboBox8.Text = "Multi Curve" And ListView2.Items.Count > 1 Then
                Form9.Show()
                Exit Sub
            End If
            If ComboBox8.SelectedItem = "Single Curve" And ListView7.Items.Count > 1 Then
                Button29.Enabled = True
                Button29.BackColor = Color.LightGray
                Button29.ForeColor = Color.Black
                Button3.Enabled = True
                Button3.BackColor = Color.LightGray
                Button3.ForeColor = Color.Black
                Form9.Show()
                Label66.ForeColor = Color.Transparent
                Label66.BackColor = Color.Transparent
                ListView5.BackColor = Color.Black
                ComboBox9.Enabled = False
                ComboBox10.Enabled = False
                ComboBox11.Enabled = False
                ComboBox15.Enabled = False
                TextBox69.Text = "Disabled"
                TextBox70.Text = "Disabled"
                TextBox71.Text = "Disabled"
                TextBox69.Enabled = False
                TextBox70.Enabled = False
                TextBox71.Enabled = False
                Label50.Enabled = False
                Label51.Enabled = False
                Label52.Enabled = False
                Label13.Enabled = False
                Button57.Enabled = False
                Button51.Enabled = False
                Button52.Enabled = False
                ListView5.Enabled = False
                Button46.Enabled = False
                Button47.Enabled = False
                Exit Sub
            Else
                Button29.Enabled = True
                Button29.BackColor = Color.LightGray
                Button29.ForeColor = Color.Black
                Button3.Enabled = True
                Button3.BackColor = Color.LightGray
                Button3.ForeColor = Color.Black
                Label66.ForeColor = Color.Transparent
                Label66.BackColor = Color.Transparent
                ListView5.BackColor = Color.Black
                ComboBox9.Enabled = False
                ComboBox10.Enabled = False
                ComboBox11.Enabled = False
                ComboBox15.Enabled = False
                TextBox69.Text = "Disabled"
                TextBox70.Text = "Disabled"
                TextBox71.Text = "Disabled"
                TextBox69.Enabled = False
                TextBox70.Enabled = False
                TextBox71.Enabled = False
                Label50.Enabled = False
                Label51.Enabled = False
                Label52.Enabled = False
                Label13.Enabled = False
                Button57.Enabled = False
                Button51.Enabled = False
                Button52.Enabled = False
                ListView5.Enabled = False
                Button46.Enabled = False
                Button47.Enabled = False
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button48.Click
        Try
            If ComboBox8.Text = "Single Curve" Then
                Form5.Show()
            Else
                Form5.Show()
                Form7.Hide()
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button50_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button50.Click
        Try
            TextBox72.Text = 0
            ListView4.Items.Clear()
            ListView7.Items.Clear()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button43_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button43.Click
        Try
            Dim SaveFile As New SaveFileDialog
            SaveFile.FileName = ""
            SaveFile.Filter = "Text Files (*.txt)|*.txt"
            SaveFile.Title = "Save"
            SaveFile.ShowDialog()
            Dim Write As New System.IO.StreamWriter(SaveFile.FileName)
            Dim col As ColumnHeader
            Dim columnnames As String = ""
            For Each col In ListView4.Columns
                If String.IsNullOrEmpty(columnnames) Then
                    columnnames = col.Text
                Else
                    columnnames &= "|" & col.Text
                End If
            Next
            Write.Write(columnnames & vbCrLf)
            For Me.baris = 1 To ListView4.Items.Count - 1
                Write.Write(ListView4.Items(baris - 1).SubItems(0).Text & "|" & ListView4.Items(baris - 1).SubItems(1).Text & "|" & ListView4.Items(baris - 1).SubItems(2).Text & "|" & ListView4.Items(baris - 1).SubItems(3).Text & "|" & ListView4.Items(baris - 1).SubItems(4).Text & "|" & ListView4.Items(baris - 1).SubItems(5).Text & "|" & ListView4.Items(baris - 1).SubItems(6).Text & "|" & ListView4.Items(baris - 1).SubItems(7).Text & "|" & ListView4.Items(baris - 1).SubItems(8).Text & vbCrLf)
            Next baris
            Write.Close()
        Catch v As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click
        Try
            ListView2.Items.Clear()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisY.Maximum = Chart1.ChartAreas("ChartArea1").AxisY.Maximum + (CDec(Chart1.ChartAreas("ChartArea1").AxisY.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisY.Minimum)) / 10
            TextBox67.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisY.Maximum = Chart1.ChartAreas("ChartArea1").AxisY.Maximum - (CDec(Chart1.ChartAreas("ChartArea1").AxisY.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisY.Minimum)) / 10
            TextBox67.Text = Chart1.ChartAreas("ChartArea1").AxisY.Maximum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button18.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisY.Minimum = Chart1.ChartAreas("ChartArea1").AxisY.Minimum + (CDec(Chart1.ChartAreas("ChartArea1").AxisY.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisY.Minimum)) / 10
            TextBox68.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
        Catch ex As Exception
        End Try
    End Sub
    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisY.Minimum = Chart1.ChartAreas("ChartArea1").AxisY.Minimum - (CDec(Chart1.ChartAreas("ChartArea1").AxisY.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisY.Minimum)) / 10
            TextBox68.Text = Chart1.ChartAreas("ChartArea1").AxisY.Minimum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button37.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisX.Maximum = Chart1.ChartAreas("ChartArea1").AxisX.Maximum + (CDec(Chart1.ChartAreas("ChartArea1").AxisX.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisX.Minimum)) / 10
            TextBox66.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button34.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisX.Maximum = Chart1.ChartAreas("ChartArea1").AxisX.Maximum - (CDec(Chart1.ChartAreas("ChartArea1").AxisX.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisX.Minimum)) / 10
            TextBox66.Text = Chart1.ChartAreas("ChartArea1").AxisX.Maximum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button44_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button44.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisX.Minimum = Chart1.ChartAreas("ChartArea1").AxisX.Minimum + (CDec(Chart1.ChartAreas("ChartArea1").AxisX.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisX.Minimum)) / 10
            TextBox65.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button38.Click
        Try
            Chart1.ChartAreas("ChartArea1").AxisX.Minimum = Chart1.ChartAreas("ChartArea1").AxisX.Minimum - (CDec(Chart1.ChartAreas("ChartArea1").AxisX.Maximum) - CDec(Chart1.ChartAreas("ChartArea1").AxisX.Minimum)) / 10
            TextBox65.Text = Chart1.ChartAreas("ChartArea1").AxisX.Minimum
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button46_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button46.Click
        Try
            If ComboBox9.Text = "Choose Variable" Then
                MsgBox("Choose Variable first to make configuration on multi curve running")
                ComboBox9.BackColor = Color.Yellow
                Exit Sub
            End If
            If ListView5.Items.Count = 10 Then
                MsgBox("Database configuration full! Please clear table first")
                Exit Sub
                Button46.Enabled = False
            End If
            If ComboBox9.Text = "" Then
                MsgBox("Choose variable first!")
                Exit Sub
            ElseIf TextBox70.Text = "" Then
                MsgBox("Arrange interval!")
                Exit Sub
            ElseIf TextBox71.Text = "" Then
                MsgBox("Determine final value!")
                Exit Sub
            ElseIf TextBox69.Text = "" Then
                MsgBox("Determine initial value!")
                Exit Sub
            Else
                If ComboBox15.Text = "Patterned" Then
                    TextBox71.Enabled = True
                    TextBox70.Enabled = True
                    Label51.Enabled = True
                    Label52.Enabled = True
                    Label50.Text = "Init Value"
                    For i = 1 To CInt(Val(TextBox71.Text - TextBox69.Text) / Val(TextBox70.Text)) + 1
                        ComboBox11.Text = ComboBox11.Text + 1
                        l = Me.ListView5.Items.Add("Curve " & ComboBox11.Text - 1)
                        l.SubItems.Add(ComboBox9.Text)
                        l.SubItems.Add(ComboBox11.Text - 1)
                        l.SubItems.Add(Val(TextBox69.Text) + (i - 1) * Val(TextBox70.Text))
                        ComboBox10.Text = "Curve " & ComboBox11.Text
                    Next
                Else
                    ComboBox11.Text = ComboBox11.Text + 1
                    l = Me.ListView5.Items.Add("Curve " & ComboBox11.Text - 1)
                    l.SubItems.Add(ComboBox9.Text)
                    l.SubItems.Add(ComboBox11.Text - 1)
                    l.SubItems.Add(TextBox69.Text)
                    ComboBox10.Text = "Curve " & ComboBox11.Text
                End If
                If ListView5.Items.Count = 10 Then
                    ComboBox10.Text = "Curve 10"
                End If
                If ComboBox11.Text > 10 Then
                    ComboBox11.Text = 10
                    MsgBox("You reach Maximum index. The maximum index is 10")
                End If
            End If
            ComboBox9.BackColor = Color.White
            ComboBox11.Text = 1
            ListView5.Enabled = True
            ListView5.BackColor = Color.White
            Button68.Enabled = True
            Button66.Enabled = True
            Button51.Enabled = True
            Button57.Enabled = True
            Button52.Enabled = True
            Button51.BackColor = Color.Yellow
            Button29.Visible = True
            Button3.Visible = True
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button57_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button57.Click
        Try
            Button46.Enabled = True
            ListView5.Items.Clear()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button52_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button52.Click
        Try
            Button46.Enabled = True
            ListView5.FullRowSelect = True
            For Each item As ListViewItem In ListView5.SelectedItems
                item.Remove()
            Next
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button49_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button49.Click
        Try
            ListView4.FullRowSelect = True
            For Each item As ListViewItem In ListView4.SelectedItems
                item.Remove()
            Next
            ListView7.FullRowSelect = True
            For Each item As ListViewItem In ListView7.SelectedItems
                item.Remove()
            Next
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button51_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button51.Click
        Form10.Show()
        curve = 0
    End Sub
    Private Sub Button47_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button47.Click
        Try
            ComboBox11.Text = "1"
            TextBox70.Text = ""
            TextBox69.Text = ""
            TextBox71.Text = ""
            ComboBox10.Text = "Curve 1"
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub ComboBox15_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox15.SelectedIndexChanged
        Try
            If ComboBox15.Text = "Specific Value" Then
                TextBox71.Enabled = False
                TextBox71.Text = "Disabled"
                TextBox70.Text = "Disabled"
                TextBox70.Enabled = False
                Label51.Enabled = False
                Label52.Enabled = False
                Label50.Text = "Spec Value"
            Else
                TextBox69.Enabled = True
                TextBox71.Enabled = True
                TextBox71.Text = ""
                TextBox70.Text = ""
                TextBox70.Enabled = True
                Label51.Enabled = True
                Label52.Enabled = True
                Label50.Text = "Init Value"
                ComboBox11.Enabled = True
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button45_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button45.Click
        Try
            Form6.Show()
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button58_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button58.Click
        Try
            k = TextBox72.Text
            For baris As Integer = 1 To ListView2.Items.Count
                ListView7.Items(baris - 1).SubItems(1 + k).Text = ListView2.Items(baris - 1).SubItems(1).Text
                ListView7.Items(baris - 1).SubItems(2 + k).Text = ListView2.Items(baris - 1).SubItems(2).Text
                ListView7.Items(baris - 1).SubItems(3 + k).Text = ListView2.Items(baris - 1).SubItems(3).Text
            Next
            TextBox72.Text = Val(TextBox72.Text) + 3
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button60_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button60.Click
        Try
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Rotation = Chart1.ChartAreas("ChartArea1").Area3DStyle.Rotation + 10
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button59_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button59.Click
        Try
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Rotation = Chart1.ChartAreas("ChartArea1").Area3DStyle.Rotation - 10
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button61_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button61.Click
        Try
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button62_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button62.Click
        Try
            Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = False
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button63.Click
        Try
            Chart1.ChartAreas("ChartArea1").Area3DStyle.PointDepth = CInt(Chart1.ChartAreas("ChartArea1").Area3DStyle.PointDepth) / 2
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button64_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button64.Click
        Try
            Chart1.ChartAreas("ChartArea1").Area3DStyle.PointDepth = CInt(Chart1.ChartAreas("ChartArea1").Area3DStyle.PointDepth) * 2
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button65_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button65.Click
        Try
            If ListView2.Items.Count < 1 Then
                MsgBox("try to run first")
                Exit Sub
            Else
                Form8.Show()
            End If
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button66_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button66.Click
        Try
            Dim SaveFile As New SaveFileDialog
            SaveFile.FileName = ""
            SaveFile.Filter = "Text Files (*.txt)|*.txt"
            SaveFile.Title = "Save"
            SaveFile.ShowDialog()
            Dim Write As New System.IO.StreamWriter(SaveFile.FileName)
            Dim col As ColumnHeader
            Dim columnnames As String = ""
            For Each col In ListView5.Columns
                If String.IsNullOrEmpty(columnnames) Then
                    columnnames = col.Text
                Else
                    columnnames &= "|" & col.Text
                End If
            Next
            For Me.baris = 1 To ListView5.Items.Count
                Write.Write(ListView5.Items(baris - 1).SubItems(0).Text & "|" & ListView5.Items(baris - 1).SubItems(1).Text & "|" & CInt(ListView5.Items(baris - 1).SubItems(2).Text) & "|" & CDec(ListView5.Items(baris - 1).SubItems(3).Text) & vbCrLf)
            Next baris
            Write.Close()
        Catch p As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button67_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button67.Click
        Try
            Dim SaveFile As New SaveFileDialog
            SaveFile.FileName = ""
            SaveFile.Filter = "Text Files (*.txt)|*.txt"
            SaveFile.Title = "Save"
            SaveFile.ShowDialog()
            Dim Write As New System.IO.StreamWriter(SaveFile.FileName)
            Dim col As ColumnHeader
            Dim columnnames As String = ""
            For Each col In ListView7.Columns
                If String.IsNullOrEmpty(columnnames) Then
                    columnnames = col.Text
                Else
                    columnnames &= "|" & col.Text
                End If
            Next
            Write.Write(columnnames & vbCrLf)
            For Me.baris = 1 To ListView7.Items.Count
                For j = 1 To ListView7.Columns.Count
                    Write.Write(ListView7.Items(baris - 1).SubItems(j - 1).Text & "|")
                Next j
                Write.Write(vbCrLf)
            Next baris
            Write.Close()
        Catch p As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button68_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button68.Click
        Try
            Dim OpenFile As New OpenFileDialog
            OpenFile.ShowDialog()
            OpenFile.Filter = "Text Files (*.txt)|*.txt"
            OpenFile.Title = "Open"
            Dim filepath As String = OpenFile.FileName
            Dim inputstream As New IO.StreamReader(filepath)
            Dim newstr(3) As String
            Do While inputstream.Peek <> -1
                newstr = inputstream.ReadLine().Split("|")
                l = ListView5.Items.Add(CStr(newstr(0)))
                l.SubItems.Add(CStr(newstr(1)))
                l.SubItems.Add(CInt(newstr(2)))
                l.SubItems.Add(CDec(newstr(3)))
            Loop
            inputstream.Close()
            MsgBox("Re-Input Configuration finish!")
        Catch es As Exception
            MsgBox("Re-Input Configuration fail!, Check your data that you open")
        End Try
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox10.Text = "Curve 11" Then
            ComboBox10.Text = "Curve 10"
            ComboBox1.Text = ComboBox10.Text
        End If
    End Sub
    Private Sub ComboBox9_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox9.SelectedIndexChanged
        ComboBox9.BackColor = Color.White
    End Sub


    Private Sub ProgressBar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProgressBar1.Click

    End Sub

    Private Sub TextBox55_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox55.TextChanged
        TextBox55.Text = Format(CDec(TextBox55.Text), "0.00000")
    End Sub

    Private Sub TextBox54_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox54.TextChanged
        TextBox54.Text = Format(CDec(TextBox54.Text), "0.00000")
    End Sub

    Private Sub Chart2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub CmbScanPort_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbScanPort.SelectedIndexChanged

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        SerialPort1.Close()
        SerialPort1.BaudRate = CmbBaud.SelectedItem
        SerialPort1.PortName = CmbScanPort.SelectedItem
        SerialPort1.Open()
        SerialPort1.Write("1") 'MenghidupSerialPort1.ReadLine.ToStringn fan
        SerialPort1.Write("2") 'MematiSerialPort1.ReadLine.ToStringn heater
        SerialPort1.Close()
        ' Timer1.Start()
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        SerialPort1.Close()
        SerialPort1.BaudRate = CmbBaud.SelectedItem
        SerialPort1.PortName = CmbScanPort.SelectedItem
        SerialPort1.Open()
        SerialPort1.Write("0") 'MematiSerialPort1.ReadLine.ToStringn fan
        SerialPort1.Write("3") 'MenghidupSerialPort1.ReadLine.ToStringn heater
        SerialPort1.Close()
    End Sub

    Private Sub Button35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button35.Click
        SerialPort1.Close()
        SerialPort1.BaudRate = CmbBaud.SelectedItem
        SerialPort1.PortName = CmbScanPort.SelectedItem
        SerialPort1.Open()
        SerialPort1.Write("1") 'MenghidupSerialPort1.ReadLine.ToStringn fan
        SerialPort1.Write("3") 'MenghidupSerialPort1.ReadLine.ToStringn heater
        SerialPort1.Close()
    End Sub

    Private Sub Button53_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button53.Click
        SerialPort1.Close()
        SerialPort1.BaudRate = CmbBaud.SelectedItem
        SerialPort1.PortName = CmbScanPort.SelectedItem
        SerialPort1.Open()
        SerialPort1.Write("0") 'MematiSerialPort1.ReadLine.ToStringn fan
        SerialPort1.Write("2") 'MematiSerialPort1.ReadLine.ToStringn heater
        SerialPort1.Close()
        ' Timer1.Stop()
    End Sub

    Private Sub TextBox74_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox74.TextChanged

    End Sub

    Private Sub TabPage3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabPage3.Click

    End Sub

    Private Sub Button54_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button54.Click
        ' Try
        Dim i As Integer
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            ' Button14.PerformClick()
            ' wait(1000)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)
            Dim ka As String
            'wait(1)

            'Denaturation prepare
            For i = 1 To 10000000
                SerialPort1.Close()
                SerialPort1.Open()
                SerialPort1.ReadLine.ToString()
                ka = CStr(SerialPort1.ReadLine.ToString)
                If IsNumeric(ka) = True Then
                    wait(TextBox60.Text)
                    Label118.Text = ka
                    ListView1.Items.Add(ListView1.Items.Count + 1)
                    ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                    ListView1.Items(i - 1).SubItems.Add(ka)
                    If i <= 1 And ComboBox3.Text = "Run All Process" And ka < TextBox48.Text Then 'memberikan label proses pertama denaturasi
                        ListView1.Items(i - 1).SubItems.Add("Denatruration")
                        Button14.PerformClick() 'heater on
                        ComboBox3.Text = "Denaturation"
                        ComboBox3.BackColor = Color.Red
                        ComboBox3.ForeColor = Color.White
                    ElseIf i <= 1 And ComboBox3.Text = "Denaturation" Then 'memberikan label proses pertama denaturasi
                        ListView1.Items(i - 1).SubItems.Add("Denatruration")
                        Button14.PerformClick() 'heater on
                        ComboBox3.Text = "Denaturation"
                        ComboBox3.BackColor = Color.Red
                        ComboBox3.ForeColor = Color.White
                        'Denaturation start
                    ElseIf ka >= CStr(TextBox48.Text) And ka <= CStr(TextBox59.Text) Then
                        ComboBox3.Text = "S-Denaturation"
                        ' MsgBox("Steady Process for Denaturation Start")
                        'i = 10000000
                        Button55.PerformClick()
                        ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                        ComboBox3.BackColor = Color.Red
                        ListView1.Items(i - 1).BackColor = Color.Red
                        ComboBox3.ForeColor = Color.White

                    ElseIf ka > TextBox59.Text And ComboBox3.Text = "Run All Process" Then
                        Button5.PerformClick()
                        MsgBox("PCR Machine is not read. PCR Machine try to match the tempreature")
                        Exit Sub
                    End If
                    'Graphic setting
                    If ComboBox4.Text = "Point" Then
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                    ElseIf ComboBox4.Text = "Area" Then
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                    ElseIf ComboBox4.Text = "Fast Line" Then
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    Else
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                    End If
                    Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                    Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                    Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                    Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                    Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                    'ListView1.Items(i - 1).EnsureVisible()
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                    If ComboBox5.Text = "Red" Then
                        Chart2.Series(0).Color = Color.Red
                    ElseIf ComboBox5.Text = "Green" Then
                        Chart2.Series(0).Color = Color.Green
                    ElseIf ComboBox5.Text = "Blue" Then
                        Chart2.Series(0).Color = Color.Blue
                    Else
                        Chart2.Series(0).Color = Color.Brown
                    End If
                    If ComboBox6.Text = "Dash" Then
                        With Chart2.ChartAreas(0)
                            .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        End With
                    Else
                        With Chart2.ChartAreas(0)
                            .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        End With
                    End If
                    Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                    Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                End If
            Next
        End If
        'Catch es As Exception
        'TextBox53.Text = TextBox53.Text + 1
        'MsgBox("Error in Denaturation Process")
        'Exit Sub
        'End Try

    End Sub
    Private Sub Button55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button55.Click
        '  Try
        Dim i As Integer
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            ' Button14.PerformClick()
            ' wait(1000)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)
            Dim ka As String
            'wait(1)
            Button55.BackColor = Color.Green
            'Denaturation prepare
            For i = 1 To 60 'CInt(CStr(TextBox52.Text) / CStr(TextBox60.Text))
                SerialPort1.Close()
                SerialPort1.Open()
                SerialPort1.ReadLine.ToString()
                ka = CStr(SerialPort1.ReadLine.ToString)
                If IsNumeric(ka) = True Then
                    wait(TextBox60.Text)
                    Label118.Text = ka
                    ListView1.Items.Add(ListView1.Items.Count + 1)
                    ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                    ListView1.Items(i - 1).SubItems.Add(ka)
                    Button55.BackColor = Color.Green
                    Button55.ForeColor = Color.White
                    If ListView1.Items.Count > 1 Then
                        If CDec(ListView1.Items(ListView1.Items.Count).SubItems(2).Text) > CDec(ListView1.Items(ListView1.Items.Count - 1).SubItems(2).Text) Then
                            SerialPort1.Close()
                            SerialPort1.BaudRate = CmbBaud.SelectedItem
                            SerialPort1.PortName = CmbScanPort.SelectedItem
                            SerialPort1.Open()
                            'wait(1000)
                            MsgBox("fan dihidupkan")
                            SerialPort1.Write("1") 'Menghidupkan fan
                            SerialPort1.Write("2") 'Mematikan heater
                        ElseIf CDec(ListView1.Items(ListView1.Items.Count).SubItems(2).Text) < CDec(ListView1.Items(ListView1.Items.Count - 1).SubItems(2).Text) Then
                            SerialPort1.Close()
                            SerialPort1.BaudRate = CmbBaud.SelectedItem
                            SerialPort1.PortName = CmbScanPort.SelectedItem
                            SerialPort1.Open()
                            wait(1000)
                            MsgBox("heater dihidupkan")
                            SerialPort1.Write("0") 'Mematikan fan
                            SerialPort1.Write("3") 'Menghidupkann heater
                        End If
                        'Graphic setting
                        If ComboBox4.Text = "Point" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox4.Text = "Area" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox4.Text = "Fast Line" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                        Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                        'ListView1.Items(i - 1).EnsureVisible()
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                        If ComboBox5.Text = "Red" Then
                            Chart2.Series(0).Color = Color.Red
                        ElseIf ComboBox5.Text = "Green" Then
                            Chart2.Series(0).Color = Color.Green
                        ElseIf ComboBox5.Text = "Blue" Then
                            Chart2.Series(0).Color = Color.Blue
                        Else
                            Chart2.Series(0).Color = Color.Brown
                        End If
                        If ComboBox6.Text = "Dash" Then
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                        Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                    End If
                End If

                If i = CInt(TextBox52.Text) Then
                    If ComboBox3.Text = "S-Denaturation" Then
                        Button56.PerformClick()
                    ElseIf ComboBox3.Text = "S-Annealing" Then
                        Button69.PerformClick()
                    ElseIf ComboBox3.Text = "S-Extension" Then
                        Button70.PerformClick()
                    ElseIf ComboBox3.Text = "S-Finishing" Then
                        Button53.PerformClick()
                    End If
                End If
            Next
        End If
        ' Catch es As Exception
        'MsgBox("Not enough data to steady state")
        'Exit Sub
        'End Try




    End Sub
    Private Sub Button56_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button56.Click
        Try
            Dim i As Integer
            With TrendLine
                .ChartType = SeriesChartType.Line
                .Color = Color.DodgerBlue
                .BorderWidth = 1
                .IsVisibleInLegend = False
            End With
            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            Else
                SerialPort1.BaudRate = CmbBaud.SelectedItem
                SerialPort1.PortName = CmbScanPort.SelectedItem
                Dim ka As String
                For i = 1 To 10000000
                    SerialPort1.Close()
                    SerialPort1.Open()
                    SerialPort1.ReadLine.ToString()
                    ka = CStr(SerialPort1.ReadLine.ToString)
                    If IsNumeric(ka) = True Then
                        wait(TextBox60.Text)
                        Label118.Text = ka
                        ListView1.Items.Add(ListView1.Items.Count + 1)
                        ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                        ListView1.Items(i - 1).SubItems.Add(ka)
                        ComboBox3.Text = "Annealing"
                        ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                        ComboBox3.BackColor = Color.Orange
                        ListView1.Items(i - 1).BackColor = Color.Orange
                        Button5.PerformClick()
                        'Graphic setting
                        If ka >= TextBox58.Text And ka <= TextBox62.Text Then
                            ComboBox3.Text = "S-Annealing"
                            ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                            ComboBox3.BackColor = Color.Orange
                            ListView1.Items(i - 1).BackColor = Color.Orange
                            ComboBox3.ForeColor = Color.Black
                            Button55.PerformClick()
                        End If
                        If ComboBox4.Text = "Point" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox4.Text = "Area" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox4.Text = "Fast Line" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                        Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                        ' ListView1.Items(i - 1).EnsureVisible()
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                        If ComboBox5.Text = "Red" Then
                            Chart2.Series(0).Color = Color.Red
                        ElseIf ComboBox5.Text = "Green" Then
                            Chart2.Series(0).Color = Color.Green
                        ElseIf ComboBox5.Text = "Blue" Then
                            Chart2.Series(0).Color = Color.Blue
                        Else
                            Chart2.Series(0).Color = Color.Brown
                        End If
                        If ComboBox6.Text = "Dash" Then
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                        Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                    End If
                Next
            End If
        Catch es As Exception
            Exit Sub
        End Try
    End Sub
    Private Sub Button69_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button69.Click
        Try
            Dim i As Integer
            With TrendLine
                .ChartType = SeriesChartType.Line
                .Color = Color.DodgerBlue
                .BorderWidth = 1
                .IsVisibleInLegend = False
            End With
            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            Else
                SerialPort1.BaudRate = CmbBaud.SelectedItem
                SerialPort1.PortName = CmbScanPort.SelectedItem
                Dim ka As String
                For i = 1 To 10000000
                    SerialPort1.Close()
                    SerialPort1.Open()
                    SerialPort1.ReadLine.ToString()
                    ka = CStr(SerialPort1.ReadLine.ToString)
                    If IsNumeric(ka) = True Then
                        wait(TextBox60.Text)
                        Label118.Text = ka
                        ListView1.Items.Add(ListView1.Items.Count + 1)
                        ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                        ListView1.Items(i - 1).SubItems.Add(ka)
                        ComboBox3.Text = "Extension"
                        ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                        ComboBox3.BackColor = Color.Yellow
                        ListView1.Items(i - 1).BackColor = Color.Yellow
                        If ComboBox3.Text = "Extension" Then
                            Button14.PerformClick()
                        End If
                        If ka >= TextBox63.Text And ka <= TextBox73.Text Then
                            ComboBox3.Text = "S-Extension"
                            ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                            ComboBox3.BackColor = Color.Yellow
                            ListView1.Items(i - 1).BackColor = Color.Yellow
                            ComboBox3.ForeColor = Color.Black
                            Button55.PerformClick()
                        End If
                        'Graphic setting
                        If ComboBox4.Text = "Point" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox4.Text = "Area" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox4.Text = "Fast Line" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                        Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                        '  ListView1.Items(i - 1).EnsureVisible()
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                        If ComboBox5.Text = "Red" Then
                            Chart2.Series(0).Color = Color.Red
                        ElseIf ComboBox5.Text = "Green" Then
                            Chart2.Series(0).Color = Color.Green
                        ElseIf ComboBox5.Text = "Blue" Then
                            Chart2.Series(0).Color = Color.Blue
                        Else
                            Chart2.Series(0).Color = Color.Brown
                        End If
                        If ComboBox6.Text = "Dash" Then
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                        Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                    End If
                Next
            End If
        Catch es As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub Button70_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button70.Click
        Try
            Dim i As Integer
            With TrendLine
                .ChartType = SeriesChartType.Line
                .Color = Color.DodgerBlue
                .BorderWidth = 1
                .IsVisibleInLegend = False
            End With
            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            Else
                SerialPort1.BaudRate = CmbBaud.SelectedItem
                SerialPort1.PortName = CmbScanPort.SelectedItem
                Dim ka As String
                For i = 1 To 10000000
                    SerialPort1.Close()
                    SerialPort1.Open()
                    SerialPort1.ReadLine.ToString()
                    ka = CStr(SerialPort1.ReadLine.ToString)
                    If IsNumeric(ka) = True Then
                        wait(TextBox60.Text)
                        Label118.Text = ka
                        ListView1.Items.Add(ListView1.Items.Count + 1)
                        ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                        ListView1.Items(i - 1).SubItems.Add(ka)
                        ComboBox3.Text = "Finishing"
                        ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                        ComboBox3.BackColor = Color.LightGreen
                        ListView1.Items(i - 1).BackColor = Color.LightGreen
                        If ComboBox3.Text = "Finishing" Then
                            Button5.PerformClick()
                        End If
                        If ka >= TextBox74.Text And ka <= TextBox75.Text Then
                            ComboBox3.Text = "S-Finishing"
                            ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                            ComboBox3.BackColor = Color.LightGreen
                            ListView1.Items(i - 1).BackColor = Color.LightGreen
                            ComboBox3.ForeColor = Color.Black
                            Button55.PerformClick()
                        End If
                        'Graphic setting
                        If ComboBox4.Text = "Point" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox4.Text = "Area" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox4.Text = "Fast Line" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                        Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                        ' ListView1.Items(i - 1).EnsureVisible()
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                        If ComboBox5.Text = "Red" Then
                            Chart2.Series(0).Color = Color.Red
                        ElseIf ComboBox5.Text = "Green" Then
                            Chart2.Series(0).Color = Color.Green
                        ElseIf ComboBox5.Text = "Blue" Then
                            Chart2.Series(0).Color = Color.Blue
                        Else
                            Chart2.Series(0).Color = Color.Brown
                        End If
                        If ComboBox6.Text = "Dash" Then
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                        Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                    End If
                Next
            End If
        Catch es As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub Button71_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button71.Click
        Timer2.Interval = 1000
        Timer2.Start()
    End Sub

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        ' Try
        Dim i As Integer = ListView1.Items.Count + 1
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            ' Button14.PerformClick()
            ' wait(1000)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)
            Dim ka As String
            'wait(1)

            'Denaturation prepare
            'For i = 1 To 10000000
            SerialPort1.Close()
            SerialPort1.Open()
            SerialPort1.ReadLine.ToString()
            ka = CStr(SerialPort1.ReadLine.ToString)
            If IsNumeric(ka) = True Then
                ' wait(TextBox60.Text)
                Label118.Text = ka
                ListView1.Items.Add(ListView1.Items.Count + 1)
                ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                ListView1.Items(i - 1).SubItems.Add(ka)
                If i <= 1 And ComboBox3.Text = "Run All Process" And ka < TextBox48.Text Then 'memberikan label proses pertama denaturasi
                    ListView1.Items(i - 1).SubItems.Add("Denatruration")
                    Button14.PerformClick() 'heater on
                    ComboBox3.Text = "Denaturation"
                    ComboBox3.BackColor = Color.Red
                    ComboBox3.ForeColor = Color.White
                ElseIf i <= 1 And ComboBox3.Text = "Denaturation" Then 'memberikan label proses pertama denaturasi
                    ListView1.Items(i - 1).SubItems.Add("Denatruration")
                    Button14.PerformClick() 'heater on
                    ComboBox3.Text = "Denaturation"
                    ComboBox3.BackColor = Color.Red
                    ComboBox3.ForeColor = Color.White
                    'Denaturation start
                ElseIf ka >= CStr(TextBox48.Text) And ka <= CStr(TextBox59.Text) Then
                    ComboBox3.Text = "S-Denaturation"
                    brs = ListView1.Items.Count
                    ' MsgBox("Steady Process for Denaturation Start")
                    '    i = 10000000
                    Timer3.Start()
                    ' Button55.PerformClick()
                    Timer2.Stop()
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.Red
                    ListView1.Items(i - 1).BackColor = Color.Red
                    ComboBox3.ForeColor = Color.White

                ElseIf ka > TextBox59.Text And ComboBox3.Text = "Run All Process" Then
                    Button5.PerformClick()
                    MsgBox("PCR Machine is not read. PCR Machine try to match the tempreature")
                    Exit Sub
                End If
                Button87.PerformClick()
                'Graphic setting
                If ComboBox4.Text = "Point" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                ElseIf ComboBox4.Text = "Area" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                ElseIf ComboBox4.Text = "Fast Line" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Else
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                End If
                Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                'ListView1.Items(i - 1).EnsureVisible()
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                If ComboBox5.Text = "Red" Then
                    Chart2.Series(0).Color = Color.Red
                ElseIf ComboBox5.Text = "Green" Then
                    Chart2.Series(0).Color = Color.Green
                ElseIf ComboBox5.Text = "Blue" Then
                    Chart2.Series(0).Color = Color.Blue
                Else
                    Chart2.Series(0).Color = Color.Brown
                End If
                If ComboBox6.Text = "Dash" Then
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    End With
                Else
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    End With
                End If
                Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
            End If
            ' Next
        End If
    End Sub
    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Dim i As Integer = ListView1.Items.Count + 1
        Dim vara1 As String = 0
        Dim vara2 As String = 0
        If ComboBox3.Text = "S-Denaturation" Then
            vara1 = TextBox48.Text
            vara2 = TextBox59.Text
        ElseIf ComboBox3.Text = "S-Annealing" Then
            vara1 = TextBox58.Text
            vara2 = TextBox62.Text
        ElseIf ComboBox3.Text = "S-Extension" Then
            vara1 = TextBox63.Text
            vara2 = TextBox73.Text
        ElseIf ComboBox3.Text = "S-Finishing" Then
            vara1 = TextBox74.Text
            vara2 = TextBox75.Text
        End If
        'i = Label131.Text
        Dim ka As String
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            If i > 1 Then
                SerialPort1.BaudRate = CmbBaud.SelectedItem
                SerialPort1.PortName = CmbScanPort.SelectedItem
                Button55.BackColor = Color.Green
                SerialPort1.Open()
                SerialPort1.ReadLine.ToString()
                ka = CStr(SerialPort1.ReadLine.ToString)
                If IsNumeric(ka) = True Then
                    Label118.Text = ka
                    ListView1.Items.Add(ListView1.Items.Count + 1)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(ka)
                    Button55.BackColor = Color.Green
                    Button55.ForeColor = Color.White
                    If ListView1.Items.Count > 1 Then
                        If CDec(ListView1.Items(CInt(ListView1.Items.Count - 1)).SubItems(2).Text) > CDec(ListView1.Items(CInt(ListView1.Items.Count - 2)).SubItems(2).Text) And CDec(ListView1.Items(CInt(ListView1.Items.Count - 1)).SubItems(2).Text) > CDec(vara1) Then
                            'brs = brs + 1
                            SerialPort1.Write("1") 'Menghidupkan fan
                            SerialPort1.Write("2") 'Mematikan heater
                        ElseIf CDec(ListView1.Items(CInt(ListView1.Items.Count - 1)).SubItems(2).Text) < CDec(ListView1.Items(CInt(ListView1.Items.Count - 2)).SubItems(2).Text) And CDec(ListView1.Items(CInt(ListView1.Items.Count - 2)).SubItems(2).Text) < CDec(vara2) Then
                            'brs = brs + 1
                            SerialPort1.Write("0") 'Mematikan fan
                            SerialPort1.Write("3") 'Menghidupkann heater
                        End If
                        'Graphic setting
                        If ComboBox4.Text = "Point" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                        ElseIf ComboBox4.Text = "Area" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                        ElseIf ComboBox4.Text = "Fast Line" Then
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                        Else
                            Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                        End If
                        Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                        Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                        Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                        'ListView1.Items(i - 1).EnsureVisible()
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                        Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                        If ComboBox5.Text = "Red" Then
                            Chart2.Series(0).Color = Color.Red
                        ElseIf ComboBox5.Text = "Green" Then
                            Chart2.Series(0).Color = Color.Green
                        ElseIf ComboBox5.Text = "Blue" Then
                            Chart2.Series(0).Color = Color.Blue
                        Else
                            Chart2.Series(0).Color = Color.Brown
                        End If
                        If ComboBox6.Text = "Dash" Then
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            End With
                        Else
                            With Chart2.ChartAreas(0)
                                .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                                .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            End With
                        End If
                        Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                        Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                    End If
                    Button87.PerformClick()
                End If
            Else
                wait(1000)
            End If
            Label131.Text = brs
            If i = CInt(TextBox52.Text + brs) Then
                If ComboBox3.Text = "S-Denaturation" Then
                    Timer4.Start()
                    Button55.BackColor = Color.Gray
                    Timer3.Stop()
                    ' Button56.PerformClick()
                ElseIf ComboBox3.Text = "S-Annealing" Then
                    Timer5.Start()
                    Button55.BackColor = Color.Orange
                    Timer3.Stop()
                ElseIf ComboBox3.Text = "S-Extension" Then
                    Timer6.Start()
                    Button55.BackColor = Color.Yellow
                    Timer3.Stop()
                ElseIf ComboBox3.Text = "S-Finishing" Then
                    Button53.PerformClick()
                    Timer3.Stop()
                End If
                'Timer3.Stop()
            End If
            'Next
        End If
    End Sub

    Private Sub Button72_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button72.Click
        Timer3.Interval = 1000
        Timer3.Start()

    End Sub

    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        Dim i As Integer
        ' SerialPort1.Close()
       
        i = ListView1.Items.Count + 1
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            
            Dim ka As String
            'For i = 1 To 10000000
            SerialPort1.Close()
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            SerialPort1.Open()
            SerialPort1.ReadLine.ToString()
            ka = CStr(SerialPort1.ReadLine.ToString)
            If IsNumeric(ka) = True Then
                ' wait(TextBox60.Text)
                Label118.Text = ka
                ListView1.Items.Add(ListView1.Items.Count + 1)
                ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                ListView1.Items(i - 1).SubItems.Add(ka)
                ComboBox3.Text = "Annealing"
                ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                ComboBox3.BackColor = Color.Orange
                ListView1.Items(i - 1).BackColor = Color.Orange
                Button5.PerformClick()
                'Graphic setting
                If ka >= TextBox58.Text And ka <= TextBox62.Text Then
                    ComboBox3.Text = "S-Annealing"
                    brs = ListView1.Items.Count
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.Orange
                    ListView1.Items(i - 1).BackColor = Color.Orange
                    ComboBox3.ForeColor = Color.Black
                    Timer3.Start()
                    Timer4.Stop()
                    ' Button55.PerformClick()
                End If
                If ComboBox4.Text = "Point" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                ElseIf ComboBox4.Text = "Area" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                ElseIf ComboBox4.Text = "Fast Line" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Else
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                End If
                Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                ' ListView1.Items(i - 1).EnsureVisible()
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                '  If ComboBox5.Text = "Red" Then
                'Chart2.Series(0).Color = Color.Red
                ' ElseIf ComboBox5.Text = "Green" Then
                Chart2.Series(0).Color = Color.Green
                '  ElseIf ComboBox5.Text = "Blue" Then
                ' Chart2.Series(0).Color = Color.Blue
                '   Else
                '  Chart2.Series(0).Color = Color.Brown
                ' End If
                If ComboBox6.Text = "Dash" Then
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    End With
                Else
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    End With
                End If
                Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                Button87.PerformClick()
            End If
            'Next
        End If
    End Sub

    Private Sub Timer5_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer5.Tick
        Dim i As Integer
        ' SerialPort1.Close()

        i = ListView1.Items.Count + 1
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else

            Dim ka As String
            'For i = 1 To 10000000
            SerialPort1.Close()
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            SerialPort1.Open()
            SerialPort1.ReadLine.ToString()
            ka = CStr(SerialPort1.ReadLine.ToString)
            If IsNumeric(ka) = True Then
                ' wait(TextBox60.Text)
                Label118.Text = ka
                ListView1.Items.Add(ListView1.Items.Count + 1)
                ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                ListView1.Items(i - 1).SubItems.Add(ka)
                ComboBox3.Text = "Extension"
                ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                ComboBox3.BackColor = Color.Yellow
                'ListView1.Items(i - 1).BackColor = Color.Yellow
                Button14.PerformClick()
                'Graphic setting
                If ka >= TextBox63.Text And ka <= TextBox73.Text Then
                    ComboBox3.Text = "S-Extension"
                    brs = ListView1.Items.Count
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.Yellow
                    ListView1.Items(i - 1).BackColor = Color.Yellow
                    ComboBox3.ForeColor = Color.Black
                    Timer3.Start()
                    Timer5.Stop()
                    ' Button55.PerformClick()
                End If
                If ComboBox4.Text = "Point" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                ElseIf ComboBox4.Text = "Area" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                ElseIf ComboBox4.Text = "Fast Line" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Else
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                End If
                Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                ' ListView1.Items(i - 1).EnsureVisible()
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                '  If ComboBox5.Text = "Red" Then
                'Chart2.Series(0).Color = Color.Red
                ' ElseIf ComboBox5.Text = "Green" Then
                'Chart2.Series(0).Color = Color.Green
                '  ElseIf ComboBox5.Text = "Blue" Then
                Chart2.Series(0).Color = Color.Blue
                '   Else
                '  Chart2.Series(0).Color = Color.Brown
                ' End If
                If ComboBox6.Text = "Dash" Then
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    End With
                Else
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    End With
                End If
                Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                Button87.PerformClick()
            End If
            'Next
        End If

    End Sub

    Private Sub Timer6_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer6.Tick
        Try
            Dim i As Integer
            ' SerialPort1.Close()

            i = ListView1.Items.Count + 1
            With TrendLine
                .ChartType = SeriesChartType.Line
                .Color = Color.DodgerBlue
                .BorderWidth = 1
                .IsVisibleInLegend = False
            End With
            If SerialPort1.IsOpen Then
                SerialPort1.Close()
            Else

                Dim ka As String
                'For i = 1 To 10000000
                SerialPort1.Close()
                SerialPort1.BaudRate = CmbBaud.SelectedItem
                SerialPort1.PortName = CmbScanPort.SelectedItem
                SerialPort1.Open()
                SerialPort1.ReadLine.ToString()
                ka = CStr(SerialPort1.ReadLine.ToString)
                If IsNumeric(ka) = True Then
                    ' wait(TextBox60.Text)
                    Label118.Text = ka
                    ListView1.Items.Add(ListView1.Items.Count + 1)
                    ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                    ListView1.Items(i - 1).SubItems.Add(ka)
                    ComboBox3.Text = "Finishing"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                    ComboBox3.BackColor = Color.LightGreen
                    'ListView1.Items(i - 1).BackColor = Color.Yellow
                    Button5.PerformClick()
                    'Graphic setting
                    If ka >= TextBox74.Text And ka <= TextBox75.Text Then
                        ComboBox3.Text = "S-Finishing"
                        brs = ListView1.Items.Count
                        ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                        ComboBox3.BackColor = Color.LightGreen
                        ListView1.Items(i - 1).BackColor = Color.LightGreen
                        ComboBox3.ForeColor = Color.Black
                        Timer3.Start()
                        Timer6.Stop()
                        ' Button55.PerformClick()
                    End If
                    If ComboBox4.Text = "Point" Then
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                    ElseIf ComboBox4.Text = "Area" Then
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                    ElseIf ComboBox4.Text = "Fast Line" Then
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                    Else
                        Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                    End If
                    Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                    Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                    Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                    Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                    Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                    ' ListView1.Items(i - 1).EnsureVisible()
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                    Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                    '  If ComboBox5.Text = "Red" Then
                    'Chart2.Series(0).Color = Color.Red
                    ' ElseIf ComboBox5.Text = "Green" Then
                    'Chart2.Series(0).Color = Color.Green
                    '  ElseIf ComboBox5.Text = "Blue" Then
                    Chart2.Series(0).Color = Color.Blue
                    '   Else
                    '  Chart2.Series(0).Color = Color.Brown
                    ' End If
                    If ComboBox6.Text = "Dash" Then
                        With Chart2.ChartAreas(0)
                            .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                            .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        End With
                    Else
                        With Chart2.ChartAreas(0)
                            .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                            .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        End With
                    End If
                    Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                    Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
                End If
                'Next
                Button87.PerformClick()
            End If
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button74_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button74.Click
        Try
            Dim i As Integer = ListView8.Items.Count
            ListView8.Items.Add(TextBox77.Text)
            ListView8.Items(i).SubItems.Add(TextBox76.Text)
            ListView8.Items(i).SubItems.Add(TextBox79.Text)
            ListView8.Items(i).SubItems.Add(TextBox80.Text)
            ListView8.Items(i).SubItems.Add(TextBox78.Text)
            If Label160.Text = "Ready" Then
                ListView8.Items(i).SubItems.Add("Ready")
            ElseIf Label160.Text = "Finish" Then
                ListView8.Items(i).SubItems.Add("Finish")
            Else
                ListView8.Items(i).SubItems.Add("On Process")
            End If
            TextBox76.Text = ""
            TextBox79.Text = ""
            TextBox78.Text = ""
            TextBox77.Text = ""
        Catch EX As Exception
        End Try
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Try
            If CheckBox1.Checked Then
                GroupBox18.Enabled = True
                ListView8.Enabled = False
            End If
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button84_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button84.Click
        Try
            ListView8.Items(CInt(TextBox103.Text) - 1).SubItems(CInt(TextBox99.Text) - 1).Text = ""
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button83_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button83.Click
        Try
            If ListView8.Items.Count < (CInt(TextBox103.Text)) Or ListView8.Columns.Count < CInt(TextBox99.Text) Then
                MsgBox("Data tidak tersedia")
            Else
                ListView8.Items(CInt(TextBox103.Text) - 1).SubItems(CInt(TextBox99.Text) - 1).Text = TextBox92.Text
            End If
        Catch EX As Exception
        End Try
    End Sub
    Private Sub Button75_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button75.Click
        Try
            TextBox76.Text = ""
            TextBox79.Text = ""
            TextBox80.Text = ""
            TextBox78.Text = ""
            TextBox77.Text = ""
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button77_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button77.Click
        Try
            If Not TextBox84.Text = "" Or Not TextBox109.Text = "" Then
                Dim i As Integer = ListView9.Items.Count
                If ListView9.Columns.Count < Val(TextBox109.Text) Then
                    MsgBox("Kolom tidak tersedia")
                ElseIf ListView9.Items.Count < TextBox84.Text Then
                    ' ListView9.Items.Add("")

                    For j As Integer = 0 To CInt(TextBox84.Text) - 1
                        ListView9.Items.Add("")
                        ListView9.Items(j).SubItems(0).Text = j + 1
                        For brs As Integer = 1 To CInt(ListView9.Items.Count)
                            ListView9.Items(brs - 1).SubItems.Add("")
                        Next
                    Next
                    ListView9.Items(CInt(TextBox84.Text) - 1).SubItems(CInt(TextBox109.Text) - 1).Text = TextBox82.Text
                Else
                    ListView9.Items(CInt(TextBox84.Text) - 1).SubItems(CInt(TextBox109.Text) - 1).Text = TextBox82.Text
                End If
            End If
            If Not TextBox85.Text = "" Or Not TextBox108.Text = "" Then
                If ListView9.Columns.Count < Val(TextBox108.Text) Then
                    MsgBox("Kolom tidak tersedia")
                ElseIf ListView9.Items.Count < TextBox85.Text Then
                    'ListView9.Items.Add("")
                    For j As Integer = 0 To CInt(TextBox85.Text) - 1
                        ListView9.Items.Add("")
                        ListView9.Items(j).SubItems(0).Text = j + 1
                        For brs As Integer = 1 To CInt(ListView9.Items.Count)
                            ListView9.Items(brs - 1).SubItems.Add("")
                        Next
                    Next
                    ListView9.Items(CInt(TextBox85.Text) - 1).SubItems(CInt(TextBox108.Text) - 1).Text = TextBox86.Text
                Else
                    ListView9.Items(CInt(TextBox85.Text) - 1).SubItems(CInt(TextBox108.Text) - 1).Text = TextBox86.Text
                End If
            End If
            If Not TextBox89.Text = "" Or Not TextBox107.Text = "" Then
                If ListView9.Columns.Count < Val(TextBox107.Text) Then
                    MsgBox("Kolom tidak tersedia")
                ElseIf ListView9.Items.Count < TextBox89.Text Then
                    'ListView9.Items.Add("")
                    For j As Integer = 0 To CInt(TextBox89.Text) - 1
                        ListView9.Items.Add("")
                        ListView9.Items(j).SubItems(0).Text = j + 1
                        For brs As Integer = 1 To CInt(ListView9.Items.Count)
                            ListView9.Items(brs - 1).SubItems.Add("")
                        Next
                    Next
                    ListView9.Items(CInt(TextBox89.Text) - 1).SubItems(CInt(TextBox107.Text) - 1).Text = TextBox90.Text
                Else
                    ListView9.Items(CInt(TextBox89.Text) - 1).SubItems(CInt(TextBox107.Text) - 1).Text = TextBox90.Text
                End If
            End If
            TextBox82.Text = ""
            TextBox85.Text = ""
            TextBox89.Text = ""
            TextBox84.Text = ""
            TextBox90.Text = ""
            TextBox109.Text = ""
            TextBox86.Text = ""
            TextBox108.Text = ""
            TextBox107.Text = ""
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button76_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button76.Click
        Try
            If ListView9.Columns.Count < Val(TextBox110.Text) Then
                For i As Integer = 0 To CInt(TextBox110.Text) - 2
                    ListView9.Columns.Add("")
                Next
                ListView9.Columns(CInt(TextBox110.Text) - 1).Text = TextBox81.Text
                ListView9.Columns(CInt(TextBox110.Text) - 1).Width = Val(TextBox83.Text)
            Else
                ListView9.Columns(CInt(TextBox110.Text) - 1).Text = TextBox81.Text
                ListView9.Columns(CInt(TextBox110.Text) - 1).Width = Val(TextBox83.Text)
            End If
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button78_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button78.Click
        Try
            ListView9.Columns(CInt(TextBox110.Text) - 1).Text = TextBox81.Text
            ListView9.Columns(CInt(TextBox110.Text) - 1).Width = Val(TextBox83.Text)
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button79_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button79.Click
        Try
            ListView9.Columns(CInt(TextBox110.Text) - 1).Text = ""
            ListView9.Columns.RemoveAt(CInt(TextBox110.Text) - 1)
        Catch EX As Exception
        End Try
    End Sub
    Private Sub Button81_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button81.Click
        Try
            ListView9.Items(CInt(TextBox88.Text) - 1).SubItems(CInt(TextBox87.Text) - 1).Text = ""
        Catch EX As Exception
        End Try
    End Sub

    Private Sub Button80_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button80.Click
        TextBox82.Text = ""
        TextBox86.Text = ""
        TextBox90.Text = ""
        TextBox109.Text = ""
        TextBox84.Text = ""
        TextBox85.Text = ""
        TextBox108.Text = ""
        TextBox89.Text = ""
        TextBox107.Text = ""
    End Sub

    Private Sub Button82_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button82.Click
        ListView9.Items.Clear()
    End Sub

   
    Private Sub Button85_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button85.Click
        ListView8.Items.Clear()
    End Sub

    Private Sub Button86_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button86.Click
        Try
            TabControl1.SelectedIndex = 4
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    Private Sub Timer7_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer7.Tick
        Dim i As Integer = ListView1.Items.Count + 1
        With TrendLine
            .ChartType = SeriesChartType.Line
            .Color = Color.DodgerBlue
            .BorderWidth = 1
            .IsVisibleInLegend = False
        End With
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        Else
            SerialPort1.BaudRate = CmbBaud.SelectedItem
            SerialPort1.PortName = CmbScanPort.SelectedItem
            SerialPort1.Open()
            SerialPort1.ReadLine.ToString()
            ' Button14.PerformClick()
            ' wait(1000)
            Dim ka = CStr(SerialPort1.ReadLine.ToString)
            ' Dim ka = CStr(SerialPort1.ReadLine.ToString)

            'wait(1)

            'Denaturation prepare
            If IsNumeric(ka) = True Then
                Label118.Text = ka
                ListView1.Items.Add(ListView1.Items.Count + 1)
                ListView1.Items(i - 1).SubItems.Add(Date.Now.ToString("HH:mm:ss"))
                ListView1.Items(i - 1).SubItems.Add(ka)
                If i <= 1 And ComboBox3.Text = "Denaturation" Then 'memberikan label proses pertama denaturasi
                    ListView1.Items(i - 1).SubItems.Add("Process to Denatruration")
                    ComboBox3.Text = "Process to Denaturation"
                    ComboBox3.BackColor = Color.White
                ElseIf ka >= TextBox48.Text And ka <= TextBox59.Text And i >= 2 And ListView1.Items(i - 2).SubItems(3).Text = "Process to Denaturation" Then
                    ComboBox3.Text = "Denaturation"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                ElseIf ka >= TextBox58.Text And ka <= TextBox62.Text And ListView1.Items(i - 2).SubItems(3).Text = "Denaturation" Then
                    ComboBox3.Text = "Annealing"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                ElseIf ka >= TextBox63.Text And ka <= TextBox73.Text And ListView1.Items(i - 2).SubItems(3).Text = "Annealing" Then
                    ComboBox3.Text = "Extension"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                ElseIf ka >= TextBox74.Text And ka <= TextBox75.Text And ListView1.Items(i - 2).SubItems(3).Text = "Extension" Then
                    ComboBox3.Text = "Finishing"
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                Else
                    ListView1.Items(i - 1).SubItems.Add(ComboBox3.Text)
                End If
                Button87.PerformClick()

                'push button

                'Graphic setting
                If ComboBox4.Text = "Point" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Point
                ElseIf ComboBox4.Text = "Area" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Area
                ElseIf ComboBox4.Text = "Fast Line" Then
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.FastLine
                Else
                    Chart2.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline
                End If
                Chart2.ChartAreas("ChartArea1").AxisY.Maximum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisY.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Minimum = Double.NaN
                Chart2.ChartAreas("ChartArea1").AxisX.Maximum = Double.NaN
                Chart2.Series(0).Points.AddXY((ListView1.Items(i - 1).SubItems(1).Text.ToString), Format(CDbl(ListView1.Items(i - 1).SubItems(2).Text.ToString), "0.0000E00"))
                ListView1.Items(i - 1).EnsureVisible()
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Size = 10
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.IsPositionedInside = True
                Chart2.ChartAreas("ChartArea1").AxisX.ScrollBar.Enabled = True
                If ComboBox5.Text = "Red" Then
                    Chart2.Series(0).Color = Color.Red
                ElseIf ComboBox5.Text = "Green" Then
                    Chart2.Series(0).Color = Color.Green
                ElseIf ComboBox5.Text = "Blue" Then
                    Chart2.Series(0).Color = Color.Blue
                Else
                    Chart2.Series(0).Color = Color.Brown
                End If
                If ComboBox6.Text = "Dash" Then
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                    End With
                Else
                    With Chart2.ChartAreas(0)
                        .AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                        .AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Solid
                    End With
                End If
                Chart2.Series(0).SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                Chart2.Series(0).SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Left
            End If
        End If
    End Sub

    Private Sub IOT(ByVal Query As String)
        
    End Sub

    Private Sub Button87_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button87.Click
        Mysqlcon = New MySqlConnection
        Mysqlcon.ConnectionString =
            "server=sql12.freemysqlhosting.net;userid=sql12352533;password=4QKZBxpXgg;database=sql12352533"
        Dim reader As MySqlDataReader
        Try
            Mysqlcon.Open()
            'MessageBox.Show("Connection Successfull")
            Dim Query As String
            Dim i As Integer = ListView1.Items.Count + 1
            Query = "insert into sql12352533.Temperature(Nomor,Waktu,Temperature) values('" & ListView1.Items(i - 1).SubItems(0).Text & "', '" & ListView1.Items(i - 1).SubItems(1).Text & "', '" & ListView1.Items(i - 1).SubItems(2).Text & "', '" & ListView1.Items(i - 1).SubItems(3).Text & "')"
            COMMAND = New MySqlCommand(Query, Mysqlcon)
            reader = COMMAND.ExecuteReader
            'MessageBox.Show("Data Saved")
            Mysqlcon.Close()
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        Finally
            Mysqlcon.Dispose()
        End Try
    End Sub
End Class
