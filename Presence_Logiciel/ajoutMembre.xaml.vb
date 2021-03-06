﻿Imports System.Text.RegularExpressions
Imports System.Windows.Media.Animation
Public Class ajoutMembre
    Public DM As PresenceEntities
    Dim newMembre As tblMembre
    Public statut As Label



    Private Sub btnAjouter_Click(sender As Object, e As RoutedEventArgs) Handles btnAjouter.Click
        Dim anim2 As Storyboard = FindResource("AnimTxtRouge")


        ''Verification du contenu des champs
        Dim myRegex1 As New Regex( _
"^[a-zA-Z]+$")
        If (myRegex1.IsMatch(txtPrenom.Text) = False) Then
            ''Debut des animations
            statut.Content = "Un prénom doit contenir que des lettres"
            Dim anim As Storyboard = FindResource("AnimLabel")
            txtPrenom.BorderBrush = Brushes.Red
            anim2.Begin(txtPrenom)

            anim.Begin(statut)
            Return
        End If
        
        If (myRegex1.IsMatch(txtNom.Text) = False) Then
            statut.Content = "Un nom doit contenir que des lettres"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)
            txtNom.BorderBrush = Brushes.Red
            anim2.Begin(txtNom)
            Return
        End If

        Dim myRegex2 As New Regex( _
"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
        If (myRegex2.IsMatch(txtTelephone.Text) = False) Then
            statut.Content = "La structure du numéro de téléphone est mauvaise"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)

            txtTelephone.BorderBrush = Brushes.Red
            anim2.Begin(txtTelephone)
            Return
        End If

        Dim myRegex3 As New Regex( _
"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        If (myRegex3.IsMatch(txtCourriel.Text) = False) Then
            statut.Content = "L'email entré est invalide"
            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(statut)

            txtCourriel.BorderBrush = Brushes.Red
            anim2.Begin(txtCourriel)
            Return
        End If

        Dim myRegex4 As New Regex( _
"^\d{1,5}$")
        If (myRegex4.IsMatch(txtNoCivique.Text) = False) Then
            statut.Content = "Un numéro civique doit comprendre au moin 1 chiffre et aucune lettre"
            Dim anim As Storyboard = FindResource("AnimLabel")

            txtNoCivique.BorderBrush = Brushes.Red
            anim2.Begin(txtNoCivique)
            anim.Begin(statut)
            Return
        End If

        ''Si tout est ok, création d'un objet tblMembre
        newMembre = New tblMembre With _
{
    .AdresseMembre = txtAdresse.Text, _
    .CourrielMembre = txtCourriel.Text, _
    .NoCiviqueMembre = txtNoCivique.Text, _
    .NomMembre = txtNom.Text, _
    .PrenomMembre = txtPrenom.Text, _
    .TelephoneMembre = txtTelephone.Text, _
    .VilleMembre = cmbVille.SelectionBoxItem.ToString
}


        Try
            ''Tentative d'ajout dans la BD

            DM.tblMembre.AddObject(newMembre)

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        ''S'il est ajouté dans la BD donc  si le radio button Professeur est coché :
        If (rdProfesseur.IsChecked = True) Then
            Dim unprof As New MembreProf
            unprof.LeNouveau = newMembre
            unprof.DM = DM
            unprof.statut = statut
            ''Ouverture de la fenêtre membreprof
            unprof.ShowDialog()
            Me.Close()
            Me.Finalize()

            ''Sinon si c'est l'étudiant
        Else
            Dim unEtudiant As New membreEtudiant
            unEtudiant.LeNouveau = newMembre
            unEtudiant.statut = statut
            unEtudiant.DM = DM
            ''Ouverture de la fenêtre MembreEtudiant
            unEtudiant.ShowDialog()
            Me.Close()
            Me.Finalize()

        End If

    End Sub





End Class
