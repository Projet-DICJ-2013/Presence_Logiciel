Imports System.Threading
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

        Dim FnPdf As New GestionPDF
        FnPdf.ShowDialog()
    End Sub

    Private Sub AffActualite(sender As Object, e As RoutedEventArgs) Handles btnActualite.Click

        'code servant au effet du bouton
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecActualite.Effect = objDropShadow
        Dim gestActu As New rssActualite
        gestActu.ShowDialog()
    End Sub

    Private Sub AffPret(sender As Object, e As RoutedEventArgs) Handles btnPret.Click
        'code servant au effet du bouton
        RemettreEffect()
        Dim objDropShadow As New Effects.DropShadowEffect
        objDropShadow.ShadowDepth = 0
        objDropShadow.BlurRadius = 0
        objDropShadow.Color = Colors.Transparent
        Me.RecPret.Effect = objDropShadow
        Dim gestPrets As New ListeExemplaire
        gestPrets.ShowDialog()
    End Sub



    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
    Private Sub AffLstPret()

        Dim fnlistePret As New ListePret
        fnlistePret.ShowDialog()

    End Sub

    Private Sub CreerPret()
        Dim fnPret As New Pret
        fnPret.ShowDialog()

    End Sub

    Private Sub AffLstExemp()
        Dim fnLstExemp As New ListeExemplaire
        fnLstExemp.ShowDialog()

    End Sub

    Private Sub CreerExemp()
        Dim fnExemp As New frmExemplaire
        fnExemp.ShowDialog()
    End Sub




End Class