Public Class MembreProf
    Public DM As PresenceEntities
    Dim newProf As tblProfesseur
    Dim unmembre As IQueryable(Of tblMembre)
    Dim donnemoiunmembre As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView


    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Dim RandGen As New Random
        newProf = New tblProfesseur With _
{
    .NoEmploye = RandGen.Next(50000, 80000).ToString, _
    .NoBureauProfesseur = txtNoBureau.Text, _
    .PosteTelephoneProfesseur = txtPoste.Text, _
    .ChargeTravailProfesseur = txtChargeTravail.Text, _
    .CourrielCegepProfesseur = txtCourriel.Text, _
    .IdMembre = txtIdMembre.Text _
}


        Try

            DM.tblProfesseur.AddObject(newProf)
            DM.SaveChanges()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        donnemoiunmembre = (From m In DM.tblMembre Select m)
        vu = New ListCollectionView(donnemoiunmembre.ToList())
        txtIdMembre.DataContext = vu
        vu.MoveCurrentToLast()
    End Sub
End Class
