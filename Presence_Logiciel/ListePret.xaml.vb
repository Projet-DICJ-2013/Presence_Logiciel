Imports System
Imports System.IO
Imports System.Linq




Class ListePret
    Public Class PretExemplaireMembre
        Property Exemplaire As tblExemplaire
        Property Pret As tblPret
        Property Membre As tblMembre
        Property PretExemplaire As tblPretExemplaire
    End Class

    Private BD As PresenceEntities
    Private req As IQueryable(Of PretExemplaireMembre)
    Private reqFiltre As IQueryable(Of PretExemplaireMembre)

    Private Sub test1()
        MessageBox.Show("test")
    End Sub

    Private Sub btnAddExe_Click(sender As Object, e As RoutedEventArgs) Handles btnAddExe.Click
        Dim fnPret As New Pret
        fnPret.ShowDialog()
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        BD = New PresenceEntities
        Charger_Liste()
    End Sub


    Private Sub Charger_Liste()
        req = (From p In BD.tblPret
                Join m In BD.tblMembre
                On p.IdMembre Equals m.IdMembre
                Join pe In BD.tblPretExemplaire
                On pe.IdPret Equals p.IdPret
                Join e In BD.tblExemplaire
                On e.CodeBarre Equals pe.CodeBarre
                Where p.TypeEtat <> "Supprimé"
                Select New PretExemplaireMembre With {.Exemplaire = e, .Membre = m, .Pret = p, .PretExemplaire = pe})
        grdListePret.ItemsSource = req
    End Sub

    Private Sub btnFiltre_Click(sender As Object, e As RoutedEventArgs) Handles btnTous.Click, btnActif.Click, btnInnactif.Click
        Dim boutton As Button = sender
        Select Case (boutton.Content)
            Case "Tous"
                reqFiltre = req.Where(Function(r) CType(r.Pret.TypeEtat, String).Contains(""))
            Case "Actif"
                reqFiltre = req.Where(Function(r) CType(r.Pret.TypeEtat, String).Equals("actif"))
            Case Else
                reqFiltre = req.Where(Function(r) CType(r.Pret.TypeEtat, String).Contains(boutton.Content))
        End Select

        grdListePret.ItemsSource = reqFiltre
    End Sub

    Private Sub btnRecherche_Click(sender As Object, e As RoutedEventArgs) Handles btnRecherche.Click
        reqFiltre = req.Where(Function(r) CType(r.Pret.IdPret, String).Contains(txtRecherche.Text) _
                                  Or CType(r.Membre.NomMembre, String).Contains(txtRecherche.Text) _
                                  Or CType(r.Membre.PrenomMembre, String).Contains(txtRecherche.Text) _
                                  Or CType(r.PretExemplaire.DateDebutPretEx, String).Contains(txtRecherche.Text) _
                                  Or CType(r.PretExemplaire.DateFinPretEx, String).Contains(txtRecherche.Text) _
                                  Or CType(r.Pret.TypeEtat, String).Contains(txtRecherche.Text))
        grdListePret.ItemsSource = reqFiltre
    End Sub


    Private Sub btnSupression_Click(sender As Object, e As RoutedEventArgs) Handles btnSupression.Click

        Dim confirmation = MessageBox.Show("Êtes-vous sûre de vouloir supprimé ce prêt?", "Suppression d'un prêt", MessageBoxButton.YesNo)
        If confirmation = MessageBoxResult.Yes Then
            MessageBox.Show("Suppression réussie", "Résultat")
            Dim _req = From p In BD.tblPret
           Where p.IdPret = CType(grdListePret.SelectedItem, PretExemplaireMembre).Pret.IdPret

            _req.FirstOrDefault.TypeEtat = "Supprimé"

            Dim _req2 = From p In BD.tblExemplaire
            Where p.CodeBarre = CType(grdListePret.SelectedItem, PretExemplaireMembre).PretExemplaire.CodeBarre
            For Each Exemplaire As tblExemplaire In _req2

                _req2.FirstOrDefault.TypeEtat = "Disponible"

            Next
            Try
                BD.SaveChanges()
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub Menu_contextuel(i As String, j As String, k As String)

        Dim confirmation = MessageBox.Show("le prèt va maintenant être " + k, "Modification d'un prêt", MessageBoxButton.YesNo)
        If confirmation = MessageBoxResult.Yes Then
            MessageBox.Show("l'élément est maintenant " + k, "Résultat")
            Dim _req = From p In BD.tblPret
           Where p.IdPret = CType(grdListePret.SelectedItem, PretExemplaireMembre).Pret.IdPret

            _req.FirstOrDefault.TypeEtat = i
            Dim _req2 = From p In BD.tblExemplaire
            Where p.CodeBarre = CType(grdListePret.SelectedItem, PretExemplaireMembre).PretExemplaire.CodeBarre
            For Each Exemplaire As tblExemplaire In _req2

                _req2.FirstOrDefault.TypeEtat = j

            Next
            BD.SaveChanges()
        End If
    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Menu_contextuel("Suprimé", "Disponible", "Suprimé")
    End Sub

    Private Sub miSuprimer_Click(sender As Object, e As RoutedEventArgs) Handles miSuprimer.Click
        Menu_contextuel("Supprimé", "Disponible", "Supprimé")
    End Sub

    Private Sub miTerminer_Click(sender As Object, e As RoutedEventArgs) Handles miTerminer.Click
        Menu_contextuel("Innactif", "Disponible", "Innactif")
    End Sub
End Class
