﻿Imports System, System.IO, System.Collections.Generic, System.Runtime.InteropServices, System.ComponentModel
Imports System.Drawing, System.Drawing.Drawing2D, System.Drawing.Imaging, System.Windows.Forms

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Credits ] --------------------
'Creator: Recuperare
'Contact: cschaefer2183 (Skype)
'Created: 10.11.2012
'Changed: 10.11.2012
'-------------------- [ /Credits ] ---------------------

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!
#Region " GLOBAL FUNCTIONS "

Class infDraw
    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
End Class

#End Region


Public Class InfluenceTheme : Inherits ContainerControl

#Region " Control Help - Movement & Flicker Control "
    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight As Integer
    Private pos As Integer = 0
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True : MouseP = e.Location
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Cap Then
            Parent.Location = MousePosition - MouseP
        End If
    End Sub
    Protected Overrides Sub OnInvalidated(ByVal e As System.Windows.Forms.InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        ParentForm.FindForm.Text = Text
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Me.ParentForm.FormBorderStyle = FormBorderStyle.None
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
    End Sub
    Private _closesEnv As Boolean = False
    Public Property CloseButtonExitsApp() As Boolean
        Get
            Return _closesEnv
        End Get
        Set(ByVal v As Boolean)
            _closesEnv = v
            Invalidate()
        End Set
    End Property

    Private _minimBool As Boolean
    Public Property MinimizeButton() As Boolean
        Get
            Return _minimBool
        End Get
        Set(ByVal v As Boolean)
            _minimBool = v
            Invalidate()
        End Set
    End Property

#End Region

    Dim WithEvents minimBtn As New InfluenceTopButton With {.ButtonIcon = InfluenceTopButton.ButtonType.Minim,
                                                 .Location = New Point(Width - 81, 0)}
    Dim WithEvents closeBtn As New InfluenceTopButton With {.ButtonIcon = InfluenceTopButton.ButtonType.Close,
                                                 .Location = New Point(Width - 52, 0)}

    Sub New()
        MyBase.New()
        Dock = DockStyle.Fill
        MoveHeight = 25
        Font = New Font("Verdana", 8.25F)
        DoubleBuffered = True
        Controls.Add(closeBtn)

        closeBtn.Refresh() : minimBtn.Refresh()
    End Sub

    Private Sub minimBtnClick() Handles minimBtn.Click
        ParentForm.FindForm.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub closeBtnClick() Handles closeBtn.Click
        If CloseButtonExitsApp Then
            System.Environment.Exit(0)
        Else
            ParentForm.FindForm.Close()
        End If
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        If _minimBool Then
            Controls.Add(minimBtn)
        Else
            Controls.Remove(minimBtn)
        End If

        minimBtn.Location = New Point(Width - 81, 0)
        closeBtn.Location = New Point(Width - 52, 0)

        G.SmoothingMode = SmoothingMode.HighSpeed
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim TransparencyKey As Color = Me.ParentForm.TransparencyKey
        Dim d As New infDraw()
        MyBase.OnPaint(e)

        G.Clear(TransparencyKey)

        G.FillPath(New SolidBrush(Color.FromArgb(20, 20, 20)), d.RoundRect(ClientRectangle, 2))


        Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
        Dim g1 As New LinearGradientBrush(New Rectangle(0, 2, Width - 1, 25), Color.FromArgb(40, 40, 40), Color.FromArgb(29, 29, 29), 90S)

        G.FillPath(g1, d.RoundRect(New Rectangle(0, 2, Width - 1, 25), 2))
        G.FillPath(h1, d.RoundRect(New Rectangle(0, 2, Width - 1, 25), 2))

        Dim s1 As New LinearGradientBrush(g1.Rectangle, Color.FromArgb(15, Color.White), Color.FromArgb(0, Color.White), 90S)
        G.FillRectangle(s1, New Rectangle(1, 1, Width - 1, 13))

        G.DrawLine(New Pen(Color.FromArgb(75, Color.White)), 1, 1, Width - 1, 1)

        G.DrawLine(New Pen(Color.FromArgb(18, 18, 18)), 1, 26, Width - 1, 26)

        G.DrawRectangle(New Pen(Color.FromArgb(37, 37, 37)), New Rectangle(1, 27, Width - 3, Height - 29))

        G.DrawPath(Pens.Black, d.RoundRect(ClientRectangle, 2))

        G.DrawString(Text, Font, Brushes.Black, New Rectangle(8, 8, Width - 1, 10), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        G.DrawString(Text, Font, Brushes.White, New Rectangle(8, 9, Width - 1, 11), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _align As StringAlignment = StringAlignment.Near
    Public Shadows Property TextAlignment() As StringAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As StringAlignment)
            _align = v
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Verdana", 8.25F)
        DoubleBuffered = True
        Size = New Size(115, 30)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim ClientRectangle = New Rectangle(0, 0, Width - 1, Height - 1)

        Dim d As New infDraw()
        G.SmoothingMode = SmoothingMode.HighQuality
        'G.Clear(Color.FromArgb(20, 20, 20))
        G.Clear(BackColor)

        Select Case State
            Case MouseState.None 'Mouse None
                G.FillPath(New SolidBrush(Color.FromArgb(20, 20, 20)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
                G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 1), 2))
                G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
                G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
            Case MouseState.Over 'Mouse Hover
                Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
                G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                'G.FillRectangle(g1, ClientRectangle)
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                'G.FillRectangle(h1, New Rectangle(0, 0, Width - 1, Height - 2))
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 1), 2))
                G.FillPath(New SolidBrush(Color.FromArgb(15, Color.White)), d.RoundRect(ClientRectangle, 2))
                G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
                G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
            Case MouseState.Down 'Mouse Down
                Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
                G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                'G.FillRectangle(g1, ClientRectangle)
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
                'G.FillRectangle(h1, New Rectangle(0, 0, Width - 1, Height - 2))
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 1), 2))
                G.FillPath(New SolidBrush(Color.FromArgb(15, Color.Black)), d.RoundRect(ClientRectangle, 2))
                G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
                G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        End Select

        'G.DrawRectangle(Pens.Black, ClientRectangle)

        G.DrawString(Text, Font, Brushes.Black, New Rectangle(5, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = _align})
        G.DrawString(Text, Font, Brushes.White, New Rectangle(5, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = _align})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceTopButton : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Enum ButtonType As Byte
        Blank = 0
        Close = 1
        Minim = 2
    End Enum
    Private State As MouseState = MouseState.None

    Private _type As ButtonType = ButtonType.Blank
    Public Property ButtonIcon() As ButtonType
        Get
            Return _type
        End Get
        Set(ByVal v As ButtonType)
            _type = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Verdana", 8.25F)
        Size = New Size(43, 21)
        DoubleBuffered = True
        Focus()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New infDraw()

        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        G.Clear(BackColor)

        Select Case State
            Case MouseState.None 'Mouse None
                Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
                G.FillRectangle(g1, g1.Rectangle)
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillRectangle(h1, g1.Rectangle)
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillRectangle(s1, s1.Rectangle)
                G.DrawRectangle(New Pen(Color.FromArgb(150, 97, 94, 90)), New Rectangle(0, 1, Width - 1, Height - 3))
                G.DrawRectangle(New Pen(Color.FromArgb(20, 20, 20)), New Rectangle(0, 0, Width - 1, Height - 1))
            Case MouseState.Over
                Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
                G.FillRectangle(g1, g1.Rectangle)
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillRectangle(h1, g1.Rectangle)
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillRectangle(s1, s1.Rectangle)
                G.FillRectangle(New SolidBrush(Color.FromArgb(15, Color.White)), New Rectangle(0, 0, Width - 1, Height - 1))
                G.DrawRectangle(New Pen(Color.FromArgb(150, 97, 94, 90)), New Rectangle(0, 1, Width - 1, Height - 3))
                G.DrawRectangle(New Pen(Color.FromArgb(20, 20, 20)), New Rectangle(0, 0, Width - 1, Height - 1))
            Case MouseState.Down
                Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
                G.FillRectangle(g1, g1.Rectangle)
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillRectangle(h1, g1.Rectangle)
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillRectangle(s1, s1.Rectangle)
                G.FillRectangle(New SolidBrush(Color.FromArgb(15, Color.Black)), New Rectangle(0, 0, Width - 1, Height - 1))
                G.DrawRectangle(New Pen(Color.FromArgb(150, 97, 94, 90)), New Rectangle(0, 1, Width - 1, Height - 3))
                G.DrawRectangle(New Pen(Color.FromArgb(20, 20, 20)), New Rectangle(0, 0, Width - 1, Height - 1))
        End Select

        Select Case _type
            Case ButtonType.Close
                Size = New Size(43, 21)
                G.DrawString("r", New Font("Marlett", 8.75), New SolidBrush(Color.FromArgb(220, Color.White)), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case ButtonType.Minim
                Size = New Size(30, 21)
                G.DrawString("0", New Font("Marlett", 8.75), New SolidBrush(Color.FromArgb(220, Color.White)), New Rectangle(1.5, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceProgressBar : Inherits Control

#Region " Control Help - Properties & Flicker Control "
    Private OFS As Integer = 0
    Private Speed As Integer = 50
    Private _Maximum As Integer = 100
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal v As Integer)
            Select Case v
                Case Is < _Value
                    _Value = v
            End Select
            _Maximum = v
            Invalidate()
        End Set
    End Property
    Private _Value As Integer = 0
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                Case Else
                    Return _Value
            End Select
        End Get
        Set(ByVal v As Integer)
            Select Case v
                Case Is > _Maximum
                    v = _Maximum
            End Select
            _Value = v
            Invalidate()
        End Set
    End Property
    Private _ShowPercentage As Boolean = False
    Public Property ShowPercentage() As Boolean
        Get
            Return _ShowPercentage
        End Get
        Set(ByVal v As Boolean)
            _ShowPercentage = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        ' Dim tmr As New Timer With {.Interval = Speed}
        ' AddHandler tmr.Tick, AddressOf Animate
        ' tmr.Start()
        Dim T As New Threading.Thread(AddressOf Animate)
        T.IsBackground = True
        'T.Start()
    End Sub
    Sub Animate()
        While True
            If OFS <= Width Then : OFS += 1
            Else : OFS = 0
            End If
            Invalidate()
            Threading.Thread.Sleep(Speed)
        End While
    End Sub
#End Region

    Sub New()
        MyBase.New()
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New infDraw()

        Dim intValue As Integer = CInt(_Value / _Maximum * Width)
        G.Clear(BackColor)

        Dim gB As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(16, 16, 16), Color.FromArgb(22, 22, 22), 90S)
        G.FillPath(gB, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        Dim g1 As New LinearGradientBrush(New Rectangle(0, 0, intValue - 1, Height - 2), Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
        G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 2), 2))
        Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
        G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 2), 2))
        Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
        G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, intValue - 1, Height / 2 - 1), 2))

        G.DrawPath(New Pen(Color.FromArgb(125, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))

        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 1), 2))
        G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 1), 2))

        If _ShowPercentage Then
            G.DrawString(Convert.ToString(String.Concat(Value, "%")), Font, Brushes.White, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceMultiLineTextBox : Inherits Control
    Dim WithEvents txtbox As New TextBox

#Region " Control Help - Properties & Flicker Control "
    Private _maxchars As Integer = 32767
    Public Property MaxCharacters() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
            Invalidate()
        End Set
    End Property
    Private _align As HorizontalAlignment
    Public Shadows Property TextAlignment() As HorizontalAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As HorizontalAlignment)
            _align = v
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        txtbox.BackColor = Color.FromArgb(43, 43, 43)
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        txtbox.Size = New Size(Width - 10, Height - 11)
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        txtbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        txtbox.Focus()
    End Sub
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = True
            .BackColor = BackColor
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(3, 4)
            .Font = New Font("Verdana", 8.25)
            .Size = New Size(Width - 10, Height - 10)
        End With
        txtbox.Font = New Font("Verdana", 8.25)
    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        Text = String.Empty
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New infDraw()
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        txtbox.TextAlign = TextAlignment

        G.Clear(BackColor)

        Dim g1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(40, 40, 40), Color.FromArgb(45, 45, 45), 90S)
        G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2), 2))
        Dim g2 As New LinearGradientBrush(New Rectangle(0, Height / 2, Width - 1, Height / 2), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), 90S)
        G.FillPath(g2, d.RoundRect(New Rectangle(0, Height / 2 - 3, Width - 1, Height / 2 + 2), 2))

        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(10, 10, 10)), d.RoundRect(ClientRectangle, 2))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceTextBox : Inherits Control
    Dim WithEvents txtbox As New TextBox

