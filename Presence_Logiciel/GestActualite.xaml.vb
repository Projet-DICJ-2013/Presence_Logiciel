
Public Class rssActualite
    Public DM As PresenceEntities
    Dim LesActu As IQueryable(Of tblActualite)
    Dim nouvelleActualite As tblActualite
    Dim vu As ListCollectionView



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

        lstNouvelles.ItemsSource = LesActu '(LesActu.First, tblActualite).TitreActu
    End Sub

    Private Sub lstNouvelles_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstNouvelles.MouseDoubleClick
        lstNouvelles.Visibility = Windows.Visibility.Hidden
        txtActu.Visibility = Windows.Visibility.Visible
        txtActu.DataContext = Nothing
        Dim lesactu2 = (From act In DM.tblActualite Where act.TitreActu = (CType(lstNouvelles.SelectedItem, tblActualite).TitreActu) Select act)
        txtActu.DataContext = CType(lesactu2.First, tblActualite)
    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub btnBack_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnBack.MouseDown
        lstNouvelles.Visibility = Windows.Visibility.Visible
        txtActu.Visibility = Windows.Visibility.Hidden
    End Sub
End Class
