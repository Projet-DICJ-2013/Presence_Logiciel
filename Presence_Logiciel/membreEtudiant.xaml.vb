﻿Imports mod_smtp
Public Class membreEtudiant
    Public DM As PresenceEntities
    Dim newEtu As tblEtudiant
    Dim unEtu As IQueryable(Of tblEtudiant)
    Dim donnemoiunmembre As IQueryable(Of tblMembre)
    Dim vu As ListCollectionView
    Dim nouvelUser As tblLogin
    Dim _envoimail As objSmtp
    Private Sub btnConfirmer_Click(sender As Object, e As RoutedEventArgs) Handles btnConfirmer.Click
        Dim dateNow As Date
        dateNow = DateValue(Now)

        newEtu = New tblEtudiant With _
{
    .Annee = cbAnnee.SelectionBoxItem.ToString, _
    .DaEtudiant = txtDaEtudiant.Text, _
    .DateInscriptionEtudiant = dateNow, _
    .IdMembre = txtIdMembre.Text _
}


        Try

            DM.tblEtudiant.AddObject(newEtu)
            DM.SaveChanges()

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        createUser2(txtDaEtudiant.Text, (lblNom1.Content & lblPrenom.Content))
        Dim lstinfolog = (From tblconstant In DM.tblConstant Select tblconstant).ToList
        _envoimail = New objSmtp("dicj@cjonquiere.qc.ca", "dicj@cjonquiere.qc.ca", "Bienvenue au Cégep de Jonquière en Ligne ", ("votre mot de passe est :" & lblNom1.Content & lblPrenom.Content & " Vous devrez le changer lors de votre première visite"), lstinfolog.Item(0).AdresseEmail, lstinfolog.Item(0).MotdePasse)
        ''_envoimail.EnvoiMessage()
        _envoimail.AddDestinataire(lblAdresseMail.DataContext.ToString)
        _envoimail.EnvoiMessage()
    End Sub


    Private Function createUser2(ByVal user As String, ByVal password As String) As String
        Dim test As New FctConnexion

        Dim LesUser As IQueryable(Of tblLogin) = (From us In DM.tblLogin Where us.IdLogin = user Select us)
        If (LesUser.Count = 0) Then
            MsgBox("rien")


            Dim HPW As String = test.StringToMd5(password)


            nouvelUser = New tblLogin With _
{
 .IdLogin = txtDaEtudiant.Text, _
 .Administrateur = "False", _
 .Hash = HPW.ToString, _
.IdMembre = txtIdMembre.Text _
}
            Try

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

    Private Sub Image_MouseDown(sender As Object, e As MouseButtonEventArgs)
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Dim dateNow As Date
        dateNow = DateValue(Now)
        donnemoiunmembre = (From m In DM.tblMembre Select m)

        vu = New ListCollectionView(donnemoiunmembre.ToList())

        vu.MoveCurrentToLast()
        txtIdMembre.DataContext = vu
        lblNom1.DataContext = vu
        lblPrenom.DataContext = vu
        lblAdresseMail.DataContext = vu
        txtDateInscription.Text = dateNow

    End Sub
End Class
