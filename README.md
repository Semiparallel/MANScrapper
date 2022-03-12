# MANScrapper (Manga, Anime, Novel) #

## Purpose: ##
Backend WORK IN PRORESS Scrapper for getting Informations of Anime's, Novels and Mangas that contains Stuff like:
#ReleaseDate #Title #Producer #Type #Rating #Descritpion #Tags #Cover in Blob Format.
Scrapped Sites at the moment.
Anime-Planet.com
## Setup ##
 * If your ChromeEXELocation is different set it in Program.cs file.

 * You do need logicaly a SQL Provider. Only providing MySQL!
   The script is asking for Credencials of the SQL.
   DB Name: MangaAnimeDbV1
 * Install the Libarys mentioned in Libarys used.
 
**Keep in Mind that it is very CPU intensive atm when set up!!!**

## Libarys used: ##
* HtmlAgilityPack; For stuff that can be done better whitout selenium.
* OpenQA.Selenium.Chrome; Using Chrome Driver for executing steps that need a simulated browser.
* System.Threading; For faster processing of the script.
* Mysql

## Future Implementation ##
Next Step: | DONE?
------------- | -------------
-Make GPU instead or choseable CPU/GPU | No
-Scrapp another Website for validation of data.  | No
-Merge the Scrappt Websites to one database. | No

In consideration: | DONE?
------------- | -------------
-Making stuff like DatabaseName, How mutch Threads are used and what else in progress gets in my mind Configurable. | No
-Track Mangas & show the person using this script when new chapters are out. | No
-Use another Browser Driver like Firefox | No
