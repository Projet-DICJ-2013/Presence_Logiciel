Imports System
Imports System.Text
Imports System.IO
Imports System.Xml.Linq
Imports System.Collections.Generic
Imports System.Xml
Imports P2013_CreateDoc


Public Class frmGenereRapport

    Private MesRapports As GenereRapport
    Private MonRapport As P2013_CreateDoc.CreateReport
    Private MonStyle As ModeleStyle
    Private MonPDF As P2013_CreateDoc.GenerePdf
    Private Path As String = Directory.GetCurrentDirectory + "\"
    Private Elem As Rapport

    Private Sub btnGenerer_Click(sender As Object, e As RoutedEventArgs) Handles btnGenerer.Click

        Elem = New Rapport
        MesRapports = New GenereRapport

        For Each El In LstRapport.Items

            Elem = El

            If Elem.Type = "Ordre du jour" Then
                MesRapports.CreerRapportOrd(Elem.Id)
            End If

            If Elem.Type = "Liste des matériel" Then
                MesRapports.CreerRapportMat(Elem.Id)
            End If

            If Elem.Type = "Liste des étudiants inscrits à un cours" Then
                MesRapports.CreerRapportCours(Elem.Id)
            End If
        Next
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub btnAddRapport_Click(sender As Object, e As RoutedEventArgs) Handles btnAddRapport.Click
        If (txtIdElem.Text IsNot Nothing And CmbTypeRapport.SelectedItem IsNot Nothing) Then
            LstRapport.Items.Add(New Rapport With {.Id = txtIdElem.Text, .Type = CmbTypeRapport.Text})
        End If
    End Sub

    Private Sub btnSupRapport_Click(sender As Object, e As RoutedEventArgs) Handles btnSupRapport.Click
        LstRapport.Items.RemoveAt(LstRapport.SelectedIndex)
    End Sub

    Private Sub OnQuit(sender As Object, e As RoutedEventArgs) Handles MyBase.Closed
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()
    End Sub
End Class

Public Class Rapport
    Public Property Type As String
    Public Property Id As String

End Class

