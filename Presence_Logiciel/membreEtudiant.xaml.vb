Public Class membreEtudiant
    Public DM As PresenceEntities
    Dim newEtu As tblEtudiant
    Dim unEtu As IQueryable(Of tblEtudiant)
    Dim donnemoiunmembre As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView
    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Dim dateNow As Date
        dateNow = DateValue(Now)

        newEtu = New tblEtudiant With _
{
    .Annee = cbAnnee.SelectionBoxItem.ToString, _
    .DaEtudiant = txtDaEtudiant.Text, _
    .DateInscriptionEtudiant = dateNow, _
    .IdMembre = txtIdMembre.Text _
}


        Try

            DM.tblEtudiant.AddObject(newEtu)
            DM.SaveChanges()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim dateNow As Date
        dateNow = DateValue(Now)
        donnemoiunmembre = (From m In DM.tblMembre Select m)
        vu = New ListCollectionView(donnemoiunmembre.ToList())
        txtIdMembre.DataContext = vu
        txtDateInscription.Text = dateNow
        vu.MoveCurrentToLast()
    End Sub
End Class
