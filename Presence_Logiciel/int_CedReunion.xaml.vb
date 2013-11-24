
Public Class int_CedReunion
    'Declaration des variables privées
    Private _objReunion As objReunion
    Private _lesInvitee As List(Of tblMembre)
    Private _redacteur As tblMembre

    Private _noordredujour As Integer
    Private _elementidentique As Boolean

    'Initialisation de la fenetre de reunion.
    Public Sub New(noordredujour As Integer)

        InitializeComponent()
        _noordredujour = noordredujour

    End Sub
    'Permet de ceduler et d'ouvrir l'interface d'envoie de mail.
    Private Sub btnCeduler_Click(sender As Object, e As RoutedEventArgs) Handles btnCeduler.Click

        If ((cmbRedac.SelectionBoxItem IsNot Nothing) And (txtEndroit.Text IsNot Nothing) And (dtpDateRenc.SelectedDate IsNot Nothing) And (lstInvite.Items.Count > 0)) Then
            _objReunion.ajoutReunionBd(dtpDateRenc.SelectedDate, txtEndroit.Text, cmbLocal.SelectionBoxItem.ToString(), _noordredujour)
            _redacteur = cmbRedac.SelectedItem
            _objReunion.ajoutMembreParticipantBd(_lesInvitee, _redacteur)
            Me.Close()
            _objReunion.OuvrirMail()
        Else
            MessageBox.Show("Veuillez remplir tous les champs.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

    End Sub
    'Permet de faire la recherche dans les combobox de recherche.
    Private Sub btnRechercher_Click(sender As Object, e As RoutedEventArgs) Handles btnRechercher.Click

        If (cmbType.SelectedIndex = 0) Then
            lstChoix.ItemsSource = _objReunion.Chargerliste(0, cmbAnnée.SelectedIndex)
        Else
            lstChoix.ItemsSource = _objReunion.Chargerliste(cmbType.SelectedIndex)
        End If

    End Sub
    'Lors de l'ouverture de l'interface et bien il y a quelques initialisations.
    Private Sub frmCedReu_Loaded(sender As Object, e As RoutedEventArgs) Handles frmCedReu.Loaded

        _objReunion = New objReunion
        _lesInvitee = New List(Of tblMembre)

        lstInvite.ItemsSource = _lesInvitee

    End Sub
    'Permet d'ajouter un membre dans la liste choix vers la liste des invités.
    Private Sub tblAjouter_Click(sender As Object, e As RoutedEventArgs) Handles tblAjouter.Click

        _elementidentique = False

        For Each invites In lstInvite.Items
            If (invites Is lstChoix.SelectedItem) Then
                _elementidentique = True
            End If
        Next

        If (cmbRedac.SelectionBoxItem Is lstChoix.SelectedItem) Then
            _elementidentique = True
        End If

        If (_elementidentique = True) Then
            MessageBox.Show("Ce membre à déjà été ajouté.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information)
        Else
            lstInvite.ItemsSource = Nothing
            _lesInvitee.Add(CType(lstChoix.SelectedItem, tblMembre))
            lstInvite.ItemsSource = _lesInvitee
        End If

    End Sub
    'Permet de retirer un invité de la liste.
    Private Sub btnRetirer_Click(sender As Object, e As RoutedEventArgs) Handles btnRetirer.Click

        _lesInvitee.Remove(CType(lstInvite.SelectedItem, tblMembre))
        lstInvite.ItemsSource = Nothing
        lstInvite.ItemsSource = _lesInvitee

    End Sub
    'Lors de changement dans entre étudiant et enseignant un combobox des années s'active ou pas.
    Private Sub cmbType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbType.SelectionChanged

        If (cmbType.SelectedIndex = 0) Then
            cmbAnnée.IsEnabled = True
        Else
            cmbAnnée.IsEnabled = False

        End If
    End Sub
    'Lorsque le combobox du local et coché ou décoché le combobox local est activé ou pas.
    Private Sub chkLocal_Click(sender As Object, e As RoutedEventArgs) Handles chkLocal.Click

        cmbLocal.IsEnabled = False
        If (chkLocal.IsChecked = True) Then
            cmbLocal.IsEnabled = True
        ElseIf (chkLocal.IsChecked = False) Then
            cmbLocal.IsEnabled = False
        End If

    End Sub
    'Verifie que la date choisi n'est pas avant celle d'aujourd'hui
    Private Sub dtpDateRenc_SelectedDateChanged(sender As Object, e As SelectionChangedEventArgs) Handles dtpDateRenc.SelectedDateChanged

        If (dtpDateRenc.SelectedDate < Date.Now) Then
            MessageBox.Show("Veuillez mettre une date ultérieur à aujourd'hui", "Attention !", MessageBoxButton.OK, MessageBoxImage.Information)
            dtpDateRenc.SelectedDate = Nothing
        End If

    End Sub
    'Load liste selon le type de redacteur
    Private Sub cmbTypeRedac_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbTypeRedac.SelectionChanged
        cmbRedac.ItemsSource = _objReunion.loadListRedac(cmbTypeRedac.SelectedIndex)
    End Sub
End Class
