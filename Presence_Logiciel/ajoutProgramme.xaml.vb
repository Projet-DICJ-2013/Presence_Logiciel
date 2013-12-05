Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions
Public Class ajoutProgramme
    Public DM As PresenceEntities
    Dim nouveauProg As tblProgramme
    Public statut As Label


    Private Sub btnAjouter_Click(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click





        Dim myRegex3 As New Regex( _
 "^\d{3}.[a-z0-9]{2}$")
        If (myRegex3.IsMatch(txtCodeProgramme.Text) = False) Then
            statut.Content = "Un code de programme doit être : 3x(chiffre) 1x(.) et 2x(lettre/chiffre)"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

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
