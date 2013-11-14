Imports System.Linq
Class frmExemplaire

    Private BD As New PresenceEntities
    Private List As ListCollectionView

    Private Sub txtNomReseau_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtNomReseau.TextChanged

    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        BindControl()

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


    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)


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
End Class