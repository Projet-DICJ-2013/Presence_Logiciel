Class ListeModele

    Private BD As New PresenceEntities

    Private Sub TabControl_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub


    Private Sub lstModele_Loaded(sender As Object, e As RoutedEventArgs) Handles lstModele.Loaded


        BindControl()
    End Sub
    Private Sub btnSelectioner_Click(sender As Object, e As RoutedEventArgs) Handles btnSelectioner.Click
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub BindControl()
        Dim tblModele = (From Modele In BD.tblModele _
         Select Modele).ToList

        lstModele.ItemsSource = tblModele
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim fnModele As New frmModele
        fnModele.ShowDialog()

        BindControl()

    End Sub

    Private Sub OnQuit(sender As Object, e As RoutedEventArgs) Handles MyBase.Closed
        Me.Close()
        Me.Finalize()
    End Sub
End Class