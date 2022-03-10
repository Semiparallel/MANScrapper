using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Selenium
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
//HTML
using HtmlAgilityPack;
using System.Net;
//File
using System.IO;
//Regex
using System.Text.RegularExpressions;
//For JPG


using System.Threading;





namespace MandACONSOLE
{
    class AnimePlanet
    {
        SQLHandler SQLHandler = new SQLHandler();
        public string ChromeEXELocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        //Search ENGINE URL
        public string AnimePlanetURL = @"https://www.anime-planet.com";
        //URL WHit Page Counter
        public string urlAnimesSearch = @"https://www.anime-planet.com/anime/all?";
        public string urlMangasSearch = @"https://www.anime-planet.com/manga/all?";
        //URL For New Items comming in.

        private string urlAnimeSearchTags = @"https://www.anime-planet.com/anime/all#multipletags";
        private string urlMangaSearchTags = @"https://www.anime-planet.com/manga/all?#multipletags";
        //url for testing purposes pls remove it later
        public string urlAnimesSearchCustom = @"https://www.anime-planet.com/anime/all?name=isekai";
        public string urlMangasSearchCustom = @"https://www.anime-planet.com/manga/all?name=isekai";

        public int howManyCrawlers = 5;
        //Selenium Options
        public ChromeDriver AnimePlanetIndex()
        {
            //get usefull data first

            //make a chromedriver call
            var options = new ChromeOptions()
            {
                BinaryLocation = ChromeEXELocation

            };

            options.AddArguments(new List<string>() { });
            var browser = new ChromeDriver(options);


            //Console.WriteLine(HowManyPages(browser, urlAnimesSearch));
            //AnimePlanetDragInfo(browser, urlMangasSearch);

            TagsDrag(browser, urlAnimeSearchTags);
            TagsDrag(browser, urlMangaSearchTags);

            return browser;
        }
        public void AnimePlanetDragInfo(string url, int whitchcrawler, int nummberofcrawler, bool SearchnewItems)
        {
            AnimePlanet AnimePlanet = new AnimePlanet();
            AnimePlanet Ani = new AnimePlanet();
            SQLHandler SQLHandler = new SQLHandler();
            
            //Make a chrome session
            var options = new ChromeOptions()
            {
                BinaryLocation = AnimePlanet.ChromeEXELocation

            };

            options.AddArguments(new List<string>() { "headless", "disable-gpu" });
            var browser = new ChromeDriver(options);

            //Get all Informations about the anime/manga
            
            if (SearchnewItems) //Only Search for Elements published in YEAR
            {
                DateTime current = DateTime.Now;
                browser.Navigate().GoToUrl(url+"year="+current.Year+"&to_year="+current.Year);
            }
            else //Download howl database
            {
                browser.Navigate().GoToUrl(url);
            }
            int MaxPage = HowManyPages(browser);
            //Make a calculation for 5 crawelrs (Webscrappers) to divide up the work.
            int MaxPage2 = MaxPage / Ani.howManyCrawlers;
            int actualSite = whitchcrawler * (MaxPage / Ani.howManyCrawlers);
            url = url + "page="+ actualSite;
            browser.Navigate().GoToUrl(url);
            //The Processing thingi
            for (int currentPage = 0 ; MaxPage2 > currentPage ; currentPage++)
            {
                string ATitle = "";
                string AType = "";
                string AProducer = "";
                string ARelease = "";
                string ARating = "";
                string ADescription = "";
                List<string> ATags = new List<string>();
                byte[] rawData = { 0, 0 };
                //Other
                IList<IWebElement> AnimesRaw = browser.FindElements(By.ClassName("card"));
                Actions action = new Actions(browser);
                int i = 0;
                foreach (IWebElement raw in AnimesRaw)
                {
                    Console.WriteLine("Scanning ... Page "+ (currentPage + 1) + "/" + MaxPage2 + " Element: " + (i + 1) + "/" + AnimesRaw.Count());

                    //Open the drag over tab
                    action.MoveToElement(raw).Perform();
             
                    IWebElement statusBand = browser.FindElement(By.ClassName("ui-tooltip-content"));

                    string innerHTML;
                    try
                    {
                        innerHTML = statusBand.GetAttribute("innerHTML");
                    }
                    catch (OpenQA.Selenium.StaleElementReferenceException)
                    {
                        do
                        {
                            try
                            {
                                Console.WriteLine("Waiting 0.5 Seconds");
                                Thread.Sleep(500);
                                innerHTML = statusBand.GetAttribute("innerHTML");
                            }
                            catch
                            {
                                innerHTML = "null";
                            }
                        } while (innerHTML != "null" || innerHTML != "");
                    }
                    i++;
                    //Enable this section only for debuging
                    //if (i >= 4)
                    //{
                    //    break;
                    //}



                    //Einsatz des HttmlAgility Pack

                    bool NOTAnime = false;
                    bool NOTManga = false;
                    HtmlDocument htmlDoc = new HtmlDocument();

                    Console.WriteLine("Saving ... Page" + (currentPage + 1) + "/" + MaxPage2 + " Element: " + (i + 1) + " / " + AnimesRaw.Count());
                    htmlDoc.LoadHtml(innerHTML);
                    IEnumerable<HtmlNode> rawhtml = htmlDoc.DocumentNode.SelectNodes("/h5");
                    foreach (var node in rawhtml)
                    {

                        ATitle = (node.InnerText);
                        //Get JPG From every Element
                        try
                        {
                            IWebElement cover = browser.FindElement(By.CssSelector("img[alt=" + @"'" + ATitle + @"']"));
                            string Ahref = cover.GetAttribute("src");
                            int indexHref = Ahref.IndexOf(".jpg");
                            string Ahref2 = "";
                            int b = 0;
                            foreach (char ch in Ahref)
                            {
                                if (b > (indexHref + 3))
                                {
                                    break;                     // TODO: SAVE THE IMAGE IN SQL
                                }
                                else
                                {
                                    Ahref2 += ch;
                                }
                                b++;
                            }
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFile(new Uri(Ahref), @"images\cover" + nummberofcrawler + i + ".jpg");
                            }
                            Thread.Sleep(600);
                            rawData = File.ReadAllBytes(@"images\cover" + nummberofcrawler + i + ".jpg");
                        }
                        catch
                        {
                            Console.WriteLine("Image = NULL");
                        }
                        if (Regex.IsMatch(ATitle, @"Novel"))
                        {
                            NOTManga = true;
                            ATitle = ATitle.Replace("(Light Novel)", "").Trim();
                            ATitle = ATitle.Replace("(Novel)", "").Trim();
                        }
                    }
                    try
                    {
                        rawhtml = htmlDoc.DocumentNode.SelectNodes("//li[@class='type']");
                        foreach (var node in rawhtml)
                        {

                            AType = (node.InnerText);

                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        //For Mangas search for Volume instead.
                        try
                        {
                            rawhtml = htmlDoc.DocumentNode.SelectNodes("//li[@class='iconVol']");
                            foreach (var node in rawhtml)
                            {
                                AType = (node.InnerText);
                                if (Regex.IsMatch(AType, @"Vol:") || Regex.IsMatch(AType, @"Ch:"))
                                {
                                    NOTAnime = true;
                                }

                            }
                        }
                        catch (System.NullReferenceException)
                        {
                            AType = ("NULL");
                        }
                    }

                    try
                    {
                        rawhtml = htmlDoc.DocumentNode.SelectNodes("/ul/li[2]");
                        foreach (var node in rawhtml)
                        {

                            AProducer = (node.InnerText);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        AProducer = ("NULL");
                    }

                    try
                    {
                        rawhtml = htmlDoc.DocumentNode.SelectNodes("//li[@class='iconYear']");
                        foreach (var node in rawhtml)
                        {

                            ARelease = (node.InnerText);
                        }
                    }

                    catch (System.NullReferenceException)
                    {
                        ARelease = ("TBA");
                    }
                    try
                    {
                        rawhtml = htmlDoc.DocumentNode.SelectNodes("//div[@class='ttRating']");
                        foreach (var node in rawhtml)
                        {

                            ARating = (node.InnerText);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        ARating = ("NULL");
                    }

                    try
                    {
                        if (NOTAnime)
                        {
                            rawhtml = htmlDoc.DocumentNode.SelectNodes("/p");
                            foreach (var node in rawhtml)
                            {
                                ADescription = (node.InnerText);
                            }
                        }
                        else
                        {
                            rawhtml = htmlDoc.DocumentNode.SelectNodes("//p");
                            foreach (var node in rawhtml)
                            {
                                ADescription = (node.InnerText);
                            }
                        }

                    }
                    catch (System.NullReferenceException)
                    {
                        ADescription = ("NULL");
                    }

                    rawhtml = htmlDoc.DocumentNode.SelectNodes("//div[@class='tags']/ul[.//li]");
                    try
                    {
                        foreach (var node in rawhtml)
                        {
                            ATags.Add(node.InnerHtml);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        ATags.Add("NULL");
                    }

                    //Console.WriteLine(ADescription[0]);

                    //For itterating trough a Nestet List and make it polished and final

                    string sentence = "";


                    foreach (var item in ATags)
                    {
                        var builder = new StringBuilder();
                        foreach (char ch in item)
                        {
                            builder.Append(ch);
                        }

                        sentence = builder.ToString().Replace("<li>", "").Trim();
                        sentence = sentence.Replace("</li>", ",").Trim();
                        sentence = sentence.Remove(sentence.Length - 1); //removes the last ","

                        //TODO Hier direkt in die MySQL Tabele einfügen falls es crasht hat man dan wenschon etwas und muss nicht von vorne anfangen!
                    }    //TODO NEXT get all animes from ANime-Planet and saved in sql whit no overlapping

                    //Check if Ani/manga/ or lightnovel
                    if (NOTAnime)
                    {
                        if (NOTManga)
                        {
                            SQLHandler.InsertFromNovelPlanet(ATitle,
                                                    AType,
                                                    AProducer,
                                                    ARelease,
                                                    ARating,
                                                    ADescription,
                                                    sentence,
                                                    rawData);
                        }
                        else
                        {
                            SQLHandler.InsertFromMangaPlanet(ATitle,
                                                    AType,
                                                    AProducer,
                                                    ARelease,
                                                    ARating,
                                                    ADescription,
                                                    sentence,
                                                    rawData);
                        }

                    }
                    else
                    {
                        SQLHandler.InsertFromAnimePlanet(ATitle,
                                                    AType,
                                                    AProducer,
                                                    ARelease,
                                                    ARating,
                                                    ADescription,
                                                    sentence,
                                                    rawData);
                    }
                    NOTAnime = false;
                    NOTManga = false;
                    i++;

    
                }

                //Print out all gotten Items (debugging)
                //foreach (var list in AnimesofAnimePlanetList)
                //{
                //    Console.WriteLine("**********************************************************************************");
                //    foreach (var item in list)
                //    {
                //        Console.WriteLine(item);
                //    }
                //}
                Console.WriteLine("Moving to next Page...");
                IWebElement NextButton = browser.FindElement(By.ClassName("nav"));
                action.MoveToElement(NextButton).Perform();
                NextButton.FindElement(By.ClassName("next")).Click();
                actualSite++;
            }
            
        }
        

        public int HowManyPages(ChromeDriver browser)
        {    
            IWebElement NextButton = browser.FindElement(By.ClassName("nav"));
            IList<IWebElement> NextButton2 = NextButton.FindElements(By.CssSelector("a"));
            return Int16.Parse(NextButton2[NextButton2.Count() -2].GetAttribute("innerHTML"));

        }
        void TagsDrag(ChromeDriver browser, string url)
        {

            browser.Navigate().GoToUrl(url);
            //Opens the Extended Tags Tab
            browser.FindElement(By.XPath("//*[\"@id=advanced_filter_tags\"]")).Click();
            //Locates first and extended Tags for Animes
            IList<IWebElement> tags = browser.FindElements(By.XPath("/html/body/div[1]/form/div[1]/div[6]/ul[1]/li/a"));
            IList<IWebElement> tagsExtended = browser.FindElements(By.XPath("/html/body/div[1]/form/div[1]/div[6]/ul[2]/li/a"));
            String[] aTags = new String[tags.Count];
            String[] bTags = new String[tagsExtended.Count];

            int i = 0;
            foreach (IWebElement element in tags)
            {
                aTags[i++] = element.Text;

            }
            i = 0;
            foreach (IWebElement element in tagsExtended)
            {
                bTags[i++] = element.Text;

            }
            //Save all Tags in a List 

            int nummberOFTags = bTags.Length + aTags.Length;
            String[] allTags = new String[nummberOFTags];
            i = 0;
            int b = 0;
            while (i < nummberOFTags)
            {
                if (aTags.Length > b)
                {
                    allTags[i++] = aTags[b];
                }

                allTags[i++] = bTags[b++];
            }
        }
        private void WriteToCsv(List<List<string>> Animes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var list in Animes)
            {
                foreach (var item in list)
                {
                    sb.AppendLine(item);
                }
            }
            File.WriteAllText("AnimePlanet.csv", sb.ToString());
            Console.WriteLine("Made a csv File!");
        }
    }
}
