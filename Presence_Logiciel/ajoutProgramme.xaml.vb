Imports System.Windows.Media.Animation
Public Class ajoutProgramme
    Public DM As PresenceEntities
    Dim nouveauProg As tblProgramme
    Public statut As Label

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

    Private Sub txtCodeProgramme_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtCodeProgramme.PreviewLostKeyboardFocus, txtObjectif.PreviewLostKeyboardFocus, txtProgramme.PreviewLostKeyboardFocus
        Dim objTextBox As TextBox = CType(sender, TextBox)
        Dim texte As String = objTextBox.Text
        If (texte = "") Then
            e.Handled = True
            statut.Content = "le champ : " + objTextBox.Name + " est vide"

            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
        End If
    End Sub
End Class
