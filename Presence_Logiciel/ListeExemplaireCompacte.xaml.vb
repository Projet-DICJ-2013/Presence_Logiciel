Public Class ListeExemplaireCompacte


    Private Sub btnSelectioner_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectioner.Click
        MyBase.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim BD As New PresenceEntities

        Dim req = (From r In BD.tblExemplaire
           Where r.TypeEtat <> "Supprimé"
     Select r)
        lstExemplaire.ItemsSource = req
    End Sub
End Class