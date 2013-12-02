Imports System
Imports System.IO
Imports System.Linq


Class ListeExemplaire
    Public Class ExemplaireModele
        Property Exemplaire As tblExemplaire
        Property Modele As tblModele
    End Class

    Private BD As PresenceEntities
    Private req As IQueryable(Of ExemplaireModele)
    Private reqFiltre As IQueryable(Of ExemplaireModele)

    Private Sub btnAddExe_Click(sender As Object, e As RoutedEventArgs) Handles btnAddExe.Click
        Dim fnExemp As New frmExemplaire
        fnExemp.Show()
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        BD = New PresenceEntities
        Charger_Liste()

    End Sub


    Private Sub Charger_Liste()
        req = (From r In BD.tblExemplaire
            Join m In BD.tblModele
            On m.NoModele Equals r.NoModele
            Where r.TypeEtat <> "Supprimé"
      Select New ExemplaireModele With {.Exemplaire = r, .Modele = m})
        grdListeExemplaire.ItemsSource = req
    End Sub

    Private Sub btnFiltre_Click(sender As Object, e As RoutedEventArgs) Handles btnModele.Click, btnDisponible.Click, btnReserve.Click, btnPrete.Click, btnBrise.Click, btnReparation.Click, btnRetard.Click, btnRemplacant.Click, btnAReimager.Click, btnHorsService.Click, btnPerdu.Click
        Dim boutton As Button = sender
        Select Case (boutton.Name)
            Case "btnModele"
                reqFiltre = req.Where(Function(r) r.Exemplaire.TypeEtat.Contains(""))

            Case "btnAReimager"
                reqFiltre = req.Where(Function(r) CType(r.Exemplaire.AReimager, String).Contains("1"))
            Case "btnReserve"
                reqFiltre = req.Where(Function(r) r.Exemplaire.TypeEtat.Contains("Reserv"))
            Case Else
                reqFiltre = req.Where(Function(r) r.Exemplaire.TypeEtat.Contains(boutton.Content))
        End Select
        grdListeExemplaire.ItemsSource = reqFiltre
    End Sub

    Private Sub btnRecherche_Click(sender As Object, e As RoutedEventArgs) Handles btnRecherche.Click
        reqFiltre = req.Where(Function(r) r.Exemplaire.TypeEtat.Contains(txtRecherche.Text) _
                                  Or r.Exemplaire.NoModele.Contains(txtRecherche.Text) _
                                  Or r.Exemplaire.NomReseau.Contains(txtRecherche.Text) _
                                  Or r.Modele.TypeMachine.Contains(txtRecherche.Text) _
                                  Or CType(r.Exemplaire.CodeBarre, String).Contains(txtRecherche.Text))
        grdListeExemplaire.ItemsSource = reqFiltre
    End Sub


    Private Sub Button_save(sender As Object, e As RoutedEventArgs) Handles btn_save.Click
        BD.SaveChanges()
    End Sub

    Private Sub btnSupression_Click(sender As Object, e As RoutedEventArgs) Handles btnSupression.Click
        Dim confirmation = MessageBox.Show("Êtes-vous sûre de vouloir supprimé cet exemplaire?", "Suppression d'un exemplaire", MessageBoxButton.YesNo)
        If confirmation = MessageBoxResult.Yes Then
            MessageBox.Show("Suppression réussie", "Résultat")
            Dim _req = From ex In BD.tblExemplaire
           Where ex.CodeBarre = CType(grdListeExemplaire.SelectedItem, ExemplaireModele).Exemplaire.CodeBarre
            _req.FirstOrDefault.TypeEtat = "Supprimé"
            BD.SaveChanges()
        End If
    End Sub
End Class
