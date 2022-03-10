using System;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace MandACONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            //Scrapping
            //string url = "http://www.googel.com";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());
            //sr.Close();


            //API Request

            //string jikanAPI = "https://api.jikan.moe/v3/search/anime?q=Fate/Zero&page=1";

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(jikanAPI);
            //request.Method = WebRequestMethods.Http.Get;
            //request.Accept = "application/json";
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            //Scrapping test from tut
            //Scrapper Scrapper = new Scrapper();
            //Scrapper.Index();

            //ScrappingAnimePlanet and save it to MYSQL
            AnimePlanet Ani = new AnimePlanet();
            SQLHandler SQLHandler = new SQLHandler();
            SQLQuerys SQLQuerys = new SQLQuerys();
            //Ask for SQL Credicials
            //for debbuging only
            SQLHandler.Server = "localhost";
            SQLHandler.Port = "";
            SQLHandler.Password = "123456789";
            SQLHandler.UserID = "root";
            //activate after debugging
            //SQLHandler.GetLoginData();

            SQLQuerys.Createdbifnotexist();
            //Where chrome.exe lies
            Ani.ChromeEXELocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            ChromeDriver browser = Ani.AnimePlanetIndex();
            //Starting of 5 Threads for AnimeSearch
            Thread ThreadObject1 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlAnimesSearch, 0, 1, false)); //Creating the Thread    
            ThreadObject1.Start(); //Starting the Thread
                                   //
            Thread ThreadObject2 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlAnimesSearch, 1, 2, false)); //Creating the Thread    
            ThreadObject2.Start(); //Starting the Thread    

            Thread ThreadObject3 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlAnimesSearch, 2, 3, false)); //Creating the Thread    
            ThreadObject3.Start(); //Starting the Thread    

            Thread ThreadObject4 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlAnimesSearch, 3, 4, false)); //Creating the Thread    
            ThreadObject4.Start(); //Starting the Thread    

            Thread ThreadObject5 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlAnimesSearch, 4, 5, false)); //Creating the Thread    
            ThreadObject5.Start(); //Starting the Thread


            //Starting another 5 Threads for MangaSearch and Novel search
            Thread ThreadObject6 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlMangasSearch, 0, 6, false)); //Creating the Thread    
            ThreadObject6.Start(); //Starting the Thread
                                   //
            Thread ThreadObject7 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlMangasSearch, 1, 7, false)); //Creating the Thread    
            ThreadObject7.Start(); //Starting the Thread    

            Thread ThreadObject8 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlMangasSearch, 2, 8, false)); //Creating the Thread    
            ThreadObject8.Start(); //Starting the Thread    

            Thread ThreadObject9 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlMangasSearch, 3, 9, false)); //Creating the Thread    
            ThreadObject9.Start(); //Starting the Thread    

            Thread ThreadObject10 = new Thread(() => Ani.AnimePlanetDragInfo(Ani.urlMangasSearch, 4, 10, false)); //Creating the Thread    
            ThreadObject10.Start(); //Starting the Thread 
        }
    }
}
