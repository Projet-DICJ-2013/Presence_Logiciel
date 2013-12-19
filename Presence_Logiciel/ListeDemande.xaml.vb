Imports System
Imports System.IO
Imports System.Linq


Class ListeDemande
    Public Class DemandeExemplaireMembre
        Property Demande As tblDemande
        Property Exemplaire As tblExemplaire
        Property Membre As tblMembre
    End Class

    Private BD As PresenceEntities
    Private req As IQueryable(Of DemandeExemplaireMembre)
    Private reqFiltre As IQueryable(Of DemandeExemplaireMembre)
    Private _Statut As Label

    Public Sub New(Statut As Label)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _Statut = Statut
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        BD = New PresenceEntities
        Charger_Liste()

    End Sub


    Private Sub Charger_Liste()
        req = (From d In BD.tblDemande
            Join m In BD.tblMembre
            On m.IdMembre Equals d.IdMembre
            Where d.TypeEtat <> "Supprimé"
      Select New DemandeExemplaireMembre With {.Demande = d, .Membre = m})
        grdListeDemande.ItemsSource = req
    End Sub

    Private Sub btnFiltre_Click(sender As Object, e As RoutedEventArgs) Handles btnModele.Click, btnActif.Click, btnInnactif.Click
        Dim boutton As Button = sender
        Select Case (boutton.Name)
            Case "btnModele"
                reqFiltre = req.Where(Function(d) d.Demande.TypeEtat.Contains(""))
            Case Else
                reqFiltre = req.Where(Function(d) d.Demande.TypeEtat.Contains(boutton.Content))
        End Select
        grdListeDemande.ItemsSource = reqFiltre
    End Sub

    Private Sub btnRecherche_Click(sender As Object, e As RoutedEventArgs) Handles btnRecherche.Click

        grdListeDemande.ItemsSource = reqFiltre
    End Sub

    Private Sub btnSupression_Click(sender As Object, e As RoutedEventArgs) Handles btnSupression.Click
    End Sub
    Private Sub MenuContextuelEx(j As String)



    End Sub

End Class
