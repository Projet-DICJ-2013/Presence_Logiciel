Imports System.Windows.Media.Animation

Public Class frmComposante


    Private _MesCompos As GestionComposante
    Private _NoModele As String
    Private _MsgErr As String
    Private _Statut As Label

    Public Sub New(ByVal NoModele As String, Statut As Label)

        InitializeComponent()
        _NoModele = NoModele
        _Statut = Statut
    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        BindControl()
    End Sub

    Private Sub BindControl()
        _MesCompos = New GestionComposante(_NoModele)

        lstComposante.DataContext = _MesCompos.AllComposante
        lstCompoModele.DataContext = CType(_MesCompos.ModeleComposante.CurrentItem, tblModele).tblCompoModele
    End Sub

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

    Private Sub AjoutModele_Click(sender As Object, e As RoutedEventArgs) Handles btnAjoutModele.Click

        _MsgErr = _MesCompos.AddCompoToModele(CType(lstComposante.SelectedItem, tblCompoModele), _NoModele)
        AffMsgErr()
        BindControl()
    End Sub

    Protected Sub AffMsgErr()

        Dim anim As Storyboard = FindResource("AnimLabel")

        _Statut.Content = _MsgErr
        anim.Begin(_Statut)
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class


Public Class GestionComposante

    Private _Compos
    Private _CompoMod
    Private BD As New PresenceEntities

    Public Property AllComposante As ListCollectionView
        Get
            Return _Compos
        End Get
        Set(value As ListCollectionView)
            _Compos = value
        End Set
    End Property

    Public Property ModeleComposante As ListCollectionView
        Get
            Return _CompoMod
        End Get
        Set(value As ListCollectionView)
            _CompoMod = value
        End Set
    End Property

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

    Public Function DeleteComposante(ByVal TypeCom As tblCompoModele) As String

        Try
            BD.tblCompoModele.DeleteObject(TypeCom)
            BD.SaveChanges()
        Catch ex As Exception
            Return "Il est impossible de supprimé une composante lié à un matériel!"
        End Try

        Return "Suppression réussie"

    End Function

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

    Protected Function GetModeleByID(ByVal _NoModele As String) As tblModele
        Dim Modele = (From Mode In BD.tblModele
                     Where Mode.NoModele = _NoModele
                     Select Mode).First

        Return Modele
    End Function
End Class
