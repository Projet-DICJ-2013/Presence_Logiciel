Imports mod_smtp
Public Class EnvoieMail

    Private _listeAdresse As List(Of tblMembre)
    Private _envoieMail As objSmtp
    Private _tblConstante As List(Of tblConstant)
    Private _entitiesReunion As PresenceEntities
    Private _texteSujet As TextRange


    Private Sub btnEnvoyer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnvoyer.Click
        CreerMail()
        _envoieMail.EnvoiMessage()
    End Sub

    Public Sub New(listeinviter As List(Of tblMembre))

        ' Cet appel est requis par le concepteur.
        InitializeComponent()
        _listeAdresse = listeinviter
        _entitiesReunion = New PresenceEntities
        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub
    Public Sub CreerMail()
        _texteSujet = New TextRange(rctMessage.Document.ContentStart, rctMessage.Document.ContentEnd)

        _tblConstante = (From constant In _entitiesReunion.tblConstant Select constant).ToList()
        _envoieMail = New objSmtp("dicj@cjonquiere.qc.ca", "dicj@cjonquiere.qc.ca", "Ordre du Jour", _texteSujet, _tblConstante.Item(0).AdresseEmail, _tblConstante.Item(0).MotdePasse)
        For Each invites In _listeAdresse
            _envoieMail.AddDestinataire(invites.CourrielMembre)
        Next
    End Sub
End Class
