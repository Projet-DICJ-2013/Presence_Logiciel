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
    Private _reunion As objReunion

    'Evenement qui appelle la fonction d'envoie de courrier
    Private Sub btnEnvoyer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnvoyer.Click

        If ((txtObj.Text IsNot Nothing) And (rctMessage.Document IsNot Nothing)) Then
            CreerMail()
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
        _envoieMail = New objSmtp("Dicj@outlook.fr", "Dicj@outlook.fr", txtObj.Text, "", _tblConstante.Item(0).AdresseEmail, _tblConstante.Item(0).MotdePasse, _texteSujet)

        For Each invites In _listeAdresse
            _envoieMail.AddCC(invites.CourrielMembre)
            _nbrMail += _nbrMail
            If (_nbrMail = 10) Then
                _envoieMail.Envoie_Reset()
                _nbrMail = 0
            End If
        Next
        _rapport.CreerRapportOrd(_idOrdre, True)
        _envoieMail.AddPieceJointe(_rapport.TempFilePDF)
        _envoieMail.EnvoiMessage()

    End Sub

    Private Sub Envoie_Loaded(sender As Object, e As RoutedEventArgs) Handles intEnvoieMail.Loaded
        Dim ID = _reunion.GetId()
        Dim titre = (From Odj In _entitiesReunion.tblOrdreDuJour
                      Where Odj.NoOrdreDuJour = ID
                      Select Odj.TitreOrdreJour)
        txtObj.Text = titre.ToString()
    End Sub
End Class
