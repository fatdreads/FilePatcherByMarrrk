originally written by Marrrk ;) check unity-community.de, search members for Marrrk.

http://forum.unity-community.de/topic/4112-patcher/page__pid__27477#entry27477

[ENGLISH]

TODO

[GERMAN]

- l�schern unben�tigter Daten/Verzeichnisse
- hinzuf�gen neuer Daten/Verzeichnisse
- aktualisieren ver�nderter Daten
- ignorieren bestimmter Verzeichnisse oder Daten beim erstellen des Patches
- vorkontrolle durch MD5 Checks ob der Patch angewendet werden kann
- nachkontrolle durch MD5 Checks ob der Patch erfolgreich angewendet wurde
- erstellen kompletter Upgrades (beliebige Vorversion auf Zielversion)
- ZIP Kompression der Patchdaten

Das ganze basiert auf 2 Klassen, den FilePatcher.Creator, welcher einen Patch von einer Vorversion auf die Zielversion erstellt, sowie den FilePatcher.Applier, welcher einen erstellen Patch auf ein Verzeichniss mit der passenden Vorversion anwendet.

Die Konfiguration der beiden Klassen ist Kinderleicht:

var creator = new FilePatcher.Creator(pfadZurVorversion, pfadZurZielVersion, pfadZumPatch);
creator.AllowCreateFileDifferences = true; // false wenn ein Upgrade erstellt werden soll
creator.DontPatchFilePaths.Add(pfadDerIgnoriertWerdenSoll);
creator.Create(); // erstellt den Patch

var applier = new FilePatcher.Applier(pfadZumPatch, pfadZumPatchendenZielVerzeichniss);
applier.SkipPreApplyCheck = false; // true um nicht zu checken ob die aktuele Version korrekt ist
applier.SkipPostApplyCheck = false; // true um nicht zu checken ob der Patch erfolgreich war
if (applier.CanApply())
  applier.Apply(); // wendet den Patch an

So geht das erstellen eines Patches von statten:

Zitat
Alle Dateien und Verzeichnisse werden gegen die Liste der zu ignorierenden Pfade gefiltert
1. Verzeichnisse werden gesucht die es in Version1 aber nicht in Version2 gibt
2. Verzeichnisse werden gesucht die es in Version2 aber nicht in Version1 gibt
3. Dateien werden gesucht die es in Version1 aber nicht in Version2 gibt
4. Dateien werden gesucht die es in Version2 aber nicht in Version1 gibt
5. Dateien werden gesucht die sich von Version1 auf Version2 ge�ndert haben
6. es werden MD5 Hashs f�r alle Datein aus Version1 erstellt
7. es werden MD5 Hashs f�r alle Dateien aus Version2 erstellt
8. Wenn n�tig werden die Dateiunterschiede zwischen version1 und Version2 gesammelt (Bin�rebene)
9. Die Patch Datei wird erstellt und geschrieben


Und so das anwenden des Patches:

Zitat
1. Wenn n�tig werden alle Dateien des Zielverzeichnisses gegen die gesammelten MD5 Hashs aus Version1 gecheckt
2. nicht mehr ben�tigte Verzeichnisse werden gel�scht
3. neu ben�tigte Verzeichnisse werden erstellt
4. nicht mehr ben�tigte Dateien werden gel�scht
5. neu ben�tigte Dateien werden erstellt
6. Datei�nderungen werden angewendet
7. Wenn n�tig werden alle Dateien des Zielverzeichnisses gegen die gesammelten MD5 Hashs aus Version2 gecheckt

Der Applier kann 2 Exceptions werfen:
Exception wenn der Patch nicht angewendet werden kann:
InvalidPatchVersionException - zeigt an welche Datei nicht der Vorversion entsprach f�r die der Patch gedacht war

Exception wenn der Patch fehlgeschlagen ist (sollte nicht passieren!):
ApplyingPatchFailedException - zeigt an welche Datei nicht der Zielversion nach dem patchen entspricht

Ich habe im Anhang eine ganze VisualStudio 2010 Solution welche alle n�tigen Klassen enth�lt, sowie 3 Beispielprogramme:
FilePatcher.exe - Konsolenanwendung welche sowohl den Creator als auch den Applier benutzt
FilePatcher.UI.exe - WindowsForms Anwendung welche alle Optionen f�r den Creator in einer grafischen Benutzeroberfl�che anzeigt und auch ausf�hrt
FilePatcher.Ui.Applier.exe - WindowsForms Anwendung welche einen Patch anwenden kann

In der Solution sind alle 3 Anwendungen vorhanden und k�nnen beliebig erweitert oder ver�ndert werden.

Der FilePatcher kann keinen Download und keine anschlie�ende Ausf�hrung anderer Anwendungen nachdem der Patch ausgef�hrt wurde, dies sind Features die auch ausserhalb des Contextes des patchers liegen, ipstyler hat dazu etwas geschrieben welche genau diese Bereiche abdeckt, daher kann da ein Blick nicht schaden.

