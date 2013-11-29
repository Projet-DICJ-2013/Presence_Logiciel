Public Class creerGroupe

    Public Forme As PresenceEntities


    Private Sub btncreergroupe_Click(sender As Object, e As RoutedEventArgs) Handles btncreergroupe.Click
        'Ajout d'un nouveau groupe
        Dim _nouveauGroupe = New tblGroupe
        Dim _condition As Boolean = False
        Dim _vrai1 As Boolean = False
        Dim _vrai2 As Boolean = False

        'Vérification du champ IdGroupe s'il correspond au conditioj
        If txtidgroupe.Text = Nothing Then
            MessageBox.Show("Entrer une valeur pour IdGroupe", "Validation", MessageBoxButton.OK)
        ElseIf IsNumeric(txtidgroupe) = False Then
            MessageBox.Show("Votre valeur entrée doit être des chiffres", "Validation", MessageBoxButton.OK)
        Else
            _vrai1 = True
        End If

        'Vérification du champ NoGroupe s'il correspond au conditioj
        If txtnogroupe.Text = Nothing Then
            MessageBox.Show("Entrer une valeur pour NoGroupe", "Validation", MessageBoxButton.OK)
        ElseIf IsNumeric(txtnogroupe) = False Then
            MessageBox.Show("Votre valeur entrée doit être des chiffres", "Validation", MessageBoxButton.OK)
        Else
            _vrai2 = True
        End If

        If _vrai1 And _vrai2 = True Then
            'ajout des valeurs dans la BD s'il corresponde au condition
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
        Else
            'Si une chose de correspond pas re-rentrer les champs dans les textbox
            MessageBox.Show("Veuillez vérifier vos champs s'ils sont numrique et/ou non vide", "Validation", MessageBoxButton.OK)
        End If
    End Sub

    Private Sub frmcreergroupe_Closed(sender As Object, e As EventArgs) Handles frmcreergroupe.Closed
        'Fermeture de la fenêtre
        Me.Finalize()
        Me.Close()

    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        'Fermeture de la fenêtre
        Me.Finalize()
        Me.Close()
    End Sub
End Class
