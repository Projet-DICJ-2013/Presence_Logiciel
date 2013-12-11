Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions
Imports mod_smtp
Public Class membreEtudiant
    Public DM As PresenceEntities
    Dim newEtu As tblEtudiant
    Dim nouvelUser As tblLogin
    Public LeNouveau As tblMembre
    Dim _envoimail As objSmtp
    Public statut As Label

    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Dim dateNow As Date
        dateNow = DateValue(Now)
        Dim anim2 As Storyboard = FindResource("AnimTxtRouge")

        ''Validation du numéro de DA
        Dim myRegex1 As New Regex( _
"\d{7,9}")
        If (myRegex1.IsMatch(txtDaEtudiant.Text) = False) Then
            statut.Content = "Un numéro de DA doit comporter entre 7 et 9 chiffres"
            Dim anim As Storyboard = FindResource("AnimLabel")

            ''Début de l'animation
            txtDaEtudiant.BorderBrush = Brushes.Red
            anim2.Begin(txtDaEtudiant)
            anim.Begin(statut)
            Return
        End If


        ''Création d'un objet tblEtudiant
        newEtu = New tblEtudiant With _
{
    .Annee = cbAnnee.SelectionBoxItem.ToString, _
    .DaEtudiant = txtDaEtudiant.Text, _
    .DateInscriptionEtudiant = dateNow, _
    .IdMembre = LeNouveau.IdMembre _
}


        Try
            ''Tentative d'ajout dans la BD
            DM.tblEtudiant.AddObject(newEtu)
            DM.SaveChanges()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
        ''Si tout est ok, création d'un utilisateur pour le site web
        createUser2(txtDaEtudiant.Text, (lblNom1.Content & lblPrenom.Content))
        Dim lstinfolog = (From tblconstant In DM.tblConstant Select tblconstant).ToList
        ''L'utilisateur et le mot de passe temporaire est envoyé par email au professeur
        _envoimail = New objSmtp("dicj@outlook.fr", "dicj@outlook.fr", "Bienvenue au Cégep de Jonquière en Ligne ", ("votre numéro de DA est : " & txtDaEtudiant.Text & " votre mot de passe est :" & lblNom1.Content & lblPrenom.Content & " Vous devrez le changer lors de votre première visite"), lstinfolog.Item(0).AdresseEmail, lstinfolog.Item(0).MotdePasse)
        _envoimail.AddCC(LeNouveau.CourrielMembre)
        _envoimail.EnvoiMessage()

    End Sub

    ''Création d'utilisateur en MD5
    Private Function createUser2(ByVal user As String, ByVal password As String) As String
        Dim test As New FctConnexion

        Dim LesUser As IQueryable(Of tblLogin) = (From us In DM.tblLogin Where us.IdLogin = user Select us)
        If (LesUser.Count = 0) Then


            Dim HPW As String = test.StringToMd5(password)


            nouvelUser = New tblLogin With _
{
 .IdLogin = txtDaEtudiant.Text, _
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



    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim dateNow As Date
        dateNow = DateValue(Now)


        lblNom1.DataContext = LeNouveau
        lblPrenom.DataContext = LeNouveau
        txtDateInscription.Text = dateNow

    End Sub
End Class
