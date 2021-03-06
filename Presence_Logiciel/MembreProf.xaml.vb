﻿Imports mod_smtp
Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class MembreProf
    Public DM As PresenceEntities
    Dim newProf As tblProfesseur
    Public LeNouveau As tblMembre
    Dim _envoimail As objSmtp
    Dim nouvelUser As tblLogin
    Public statut As Label


    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Dim RandGen As New Random
        Dim anim As Storyboard = FindResource("AnimLabel")
        Dim anim2 As Storyboard = FindResource("AnimTxtRouge")

        ''Vérification du contenu des champs
        Dim myRegex1 As New Regex( _
"[a-zA-z0-9]{3,15}")
        If (myRegex1.IsMatch(txtUtilisateur.Text) = False) Then
            ''Début des animations de la barre de statut
            statut.Content = "Nom d'utilisateur doit comprendre entre 3 et 15 lettres et chiffres"
            txtUtilisateur.BorderBrush = Brushes.Red
            anim2.Begin(txtUtilisateur)

            anim.Begin(statut)
            Return
        End If

        Dim myRegex2 As New Regex( _
"[1-2][2-4]")
        If (myRegex2.IsMatch(txtChargeTravail.Text) = False) Then
            ''Début des animations de la barre de statut
            statut.Content = "La charge de travail d'un professeur doit être située entre 12 et 25 heures"
            txtChargeTravail.BorderBrush = Brushes.Red
            anim2.Begin(txtChargeTravail)

            anim.Begin(statut)
            Return
        End If

        Dim myRegex3 As New Regex( _
"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        If (myRegex3.IsMatch(txtCourriel.Text) = False) Then
            ''Début des animations de la barre de statut
            statut.Content = "Le courriel est invalide"
            txtCourriel.BorderBrush = Brushes.Red
            anim2.Begin(txtCourriel)

            anim.Begin(statut)
            Return
        End If

        Dim myRegex4 As New Regex( _
"\d{3}.\d")
        If (myRegex4.IsMatch(txtNoBureau.Text) = False) Then
            ''Début des animations de la barre de statut
            statut.Content = "Un numéro de bureau doit être structuré 3x(chiffre) 1x(.) et 1x(chiffre)"
            txtNoBureau.BorderBrush = Brushes.Red
            anim2.Begin(txtNoBureau)
            anim.Begin(statut)
            Return
        End If

        Dim myRegex5 As New Regex( _
"\d{1,4}")
        If (myRegex5.IsMatch(txtPoste.Text) = False) Then
            ''Début des animations de la barre de statut
            statut.Content = "Un numéro de poste doit comprendre entre 1 et 4 chiffre "
            txtPoste.BorderBrush = Brushes.Red
            anim2.Begin(txtPoste)

            anim.Begin(statut)
            Return
        End If

        ''Si tout est correct, création d'un nouvel objet tblProfesseur
        newProf = New tblProfesseur With _
{
    .NoEmploye = RandGen.Next(50000, 80000).ToString, _
    .NoBureauProfesseur = txtNoBureau.Text, _
    .PosteTelephoneProfesseur = txtPoste.Text, _
    .ChargeTravailProfesseur = txtChargeTravail.Text, _
    .CourrielCegepProfesseur = txtCourriel.Text, _
    .IdMembre = LeNouveau.IdMembre _
}


        Try
            ''Tentative d'ajout d'un objet tblProfesseur
            DM.tblProfesseur.AddObject(newProf)
            DM.SaveChanges()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        ''Création d'un utilisateur pour le site web
        createUser2(txtUtilisateur.Text, (LeNouveau.NomMembre & LeNouveau.PrenomMembre))
        Dim lstinfolog = (From tblconstant In DM.tblConstant Select tblconstant).ToList

        ''Envoie d'un email à l'utilisateur comprenant le nom d'utilisateur et le mot de passe temporaire
        _envoimail = New objSmtp("dicj@outlook.fr", "dicj@outlook.fr", "Bienvenue au Cégep de Jonquière en Ligne ", ("Votre nom d'utilisateur est: " & txtUtilisateur.Text & "votre mot de passe temporaire est :" & LeNouveau.NomMembre & LeNouveau.PrenomMembre & " Vous devrez le changer lors de votre première visite"), lstinfolog.Item(0).AdresseEmail, lstinfolog.Item(0).MotdePasse)
        _envoimail.AddCC(newProf.CourrielCegepProfesseur)
        _envoimail.EnvoiMessage()

    End Sub

    ''Fonction servant à créer un utilisateur en  MD5
    Private Function createUser2(ByVal user As String, ByVal password As String) As String
        Dim test As New FctConnexion

        Dim LesUser As IQueryable(Of tblLogin) = (From us In DM.tblLogin Where us.IdLogin = user Select us)
        If (LesUser.Count = 0) Then
            MsgBox("rien")


            Dim HPW As String = test.StringToMd5(password)


            nouvelUser = New tblLogin With _
{
 .IdLogin = user, _
 .Administrateur = "False", _
 .Hash = HPW.ToString, _
.IdMembre = LeNouveau.IdMembre _
}
            Try
                DM.SaveChanges()
                DM.AddTotblLogin(nouvelUser)
                DM.SaveChanges()
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try

        Else
            Dim userTest As tblLogin = LesUser.First()
            MsgBox("existe deja")
        End If

        Return user
        Return password
    End Function



 
End Class
