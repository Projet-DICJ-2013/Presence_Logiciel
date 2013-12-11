﻿Imports System.Data.Objects
Imports System.Windows.Media.Animation

Public Class frmModele

    Private Modele As GestionModele
    Private _Statut As Label
    Private _MsgErreur

    Public Sub New(Statut As Label)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _Statut = Statut
    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        Modele = New GestionModele

        SynchroControl()

    End Sub

    Private Sub SynchroControl(Optional ByVal CurrentPos As Integer = 0)

        Modele.Collection.MoveCurrentTo(CurrentPos)

        Try
            TxtMarque.DataContext = Modele.Collection
            TxtModele.DataContext = Modele.Collection
            TxtType.DataContext = Modele.Collection
            TxtGaranti.DataContext = Modele.Collection
            TxtPrix.DataContext = Modele.Collection
            UpdateComposante()
            TxtRech.Text = "Saisir un critère de recherche"
        Catch ex As Exception
            _MsgErreur = "Un problème est survenu lors de la synchronisation des données!"
            AffMsgErr()
        End Try

    End Sub

    Private Sub btnFirst_Click(sender As Object, e As RoutedEventArgs) Handles btnFirst.Click
        Modele.Collection.MoveCurrentToFirst()
        UpdateComposante()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As RoutedEventArgs) Handles btnPrevious.Click
        Modele.Collection.MoveCurrentToPrevious()
        UpdateComposante()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As RoutedEventArgs) Handles btnNext.Click
        Modele.Collection.MoveCurrentToNext()
        UpdateComposante()
    End Sub

    Private Sub btnLast_Click(sender As Object, e As RoutedEventArgs) Handles btnLast.Click
        Modele.Collection.MoveCurrentToLast()
        UpdateComposante()
    End Sub

    Private Sub UpdateComposante()

        If Modele.Collection.CurrentItem IsNot Nothing Then
            ViewComposante.ItemsSource = CType(Modele.Collection.CurrentItem, tblModele).tblCompoModele
        Else
            ViewComposante.ItemsSource = Nothing
        End If

    End Sub

    Private Sub btnAddCompo_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCompo.Click

        If Modele.Collection.CurrentItem IsNot Nothing Then
            Dim FnComposante As New frmComposante(CType(Modele.Collection.CurrentItem, tblModele).NoModele, _Statut)
            FnComposante.ShowDialog()
            SynchroControl()
        Else
            _MsgErreur = "Pour ajouter une composante, vous devez sélectionner un modèle"
            AffMsgErr()
        End If
    End Sub

    Private Sub btnAddNewItem_Click(sender As Object, e As RoutedEventArgs) Handles btnAddNewItem.Click
        Dim tblModele As tblModele

        If TxtGaranti.Text = Nothing Or TxtMarque.Text = Nothing Or TxtModele.Text = Nothing Or TxtType.Text = Nothing Then
            MsgBox("Veuillez remplir tous les champs pour enregistrer un nouveau modèle")
            Return
        End If

        tblModele = New tblModele With {.NoModele = TxtModele.Text, _
                                                .Marque = TxtMarque.Text, _
                                                .NbAnneeGarantie = TxtGaranti.Text, _
                                                .TypeMachine = TxtMarque.Text, _
                                                .PrixModele = TxtPrix.Text}

        If (tblModele IsNot Nothing) Then
            _MsgErreur = Modele.AddModele(tblModele)
            AffMsgErr()
        End If
    End Sub

    Private Sub OnQuit(sender As Object, e As RoutedEventArgs) Handles MyBase.Closed
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub txtModele_LostFocus(sender As Object, e As RoutedEventArgs) Handles TxtModele.LostFocus
        If TxtModele.Text = "" Then
            TxtModele.BorderBrush = Brushes.Red
            TxtModele.Text = "0"
        Else
            TxtModele.BorderBrush = Brushes.Green
        End If
    End Sub

    Private Sub TxtRech_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles TxtRech.PreviewMouseDown
        If TxtRech.Text = "Saisir un critère de recherche" Then
            TxtRech.Text = " "
        End If
    End Sub


    Private Sub btnReset_Click(sender As Object, e As RoutedEventArgs) Handles btnReset.Click
        Modele = New GestionModele
        Modele.Collection.MoveCurrentToLast()

        Modele.Collection.MoveCurrentTo(Modele.Collection.CurrentPosition + 1)
    End Sub


    Private Sub btnRech_Click(sender As Object, e As RoutedEventArgs) Handles btnRech.Click

        Modele.GetModeleRech(TxtRech.Text)
        SynchroControl()

    End Sub

    Protected Sub AffMsgErr()

        Dim anim As Storyboard = FindResource("AnimLabel")

        _Statut.Content = _MsgErreur
        anim.Begin(_Statut)
    End Sub
End Class

Public Class GestionModele

    Private i
    Private BD As New PresenceEntities

    Public Property Collection As ListCollectionView
        Get
            Return i
        End Get
        Set(value As ListCollectionView)
            i = value
        End Set
    End Property

    Public Sub New()

        Dim MonMode = From Ex In BD.tblModele
                        Select Ex

        Collection = New ListCollectionView(MonMode.ToList())

    End Sub

    Public Function GetComposante(ByVal _Modele As String) As ICollection

        Dim MesCompo = From Mode In BD.tblModele
        Where Mode.NoModele = _Modele
        Select Mode.tblCompoModele

        Return MesCompo.ToList()
    End Function

    Public Sub GetModeleRech(ByVal Critere As Object)
        Dim req As IQueryable(Of tblModele) = From Mode In BD.tblModele
                                              Select Mode



        req.Where(Function(x) x.NoModele = Critere Or _
                              x.Marque = Critere Or _
                              x.TypeMachine = Critere)

        Collection = New ListCollectionView(req.ToList)
    End Sub

    Public Sub SelectItemByType(ByVal Type As String)

        Dim Collect = From Ex In BD.tblModele
                      Where Ex.TypeMachine = Type
                        Select Ex

        Collection = New ListCollectionView(Collect.ToList())

    End Sub

    Public Function AddModele(ByVal MonModele As tblModele) As String

        Dim ModeleChange As IQueryable(Of tblModele) = (From Mode In BD.tblModele
                                 Where Mode.NoModele = MonModele.NoModele
                                 Select Mode)
        Dim Modele As tblModele

        If ModeleChange.Count Then
            Modele = ModeleChange.First
            Modele = MonModele
        Else
            BD.AddTotblModele(MonModele)
        End If
        Try
            BD.SaveChanges()
        Catch ex As Exception
            Return "Les modifications ont échouées"
        End Try

        Return "Les modifications ont réussies"

    End Function



    'PARTIE DE LA VALIDATION DES ENTRÉES
    Private Function valider_modele()
        Dim verifie As Boolean = True

    End Function

    Private Function verifier_doublon_nomodele(ByVal nomod As String) As Boolean
        Dim modV = (From modB In BD.tblModele
                   Where modB.NoModele = nomod
                   Select modB)
        If modV.ToList.Count() > 0 Then
            Return False
        Else
            Return True
        End If
    End Function
End Class