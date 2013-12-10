Imports System
Imports System.Text
Imports System.IO
Imports System.Xml.Linq
Imports System.Collections.Generic
Imports System.Xml
Imports P2013_CreateDoc

Public Module ModRapport


    Public Class GenereRapport

        Private ModeleRapport As RaportOrd
        Private ModeleMat As RaportModele
        Private ModeleCours As RaportCours
        Private MonRapport As P2013_CreateDoc.CreateReport
        Private MonStyle As ModeleStyle
        Private MonPDF As P2013_CreateDoc.GenerePdf
        Private Path As String
        Private Temp As String
        Private FilePDF As String
        Private FileWord As String

        Public Property TempFilePDF As String
            Get
                Return FilePDF
            End Get
            Set(value As String)
                FilePDF = value
            End Set
        End Property

        Public Sub New()
            Path = Directory.GetCurrentDirectory + "\"
            Temp = Guid.NewGuid().ToString
        End Sub

        Public Sub CreerRapportOrd(ByVal Id As Integer)

            Dim mon_msg As String

            ModeleRapport = New RaportOrd(Id)

            MonStyle = New ModeleStyle("OrdJour001")

            MonRapport = New P2013_CreateDoc.CreateReport(ModeleRapport.GetContenuDoc, Temp, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), Path + MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()

            TempFilePDF = Path + Temp
        End Sub

        Public Sub CreerRapportMat(ByVal Id As String)

            Dim mon_msg As String

            ModeleMat = New RaportModele(Id)

            MonStyle = New ModeleStyle("LstMat001")

            MonRapport = New P2013_CreateDoc.CreateReport(ModeleMat.GetContenuDoc, Temp, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()

            TempFilePDF = Path + Temp
        End Sub

        Public Sub CreerRapportCours(ByVal Id As String)

            Dim mon_msg As String

            ModeleCours = New RaportCours(Id)

            MonStyle = New ModeleStyle("LCours001")

            MonRapport = New P2013_CreateDoc.CreateReport(ModeleCours.GetContenuDoc, Temp, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()

            TempFilePDF = Path + Temp
        End Sub
    End Class

    Public Class RaportOrd
        Inherits P2013_CreateDoc.ModeleInfos

        Private _Bd_Presence As New PresenceEntities

        Public Sub New(ByVal IdOrdre As String)
            MyBase.New(IdOrdre)
            GetData()
        End Sub

        Protected Overloads Sub GetData()

            Dim NoOrdre = (From Ordre In _Bd_Presence.tblOrdreDuJour
                           Where Ordre.NoOrdreDuJour = _IdElem
                           Select Ordre.TitreOrdreJour)


            Dim res = From od As tblListePoint In _Bd_Presence.tblListePoint Where od.NoOrdreDuJour = _IdElem Select od
            Dim prem = res.First()

            Dim rap = New XElement("root", New XElement("Contenu", New XElement("Head", New XAttribute("id", "Pts003"), NoOrdre)))

            TraiterPoint(rap, prem.tblPoints1)

            _ContenuDoc = rap

        End Sub

        Protected Sub TraiterPoint(parent As XElement, lst As IEnumerable(Of tblPoints))
            Dim enf As XElement
            For Each p As tblPoints In lst
                Try
                    enf = New XElement("Titre", p.ChiffrePoint & "  " & p.TitrePoint)
                    parent.Element("Contenu").Add(enf)

                    If p.ChiffrePoint.Length >= 6 Then
                        enf.Add(New XAttribute("id", "Pts004"))
                    ElseIf p.ChiffrePoint.Length >= 4 And p.ChiffrePoint.Length < 6 Then
                        enf.Add(New XAttribute("id", "Pts002"))
                    ElseIf p.ChiffrePoint.Length >= 2 And p.ChiffrePoint.Length < 4 Then
                        enf.Add(New XAttribute("id", "Pts001"))
                    End If

                    If p.ListeEnfants IsNot Nothing Then
                        TraiterPoint(parent, p.tblListePoint.tblPoints1)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Crash dans TraiterPoint")
                End Try

            Next

        End Sub

        Public Overloads Function GetContenuDoc()
            Return _ContenuDoc
        End Function



    End Class

    Public Class RaportModele
        Inherits P2013_CreateDoc.ModeleInfos

        Private _Bd_Presence As New PresenceEntities

        Public Sub New(ByVal IdOrdre As String)
            MyBase.New(IdOrdre)
            GetData()
        End Sub

        Protected Overloads Sub GetData()

            _ContenuDoc = New XElement("Root", New XElement("Head", New XElement("header", New XAttribute("id", "Mat005"), "Rapport de prêt - " & Date.Today)),
                (From Pret In _Bd_Presence.tblPret
                 Join Membre In _Bd_Presence.tblMembre
                 On Pret.IdMembre Equals Membre.IdMembre
                 Join PretExemp In _Bd_Presence.tblPretExemplaire
                 On PretExemp.IdPret Equals Pret.IdPret
                 Join Exemp In _Bd_Presence.tblExemplaire
                 On PretExemp.CodeBarre Equals Exemp.CodeBarre
                 Join Modele In _Bd_Presence.tblModele
                 On Exemp.NoModele Equals Modele.NoModele
            Where Pret.IdPret = _IdElem
                    Select New With {Pret.TypeEtat,
                                     Membre.PrenomMembre,
                                     Membre.NomMembre,
                                     Modele.NoModele,
                                     PretExemp.CommentairePretEx,
                                     PretExemp.DateDebutPretEx,
                                     PretExemp.DateFinPretEx,
                                     Exemp.CodeBarre,
                                     Exemp.NoSerie,
                                     Modele.Marque,
                                     Modele.NbAnneeGarantie,
                                     Modele.TypeMachine,
                                     Modele.PrixModele}).ToList.Select(
                          Function(x) New XElement("Modele",
                                New XElement("Conteneur",
                                    New XElement("Nom",
                                           New XAttribute("id", "Mat002"),
                                           "Matériel prêté:  " & x.PrenomMembre & "  " & x.NomMembre),
                                    New XElement("DateDeb",
                                        New XAttribute("id", "Mat002"),
                                        "Du: " & x.DateDebutPretEx),
                                    New XElement("DateFin",
                                        New XAttribute("id", "Mat002"),
                                        "Au: " & x.DateFinPretEx),
                                    New XElement("Commentaire",
                                        New XAttribute("id", "Mat002"),
                                        "Notes: " & x.CommentairePretEx),
                                    New XElement("Liste",
                                       New XAttribute("id", "Mat001"),
                                       "Vos Emprunts: "),
                                   New XElement("CodeBarre",
                                       New XAttribute("id", "Mat003"),
                                       "Code barre: " & x.CodeBarre),
                                   New XElement("NoSerie",
                                       New XAttribute("id", "Mat003"),
                                       "No série: " & x.NoSerie),
                                   New XElement("Marque",
                                       New XAttribute("id", "Mat003"),
                                       "Marque: " & x.Marque),
                                   New XElement("Garantie",
                                       New XAttribute("id", "Mat003"),
                                       "Nombre d'années de garanti: " & x.NbAnneeGarantie),
                                   New XElement("TypeMachine",
                                       New XAttribute("id", "Mat003"),
                                       "Type de machine: " & x.TypeMachine)
                                   ))))


        End Sub

        Public Overloads Function GetContenuDoc()
            Return _ContenuDoc
        End Function



    End Class

    Public Class RaportCours
        Inherits P2013_CreateDoc.ModeleInfos

        Private _Bd_Presence As New PresenceEntities

        Public Sub New(ByVal IdOrdre As String)
            MyBase.New(IdOrdre)
            GetData()
        End Sub

        Protected Overloads Sub GetData()

            '_ContenuDoc = New XElement("Root",
            '    (From Lst In _Bd_Presence.LstEtu(_IdElem)
            '        Select New With {Lst.NomCours,
            '                         Lst.PonderationCours,
            '                         Lst.DescriptionCours,
            '                         Lst.AnneeCours,
            '                         Lst.DaEtudiant,
            '                         Lst.PrenomMembre,
            '                         Lst.NomMembre,
            '                         Lst.AdresseMembre,
            '                         Lst.CourrielMembre}).ToList.Select(
            '              Function(x) New XElement("Cours", New XElement("Conteneur",
            '                       New XElement("NomCours",
            '                           New XAttribute("id", "Co001"),
            '                           x.NomCours),
            '                        New XElement("Ponderation",
            '                               New XAttribute("id", "Co002"),
            '                               x.PonderationCours),
            '                       New XElement("DescCours",
            '                           New XAttribute("id", "Co002"),
            '                           x.DescriptionCours),
            '                       New XElement("Annee",
            '                           New XAttribute("id", "Co002"),
            '                           x.AnneeCours),
            '                    New XElement("Fiche", New XAttribute("id", "Co004"),
            '                        New XElement("DaEtudiant",
            '                               New XAttribute("id", "Co003"),
            '                               x.DaEtudiant),
            '                       New XElement("Prenom",
            '                           New XAttribute("id", "Co003"),
            '                           x.PrenomMembre),
            '                       New XElement("NomMembre",
            '                           New XAttribute("id", "Co003"),
            '                           x.NomMembre),
            '                        New XElement("Adresse",
            '                               New XAttribute("id", "Co003"),
            '                               x.AdresseMembre),
            '                       New XElement("Courriel",
            '                           New XAttribute("id", "Co003"),
            '                           x.CourrielMembre)
            '                       )))))


        End Sub

        Public Overloads Function GetContenuDoc()
            Return _ContenuDoc
        End Function



    End Class

End Module
