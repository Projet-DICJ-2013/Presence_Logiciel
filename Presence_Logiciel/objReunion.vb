Public Class objReunion
    'Déclaration des variables privés
    Private _entitiesReunion As PresenceEntities
    Private _lstmembres As List(Of tblMembre)
    Private _reunion As tblReunion
    Private _membreParticipant As tblMembreParticipantReunion
    Private _lstRedac As List(Of tblMembre)
    Private _intMail As EnvoieMail
    Private _invites As List(Of tblMembre)
    Private _idOrdre

    'Déclaration de l'interface d'envoie de courriels 
    Public Sub New()

        _entitiesReunion = New PresenceEntities
        _reunion = New tblReunion
        _membreParticipant = New tblMembreParticipantReunion

    End Sub
    'Fonction permetant de charger la liste de membres selon le choix fait.
    Public Function Chargerliste(Type As Int16, Optional Année As Int16 = 50)

        If (Type = 0) Then
            'Étudiant selon année
            Select Case Année
                Case 0
                    Année = 1
                Case 1
                    Année = 2
                Case 2
                    Année = 3
            End Select
            _lstmembres = (From membre In _entitiesReunion.tblMembre Join etudiant In _entitiesReunion.tblEtudiant On etudiant.IdMembre Equals membre.IdMembre Where etudiant.Annee = Année Select membre).ToList()
        Else
            'Enseignant
            _lstmembres = (From membre In _entitiesReunion.tblMembre Join prof In _entitiesReunion.tblProfesseur On prof.IdMembre Equals membre.IdMembre Select membre).ToList()

        End If

        Return _lstmembres

    End Function
    'Ajout les données de la réunion a la bd
    Public Function ajoutReunionBd(dateReunion As Date, Endroit As String, nolocal As String, nooredredujour As Int32)

        _idOrdre = nooredredujour
        _reunion.DateReunion = dateReunion
        _reunion.EndroitReunion = Endroit

        If (nolocal IsNot Nothing) Then
            _reunion.NoLocal = nolocal
        End If

        _reunion.NoOrdreDuJour = nooredredujour

        _entitiesReunion.tblReunion.AddObject(_reunion)
        _entitiesReunion.SaveChanges()



        Return 0

    End Function
    'Ajout les membres invités à la réunion.
    Public Function ajoutMembreParticipantBd(invites As List(Of tblMembre), redacteur As tblMembre)
        Dim numDerReu As List(Of tblReunion)
        Dim nombre As Int16
        numDerReu = (From reu In _entitiesReunion.tblReunion Select reu Order By reu.NoReunion).ToList

        nombre = numDerReu.Last().NoReunion


        For Each membreinvite In invites
            _membreParticipant = New tblMembreParticipantReunion
            _membreParticipant.IdMembre = membreinvite.IdMembre
            _membreParticipant.NoReunion = nombre
            _membreParticipant.IdTypeMembre = "Participant"

            _entitiesReunion.tblMembreParticipantReunion.AddObject(_membreParticipant)

            _entitiesReunion.SaveChanges()
        Next
        _membreParticipant = New tblMembreParticipantReunion
        _membreParticipant.IdMembre = redacteur.IdMembre
        _membreParticipant.NoReunion = nombre
        _membreParticipant.IdTypeMembre = "Rédacteur"

        _entitiesReunion.tblMembreParticipantReunion.AddObject(_membreParticipant)
        _entitiesReunion.SaveChanges()
        _invites = invites
        Return 0
    End Function
    'Permet d'ouvrir ceux qui pouront être rédacteur.
    Public Function loadListRedac(typemembre As Int16)
        If (typemembre = 0) Then
            _lstRedac = (From membre In _entitiesReunion.tblMembre Join prof In _entitiesReunion.tblProfesseur On prof.IdMembre Equals membre.IdMembre Select membre).ToList()
        Else
            _lstRedac = (From membre In _entitiesReunion.tblMembre Join etu In _entitiesReunion.tblEtudiant On etu.IdMembre Equals membre.IdMembre Select membre).ToList()

        End If


        Return _lstRedac

    End Function
    'Ouvre l'interface d'envoie de courriers 
    Public Sub OuvrirMail()

        _intMail = New EnvoieMail(_invites, _idOrdre)
        _intMail.ShowDialog()

    End Sub
End Class
