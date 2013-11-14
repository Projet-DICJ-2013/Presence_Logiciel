Class ListeMembre

    Private Sub TabControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub


    Private Sub lstModele_Loaded(sender As Object, e As RoutedEventArgs) Handles lstMembre.Loaded
        Dim BD As New PresenceEntities


        Dim tblMembre = (From Modele In BD.tblMembre _
                        Select Modele).ToList

        lstMembre.ItemsSource = tblMembre
        
    End Sub
    Private Sub btnSelectioner_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectioner.Click
        MyBase.Close()
    End Sub
End Class