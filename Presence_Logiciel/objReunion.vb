Public Class objReunion

    Private _entitiesReunion As PresenceEntities
    Private _lstmembres As List(Of tblMembre)
    Private _reunion As tblReunion
    Private _membreParticipant As tblMembreParticipantReunion
    Private _lstProf As List(Of tblMembre)
    Private _intMail As EnvoieMail
    Private _invites As List(Of tblMembre)

    Public Sub New()
        _entitiesReunion = New PresenceEntities
        _reunion = New tblReunion

        _membreParticipant = New tblMembreParticipantReunion

    End Sub

    Public Function Chargerliste(Type As Int16, Optional Année As Int16 = 50)

        If (Type = 0) Then
            'Étudiant

            'Selon année
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
    Public Function ajoutReunionBd(dateReunion As Date, Endroit As String, nolocal As String, noordredujour As Integer)
        _reunion.DateReunion = dateReunion
        _reunion.EndroitReunion = Endroit
        _reunion.NoLocal = nolocal
        _reunion.NoOrdreDuJour = noordredujour

        _entitiesReunion.AddTotblReunion(_reunion)
        Try
            _entitiesReunion.SaveChanges()
        Catch ex As Exception

        End Try

        Return 0
    End Function
    Public Function ajoutMembreParticipantBd(invites As List(Of tblMembre), redacteur As tblMembre)
        Dim numDerReu As List(Of tblReunion)
        Dim nombre As Int16
        numDerReu = (From reu In _entitiesReunion.tblReunion Select reu).ToList()
        nombre = numDerReu.Last.NoReunion
      


        For Each membreinvite In invites
            _membreParticipant = New tblMembreParticipantReunion
            _membreParticipant.IdMembre = membreinvite.IdMembre
            _membreParticipant.NoReunion = nombre
            _membreParticipant.IdTypeMembre = "Participant"

            _entitiesReunion.AddTotblMembreParticipantReunion(_membreParticipant)
            _entitiesReunion.SaveChanges()
        Next
        _membreParticipant = New tblMembreParticipantReunion
        _membreParticipant.IdMembre = redacteur.IdMembre
        _membreParticipant.NoReunion = nombre
        _membreParticipant.IdTypeMembre = "Rédacteur"

        _entitiesReunion.AddTotblMembreParticipantReunion(_membreParticipant)
        _entitiesReunion.SaveChanges()
        _invites = invites
        Return 0
    End Function
    Public Function loadListRedac()
        _lstProf = (From membre In _entitiesReunion.tblMembre Join prof In _entitiesReunion.tblProfesseur On prof.IdMembre Equals membre.IdMembre Select membre).ToList()

        Return _lstProf
    End Function
    Public Sub OuvrirMail()
        _intMail = New EnvoieMail(_invites)
        _intMail.Show()

    End Sub
End Class
