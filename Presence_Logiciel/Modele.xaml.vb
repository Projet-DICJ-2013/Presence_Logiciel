Imports System.Data.Objects
'PATRICK EST BIN TROP COOL POUR METTRE DES COMMENTAIRES IL SE PENSE HOT LE GARS
Public Class frmModele

    Private Modele As GestionModele

    Private Sub FormLoad(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded

        Modele = New GestionModele

        SynchroControl()

    End Sub

    Private Sub SynchroControl()

        Try
            TxtMarque.DataContext = Modele.Collection
            TxtModele.DataContext = Modele.Collection
            TxtType.DataContext = Modele.Collection
            TxtGaranti.DataContext = Modele.Collection
            TxtPrix.DataContext = Modele.Collection
            ViewComposante.ItemsSource = CType(Modele.Collection.CurrentItem, tblModele).tblCompoModele
            TxtRech.Text = "Saisir un critère de recherche"
        Catch ex As Exception

        End Try


    End Sub

    Private Sub btnFirst_Click(sender As Object, e As RoutedEventArgs) Handles btnFirst.Click
        Modele.Collection.MoveCurrentToFirst()

    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As RoutedEventArgs) Handles btnPrevious.Click
        Modele.Collection.MoveCurrentToPrevious()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As RoutedEventArgs) Handles btnNext.Click
        Modele.Collection.MoveCurrentToNext()
    End Sub

    Private Sub btnLast_Click(sender As Object, e As RoutedEventArgs) Handles btnLast.Click
        Modele.Collection.MoveCurrentToLast()
    End Sub

    Private Sub TxtModele_TextChanged(sender As Object, e As TextChangedEventArgs) Handles TxtModele.TextChanged

        ViewComposante.ItemsSource = Modele.GetComposante(TxtModele.Text)
    End Sub

    Private Sub btnAddCompo_Click(sender As Object, e As RoutedEventArgs) Handles btnAddCompo.Click

        Dim FnComposante As New frmComposante(CType(Modele.Collection.CurrentItem, tblModele).NoModele, _
                                              CType(Modele.Collection.CurrentItem, tblModele))
        FnComposante.ShowDialog()

        SynchroControl()
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
                Modele.AddModele(tblModele)
            End If
    End Sub


    Private Sub OnQuit(sender As Object, e As RoutedEventArgs) Handles MyBase.Closed
        Me.Close()
        Me.Finalize()
    End Sub

    Private Sub txtModele_LostFocus(sender As Object, e As RoutedEventArgs) Handles TxtModele.LostFocus
        If txtModele.Text = "" Then
            txtModele.BorderBrush = Brushes.Red
            txtModele.Text = "0"
        Else
            txtModele.BorderBrush = Brushes.Green
        End If
    End Sub

    Private Sub TxtRech_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles TxtRech.MouseDoubleClick
        TxtRech.Text = ""
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

    Public Function AddModele(ByVal MonModele As tblModele) As Boolean

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
                Return False
            End Try

            Return True

    End Function



    'PARTIE DE LA VALIDATION DES ENTRÉES

End Class