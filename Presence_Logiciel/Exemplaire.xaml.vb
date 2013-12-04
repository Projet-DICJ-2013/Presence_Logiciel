Imports System.Linq
Class frmExemplaire
    Private App As New FonctionsGlobales
    Private BD As New PresenceEntities
    Private List As ListCollectionView

    Private Sub txtNomReseau_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtNomReseau.TextChanged

    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        ' BindControl()

    End Sub

    Private Sub BindControl()
        Dim Mon_Exemp = (From Exemplaire In BD.tblExemplaire
                         Select Exemplaire).ToList

        List = New ListCollectionView(Mon_Exemp)

        txtCodeBarre.DataContext = Mon_Exemp
        txtNomReseau.DataContext = Mon_Exemp
        txtNoSerie.DataContext = Mon_Exemp
        txtModele.DataContext = Mon_Exemp
        dtpDateAchat.DataContext = Mon_Exemp
        txtNote.DataContext = Mon_Exemp

    End Sub

    'La procédure ne s'exécutera pas si la validation n'est pas acceptée.
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        If Valider_Exemplaire() Then
            Dim exemp As New tblExemplaire With _
        {
            .CodeBarre = txtCodeBarre.Text, _
            .NomReseau = txtNomReseau.Text, _
            .NoSerie = txtNoSerie.Text, _
            .NoModele = txtModele.Text, _
            .NoteExemplaire = txtNote.Text, _
            .TypeEtat = "Disponible"
        }

            Dim etat = (From _et In BD.tblEtatExemplaire Where _et.TypeEtat = "Disponible" Select _et).FirstOrDefault()
            Try
                BD.AddTotblExemplaire(exemp)
                BD.SaveChanges()
            Catch ex As System.Data.SqlClient.SqlException

            End Try


            txtCodeBarre.Clear()
            txtModele.Clear()
            txtNomReseau.Clear()
            txtNoSerie.Clear()
            txtNote.Clear()
        End If
    End Sub

    Private Sub txtModele_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles txtModele.MouseDoubleClick

        Dim fnListeModele As New ListeModele
        fnListeModele.ShowDialog()

        If (fnListeModele.lstModele.SelectedItem IsNot Nothing) Then
            txtModele.Text = CType(fnListeModele.lstModele.SelectedItem, tblModele).NoModele
        End If

    End Sub

    Private Sub btnFirst_Click(sender As Object, e As RoutedEventArgs) Handles btnFirst.Click
        List.MoveCurrentToFirst()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        List.MoveCurrentToLast()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As RoutedEventArgs) Handles btnNext.Click
        List.MoveCurrentToNext()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As RoutedEventArgs) Handles btnPrevious.Click
        List.MoveCurrentToPrevious()
    End Sub


    Private Sub txtModele_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtModele.LostFocus
        If txtModele.Text = "" Then
            txtModele.BorderBrush = Brushes.Red
            txtModele.Text = "0"
        Else
            txtModele.BorderBrush = Brushes.Green
        End If
    End Sub

    'PARTIE DE VALIDATION'

    'Vérifie si le code a une valeur valide. Appele la fonction qui vérifie si le code entré existe déjà dans la BD. Les doublons ne sont pas acceptés.
    Private Function valider_code_barre(ByVal code As String)
        Try
            If App.verifier_int(code) = True And code > 1000 = True And Verifier_doublon_code(code) = True And App.verifier_null(code) = True Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try
    End Function

    'Vérifie si le nom du réseau est à moins de 51 caractères
    Private Function valider_nom_reseau(ByVal nomreseau As String)
        If nomreseau.Length() > 50 Then
            Return False
        Else
            Return True
        End If
    End Function

    'Vérifie si le no de série est non null et a 20 caracteres ou moins
    Private Function valider_no_serie(ByVal noserie As String)

        If noserie.Length() = 0 Or noserie.Length() > 20 Then
            Return False
        Else
            Return True
        End If
    End Function

    'Vérifie si le no de modele sélectionné est bel et bien existant. Aucun no de model non existant n'est accepté.
    Private Function valider_modele(ByVal nodemodele As String)
        Dim ModeleR = (From Modele In BD.tblModele
                      Where Modele.NoModele = nodemodele
                      Select Modele)
        If ModeleR.ToList.Count() > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    'Vérifie si le code barre envoyé en paramètre est un doublon
    Private Function Verifier_doublon_code(ByVal codebar As String)
        Dim codeV = (From codeB In BD.tblExemplaire
                     Where codeB.CodeBarre = codebar
                     Select codeB)
        If codeV.ToList.Count() > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    'Fonction qui appele toutes les mini fonctions de validation. Elle est utilisée dans la création de données dans la BD avant d'officialiser l'entrée.
    Private Function Valider_Exemplaire()
        Dim valide As Boolean = True
        If valider_code_barre(txtCodeBarre.Text) = False Then
            App.message_box_validation("code barre")
            valide = False
            txtCodeBarre.Clear()
        End If
        If valide = True And valider_nom_reseau(txtNomReseau.Text) = False Then
            valide = False
            txtNomReseau.Clear()
            App.message_box_validation("nom de réseau")
        End If
        If valide = True And valider_no_serie(txtNoSerie.Text) = False Then
            valide = False
            txtNoSerie.Clear()
            App.message_box_validation("no de série")
        End If
        If valide = True And valider_modele(txtModele.Text) = False Then
            valide = False
            txtModele.Clear()
            App.message_box_validation("no de modele")
        End If
        If valide = True And dtpDateAchat.SelectedDate Is Nothing Then
            valide = False
            MessageBox.Show("Veuillez entrer une date")
        End If
        Return valide
    End Function
End Class