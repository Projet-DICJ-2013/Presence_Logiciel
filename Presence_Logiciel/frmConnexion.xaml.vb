Imports System.Windows.Media.Animation
Imports System.Data
Imports System.Data.SqlClient




Public Class frmConnexion
    Dim test As New FctConnexion
    Dim DM As PresenceEntities
    Dim LesActualites As IQueryable(Of tblActualite)
    Dim vu As ListCollectionView
    Dim tim As System.Windows.Threading.DispatcherTimer
    Dim CptActu As Integer
    Dim IndexActu As String



    Private Sub enregistrerconnexion(sender As Object, e As RoutedEventArgs)
        'test.EncodingLogin(txtIp.Text, txtBD.Text, txtLogin.Text, txtPassword.Text)

    End Sub

    Private Sub Connexion(sender As Object, e As RoutedEventArgs) Handles btnconnexion.Click
        Try
            test.DecodingLogin(txtLogin.Text, txtPassword.Text)
        Catch ex1 As Exception
            MsgBox("Mauvaise information de login.")
            Exit Sub
        End Try


        Dim connectionString As String
        Dim SqlCon As New SqlClient.SqlConnection

        connectionString = test.GetConnectionString(txtLogin.Text, txtPassword.Text)

        SqlCon = New SqlConnection(connectionString)
        Try
            SqlCon.Open()
            MsgBox("reussi")

        Catch ex As Exception
            MsgBox("Erreur de connection ! ")
        End Try

    End Sub

    Private Sub fermer(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub




    Private Sub minimiser(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub

    Private Sub AfficherHeure(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesActualites = (From act In DM.tblActualite Select act)

        vu = New ListCollectionView(LesActualites.ToList())
        txtActualite.DataContext = vu

        tim = Nothing

    End Sub


    Private Sub imglogo_IsMouseDirectlyOverChanged(sender As Object, e As DependencyPropertyChangedEventArgs)

    End Sub

    Private Sub imglogo_MouseEnter(sender As Object, e As MouseEventArgs) Handles imglogo.MouseEnter
        CptActu = 0
        Dim anim As Storyboard = FindResource("AnimTextBox")
        anim.Begin(txtActualite)
        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        If tim Is Nothing Then
            tim = New System.Windows.Threading.DispatcherTimer(New TimeSpan(0, 0, 0, 5, 0), Windows.Threading.DispatcherPriority.Normal, AddressOf affActuTim, Dispatcher)
            tim.Stop()
        End If
        tim.Start()
        If (vu.CurrentPosition = 0) Then

        End If

    End Sub
    Private Sub affActuTim(sender As Object, e As EventArgs)
        Dim anim As Storyboard = FindResource("AnimRecBlanc")
        Dim anim2 As Storyboard = FindResource("AnimRecBleu")

        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)

        If (vu.CurrentPosition < LesActualites.Count() - 1) Then
            vu.MoveCurrentToNext()

        Else
            vu.MoveCurrentToFirst()
        End If

        Dim number As Integer = vu.CurrentPosition
        Select Case number
            Case 0
                anim2.Begin(rec1)
            Case 1
                anim2.Begin(rec2)
            Case 2
                anim2.Begin(rec3)
            Case 3
                anim2.Begin(rec4)
            Case Else
                Return

        End Select


    End Sub
    Private Sub imglogo_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles imglogo.MouseLeftButtonDown
        Dim anim As Storyboard = FindResource("AnimTextBox2")
        anim.Begin(txtActualite)
        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        tim.Stop()

    End Sub

    Private Sub rec1_MouseEnter(sender As Object, e As MouseEventArgs) Handles rec1.MouseEnter
        tim.Stop()
        vu.MoveCurrentToFirst()
        Dim anim As Storyboard = FindResource("AnimRecBlanc")
        Dim anim2 As Storyboard = FindResource("AnimRecBleu")

        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        anim2.Begin(rec1)
    End Sub

    Private Sub rec2_MouseEnter(sender As Object, e As MouseEventArgs) Handles rec2.MouseEnter
        tim.Stop()
        vu.MoveCurrentToFirst()
        vu.MoveCurrentToNext()
        Dim anim As Storyboard = FindResource("AnimRecBlanc")
        Dim anim2 As Storyboard = FindResource("AnimRecBleu")

        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        anim2.Begin(rec2)
    End Sub

    Private Sub rec3_MouseEnter(sender As Object, e As MouseEventArgs) Handles rec3.MouseEnter
        tim.Stop()
        vu.MoveCurrentToLast()
        vu.MoveCurrentToPrevious()
        Dim anim As Storyboard = FindResource("AnimRecBlanc")
        Dim anim2 As Storyboard = FindResource("AnimRecBleu")

        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        anim2.Begin(rec3)

    End Sub

    Private Sub rec4_MouseEnter(sender As Object, e As MouseEventArgs) Handles rec4.MouseEnter
        tim.Stop()
        vu.MoveCurrentToLast()
        Dim anim As Storyboard = FindResource("AnimRecBlanc")
        Dim anim2 As Storyboard = FindResource("AnimRecBleu")

        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        anim2.Begin(rec4)
    End Sub

    Private Sub rec1_MouseLeave(sender As Object, e As MouseEventArgs) Handles rec1.MouseLeave, rec2.MouseLeave, rec3.MouseLeave, rec4.MouseLeave
        tim.Start()
    End Sub
End Class