#Region " Control Help - Properties & Flicker Control "
    Private _passmask As Boolean = False
    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _passmask
        End Get
        Set(ByVal v As Boolean)
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar
            _passmask = v
            Invalidate()
        End Set
    End Property
    Private _maxchars As Integer = 32767
    Public Shadows Property MaxLength() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
            txtbox.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property
    Private _align As HorizontalAlignment
    Public Shadows Property TextAlignment() As HorizontalAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As HorizontalAlignment)
            _align = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        txtbox.BackColor = Color.FromArgb(43, 43, 43)
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        txtbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        txtbox.Focus()
    End Sub
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = False
            .BackColor = Color.FromArgb(43, 43, 43)
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(5, 5)
            .Font = New Font("Verdana", 8.25)
            .Size = New Size(Width - 10, Height - 11)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        Text = ""
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New infDraw()
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        Height = txtbox.Height + 11
        With txtbox
            .Width = Width - 10
            .TextAlign = TextAlignment
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(BackColor)

        Dim g1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(40, 40, 40), Color.FromArgb(45, 45, 45), 90S)
        G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2), 2))
        Dim g2 As New LinearGradientBrush(New Rectangle(0, Height / 2, Width - 1, Height / 2), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), 90S)
        G.FillPath(g2, d.RoundRect(New Rectangle(0, Height / 2 - 3, Width - 1, Height / 2 + 2), 2))

        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(10, 10, 10)), d.RoundRect(ClientRectangle, 2))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceListBox : Inherits Control
    Public WithEvents lstbox As New ListBox
    Private __Items As String() = {""}

