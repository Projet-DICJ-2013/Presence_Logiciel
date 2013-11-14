Public Class LierCoursProgramme
    Public DM As PresenceEntities
    Public codeprog As String
    Dim lesCours As tblCours
    Dim leprog As tblProgramme
    Dim lstprog As IQueryable(Of tblProgramme)


    Private Sub Fermer(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Finalize()
        Me.Close()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

        lstprog = (From p In DM.tblProgramme Select p Where p.CodeProg = codeprog)
        leprog = lstprog.First
        lvCoursActuels.ItemsSource = CType(leprog, tblProgramme).tblCours

        Dim lesCours = (From c In DM.tblCours Select c)
        lvTousCours.ItemsSource = lesCours

    End Sub

    Private Sub lvTousCours_PreviewMouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs) Handles lvTousCours.PreviewMouseLeftButtonUp

    End Sub

  

    Private Sub lvCoursActuels_Drop(sender As Object, e As DragEventArgs) Handles lvCoursActuels.Drop
        Dim donnee = e.Data.GetData("chose")
        If CType(leprog, tblProgramme).tblCours.Contains(donnee) = False Then

            CType(leprog, tblProgramme).tblCours.Add(donnee)
            lvCoursActuels.ItemsSource = Nothing
            lvCoursActuels.ItemsSource = CType(leprog, tblProgramme).tblCours

        Else
            e.Effects = DragDropEffects.None
        End If
    End Sub

    Private Sub lvTousCours_MouseMove(sender As Object, e As MouseEventArgs) Handles lvTousCours.MouseMove
        If e.LeftButton Then
            Dim p = lvTousCours.SelectedItem

            Dim donnee = New DataObject

            donnee.SetData("chose", p)

            Dim s = DragDrop.DoDragDrop(sender, donnee, DragDropEffects.Move)

            If s = DragDropEffects.Move Then

            End If
        End If
    End Sub

    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Try
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try


    End Sub

    Private Sub lvTousCours_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lvTousCours.SelectionChanged
 
        lblCodeCours.DataContext = Nothing
        lblCodeCours.DataContext = lvTousCours.SelectedItem

    End Sub

    Private Sub lvCoursActuels_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lvCoursActuels.SelectionChanged
        lblCodeCours.DataContext = Nothing
        lblCodeCours.DataContext = lvCoursActuels.SelectedItem
    End Sub

    Private Sub lvCoursActuels_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lvCoursActuels.MouseDoubleClick

        CType(leprog, tblProgramme).tblCours.Remove(lvCoursActuels.SelectedItem)
            lvCoursActuels.ItemsSource = Nothing
            lvCoursActuels.ItemsSource = CType(leprog, tblProgramme).tblCours

    End Sub

    Private Sub lvTousCours_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lvTousCours.MouseDoubleClick
        CType(leprog, tblProgramme).tblCours.Add(lvTousCours.SelectedItem)
        lvCoursActuels.ItemsSource = Nothing
        lvCoursActuels.ItemsSource = CType(leprog, tblProgramme).tblCours
    End Sub

    Private Sub btnSupprimer1_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnSupprimer1.MouseDown
        If (lvCoursActuels.SelectedItem Is Nothing) Then
            Return
        Else
            CType(leprog, tblProgramme).tblCours.Remove(lvCoursActuels.SelectedItem)
            lvCoursActuels.ItemsSource = Nothing
            lvCoursActuels.ItemsSource = CType(leprog, tblProgramme).tblCours
        End If
    End Sub
End Class
