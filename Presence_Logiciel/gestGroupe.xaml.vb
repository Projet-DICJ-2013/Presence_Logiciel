Imports System.Linq
Imports System.IO
Imports System
Imports System.Collections.Generic


Class frmGestGroupe

    Dim BD As PresenceEntities

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        BD = New PresenceEntities
        'Pompage de données dans les tables des programmes pour pouvoir les afficher
        Dim _afficherProgramme = (From _programme In BD.tblProgramme Select _programme)
        'Afficher les données dans les liste view
        Try
            cmbProgramme.ItemsSource = _afficherProgramme
        Catch ex As Exception
        End Try

        'Désactiver les radio boutons des années tant qu'on a pas choisi le programme
        btnEnregistrer.IsEnabled = False
        btnajouteretu.IsEnabled = False
        btnsupprimeretu.IsEnabled = False
        ActiverDesactiver(False)
    End Sub

    Private Sub cmbProgramme_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbProgramme.SelectionChanged
        'Pompage de données dans les tables des sessions pour pouvoir les afficher
        Dim _afficherSession = (From _session In BD.tblCoursSessionGroupe Select _session).Distinct

        'Afficher les données dans les liste view
        Try
            cmbSession.ItemsSource = _afficherSession
        Catch ex As Exception
        End Try

        'Activation des radio bouton après la sélection d'un programme
        lstvEtudiants.ItemsSource = Nothing
        cmbProgramme.IsEnabled = False
        ActiverDesactiver(True)
    End Sub

    Private Sub cmbSession_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbSession.SelectionChanged
        If (cmbSession.SelectedItem IsNot Nothing) Then
            'Pompage de données de cours selon la session
            Dim _afficherCours = (From _cours In CType(cmbProgramme.SelectedItem, tblProgramme).tblCours Where _cours.tblCoursSessionGroupe.Contains(CType(cmbSession.SelectedItem, tblCoursSessionGroupe)) Select _cours)
            'Affichage des données
            cmbCours.ItemsSource = Nothing
            cmbCours.ItemsSource = _afficherCours
        End If

    End Sub

    Private Sub cmbCours_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbCours.SelectionChanged
        If (cmbCours.SelectedItem IsNot Nothing) Then
            'Pompage de données de groupe selon la session et le cours
            Dim _afficherGroupe = From _g In CType(cmbCours.SelectedItem, tblCours).tblCoursSessionGroupe Where _g.IdSession = CType(cmbSession.SelectedItem, tblCoursSessionGroupe).IdSession Select _g.tblGroupe
            'Affichage des données
            cmbGroupe.ItemsSource = Nothing
            cmbGroupe.ItemsSource = _afficherGroupe
        End If
    End Sub

    Private Sub cmbGroupe_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbGroupe.SelectionChanged
        'Pompage de données des étudiants selon la session, le cours et le groupe
        If (cmbGroupe.SelectedItem IsNot Nothing) Then
            Dim _afficherEtudiants = (From _e In CType(cmbGroupe.SelectedItem, tblGroupe).tblEtudiant Where _e.tblGroupe.Contains(CType(cmbGroupe.SelectedItem, tblGroupe)) Select _e.tblMembre)
            'Pompage des données
            lstvGroupe.ItemsSource = Nothing
            lstvGroupe.ItemsSource = _afficherEtudiants

            btnajouteretu.IsEnabled = True
            btnsupprimeretu.IsEnabled = True


        End If
    End Sub

    Private Sub rdb1_Checked(sender As Object, e As RoutedEventArgs) Handles rdb1.Checked
        AfficherEtu(1)
    End Sub

    Private Sub rdb2_Checked(sender As Object, e As RoutedEventArgs) Handles rdb2.Checked
        AfficherEtu(2)
    End Sub

    Private Sub rdb3_Checked(sender As Object, e As RoutedEventArgs) Handles rdb3.Checked
        AfficherEtu(3)
    End Sub

    Private Sub rdbautre_Checked(sender As Object, e As RoutedEventArgs) Handles rdbautre.Checked
        AfficherEtu(4)
    End Sub

    Private Sub rdbTous_Checked(sender As Object, e As RoutedEventArgs) Handles rdbTous.Checked
        AfficherEtu(0)
    End Sub

    Sub ActiverDesactiver(valeur As Boolean)
        'Activation ou Désactivation des controles suivant
        rdb1.IsEnabled = valeur
        rdb2.IsEnabled = valeur
        rdb3.IsEnabled = valeur
        rdbautre.IsEnabled = valeur
        rdbTous.IsEnabled = valeur
        cmbSession.IsEnabled = valeur
        cmbCours.IsEnabled = valeur
        cmbGroupe.IsEnabled = valeur
    End Sub

    Sub AfficherEtu(valeur As Int16)
        'initialiser _afficherEtu
        Dim _afficherEtu = Nothing

        'Pompe les données en fonction de l'option choisi par l'utilisateur
        If valeur > 0 And valeur <= 3 Then
            _afficherEtu = From _etu In CType(cmbProgramme.SelectedItem, tblProgramme).tblEtudiant Where _etu.Annee = valeur Select _etu.tblMembre
        ElseIf valeur > 3 Then
            _afficherEtu = From _etu In CType(cmbProgramme.SelectedItem, tblProgramme).tblEtudiant Where _etu.Annee > 3 Select _etu.tblMembre
        ElseIf valeur = 0 Then
            _afficherEtu = From _etu In CType(cmbProgramme.SelectedItem, tblProgramme).tblEtudiant Select _etu.tblMembre
        End If

        'Affiche les données demandés par l'utilisateur
        lstvEtudiants.ItemsSource = Nothing
        lstvEtudiants.ItemsSource = _afficherEtu

    End Sub

    Private Sub lstvGroupe_Drop(sender As Object, e As DragEventArgs) Handles lstvGroupe.Drop
        'Enregistre les données à déplacer
        Dim donnee = CType(e.Data.GetData("chose"), tblMembre)
        'Si la donnée existe deja dans la liste groupe ne pas l'ajouter
        Try
            If CType(lstvGroupe.ItemsSource, IEnumerable(Of tblMembre)).Contains(donnee) = False Then
                'sinon l'ajouter
                AjouterEtu()
            Else
                'on annule le drag and drop
                e.Effects = DragDropEffects.None
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub lstvEtudiants_Drop(sender As Object, e As DragEventArgs) Handles lstvEtudiants.Drop
        SupprimerEtu()
    End Sub

    Private Sub List_MouseMove(sender As Object, e As MouseEventArgs)
        BougerSouris(sender, e)
    End Sub

    Private Sub List_MouseMove2(sender As Object, e As MouseEventArgs)
        BougerSouris(sender, e)
    End Sub

    Sub BougerSouris(sender As Object, e As MouseEventArgs)
        Try
            If e.LeftButton Then
                Dim _p = lstvEtudiants.SelectedItem
                Dim donnee = New DataObject

                donnee.SetData("chose", _p)

                Dim _q = DragDrop.DoDragDrop(sender, donnee, DragDropEffects.Move)

                If _q = DragDropEffects.Move Then
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    Private Sub lstvEtudiants_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstvEtudiants.MouseDoubleClick
        AjouterEtu()
    End Sub

    Private Sub lstvGroupe_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstvGroupe.MouseDoubleClick
        SupprimerEtu()
    End Sub

    Private Sub btnajouteretu_Click(sender As Object, e As RoutedEventArgs) Handles btnajouteretu.Click
        AjouterEtu()
    End Sub

    Private Sub btnsupprimeretu_Click(sender As Object, e As RoutedEventArgs) Handles btnsupprimeretu.Click
        SupprimerEtu()
    End Sub

    Sub AjouterEtu()
        'Ajouter un Etudiant dans un groupe avec le double-click
        Dim donnee = CType(lstvEtudiants.SelectedItem, tblMembre)
        CType(cmbGroupe.SelectedItem, tblGroupe).tblEtudiant.Add(donnee.tblEtudiant.First())
        'Affichage de la modification
        lstvGroupe.ItemsSource = Nothing
        lstvGroupe.ItemsSource = From _m In CType(cmbGroupe.SelectedItem, tblGroupe).tblEtudiant Select _m.tblMembre

        btnEnregistrer.IsEnabled = True
        lstvEtudiants.AllowDrop = True
        lstvGroupe.AllowDrop = True
    End Sub

    Sub SupprimerEtu()
        'Supprimer un Etudiant dans le groupe avec le double-click
        Try
            Dim donnee = CType(lstvGroupe.SelectedItem, tblMembre)
            CType(cmbGroupe.SelectedItem, tblGroupe).tblEtudiant.Remove(donnee.tblEtudiant.First())
            'Affichage de cette modification
            lstvGroupe.ItemsSource = Nothing
            lstvGroupe.ItemsSource = From _m In CType(cmbGroupe.SelectedItem, tblGroupe).tblEtudiant Select _m.tblMembre
        Catch ex As Exception
        End Try
        btnEnregistrer.IsEnabled = True
        lstvEtudiants.AllowDrop = True
        lstvGroupe.AllowDrop = True
    End Sub

    Private Sub btnEnregistrer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnregistrer.Click
        'validation de l'enregistrement
        Dim _reponse = MessageBox.Show("Voulez-vous vraiment enregistrer les modifications à ce groupe", "Enregistrement", MessageBoxButton.YesNo)
        If _reponse = 6 Then
            'Enregistrement envoyé à la BD
            BD.SaveChanges()
            MessageBox.Show("Votre modification a bien été enregistré", "Succès", MessageBoxButton.OK)
            Me.Close()
        Else
            Return
        End If
    End Sub

    Private Sub btncreergroupe1_Click(sender As Object, e As RoutedEventArgs) Handles btncreergroupe1.Click
        'Creer un groupe
        Dim ngroupe As New creerGroupe
        ngroupe.Forme = BD
        ngroupe.ShowDialog()
    End Sub

    Public Sub refresh()
        'Videngage des listes
        lstvEtudiants.ItemsSource = Nothing
        lstvGroupe.ItemsSource = Nothing

        BD.AcceptAllChanges()
        'Décocher et remettre a rien les sélection d'une précédente modification
        cmbGroupe.SelectedItem = Nothing
        cmbCours.SelectedItem = Nothing
        cmbSession.SelectedItem = Nothing
        cmbProgramme.SelectedItem = Nothing

        rdb1.IsChecked = False
        rdb2.IsChecked = False
        rdb3.IsChecked = False
        rdbautre.IsChecked = False
        rdbTous.IsChecked = False

        cmbProgramme.IsEnabled = True
        ActiverDesactiver(False)
    End Sub

    Private Sub btnnouvellemodification_Click(sender As Object, e As RoutedEventArgs) Handles btnnouvellemodification.Click
        refresh()
    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Finalize()
        Me.Close()
    End Sub
End Class