﻿
Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions


Class gestCours

    Dim DM As PresenceEntities
    Dim LesCours As IQueryable(Of tblCours)
    Dim vu As ListCollectionView
    Dim lecours As tblCours
    Dim tCours As IQueryable(Of tblCours)
    Dim NomStatutCours As String
    Dim CodeStatutCours As String
    Public statut As Label
    Dim StatutVisible As Boolean
    Private StatutChoisi As String

    ''Ce produit lors du click sur le boutton (+)     Permet l'ajout d'un cours
    Private Sub affAjoutCours(sender As Object, e As RoutedEventArgs)
        Dim uncours As New ajoutCour

        uncours.DM = DM
        uncours.statut = statut
        uncours.ShowDialog()
        LesCours = (From cours In DM.tblCours Select cours)
        vu = New ListCollectionView(LesCours.ToList())
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu

    End Sub


    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesCours = (From cours In DM.tblCours Select cours)

        vu = New ListCollectionView(LesCours.ToList())
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu



        afficherStatut()


    End Sub

    Private Sub UpdContext(Optional ByVal CurPos As Integer = 0)
        DM = New PresenceEntities
        LesCours = (From cours In DM.tblCours Select cours)

        vu = New ListCollectionView(LesCours.ToList())
        vu.MoveCurrentToPosition(CurPos)
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu



        afficherStatut()
    End Sub

    Private Sub AfficherSuivant(sender As Object, e As MouseButtonEventArgs) Handles btnSuivant.MouseDown
        If (vu.CurrentPosition < LesCours.Count() - 1) Then
            vu.MoveCurrentToNext()
            afficherStatut()

        End If
    End Sub

    Private Sub AfficherPrecedent(sender As Object, e As MouseButtonEventArgs) Handles btnPrecedent.MouseDown
        If (vu.CurrentPosition > 0) Then
            vu.MoveCurrentToPrevious()
            afficherStatut()
        End If
    End Sub

    Private Sub AfficherDernier(sender As Object, e As MouseButtonEventArgs) Handles btnDernier.MouseDown
        vu.MoveCurrentToLast()
        afficherStatut()


    End Sub

    Private Sub AfficherPremier(sender As Object, e As MouseButtonEventArgs) Handles btnPremier.MouseDown
        vu.MoveCurrentToFirst()
        afficherStatut()
    End Sub



    Private Sub SupprimerElement(sender As Object, e As MouseButtonEventArgs) Handles btnSupprimer.MouseDown

        Try
            Dim SupprTest As IQueryable(Of tblCours) = (From c In DM.tblCours Where c.CodeCours = txtNumCours.Text Select c)
            Dim CoursASupprimer As tblCours = SupprTest.First()
            DM.DeleteObject(CoursASupprimer)
            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer ce cours car il est actif à un programme de la session courrante")
        Catch ex As Exception
            statut.Content = "Impossible de supprimer ce cours car il est actif à un programme de la session courrante"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
 
        End Try
        UpdContext(vu.CurrentPosition)


    End Sub

    Private Sub Enregistrer(sender As Object, e As MouseButtonEventArgs) Handles btnEnregistrer.MouseDown

    Dim anim2 As Storyboard = FindResource("AnimTxtRouge")

        ''Vérification des données entrées

        Dim myRegex2 As New Regex( _
"\d-\d-\d")
        If (myRegex2.IsMatch(txtPonderation.Text) = False) Then
            statut.Content = "Une pondération de cours doit être : 1x(chiffre) 1x(-) 1x(chiffre) 1x(-) et 1x(chiffre) )"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)

            txtPonderation.BorderBrush = Brushes.Red
            anim2.Begin(txtPonderation)
            Return
        End If

        Dim myRegex3 As New Regex( _
"[1-3]")
        If (myRegex3.IsMatch(txtAnneeCours.Text) = False) Then
            statut.Content = "Une année de cours doit être entre : 1 et 3 )"
            Dim anim As Storyboard = FindResource("AnimLabel")
            txtAnneeCours.BorderBrush = Brushes.Red
            anim2.Begin(txtAnneeCours)

            anim.Begin(statut)
            Return
        End If

        Try

            ''Essai de modifier le cours sur la BD
            Dim modTest As IQueryable(Of tblCours) = (From c In DM.tblCours Where c.CodeCours = txtNumCours.Text Select c)
            Dim CoursAmodifier As tblCours = modTest.First()

            CoursAmodifier.AnneeCours = txtAnneeCours.Text
            CoursAmodifier.DescriptionCours = txtDescription.Text
            CoursAmodifier.NomCours = txtNomCours.Text
            CoursAmodifier.PonderationCours = txtPonderation.Text

            DM.SaveChanges()

            ''Si les rectangles de statut sont encore visible alors
            If (StatutVisible = True) Then
                CacherStatuts()
            End If

        Catch ex2 As System.Data.SqlClient.SqlException
            statut.Content = ex2.ToString
        Catch ex As Exception
            statut.Content = ex.ToString
        End Try

        txtAnneeCours.IsEnabled = False
        txtDescription.IsEnabled = False
        txtNomCours.IsEnabled = False
        txtPonderation.IsEnabled = False
        btnfontEnregistrer.Visibility = Windows.Visibility.Hidden
        btnEnregistrer.Visibility = Windows.Visibility.Hidden
        btnO.Visibility = Windows.Visibility.Hidden
        btnSuivant.IsEnabled = True
        btnPrecedent.IsEnabled = True
        btnPremier.IsEnabled = True
        btnDernier.IsEnabled = True


        ''Actualisation du statut
        afficherStatut()



    End Sub

    Private Sub ModifierCours(sender As Object, e As MouseButtonEventArgs) Handles btnEdit.MouseDown
        btnfontEnregistrer.Visibility = Windows.Visibility.Visible
        btnEnregistrer.Visibility = Windows.Visibility.Visible
        btnSuivant.IsEnabled = False
        btnPrecedent.IsEnabled = False
        btnPremier.IsEnabled = False
        btnDernier.IsEnabled = False

        NomStatutCours = lblStatutCours.Content
        CodeStatutCours = txtNumCours.Text

        btnO.Visibility = Windows.Visibility.Visible
        txtAnneeCours.IsEnabled = True
        txtDescription.IsEnabled = True
        txtNomCours.IsEnabled = True
        txtPonderation.IsEnabled = True
        StatutVisible = False



    End Sub

    Private Sub afficherStatut()
        Try

            lblDateAcquisition.DataContext = Nothing
            Dim eCours = (From l In DM.tblStatutCoursCours Where l.CodeCours = CType(vu.CurrentItem, tblCours).CodeCours Order By l.DateAcquisitionStatut Descending Select l)
            Dim leecour As tblStatutCoursCours
            leecour = eCours.FirstOrDefault

            lblStatutCours.DataContext = CType(vu.CurrentItem, tblCours).tblStatutCoursCours

            lblDateAcquisition.DataContext = CType(leecour, tblStatutCoursCours)
        Catch ex As Exception

            statut.Content = "Ce cours n'exise pas"
            Dim anim As Storyboard = FindResource("AnimLabel")
            Dim anim2 As Storyboard = FindResource("AnimTxtRouge")
            txtRecherche.BorderBrush = Brushes.Red

            anim.Begin(statut)
            anim2.Begin(txtRecherche)

        End Try

    End Sub

    Private Sub btnO_Click(sender As Object, e As RoutedEventArgs) Handles btnO.Click

        recActif.Visibility = Windows.Visibility.Visible
        recAnnulé.Visibility = Windows.Visibility.Visible
        recInactif.Visibility = Windows.Visibility.Visible
        If (recActif.Opacity = 0) Then
            Dim anim1 As Storyboard = FindResource("AnimRect1")
            Dim anim2 As Storyboard = FindResource("AnimRect2")
            Dim anim3 As Storyboard = FindResource("AnimRect3")

            anim1.Begin(recActif)
            anim1.Begin(lblActif)
            anim2.Begin(recAnnulé)
            anim2.Begin(lblAnnule)
            anim3.Begin(lblInactif)
            anim3.Begin(recInactif)
            StatutVisible = True

        Else
            Dim anim As Storyboard = FindResource("AnimRectFin")

            anim.Begin(recActif)
            anim.Begin(recAnnulé)
            anim.Begin(recInactif)
            anim.Begin(lblActif)
            anim.Begin(lblAnnule)
            anim.Begin(lblInactif)
            StatutVisible = False
        End If
    End Sub

    
    Private Sub recActif_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles recActif.MouseDown
        StatutChoisi = "Actif"
        ModifStatut()
        CacherStatuts()
        UpdContext(vu.CurrentPosition)
    End Sub

    Private Sub CacherStatuts()
        Dim anim As Storyboard = FindResource("AnimRectFin")
        Dim anim2 As Storyboard = FindResource("AnimLblFin")
        anim.Begin(recActif)
        anim.Begin(recAnnulé)
        anim.Begin(recInactif)
        anim2.Begin(lblActif)
        anim2.Begin(lblAnnule)
        anim2.Begin(lblInactif)
        recActif.Visibility = Windows.Visibility.Hidden
        recAnnulé.Visibility = Windows.Visibility.Hidden
        recInactif.Visibility = Windows.Visibility.Hidden
        StatutVisible = False
    End Sub

    Private Sub recInactif_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles recInactif.MouseDown
        StatutChoisi = "En attente"
        ModifStatut()
        CacherStatuts()
        UpdContext(vu.CurrentPosition)
    End Sub

    Private Sub recAnnulé_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles recAnnulé.MouseDown
        StatutChoisi = "Annulé"
        ModifStatut()
        CacherStatuts()
        UpdContext(vu.CurrentPosition)
    End Sub

    Protected Sub ModifStatut()

        Dim newStatut As tblStatutCoursCours
        newStatut = New tblStatutCoursCours With _
       {
           .CodeCours = txtNumCours.Text, _
           .DateAcquisitionStatut = DateTime.Now().ToString, _
           .NomStatutCours = StatutChoisi
       }

        CType(vu.CurrentItem, tblCours).tblStatutCoursCours.Add(newStatut)
        Try
            DM.SaveChanges()
        Catch ex As Exception
            statut.Content = "Erreur lors du changement du statut"
        End Try
    End Sub


    ''Vérifi si un des champ est vide

    Private Sub txtNomCours_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtNomCours.PreviewLostKeyboardFocus, txtAnneeCours.PreviewLostKeyboardFocus, txtPonderation.PreviewLostKeyboardFocus
        Dim objTextBox As TextBox = CType(sender, TextBox)
        Dim texte As String = objTextBox.Text
        If (texte = "") Then
            e.Handled = True
            statut.Content = "le champ : " + objTextBox.Name + " est vide"

            Dim anim As Storyboard = FindResource("AnimLabel")
            Dim anim2 As Storyboard = FindResource("AnimTxtRouge")
            objTextBox.BorderBrush = Brushes.Red

            anim.Begin(statut)
            anim2.Begin(objTextBox)
        End If
    End Sub

    Private Sub Rechercher(sender As Object, e As RoutedEventArgs) Handles btnRecherche.Click
        If (txtRecherche.Text = "") Then
            Return
        End If


        LesCours = (From cours In DM.tblCours Where cours.CodeCours = txtRecherche.Text Select cours)
        vu = Nothing
        vu = New ListCollectionView(LesCours.ToList())
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu
        afficherStatut()

    End Sub

    Private Sub txtRecherche_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtRecherche.PreviewLostKeyboardFocus
        If (txtRecherche.Text = "") Then
            LesCours = (From cours In DM.tblCours Select cours)
            vu = Nothing
            vu = New ListCollectionView(LesCours.ToList())
            txtNomCours.DataContext = vu
            txtAnneeCours.DataContext = vu
            txtDescription.DataContext = vu
            txtNumCours.DataContext = vu

            txtPonderation.DataContext = vu
            afficherStatut()
        End If
    End Sub


End Class
