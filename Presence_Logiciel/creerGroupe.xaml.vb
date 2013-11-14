Public Class creerGroupe

    Public Forme As PresenceEntities


    Private Sub btncreergroupe_Click(sender As Object, e As RoutedEventArgs) Handles btncreergroupe.Click
        'Ajout d'un nouveau groupe
        Dim _nouveauGroupe = New tblGroupe

        _nouveauGroupe.IdGroupe = txtidgroupe.Text
        _nouveauGroupe.NoGroupe = txtnogroupe.Text

        Forme.tblGroupe.AddObject(_nouveauGroupe)
        ' Validation de cet ajout
        Dim _reponse = MessageBox.Show("Voulez-vous vraiment creer ce nouveau groupe", "Créer un groupe", MessageBoxButton.YesNo)
        If _reponse = 6 Then
            'Enregistrement du nouveau groupe
            Forme.SaveChanges()
            MessageBox.Show("Un nouveau groupe a bien été enregistré", "Succès", MessageBoxButton.OK)
        Else
            Return
        End If
    End Sub

    Private Sub frmcreergroupe_Closed(sender As Object, e As EventArgs) Handles frmcreergroupe.Closed
        'Fermeture de la fenêtre
        Me.Finalize()
        Me.Close()

    End Sub
End Class
