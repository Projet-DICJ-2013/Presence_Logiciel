Public Class gestProgrammes
    Dim DM As PresenceEntities
    Dim LesProgrammes As IQueryable(Of tblProgramme)
    Dim vu As ListCollectionView
    Dim codeprog As String



    Private Sub AffProgramme(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesProgrammes = (From prog In DM.tblProgramme Select prog)
        vu = New ListCollectionView(LesProgrammes.ToList())
        txtNumProgramme.DataContext = vu
        txtNomProgramme.DataContext = vu
        txtObjectif.DataContext = vu
        lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours



    End Sub




    Private Sub ChargerListecours(sender As Object, e As TextChangedEventArgs) Handles txtNumProgramme.TextChanged
        Dim lstProg = (From p In DM.tblProgramme Select p Where p.CodeProg = txtNumProgramme.Text)
        Dim LeProg As tblProgramme = lstProg.First
        lvCours.ItemsSource = CType(LeProg, tblProgramme).tblCours


    End Sub







    Private Sub AfficherSuivant(sender As Object, e As RoutedEventArgs) Handles btnSuivant.Click
        If (vu.CurrentPosition < LesProgrammes.Count() - 1) Then
            vu.MoveCurrentToNext()
        End If
    End Sub

    Private Sub affAjoutCours(sender As Object, e As RoutedEventArgs)
        Dim uncours As New LierCoursProgramme
        codeprog = txtNumProgramme.Text
        uncours.DM = DM
        uncours.codeprog = codeprog
        uncours.ShowDialog()
    End Sub

    Private Sub btnPremier_Click(sender As Object, e As RoutedEventArgs) Handles btnPremier.Click
        vu.MoveCurrentToFirst()
    End Sub

    Private Sub btnPrecedent_Click(sender As Object, e As RoutedEventArgs) Handles btnPrecedent.Click
        If (vu.CurrentPosition > 0) Then
            vu.MoveCurrentToPrevious()
        End If
    End Sub

    Private Sub btnDernier_Click(sender As Object, e As RoutedEventArgs) Handles btnDernier.Click
        vu.MoveCurrentToLast()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        btnFontEnregistrer.Visibility = Windows.Visibility.Visible
        btnEnregistrer.Visibility = Windows.Visibility.Visible
        txtNomProgramme.IsEnabled = True
        txtObjectif.IsEnabled = True
    End Sub

    Private Sub btnSupprimer_Click(sender As Object, e As RoutedEventArgs) Handles btnSupprimer.Click
        Try
            Dim SupprTest As IQueryable(Of tblProgramme) = (From p In DM.tblProgramme Where p.CodeProg = txtNumProgramme.Text Select p)
            Dim CoursASupprimer As tblProgramme = SupprTest.First()
            DM.DeleteObject(CoursASupprimer)
            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer ce cours car il est actif à un programme de la session courrante")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            'Dit l 'erreur
        End Try
        LesProgrammes = (From prog In DM.tblProgramme Select prog)
        vu = New ListCollectionView(LesProgrammes.ToList())
        txtNumProgramme.DataContext = vu
        txtNomProgramme.DataContext = vu
        txtObjectif.DataContext = vu
        lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours
    End Sub

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)
        AffProgramme(sender, e)
    End Sub

    Private Sub imgX_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles imgX.MouseDown
        Me.Close()
        Me.Finalize()
    End Sub


    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click
        Dim ajProg As New ajoutProgramme
        ajProg.DM = DM
        ajProg.ShowDialog()
        LesProgrammes = (From prog In DM.tblProgramme Select prog)
        vu = New ListCollectionView(LesProgrammes.ToList())
        txtNumProgramme.DataContext = vu
        txtNomProgramme.DataContext = vu
        txtObjectif.DataContext = vu
        lvCours.ItemsSource = CType(LesProgrammes.First, tblProgramme).tblCours
    End Sub

    Private Sub btnLier_Click(sender As Object, e As RoutedEventArgs) Handles btnLier.Click
        Dim uncours As New LierCoursProgramme
        codeprog = txtNumProgramme.Text
        uncours.DM = DM
        uncours.codeprog = codeprog
        uncours.ShowDialog()
    End Sub

    Private Sub btnEnregistrer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnregistrer.Click
        txtNomProgramme.IsEnabled = False
        txtObjectif.IsEnabled = False
        btnFontEnregistrer.Visibility = Windows.Visibility.Hidden
        btnEnregistrer.Visibility = Windows.Visibility.Hidden

        Try
            Dim modTest As IQueryable(Of tblProgramme) = (From p In DM.tblProgramme Where p.CodeProg = txtNumProgramme.Text Select p)
            Dim CoursAmodifier As tblProgramme = modTest.First()
            CoursAmodifier.CodeProg = txtNumProgramme.Text
            CoursAmodifier.NomProg = txtNomProgramme.Text
            CoursAmodifier.ObjectifProg = txtObjectif.DataContext.ToString

            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer ce cours car il est actif à un programme de la session courrante")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            'Dit l 'erreur
        End Try
    End Sub
End Class
