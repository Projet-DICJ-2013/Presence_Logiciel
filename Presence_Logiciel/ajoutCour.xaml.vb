Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions
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

        Dim myRegex1 As New Regex( _
"[A-Z0-9]{3}-[A-Z0-9]{3}-[A-Z0-9]{2}")
        If (myRegex1.IsMatch(txtCodeCours.Text) = False) Then
            statut.Content = "Un code de cours doit être : 3x(chiffre) 1x(-) 3x(lettre) 1x(-) et 2x(lettre)"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

        Dim myRegex2 As New Regex( _
"\d-\d-\d")
        If (myRegex2.IsMatch(txtPonderation.Text) = False) Then
            statut.Content = "Une pondération de cours doit être : 1x(chiffre) 1x(-) 1x(chiffre) 1x(-) et 1x(chiffre) )"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

        Dim myRegex3 As New Regex( _
"[1-3]")
        If (myRegex3.IsMatch(txtAnneeCours.Text) = False) Then
            statut.Content = "Une année de cours doit être entre : 1 et 3 )"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

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
