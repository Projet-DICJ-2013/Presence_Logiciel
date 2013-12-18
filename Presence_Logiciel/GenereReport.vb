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

        Public Sub CreerRapportOrd(ByVal Id As Integer, Optional ByVal _IsPDF As Boolean = False)
            ModeleRapport = New RaportOrd(Id)
            CreerRapport(ModeleRapport.GetContenuDoc, _IsPDF, "OrdJour001")
        End Sub

        Public Sub CreerRapportMat(ByVal Id As String, Optional ByVal _IsPDF As Boolean = False)
            ModeleMat = New RaportModele(Id)
            CreerRapport(ModeleMat.GetContenuDoc, _IsPDF, "LstMat001")
        End Sub

        Public Sub CreerRapportCours(ByVal Id As String, Optional ByVal _IsPDF As Boolean = False)
            ModeleCours = New RaportCours(Id)
            CreerRapport(ModeleCours.GetContenuDoc, _IsPDF, "LCours001")
        End Sub

        Protected Sub CreerRapport(ByVal Contenu As XElement, ByVal IsPDF As Boolean, ByVal IdStyle As String)

            Dim mon_msg As String

            MonStyle = New ModeleStyle(IdStyle)

            MonRapport = New P2013_CreateDoc.CreateReport(Contenu, Temp, Path, False)

            MonRapport.DefineStyle(MonStyle.GetStyle(), MonStyle.GetModele())

            MonRapport.CreerWorld()
            mon_msg = MonRapport.IsGenere()

            If IsPDF Then
                CreerPDF(Temp, Path)
            Else
                TempFilePDF = Path + Temp
            End If

        End Sub

        Protected Sub CreerPDF(ByVal Temp As String, ByVal Path As String)

            MonPDF = New GenerePdf(Temp, Path)
            MonPDF.ConvertToPDF()
            TempFilePDF = Path + Temp + ".pdf"

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

        Public Overloads Function GetContenuDoc() As XElement
            Return _ContenuDoc
        End Function



    End Class

    Public Class RaportModele
        Inherits P2013_CreateDoc.ModeleInfos

        Private _Bd_Presence As New PresenceEntities
        Private _ModListe As XElement = <Liste>
                                            <Date></Date>
                                            <NoSerie></NoSerie>
                                            <Marque></Marque>
                                            <TypeMachine></TypeMachine>
                                            <Prix></Prix>
                                            <Commentaire></Commentaire>
                                        </Liste>

        Public Sub New(ByVal IdOrdre As String)
            MyBase.New(IdOrdre)
            GetData()
        End Sub

        Protected Overloads Sub GetData()

            Dim Materiel As XElement
            Dim Rap As New XElement("Root", New XElement("Head", New XElement _
            ("header", New XAttribute("id", "Mat005"), "Rapport de prêt - " & Date.Today)))

            Dim Pret = (From MonPret As tblPret In _Bd_Presence.tblPret
                        Where MonPret.IdPret = _IdElem
                        Select MonPret).First

            Materiel = New XElement("LstEmprunt", New XAttribute("id", "Mat006"))

            Rap.Add(New XElement("Corps",
                    New XElement("Nom",
                        New XAttribute("id", "Mat002"),
                        "Matériel prêté:  " & Pret.tblMembre.PrenomMembre _
                        & "  " & Pret.tblMembre.NomMembre), Materiel))

            Materiel.Add(CreateLstMat("Date du prêt", "Numéro de série", "Marque", "Type de machine", "Prix", "Commentaire", _
                                      New XAttribute("id", "Mat003")))

            Dim PretExemp As IEnumerable(Of tblPretExemplaire) = Pret.tblPretExemplaire

            For Each exemp As tblPretExemplaire In PretExemp

                Materiel.Add(CreateLstMat("Du: " & exemp.DateDebutPretEx & "  " & "Au: " & exemp.DateFinPretEx, _
                                exemp.tblExemplaire.NoSerie, _
                                exemp.tblExemplaire.tblModele.Marque, _
                                exemp.tblExemplaire.tblModele.TypeMachine, _
                                exemp.tblExemplaire.tblModele.PrixModele, _
                                exemp.CommentairePretEx, _
                                 New XAttribute("id", "Mat002")))
            Next


            _ContenuDoc = Rap

        End Sub

        Public Overloads Function GetContenuDoc() As XElement
            Return _ContenuDoc
        End Function

        Protected Function CreateLstMat(DatePret As String, NoSerie As String, Marque As String, Type As String, _
                                         ByVal Prix As String, ByVal Commentaire As String, ByVal Attr As XAttribute) As XElement
            Dim lstMat As New XElement(_ModListe)

            lstMat.Element("Date").Add(New XAttribute(Attr), New XText(DatePret))
            lstMat.Element("NoSerie").Add(New XAttribute(Attr), New XText(NoSerie))
            lstMat.Element("Marque").Add(New XAttribute(Attr), New XText(Marque))
            lstMat.Element("TypeMachine").Add(New XAttribute(Attr), New XText(Type))
            lstMat.Element("Prix").Add(New XAttribute(Attr), New XText(Prix))
            If (Commentaire Is Nothing) Then
                lstMat.Element("Commentaire").Add(New XAttribute(Attr), New XText("Aucun"))
            Else
                lstMat.Element("Commentaire").Add(New XAttribute(Attr), New XText(Commentaire))
            End If


            Return lstMat
        End Function

    End Class

    Public Class RaportCours
        Inherits P2013_CreateDoc.ModeleInfos

        Private _Bd_Presence As New PresenceEntities
        Private _ModListe As XElement = <Fiche>
                                            <DaEtudiant></DaEtudiant>
                                            <Prenom></Prenom>
                                            <Nom></Nom>
                                            <Adresse></Adresse>
                                            <Courriel></Courriel>
                                        </Fiche>

        Public Sub New(ByVal IdOrdre As String)
            MyBase.New(IdOrdre)
            GetData()
        End Sub

        Protected Overloads Sub GetData()

            Dim LstEtu As New XElement(_ModListe)
            Dim Rap As New XElement("Root", New XElement("Head", New XElement _
            ("header", New XAttribute("id", "Co005"), "Liste des étudiants - " & Date.Today)))

            Dim Cours = (From MonCours In _Bd_Presence.tblCours
                 Where MonCours.CodeCours = _IdElem
                    Select MonCours).First


            LstEtu = New XElement("LstEtu", New XAttribute("id", "Co004"))

            Rap.Add(New XElement("Cours",
                        New XElement("Conteneur",
                            New XElement("NomCours",
                                New XAttribute("id", "Co001"),
                                "Nom du cours: " & Cours.NomCours),
                            New XElement("Ponderation",
                                    New XAttribute("id", "Co002"),
                                    "Pondération: " & Cours.PonderationCours),
                            New XElement("DescCours",
                                New XAttribute("id", "Co002"),
                                "Description: " & Cours.DescriptionCours),
                            New XElement("Annee",
                                New XAttribute("id", "Co002"),
                                "Année: " & Cours.AnneeCours),
                            New XElement("Prof",
                                New XAttribute("id", "Co002"),
                                "Nom du professeur: " & _
                                Cours.tblCoursSessionGroupe.First.tblGroupe.tblProfesseur.First.tblMembre.PrenomMembre & " " & _
                                Cours.tblCoursSessionGroupe.First.tblGroupe.tblProfesseur.First.tblMembre.NomMembre), LstEtu
                        )))

            LstEtu.Add(CreateLstEtu("Numéro de DA", "Prénom de l'étudiant", "Nom de l'étudiant", "Adresse", "Courriel", _
                                      New XAttribute("id", "Co003")))

            For Each Etu In Cours.tblCoursSessionGroupe.First.tblGroupe.tblEtudiant
                LstEtu.Add(CreateLstEtu(Etu.DaEtudiant,
                            Etu.tblMembre.PrenomMembre,
                            Etu.tblMembre.NomMembre,
                            Etu.tblMembre.AdresseMembre,
                            Etu.tblMembre.CourrielMembre,
                            New XAttribute("id", "Co003")))
            Next

            _ContenuDoc = Rap

        End Sub

        Public Overloads Function GetContenuDoc() As XElement
            Return _ContenuDoc
        End Function

        Protected Function CreateLstEtu(DaEtudiant As String, Prenom As String, Nom As String, Adresse As String, _
                                         ByVal Courriel As String, ByVal Attr As XAttribute) As XElement
            Dim lstCours As New XElement(_ModListe)

            lstCours.Element("DaEtudiant").Add(New XAttribute(Attr), New XText(DaEtudiant))
            lstCours.Element("Prenom").Add(New XAttribute(Attr), New XText(Prenom))
            lstCours.Element("Nom").Add(New XAttribute(Attr), New XText(Nom))
            lstCours.Element("Adresse").Add(New XAttribute(Attr), New XText(Adresse))
            lstCours.Element("Courriel").Add(New XAttribute(Attr), New XText(Courriel))


            Return lstCours
        End Function

    End Class

End Module
