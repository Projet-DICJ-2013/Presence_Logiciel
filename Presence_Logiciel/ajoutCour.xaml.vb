Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions
Public Class ajoutCour
    Public DM As PresenceEntities
    Dim newCours As tblCours
    Public statut As Label


   

    Private Sub AjouterCours(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        ''Initialisation des animations créées dans le XAML
        Dim anim As Storyboard = FindResource("AnimLabel")
        Dim anim2 As Storyboard = FindResource("AnimTxtRouge")


        ''Vérification du contenu des champs
        Dim myRegex1 As New Regex( _
"[A-Z0-9]{3}-[A-Z0-9]{3}-[A-Z0-9]{2}")
        If (myRegex1.IsMatch(txtCodeCours.Text) = False) Then
            statut.Content = "Un code de cours doit être : 3x(chiffre) 1x(-) 3x(lettre) 1x(-) et 2x(lettre)"
            txtCodeCours.BorderBrush = Brushes.Red
            anim2.Begin(txtCodeCours)
            anim.Begin(statut)
            Return
        End If

        Dim myRegex2 As New Regex( _
"\d-\d-\d")
        If (myRegex2.IsMatch(txtPonderation.Text) = False) Then
            statut.Content = "Une pondération de cours doit être : 1x(chiffre) 1x(-) 1x(chiffre) 1x(-) et 1x(chiffre) )"
            txtPonderation.BorderBrush = Brushes.Red
            anim2.Begin(txtPonderation)


            anim.Begin(statut)
            Return
        End If

        Dim myRegex3 As New Regex( _
"[1-3]")
        If (myRegex3.IsMatch(txtAnneeCours.Text) = False) Then
            statut.Content = "Une année de cours doit être entre : 1 et 3 )"
            txtAnneeCours.BorderBrush = Brushes.Red
            anim2.Begin(txtAnneeCours)

            anim.Begin(statut)
            Return
        End If


        ''Création d'un nouvel objet  tblcours
        newCours = New tblCours With _
        {
            .AnneeCours = txtAnneeCours.Text, _
            .NomCours = txtNomCours.Text, _
            .CodeCours = txtCodeCours.Text, _
            .PonderationCours = txtPonderation.Text, _
            .DescriptionCours = txtDescription.Text
        }





        Try
            ''Essai d'ajouter cet objet sur la BD
            DM.tblCours.AddObject(newCours)
            DM.SaveChanges()
        Catch ex As Exception

            MessageBox.Show(ex.ToString)
        End Try

        Try
            ''Si passe, Attribution d'un statut au cours
            Dim newStatut As tblStatutCoursCours
            newStatut = New tblStatutCoursCours With _
   {
       .CodeCours = txtCodeCours.Text, _
       .DateAcquisitionStatut = DateTime.Now().ToString, _
       .NomStatutCours = CType(cmbStatut.SelectedItem, ComboBoxItem).Content.ToString()
   }

        Catch ex As Exception
            MsgBox(ex)

        End Try


    End Sub

    ''Se produit si un champ est vide
    Private Sub txtCodeCours_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtCodeCours.PreviewLostKeyboardFocus, txtNomCours.PreviewLostKeyboardFocus, txtAnneeCours.PreviewLostKeyboardFocus, txtPonderation.PreviewLostKeyboardFocus
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
