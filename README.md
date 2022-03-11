# MANScrapper
Backend Script for an SQL Database containing all useful information scraped from the Website for Animes, Mangas and Novels. Very CPU intensive when set up. 

Next step:
Switching to GPU processing
>Any input is appreciated

## Setup
To run this project
SQL set up: 
```
Uncomment in Program.cs
//activate after debugging
//SQLHandler.GetLoginData();
and comment
SQLHandler.Server = "localhost";
SQLHandler.Port = "";
SQLHandler.Password = "123456789";
SQLHandler.UserID = "root";
otherwise its using the crediencals above.
```

Dont forget to download the repositorys. Its not in .exe format jet so you have to do it yourself.
