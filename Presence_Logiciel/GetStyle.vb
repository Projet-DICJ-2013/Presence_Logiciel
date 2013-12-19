'Auteur: Patrick Pearson
'Objectif: Ce module permet de créer le XML relatif au Style à partir des données inscrites dans la base de données
Public Module StyleRapport

    Public Class ModeleStyle

        Private IdStyle As String
        Private Bd_Gest_Film As New PresenceEntities

        Public Sub New(ByVal _MonId As String)

            'Définit style associé le type de rapport
            IdStyle = _MonId

        End Sub

        Public Function GetStyle() As XElement

            'Recherche dans la BD les attributs de style relatif au type rapport et les écrits sous forme de XML
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

            'Fonction qui retourne le nom de fichier modèle associé au type du rapport
            Dim Modele As IQueryable(Of String) = _
            From TypeRapport In Bd_Gest_Film.tblTypeRapport
            Where TypeRapport.IdTypeRapport = IdStyle
            Select TypeRapport.NomFichierRapport

            Return Modele.First

        End Function

    End Class

End Module