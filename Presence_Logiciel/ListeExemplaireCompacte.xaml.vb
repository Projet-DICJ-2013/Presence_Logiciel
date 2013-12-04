Public Class ListeExemplaireCompacte

    Public BD As PresenceEntities

    Private Sub btnSelectioner_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectioner.Click
        MyBase.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)


        Dim req = (From r In BD.tblExemplaire
           Where r.TypeEtat = "Disponible"
     Select r)
        lstExemplaire.ItemsSource = req
    End Sub
End Class