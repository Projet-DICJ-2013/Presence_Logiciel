
Public Class int_CedReunion
    Private _objReunion As objReunion
    Private _lesInvitee As List(Of tblMembre)
    Private _redacteur As tblMembre
    Private _noordredujour As Integer


    Public Sub New(noordredujour As Integer)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()
        _noordredujour = noordredujour
        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub

    Private Sub btnCeduler_Click(sender As Object, e As RoutedEventArgs) Handles btnCeduler.Click
        _objReunion.ajoutReunionBd(dtpDateRenc.SelectedDate, txtEndroit.Text, cmbLocal.SelectionBoxItem.ToString(), _noordredujour)
        _redacteur = cmbRedac.SelectedItem
        _objReunion.ajoutMembreParticipantBd(_lesInvitee, _redacteur)
        _objReunion.OuvrirMail()


    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        If (chkLocal.IsChecked = True) Then
            cmbLocal.IsEnabled = True
        Else
            cmbLocal.IsEnabled = False
        End If


    End Sub

    Private Sub btnRechercher_Click(sender As Object, e As RoutedEventArgs) Handles btnRechercher.Click
        If (cmbType.SelectedIndex = 0) Then
            lstChoix.ItemsSource = _objReunion.Chargerliste(0, cmbAnnée.SelectedIndex)
        Else
            lstChoix.ItemsSource = _objReunion.Chargerliste(cmbType.SelectedIndex)
        End If

    End Sub

    Private Sub frmCedReu_Loaded(sender As Object, e As RoutedEventArgs) Handles frmCedReu.Loaded
        _objReunion = New objReunion
        _lesInvitee = New List(Of tblMembre)
        cmbRedac.ItemsSource = _objReunion.loadListRedac
        lstInvite.ItemsSource = _lesInvitee
    End Sub

    Private Sub tblAjouter_Click(sender As Object, e As RoutedEventArgs) Handles tblAjouter.Click
        lstInvite.ItemsSource = Nothing
        _lesInvitee.Add(CType(lstChoix.SelectedItem, tblMembre))
        lstInvite.ItemsSource = _lesInvitee
    End Sub

    Private Sub btnRetirer_Click(sender As Object, e As RoutedEventArgs) Handles btnRetirer.Click

        _lesInvitee.Remove(CType(lstInvite.SelectedItem, tblMembre))
        lstInvite.ItemsSource = Nothing
        lstInvite.ItemsSource = _lesInvitee
    End Sub

    Private Sub cmbType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbType.SelectionChanged
        If (cmbType.SelectedIndex = 0) Then
            cmbAnnée.IsEnabled = True
        Else
            cmbAnnée.IsEnabled = False

        End If
    End Sub
End Class
