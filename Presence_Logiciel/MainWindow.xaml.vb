Imports System.Threading
Imports System.Windows.Media.Color
Imports System.Windows.Media.Animation
Imports System.Windows.Media.LinearGradientBrush
Imports System.IO
Class MainWindow


    Private Sub fermer(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub




    Private Sub minimiser(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub

    Private Sub AffOrdreDuJour(sender As Object, e As RoutedEventArgs) Handles btnOrdreJour.Click
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecOrdreJour.Effect = objDropShadow
        Dim frmOrdre As New frmGestOrdJour
        frmOrdre.ShowDialog()
    End Sub
    Private Sub RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 7
        objDropShadow.BlurRadius = 5
        objDropShadow.Color = Colors.Black

        Me.RecOrdreJour.Effect = objDropShadow
        Me.RecActualite.Effect = objDropShadow
        Me.RecGroupe.Effect = objDropShadow
        Me.RecPdf.Effect = objDropShadow
        Me.RecProgramme.Effect = objDropShadow
        Me.RecCours.Effect = objDropShadow
        Me.RecAsso.Effect = objDropShadow
        Me.RecPret.Effect = objDropShadow

    End Sub

    Private Sub affProgramme(sender As Object, e As RoutedEventArgs) Handles btnProgramme.Click
        RemettreEffect()

        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecProgramme.Effect = objDropShadow
        Dim gestProgramme As New gestProgrammes
        gestProgramme.statut = lblStatut
        gestProgramme.ShowDialog()

    End Sub

    Private Sub AffCours(sender As Object, e As RoutedEventArgs) Handles btnCours.Click
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecCours.Effect = objDropShadow
        Dim gestCours As New gestCours

        gestCours.statut = lblStatut
        gestCours.ShowDialog()
    End Sub

    Private Sub AffGroupe(sender As Object, e As RoutedEventArgs) Handles btnGroupe.Click
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecGroupe.Effect = objDropShadow
        Dim gestMembre As New gestEtudiant
        gestMembre.statut = lblStatut
        gestMembre.ShowDialog()
    End Sub

    Private Sub AffAsso(sender As Object, e As RoutedEventArgs) Handles btnAsso.Click
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecAsso.Effect = objDropShadow
    End Sub

    Private Sub AffPdf(sender As Object, e As RoutedEventArgs) Handles btnPdf.Click
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecPdf.Effect = objDropShadow

        Dim FnPdf As New GestionPDF()
        FnPdf.PStatut = lblStatut
        FnPdf.ShowDialog()
    End Sub

    Private Sub AffActualite(sender As Object, e As RoutedEventArgs) Handles btnActualite.Click

        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecActualite.Effect = objDropShadow
        Dim gestActu As New rssActualite
        gestActu.statut = lblStatut
        gestActu.ShowDialog()
    End Sub

    Private Sub AffPret(sender As Object, e As RoutedEventArgs) Handles btnPret.Click

        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecPret.Effect = objDropShadow
        Dim gestPrets As New ListeExemplaire(lblStatut)
        gestPrets.ShowDialog()
    End Sub



    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub AffLstPret()

        Dim fnlistePret As New ListePret(lblStatut)
        fnlistePret.ShowDialog()

    End Sub

    Private Sub CreerPret()
        Dim fnPret As New Pret(lblStatut)
        fnPret.ShowDialog()

    End Sub

    Private Sub AffLstExemp()
        Dim fnLstExemp As New ListeExemplaire(lblStatut)
        fnLstExemp.ShowDialog()

    End Sub

    Private Sub AffLstDemande()
        ' Dim fnLstDemande As New ListeDemande(lblStatut)
        'fnLstDemande.ShowDialog()

    End Sub

    Private Sub CreerExemp()
        Dim fnExemp As New frmExemplaire(lblStatut)
        fnExemp.ShowDialog()
    End Sub


    Public Function verifier_int(ByVal entree As Integer)
        Dim Resultat As Boolean
        Resultat = IsNumeric(entree)
        Return Resultat
    End Function
    Public Function verifier_null(ByVal entree As String)
        If entree IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub message_box_validation(ByVal variable As String)
        MessageBox.Show("Le " + variable + " entré ne correspond pas aux critères établis")
    End Sub


    Public Sub changer_statut(ByVal statut As String)
        lblStatut.Content = statut
        Dim anim As Storyboard = FindResource("AnimLabel")
        anim.Begin(lblStatut)
    End Sub

    Private Sub OuvrirAPropos(sender As Object, e As RoutedEventArgs)
        Dim aPropos As New frmaPropos
        aPropos.ShowDialog()
    End Sub




    Private Sub CouleurBleu(sender As Object, e As RoutedEventArgs)
        Dim couleur = New SolidColorBrush(Color.FromRgb(194, 114, 23))
        Dim couleur2 = New SolidColorBrush(Color.FromRgb(23, 103, 194))
        Dim couleur3 = New SolidColorBrush(Color.FromRgb(23, 103, 194))
        Dim gradientBrush = New LinearGradientBrush(Color.FromRgb(23, 103, 194), Color.FromRgb(8, 7, 7), New Point(0.5, 0), New Point(0.5, 1))
        Dim gradientBrush2 = New LinearGradientBrush(Color.FromRgb(8, 7, 7), Color.FromRgb(23, 103, 194), New Point(0.5, 0), New Point(0.5, 1))
        rectMenu.Fill = couleur2
        rectBottom.Fill = gradientBrush
        MenuPrinc.Background = gradientBrush2
        menuBleu.Background = couleur2
        menuRouge.Background = couleur2
        menuCreerExemplaire.Background = couleur2
        menuCreerPret.Background = couleur2
        menuGris.Background = couleur2
        menuListeExemplaire.Background = couleur2
        menuListePret.Background = couleur2
        menuVert.Background = couleur2
        lblActualite.Foreground = Brushes.White
        lblAsso.Foreground = Brushes.White
        lblCours.Foreground = Brushes.White
        lblGestMembre.Foreground = Brushes.White
        lblGestPret.Foreground = Brushes.White
        lblOrdreJour.Foreground = Brushes.White
        lblPdf.Foreground = Brushes.White
        lblProgramme.Foreground = Brushes.White
        RecCours.Fill = couleur
        RecActualite.Fill = couleur
        RecAsso.Fill = couleur
        RecGroupe.Fill = couleur
        RecPdf.Fill = couleur
        RecOrdreJour.Fill = couleur
        RecPret.Fill = couleur
        RecProgramme.Fill = couleur
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Bleu", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)
    End Sub

    Private Sub CouleurGris(sender As Object, e As RoutedEventArgs)
        rectMenu.Fill = Brushes.DimGray
        rectBottom.Fill = Brushes.DimGray
        MenuPrinc.Background = Brushes.DimGray
        menuBleu.Background = Brushes.DimGray
        menuRouge.Background = Brushes.DimGray
        menuCreerExemplaire.Background = Brushes.DimGray
        menuCreerPret.Background = Brushes.DimGray
        menuGris.Background = Brushes.DimGray
        menuListeExemplaire.Background = Brushes.DimGray
        menuListePret.Background = Brushes.DimGray
        menuVert.Background = Brushes.DimGray
        lblActualite.Foreground = Brushes.White
        lblAsso.Foreground = Brushes.White
        lblCours.Foreground = Brushes.White
        lblGestMembre.Foreground = Brushes.White
        lblGestPret.Foreground = Brushes.White
        lblOrdreJour.Foreground = Brushes.White
        lblPdf.Foreground = Brushes.White
        lblProgramme.Foreground = Brushes.White
        RecCours.Fill = Brushes.Silver
        RecActualite.Fill = Brushes.Silver
        RecAsso.Fill = Brushes.Silver
        RecGroupe.Fill = Brushes.Silver
        RecPdf.Fill = Brushes.Silver
        RecOrdreJour.Fill = Brushes.Silver
        RecPret.Fill = Brushes.Silver
        RecProgramme.Fill = Brushes.Silver
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Gris", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)

    End Sub

    Private Sub CouleurRouge(sender As Object, e As RoutedEventArgs)
        rectMenu.Fill = Brushes.IndianRed
        rectBottom.Fill = Brushes.IndianRed
        MenuPrinc.Background = Brushes.DarkRed
        menuBleu.Background = Brushes.DarkRed
        menuRouge.Background = Brushes.DarkRed
        menuCreerExemplaire.Background = Brushes.DarkRed
        menuCreerPret.Background = Brushes.DarkRed
        menuGris.Background = Brushes.DarkRed
        menuListeExemplaire.Background = Brushes.DarkRed
        menuListePret.Background = Brushes.DarkRed
        menuVert.Background = Brushes.DarkRed
        RecCours.Fill = Brushes.Silver
        RecActualite.Fill = Brushes.Silver
        RecAsso.Fill = Brushes.Silver
        RecGroupe.Fill = Brushes.Silver
        RecPdf.Fill = Brushes.Silver
        RecOrdreJour.Fill = Brushes.Silver
        RecPret.Fill = Brushes.Silver
        RecProgramme.Fill = Brushes.Silver
        lblActualite.Foreground = Brushes.White
        lblAsso.Foreground = Brushes.White
        lblCours.Foreground = Brushes.White
        lblGestMembre.Foreground = Brushes.White
        lblGestPret.Foreground = Brushes.White
        lblOrdreJour.Foreground = Brushes.White
        lblPdf.Foreground = Brushes.White
        lblProgramme.Foreground = Brushes.White
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Rouge", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)
     

    End Sub

    Private Sub CouleurVert(sender As Object, e As RoutedEventArgs)
        rectMenu.Fill = Brushes.Khaki
        rectBottom.Fill = Brushes.DarkGreen
        MenuPrinc.Background = Brushes.DarkGreen
        menuBleu.Background = Brushes.DarkGreen
        menuRouge.Background = Brushes.DarkGreen
        menuCreerExemplaire.Background = Brushes.DarkGreen
        menuCreerPret.Background = Brushes.DarkGreen
        menuGris.Background = Brushes.DarkGreen
        menuListeExemplaire.Background = Brushes.DarkGreen
        menuListePret.Background = Brushes.DarkGreen
        menuVert.Background = Brushes.DarkGreen
        RecCours.Fill = Brushes.DarkKhaki
        RecActualite.Fill = Brushes.DarkKhaki
        RecAsso.Fill = Brushes.DarkKhaki
        RecGroupe.Fill = Brushes.DarkKhaki
        RecPdf.Fill = Brushes.DarkKhaki
        RecOrdreJour.Fill = Brushes.DarkKhaki
        RecPret.Fill = Brushes.DarkKhaki
        RecProgramme.Fill = Brushes.DarkKhaki
        lblActualite.Foreground = Brushes.Black
        lblAsso.Foreground = Brushes.Black
        lblCours.Foreground = Brushes.Black
        lblGestMembre.Foreground = Brushes.Black
        lblGestPret.Foreground = Brushes.Black
        lblOrdreJour.Foreground = Brushes.Black
        lblPdf.Foreground = Brushes.Black
        lblProgramme.Foreground = Brushes.Black
        File.SetAttributes("couleur.txt", FileAttributes.Normal)
        My.Computer.FileSystem.WriteAllText("couleur.txt", "Vert", False)
        File.SetAttributes("couleur.txt", FileAttributes.Hidden)
    End Sub

    Private Sub frmMain_Loaded(sender As Object, e As RoutedEventArgs) Handles frmMain.Loaded
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
End Class

Public Class FonctionsGlobales
    Private Main As MainWindow
    Public Function verifier_int(ByVal entree As Integer)
        Dim Resultat As Boolean
        Resultat = IsNumeric(entree)
        Return Resultat
    End Function
    Public Function verifier_null(ByVal entree As String)
        If entree IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub message_box_validation(ByVal variable As String)
        MessageBox.Show("Le " + variable + " entré ne correspond pas aux critères établis")
    End Sub


End Class