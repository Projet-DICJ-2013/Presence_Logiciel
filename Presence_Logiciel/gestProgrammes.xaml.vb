﻿Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class gestProgrammes
    Dim DM As PresenceEntities
    Dim LesProgrammes As IQueryable(Of tblProgramme)
    Dim vu As ListCollectionView
    Dim codeprog As String
    Public statut As Label




    Private Sub AffProgramme(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesProgrammes = (From prog In DM.tblProgramme Select prog)
        vu = New ListCollectionView(LesProgrammes.ToList())
        txtNumProgramme.DataContext = vu
        txtNomProgramme.DataContext = vu
        txtObjectif.DataContext = vu
        lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours



    End Sub




    Private Sub ChargerListecours(sender As Object, e As TextChangedEventArgs) Handles txtNumProgramme.TextChanged
        Dim lstProg = (From p In DM.tblProgramme Select p Where p.CodeProg = txtNumProgramme.Text)
        Dim LeProg As tblProgramme = lstProg.First
        lvCours.ItemsSource = CType(LeProg, tblProgramme).tblCours


    End Sub







    Private Sub AfficherSuivant(sender As Object, e As RoutedEventArgs) Handles btnSuivant.Click
        If (vu.CurrentPosition < LesProgrammes.Count() - 1) Then
            vu.MoveCurrentToNext()
        End If
    End Sub

    Private Sub affAjoutCours(sender As Object, e As RoutedEventArgs)
        Dim uncours As New LierCoursProgramme
        codeprog = txtNumProgramme.Text
        uncours.DM = DM
        uncours.codeprog = codeprog
        uncours.ShowDialog()
    End Sub

    Private Sub btnPremier_Click(sender As Object, e As RoutedEventArgs) Handles btnPremier.Click
        vu.MoveCurrentToFirst()
    End Sub

    Private Sub btnPrecedent_Click(sender As Object, e As RoutedEventArgs) Handles btnPrecedent.Click
        If (vu.CurrentPosition > 0) Then
            vu.MoveCurrentToPrevious()
        End If
    End Sub

    Private Sub btnDernier_Click(sender As Object, e As RoutedEventArgs) Handles btnDernier.Click
        vu.MoveCurrentToLast()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        btnFontEnregistrer.Visibility = Windows.Visibility.Visible
        btnEnregistrer.Visibility = Windows.Visibility.Visible
        txtNomProgramme.IsEnabled = True
        txtObjectif.IsEnabled = True
    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As RoutedEventArgs) Handles btnSupprimer.Click
        Try
            Dim SupprTest As IQueryable(Of tblProgramme) = (From p In DM.tblProgramme Where p.CodeProg = txtNumProgramme.Text Select p)
            Dim CoursASupprimer As tblProgramme = SupprTest.First()

            DM.DeleteObject(CoursASupprimer)
            DM.SaveChanges()

        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer ce programme car il a des cours attribué")
        Catch ex As Exception
            statut.Content = "Impossible de supprimer ce programme car il a des cours attribué"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)

            'Dit l 'erreur
        End Try



        LesProgrammes = (From prog In DM.tblProgramme Select prog)
        vu = New ListCollectionView(LesProgrammes.ToList())
        txtNumProgramme.DataContext = vu
        txtNomProgramme.DataContext = vu
        txtObjectif.DataContext = vu
        lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours
    End Sub

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)
        ''Initialisation de la forme
        AffProgramme(sender, e)
    End Sub

 

    '' Apparition de la forme d'ajout de programme lors du clic sur le bouton (+)
    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click
        Dim ajProg As New ajoutProgramme
        ajProg.DM = DM
        ajProg.statut = statut
        ajProg.ShowDialog()
        LesProgrammes = (From prog In DM.tblProgramme Select prog)
        vu = New ListCollectionView(LesProgrammes.ToList())
        txtNumProgramme.DataContext = vu
        txtNomProgramme.DataContext = vu
        txtObjectif.DataContext = vu
        lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours
    End Sub

    ''Apparition de la forme pour ajouter des cours au programme
    Private Sub btnLier_Click(sender As Object, e As RoutedEventArgs) Handles btnLier.Click
        Dim uncours As New LierCoursProgramme
        codeprog = txtNumProgramme.Text
        uncours.DM = DM
        uncours.codeprog = codeprog
        uncours.ShowDialog()
    End Sub

    Private Sub btnEnregistrer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnregistrer.Click
        txtNomProgramme.IsEnabled = False
        txtObjectif.IsEnabled = False
        btnFontEnregistrer.Visibility = Windows.Visibility.Hidden
        btnEnregistrer.Visibility = Windows.Visibility.Hidden



        ''Début des modifications
        Try
            Dim modTest As IQueryable(Of tblProgramme) = (From p In DM.tblProgramme Where p.CodeProg = txtNumProgramme.Text Select p)
            Dim CoursAmodifier As tblProgramme = modTest.First()
            CoursAmodifier.CodeProg = txtNumProgramme.Text
            CoursAmodifier.NomProg = txtNomProgramme.Text
            CoursAmodifier.ObjectifProg = txtObjectif.Text


            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            statut.Content = "Impossible de supprimer ce cours car il est actif à un programme de la session courrante"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            'Dit l 'erreur
        End Try
    End Sub

    ''Se produit si un champs modifié est vide
    Private Sub txtNomProgramme_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtNomProgramme.PreviewLostKeyboardFocus
        Dim objTextBox As TextBox = CType(sender, TextBox)
        Dim texte As String = objTextBox.Text
        If (texte = "") Then
            e.Handled = True
            statut.Content = "le champ : " + objTextBox.Name + " est vide"

            Dim anim As Storyboard = FindResource("AnimLabel")

            Dim anim2 As Storyboard = FindResource("AnimTxtRouge")
            anim.Begin(statut)
            objTextBox.BorderBrush = Brushes.Red

            anim2.Begin(objTextBox)
        End If



    End Sub

    Private Sub btnRechercher_Click(sender As Object, e As RoutedEventArgs) Handles btnRechercher.Click
        If (txtRecherche.Text = "") Then
            Return
        End If
        txtNomProgramme.Text = ""
        txtObjectif.Text = ""
        Try

            LesProgrammes = (From prog In DM.tblProgramme Where prog.CodeProg = txtRecherche.Text Select prog)
            vu = Nothing
     
            vu = New ListCollectionView(LesProgrammes.ToList())
            txtNumProgramme.DataContext = vu
            txtNomProgramme.DataContext = vu
            txtObjectif.DataContext = vu
            lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours
        Catch ex As Exception
            statut.Content = "Ce programme ne figure pas dans la liste"
            Dim anim2 As Storyboard = FindResource("AnimTxtRouge")
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            txtRecherche.BorderBrush = Brushes.Red
            anim2.Begin(txtRecherche)
        End Try

    End Sub

    Private Sub txtRecherche_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtRecherche.PreviewLostKeyboardFocus
        If (txtRecherche.Text = "") Then

            LesProgrammes = (From prog In DM.tblProgramme Select prog)
            vu = Nothing
            txtNomProgramme.Text = ""
            txtObjectif.Text = ""
            vu = New ListCollectionView(LesProgrammes.ToList())
            txtNumProgramme.DataContext = vu
            txtNomProgramme.DataContext = vu
            txtObjectif.DataContext = vu
            lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours
        End If
    End Sub
End Class
