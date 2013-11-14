Public Class gestEtudiant

    Dim DM As PresenceEntities
    Dim LesMembres As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView
    Dim lemembre As tblMembre
    Dim tProf As IQueryable(Of tblMembre)
    Dim leprof As tblMembre



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
        If (txtDaEtudiant.DataContext Is Nothing) Then
            TabTypeMembre.SelectedIndex = 0
        Else
            TabTypeMembre.SelectedIndex = 1
        End If


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

        If (txtDaEtudiant.DataContext Is Nothing) Then
            TabTypeMembre.SelectedIndex = 0
        Else
            TabTypeMembre.SelectedIndex = 1
        End If




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
        txtAnnee.IsEnabled = True

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
        txtAnnee.IsEnabled = False

        txtDateInscription.IsEnabled = False
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

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub btnGestGroupe_Click(sender As Object, e As RoutedEventArgs) Handles btnGestGroupe.Click
        Dim gestgroupe As New frmGestGroupe
        gestgroupe.ShowDialog()
    End Sub
End Class
