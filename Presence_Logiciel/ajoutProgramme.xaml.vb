Public Class ajoutProgramme
    Public DM As PresenceEntities
    Dim nouveauProg As tblProgramme

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Finalize()
        Me.Close()
    End Sub

    Private Sub btnAjouter_Click(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        nouveauProg = New tblProgramme With _
{
    .CodeProg = txtCodeProgramme.Text, _
    .NomProg = txtProgramme.Text, _
    .ObjectifProg = txtObjectif.Text
}


        Try

            DM.AddTotblProgramme(nouveauProg)
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub
End Class
