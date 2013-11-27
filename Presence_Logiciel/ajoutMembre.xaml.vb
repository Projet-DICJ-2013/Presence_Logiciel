Public Class ajoutMembre
    Public DM As PresenceEntities
    Dim newMembre As tblMembre
    Dim LesMembres As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView
    Dim lemembre As tblMembre


    Private Sub cmbVille_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnAjouter_Click(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        newMembre = New tblMembre With _
{
    .AdresseMembre = txtAdresse.Text, _
    .CourrielMembre = txtCourriel.Text, _
    .NoCiviqueMembre = txtNoCivique.Text, _
    .NomMembre = txtNom.Text, _
    .PrenomMembre = txtPrenom.Text, _
    .TelephoneMembre = txtTelephone.Text, _
    .VilleMembre = cmbVille.SelectionBoxItem.ToString
}


        Try

            DM.tblMembre.AddObject(newMembre)
  
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
        If (rdProfesseur.IsChecked = True) Then
            Dim unprof As New MembreProf
            unprof.DM = DM
            unprof.ShowDialog()
        Else
            Dim unEtudiant As New membreEtudiant
            unEtudiant.LeNouveau = newMembre
            unEtudiant.DM = DM
            unEtudiant.ShowDialog()
        End If

    End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub


    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub
End Class
