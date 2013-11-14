Public Class ajoutCour
    Public DM As PresenceEntities
    Dim newCours As tblCours

    Private Sub Fer(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub FermerAjoutCours(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub AjouterCours(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        newCours = New tblCours With _
        {
            .AnneeCours = txtAnneeCours.Text, _
            .NomCours = txtNomCours.Text, _
            .CodeCours = txtCodeCours.Text, _
            .PonderationCours = txtPonderation.Text, _
            .DescriptionCours = txtDescription.Text
        }


        Try

            DM.tblCours.AddObject(newCours)
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


    End Sub

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
