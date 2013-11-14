Public Class Pret

    Dim BD As New PresenceEntities

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
        _aPreter = New List(Of tblPretExemplaire)

        dtgItemPret.ItemsSource = _aPreter


    End Sub

    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click

        Dim p = New tblPret

        p.tblPretExemplaire.Add(_aPreter.First())


    End Sub

    Private Sub dtgItemPret_CellEditEnding(sender As Object, e As DataGridCellEditEndingEventArgs) Handles dtgItemPret.CellEditEnding


    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim Pret As New tblPret


        While (_aPreter.Count <> 0)

            Pret.tblPretExemplaire.Add(_aPreter.First())
            _aPreter.RemoveAt(0)

        End While

        Pret.IdMembre = txtMembre.Text
        Pret.TypeEtat = "actif"

        BD.AddTotblPret(Pret)
        BD.SaveChanges()
        txtIdPret.Clear()
        txtMembre.Clear()




    End Sub


    Private Sub dtgItemPret_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles dtgItemPret.MouseDoubleClick
        Dim i = e.OriginalSource.GetType()
        Dim fnListeExemplaireCompacte As New ListeExemplaireCompacte

        If e.OriginalSource.GetType().Name = "TextBlock" Then
            Dim tb = CType(sender, DataGrid).CurrentCell.Column.Header
            If (tb.ToString = "Code Barre") Then
                fnListeExemplaireCompacte.ShowDialog()
                CType(CType(sender, DataGrid).CurrentCell.Item, tblPretExemplaire).CodeBarre = CType(fnListeExemplaireCompacte.lstExemplaire.SelectedItem, tblExemplaire).CodeBarre
            End If
        End If

    End Sub
End Class