﻿Imports mod_smtp
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
    Private _titre As String
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
    Public Sub New(listeinviter As List(Of tblMembre), idOrdre As Integer, titre As String, _DateReunion As Date)

        _idOrdre = idOrdre
        InitializeComponent()
        _listeAdresse = listeinviter
        _entitiesReunion = New PresenceEntities
        _rapport = New GenereRapport
        _titre = titre
        message(titre, _DateReunion)
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

    Private Sub message(titreOdj As String, _DateReunion As Date)
        txtObj.Text = titreOdj

        rctMessage.AppendText("Bonjour à tous, si vous avez reçu ce mail, celà signifie que vous êtes convivé à une réunion concernant " + titreOdj +
                              ". La réunion aura lieu en date du " + FormaterDate(_DateReunion) + ". ")
    End Sub

    Private Function FormaterDate(DateReunion As Date)
        Dim DateMois As String = DateReunion.Month.ToString()
        Select Case DateMois
            Case "1"
                DateMois = " Janvier "
            Case "2"
                DateMois = " Février "
            Case "3"
                DateMois = " Mars "
            Case "4"
                DateMois = " Avril "
            Case "5"
                DateMois = " Mai "
            Case "6"
                DateMois = " Juin "
            Case "7"
                DateMois = " Juillet "
            Case "8"
                DateMois = " Août "
            Case "9"
                DateMois = " Septembre "
            Case "10"
                DateMois = " Octobre "
            Case "11"
                DateMois = " Novembre "
            Case "12"
                DateMois = " Décembre "
        End Select
        Return DateReunion.Day.ToString() + DateMois + DateReunion.Year.ToString()
    End Function
End Class
