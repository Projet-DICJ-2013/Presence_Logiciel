Imports System.IO
Imports System.Windows.Media.Animation
Imports System.Windows.Media.LinearGradientBrush
Imports System.Windows.Media.Color






Public Class frmConnexion
    Dim test As New FctConnexion
    Dim DM As PresenceEntities
    Dim LesActualites As IQueryable(Of tblActualite)
    Dim vu As ListCollectionView
    Dim tim As System.Windows.Threading.DispatcherTimer


    ''Fonction servant à l'enctyption d'un string en MD5
#Region "functions"
    Private Function StringToMd5(ByRef Content As String) As String
        Dim M5 As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim ByteString() As Byte = System.Text.Encoding.ASCII.GetBytes(Content)
        ByteString = M5.ComputeHash(ByteString)
        Dim FinalString As String = Nothing

        For Each byt As Byte In ByteString
            FinalString &= byt.ToString("x2")

        Next
        Return FinalString.ToUpper()
    End Function

#End Region



    Private Function verifierLogin(ByVal user As String, ByVal password As String) As String
        Try
            Dim LesUser As IQueryable(Of tblLogin) = (From us In DM.tblLogin Where us.IdLogin = user Select us)



            If (LesUser.Count = 0) Then
                MsgBox("mauvaise informations de login")

                Return user
                Return password

            End If

            Dim TempUser As tblLogin = LesUser.First()
            Dim HPW As String = StringToMd5(password)

            If (TempUser.Hash = HPW) Then
                If (TempUser.Administrateur = False) Then
                    MsgBox("Vous n'avez pas les droits d'administrateur")
                    Me.Close()
                    Me.Finalize()
                    Return user
                    Return password

                Else
                    Dim main As New MainWindow
                    main.Show()
                    Me.Close()
                    Me.Finalize()
                    Return user
                    Return password
                End If


            End If
            MsgBox("mauvaise informations de login")

        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try

        Return user
        Return password
    End Function




    Private Sub Connexion(sender As Object, e As RoutedEventArgs) Handles btnconnexion.Click
        verifierLogin(txtLogin.Text, txtPassword.Password)



    End Sub

    Private Sub fermer(sender As Object, e As RoutedEventArgs)
        Me.Close()
        Me.Finalize()
    End Sub




    Private Sub minimiser(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub




    ''Actions effectués lorsque la souris est placé sur le logo
    Private Sub imglogo_MouseEnter(sender As Object, e As MouseEventArgs) Handles imglogo.MouseEnter

        ''Création d'une animation précédement déclaré dans le XAML
        Dim anim As Storyboard = FindResource("AnimTextBox")

        ''Ciblage de l'objet que nous voulont effectué l'animation
        anim.Begin(txtActualite)
        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)

        ''Déclaration d'un timer qui est utilisé pour le changement d'actualités
        If tim Is Nothing Then
            tim = New System.Windows.Threading.DispatcherTimer(New TimeSpan(0, 0, 0, 5, 0), Windows.Threading.DispatcherPriority.Normal, AddressOf affActuTim, Dispatcher)
            tim.Stop()
        End If
        tim.Start()
        If (vu.CurrentPosition = 0) Then

        End If


    End Sub

    ''Animations à chaque 5 seconde du début du timer
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

    ''Animations lors de la fermeture du logo (click)
    Private Sub imglogo_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles imglogo.MouseLeftButtonDown
        Dim anim As Storyboard = FindResource("AnimTextBox2")
        anim.Begin(txtActualite)
        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        tim.Stop()

    End Sub


    ''Sert à changer la position de l'actualité manuellement à l'aide du rectangle
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

    ''Sert à changer la position de l'actualité manuellement à l'aide du rectangle
    Private Sub rec2_MouseEnter(sender As Object, e As MouseEventArgs) Handles rec2.MouseEnter
        Try


            tim.Stop()
        Catch ex As Exception
            Dim erreur As Object = 0
        End Try
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

    ''Sert à changer la position de l'actualité manuellement à l'aide du rectangle
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

    ''Sert à changer la position de l'actualité manuellement à l'aide du rectangle
    Private Sub rec4_MouseEnter(sender As Object, e As MouseEventArgs) Handles rec4.MouseEnter
        Try
            tim.Stop()
        Catch ex As Exception

        End Try

        vu.MoveCurrentToLast()
        Dim anim As Storyboard = FindResource("AnimRecBlanc")
        Dim anim2 As Storyboard = FindResource("AnimRecBleu")

        anim.Begin(rec1)
        anim.Begin(rec2)
        anim.Begin(rec3)
        anim.Begin(rec4)
        anim2.Begin(rec4)
    End Sub

    ''Redémarrage du timer lorsque la souris quitte un des rectangle
    Private Sub rec1_MouseLeave(sender As Object, e As MouseEventArgs) Handles rec1.MouseLeave, rec2.MouseLeave, rec3.MouseLeave, rec4.MouseLeave
        Try
            tim.Start()
        Catch ex As Exception

        End Try

    End Sub


    ''Page_Load
    Private Sub ouverture(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesActualites = (From act In DM.tblActualite Select act)

        vu = New ListCollectionView(LesActualites.ToList())
        txtActualite.DataContext = vu

        tim = Nothing

        If (IO.File.Exists("couleur.txt")) Then

            If (IO.File.ReadAllText("couleur.txt") = "Vert") Then
                CouleurVert(sender, e)

            End If
            If (IO.File.ReadAllText("couleur.txt") = "Rouge") Then
                CouleurRouge(sender, e)
            End If
            If (IO.File.ReadAllText("couleur.txt") = "Gris") Then
                CouleurGris(sender, e)
            End If
            If (IO.File.ReadAllText("couleur.txt") = "Bleu") Then
                CouleurBleu(sender, e)
            End If
        Else

            My.Computer.FileSystem.WriteAllText("couleur.txt", "Bleu", False)
            File.SetAttributes("couleur.txt", FileAttributes.Hidden)
        End If
    End Sub


    Private Sub CouleurBleu(sender As Object, e As RoutedEventArgs)

        Dim couleur = New SolidColorBrush(Color.FromRgb(37, 88, 179))
        Dim gradientBrush = New LinearGradientBrush(Color.FromRgb(0, 0, 0), Color.FromRgb(194, 114, 23), New Point(0.5, 0), New Point(0.5, 1))
        Dim gradientBrush2 = New LinearGradientBrush(Color.FromRgb(8, 7, 7), Color.FromRgb(23, 103, 194), New Point(0.5, 0), New Point(0.5, 1))
        MainWindowFond.Background = gradientBrush
        MenuPrinc.Background = gradientBrush2
        btnconnexion.Background = couleur
        menuBleu.Background = couleur
        menuRouge.Background = couleur
        menuGris.Background = couleur
        menuVert.Background = couleur
        menuFermer.Background = couleur
        menuApropos.Background = couleur
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Bleu", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)
    End Sub

    Private Sub CouleurGris(sender As Object, e As RoutedEventArgs)
        Dim gradientBrush1 = New LinearGradientBrush(Color.FromRgb(31, 27, 27), Color.FromRgb(109, 109, 109), New Point(0.5, 0), New Point(0.5, 1))
        MainWindowFond.Background = gradientBrush1
        menuPrinc.Background = Brushes.DimGray
        btnconnexion.Background = Brushes.Silver
        menuBleu.Background = Brushes.DimGray
        menuRouge.Background = Brushes.DimGray
        menuGris.Background = Brushes.DimGray
        menuVert.Background = Brushes.DimGray
        menuFermer.Background = Brushes.DimGray
        menuApropos.Background = Brushes.DimGray

        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Gris", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)

    End Sub

    Private Sub CouleurRouge(sender As Object, e As RoutedEventArgs)
        Dim gradientBrush = New LinearGradientBrush(Color.FromRgb(220, 75, 75), Color.FromRgb(255, 129, 129), New Point(0.5, 0), New Point(0.5, 1))
        MainWindowFond.Background = gradientBrush
        menuPrinc.Background = Brushes.DarkRed
        btnconnexion.Background = Brushes.DarkRed
        menuBleu.Background = Brushes.DarkRed
        menuRouge.Background = Brushes.DarkRed
        menuGris.Background = Brushes.DarkRed
        menuVert.Background = Brushes.DarkRed
        menuFermer.Background = Brushes.DarkRed
        menuApropos.Background = Brushes.DarkRed
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Rouge", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)



    End Sub

    Private Sub CouleurVert(sender As Object, e As RoutedEventArgs)
        Dim gradientBrush = New LinearGradientBrush(Color.FromRgb(115, 110, 63), Color.FromRgb(240, 230, 140), New Point(0.5, 0), New Point(0.5, 1))
        MainWindowFond.Background = gradientBrush
        menuPrinc.Background = Brushes.DarkGreen
        btnconnexion.Background = Brushes.DarkGreen
        menuBleu.Background = Brushes.DarkGreen
        menuRouge.Background = Brushes.DarkGreen
        menuGris.Background = Brushes.DarkGreen
        menuVert.Background = Brushes.DarkGreen
        menuFermer.Background = Brushes.DarkGreen
        menuApropos.Background = Brushes.DarkGreen
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Vert", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)
    End Sub




End Class
