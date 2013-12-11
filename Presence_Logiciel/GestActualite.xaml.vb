Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class rssActualite
    Public DM As PresenceEntities
    Dim LesActu As IQueryable(Of tblActualite)
    Dim nouvelleActualite As tblActualite
    Public statut As Label



    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesActu = (From actu In DM.tblActualite Select actu)
        lstNouvelles.ItemsSource = LesActu
    End Sub

    ''Sert à afficher le contenu de l'actualité lors d'un click sur le titre
    Private Sub lstNouvelles_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles lstNouvelles.MouseDoubleClick
        lstNouvelles.Visibility = Windows.Visibility.Hidden
        txtActu.Visibility = Windows.Visibility.Visible
        txtActu.DataContext = Nothing
        btnbackk.Visibility = Windows.Visibility.Visible
        Dim lesactu2 = (From act In DM.tblActualite Where act.TitreActu = (CType(lstNouvelles.SelectedItem, tblActualite).TitreActu) Select act)
        txtActu.DataContext = CType(lesactu2.First, tblActualite)
        btnEdit.Visibility = Windows.Visibility.Visible

    End Sub

    ''Quitter le contenu de l'actualité pour retourner à la liste de titre
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

        ''Vérification du contenu vide
        If (txtTitre.Text = "") Or (txtContenu.Text = "") Then
            ''Utilisation de l'animation de la barre de statut
            statut.Content = "Un de vos champs est vide"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            Return
        End If

        ''Création d'un objet tblActualite
        nouvelleActualite = New tblActualite With _
{
.TitreActu = txtTitre.Text, _
.TexteActu = txtContenu.Text, _
.AuteurActu = "Admin"
}

        Try
            ''Tentative d'ajout de l'objet TblActualite dans la BD
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
            'Début de l'animation de la barre de statut

            statut.Content = "Impossible de modifier cette actualité"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try
        txtActu.IsEnabled = False
        btnSavee.Visibility = Windows.Visibility.Hidden
    End Sub

    'Force le cotenu du titre à disparaitre si précédement il n'a pas été fermé lors du changement d'onglet
    Private Sub TabItem_MouseDown(sender As Object, e As MouseButtonEventArgs)
        txtActu.Visibility = Windows.Visibility.Hidden
    End Sub


End Class
