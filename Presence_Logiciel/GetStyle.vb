Public Module StyleRapport

    Public Class ModeleStyle

        Private IdStyle As String
        Private Bd_Gest_Film As New PresenceEntities

        Public Sub New(ByVal _MonId As String)

            IdStyle = _MonId

        End Sub

        Public Function GetStyle() As XElement

            Dim StyleDefinition As XElement = New XElement("Root", _
                (From TypeRapport In Bd_Gest_Film.tblTypeRapport
                    Join Elem In Bd_Gest_Film.tblElement
                    On Elem.IdTypeRapport Equals TypeRapport.IdTypeRapport
                    Where TypeRapport.IdTypeRapport = IdStyle
                    Select New With {Elem.IdTypeElement,
                                     Elem.TypeElement,
                                     Elem.IdStyle,
                                     Elem.PositionElement}).ToList().Select(
                                Function(x) New XElement("Infos",
                               New XElement("Id", x.IdTypeElement),
                               New XElement("Type", x.TypeElement),
                               New XElement("Style", x.IdStyle),
                               New XElement("Pos", x.PositionElement))))

            Return StyleDefinition
        End Function

        Public Function GetModele() As String

            Dim Modele As IQueryable(Of String) = _
            From TypeRapport In Bd_Gest_Film.tblTypeRapport
            Where TypeRapport.IdTypeRapport = IdStyle
            Select TypeRapport.NomFichierRapport

            Return Modele.First

        End Function

    End Class

End Module