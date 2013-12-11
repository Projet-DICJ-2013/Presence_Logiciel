Imports System.Windows.Media.Animation

Public Class frmComposante

    Private _MesCompos As GestionComposante
    Private _NoModele As String
    Private _Modele As tblModele
    Private app As FonctionsGlobales
    Private _MsgErr

    Public Sub New(ByVal NoModele As String, ByVal Modele As tblModele)

        InitializeComponent()
        _NoModele = NoModele
        _Modele = Modele
        app = New FonctionsGlobales

    End Sub

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded
        BindControl()
    End Sub

    Private Sub BindControl()
        _MesCompos = New GestionComposante(_NoModele)

        lstComposante.DataContext = _MesCompos.CollectComposante
        lstCompoModele.DataContext = CType(_MesCompos.ModeleComposante.CurrentItem, tblModele).tblCompoModele
    End Sub

    Private Sub AddCompo_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCompo.Click

        If app.verifier_null(txtCompo.Text) Then
            _MsgErr = _MesCompos.AddComposante(txtCompo.Text)
            app.changer_statut(_MsgErr)
        Else
            app.changer_statut("Veuillez Entrer une valeur de type composante")
        End If

        BindControl()

    End Sub

    Private Sub SupCompo_Click(sender As Object, e As RoutedEventArgs) Handles btnSupCompo.Click

        _MsgErr = _MesCompos.DeleteComposante(CType(lstComposante.SelectedValue, tblCompoModele))

        app.changer_statut(_MsgErr)

        BindControl()
    End Sub

    Private Sub btnSupModele_Click(sender As Object, e As RoutedEventArgs) Handles btnSupModele.Click

        app.changer_statut(_MsgErr)

    End Sub

    Private Sub AjoutModele_Click(sender As Object, e As RoutedEventArgs) Handles btnAjoutModele.Click

        _MsgErr = _MesCompos.AddCompoToModele(CType(lstComposante.SelectedItem, tblCompoModele), _Modele)

        app.changer_statut(_MsgErr)

        BindControl()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

    End Sub
End Class


Public Class GestionComposante

    Private _Compos
    Private _CompoMod
    Private BD As New PresenceEntities

    Public Property CollectComposante As ListCollectionView
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

        Dim MesCompos = From el In BD.GetLstCompo(_NoModele)

        CollectComposante = New ListCollectionView(MesCompos.ToList)

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

    Public Function AddCompoToModele(ByVal Content As tblCompoModele, ByVal Modele As tblModele) As String

        Try

            Modele.tblCompoModele.Add(Content)
            BD.AddTotblModele(Modele)
            BD.SaveChanges()
        Catch ex As Exception
            Return "Un problème est survenu lors de l'ajout de la composante!"
        End Try
        Return "Composante ajoutée sans problème!"

    End Function
End Class
