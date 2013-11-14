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
        Private Path As String = Directory.GetCurrentDirectory + "\"
        Private Elem As Rapport

        Public Sub CreerRapportOrd(ByVal Id As Integer)

            Dim mon_msg As String

            ModeleRapport = New RaportOrd(Id)

            MonStyle = New ModeleStyle("OrdJour001")

            MonRapport = New P2013_CreateDoc.CreateReport(ModeleRapport.GetContenuDoc, Id, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()
            MsgBox(mon_msg)

            MonPDF = New P2013_CreateDoc.GenerePdf(Id, Path)
            mon_msg = MonPDF.ConvertToPDF()
            MsgBox(mon_msg)

        End Sub

        Public Sub CreerRapportMat(ByVal Id As String)

            Dim mon_msg As String

            ModeleMat = New RaportModele(Id)

            MonStyle = New ModeleStyle("LstMat001")

            MonRapport = New P2013_CreateDoc.CreateReport(ModeleMat.GetContenuDoc, Id, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()
            MsgBox(mon_msg)

            MonPDF = New P2013_CreateDoc.GenerePdf(Id, Path)
            mon_msg = MonPDF.ConvertToPDF()
            MsgBox(mon_msg)

        End Sub

        Public Sub CreerRapportCours(ByVal Id As String)

            Dim mon_msg As String

            ModeleCours = New RaportCours(Id)

            MonStyle = New ModeleStyle("LCours001")

            MonRapport = New P2013_CreateDoc.CreateReport(ModeleCours.GetContenuDoc, Id, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()
            MsgBox(mon_msg)

            MonPDF = New P2013_CreateDoc.GenerePdf(Id, Path)
            mon_msg = MonPDF.ConvertToPDF()
            MsgBox(mon_msg)

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

            _ContenuDoc = New XElement("Root",
            (From List In _Bd_Presence.tblListePoint
               Join Point In _Bd_Presence.tblPoints
               On Point.tblListePoint1.FirstOrDefault Equals List
                Where List.NoOrdreDuJour = _IdElem
                Select New With {
                                  Point.TitrePoint,
                                 Point.NumeroPoint,
                                 Point.InformationPoint}).ToList.Select(
                      Function(x) New XElement("Point", New XElement("Conteneur",
                               New XElement("Nom",
                                   New XAttribute("id", "Pts001"),
                                   x.NumeroPoint),
                                New XElement("No",
                                       New XAttribute("id", "Pts002"),
                                       x.TitrePoint),
                               New XElement("Infos",
                                   New XAttribute("id", "Pts002"),
                                   x.InformationPoint)))))


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

            _ContenuDoc = New XElement("Root",
                (From Modele In _Bd_Presence.tblModele
                 Join Exemp In _Bd_Presence.tblExemplaire
                 On Exemp.tblModele.NoModele Equals Modele.NoModele
                    Select New With {Modele.NoModele,
                                     Modele.Marque,
                                     Modele.NbAnneeGarantie,
                                     Modele.TypeMachine,
                                     Exemp.CodeBarre,
                                     Exemp.TypeEtat}).ToList.Select(
                          Function(x) New XElement("Modele", New XElement("Conteneur", New XAttribute("id", "Mat004"),
                                   New XElement("NoModele",
                                       New XAttribute("id", "Mat001"),
                                       x.NoModele),
                                    New XElement("Marque",
                                           New XAttribute("id", "Mat002"),
                                           x.Marque),
                                   New XElement("Garantie",
                                       New XAttribute("id", "Mat002"),
                                       x.NbAnneeGarantie),
                                   New XElement("Type",
                                       New XAttribute("id", "Mat002"),
                                       x.TypeMachine),
                                    New XElement("CodeBarre",
                                           New XAttribute("id", "Mat003"),
                                           x.CodeBarre),
                                   New XElement("Etat",
                                       New XAttribute("id", "Mat003"),
                                       x.TypeEtat)
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

            _ContenuDoc = New XElement("Root",
                (From Cours In _Bd_Presence.tblCours
                 Join Prog In _Bd_Presence.tblProgramme
                 On Prog.tblCours.FirstOrDefault Equals Cours
                 Join Etu In _Bd_Presence.tblEtudiant
                On Etu.tblProgramme.FirstOrDefault Equals Prog
                 Join Membre In _Bd_Presence.tblMembre
                 On Membre.IdMembre Equals Etu.IdMembre
            Where Cours.CodeCours = _IdElem
                    Select New With {Cours.NomCours,
                                     Cours.PonderationCours,
                                     Cours.DescriptionCours,
                                     Cours.AnneeCours,
                                     Etu.DaEtudiant,
                                     Membre.PrenomMembre,
                                     Membre.NomMembre,
                                     Membre.AdresseMembre,
                                     Membre.CourrielMembre}).ToList.Select(
                          Function(x) New XElement("Cours", New XElement("Conteneur",
                                   New XElement("NomCours",
                                       New XAttribute("id", "Co001"),
                                       x.NomCours),
                                    New XElement("Ponderation",
                                           New XAttribute("id", "Co002"),
                                           x.PonderationCours),
                                   New XElement("DescCours",
                                       New XAttribute("id", "Co002"),
                                       x.DescriptionCours),
                                   New XElement("Annee",
                                       New XAttribute("id", "Co002"),
                                       x.AnneeCours),
                                    New XElement("DaEtudiant",
                                           New XAttribute("id", "Co003"),
                                           x.DaEtudiant),
                                   New XElement("Prenom",
                                       New XAttribute("id", "Co003"),
                                       x.PrenomMembre),
                                   New XElement("NomMembre",
                                       New XAttribute("id", "Co003"),
                                       x.NomMembre),
                                    New XElement("Adresse",
                                           New XAttribute("id", "Co003"),
                                           x.AdresseMembre),
                                   New XElement("Courriel",
                                       New XAttribute("id", "Co003"),
                                       x.CourrielMembre)
                                   ))))


        End Sub

        Public Overloads Function GetContenuDoc()
            Return _ContenuDoc
        End Function



    End Class

End Module

