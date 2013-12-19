'Auteur: Patrick Pearson
'Objectif: Cette interface permet de faire la gestion des modèles dans la base de données et assure la naviguabilité dans les modèles déjà existants

Imports System.Data.Objects
Imports System.Windows.Media.Animation
Imports System.Text.RegularExpressions

Public Class frmModele

    Private Modele As GestionModele
    Private _Statut As Label
    Private _MsgErreur

    'Cette région  contient les procédures permettant d'afficher la fenêtre à son état innitial
    'Le constructeur reçoit en paramètre la barre de statut pour y afficher les messages d'erreur

#Region "Initialisation"

    Public Sub New(Statut As Label)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _Statut = Statut
    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        SynchroControl(False)

    End Sub

    Private Sub TxtRech_PreviewMouseDown(sender As Object, e As MouseButtonEventArgs) Handles TxtRech.PreviewMouseDown
        If TxtRech.Text = "Saisir un critère de recherche" Then
            TxtRech.Text = ""
        End If
    End Sub

#End Region

    'Cette région contient les procédures de naviguation entres les modèles
    'Les controles sont liés à l'ensemble des données où à un critère de recherche saisie par l'utilisateur

#Region "Collection et naviguabilité"

    Private Sub SynchroControl(ByVal _IsCrit As Boolean, Optional ByVal CurrentPos As Integer = 0)

        'L'objet gestion modèle prend un paramètre facultatif
        'Si rien n'est précisé la collection contient tous les modèles
        'Sinon la collection contient les modèles corespondants à un critère parmi le # du modèle, type de machine ou la marque
        If _IsCrit Then
            Modele = New GestionModele(TxtRech.Text)
        Else
            Modele = New GestionModele
        End If

        'Si une position est précisé l'item courant se déplace à la valeur de celui-ci, sinon il est égal à 0
        Modele.Collection.MoveCurrentToPosition(CurrentPos)

        'Lie les controles à la source de données
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

    'Permet de mettre à jour la liste des composantes rattachées à un modèle
    Private Sub UpdateComposante()

        If Modele.Collection.CurrentItem IsNot Nothing Then
            ViewComposante.ItemsSource = CType(Modele.Collection.CurrentItem, tblModele).tblCompoModele
        Else
            ViewComposante.ItemsSource = Nothing
        End If

    End Sub

    'Récupère le critère de recherche et appel la procédure mettant à jour la collection de  modèle
    Private Sub btnRech_Click(sender As Object, e As RoutedEventArgs) Handles btnRech.Click

        If TxtRech.Text IsNot Nothing Then
            SynchroControl(True)
        Else
            _MsgErreur = "Vous devez remplir le champ de recherche"
            AffMsgErr()
        End If
    End Sub

    'Réinitialise la collection de modèle
    Private Sub btnReset_Click(sender As Object, e As RoutedEventArgs) Handles btnReset.Click
        SynchroControl(False)
    End Sub

#End Region

    'Cette région contient les procédures permettant de faire le transfert de données vers les fonctions d'ajouter, mettre à jour 
    'et suppression de modèle dans la base de données

#Region "Gestion des données"

    'Permet d'ouvrir la fenêtre d'ajout et de suppression d'une composante à un modèle
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

        Dim tblModele As tblModele = Nothing

        If VerifSaisie() Then
            tblModele = New tblModele With {.NoModele = TxtModele.Text, _
                                                    .Marque = TxtMarque.Text, _
                                                    .NbAnneeGarantie = TxtGaranti.Text, _
                                                    .TypeMachine = TxtMarque.Text, _
                                                    .PrixModele = TxtPrix.Text.Replace(".", ",")}
        End If

        If tblModele IsNot Nothing Then
            _MsgErreur = Modele.AddModele(tblModele)
            AffMsgErr()
        End If

    End Sub

    Private Sub btnSup_Click(sender As Object, e As RoutedEventArgs) Handles btnSup.Click

        If Modele.Collection.CurrentItem IsNot Nothing Then
            _MsgErreur = Modele.SupModele(CType(Modele.Collection.CurrentItem, tblModele))
            AffMsgErr()
            SynchroControl(False)
        End If

    End Sub

#End Region

    'Cette région contient les procédures permettant de verifier le données saisies et d'afficher les messages d'erreur dans la barre de statut

#Region "Validation et gestion des erreurs"

    Private Sub txtModele_LostFocus(sender As Object, e As RoutedEventArgs) Handles TxtModele.LostFocus
        If TxtModele.Text = "" Then
            TxtModele.BorderBrush = Brushes.Red
            TxtModele.Text = "0"
        Else
            TxtModele.BorderBrush = Brushes.Green
        End If
    End Sub

    Protected Sub AffMsgErr()

        Dim anim As Storyboard = FindResource("AnimLabel")

        _Statut.Content = _MsgErreur
        anim.Begin(_Statut)
    End Sub

    Protected Function VerifSaisie() As Boolean
        Dim regex1 As New Regex("^[1-9]\d*(\.\d+)?$")

        If TxtGaranti.Text = Nothing Or TxtMarque.Text = Nothing Or TxtModele.Text = Nothing Or TxtType.Text = Nothing Then
            _MsgErreur = ("Veuillez remplir tous les champs pour enregistrer un nouveau modèle")
            AffMsgErr()
            Return False
        End If

        If (regex1.IsMatch(TxtPrix.Text) = False) Then
            _MsgErreur = ("Le prix d'un Modele doit être dans le format suivant 000,000")
            AffMsgErr()
            Return False
        End If

        Return True
    End Function

#End Region

End Class

Public Class GestionModele

    Private i
    Private BD As New PresenceEntities

    'Permet de définir ou d'accéder à la collection de modèle
    Public Property Collection As ListCollectionView
        Get
            Return i
        End Get
        Set(value As ListCollectionView)
            i = value
        End Set
    End Property

    'Génère la liste de modèle en fonction d'un critère de recherche facultatif
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

    'Obtient la liste des composantes
    Public Function GetComposante(ByVal _Modele As String) As ICollection

        Dim MesCompo = From Mode In BD.tblModele
        Where Mode.NoModele = _Modele
        Select Mode.tblCompoModele

        Return MesCompo.ToList()
    End Function


    Public Function AddModele(ByVal MonModele As tblModele) As String

        'Permet de verifier si le modèle existe déjà
        Dim ModeleChange As IQueryable(Of tblModele) = (From Mode In BD.tblModele
                                 Where Mode.NoModele = MonModele.NoModele
                                 Select Mode)
        Dim Modele As tblModele

        'Si le modèle existe le modifié, sinon en ajoiuter un nouveau
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

    'Supprime le modèle de la base de données
    Public Function SupModele(ByVal MonModele As tblModele) As String

        BD.DeleteObject(MonModele)

        Try
            BD.SaveChanges()
        Catch ex As Exception
            Return "La supression du modèle à échoué, veuillez verifier qu'aucun exemplaires ou composantes y sont rattaché"
        End Try

        Return "La supression du modèle a réussie!"

    End Function

End Class


