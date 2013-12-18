Imports System.IO
Imports Microsoft.Office.Interop.Word
Imports Microsoft.Win32
Imports System.Windows.Xps.Packaging
Imports System.Windows.Media.Animation


Public Class GestionPDF

    Private Model As PresenceEntities
    Private Statut As Label
    Private IsType As Boolean
    Private IsId As Boolean
    Private Rapport As GenereRapport
    Private ListElem As ListId

    Public Property PStatut As Label
        Get
            Return Statut
        End Get
        Set(value As Label)
            Statut = value
        End Set
    End Property

    Private Sub BrowseButton_Click(sender As Object, e As RoutedEventArgs)
        Dim DocChoisi As String
        Dim newXPSDocumentName As String
        documentViewer1.Document = Nothing
        Dim IdRap As Integer

        Rapport = New GenereRapport

        If IsId = True And IsType = True Then

            Select Case txtTypeRap.Text
                Case "OrdreDuJour"
                    Rapport.CreerRapportOrd(txtIdRap.Text.Substring(0, 4))
                Case "ListeEtudiant"
                    Rapport.CreerRapportCours(txtIdRap.Text.Substring(0, 10))
                Case "Facture"
                    Rapport.CreerRapportMat(txtIdRap.Text.Substring(0, 4))
            End Select

            DocChoisi = Rapport.TempFilePDF & ".docx"
            newXPSDocumentName = Rapport.TempFilePDF & ".xps"

            documentViewer1.Document = ConvertirXPS(DocChoisi, newXPSDocumentName).GetFixedDocumentSequence()

        End If
    End Sub

    Private Function ConvertirXPS(ByVal wordDocName As String, ByVal xpsDocName As String) As XpsDocument

        Dim wordApplication As New Microsoft.Office.Interop.Word.Application()
        wordApplication.Documents.Add(wordDocName)

        Dim doc As Document = wordApplication.ActiveDocument
        Try
            doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS)
            wordApplication.Quit()

            Dim xpsDoc As New XpsDocument(xpsDocName, System.IO.FileAccess.Read)
            Return xpsDoc
        Catch exp As Exception
            Dim str As String = exp.Message
        End Try
        Return Nothing
    End Function

    Private Sub txtTypeRapport_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtTypeRap.LostFocus

        If txtTypeRap.Text <> "Facture" And txtTypeRap.Text <> "ListeEtudiant" And txtTypeRap.Text <> "OrdreDuJour" Then
            txtTypeRap.BorderBrush = Brushes.Red
            Statut.Content = "Le type de rapport doit être de " & "Facture" & " ou " & "ListeEtudiant" & " ou " & "OrdreDuJour"

            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(Statut)

            IsType = False


        Else
            txtTypeRap.BorderBrush = Brushes.Green
            IsType = True

            ListElem = New ListId(txtTypeRap.Text)

            txtIdRap.ItemsSource = ListElem.List
        End If

    End Sub

    Private Sub txtIdRapport_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtIdRap.LostFocus

        If txtIdRap.Text = "" Then
            txtIdRap.BorderBrush = Brushes.Red
            Statut.Content = "Ce champ ne peut être vide"

            Dim anim As Storyboard = FindResource("AnimLabel")
            anim.Begin(Statut)

            IsId = False
        Else
            txtIdRap.BorderBrush = Brushes.Green
            IsId = True
        End If

    End Sub

    Private Sub txtTypeRap_PreviewMouseDown(sender As Object, e As EventArgs) Handles txtTypeRap.PreviewMouseDown
        If txtTypeRap.Text = "Saisir le type du rapport" Then
            txtTypeRap.Text = ""
        End If
    End Sub

    Private Sub txtIdRap_PreviewMouseDown(sender As Object, e As EventArgs) Handles txtIdRap.PreviewMouseDown
        If txtIdRap.Text = "Saisir l'id du rapport" Then
            txtIdRap.Text = ""
        End If
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        txtTypeRap.Items.Add("Facture")
        txtTypeRap.Items.Add("ListeEtudiant")
        txtTypeRap.Items.Add("OrdreDuJour")
    End Sub
End Class

Public Class ListId

    Private _List As List(Of String)
    Private _Modele As New PresenceEntities

    Public Property List As List(Of String)
        Get
            Return _List
        End Get
        Set(value As List(Of String))
            _List = value
        End Set
    End Property


    Public Sub New(ByVal _Type As String)

        Select Case _Type
            Case "OrdreDuJour"
                GetOrdElem()
            Case "ListeEtudiant"
                GetCoursElem()
            Case "Facture"
                GetMatElem()
        End Select

    End Sub

    Public Sub GetOrdElem()
        Dim LstOrd As List(Of String) = (From Ord In _Modele.tblOrdreDuJour
                             Select Id = Ord.NoOrdreDuJour & " - " & Ord.TitreOrdreJour).ToList

        List = LstOrd
    End Sub

    Public Sub GetCoursElem()
        Dim LstCours As List(Of String) = (From Cours In _Modele.tblCours
                             Select Id = Cours.CodeCours & " - " & Cours.NomCours).ToList

        List = LstCours
    End Sub

    Public Sub GetMatElem()
        Dim LstMat As List(Of String) = (From Pret In _Modele.tblPret
                             Select Id = Pret.IdPret & "  ").ToList

        List = LstMat
    End Sub


End Class