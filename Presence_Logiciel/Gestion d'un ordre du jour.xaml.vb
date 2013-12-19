Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Objects


Public Class frmGestOrdJour

    'Declaration des variables privées
    Private startPoint As New Point
    Private OrdreDuJour As GestionOdj
    Private _intReu As int_CedReunion

    Private Sub Grid_Loaded(sender As Object, e As RoutedEventArgs)
        'Initialisation de la fenêtre
        OrdreDuJour = New GestionOdj
        lstOrdreJour.ItemsSource = OrdreDuJour.Collection
    End Sub
    Private Sub btnNouveauPoint_Click(sender As Object, e As RoutedEventArgs) Handles btnNouveauPoint.Click
        'Fonction permettant l'ajout d'un élément en dernière position dans le treeview
        If txtTitrePoint.Text <> "" Then
            NettoyerArbre()
            lstOdj.Items.Add(New TreeViewItem() With {.Header = txtTitrePoint.Text})
            NumeroterArbre()
            btnPlanifReun.IsEnabled = False
            btnEnregistrer.IsEnabled = True
        Else
            MessageBox.Show("Le nouveau point n'a pas de titre!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub
    Private Sub btnAttacher_Click(sender As Object, e As RoutedEventArgs) Handles btnAttacher.Click
        'Fonction permettant d'attacher un élément à l'élément selectionné dans le treeview
        Dim item As TreeViewItem = TryCast(lstOdj.SelectedItem, TreeViewItem)
        If item IsNot Nothing And txtTitrePoint.Text <> "" Then
            NettoyerArbre()
            item.Items.Add(New TreeViewItem() With {.Header = txtTitrePoint.Text})
            item.ExpandSubtree()
            NumeroterArbre()
            btnPlanifReun.IsEnabled = False
            btnEnregistrer.IsEnabled = True
        Else
            MessageBox.Show("Aucun point est séléctionné ou le nouveau point n'a pas de titre!", "Attention!", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub
    Private Sub btnSupprimerPoint_Click(sender As Object, e As RoutedEventArgs) Handles btnSupprimerPoint.Click
        'Fonction permettant la suppression de l'élément sélectionné dans le treeview
        Dim ElementTemporaire As TreeViewItem = DirectCast(lstOdj.SelectedItem, TreeViewItem)
        If ElementTemporaire IsNot Nothing Then
            If TypeOf ElementTemporaire.Parent Is TreeViewItem Then
                TryCast(ElementTemporaire.Parent, TreeViewItem).Items.Remove(ElementTemporaire)
            Else
                Me.lstOdj.Items.Remove(ElementTemporaire)
            End If

            btnPlanifReun.IsEnabled = False
            btnEnregistrer.IsEnabled = True
        End If
    End Sub
    Private Sub lstOdj_KeyDown(sender As Object, e As KeyEventArgs) Handles lstOdj.KeyDown
        'Cette fonction défini l'action produite dans le treeview suite à l'utilisation du clavier
        If lstOdj.SelectedItem IsNot Nothing Then
            Select Case e.Key
                Case Key.Delete
                    'Supprime l'element selectionné
                    If TypeOf lstOdj.SelectedItem.Parent Is TreeViewItem Then
                        TryCast(lstOdj.SelectedItem.Parent, TreeViewItem).Items.Remove(lstOdj.SelectedItem)
                    Else
                        Me.lstOdj.Items.Remove(lstOdj.SelectedItem)
                    End If
                    btnEnregistrer.IsEnabled = True
                Case Key.Down
                    'Navigue vers le bas
                    Dim i = 0
                    i = lstOdj.Items.IndexOf(lstOdj.SelectedItem)
                    lstOdj.SelectedItem.IsSelected = False
                    If lstOdj.Items.Count > i + 1 Then
                        lstOdj.Items(i + 1).IsSelected = True
                    End If
                Case Key.Up
                    'Navigue vers le haut
                    Dim i = 0
                    i = lstOdj.Items.IndexOf(lstOdj.SelectedItem)
                    lstOdj.SelectedItem.IsSelected = False
                    If -1 < i - 1 Then
                        lstOdj.Items(i - 1).IsSelected = True
                    Else
                        i = lstOdj.Items.Count
                        lstOdj.Items(i - 1).IsSelected = True
                    End If
            End Select
        End If
    End Sub
    Private Sub btnCreerOdj_Click(sender As Object, e As RoutedEventArgs) Handles btnCreerOdj.Click
        'Cette fonction créer un ordre du jour dans la BD avec 5 points de base et l'affiche dans le treeview
        Dim tblOrdreDuJour As tblOrdreDuJour
        Dim tblListePoint As tblListePoint

        If txtTitreOdj.Text <> "" Then

            tblOrdreDuJour = New tblOrdreDuJour With {.TitreOrdreJour = txtTitreOdj.Text, _
                                                    .Notes = Nothing}
            If (tblOrdreDuJour IsNot Nothing) Then
                OrdreDuJour.AddOdj(tblOrdreDuJour)

                lstOrdreJour.SelectedItem = OrdreDuJour.Collection.CurrentItem

                tblListePoint = New tblListePoint With {.NoOrdreDuJour = CType(OrdreDuJour.Collection.CurrentItem, tblOrdreDuJour).NoOrdreDuJour}

                If (tblListePoint IsNot Nothing) Then
                    OrdreDuJour.AddListe(tblListePoint)
                End If
            End If

            OrdreDuJour.AddPoint(1, "Acceptation et ouverture de l'ordre du jour", OrdreDuJour.GetNoListePoint(OrdreDuJour.Collection.CurrentItem), "1.")
            OrdreDuJour.AddPoint(2, "Acceptation des procès-verbaux", OrdreDuJour.GetNoListePoint(OrdreDuJour.Collection.CurrentItem), "2.")
            OrdreDuJour.AddPoint(3, "Informations", OrdreDuJour.GetNoListePoint(OrdreDuJour.Collection.CurrentItem), "3.")
            OrdreDuJour.AddPoint(4, "Divers", OrdreDuJour.GetNoListePoint(OrdreDuJour.Collection.CurrentItem), "4.")
            OrdreDuJour.AddPoint(5, "Fermeture de l'ordre du jour", OrdreDuJour.GetNoListePoint(OrdreDuJour.Collection.CurrentItem), "5.")

            txtTitreOdj.Text = ""
            lstOrdreJour.ItemsSource = OrdreDuJour.Collection
        Else
            MsgBox("Veuillez remplir tous les champs pour enregistrer un nouvelle ordre du jour!")
            Return
        End If
    End Sub
    Private Sub lstOrdreJour_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles lstOrdreJour.SelectionChanged
        'Cet évènement permet de recharger le treeview en fonction de l'ordre du jour sélèctioné
        ReloadList()
        If lstOrdreJour.SelectedIndex > -1 Then
            btnPlanifReun.IsEnabled = True
            btnEnregistrer.IsEnabled = False
        Else
            btnPlanifReun.IsEnabled = False
            btnEnregistrer.IsEnabled = True
        End If
    End Sub
    Private Function AjouterEnfant(ByVal MonPoint As tblPoints, ByVal MonObject As TreeViewItem) As Boolean
        'Cette fonction permet d'ajouter chacun des points d'une table de points ainsi que chacun de ces enfants au TreeView
        For Each Element As tblPoints In MonPoint.tblListePoint.tblPoints1
            Dim cr = New TreeViewItem() With {.Header = Element.TitrePoint}
            MonObject.Items.Add(cr)
            If Element.ListeEnfants.HasValue() Then
                AjouterEnfant(Element, cr)
            End If
        Next
        Return True
    End Function
    Private Sub btnEnregistrer_Click(sender As Object, e As RoutedEventArgs) Handles btnEnregistrer.Click
        'Cet évènement permet l'enregistrement d'un ordre du jour, des listes de points et des points dans la base de donnée en fonction du TreeView
        OrdreDuJour.DeletePointsOrdreDuJour(lstOrdreJour.SelectedItem)
        NettoyerArbre()
        OrdreDuJour.AjouterOrdreDuJour(lstOdj, lstOrdreJour.SelectedItem)
        ReloadList()

        btnPlanifReun.IsEnabled = True
        btnEnregistrer.IsEnabled = False
    End Sub
    Private Sub ReloadList()
        'Cette fonction permet de vider le treeview et de la remplir par la suite a partir de la BD
        OrdreDuJour.Collection.MoveCurrentTo(lstOrdreJour.SelectedItem)
        lstOdj.Items.Clear()
        Dim ListePoint As List(Of tblPoints)

        ListePoint = New List(Of tblPoints)
        ListePoint = OrdreDuJour.RetourPointsOdj(lstOrdreJour.SelectedItem)
        For Each Point In ListePoint
            Dim cr = New TreeViewItem() With {.Header = Point.TitrePoint}
            lstOdj.Items.Add(cr)
            If Point.ListeEnfants.HasValue() Then
                AjouterEnfant(Point, cr)
            End If

        Next
        NumeroterArbre()
    End Sub
    Private Sub btnPlanifReun_Click(sender As Object, e As RoutedEventArgs) Handles btnPlanifReun.Click
        'Cet évènement permet d'ouvrir la fenêtre pour planifier une réunion
        _intReu = New int_CedReunion(CType(OrdreDuJour.Collection.CurrentItem, tblOrdreDuJour).NoOrdreDuJour, CType(OrdreDuJour.Collection.CurrentItem, tblOrdreDuJour).TitreOrdreJour)
        _intReu.ShowDialog()
    End Sub
    Private Sub btnFermer_Click(sender As Object, e As RoutedEventArgs) Handles btnFermer.Click
        'Cet évènement permet de quitter l'interface de gestion d'ordre du jour
        Dim DialogResult = MessageBox.Show("Êtes-vous sur de vouloir quitter?", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning)
        If DialogResult = MessageBoxResult.Yes Then
            Me.Close()
        End If
    End Sub

    'Les 4 fonctions suivantes permettent de numeroter ou d'enlever les numeros de chacun des éléments dans le Treeview
    Private Sub NumeroterArbre()
        Dim i As Int16
        Dim MonId As String
        i = 1
        For Each Element As TreeViewItem In lstOdj.Items
            MonId = Convert.ToString(i) + "."
            Element.Header = MonId + " " + Element.Header
            NumeroterEnfant(Element, MonId)
            i = i + 1
        Next
    End Sub
    Private Sub NumeroterEnfant(ByVal MonItem As TreeViewItem, ByVal IdParent As String)
        If MonItem.HasItems Then
            Dim i As Int16
            Dim MonId As String
            i = 1
            For Each Element As TreeViewItem In MonItem.Items
                MonId = IdParent + Convert.ToString(i) + "."
                Element.Header = MonId + " " + Element.Header
                NumeroterEnfant(Element, MonId)
                i = i + 1
            Next
        End If
    End Sub
    Private Sub NettoyerArbre()
        For Each Element As TreeViewItem In lstOdj.Items
            Element.Header = Element.Header.Substring(InStr(Element.Header, " "))
            NettoyerEnfant(Element)
        Next
    End Sub
    Private Sub NettoyerEnfant(ByVal MonItem As TreeViewItem)
        If MonItem.HasItems Then
            For Each Element As TreeViewItem In MonItem.Items
                Element.Header = Element.Header.Substring(InStr(Element.Header, " "))
                NettoyerEnfant(Element)
            Next
        End If
    End Sub

    'Les 4 fonctions suivantes permettent le déplacement des éléments dans le treeview par Drag And Drop
    Private Sub lstOdj_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lstOdj.PreviewMouseLeftButtonDown
        startPoint = e.GetPosition(Nothing)
    End Sub
    Private Sub lstOdj_MouseMove(sender As Object, e As MouseEventArgs) Handles lstOdj.MouseMove
        Dim mousePos As Point = e.GetPosition(Nothing)
        Dim diff As Vector = startPoint - mousePos

        If e.LeftButton = MouseButtonState.Pressed AndAlso (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance OrElse Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance) Then
            Dim treeView As TreeView = TryCast(sender, TreeView)
            Dim treeViewItem As TreeViewItem = FindAnchestor(Of TreeViewItem)(DirectCast(e.OriginalSource, DependencyObject))
            Dim contact As TreeViewItem

            If TypeOf e.Source Is TreeViewItem Then
                contact = e.Source

                Dim dragData As New DataObject("myFormat", contact)
                DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move)
            End If
        End If
    End Sub
    Private Shared Function FindAnchestor(Of T As DependencyObject)(current As DependencyObject) As T
        While current IsNot Nothing
            If TypeOf current Is T Then
                Return DirectCast(current, T)
            End If
            current = VisualTreeHelper.GetParent(current)
        End While
        Return Nothing
    End Function
    Private Sub lstOdj_Drop(sender As Object, e As DragEventArgs) Handles lstOdj.Drop
        If e.Data.GetDataPresent("myFormat") Then
            Dim treeViewItem As TreeViewItem = FindAnchestor(Of TreeViewItem)(DirectCast(e.OriginalSource, DependencyObject))
            Dim contact As TreeViewItem = TryCast(e.Data.GetData("myFormat"), TreeViewItem)
            Dim treeView As TreeView = TryCast(sender, TreeView)
            Dim MonParentArbre As TreeView
            Dim MonParentNoeud As TreeViewItem
            Dim i As Int16
            NettoyerArbre()
            If TypeOf contact.Parent Is TreeViewItem Then
                MonParentNoeud = contact.Parent
                i = MonParentNoeud.Items.IndexOf(treeViewItem)
                MonParentNoeud.Items.Remove(contact)
                If i > -1 And i = treeView.Items.Count Then
                    MonParentNoeud.Items.Remove(contact)
                    MonParentNoeud.Items.Add(contact)
                ElseIf i > -1 Then
                    MonParentNoeud.Items.Remove(contact)
                    MonParentNoeud.Items.Insert(i, contact)
                End If
            Else
                MonParentArbre = contact.Parent
                i = MonParentArbre.Items.IndexOf(treeViewItem)

                If i = treeView.Items.Count Then
                    MonParentArbre.Items.Remove(contact)
                    MonParentArbre.Items.Add(contact)
                ElseIf i > -1 Then
                    MonParentArbre.Items.Remove(contact)
                    MonParentArbre.Items.Insert(i, contact)
                End If
            End If
            NumeroterArbre()
            btnPlanifReun.IsEnabled = False
            btnEnregistrer.IsEnabled = True
        End If
    End Sub

End Class

Public Class GestionOdj
    'Déclaration des variables privées
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
        'Initialise la classe en remplissant une ListCollectionView avec la liste des ordres du jour dans la BD
        Dim MonOdj = From Ex In BD.tblOrdreDuJour
                        Select Ex

        Collection = New ListCollectionView(MonOdj.ToList())
    End Sub

    Public Function SupprimerLiaison(ByVal _Point As tblPoints) As Boolean
        'Cette fonction supprime la liaison dans la table PointListePoint entre chacun des points passés en parametre et sa liste parent
        While _Point.tblListePoint1.Count() > 0
            _Point.tblListePoint1.Remove(_Point.tblListePoint1.FirstOrDefault())
        End While
        Return True
    End Function

    Public Function GetNoListePoint(ByVal _NoOdj As tblOrdreDuJour) As tblListePoint
        'Cette fonction retourne la liste rattaché à un ordre du jour 
        Dim Numero = (From Liste In BD.tblListePoint
                                                     Where Liste.NoOrdreDuJour = _NoOdj.NoOrdreDuJour
                                                      Select Liste).ToList
        Return CType(Numero.First, tblListePoint)
    End Function

    Public Function AddOdj(ByVal MonOrdreDuJour As tblOrdreDuJour) As Boolean
        'Cette fonction ajoute l'ordre du jour passé en parametre dans la BD
        Dim OrdreDuJourChange As IQueryable(Of tblOrdreDuJour) = (From Odj In BD.tblOrdreDuJour
                                 Where Odj.NoOrdreDuJour = MonOrdreDuJour.NoOrdreDuJour
                                 Select Odj)
        Dim OrdreDuJour As tblOrdreDuJour

        Try
            If OrdreDuJourChange.Count Then
                OrdreDuJour = OrdreDuJourChange.First
                OrdreDuJour = MonOrdreDuJour
            Else
                BD.AddTotblOrdreDuJour(MonOrdreDuJour)
            End If
            BD.SaveChanges()
        Catch ex As Exception
            Return False
        End Try

        Dim MonOdj = From Ex In BD.tblOrdreDuJour
                       Select Ex

        Collection = New ListCollectionView(MonOdj.ToList())
        Collection.MoveCurrentTo(MonOrdreDuJour)
        Return True

    End Function

    Public Function AddListe(ByVal MaListe As tblListePoint) As Boolean
        'Cette fonction ajoute la liste passé en paramètre dans la BD
        Dim ListeChange As IQueryable(Of tblListePoint) = (From Liste In BD.tblListePoint
                                 Where Liste.NoListePoint = MaListe.NoListePoint
                                 Select Liste)
        Dim ListePoint As tblListePoint

        Try
            If ListeChange.Count Then
                ListePoint = ListeChange.First
                ListePoint = MaListe
            Else
                BD.AddTotblListePoint(MaListe)
            End If
            BD.SaveChanges()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    Public Function AddPoint(ByVal NumPoint As Int32, TitrePoint As String, ByVal ListePoint As tblListePoint, ByVal _nomPoint As String) As Boolean
        'Cette fonction ajoute un point dans la BD en fonction des informations passées en paramètre
        Dim MonPoint As tblPoints
        MonPoint = New tblPoints With {.NumeroPoint = NumPoint, .TitrePoint = TitrePoint, .ChiffrePoint = _nomPoint}
        MonPoint.tblListePoint1.Add(ListePoint)

        If (MonPoint IsNot Nothing) Then
            Try
                BD.AddTotblPoints(MonPoint)
                BD.SaveChanges()
            Catch ex As Exception
                Return False
            End Try
        End If
        Return True
    End Function

    Public Function RetourPointsOdj(ByVal MonOrdreDuJour As tblOrdreDuJour) As List(Of tblPoints)
        'Retourne tous les points dans la BD concernant l'ordre du jour passé en paramètre 
        Dim MesPoints = From MaListe In BD.tblListePoint
                    Where MaListe.NoOrdreDuJour = MonOrdreDuJour.NoOrdreDuJour
                    Join Point In BD.tblPoints
                    On Point.tblListePoint1.FirstOrDefault Equals MaListe
                    Select Point
        Return MesPoints.ToList()
    End Function


    Public Function DeletePointsOrdreDuJour(ByVal OrdreDuJour As tblOrdreDuJour)
        'Cette fonction cherche tous les points rattachées à un ordre du jour dans la BD et les supprime
        Dim ListePoint As List(Of tblPoints)
        ListePoint = New List(Of tblPoints)
        ListePoint = RetourPointsOdj(OrdreDuJour)
        For Each Point In ListePoint
            DeletePoints(Point)
        Next
        Return True
    End Function
    Public Function DeletePoints(ByVal Point As tblPoints) As Boolean
        'Cette fonction supprime les enfants du point passé en paramètre et finalment le point lui même (dans la BD)

        If Point.ListeEnfants.HasValue Then
            Dim LesEnfants As List(Of tblPoints)
            LesEnfants = BD.RetournerEnfants(Point.IDPoint).ToList()

            For Each ElementEnfants In LesEnfants
                DeletePoints(ElementEnfants)
            Next

            SupprimerLiaison(Point)
            BD.tblPoints.DeleteObject(Point)
        Else
            SupprimerLiaison(Point)
            BD.tblPoints.DeleteObject(Point)
        End If
        Try
            BD.SaveChanges()
        Catch ex As Exception
        End Try
        Return True
    End Function
    Public Function AjouterOrdreDuJour(ByVal Arbre As TreeView, ByVal OrdreDuJour As tblOrdreDuJour)

        Dim ListePoint As List(Of tblListePoint) = (From MaListe In BD.tblListePoint
                         Where MaListe.NoOrdreDuJour = OrdreDuJour.NoOrdreDuJour
                         Select MaListe).ToList

        Dim j As Int32
        j = 1
        For Each TreeViewItem As TreeViewItem In Arbre.Items
            AjouterPoints(TreeViewItem, ListePoint.First(), j, Convert.ToString(j) + ".")
            j = 1 + j
        Next
        Return True
    End Function

    Public Function AjouterPoints(ByVal MonPoint As TreeViewItem, ByVal MaListe As tblListePoint, ByVal Numero As Int32, ByVal NomPoint As String)
        'Cette fonction permet d'ajouter un point du TreeView et ces enfants dans la BD
        Dim PointTemporaire As New tblPoints With {.TitrePoint = MonPoint.Header, .NumeroPoint = Numero, .ChiffrePoint = NomPoint}
        PointTemporaire.tblListePoint1.Add(MaListe)
        BD.AddTotblPoints(PointTemporaire)

        If MonPoint.HasItems Then
            Dim NouvelleListe As New tblListePoint
            NouvelleListe.tblPoints.Add(PointTemporaire)
            BD.AddTotblListePoint(NouvelleListe)
            Dim j As Int32
            j = 1
            For Each Element As TreeViewItem In MonPoint.Items
                AjouterPoints(Element, NouvelleListe, j, NomPoint + Convert.ToString(j) + ".")
                j = j + 1
            Next
        End If
        Try
            BD.SaveChanges()
        Catch ex As Exception
        End Try
        Return True
    End Function
End Class