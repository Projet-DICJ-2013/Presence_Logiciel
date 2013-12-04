Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class rssActualite
    Public DM As PresenceEntities
    Dim LesActu As IQueryable(Of tblActualite)
    Dim nouvelleActualite As tblActualite
    Dim vu As ListCollectionView
    Public statut As Label



    Private Sub btnAdd_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnAdd.MouseDown
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities

        LesActu = (From actu In DM.tblActualite Select actu)

        lstNouvelles.ItemsSource = LesActu '(LesActu.First, tblActualite).TitreActu
    End Sub

    Private Sub lstNouvelles_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstNouvelles.MouseDoubleClick
        lstNouvelles.Visibility = Windows.Visibility.Hidden
        txtActu.Visibility = Windows.Visibility.Visible
        txtActu.DataContext = Nothing
        btnbackk.Visibility = Windows.Visibility.Visible
        Dim lesactu2 = (From act In DM.tblActualite Where act.TitreActu = (CType(lstNouvelles.SelectedItem, tblActualite).TitreActu) Select act)
        txtActu.DataContext = CType(lesactu2.First, tblActualite)
        btnEdit.Visibility = Windows.Visibility.Visible

    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub btnBack_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles btnBack.MouseDown
        lstNouvelles.Visibility = Windows.Visibility.Visible
        txtActu.Visibility = Windows.Visibility.Hidden
        btnbackk.Visibility = Windows.Visibility.Hidden
        btnEdit.Visibility = Windows.Visibility.Hidden

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click
        Try
            Dim SupprTest As IQueryable(Of tblActualite) = (From act In DM.tblActualite Where act.TitreActu = (CType(lstNouvelles.SelectedItem, tblActualite).TitreActu) Select act)
            Dim ActASupprimer As tblActualite = SupprTest.First()
            DM.DeleteObject(ActASupprimer)
            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer cette actualité")
        Catch ex As Exception
            statut.Content = "Impossible de supprimer cette actualité"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            'Dit l 'erreur
        End Try


    End Sub

    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click

        If (txtTitre.Text = "") Or (txtContenu.Text = "") Then
            statut.Content = "Un de vos champs est vide"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            Return
        End If
        nouvelleActualite = New tblActualite With _
{
.TitreActu = txtTitre.Text, _
.TexteActu = txtContenu.Text, _
.AuteurActu = "Admin"
}

        Try

            DM.AddTotblActualite(nouvelleActualite)
            DM.SaveChanges()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As RoutedEventArgs) Handles btnEdit.Click
        txtActu.IsEnabled = True
        btnSavee.Visibility = Windows.Visibility.Visible

    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSavee.Click
        Try
            Dim modTest As IQueryable(Of tblActualite) = (From act In DM.tblActualite Where act.TitreActu = (CType(lstNouvelles.SelectedItem, tblActualite).TitreActu) Select act)
            Dim ActaModifier As tblActualite = modTest.First()
            ActaModifier.TexteActu = txtActu.Text

            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            statut.Content = "Impossible de modifier cette actualité"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            'Dit l 'erreur
        End Try
        txtActu.IsEnabled = False
        btnSavee.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub TabItem_MouseDown(sender As Object, e As MouseButtonEventArgs)
        txtActu.Visibility = Windows.Visibility.Hidden
    End Sub
End Class