#Region " Control Help - Properties & Flicker Control "
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        lstbox.Size = New Size(Width - 6, Height - 6)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        lstbox.BackColor = Color.FromArgb(43, 43, 43)
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        lstbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        lstbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        lstbox.Focus()
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        lstbox.Size = New Size(Width - 11, Height - 11)
        Invalidate()
    End Sub
    Public Property Items As String()
        Get
            Return __Items
            Invalidate()
        End Get
        Set(ByVal value As String())
            __Items = value
            lstbox.Items.Clear()
            Invalidate()
            lstbox.Items.AddRange(value)
            Invalidate()
        End Set
    End Property
    Public ReadOnly Property SelectedItem() As String
        Get
            Return lstbox.SelectedItem
        End Get
    End Property
    Sub DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles lstbox.DrawItem
        Try
            e.DrawBackground()
            e.Graphics.DrawString(lstbox.Items(e.Index).ToString(), _
            e.Font, New SolidBrush(lstbox.ForeColor), e.Bounds, StringFormat.GenericDefault)
            e.DrawFocusRectangle()
        Catch
        End Try
    End Sub
    Sub AddRange(ByVal Items As Object())
        lstbox.Items.Remove("")
        lstbox.Items.AddRange(Items)
        Invalidate()
    End Sub
    Sub AddItem(ByVal Item As Object)
        lstbox.Items.Add(Item)
        Invalidate()
    End Sub
    Sub NewListBox()
        lstbox.Size = New Size(Width - 8, Height - 8)
        lstbox.BorderStyle = BorderStyle.None
        lstbox.DrawMode = DrawMode.OwnerDrawVariable
        lstbox.Location = New Point(3, 3)
        lstbox.ForeColor = Color.White
        lstbox.BackColor = Color.FromArgb(43, 43, 43)
        lstbox.Items.Clear()
    End Sub

