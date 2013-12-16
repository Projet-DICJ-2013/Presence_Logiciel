Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class gestEtudiant

    Dim DM As PresenceEntities
    Dim LesMembres As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView
    Dim lemembre As tblMembre
    Dim tProf As IQueryable(Of tblMembre)
    Dim leprof As tblMembre
    Public statut As Label



    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesMembres = (From m In DM.tblMembre Select m)
        vu = New ListCollectionView(LesMembres.ToList())
        txtAdresse.DataContext = vu
        txtCourriel.DataContext = vu
        txtIdMembre.DataContext = vu
        txtNoCivique.DataContext = vu
        txtNomMembre.DataContext = vu
        txtPrenomMembre.DataContext = vu
        txtTelephoneMembre.DataContext = vu
        txtVille.DataContext = vu
        afficherTypeMembre()




    End Sub
    Private Sub afficherTypeMembre()

        tProf = (From pro In DM.tblMembre Where pro.IdMembre = CType(vu.CurrentItem, tblMembre).IdMembre Select pro)
        txtCourrielProfesseur.DataContext = Nothing
        txtNoBureau.DataContext = Nothing
        txtPosteTelephone.DataContext = Nothing
        txtChargeTravail.DataContext = Nothing
        txtAnnee.DataContext = Nothing
        txtDaEtudiant.DataContext = Nothing
        txtDateInscription.DataContext = Nothing
        leprof = tProf.First()
        txtNoBureau.DataContext = CType(leprof, tblMembre).tblProfesseur
        txtPosteTelephone.DataContext = CType(leprof, tblMembre).tblProfesseur
        txtCourrielProfesseur.DataContext = CType(leprof, tblMembre).tblProfesseur
        txtChargeTravail.DataContext = CType(leprof, tblMembre).tblProfesseur
        txtAnnee.DataContext = CType(leprof, tblMembre).tblEtudiant
        txtDaEtudiant.DataContext = CType(leprof, tblMembre).tblEtudiant
        txtDateInscription.DataContext = CType(leprof, tblMembre).tblEtudiant

        TabTypeMembre.IsEnabled = True
        SelectTab()




    End Sub

    Private Sub btnSuivant_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnSuivant.MouseDown
        If (vu.CurrentPosition < LesMembres.Count() - 1) Then
            vu.MoveCurrentToNext()
            afficherTypeMembre()
        End If
    End Sub

    Private Sub btnPrecedent_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnPrecedent.MouseDown
        If (vu.CurrentPosition > 0) Then
            vu.MoveCurrentToPrevious()
            afficherTypeMembre()

        End If
    End Sub

    Private Sub btnDernier_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnDernier.MouseDown
        vu.MoveCurrentToLast()
        afficherTypeMembre()
    End Sub

    Private Sub btnPremier_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnPremier.MouseDown
        vu.MoveCurrentToFirst()
        afficherTypeMembre()
    End Sub

    Private Sub btnAdd_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnAdd.MouseDown
        Dim unmembre As New ajoutMembre

        unmembre.statut = statut
        unmembre.DM = DM
        unmembre.ShowDialog()
        LesMembres = (From m In DM.tblMembre Select m)

        txtAdresse.DataContext = vu
        txtCourriel.DataContext = vu
        txtIdMembre.DataContext = vu
        txtNoCivique.DataContext = vu
        txtNomMembre.DataContext = vu
        txtPrenomMembre.DataContext = vu
        txtTelephoneMembre.DataContext = vu
        txtVille.DataContext = vu


    End Sub

    Private Sub btnEdit_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnEdit.MouseDown
        btnfontEnregistrer.Visibility = Windows.Visibility.Visible
        btnEnregistrer.Visibility = Windows.Visibility.Visible
        txtAdresse.IsEnabled = True
        txtCourriel.IsEnabled = True
        txtNoCivique.IsEnabled = True
        txtNomMembre.IsEnabled = True
        txtPrenomMembre.IsEnabled = True
        txtTelephoneMembre.IsEnabled = True
        txtVille.IsEnabled = True
        txtCourrielProfesseur.IsEnabled = True
        txtNoBureau.IsEnabled = True
        txtPosteTelephone.IsEnabled = True
        txtChargeTravail.IsEnabled = True
        TabTypeMembre.IsEnabled = True
        txtDateInscription.IsEnabled = True

    End Sub

    Private Sub btnEnregistrer_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnEnregistrer.MouseDown
        btnfontEnregistrer.Visibility = Windows.Visibility.Hidden
        btnEnregistrer.Visibility = Windows.Visibility.Hidden
        txtAdresse.IsEnabled = False
        txtCourriel.IsEnabled = False
        txtNoCivique.IsEnabled = False
        txtNomMembre.IsEnabled = False
        txtPrenomMembre.IsEnabled = False
        txtTelephoneMembre.IsEnabled = False
        txtVille.IsEnabled = False
        txtCourrielProfesseur.IsEnabled = False
        txtNoBureau.IsEnabled = False
        txtPosteTelephone.IsEnabled = False
        txtChargeTravail.IsEnabled = False
        txtDateInscription.IsEnabled = False
        TabTypeMembre.IsEnabled = False

        '        Dim myRegex1 As New Regex( _
        '"^[a-zA-Z]+$")
        '        If (myRegex1.IsMatch(txtPrenomMembre.Text) = False) Then
        '            statut.Content = "Un prénom doit contenir que des lettres"
        '            Dim anim As Storyboard = FindResource("AnimLabel")

        '            anim.Begin(statut)
        '            Return
        '        End If

        '        If (myRegex1.IsMatch(txtNomMembre.Text) = False) Then
        '            statut.Content = "Un nom doit contenir que des lettres"
        '            Dim anim As Storyboard = FindResource("AnimLabel")
        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex2 As New Regex( _
        '"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
        '        If (myRegex2.IsMatch(txtTelephoneMembre.Text) = False) Then
        '            statut.Content = "La structure du numéro de téléphone est mauvaise"
        '            Dim anim As Storyboard = FindResource("AnimLabel")
        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex3 As New Regex( _
        '"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        '        If (myRegex3.IsMatch(txtCourriel.Text) = False) Then
        '            statut.Content = "L'email entré est invalide"
        '            Dim anim As Storyboard = FindResource("AnimLabel")
        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex4 As New Regex( _
        '"^\d{1,5}$")
        '        If (myRegex4.IsMatch(txtNoCivique.Text) = False) Then
        '            statut.Content = "Un numéro civique doit comprendre au moin 1 chiffre et aucune lettre"
        '            Dim anim As Storyboard = FindResource("AnimLabel")
        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex5 As New Regex( _
        '"\d{7,9}")
        '        If (myRegex5.IsMatch(txtDaEtudiant.Text) = False) Then
        '            statut.Content = "Un numéro de DA doit comporter entre 7 et 9 chiffres"
        '            Dim anim As Storyboard = FindResource("AnimLabel")

        '            anim.Begin(statut)
        '            Return
        '        End If


        '        Dim myRegex6 As New Regex( _
        '"[1-2][2-4]")
        '        If (myRegex6.IsMatch(txtChargeTravail.Text) = False) Then
        '            statut.Content = "La charge de travail d'un professeur doit être située entre 12 et 25 heures"
        '            Dim anim As Storyboard = FindResource("AnimLabel")

        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex7 As New Regex( _
        '"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        '        If (myRegex7.IsMatch(txtCourrielProfesseur.Text) = False) Then
        '            statut.Content = "Le courriel est invalide"
        '            Dim anim As Storyboard = FindResource("AnimLabel")

        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex8 As New Regex( _
        '"\d{3}.\d")
        '        If (myRegex8.IsMatch(txtNoBureau.Text) = False) Then
        '            statut.Content = "Un numéro de bureau doit être structuré 3x(chiffre) 1x(.) et 1x(chiffre)"
        '            Dim anim As Storyboard = FindResource("AnimLabel")

        '            anim.Begin(statut)
        '            Return
        '        End If

        '        Dim myRegex9 As New Regex( _
        '"\d{1,4}")
        '        If (myRegex9.IsMatch(txtPosteTelephone.Text) = False) Then
        '            statut.Content = "Un numéro de poste doit comprendre entre 1 et 4 chiffre "
        '            Dim anim As Storyboard = FindResource("AnimLabel")

        '            anim.Begin(statut)
        '            Return
        '        End If

        Try
            Dim modTest As IQueryable(Of tblMembre) = (From m In DM.tblMembre Where m.IdMembre = txtIdMembre.Text Select m)
            Dim MembreaModifier As tblMembre = modTest.First()
            MembreaModifier.AdresseMembre = txtAdresse.Text
            MembreaModifier.CourrielMembre = txtCourriel.Text
            MembreaModifier.NoCiviqueMembre = txtNoCivique.Text
            MembreaModifier.NomMembre = txtNomMembre.Text
            MembreaModifier.PrenomMembre = txtPrenomMembre.Text
            MembreaModifier.TelephoneMembre = txtTelephoneMembre.Text
            MembreaModifier.VilleMembre = txtVille.Text
            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de modifier ce cours, veuillez verifier vos entrées")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            'Dit l 'erreur
        End Try

        If (txtCourrielProfesseur.Text IsNot "") Then
            Try
                Dim modTest As IQueryable(Of tblProfesseur) = (From m In DM.tblProfesseur Where m.IdMembre = txtIdMembre.Text Select m)
                Dim ProfaModifier As tblProfesseur = modTest.First()
                ProfaModifier.CourrielCegepProfesseur = txtCourrielProfesseur.Text
                ProfaModifier.PosteTelephoneProfesseur = txtPosteTelephone.Text
                ProfaModifier.NoBureauProfesseur = txtNoBureau.Text

                DM.SaveChanges()


            Catch ex2 As System.Data.SqlClient.SqlException
                MessageBox.Show("Impossible de modifier ce professeur, veuillez verifier vos entrées")
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
                'Dit l 'erreur
            End Try

        End If

        If (txtDaEtudiant.Text IsNot "") Then
            Try
                Dim modTest As IQueryable(Of tblEtudiant) = (From m In DM.tblEtudiant Where m.IdMembre = txtIdMembre.Text Select m)
                Dim EtudiantaModifier As tblEtudiant = modTest.First()
                EtudiantaModifier.DateInscriptionEtudiant = txtDateInscription.Text
                EtudiantaModifier.Annee = txtAnnee.Text

                DM.SaveChanges()


            Catch ex2 As System.Data.SqlClient.SqlException
                MessageBox.Show("Impossible de modifier cet etudiant, veuillez verifier vos entrées")
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
                'Dit l 'erreur
            End Try
        End If

    End Sub

    Private Sub btnGestGroupe_Click(sender As Object, e As RoutedEventArgs) Handles btnGestGroupe.Click
        Dim gestgroupe As New frmGestGroupe
        gestgroupe.ShowDialog()
    End Sub

    Private Sub txtNomMembre_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtNomMembre.PreviewLostKeyboardFocus, txtAdresse.PreviewLostKeyboardFocus, txtCourriel.PreviewLostKeyboardFocus, txtNoCivique.PreviewLostKeyboardFocus, txtPrenomMembre.PreviewLostKeyboardFocus, txtTelephoneMembre.PreviewLostKeyboardFocus, txtVille.PreviewLostKeyboardFocus
        Dim objTextBox As TextBox = CType(sender, TextBox)
        Dim texte As String = objTextBox.Text
        If (texte = "") Then
            e.Handled = True
            statut.Content = "le champ : " + objTextBox.Name + " est vide"

            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
        End If
    End Sub

    Private Sub btnRechercher_Click(sender As Object, e As RoutedEventArgs) Handles btnRechercher.Click
        If (txtRecherche.Text = "") Then
            Return
        End If

        Try
            LesMembres = (From m In DM.tblMembre Where m.TelephoneMembre = txtRecherche.Text Select m)
            vu = Nothing
            vu = New ListCollectionView(LesMembres.ToList())
            txtAdresse.DataContext = vu
            txtCourriel.DataContext = vu
            txtIdMembre.DataContext = vu
            txtNoCivique.DataContext = vu
            txtNomMembre.DataContext = vu
            txtPrenomMembre.DataContext = vu
            txtTelephoneMembre.DataContext = vu
            txtVille.DataContext = vu
            afficherTypeMembre()
            If (txtDaEtudiant.DataContext Is Nothing) Then
                TabTypeMembre.SelectedIndex = 0
            Else
                TabTypeMembre.SelectedIndex = 1
            End If
        Catch ex As Exception
            Dim anim2 As Storyboard = FindResource("AnimTxtRouge")
            statut.Content = " Membre introuvable, veuillez entrer un numéro de téléphone existant"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            txtRecherche.BorderBrush = Brushes.Red
            anim2.Begin(txtRecherche)

        End Try
    End Sub

    Private Sub txtRecherche_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtRecherche.PreviewLostKeyboardFocus

        If (txtRecherche.Text = "") Then
            Window_Loaded(sender, e)
        End If
    End Sub

    Private Sub SelectTab()


        If txtDaEtudiant.Text <> "" Then
            TabTypeMembre.SelectedIndex = 1
        Else
            TabTypeMembre.SelectedIndex = 0
        End If

        TabTypeMembre.IsEnabled = False
    End Sub

 
End Class
