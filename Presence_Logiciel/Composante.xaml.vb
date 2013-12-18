'Auteur: Patrick Pearson
'Objectif: Cette interface permet de faire la gestion des composantes dans la base de données et permet d'associer des composantes au modèle sélectionné précedement
Imports System.Windows.Media.Animation

Public Class frmComposante

    Private _MesCompos As GestionComposante
    Private _NoModele As String
    Private _MsgErr As String
    Private _Statut As Label


#Region "Initialisation"

    'Constructeur recevant en paramètre la barre de statut et le modèle auquelle on ajoute les composantes
    Public Sub New(ByVal NoModele As String, Statut As Label)
        InitializeComponent()
        _NoModele = NoModele
        _Statut = Statut
    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        BindControl()
    End Sub

    'Connexion des controles aux valeurs de la BD
    Private Sub BindControl()
        _MesCompos = New GestionComposante(_NoModele)

        lstComposante.DataContext = _MesCompos.AllComposante
        lstCompoModele.DataContext = CType(_MesCompos.ModeleComposante.CurrentItem, tblModele).tblCompoModele
    End Sub

#End Region

#Region "Modifier les composantes"


    Private Sub AddCompo_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCompo.Click

        If txtCompo.Text <> " " Then
            _MsgErr = _MesCompos.AddComposante(txtCompo.Text)
            AffMsgErr()
        Else
            _MsgErr = "Veuillez Entrer une valeur de type composante"
            AffMsgErr()
        End If

        BindControl()

    End Sub


    Private Sub SupCompo_Click(sender As Object, e As RoutedEventArgs) Handles btnSupCompo.Click

        _MsgErr = _MesCompos.DeleteComposante(CType(lstComposante.SelectedValue, tblCompoModele))
        AffMsgErr()
        BindControl()
    End Sub


    Private Sub btnSupModele_Click(sender As Object, e As RoutedEventArgs) Handles btnSupModele.Click

        _MsgErr = _MesCompos.DeleteCompoToModele(CType(lstCompoModele.SelectedItem, tblCompoModele), _NoModele)
        AffMsgErr()
        BindControl()

    End Sub


    Private Sub btnAjoutModele_Click(sender As Object, e As RoutedEventArgs) Handles btnAjoutModele.Click

        _MsgErr = _MesCompos.AddCompoToModele(CType(lstComposante.SelectedItem, tblCompoModele), _NoModele)
        AffMsgErr()
        BindControl()
    End Sub

#End Region

#Region "Erreur"
    Protected Sub AffMsgErr()

        Dim anim As Storyboard = FindResource("AnimLabel")

        _Statut.Content = _MsgErr
        anim.Begin(_Statut)

    End Sub
#End Region

End Class


Public Class GestionComposante

    Private _Compos
    Private _CompoMod
    Private BD As New PresenceEntities

    'Permet d'obtenir ou de définir la liste de toutes les composantes dans la BD
    Public Property AllComposante As ListCollectionView
        Get
            Return _Compos
        End Get
        Set(value As ListCollectionView)
            _Compos = value
        End Set
    End Property

    'Permet d'obtenir ou de définir la liste de toutes les composantes relié au modèle sélectionné dans la BD
    Public Property ModeleComposante As ListCollectionView
        Get
            Return _CompoMod
        End Get
        Set(value As ListCollectionView)
            _CompoMod = value
        End Set
    End Property

    'Recherche les composantes dans la BD et définit les deux propriétés 
    Public Sub New(ByVal _NoModele As String)
        Try
            Dim MesCompos = (From el In BD.GetLstCompo(_NoModele))

            AllComposante = New ListCollectionView(MesCompos.ToList)
        Catch ex As Exception

        End Try
        Dim CompoModele = From Compo In BD.tblModele
                          Where Compo.NoModele = _NoModele
                          Select Compo

        ModeleComposante = New ListCollectionView(CompoModele.ToList)

    End Sub

    'Ajouter un nouveau composante dans la base de données
    Public Function AddComposante(ByVal TypeCom As String) As String

        Dim Composante As New tblCompoModele With {.TypeCompo = TypeCom}

        Try
            BD.AddTotblCompoModele(Composante)
            BD.SaveChanges()
        Catch ex As Exception
            Return "L'ajout de la composante à échoué"
        End Try

        Return "L'ajout de la composante à réussi"
    End Function

    'Supprimer une composante sélectionné dans la base de données
    Public Function DeleteComposante(ByVal TypeCom As tblCompoModele) As String

        Try
            BD.tblCompoModele.DeleteObject(TypeCom)
            BD.SaveChanges()
        Catch ex As Exception
            Return "Il est impossible de supprimé une composante lié à un matériel!"
        End Try

        Return "Suppression réussie"

    End Function

    'Associer du modèle sélectionné la composante sélectionnée
    Public Function AddCompoToModele(ByVal Content As tblCompoModele, ByVal _NoModele As String) As String

        Dim Modele As tblModele = GetModeleByID(_NoModele)

        Try
            Modele.tblCompoModele.Add(Content)
            BD.SaveChanges()
        Catch ex As Exception
            Return "Un problème est survenu lors de l'ajout de la composante au modèle!"
        End Try
        Return "La composante à été ajouté du modèle sans problème!"

    End Function

    'Dissocier du modèle sélectionné la composante sélectionnée
    Public Function DeleteCompoToModele(ByVal Content As tblCompoModele, ByVal _NoModele As String) As String

        Dim Modele As tblModele = GetModeleByID(_NoModele)

        Try
            Modele.tblCompoModele.Remove(Content)
            BD.SaveChanges()
        Catch ex As Exception
            Return "Un problème est survenu lors du retrait de la composante du modèle!"
        End Try
        Return "La composante à été retiré du modèle sans problème!"

    End Function

    'Retourne l'entité du modèle en fonction de l'id envoyé en paramètre
    Protected Function GetModeleByID(ByVal _NoModele As String) As tblModele
        Dim Modele = (From Mode In BD.tblModele
                     Where Mode.NoModele = _NoModele
                     Select Mode).First

        Return Modele
    End Function
End Class
