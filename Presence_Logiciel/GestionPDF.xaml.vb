Imports System.IO
Imports Microsoft.Office.Interop.Word
Imports Microsoft.Win32
Imports System.Windows.Xps.Packaging


Public Class GestionPDF

    Private Sub BrowseButton_Click(sender As Object, e As RoutedEventArgs)
        Dim DocChoisi As New Microsoft.Win32.OpenFileDialog()

        documentViewer1.Document = Nothing

        DocChoisi.DefaultExt = ".docx"
        DocChoisi.Filter = "Word documents (.docx)|*.docx"

        If DocChoisi.ShowDialog() = True Then
            If DocChoisi.FileName.Length > 0 Then
                SelectedFileTextBox.Text = DocChoisi.FileName
                Dim newXPSDocumentName As String = [String].Concat(System.IO.Path.GetDirectoryName(DocChoisi.FileName), _
                                            "\", System.IO.Path.GetFileNameWithoutExtension(DocChoisi.FileName), ".xps")

                documentViewer1.Document = ConvertirXPS(DocChoisi.FileName, newXPSDocumentName).GetFixedDocumentSequence()
            End If
        End If
    End Sub

    Private Function ConvertirXPS(ByVal wordDocName As String, ByVal xpsDocName As String) As XpsDocument

        Dim wordApplication As New Microsoft.Office.Interop.Word.Application()
        wordApplication.Documents.Add(wordDocName)

        Dim doc As Document = wordApplication.ActiveDocument
        Try
            doc.SaveAs(xpsDocName, WdSaveFormat.wdFormatXPS)
            wordApplication.Quit()

            Dim xpsDoc As New XpsDocument(xpsDocName, System.IO.FileAccess.Read)
            Return xpsDoc
        Catch exp As Exception
            Dim str As String = exp.Message
        End Try
        Return Nothing
    End Function

    Private Sub AddNewReport_Click(sender As Object, e As RoutedEventArgs) Handles AddNewReport.Click
        Dim fnGestRapport As New frmGenereRapport
        fnGestRapport.ShowDialog()

    End Sub

    Private Sub btnX_Click(sender As Object, e As RoutedEventArgs) Handles btnX.Click
        Me.Close()
        Me.Finalize()

    End Sub
End Class

