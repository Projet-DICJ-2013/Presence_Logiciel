Imports System.Data
Imports System.Data.SqlClient


Public Class frmConnexion
    Dim test As New FctConnexion



    Private Sub enregistrerconnexion(sender As Object, e As RoutedEventArgs)
        'test.EncodingLogin(txtIp.Text, txtBD.Text, txtLogin.Text, txtPassword.Text)

    End Sub

    Private Sub Connexion(sender As Object, e As RoutedEventArgs) Handles btnconnexion.Click
        Try
            test.DecodingLogin(txtLogin.Text, txtPassword.Text)
        Catch ex1 As Exception
            MsgBox("Mauvaise information de login.")
            Exit Sub
        End Try


        Dim connectionString As String
        Dim SqlCon As New SqlClient.SqlConnection

        connectionString = test.GetConnectionString(txtLogin.Text, txtPassword.Text)

        SqlCon = New SqlConnection(connectionString)
        Try
            SqlCon.Open()
            MsgBox("reussi")

        Catch ex As Exception
            MsgBox("Erreur de connection ! ")
        End Try

    End Sub

    Private Sub fermer(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub




    Private Sub minimiser(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub

    Private Sub AfficherHeure(sender As Object, e As RoutedEventArgs)


    End Sub


End Class
