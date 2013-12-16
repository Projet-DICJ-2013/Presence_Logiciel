Imports System.Data.Objects
Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

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

        SynchroControl(False)

    End Sub

    Private Sub SynchroControl(ByVal _IsCrit As Boolean, Optional ByVal CurrentPos As Integer = 0)

        If _IsCrit Then
            Modele = New GestionModele(TxtRech.Text)
        Else
            Modele = New GestionModele
        End If

        Modele.Collection.MoveCurrentToPosition(CurrentPos)

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
            SynchroControl(False, Modele.Collection.CurrentPosition)
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

        Dim regex1 As New Regex("\d,\d")

        If (regex1.IsMatch(TxtPrix.Text) = False) Then
            MsgBox("Le prix d'un Modele doit être dans le format suivant 000,000")
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

    Private Sub TxtRech_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs) Handles TxtRech.PreviewMouseDown
        If TxtRech.Text = "Saisir un critère de recherche" Then
            TxtRech.Text = ""
        End If
    End Sub


    Private Sub btnReset_Click(sender As Object, e As RoutedEventArgs) Handles btnReset.Click
        SynchroControl(False)
    End Sub


    Private Sub btnRech_Click(sender As Object, e As RoutedEventArgs) Handles btnRech.Click

        If TxtRech.Text IsNot Nothing Then
            SynchroControl(True)
        Else
            _MsgErreur = "Vous devez remplir le champ de recherche"
            AffMsgErr()
        End If
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

    Public Sub New(Optional ByVal Critere As String = Nothing)

        Dim MonMode = From Ex In BD.tblModele
                        Select Ex

        If Critere IsNot Nothing Then
            Collection = New ListCollectionView(MonMode.Where( _
                Function(x) x.NoModele.Contains(Critere) Or _
                            x.Marque.Contains(Critere) Or _
                            x.TypeMachine.Contains(Critere)).ToList)
            Return
        End If

        Collection = New ListCollectionView(MonMode.ToList)

    End Sub

    Public Function GetComposante(ByVal _Modele As String) As ICollection

        Dim MesCompo = From Mode In BD.tblModele
        Where Mode.NoModele = _Modele
        Select Mode.tblCompoModele

        Return MesCompo.ToList()
    End Function

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












End Class


