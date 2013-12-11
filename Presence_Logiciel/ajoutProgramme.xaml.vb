Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions
Public Class ajoutProgramme
    Public DM As PresenceEntities
    Dim nouveauProg As tblProgramme
    Public statut As Label


    Private Sub btnAjouter_Click(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        ''Initialisation de l'animation des textbox créée dans le XAML
        Dim anim2 As Storyboard = FindResource("AnimTxtRouge")


        ''Vérification du contenu des textbox
        Dim myRegex3 As New Regex( _
 "^\d{3}.[a-z0-9]{2}$")
        If (myRegex3.IsMatch(txtCodeProgramme.Text) = False) Then
            statut.Content = "Un code de programme doit être : 3x(chiffre) 1x(.) et 2x(lettre/chiffre)"
            Dim anim As Storyboard = FindResource("AnimLabel")
            txtCodeProgramme.BorderBrush = Brushes.Red
            anim2.Begin(txtCodeProgramme)

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
            ''tentative d'ajouter un nouvel objet de tblProgramme
            DM.AddTotblProgramme(nouveauProg)
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    ''Vérifie si les champs sont vide
    Private Sub txtCodeProgramme_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtCodeProgramme.PreviewLostKeyboardFocus, txtObjectif.PreviewLostKeyboardFocus, txtProgramme.PreviewLostKeyboardFocus
        Dim objTextBox As TextBox = CType(sender, TextBox)
        Dim texte As String = objTextBox.Text
        If (texte = "") Then
            e.Handled = True
            statut.Content = "le champ : " + objTextBox.Name + " est vide"

            Dim anim As Storyboard = FindResource("AnimLabel")
            Dim anim2 As Storyboard = FindResource("AnimTxtRouge")
            anim.Begin(statut)

            objTextBox.BorderBrush = Brushes.Red

            anim2.Begin(objTextBox)


        End If


    End Sub
End Class
