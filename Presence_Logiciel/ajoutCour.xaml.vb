Imports System.Windows.Media.Animation
Public Class ajoutCour
    Public DM As PresenceEntities
    Dim newCours As tblCours
    Public statut As Label

    Private Sub Fer(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub FermerAjoutCours(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub AjouterCours(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        newCours = New tblCours With _
        {
            .AnneeCours = txtAnneeCours.Text, _
            .NomCours = txtNomCours.Text, _
            .CodeCours = txtCodeCours.Text, _
            .PonderationCours = txtPonderation.Text, _
            .DescriptionCours = txtDescription.Text
        }


        Try

            DM.tblCours.AddObject(newCours)
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try


    End Sub

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub txtCodeCours_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtCodeCours.PreviewLostKeyboardFocus, txtNomCours.PreviewLostKeyboardFocus, txtAnneeCours.PreviewLostKeyboardFocus, txtPonderation.PreviewLostKeyboardFocus
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
