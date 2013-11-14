Imports System.Data
Imports System.Data.SqlClient
Imports System.String

    Public Class FctConnexion
        Dim CS As String
        Dim AddIp As String
    Dim NomBd As String



        ' Voici notre function servant à la connectionstring 

    Public Function GetConnectionString(ByVal user, ByVal password)
        CS = "Data Source=" + AddIp + ";Initial Catalog=" + NomBd + ";User ID=" + user + ";Password=" + password + ""
        Return CS
    End Function

    Sub EncodingLogin(ByVal _ip, ByVal _nomdebd, ByVal _utilisateur, ByVal _motdepasse)

        Dim InfConn As String = _ip + " " + _nomdebd
        Dim CodeCrypt As String = _utilisateur + " " + _motdepasse

        Dim wrapper As New Simple3Des(CodeCrypt)
        Dim cipherText As String = wrapper.EncryptData(InfConn)

        My.Computer.FileSystem.WriteAllText(
             My.Computer.FileSystem.SpecialDirectories.MyDocuments &
           "\InfServerBd2.txt", cipherText, False)
    End Sub

    Sub DecodingLogin(ByVal _utilisateur, ByVal _motdepasse)


        Dim cipherText As String = My.Computer.FileSystem.ReadAllText(
            My.Computer.FileSystem.SpecialDirectories.MyDocuments &
                "\InfServerBd2.txt")
        Dim CodeCrypt As String = _utilisateur + " " + _motdepasse
        Dim wrapper As New Simple3Des(CodeCrypt)

        ' DecryptData throws if the wrong password is used. 

        Dim infoLogin As String = wrapper.DecryptData(cipherText)

        Dim words As String() = infoLogin.Split(" ")
        AddIp = words(0)
        NomBd = words(1)

    End Sub
End Class
