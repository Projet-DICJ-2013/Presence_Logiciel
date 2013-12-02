
Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions


Class gestCours

    Dim DM As PresenceEntities
    Dim LesCours As IQueryable(Of tblCours)
    Dim vu As ListCollectionView
    Dim lecours As tblCours
    Dim tCours As IQueryable(Of tblCours)
    Dim NomStatutCours As String
    Dim CodeStatutCours As String
    Public statut As Label
    Dim StatutVisible As Boolean










    Private Sub affAjoutCours(sender As Object, e As RoutedEventArgs)
        Dim uncours As New ajoutCour

        uncours.DM = DM
        uncours.statut = statut
        uncours.ShowDialog()
        LesCours = (From cours In DM.tblCours Select cours)
        vu = New ListCollectionView(LesCours.ToList())
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu





    End Sub

    Private Sub test(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        DM = New PresenceEntities
        LesCours = (From cours In DM.tblCours Select cours)

        vu = New ListCollectionView(LesCours.ToList())
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu



        afficherStatut()


    End Sub



    Private Sub AfficherSuivant(sender As Object, e As MouseButtonEventArgs) Handles btnSuivant.MouseDown
        If (vu.CurrentPosition < LesCours.Count() - 1) Then
            vu.MoveCurrentToNext()
            afficherStatut()

        End If
    End Sub

    Private Sub AfficherPrecedent(sender As Object, e As MouseButtonEventArgs) Handles btnPrecedent.MouseDown
        If (vu.CurrentPosition > 0) Then
            vu.MoveCurrentToPrevious()
            afficherStatut()
        End If
    End Sub

    Private Sub AfficherDernier(sender As Object, e As MouseButtonEventArgs) Handles btnDernier.MouseDown
        vu.MoveCurrentToLast()
        afficherStatut()


    End Sub

    Private Sub AfficherPremier(sender As Object, e As MouseButtonEventArgs) Handles btnPremier.MouseDown
        vu.MoveCurrentToFirst()
        afficherStatut()
    End Sub



    Private Sub SupprimerElement(sender As Object, e As MouseButtonEventArgs) Handles btnSupprimer.MouseDown

        Try
            Dim SupprTest As IQueryable(Of tblCours) = (From c In DM.tblCours Where c.CodeCours = txtNumCours.Text Select c)
            Dim CoursASupprimer As tblCours = SupprTest.First()
            DM.DeleteObject(CoursASupprimer)
            DM.SaveChanges()


        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer ce cours car il est actif à un programme de la session courrante")
        Catch ex As Exception
            statut.Content = "Impossible de supprimer ce cours car il est actif à un programme de la session courrante"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            'Dit l 'erreur
        End Try
        LesCours = (From cours In DM.tblCours Select cours)
        vu = New ListCollectionView(LesCours.ToList())
        txtNomCours.DataContext = vu
        txtAnneeCours.DataContext = vu
        txtDescription.DataContext = vu
        txtNumCours.DataContext = vu
        txtPonderation.DataContext = vu



    End Sub

    Private Sub Enregistrer(sender As Object, e As MouseButtonEventArgs) Handles btnEnregistrer.MouseDown
        Dim newStatut As tblStatutCoursCours
        txtAnneeCours.IsEnabled = False
        txtDescription.IsEnabled = False
        txtNomCours.IsEnabled = False
        txtPonderation.IsEnabled = False
        btnfontEnregistrer.Visibility = Windows.Visibility.Hidden
        btnEnregistrer.Visibility = Windows.Visibility.Hidden
        btnO.Visibility = Windows.Visibility.Hidden
        btnSuivant.IsEnabled = True
        btnPrecedent.IsEnabled = True
        btnPremier.IsEnabled = True
        btnDernier.IsEnabled = True



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



        newStatut = New tblStatutCoursCours With _
       {
           .CodeCours = txtNumCours.Text, _
           .DateAcquisitionStatut = DateTime.Now().ToString, _
           .NomStatutCours = lblStatutCours.Content
       }


        Try
            Dim modTest As IQueryable(Of tblCours) = (From c In DM.tblCours Where c.CodeCours = txtNumCours.Text Select c)
            Dim CoursAmodifier As tblCours = modTest.First()

            CoursAmodifier.AnneeCours = txtAnneeCours.Text
            CoursAmodifier.DescriptionCours = txtDescription.Text
            CoursAmodifier.NomCours = txtNomCours.Text
            CoursAmodifier.PonderationCours = txtPonderation.Text

            If Not (NomStatutCours = lblStatutCours.Content And CodeStatutCours = txtNumCours.Text) Then
                DM.tblStatutCoursCours.AddObject(newStatut)
            End If

            DM.SaveChanges()
            If (StatutVisible = True) Then
                Dim anim As Storyboard = FindResource("AnimRectFin")

                anim.Begin(recActif)
                anim.Begin(recAnnulé)
                anim.Begin(recInactif)
                anim.Begin(lblActif)
                anim.Begin(lblAnnule)
                anim.Begin(lblInactif)
            End If

        Catch ex2 As System.Data.SqlClient.SqlException
            MessageBox.Show("Impossible de supprimer ce cours car il est actif à un programme de la session courrante")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            'Dit l 'erreur
        End Try

        Page_Loaded(sender, e)
        afficherStatut()



    End Sub

    Private Sub ModifierCours(sender As Object, e As MouseButtonEventArgs) Handles btnEdit.MouseDown
        btnfontEnregistrer.Visibility = Windows.Visibility.Visible
        btnEnregistrer.Visibility = Windows.Visibility.Visible
        btnSuivant.IsEnabled = False
        btnPrecedent.IsEnabled = False
        btnPremier.IsEnabled = False
        btnDernier.IsEnabled = False

        NomStatutCours = lblStatutCours.Content
        CodeStatutCours = txtNumCours.Text

        btnO.Visibility = Windows.Visibility.Visible
        txtAnneeCours.IsEnabled = True
        txtDescription.IsEnabled = True
        txtNomCours.IsEnabled = True
        txtPonderation.IsEnabled = True
        StatutVisible = False


    End Sub

    Private Sub afficherStatut()
        Try


            lblStatutCours.DataContext = Nothing
            lblDateAcquisition.DataContext = Nothing
            Dim eCours = (From l In DM.tblStatutCoursCours Where l.CodeCours = CType(vu.CurrentItem, tblCours).CodeCours Order By l.DateAcquisitionStatut Descending Select l)
            Dim leecour As tblStatutCoursCours
            leecour = eCours.FirstOrDefault
            lblStatutCours.DataContext = CType(leecour, tblStatutCoursCours)
            lblDateAcquisition.DataContext = CType(leecour, tblStatutCoursCours)
        Catch ex As Exception

            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnO_Click(sender As Object, e As RoutedEventArgs) Handles btnO.Click


        If (recActif.Opacity = 0) Then
            Dim anim1 As Storyboard = FindResource("AnimRect1")
            Dim anim2 As Storyboard = FindResource("AnimRect2")
            Dim anim3 As Storyboard = FindResource("AnimRect3")

            anim1.Begin(recActif)
            anim1.Begin(lblActif)
            anim2.Begin(recAnnulé)
            anim2.Begin(lblAnnule)
            anim3.Begin(lblInactif)
            anim3.Begin(recInactif)
            StatutVisible = True

        Else
            Dim anim As Storyboard = FindResource("AnimRectFin")

            anim.Begin(recActif)
            anim.Begin(recAnnulé)
            anim.Begin(recInactif)
            anim.Begin(lblActif)
            anim.Begin(lblAnnule)
            anim.Begin(lblInactif)
            StatutVisible = False
        End If
    End Sub

    Private Sub recActif_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles recActif.MouseDown, recAnnulé.MouseDown, recInactif.MouseDown
        lblStatutCours.Content = lblActif.Content

        Dim anim As Storyboard = FindResource("AnimRectFin")

        anim.Begin(recActif)
        anim.Begin(recAnnulé)
        anim.Begin(recInactif)
        anim.Begin(lblActif)
        anim.Begin(lblAnnule)
        anim.Begin(lblInactif)
    End Sub

    Private Sub recInactif_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles recInactif.MouseDown
        lblStatutCours.Content = lblInactif.Content

    End Sub

    Private Sub recAnnulé_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles recAnnulé.MouseDown
        lblStatutCours.Content = lblAnnule.Content
    End Sub



    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()

    End Sub



    Private Sub txtNomCours_PreviewLostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs) Handles txtNomCours.PreviewLostKeyboardFocus, txtAnneeCours.PreviewLostKeyboardFocus, txtPonderation.PreviewLostKeyboardFocus
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
