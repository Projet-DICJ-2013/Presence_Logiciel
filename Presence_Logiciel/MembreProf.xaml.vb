Imports mod_smtp
Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class MembreProf
    Public DM As PresenceEntities
    Dim newProf As tblProfesseur
    Dim unmembre As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView
    Public LeNouveau As tblMembre
    Dim _envoimail As objSmtp
    Dim nouvelUser As tblLogin
    Public statut As Label


    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Dim RandGen As New Random


        Dim myRegex1 As New Regex( _
"[a-zA-z0-9]{3,15}")
        If (myRegex1.IsMatch(txtUtilisateur.Text) = False) Then
            statut.Content = "Nom d'utilisateur doit comprendre entre 3 et 15 lettres et chiffres"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

        Dim myRegex2 As New Regex( _
"[1-2][2-4]")
        If (myRegex2.IsMatch(txtChargeTravail.Text) = False) Then
            statut.Content = "La charge de travail d'un professeur doit être située entre 12 et 25 heures"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

        Dim myRegex3 As New Regex( _
"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        If (myRegex3.IsMatch(txtCourriel.Text) = False) Then
            statut.Content = "Le courriel est invalide"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

        Dim myRegex4 As New Regex( _
"\d{3}.\d")
        If (myRegex4.IsMatch(txtNoBureau.Text) = False) Then
            statut.Content = "Un numéro de bureau doit être structuré 3x(chiffre) 1x(.) et 1x(chiffre)"
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If

        Dim myRegex5 As New Regex( _
"\d{1,4}")
        If (myRegex5.IsMatch(txtPoste.Text) = False) Then
            statut.Content = "Un numéro de poste doit comprendre entre 1 et 4 chiffre "
            Dim anim As Storyboard = FindResource("AnimLabel")

            anim.Begin(statut)
            Return
        End If


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

            DM.tblProfesseur.AddObject(newProf)
            DM.SaveChanges()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        createUser2(txtUtilisateur.Text, (LeNouveau.NomMembre & LeNouveau.PrenomMembre))
<<<<<<< HEAD
        Dim lstinfolog = (From tblconstant In DM.tblConstant Select tblconstant).ToList
        _envoimail = New objSmtp("dicj@outlook.fr", "dicj@outlook.fr", "Bienvenue au Cégep de Jonquière en Ligne ", ("Votre nom d'utilisateur est: " & txtUtilisateur.Text & " votre mot de passe temporaire est :" & LeNouveau.NomMembre & LeNouveau.PrenomMembre & " Vous devrez le changer lors de votre première visite"), lstinfolog.Item(0).AdresseEmail, lstinfolog.Item(0).MotdePasse)
        _envoimail.AddCC(newProf.CourrielCegepProfesseur)
        _envoimail.EnvoiMessage()
=======
        'Dim lstinfolog = (From tblconstant In DM.tblConstant Select tblconstant).ToList
        '_envoimail = New objSmtp("dicj@cjonquiere.qc.ca", "dicj@cjonquiere.qc.ca", "Bienvenue au Cégep de Jonquière en Ligne ", ("votre mot de passe temporaire est :" & LeNouveau.NomMembre & LeNouveau.PrenomMembre & " Vous devrez le changer lors de votre première visite"), lstinfolog.Item(0).AdresseEmail, lstinfolog.Item(0).MotdePasse)
        '_envoimail.AddCC(newProf.CourrielCegepProfesseur)
        '_envoimail.EnvoiMessage()
>>>>>>> parent of 78c4571... Merge branch 'master' of https://github.com/Projet-DICJ-2013/Presence_Logiciel

    End Sub


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

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)


    End Sub
End Class
