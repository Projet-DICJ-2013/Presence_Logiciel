
Public Class rssActualite
    Public DM As PresenceEntities
    Dim LesActu As IQueryable(Of tblActualite)
    Dim nouvelleActualite As tblActualite

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub btnAdd_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnAdd.MouseDown
        nouvelleActualite = New tblActualite With _
{
  .TitreActu = txtTitre.Text, _
  .TexteActu = txtContenu.Text, _
  .AuteurActu = "Admin"
}

        Try

            DM.AddTotblActualite(nouvelleActualite)
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities

        LesActu = (From actu In DM.tblActualite Select actu)

        lstNouvelles.ItemsSource = CType(LesActu.First, tblActualite).TitreActu
    End Sub
End Class