#End Region

    Sub New()
        MyBase.New()

        NewListBox()
        Controls.Add(lstbox)

        Size = New Size(128, 108)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New infDraw()
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        lstbox.Size = New Size(Width - 7, Height - 7)

        G.Clear(BackColor)

        Dim g1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(40, 40, 40), Color.FromArgb(45, 45, 45), 90S)
        G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2), 2))
        Dim g2 As New LinearGradientBrush(New Rectangle(0, Height / 2, Width - 1, Height / 2), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), 90S)
        G.FillPath(g2, d.RoundRect(New Rectangle(0, Height / 2 - 3, Width - 1, Height / 2 + 2), 2))

        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(10, 10, 10)), d.RoundRect(ClientRectangle, 2))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceComboBox : Inherits ComboBox
#Region " Control Help - Properties & Flicker Control "
    Private _StartIndex As Integer = 0
    Public Property StartIndex As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property
    Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(New SolidBrush(_highlightColor), e.Bounds) '37 37 37
            End If
            Using b As New SolidBrush(e.ForeColor)
                e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, b, e.Bounds)
            End Using
        Catch
        End Try
        e.DrawFocusRectangle()
        Invalidate()
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
    End Sub
    Private _highlightColor As Color = Color.FromArgb(128, 128, 128)
    Public Property ItemHighlightColor() As Color
        Get
            Return _highlightColor
        End Get
        Set(ByVal v As Color)
            _highlightColor = v
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        BackColor = Color.Transparent
        ForeColor = Color.White
        DropDownStyle = ComboBoxStyle.DropDownList
        DoubleBuffered = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New infDraw()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(BackColor)

        Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
        G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
        'G.FillRectangle(g1, ClientRectangle)
        Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
        G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
        'G.FillRectangle(h1, New Rectangle(0, 0, Width - 1, Height - 2))
        Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
        G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 1), 2))
        G.DrawLine(New Pen(Color.FromArgb(85, 83, 80)), Width - 18, 0, Width - 18, Height - 1)
        G.DrawLine(New Pen(Color.FromArgb(15, 13, 10)), Width - 19, 0, Width - 19, Height - 1)
        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        '                                      Far Point                Near Point                Middle Point
        DrawTriangle(Color.White, New Point(Width - 14, 12), New Point(Width - 7, 12), New Point(Width - 11, 15), G)
        DrawTriangle(Color.White, New Point(Width - 14, 9), New Point(Width - 7, 9), New Point(Width - 11, 6), G)

        Try
            G.DrawString(Text, Font, Brushes.White, New Rectangle(3, 0, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        Catch
        End Try

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class InfluenceCheckBox : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 16
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(145, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New infDraw()

        Dim checkBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.Clear(BackColor)

        Select Case Checked
            Case False
                Dim g1 As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(10, 10, 10), Color.FromArgb(16, 16, 16), 90S)
                G.FillPath(g1, d.RoundRect(checkBoxRectangle, 2))
                G.DrawPath(New Pen(Color.FromArgb(80, 97, 94, 90)), d.RoundRect(New Rectangle(1, 1, Height - 3, Height - 3), 2))
                G.DrawPath(Pens.Black, d.RoundRect(checkBoxRectangle, 2))
            Case True
                Dim g1 As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(10, 10, 10), Color.FromArgb(16, 16, 16), 90S)
                G.FillPath(g1, d.RoundRect(checkBoxRectangle, 2))
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, Height - 1, Height - 2), 2))
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Height - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, Height - 1, Height / 2 - 1), 2))
                G.DrawPath(New Pen(Color.FromArgb(80, 97, 94, 90)), d.RoundRect(New Rectangle(1, 1, Height - 3, Height - 3), 2))
                G.DrawPath(Pens.Black, d.RoundRect(checkBoxRectangle, 2))

                Dim chkPoly As Rectangle = New Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width \ 2, checkBoxRectangle.Height \ 2)
                Dim Poly() As Point = {New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), _
                               New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), _
                               New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
                If Checked Then
                    G.SmoothingMode = SmoothingMode.HighQuality
                    Dim P1 As New Pen(Color.FromArgb(250, 255, 255, 255), 2)
                    For i = 0 To Poly.Length - 2
                        G.DrawLine(P1, Poly(i), Poly(i + 1))
                    Next
                End If

        End Select

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(16, 1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class InfluenceRadioButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private R1 As Rectangle
    Private G1 As LinearGradientBrush

    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 16
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is InfluenceRadioButton Then
                DirectCast(C, InfluenceRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(150, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New infDraw()
        Dim radioBtnRectangle = New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(BackColor)

        Select Case Checked
            Case False
                Dim g1 As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(10, 10, 10), Color.FromArgb(16, 16, 16), 90S)
                G.FillEllipse(g1, radioBtnRectangle)
                G.DrawEllipse(New Pen(Color.FromArgb(80, 97, 94, 90)), New Rectangle(1, 1, Height - 3, Height - 3))
                G.DrawEllipse(Pens.Black, radioBtnRectangle)
            Case True
                Dim g1 As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(10, 10, 10), Color.FromArgb(16, 16, 16), 90S)
                G.FillEllipse(g1, radioBtnRectangle)
                Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
                G.FillEllipse(h1, radioBtnRectangle)
                Dim s1 As New LinearGradientBrush(New Rectangle(0, 1, Height - 1, Height / 2 - 1), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
                G.FillEllipse(s1, s1.Rectangle)

                G.FillEllipse(New SolidBrush(Color.FromArgb(15, 15, 15)), New Rectangle(4, 4, Height - 9, Height - 9))
                G.FillEllipse(New SolidBrush(Color.FromArgb(250, 255, 255, 255)), New Rectangle(5, 5, Height - 11, Height - 11))

                G.DrawEllipse(New Pen(Color.FromArgb(80, 97, 94, 90)), New Rectangle(1, 1, Height - 3, Height - 3))
                G.DrawEllipse(Pens.Black, radioBtnRectangle)
        End Select

        G.DrawString(Text, Font, Brushes.White, New Point(16, 0), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

Public Class InfluenceGroupBox : Inherits ContainerControl
#Region " Control Help - Properties & Flicker Control"
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
#End Region

    Sub New()
        MyBase.New()
        Size = New Size(200, 100)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighSpeed
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim TransparencyKey As Color = Me.ParentForm.TransparencyKey
        Dim d As New infDraw()
        MyBase.OnPaint(e)

        G.Clear(BackColor)

        G.FillPath(New SolidBrush(Color.FromArgb(20, 20, 20)), d.RoundRect(ClientRectangle, 2))


        Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
        Dim g1 As New LinearGradientBrush(New Rectangle(0, 2, Width - 1, 25), Color.FromArgb(40, 40, 40), Color.FromArgb(29, 29, 29), 90S)

        G.FillPath(g1, d.RoundRect(New Rectangle(0, 2, Width - 1, 25), 2))
        G.FillPath(h1, d.RoundRect(New Rectangle(0, 2, Width - 1, 25), 2))

        Dim s1 As New LinearGradientBrush(g1.Rectangle, Color.FromArgb(15, Color.White), Color.FromArgb(0, Color.White), 90S)
        G.FillRectangle(s1, New Rectangle(1, 1, Width - 1, 13))

        G.DrawLine(New Pen(Color.FromArgb(75, Color.White)), 1, 1, Width - 1, 1)

        G.DrawLine(New Pen(Color.FromArgb(18, 18, 18)), 1, 26, Width - 1, 26)

        G.DrawRectangle(New Pen(Color.FromArgb(37, 37, 37)), New Rectangle(1, 27, Width - 3, Height - 29))

        G.DrawPath(Pens.Black, d.RoundRect(ClientRectangle, 2))

        G.DrawString(Text, Font, Brushes.Black, New Rectangle(8, 8, Width - 1, 10), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        G.DrawString(Text, Font, Brushes.White, New Rectangle(8, 9, Width - 1, 11), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class InfluenceNumericUpDown : Inherits Control

#Region " Properties & Flicker Control "
    Private State As New MouseState
    Private X As Integer
    Private Y As Integer
    Private _Value As Long
    Private _Max As Long
    Private _Min As Long
    Private Typing As Boolean
    Public Property Value As Long
        Get
            Return _Value
        End Get
        Set(ByVal V As Long)
            If V <= _Max And V >= _Min Then _Value = V
            Invalidate()
        End Set
    End Property
    Public Property Maximum As Long
        Get
            Return _Max
        End Get
        Set(ByVal V As Long)
            If V > _Min Then _Max = V
            If _Value > _Max Then _Value = _Max
            Invalidate()
        End Set
    End Property
    Public Property Minimum As Long
        Get
            Return _Min
        End Get
        Set(ByVal V As Long)
            If V < _Max Then _Min = V
            If _Value < _Min Then _Value = _Min
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Y = e.Location.Y
        Invalidate()
        If e.X < Width - 23 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Default
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Me.Height = 22
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseClick(e)
        If X > Me.Width - 21 Then
            If Y < 10 Then
                If (Value + 1) <= _Max Then _Value += 1
            Else
                If (Value - 1) >= _Min Then _Value -= 1
            End If
        Else
            Typing = Not Typing
            Focus()
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        MyBase.OnKeyPress(e)
        Try
            If Typing Then _Value = CStr(CStr(_Value) & e.KeyChar.ToString)
        Catch ex As Exception : End Try
    End Sub
    Protected Overrides Sub OnKeyup(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyUp(e)
        If e.KeyCode = Keys.Up Then
            If (Value + 1) <= _Max Then _Value += 1
            Invalidate()
        ElseIf e.KeyCode = Keys.Down Then
            If (Value - 1) >= _Min Then _Value -= 1
        End If
        Invalidate()
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
    End Sub
#End Region

    Sub New()
        _Max = 9999999
        _Min = 0
        Cursor = Cursors.IBeam
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(BackColor)

        Dim d As New infDraw()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Color.FromArgb(20, 20, 20))

        Dim g1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 78, 75, 73), Color.FromArgb(125, 61, 59, 55), 90S)
        G.FillPath(g1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
        Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 31, 31, 31), Color.FromArgb(100, 36, 36, 36))
        G.FillPath(h1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 2))
        Dim s1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(35, Color.White), Color.FromArgb(0, Color.White), 90S)
        G.FillPath(s1, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 1), 2))
        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))

        ''Separator Lines
        G.DrawLine(New Pen(Color.FromArgb(10, 10, 10)), New Point(Width - 21, 0), New Point(Width - 21, Height))
        G.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), New Point(Width - 20, 1), New Point(Width - 20, Height - 3))
        G.DrawLine(New Pen(Color.FromArgb(10, 10, 10)), New Point(Width - 20, 10), New Point(Width, 10))
        G.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), New Point(Width - 19, 11), New Point(Width - 3, 11))

        'Top Triangle
        DrawTriangle(Color.White, New Point(Width - 14, 14), New Point(Width - 7, 14), New Point(Width - 11, 17.5), G)

        'Bottom Triangle
        DrawTriangle(Color.White, New Point(Width - 14, 7), New Point(Width - 7, 7), New Point(Width - 11, 3), G)

        G.DrawString(Value, Font, Brushes.White, New Point(5, 4))
        'G.DrawRectangle(New Pen(Color.FromArgb(70, 70, 70)), New Rectangle(1, 1, Width - 3, Height - 3))
        'G.DrawRectangle(New Pen(Color.FromArgb(30, 30, 30)), New Rectangle(0, 0, Width - 1, Height - 1))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class