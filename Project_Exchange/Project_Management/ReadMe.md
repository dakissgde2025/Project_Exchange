Projekt feladat dokumentációja
Készítők:
Kiss Dániel MZS4YZ
Csepregi Máté OE4ZBJ
Radnai Gàbor 	M4I897
A weboldal elérhetősége:

A weboldal bemutatása:
A projektünk egy Golden Scale nevű, pénzváltással és értékbecsléssel foglalkozó cég bemutatkozó weboldala. A weboldal célja, hogy professzionális felületet biztosítson az ügyfelek tájékoztatására és az időpontfoglalásra.
A weboldal funkciói:
1.	Cégbemutató főoldal (Menüpont 1):
o	Kezdőlap, amely bemutatja a cég fő tevékenységeit (pénzváltás, értékbecslés).
o	Tartalmaz egy képgalériát (prezentáció), ami a bevizsgált tárgyakról (műtárgyak, érmék, relikviák) mutat be képeket.

2.	Aktuális árfolyamok (Menüpont 2):
o	Ezen az oldalon egy táblázat jeleníti meg a valós idejű valutaárfolyamokat.
o	Az adatok dinamikusan, a .NET backend API-ról érkeznek, amely egy MSSQL adatbázisból olvassa ki azokat.
o	Az adatok forrása: https://www.mnb.hu/arfolyamok
3.	Időpontfoglalás értékbecsléshez (Menüpont 3):
o	Ez a funkció teljesíti az űrlapra vonatkozó követelményt.
o	A folyamat több lépcsős:
	Regisztráció: Új felhasználók fiókot hozhatnak létre.
	Bejelentkezés: A regisztrált felhasználók be tudnak lépni a rendszerbe.
	Időpontfoglalás: Bejelentkezés után a felhasználó egy naptáras felületen választhat magának szabad időpontot értékbecslésre, és megadhatja a becsülni kívánt tárgy rövid leírását (űrlap).
Felhasznált technológiák:
•	Backend: .NET 8.0 (C#), Entity Framework Core, MSSQL adatbázis, ASP .Net MVC
•	Frontend: html5, CSS, JavaScript
