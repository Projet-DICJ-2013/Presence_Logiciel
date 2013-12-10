Public Class Pret

    Dim BD As PresenceEntities

    Dim _aPreter As List(Of tblPretExemplaire)

    Private Sub txtMembre_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles txtMembre.MouseDoubleClick
        Dim fnListeMembre As New ListeMembre
        fnListeMembre.ShowDialog()
        If Not IsNothing(fnListeMembre.lstMembre.SelectedItem) Then
            txtMembre.Text = CType(fnListeMembre.lstMembre.SelectedItem, tblMembre).IdMembre
        End If
    End Sub

    Private Sub ListeCodeBarre_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)


    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        BD = New PresenceEntities
        _aPreter = New List(Of tblPretExemplaire)

        dtgItemPret.ItemsSource = _aPreter


    End Sub

    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click




    End Sub

    Private Sub dtgItemPret_CellEditEnding(sender As Object, e As DataGridCellEditEndingEventArgs) Handles dtgItemPret.CellEditEnding


    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim Pret = New tblPret
        Dim exemplaire As New tblExemplaire



        For i As Integer = 0 To _aPreter.Count() - 1

            Pret.tblPretExemplaire.Add(_aPreter(i))
            _aPreter(i).tblExemplaire.TypeEtat = "Prêté"



        Next



        '_aPreter.Clear()

        Pret.IdMembre = txtMembre.Text
        Pret.TypeEtat = "actif"


        Try
            BD.tblPret.AddObject(Pret)
            BD.SaveChanges()
        Catch ex As Exception

        End Try

        txtIdPret.Clear()
        txtMembre.Clear()

        Dim r = From p In BD.tblPret Select p


    End Sub


    Private Sub dtgItemPret_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles dtgItemPret.MouseDoubleClick
        Dim i = e.OriginalSource.GetType()
        Dim fnListeExemplaireCompacte As New ListeExemplaireCompacte

        fnListeExemplaireCompacte.BD = BD
        If e.OriginalSource.GetType().Name = "TextBlock" Then
            Dim tb = CType(sender, DataGrid).CurrentCell.Column.Header
            If (tb.ToString = "Code Barre") Then
                fnListeExemplaireCompacte.ShowDialog()
                ' CType(CType(sender, DataGrid).CurrentCell.Item, tblPretExemplaire).CodeBarre = CType(fnListeExemplaireCompacte.lstExemplaire.SelectedItem, tblExemplaire).CodeBarre
                CType(CType(sender, DataGrid).CurrentCell.Item, tblPretExemplaire).tblExemplaire = CType(fnListeExemplaireCompacte.lstExemplaire.SelectedItem, tblExemplaire)
            End If
        End If

    End Sub
End Class