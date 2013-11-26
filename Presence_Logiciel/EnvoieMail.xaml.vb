Imports mod_smtp
Imports System.IO
Public Class EnvoieMail

    'Declaration des données privées 
    Private _listeAdresse As List(Of tblMembre)
    Private _envoieMail As objSmtp
    Private _tblConstante As List(Of tblConstant)
    Private _entitiesReunion As PresenceEntities
    Private _texteSujet As TextRange
    Private _nbrMail As Int16
    Private _rapport As GenereRapport
    Private _idOrdre As Integer

    'Evenement qui appelle la fonction d'envoie de courrier
    Private Sub btnEnvoyer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnvoyer.Click

        If ((txtObj.Text IsNot Nothing) And (rctMessage.Document IsNot Nothing)) Then
            CreerMail()
            _envoieMail.EnvoiMessage()
            Me.Close()
        Else
            MessageBox.Show("Veuillez remplir tous les champs.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

    End Sub
    'Initialise certain composant a l'aide de données venant de l'interface précédente
    Public Sub New(listeinviter As List(Of tblMembre), idOrdre As Integer)

        _idOrdre = idOrdre
        InitializeComponent()
        _listeAdresse = listeinviter
        _entitiesReunion = New PresenceEntities
        _rapport = New GenereRapport

    End Sub
    'Creer l'objet mail et l'envoie
    Public Sub CreerMail()
        _nbrMail = 0
        _texteSujet = New TextRange(rctMessage.Document.ContentStart, rctMessage.Document.ContentEnd)
        _tblConstante = (From constant In _entitiesReunion.tblConstant Select constant).ToList()
        _envoieMail = New objSmtp("dicj@cjonquiere.qc.ca", "dicj@cjonquiere.qc.ca", txtObj.Text, "", _tblConstante.Item(0).AdresseEmail, _tblConstante.Item(0).MotdePasse, _texteSujet)
        _rapport.CreerRapportOrd(_idOrdre)
        _envoieMail.AddPieceJointe(_rapport.TempFilePDF)
        File.Delete(_rapport.TempFilePDF)
        For Each invites In _listeAdresse
            _envoieMail.AddDestinataire(invites.CourrielMembre)
            _nbrMail += _nbrMail
            If (_nbrMail = 10) Then
                EnvoieMail.Envoie_Reset()

            End If
        Next

    End Sub
End Class
