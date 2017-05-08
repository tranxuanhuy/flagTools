using System;
using System.Linq;
using System.IO;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EAGetMail;
using System.Text.RegularExpressions;

class SelenFirefox
{

    [DllImport("wininet.dll")]
    public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
    public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
    public const int INTERNET_OPTION_REFRESH = 37;
    static bool settingsReturn, refreshReturn;
    //static void Main(string[] args)
    //{
    //check license

    //flag proxy
    //FlagProxyModuleNoPR();

    //IWebDriver WebDriver = null;

    //try
    //{
    //    System.Uri uri = new System.Uri("http://localhost:7055/hub");
    //    WebDriver = new RemoteWebDriver(uri, DesiredCapabilities.Firefox());
    //    Console.WriteLine("Executed on remote driver");
    //}
    //catch (Exception)
    //{
    //    WebDriver = new FirefoxDriver();
    //    Console.WriteLine("Executed on New FireFox driver");
    //}

    //FlagSockModuleNoPR();
    //ChromeFlagSockModuleNoPR();
    //IeFlagProxyModuleNoPR();

    //CheckLinkDie();

    //string textBody = File.ReadAllText("adsbody.txt");
    //Console.WriteLine(RandomReplaceCharacter(textBody,5));
    //Console.Read();

    //PostingProxyModuleNoPR();

    //GetZipcodeOfProxy();

    //BuildCredenticalXmlFileForFirefox();
    //ChangeAnnotation();

    //BuildProxifierProfierWithProxy();
    //BuildProxifierProfierWithSocks5();



    //FirefoxProfileManager profileManager = new FirefoxProfileManager();
    //FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
    //IWebDriver driver = new FirefoxDriver(profile);
    //driver.Navigate().GoToUrl("http://www.ip2location.com");



    //GetCityOfProxy();




    //}

    public static void IeFlagProxyModuleNoPR()
    {
        int tab;
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();
        //ReplacePR();

        IWebDriver driver = new InternetExplorerDriver(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\");

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            //driver = RestartFirefox(profile);
            driver = new InternetExplorerDriver(@"C:\Users\Administrator\Documents\Visual Studio 2010\Projects\");

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                //cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                string proxy = ReadProxyAtLine(i, "proxy.txt");
                SetProxyEntireComputer(proxy);
                //var userAgent = ReadRandomLineOfFile("useragentswitcher.txt");
                //SetUserAgentEntireComputer(userAgent);

                var links = ReadnLinks();

                //load links
                tab = 1;
                int[] live = new int[5];

                {
                    try
                    {
                        foreach (string link in links)
                        {
                            IWebElement body = driver.FindElement(By.TagName("body"));
                            driver.Navigate().GoToUrl(link);

                            driver.FindElement(By.PartialLinkText("prohibited")).Click();
                            System.Threading.Thread.Sleep(2000);

                            string a = driver.FindElement(By.TagName("body")).Text;
                            if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                                if (driver.Url.Contains("craigslist.org") && !driver.Url.Contains("post.craigslist.org"))
                                    live[tab - 1] = 1;
                        }
                    }
                    catch (Exception e)
                    {
                        for (int i1 = 0; i1 < 4; i1++)
                        {
                            live[i1] = 1;
                        }
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }


                //deleteDeadLink 
                DeleteDeadLink(driver, links, live);
            }

            DeleteFirstLineSock("proxy.txt");
            ReplacePR();
        }
    }

    public static void SetUserAgentEntireComputer(string proxy)
    {
        RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
        registry.SetValue("User Agent", proxy);
        // These lines implement the Interface in the beginning of program 
        // They cause the OS to refresh the settings, causing IP to realy update
        settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
    }

    //socks.txt => proxyWithCity.txt
    public static void GetCityOfSock()
    {
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        //driver.Quit();
        //driver = RestartFirefox(profile);
        //File.Delete("proxyWithCity.txt");


        for (int i = 0; i < File.ReadAllLines("socks.txt").Count(); i++)
        {
            string proxy = ReadProxyAtLine(i + 1, "socks.txt");
            //set socks for firefox
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
            {
                file.Write(proxy);
            }
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("ChangeSockFirefox.exe");
            cmd.WaitForExit();

            //driver.Navigate().GoToUrl("https://www.privateinternetaccess.com/pages/whats-my-ip/");
            driver.Navigate().GoToUrl("http://www.ip-score.com/");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCity.txt", true))
            {
                try
                {
                    //file.WriteLine(File.ReadLines("socks.txt").Skip(i).First() + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[3]/span[2]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[4]/span")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[5]/span")).Text); 
                    file.WriteLine(File.ReadLines("proxy.txt").Skip(i).First() + "\t" + driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[1]/p[2]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[1]/p[3]")).Text);
                    Console.WriteLine(i + 1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }
        }
    }

    //proxy.txt => proxyWithCity.txt
    public static void GetCityOfProxy()
    {
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        //driver.Quit();
        //driver = RestartFirefox(profile);
        //File.Delete("proxyWithCity.txt");

        for (int i = 0; i < File.ReadAllLines("proxy.txt").Count(); i++)
        {
            string proxy = ReadProxyAtLine(i + 1, "proxy.txt");
            //set socks for firefox
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
            {
                file.Write(proxy);
            }
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("ChangeProxyFirefox.exe");
            cmd.WaitForExit();

            //driver.Navigate().GoToUrl("https://www.privateinternetaccess.com/pages/whats-my-ip/");
            driver.Navigate().GoToUrl("http://www.ip-score.com/");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCity.txt", true))
            {
                try
                {
                    //file.WriteLine(File.ReadLines("socks.txt").Skip(i).First() + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[3]/span[2]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[4]/span")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[5]/span")).Text); 
                    string info = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[1]/p[3]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[1]/p[2]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div/div[2]/div[1]/div[1]/div/div[1]/p[1]")).Text;
                    info = info.Replace("State: ", "");
                    info = info.Replace("City: ", "");
                    info = info.Replace(".", "");
                    for (int k1 = 0; k1 < 10; k1++)
                    {
                        info = info.Replace(k1 + " ", "");
                        info = info.Replace(k1.ToString(), "");
                    }
                    file.WriteLine(File.ReadLines("proxy.txt").Skip(i).First() + "\t" + info);
                    Console.WriteLine(i + 1);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }
        }
    }

    //dnslist.txt
    public static void GetDns()
    {
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        driver.Navigate().GoToUrl("http://public-dns.tk/nameserver/us.html");

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", false))
        {
            try
            {
                file.WriteLine(driver.FindElement(By.TagName("body")).Text);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }

        File.Delete("dnslist.txt");
        var proxies = File.ReadAllLines("temp.txt");
        foreach (string proxy in proxies)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("dnslist.txt", true))
            {
                var match = Regex.Match(proxy, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
                if (match.Success) file.WriteLine(match.Captures[0] + "|");
            }
        }
    }

    public class Demo
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public void WriteToFileThreadSafe(string text, string path)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }
    }

    public class MyThread
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;

            string MyProxyHostString = proxy.Split(':')[0];
            int MyProxyPort = int.Parse(proxy.Split(':')[1]);
            Demo w = new Demo();
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create("http://ip-score.com");
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                string filename = thr.Name.Replace(".", " ");
                filename = filename.Replace(":", " ");
                filename = "temp\\output" + filename + ".txt";
                File.WriteAllText(filename, responseFromServer);
                string info = File.ReadLines(filename).Skip(145).First() + File.ReadLines(filename).Skip(146).First() + File.ReadLines(filename).Skip(147).First();
                info = info.Remove(0, info.IndexOf("png\">") + 6);
                info = info.Replace("</p>							<p><em>State:</em> ", "\t");
                info = info.Replace("</p>							<p><em>City:</em> ", "\t");
                info = info.Replace("</p>", "");
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
                //{
                //    try
                //    {
                //        file.WriteLine(proxy + "\t" + info);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}   
                w.WriteToFileThreadSafe(proxy + "\t" + info, "proxyWithCityHttpRequest.txt");
            }
            catch (Exception e)
            {
                w.WriteToFileThreadSafe(proxy, "proxyWithCityHttpRequest.txt");
                Console.WriteLine("{0} Second exception caught.", e);
            }

            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();
        }
    }

    public class MyThread10
    {
        public void Thread2()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            string pvas = thr.Name;

            bool systemfake = true;
            System.Diagnostics.Process cmd;
            IWebDriver driver = null;

            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("default");

            //profile.SetPreference("network.proxy.type", 1);
            //profile.SetPreference("network.proxy.socks", "127.0.0.1");
            //profile.SetPreference("network.proxy.socks_port", int.Parse(pvas.Split('\t')[3]));
            //profile.SetPreference("network.proxy.http", "");
            //profile.SetPreference("network.proxy.http_port", "");
            ChangeSockFirefox(profile, ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt"));
            driver = new FirefoxDriver(profile);
            //driver = new FirefoxDriver();


            bool us = true;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));


            try
            {
                driver.Navigate().GoToUrl("http://craigslist.org");
                driver.FindElement(By.LinkText("post to classifieds")).Click();
                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(6, 10);

                //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[6]")).Click();
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();

                //Numrd = rd.Next(1, 40);
                //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
                if (Numrd == 6)
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[21]")).Click();
                }
                else
                    driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[20]")).Click();

            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                try
                {
                    string email = pvas.Split('\t')[0];
                    if (!email.Contains("@")) email += "@gmail.com";
                    driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                    driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                    driver.FindElement(By.Id("inputPassword")).Submit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            try
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                try
                {
                    driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                    driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //input adds content
                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(10000, 99999);
                driver.FindElement(By.Id("postal_code")).SendKeys(Numrd + "");
                //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
                //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                //string textBody = "<pre>                                                                                                                                                                                                              ";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
                //textBody += "</pre>";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

                //GetRandomAds(driver1);
                string title = File.ReadAllText("adstitle.txt").Split('|')[Numrd];
                //title = RandomUppercaseCharacter(title, 10);
                //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
                Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
                string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];
                textBody = textBody.Replace(".", ReadRandomLineOfFile("specialSymbol.txt") + ".");
                //string phone = "Cell no: 706 801 7213";
                //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

                Numrd = rd.Next(6, 20);
                string subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                string pre = "", post = "", titlejunk = "";
                for (int i = 0; i < rd.Next(20); i++)
                {
                    pre += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                for (int i = 0; i < rd.Next(700, 1000); i++)
                {
                    if (!post.Contains(ReadRandomLineOfFile("adsjunk.txt")))
                    {
                        post += ReadRandomLineOfFile("adsjunk.txt") + " ";
                    }
                }
                driver.FindElement(By.Id("PostingBody")).SendKeys(textBody + "\n" + post);

                Numrd = rd.Next(1, 4);
                subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                for (int i = 0; i < rd.Next(2, 3); i++)
                {
                    if (!titlejunk.Contains(ReadRandomLineOfFile("adsjunk.txt")))
                    {
                        titlejunk += ReadRandomLineOfFile("adsjunk.txt") + " ";
                    }
                }
                driver.FindElement(By.Id("PostingTitle")).SendKeys(ReadRandomLineOfFile("specialSymbol.txt") + title + " " + titlejunk);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            try
            {
                driver.FindElement(By.Id("wantamap")).Click();
                driver.FindElement(By.Id("contact_name")).Submit();

                ////upload imageg
                //driver.FindElement(By.Id("plupload")).Click();

                //string link = @"C:\imageupload\";
                //var files = new DirectoryInfo(link).GetFiles();
                //int index1 = new Random().Next(0, files.Length);

                //using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
                //{
                //    file.Write(link + files[index1].Name);
                //}
                //System.Diagnostics.Process cmd1;
                //cmd1 = System.Diagnostics.Process.Start("uploadImage.exe");
                //cmd1.WaitForExit();
                //System.Threading.Thread.Sleep(60000);
                ////end upload image

                driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                ////publish
                driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            //skip map
            try
            {
                driver.FindElement(By.ClassName("skipmap")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }


            if (!driver.FindElement(By.TagName("body")).Text.Contains("Thanks for posting with us. We really appreciate it!"))
            {
                //lay mail confirm ve may
                int stt = getMailConfirm(pvas);


                //load link confirm len ff
                var links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                foreach (string link in links)
                    if (driver.FindElement(By.TagName("body")).Text.Contains(link.Split('\t')[0]))
                    {
                        driver.Navigate().GoToUrl(link.Split('\t')[1]);
                        break;
                    }
            }

            //click link confirm & accept ToS        
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {
                Demo w = new Demo();
                w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam(), "adsLogLink.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            //driver.Quit();
        }

        public static int getMailConfirm(string pvas, int reset = 0, int numMail = 0)
        {
            if (numMail == 0)
            {
                numMail = File.ReadAllLines("pvas.txt").Count();
            }
            //System.Threading.Thread.Sleep(120000);
            int stt = int.Parse(pvas.Split('\t')[4]);
            bool outlook = pvas.Contains("@outlook.com");
            if (outlook)
            {
                getMailOutlook(stt, numMail, reset);
            }
            else
            {
                getMailGoogle(stt, numMail, reset);
            }
            return stt;
        }
        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            string pvas = thr.Name;

            bool systemfake = true;
            System.Diagnostics.Process cmd;
            IWebDriver driver = null;

            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("default");

            //profile.SetPreference("network.proxy.type", 1);
            //profile.SetPreference("network.proxy.socks", "127.0.0.1");
            //profile.SetPreference("network.proxy.socks_port", int.Parse(pvas.Split('\t')[3]));
            //profile.SetPreference("network.proxy.http", "");
            //profile.SetPreference("network.proxy.http_port", "");

            ChangeSockFirefox(profile, ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt"));
            driver = new FirefoxDriver(profile);
            //driver = new InternetExplorerDriver(@"C:\");
            //driver = new FirefoxDriver();


            bool us = true;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));



            try
            {
                //                  driver.Navigate().GoToUrl("http://craigslist.org");

                //driver.FindElement(By.LinkText("post to classifieds")).Click();
                driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
                driver.FindElement(By.LinkText("post to classifieds")).Click();
                int Numrd;
                Random rd = new Random();
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[10]")).Click();
                System.Threading.Thread.Sleep(3000);
                Numrd = rd.Next(1, 18);
                //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[12]")).Click();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/table/tbody/tr/td[1]/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                try
                {
                    string email = pvas.Split('\t')[0];
                    if (!email.Contains("@")) email += "@gmail.com";
                    driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                    driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                    driver.FindElement(By.Id("inputPassword")).Submit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            try
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                try
                {
                    driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                    driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //accept tos
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(10000, 99999);
                //input adds content
                //string zip = GetZipcodeOfProxyByHttpRequest();
                //if (zip.Contains("N/A"))
                //{
                //    zip = "59000";
                //}
                driver.FindElement(By.Id("postal_code")).SendKeys(Numrd + "");
                Numrd = rd.Next(100000000, 999999999);
                //driver.FindElement(By.Id("contact_phone")).SendKeys("0" + Numrd);
                Numrd = rd.Next(100, 999);
                driver.FindElement(By.Id("contact_phone_extension")).SendKeys("" + Numrd);
                driver.FindElement(By.Id("GeographicArea")).SendKeys(Path.GetRandomFileName().Replace(".", ""));
                driver.FindElement(By.Id("contact_name")).SendKeys(Path.GetRandomFileName().Replace(".", ""));
                //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
                //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                //string textBody = "<pre>                                                                                                                                                                                                              ";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
                //textBody += "</pre>";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));


                Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

                //GetRandomAds(driver1);
                string title = ReadRandomLineOfFile("adstitle.txt");
                //title = RandomUppercaseCharacter(title, 10);
                //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
                Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
                string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];
                textBody = textBody.Replace(".", ReadRandomLineOfFile("specialSymbol.txt") + ".");
                //string phone = "Cell no: 706 801 7213";
                //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

                Numrd = rd.Next(6, 20);
                string subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                string pre = "", post = "", titlejunk = "";
                for (int i = 0; i < rd.Next(20); i++)
                {
                    pre += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                for (int i = 0; i < rd.Next(20); i++)
                {
                    post += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                //for (int i = 0; i < rd.Next(700, 1000); i++)
                //{
                //    if (!post.Contains(ReadRandomLineOfFile("adsjunk.txt")))
                //    {
                //        post += ReadRandomLineOfFile("adsjunk.txt") + " ";
                //    }
                //}
                driver.FindElement(By.Id("PostingBody")).SendKeys(pre + "\n" + textBody + "\n" + post);
                //driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);

                Numrd = rd.Next(1, 4);
                subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                for (int i = 0; i < rd.Next(2, 3); i++)
                {
                    if (!titlejunk.Contains(ReadRandomLineOfFile("adsjunk.txt")))
                    {
                        titlejunk += ReadRandomLineOfFile("adsjunk.txt") + " ";
                    }
                }
                //driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
                driver.FindElement(By.Id("PostingTitle")).SendKeys(ReadRandomLineOfFile("specialSymbol.txt") + title + " " + Path.GetRandomFileName().Replace(".", ""));
                //driver.FindElement(By.Id("PostingTitle")).Clear();
                //cmd = System.Diagnostics.Process.Start("postingtitle.exe");
                //cmd.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {

                driver.FindElement(By.Id("wantamap")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            try
            {

                driver.FindElement(By.Id("contact_name")).Submit();
                driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //publish
                driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            if (!driver.FindElement(By.TagName("body")).Text.Contains("Thanks for posting with us. We really appreciate it!"))
            {
                //lay mail confirm ve may
                int stt = getMailConfirm(pvas);


                //load link confirm len ff
                var links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                foreach (string link in links)
                    if (driver.FindElement(By.TagName("body")).Text.Contains(link.Split('\t')[0]))
                    {
                        driver.Navigate().GoToUrl(link.Split('\t')[1]);
                        break;
                    }
            }

            //click link confirm & accept ToS        
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {
                Demo w = new Demo();
                w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam(), "adsLogLink.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            driver.Quit();
        }

        public void Thread5()
        {
            Thread thr = Thread.CurrentThread;
            string pvas = thr.Name;

        Restart:
            IWebDriver driver = null;
            string proxy = "";


            //InternetExplorerOptions options = new InternetExplorerOptions();

            //options.ForceCreateProcessApi = true;
            //options.BrowserCommandLineArguments = "-private";
            //driver = new InternetExplorerDriver(options);

            ChromeOptions options = new ChromeOptions();
            if (int.Parse(pvas.Split('\t')[6]) != 77)
            {
                proxy = ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt");
                options.AddArguments("--proxy-server=socks5://" + proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);
            }


            driver = new ChromeDriver(@"C:\", options);


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            try
            {
                //                  driver.Navigate().GoToUrl("http://craigslist.org");

                //driver.FindElement(By.LinkText("post to classifieds")).Click();
                driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
                driver.FindElement(By.LinkText("post to classifieds")).Click();
                Random rd = new Random();
                string code = "";
                while (code == "")
                {
                    try
                    {
                        code = driver.FindElement(By.TagName("body")).Text;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
                while (!driver.FindElement(By.TagName("body")).Text.Contains("please choose a category"))
                {
                    driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[10]/input")).Click();
                    System.Threading.Thread.Sleep(5000);
                }
                //System.Threading.Thread.Sleep(5000);
                driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[12]/input")).Click();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            //create pva
            int pass = 0;
            while (true)
            {
                if (pass == 1) break;
                try
                {
                    while (!driver.FindElement(By.TagName("body")).Text.Contains("Thanks for signing up for a craigslist account."))
                    {
                        if (driver.FindElement(By.TagName("body")).Text.Contains("post to classifieds"))
                        {
                            driver.Quit();
                            goto Restart;
                        }

                        try
                        {
                            driver.FindElement(By.Id("emailAddress")).SendKeys(pvas.Split('\t')[0]);
                            driver.FindElement(By.Id("emailAddress")).Submit();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                    pass = 1;
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            string codebody = "";
            try
            {
                codebody = driver.FindElement(By.TagName("body")).Text;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
                //goto publish;
            }
            while (!codebody.Contains("Thanks for posting with us. We really appreciate it!"))
            {
                ResetProxySockEntireComputer();
                int ok = 0;
            //lay mail confirm ve may
            getmail:
                int stt = MyThread10.getMailConfirm(pvas, 1);

                //load link confirm len ff
                var links = File.ReadAllLines("socks.txt");
                try
                {
                    links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                    goto getmail;
                }
                foreach (string link in links)
                    if (codebody.Contains(link.Split('\t')[0]))
                    {
                        driver.Navigate().GoToUrl(link.Split('\t')[1]);
                        ok = 1;
                        break;
                    }
                if (ok == 1)
                {
                    break;
                }
            }

            //click link confirm & accept ToS        
            try
            {
                string password = pvas.Split('\t')[1];
                driver.FindElement(By.Id("p1")).SendKeys(password);
                driver.FindElement(By.Id("p2")).SendKeys(password);
                driver.FindElement(By.Id("p2")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            //end create pva
            //post first ads

            try
            {
                //                  driver.Navigate().GoToUrl("http://craigslist.org");

                //driver.FindElement(By.LinkText("post to classifieds")).Click();
                driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
                driver.FindElement(By.LinkText("post to classifieds")).Click();
                System.Threading.Thread.Sleep(5000);
                //accept tos
                try
                {
                    driver.FindElement(By.XPath("/html/body/form[1]/input[4]")).Click();
                    System.Threading.Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                driver.Navigate().GoToUrl("http://craigslist.org");
                driver.FindElement(By.LinkText("post to classifieds")).Click();
                Random rd = new Random();
                string code = "";
                while (code == "")
                {
                    try
                    {
                        code = driver.FindElement(By.TagName("body")).Text;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
                while (!driver.FindElement(By.TagName("body")).Text.Contains("please choose a category"))
                {
                    driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[10]/input")).Click();
                    System.Threading.Thread.Sleep(5000);
                }
                //System.Threading.Thread.Sleep(5000);
                driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[12]/input")).Click();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            pass = 0;
            while (true)
            {
                if (pass == 1) break;
                try
                {
                    while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
                    {
                        if (driver.FindElement(By.TagName("body")).Text.Contains("post to classifieds"))
                        {
                            driver.Quit();
                            goto Restart;
                        }

                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[1]/input")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                        try
                        {
                            driver.FindElement(By.XPath("/html/body/article/section/form/table/tbody/tr/td[1]/blockquote/label[1]")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                        try
                        {
                            string email = pvas.Split('\t')[0];
                            if (!email.Contains("@")) email += "@gmail.com";
                            driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                            driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                            driver.FindElement(By.Id("inputPassword")).Submit();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                    pass = 1;
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            try
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                try
                {
                    driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                    driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //accept tos
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(10000, 99999);
                //input adds content
                //string zip = GetZipcodeOfProxyByHttpRequest();
                //if (zip.Contains("N/A"))
                //{
                //    zip = "59000";
                //}
                driver.FindElement(By.Id("postal_code")).SendKeys("00000");
                //driver.FindElement(By.Id("postal_code")).SendKeys("A0A");
                //driver.FindElement(By.Id("postal_code")).SendKeys(Numrd + "");
                //Numrd = rd.Next(100000000, 999999999);
                //Numrd = rd.Next(100, 999);
                //driver.FindElement(By.Id("contact_phone_extension")).SendKeys("" + Numrd);
                //driver.FindElement(By.Id("GeographicArea")).SendKeys(Path.GetRandomFileName().Replace(".", ""));
                //driver.FindElement(By.Id("contact_name")).SendKeys(Path.GetRandomFileName().Replace(".", ""));

                //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
                //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                //string textBody = "<pre>                                                                                                                                                                                                              ";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
                //textBody += "</pre>";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));


                Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

                //GetRandomAds(driver1);
                string title = ReadRandomLineOfFile("adstitle.txt").Replace("\t", "");
                //title = RandomUppercaseCharacter(title, 10);
                //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
                Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
                string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd].Replace("\t", "");
                textBody = textBody.Replace(".", ReadRandomLineOfFile("specialSymbol.txt") + ".");
                //string phone = "Cell no: 706 801 7213";
                //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

                Numrd = rd.Next(6, 20);
                string subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                string pre = "", post = "", titlejunk = "";
                for (int i = 0; i < rd.Next(20); i++)
                {
                    pre += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                for (int i = 0; i < rd.Next(20); i++)
                {
                    post += Path.GetRandomFileName().Replace(".", "") + " ";
                }

                textBody = textBody.Replace("xxx", MyThread10.getphoneNum());
                textBody = pre + "\n" + textBody + "\n" + post;
                driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);
                //driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);

                Numrd = rd.Next(1, 4);
                subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                for (int i = 0; i < rd.Next(2, 3); i++)
                {
                    if (!titlejunk.Contains(ReadRandomLineOfFile("adsjunk.txt")))
                    {
                        titlejunk += ReadRandomLineOfFile("adsjunk.txt") + " ";
                    }
                }
                //driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
                title = ReadRandomLineOfFile("specialSymbol.txt") + title + " " + Path.GetRandomFileName().Replace(".", "");
                driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
                //driver.FindElement(By.Id("PostingTitle")).Clear();
                //cmd = System.Diagnostics.Process.Start("postingtitle.exe");
                //cmd.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {

                driver.FindElement(By.Id("wantamap")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        publish:
            try
            {

                driver.FindElement(By.Id("contact_name")).Submit();
                driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //publish
                driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            codebody = "";
            try
            {
                codebody = driver.FindElement(By.TagName("body")).Text;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
                goto publish;
            }
            while (!codebody.Contains("Thanks for posting with us. We really appreciate it!"))
            {
                ResetProxySockEntireComputer();
                int ok = 0;
            //lay mail confirm ve may
            getmail:
                int stt = MyThread10.getMailConfirm(pvas);

                //load link confirm len ff
                var links = File.ReadAllLines("socks.txt");
                try
                {
                    links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                    goto getmail;
                }
                foreach (string link in links)
                    if (codebody.Contains(link.Split('\t')[0]) && driver.Url.Contains(link.Split('\t')[1].Split('/')[4]))
                    {
                        driver.Navigate().GoToUrl(link.Split('\t')[1]);
                        ok = 1;
                        break;
                    }
                if (ok == 1)
                {
                    break;
                }
            }

            //click link confirm & accept ToS        
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {
                Demo w = new Demo();
                w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam() + "\t" + pvas.Split('\t')[0] + "\t" + proxy, "adsLogLink.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            driver.Quit();
            ResetProxySockEntireComputer();
        }

        public void Thread7()
        {
            Thread thr = Thread.CurrentThread;
            string pvas = thr.Name;

        Restart:
            IWebDriver driver = null;
            string proxy = "";


            //InternetExplorerOptions options = new InternetExplorerOptions();

            //options.ForceCreateProcessApi = true;
            //options.BrowserCommandLineArguments = "-private";
            //driver = new InternetExplorerDriver(options);

            ChromeOptions options = new ChromeOptions();
            if (int.Parse(pvas.Split('\t')[6]) != 77)
            {
                proxy = ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt");
                options.AddArguments("--proxy-server=socks5://" + proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);
            }


            driver = new ChromeDriver(@"C:\", options);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            try
            {
                //                  driver.Navigate().GoToUrl("http://craigslist.org");

                //driver.FindElement(By.LinkText("post to classifieds")).Click();
                driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
                if (driver.Url.Contains(".cn"))
                {
                    driver.Navigate().GoToUrl(driver.Url + "?lang=en&cc=us");
                }
                driver.FindElement(By.LinkText("post to classifieds")).Click();
                Random rd = new Random();
                string code = "";
                while (code == "")
                {
                    try
                    {
                        code = driver.FindElement(By.TagName("body")).Text;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
                while (!driver.FindElement(By.TagName("body")).Text.Contains("please choose a category"))
                {
                    driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[10]/input")).Click();
                    System.Threading.Thread.Sleep(5000);
                }
                //System.Threading.Thread.Sleep(5000);
                driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[12]/input")).Click();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            int pass = 0;
            while (true)
            {
                if (pass == 1) break;
                try
                {
                    while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
                    {
                        if (driver.FindElement(By.TagName("body")).Text.Contains("post to classifieds"))
                        {
                            driver.Quit();
                            goto Restart;
                        }

                        try
                        {
                            driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[1]/input")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                        try
                        {
                            driver.FindElement(By.XPath("/html/body/article/section/form/table/tbody/tr/td[1]/blockquote/label[1]")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                        try
                        {
                            string email = pvas.Split('\t')[0];
                            if (!email.Contains("@")) email += "@gmail.com";
                            driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                            driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                            driver.FindElement(By.Id("inputPassword")).Submit();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                    pass = 1;
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            try
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                try
                {
                    driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                    driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //accept tos
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(10000, 99999);
                //input adds content
                //string zip = GetZipcodeOfProxyByHttpRequest();
                //if (zip.Contains("N/A"))
                //{
                //    zip = "59000";
                //}
                driver.FindElement(By.Id("postal_code")).SendKeys("00000");
                //driver.FindElement(By.Id("postal_code")).SendKeys("A0A");
                //driver.FindElement(By.Id("postal_code")).SendKeys(Numrd + "");
                //Numrd = rd.Next(100000000, 999999999);
                //Numrd = rd.Next(100, 999);
                //driver.FindElement(By.Id("contact_phone_extension")).SendKeys("" + Numrd);
                //driver.FindElement(By.Id("GeographicArea")).SendKeys(Path.GetRandomFileName().Replace(".", ""));
                //driver.FindElement(By.Id("contact_name")).SendKeys(Path.GetRandomFileName().Replace(".", ""));

                //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
                //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                //string textBody = "<pre>                                                                                                                                                                                                              ";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
                //textBody += "</pre>";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));


                Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

                //GetRandomAds(driver1);
                string title = ReadRandomLineOfFile("adstitle.txt").Replace("\t", "");
                //title = RandomUppercaseCharacter(title, 10);
                //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
                Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
                string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd].Replace("\t", "");
                textBody = textBody.Replace(".", ReadRandomLineOfFile("specialSymbol.txt") + ".");
                //string phone = "Cell no: 706 801 7213";
                //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

                Numrd = rd.Next(6, 20);
                string subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                string pre = "", post = "", titlejunk = "";
                for (int i = 0; i < rd.Next(20); i++)
                {
                    pre += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                for (int i = 0; i < rd.Next(20); i++)
                {
                    post += Path.GetRandomFileName().Replace(".", "") + " ";
                }

                textBody = textBody.Replace("xxx", MyThread10.getphoneNum());
                textBody = pre + "\n" + textBody + "\n" + post;
                driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);
                //driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);

                Numrd = rd.Next(1, 4);
                subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                for (int i = 0; i < rd.Next(2, 3); i++)
                {
                    if (!titlejunk.Contains(ReadRandomLineOfFile("adsjunk.txt")))
                    {
                        titlejunk += ReadRandomLineOfFile("adsjunk.txt") + " ";
                    }
                }
                //driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
                title = ReadRandomLineOfFile("specialSymbol.txt") + title + " " + Path.GetRandomFileName().Replace(".", "");
                driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
                //driver.FindElement(By.Id("PostingTitle")).Clear();
                //cmd = System.Diagnostics.Process.Start("postingtitle.exe");
                //cmd.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {

                driver.FindElement(By.Id("wantamap")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        publish:
            try
            {

                driver.FindElement(By.Id("contact_name")).Submit();
                driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //publish
                driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            string codebody = "";
            try
            {
                codebody = driver.FindElement(By.TagName("body")).Text;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
                goto publish;
            }

            ResetProxySockEntireComputer();
            while (!codebody.Contains("Thanks for posting with us. We really appreciate it!"))
            {
                int ok = 0;
            //lay mail confirm ve may
            getmail:
                int stt = MyThread10.getMailConfirm(pvas);

                //get link confirm len ff
                var links = File.ReadAllLines("socks.txt");
                try
                {
                    links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                    goto getmail;
                }
                foreach (string link in links)
                    if (codebody.Contains(link.Split('\t')[0]) && driver.Url.Contains(link.Split('\t')[1].Split('/')[4]))
                    {
                        ok = 1;
                        break;
                    }
                if (ok == 1)
                {
                    break;
                }
            }

            if (proxy != "")
            {
                SetSockEntireComputer(proxy);
            }
            //load link confirm len ff
            var alinks = File.ReadAllLines("socks.txt");
            try
            {
                alinks = File.ReadAllLines("mailConfirm" + pvas.Split('\t')[4] + ".txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            foreach (string link in alinks)
                if (codebody.Contains(link.Split('\t')[0]) && driver.Url.Contains(link.Split('\t')[1].Split('/')[4]))
                {
                    driver.Navigate().GoToUrl(link.Split('\t')[1]);
                    break;
                }

            //click link confirm & accept ToS        
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {
                Demo w = new Demo();
                w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam() + "\t" + pvas.Split('\t')[0] + "\t" + proxy, "adsLogLink.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            driver.Quit();
            ResetProxySockEntireComputer();
        }

        private static void waitOtherThread(string pvas, string filename)
        {
            try
            {
                Demo w = new Demo();
                w.WriteToFileThreadSafe(pvas.Split('\t')[4], filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            //cho thread khac
            int a = -1, b = File.ReadAllLines("pvas.txt").Count(); ;
            while (a != b)
            {
                a = File.ReadAllLines(filename).Count();
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static string getphoneNum()
        {
            Random rd = new Random();
            string textBody = File.ReadLines("phonenumber.txt").First();
            string textBody1 = "";
            string symbol = ReadRandomLineOfFile("specialSymbol.txt");

            for (int i = 0; i < textBody.Length; i++)
            {
                if (rd.Next(10) > 8)
                {
                    textBody1 += File.ReadLines("number.txt").Skip(int.Parse(textBody.ElementAt(i).ToString())).First();
                }
                else
                    textBody1 += textBody.ElementAt(i) + "";
                System.Threading.Thread.Sleep(500);
                textBody1 += "-";
                //textBody1 += " "+symbol+" ";
            }

            symbol = "";
            int loop = rd.Next(5, 10);
            for (int i = 0; i < loop; i++)
            {
                symbol += ReadRandomLineOfFile("specialSymbol.txt");
            }
            textBody1 = "&#9742;+1 " + textBody1 + " ";
            return textBody1;
        }

        public void Thread6()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            string pvas = thr.Name;

            bool systemfake = true;
            System.Diagnostics.Process cmd;
            IWebDriver driver = null;

            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("newprofile");
            driver = new FirefoxDriver(profile);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            try
            {
                driver.Navigate().GoToUrl("https://outlook.live.com/owa/");

                //login
                //driver.FindElement(By.XPath("/html/body/div/form/div/div/section/div/div[1]/div/div/div/div[4]/div[1]/div/fieldset/div[2]/div/div[2]/div/div[2]/div/div[2]/div")).SendKeys(pvas.Split('\t')[0]);
                //driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div[2]/div[6]/div[1]/form/div[1]/div[6]/div/input")).SendKeys(pvas.Split('\t')[1]);
                //driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div[2]/div[6]/div[1]/form/div[1]/div[6]/div/input")).Submit();

                //{
                //    System.Threading.Thread.Sleep(15000);
                //}
                driver.Navigate().GoToUrl("https://account.live.com/names/Manage");

                //verify phone (co the xay ra)
                //driver.FindElement(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[3]/label")).Click();
                //driver.FindElement(By.Id("idSubmit_SAOTCS_SendCode")).Click();
                //end verify phone

                //driver.FindElement(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[3]/label")).Click();
                //driver.FindElement(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[3]/div/div[1]/div[3]/div/input")).SendKeys("3101");
                //driver.FindElement(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[3]/div/div[1]/div[3]/div/input")).Submit();

                //add alias

                Console.WriteLine("Cookie:");
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("idAddAliasLink")));
                        driver.FindElement(By.LinkText("Add email")).Click();
                        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("SubmitYes")));
                        string mail = ReadRandomLineOfFile("usaname.txt");
                        System.Threading.Thread.Sleep(1000);
                        mail += ReadRandomLineOfFile("usaname.txt") + Path.GetRandomFileName().Replace(".", "");
                        driver.FindElement(By.Id("AssociatedIdLive")).SendKeys(mail);
                        cmd = System.Diagnostics.Process.Start("changeMailDomain.exe");
                        cmd.WaitForExit();
                        driver.FindElement(By.Id("AssociatedIdLive")).Submit();
                        int stt = int.Parse(pvas.Split('\t')[4]);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
                        {
                            file.WriteLine(mail + "@outlook.com");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //forward mail
                driver.Navigate().GoToUrl("https://outlook.live.com/owa/?path=/options/forwarding");
                Console.WriteLine("Cookie:");

                //driver.Quit();
                //driver = new FirefoxDriver(profile);

                //driver.Navigate().GoToUrl("https://outlook.live.com/owa/");

                ////login
                //driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div[2]/div[6]/div[1]/form/div[1]/div[4]/div/input")).SendKeys(pvas.Split('\t')[0]);
                //driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div[2]/div[6]/div[1]/form/div[1]/div[6]/div/input")).SendKeys(pvas.Split('\t')[1]);
                //driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div[2]/div[6]/div[1]/form/div[1]/div[6]/div/input")).Submit();

                //{
                //    System.Threading.Thread.Sleep(15000);
                //}
                //driver.Navigate().GoToUrl("https://outlook.live.com/owa/?path=/options/forwarding");
                while (!driver.FindElement(By.TagName("body")).Text.Contains("Save"))
                {
                    System.Threading.Thread.Sleep(2000);
                }
                driver.FindElement(By.XPath("/html/body/div[2]/div/div[3]/div[4]/div/div/div/div[3]/div/div/div[3]/div[2]/div/div/label[1]/div/span[2]")).Click();
                driver.FindElement(By.XPath("/html/body/div[2]/div/div[3]/div[4]/div/div/div/div[3]/div/div/div[3]/div[2]/div/div/div/input")).SendKeys("tamtho123@outlook.com");
                driver.FindElement(By.XPath("/html/body/div[2]/div/div[3]/div[4]/div/div/div/div[3]/div/div/div[3]/div[2]/div/div/div/button")).Click();
                driver.FindElement(By.XPath("/html/body/div[2]/div/div[3]/div[4]/div/div/div/div[3]/div/div/div[5]/div/div[2]/div/button[1]")).Click();
                Console.WriteLine("Cookie:");

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            System.Threading.Thread.Sleep(3000);
            driver.Quit();
        }

        public void Thread4()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            string pvas = thr.Name;

            bool systemfake = true;
            System.Diagnostics.Process cmd;
            IWebDriver driver = null;

            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("default");

            //profile.SetPreference("network.proxy.type", 1);
            //profile.SetPreference("network.proxy.socks", "127.0.0.1");
            //profile.SetPreference("network.proxy.socks_port", int.Parse(pvas.Split('\t')[3]));
            //profile.SetPreference("network.proxy.http", "");
            //profile.SetPreference("network.proxy.http_port", "");

            //ChangeSockFirefox(profile, ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt"));
            //driver = new FirefoxDriver(profile);
            driver = new InternetExplorerDriver(@"C:\");
            //driver = new FirefoxDriver();


            bool us = true;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));



            try
            {
                //                  driver.Navigate().GoToUrl("http://craigslist.org");

                //driver.FindElement(By.LinkText("post to classifieds")).Click();
                driver.Navigate().GoToUrl(pvas.Split('\t')[2]);
                driver.FindElement(By.LinkText("post")).Click();
                int Numrd;
                Random rd = new Random();
                driver.FindElement(By.PartialLinkText("service")).Click();
                Numrd = rd.Next(1, 18);
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
                //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[12]")).Click();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/table/tbody/tr/td[1]/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                try
                {
                    string email = pvas.Split('\t')[0];
                    if (!email.Contains("@")) email += "@gmail.com";
                    driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                    driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                    driver.FindElement(By.Id("inputPassword")).Submit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            try
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                try
                {
                    driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                    driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //accept tos
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

                //input adds content
                //string zip = GetZipcodeOfProxyByHttpRequest();
                //if (zip.Contains("N/A"))
                //{
                //    zip = "59000";
                //}
                driver.FindElement(By.Id("postal_code")).SendKeys("00000");
                //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
                //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                //string textBody = "<pre>                                                                                                                                                                                                              ";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
                //textBody += "</pre>";
                //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

                //GetRandomAds(driver1);
                string title = File.ReadAllText("adstitle.txt").Split('|')[Numrd];
                //title = RandomUppercaseCharacter(title, 10);
                //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
                Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
                string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];
                textBody = textBody.Replace(".", ReadRandomLineOfFile("specialSymbol.txt") + ".");
                //string phone = "Cell no: 706 801 7213";
                //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

                Numrd = rd.Next(6, 20);
                string subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }

                string pre = "", post = "";
                for (int i = 0; i < rd.Next(20); i++)
                {
                    pre += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                for (int i = 0; i < rd.Next(20); i++)
                {
                    post += Path.GetRandomFileName().Replace(".", "") + " ";
                }
                driver.FindElement(By.Id("PostingBody")).SendKeys(pre + "\n" + textBody + "\n" + post);

                Numrd = rd.Next(1, 4);
                subtring = "";
                for (int i = 0; i < Numrd; i++)
                {
                    subtring += ReadRandomLineOfFile("specialSymbol.txt");
                }
                driver.FindElement(By.Id("PostingTitle")).SendKeys(ReadRandomLineOfFile("specialSymbol.txt") + title + " " + Path.GetRandomFileName().Replace(".", ""));
                //driver.FindElement(By.Id("PostingTitle")).Clear();
                //cmd = System.Diagnostics.Process.Start("postingtitle.exe");
                //cmd.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            try
            {
                driver.FindElement(By.Id("wantamap")).Click();
                driver.FindElement(By.Id("contact_name")).Submit();
                driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
                //publish
                driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            string codebody = driver.FindElement(By.TagName("body")).Text;
            while (!codebody.Contains("Thanks for posting with us. We really appreciate it!"))
            {
                int ok = 0;
                //lay mail confirm ve may
                int stt = getMailConfirm(pvas);


                //load link confirm len ff
                var links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                foreach (string link in links)
                    if (codebody.Contains(link.Split('\t')[0]))
                    {
                        driver.Navigate().GoToUrl(link.Split('\t')[1]);
                        ok = 1;
                        break;
                    }
                if (ok == 1)
                {
                    break;
                }
            }

            //click link confirm & accept ToS        
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            try
            {
                Demo w = new Demo();
                w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam(), "adsLogLink.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            driver.Quit();
        }

        //change pass pva
        public void Thread3()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            string pvas = thr.Name;

            bool systemfake = true;
            System.Diagnostics.Process cmd;
            IWebDriver driver = null;

            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile("default");

            //profile.SetPreference("network.proxy.type", 1);
            //profile.SetPreference("network.proxy.socks", "127.0.0.1");
            //profile.SetPreference("network.proxy.socks_port", int.Parse(pvas.Split('\t')[3]));
            //profile.SetPreference("network.proxy.http", "");
            //profile.SetPreference("network.proxy.http_port", "");

            ChangeSockFirefox(profile, ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt"));
            driver = new FirefoxDriver(profile);
            //driver = new FirefoxDriver();


            bool us = true;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));



            try
            {

                driver.Navigate().GoToUrl(pvas.Split('\t')[2]);
                driver.FindElement(By.LinkText("post")).Click();
                int Numrd;
                Random rd = new Random();
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[10]")).Click();
                Numrd = rd.Next(1, 18);
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
                //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[12]")).Click();
            }
            catch (Exception e)
            {
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
                //cmd.WaitForExit();
                //return;
                Console.WriteLine("{0} Second exception caught.", e);
            }

            while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
            {
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                try
                {
                    driver.FindElement(By.XPath("/html/body/article/section/form/table/tbody/tr/td[1]/blockquote/label[1]")).Click();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
                try
                {
                    string email = pvas.Split('\t')[0];
                    if (!email.Contains("@")) email += "@gmail.com";
                    driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                    driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                    driver.FindElement(By.Id("inputPassword")).Submit();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            try
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                try
                {
                    driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                    driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            try
            {
                driver.FindElement(By.PartialLinkText("Forgot password")).Click();
                driver.FindElement(By.Name("emailAddressHandle")).SendKeys(pvas.Split('\t')[0]);
                driver.FindElement(By.Name("emailAddressHandle")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            {
                //lay mail reset confirm ve may
                int stt = getMailConfirm(pvas, 1);


                //load link confirm len ff
                var links = File.ReadAllLines("mailConfirm" + stt + ".txt");
                foreach (string link in links)
                    if (pvas.Split('\t')[0] == link.Split('\t')[0])
                    {
                        driver.Navigate().GoToUrl(link.Split('\t')[1]);
                        break;
                    }
            }

            //click link confirm & accept ToS        
            try
            {
                string pass = pvas.Split('\t')[1];
                driver.FindElement(By.Id("p1")).SendKeys(pass);
                driver.FindElement(By.Id("p2")).SendKeys(pass);
                driver.FindElement(By.Id("p2")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            driver.Quit();
        }

        public static void getMailOutlook(int stt, int num, int reset)
        {
            // Create a folder named "inbox" under current directory
            // to save the email retrieved.
            string curpath = Directory.GetCurrentDirectory();
            string mailbox = String.Format("{0}\\inbox" + stt, curpath);

            File.Delete("mailConfirm" + stt + ".txt");
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(curpath + "\\inbox" + stt + "\\");

            try
            {
                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }


            // If the folder is not existed, create it.
            if (!Directory.Exists(mailbox))
            {
                Directory.CreateDirectory(mailbox);
            }

            MailServer oServer = new MailServer("pop-mail.outlook.com",
                      "tamtho123@outlook.com", "idew!jjIHH1", ServerProtocol.Pop3);
            MailClient oClient = new MailClient("TryIt");

            // If your POP3 server requires SSL connection,
            // Please add the following codes:
            oServer.SSLConnection = true;
            oServer.Port = 995;

            try
            {
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();
                for (int i = infos.Length - 1; i > infos.Length - (num + 1) * 2; i--)
                {
                    MailInfo info = infos[i];
                    //Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}",
                    //    info.Index, info.Size, info.UIDL);

                    // Receive email from POP3 server
                    Mail oMail = oClient.GetMail(info);

                    //Console.WriteLine("From: {0}", oMail.From.ToString());
                    //Console.WriteLine("Subject: {0}\r\n", oMail.Subject);

                    // Generate an email file name based on date time.
                    System.DateTime d = System.DateTime.Now;
                    System.Globalization.CultureInfo cur = new
                        System.Globalization.CultureInfo("en-US");
                    string sdate = d.ToString("yyyyMMddHHmmss", cur);
                    string fileName = String.Format("{0}\\{1}{2}{3}.eml",
                        mailbox, sdate, d.Millisecond.ToString("d3"), i);

                    // Save email to local disk
                    oMail.SaveAs(fileName, true);

                    // Mark email as deleted from POP3 server.
                    //oClient.Delete(info);
                }

                // Quit and pure emails marked as deleted from POP3 server.
                oClient.Quit();
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep.Message);
            }

            ParseEmailMain(stt, reset);

        }

        public static void getMailGoogle(int stt, int num, int reset)
        {
            // Create a folder named "inbox" under current directory
            // to save the email retrieved.
            string curpath = Directory.GetCurrentDirectory();
            string mailbox = String.Format("{0}\\inbox" + stt, curpath);

            File.Delete("mailConfirm" + stt + ".txt");
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(curpath + "\\inbox" + stt + "\\");

            try
            {
                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }


            // If the folder is not existed, create it.
            if (!Directory.Exists(mailbox))
            {
                Directory.CreateDirectory(mailbox);
            }

            // Gmail IMAP4 server is "imap.gmail.com"
            MailServer oServer = new MailServer("imap.gmail.com",
                        "hodinhlam911@gmail.com", "Dangtinthue12345678", ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            // Set SSL connection,
            oServer.SSLConnection = true;

            // Set 993 IMAP4 port
            oServer.Port = 993;

            try
            {
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();
                for (int i = infos.Length - 1; i > infos.Length - (num + 1) * 2; i--)
                {
                    MailInfo info = infos[i];
                    //Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}",
                    //    info.Index, info.Size, info.UIDL);

                    // Download email from GMail IMAP4 server
                    Mail oMail = oClient.GetMail(info);

                    //Console.WriteLine("From: {0}", oMail.From.ToString());
                    //Console.WriteLine("Subject: {0}\r\n", oMail.Subject);

                    // Generate an email file name based on date time.
                    System.DateTime d = System.DateTime.Now;
                    System.Globalization.CultureInfo cur = new
                        System.Globalization.CultureInfo("en-US");
                    string sdate = d.ToString("yyyyMMddHHmmss", cur);
                    string fileName = String.Format("{0}\\{1}{2}{3}.eml",
                        mailbox, sdate, d.Millisecond.ToString("d3"), i);

                    // Save email to local disk
                    oMail.SaveAs(fileName, true);

                    // Mark email as deleted in GMail account.
                    //oClient.Delete(info);
                }

                // Quit and pure emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
            }
            catch (Exception ep)
            {
                Console.WriteLine(ep.Message);
            }

            ParseEmailMain(stt, reset);

        }

        public static void ParseEmail(string emlFile, int stt, int reset)
        {
            Mail oMail = new Mail("TryIt");
            oMail.Load(emlFile, false);

            // Parse Mail From, Sender
            //Console.WriteLine("From: {0}", oMail.From.ToString());

            // Parse Mail To, Recipient
            MailAddress[] addrs = oMail.To;
            //for (int i = 0; i < addrs.Length; i++)
            //{
            //    Console.WriteLine("To: {0}", addrs[i].ToString());
            //}

            // Parse Mail CC
            addrs = oMail.Cc;
            //for (int i = 0; i < addrs.Length; i++)
            //{
            //    Console.WriteLine("To: {0}", addrs[i].ToString());
            //}

            //// Parse Mail Subject
            //Console.WriteLine("Subject: {0}", oMail.Subject);

            //// Parse Mail Text/Plain body
            //Console.WriteLine("TextBody: {0}", oMail.TextBody);

            //// Parse Mail Html Body
            //Console.WriteLine("HtmlBody: {0}", oMail.HtmlBody);

            // Parse Attachments
            Attachment[] atts = oMail.Attachments;
            //for (int i = 0; i < atts.Length; i++)
            //{
            //    Console.WriteLine("Attachment: {0}", atts[i].Name);
            //}
            addrs = oMail.To;
            if (reset == 0)
            {
                if (oMail.Subject.Contains("POST/EDIT/DELETE"))
                {
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
                        {


                            for (int i = 0; i < addrs.Length; i++)
                            {

                                string[] text = oMail.TextBody.Split('/');
                                string link = "https://post.craigslist.org/u/" + text[4] + "/" + text[5].Split('\r')[0];
                                file.WriteLine(addrs[i].ToString().Split('<')[1].Split('>')[0] + "\t" + link);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
            }
            else if (reset == 2)
            {
                if (oMail.Subject.Contains("Gmail Forwarding Confirmation"))
                {
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
                        {
                            for (int i = 0; i < addrs.Length; i++)
                            {
                                file.WriteLine(oMail.TextBody.Split('@')[0] + "\t" + oMail.TextBody.Split(':')[1].Substring(1, 9));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
            }
            else
            {
                if (oMail.Subject.Contains("Request to Reset Account Password for"))
                {
                    if (oMail.TextBody.Contains("lang=en&cc=us"))
                    {
                        try
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
                            {


                                for (int i = 0; i < addrs.Length; i++)
                                {

                                    string[] text = oMail.TextBody.Split('=');
                                    string link = "https://accounts.craigslist.org/pass?lang=en&cc=us&ui=" + text[3].Split('&')[0] + "&ip=" + text[4].Split('\r')[0];
                                    file.WriteLine(addrs[i].ToString().Split('<')[1].Split('>')[0] + "\t" + link);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                    else if (!oMail.TextBody.Contains("lang=en&cc=us"))
                    {
                        try
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
                            {


                                for (int i = 0; i < addrs.Length; i++)
                                {

                                    string[] text = oMail.TextBody.Split('=');
                                    string link = "https://accounts.craigslist.org/pass?ui=" + text[1].Split('&')[0] + "&ip=" + text[2].Split('\r')[0];
                                    file.WriteLine(addrs[i].ToString().Split('<')[1].Split('>')[0] + "\t" + link);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                }
                else if (oMail.Subject.Contains("New Craigslist Account"))
                {
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
                        {


                            for (int i = 0; i < addrs.Length; i++)
                            {

                                string[] text = oMail.TextBody.Replace("lang=en&cc=us&", "").Split('=');
                                string link = "https://accounts.craigslist.org/pass?ui=" + text[1].Split('&')[0] + "&ip=" + text[2].Split('&')[0] + "&rt=P&rp=" + text[4].Split('\r')[0];
                                file.WriteLine(addrs[i].ToString().Split('<')[1].Split('>')[0] + "\t" + link);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
            }

        }

        public static void ParseEmailMain(int stt, int reset)
        {
            string curpath = Directory.GetCurrentDirectory();
            string mailbox = String.Format("{0}\\inbox" + stt, curpath);

            // If the folder is not existed, create it.
            if (!Directory.Exists(mailbox))
            {
                Directory.CreateDirectory(mailbox);
            }

            // Get all *.eml files in specified folder and parse it one by one.
            string[] files = Directory.GetFiles(mailbox, "*.eml");
            for (int i = 0; i < files.Length; i++)
            {
                ParseEmail(files[i], stt, reset);
            }
        }
    }

    //public class MyThread11
    //{
    //    IWebDriver driver = null;
    //    public void Thread1()
    //    {
    //        Thread thr = Thread.CurrentThread;
    //        string proxy = thr.Name;
    //        string pvas = thr.Name;

    //        bool systemfake = true;
    //        System.Diagnostics.Process cmd;

    //        FirefoxProfileManager profileManager = new FirefoxProfileManager();
    //        FirefoxProfile profile = profileManager.GetProfile("default");

    //        //profile.SetPreference("network.proxy.type", 1);
    //        //profile.SetPreference("network.proxy.socks", "127.0.0.1");
    //        //profile.SetPreference("network.proxy.socks_port", int.Parse(pvas.Split('\t')[3]));
    //        //profile.SetPreference("network.proxy.http", "");
    //        //profile.SetPreference("network.proxy.http_port", "");
    //        ChangeSockFirefox(profile, ReadProxyAtLine(int.Parse(pvas.Split('\t')[3]), "socks.txt"));
    //        driver = new FirefoxDriver(profile);
    //        //driver = new FirefoxDriver();


    //        bool us = true;
    //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));


    //        try
    //        {

    //            driver.Navigate().GoToUrl(pvas.Split('\t')[2]);
    //            driver.FindElement(By.LinkText("post")).Click();
    //            int Numrd;
    //            Random rd = new Random();
    //            Numrd = rd.Next(6, 10);
    //            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[6]")).Click();
    //            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();

    //            //Numrd = rd.Next(1, 40);
    //            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
    //            if (Numrd == 6)
    //            {
    //                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[21]")).Click();
    //            }
    //            else
    //                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[20]")).Click();
    //        }
    //        catch (Exception e)
    //        {
    //            //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
    //            //cmd.WaitForExit();
    //            //return;
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }

    //        while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
    //        {
    //            try
    //            {
    //                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine("{0} Second exception caught.", e);
    //            }

    //            try
    //            {
    //                string email = pvas.Split('\t')[0];
    //                if (!email.Contains("@")) email += "@gmail.com";
    //                driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
    //                driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
    //                driver.FindElement(By.Id("inputPassword")).Submit();
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine("{0} Second exception caught.", e);
    //            }
    //        }

    //        try
    //        {
    //            string email = pvas.Split('\t')[0];
    //            if (!email.Contains("@")) email += "@gmail.com";
    //            try
    //            {
    //                driver.FindElement(By.Id("FromEMail")).SendKeys(email);
    //                driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine("{0} Second exception caught.", e);
    //            }

    //            //input adds content
    //            //string zip = GetZipcodeOfProxyByHttpRequest();
    //            //if (zip.Contains("N/A"))
    //            //{
    //            //    zip = "59000";
    //            //}
    //            driver.FindElement(By.Id("postal_code")).SendKeys("A0A");
    //            //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
    //            //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

    //            //string textBody = "<pre>                                                                                                                                                                                                              ";
    //            //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
    //            //textBody += "</pre>";
    //            //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

    //            int Numrd;
    //            Random rd = new Random();
    //            Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

    //            //GetRandomAds(driver1);
    //            string title = File.ReadAllText("adstitle.txt").Split('|')[Numrd];
    //            title = RandomUppercaseCharacter(title, 0);
    //            //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
    //            Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
    //            string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];
    //            textBody = RandomUppercaseCharacter(textBody, 0);
    //            //string phone = "Cell no: 706 801 7213";
    //            //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

    //            Numrd = rd.Next(6, 20);
    //            string subtring = "";
    //            for (int i = 0; i < Numrd; i++)
    //            {
    //                subtring += ReadRandomLineOfFile("specialSymbol.txt");
    //            }
    //            driver.FindElement(By.Id("PostingBody")).SendKeys(subtring + textBody + "\n" + subtring);
    //            //driver.FindElement(By.Id("PostingTitle")).Clear();
    //            //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("postingtitle.exe");
    //            //cmd.WaitForExit();
    //            Numrd = rd.Next(1, 4);
    //            subtring = "";
    //            for (int i = 0; i < Numrd; i++)
    //            {
    //                subtring += ReadRandomLineOfFile("specialSymbol.txt");
    //            }
    //            driver.FindElement(By.Id("PostingTitle")).SendKeys(ReadRandomLineOfFile("specialSymbol.txt") + title + subtring);
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }

    //        try
    //        {
    //            driver.FindElement(By.Id("wantamap")).Click();
    //            driver.FindElement(By.Id("contact_name")).Submit();

    //            //upload imageg
    //            driver.FindElement(By.Id("plupload")).Click();

    //            string link = @"C:\imageupload\";
    //            var files = new DirectoryInfo(link).GetFiles();
    //            int index1 = new Random().Next(0, files.Length);

    //            using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
    //            {
    //                file.Write(link + files[index1].Name);
    //            }
    //            System.Diagnostics.Process cmd1;
    //            cmd1 = System.Diagnostics.Process.Start("uploadImage.exe");
    //            cmd1.WaitForExit();
    //            System.Threading.Thread.Sleep(60000);

    //            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
    //            ////publish
    //            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }
    //        //skip map
    //        try
    //        {
    //            driver.FindElement(By.ClassName("skipmap")).Submit();
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }


    //        //lay mail confirm ve may
    //        System.Threading.Thread.Sleep(120000);
    //        int stt = int.Parse(pvas.Split('\t')[3]);
    //        getMail(stt);


    //        //load link confirm len ff
    //        var links = File.ReadAllLines("mailConfirm" + stt + ".txt");
    //        foreach (string link in links)
    //            if (driver.FindElement(By.TagName("body")).Text.Contains(link.Split('\t')[0]))
    //            {
    //                try
    //                {
    //                    driver.Navigate().GoToUrl(link.Split('\t')[1]);
    //                    break;
    //                }
    //                catch (Exception e)
    //                {
    //                    Console.WriteLine("{0} Second exception caught.", e);
    //                }
    //            }

    //        //click link confirm & accept ToS        
    //        try
    //        {
    //            driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }
    //        try
    //        {
    //            Demo w = new Demo();
    //            w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam(), "adsLogLink.txt");
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }
    //    }

    //    public static void getMail(int stt)
    //    {
    //        // Create a folder named "inbox" under current directory
    //        // to save the email retrieved.
    //        string curpath = Directory.GetCurrentDirectory();
    //        string mailbox = String.Format("{0}\\inbox" + stt, curpath);

    //        File.Delete("mailConfirm" + stt + ".txt");
    //        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(curpath + "\\inbox" + stt + "\\");

    //        try
    //        {
    //            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
    //            {
    //                file.Delete();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }


    //        // If the folder is not existed, create it.
    //        if (!Directory.Exists(mailbox))
    //        {
    //            Directory.CreateDirectory(mailbox);
    //        }

    //        MailServer oServer = new MailServer("pop-mail.outlook.com",
    //                  "tamtho123@outlook.com", "idew!jjIHH1", ServerProtocol.Pop3);
    //        MailClient oClient = new MailClient("TryIt");

    //        // If your POP3 server requires SSL connection,
    //        // Please add the following codes:
    //        oServer.SSLConnection = true;
    //        oServer.Port = 995;

    //        try
    //        {
    //            oClient.Connect(oServer);
    //            MailInfo[] infos = oClient.GetMailInfos();
    //            for (int i = infos.Length - 1; i > infos.Length - 5; i--)
    //            {
    //                MailInfo info = infos[i];
    //                Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}",
    //                    info.Index, info.Size, info.UIDL);

    //                // Receive email from POP3 server
    //                Mail oMail = oClient.GetMail(info);

    //                Console.WriteLine("From: {0}", oMail.From.ToString());
    //                Console.WriteLine("Subject: {0}\r\n", oMail.Subject);

    //                // Generate an email file name based on date time.
    //                System.DateTime d = System.DateTime.Now;
    //                System.Globalization.CultureInfo cur = new
    //                    System.Globalization.CultureInfo("en-US");
    //                string sdate = d.ToString("yyyyMMddHHmmss", cur);
    //                string fileName = String.Format("{0}\\{1}{2}{3}.eml",
    //                    mailbox, sdate, d.Millisecond.ToString("d3"), i);

    //                // Save email to local disk
    //                oMail.SaveAs(fileName, true);

    //                // Mark email as deleted from POP3 server.
    //                //oClient.Delete(info);
    //            }

    //            // Quit and pure emails marked as deleted from POP3 server.
    //            oClient.Quit();
    //        }
    //        catch (Exception ep)
    //        {
    //            Console.WriteLine(ep.Message);
    //        }

    //        ParseEmailMain(stt);

    //    }

    //    public static void ParseEmail(string emlFile, int stt)
    //    {
    //        Mail oMail = new Mail("TryIt");
    //        oMail.Load(emlFile, false);

    //        // Parse Mail From, Sender
    //        Console.WriteLine("From: {0}", oMail.From.ToString());

    //        // Parse Mail To, Recipient
    //        MailAddress[] addrs = oMail.To;
    //        for (int i = 0; i < addrs.Length; i++)
    //        {
    //            Console.WriteLine("To: {0}", addrs[i].ToString());
    //        }

    //        // Parse Mail CC
    //        addrs = oMail.Cc;
    //        for (int i = 0; i < addrs.Length; i++)
    //        {
    //            Console.WriteLine("To: {0}", addrs[i].ToString());
    //        }

    //        // Parse Mail Subject
    //        Console.WriteLine("Subject: {0}", oMail.Subject);

    //        // Parse Mail Text/Plain body
    //        Console.WriteLine("TextBody: {0}", oMail.TextBody);

    //        // Parse Mail Html Body
    //        Console.WriteLine("HtmlBody: {0}", oMail.HtmlBody);

    //        // Parse Attachments
    //        Attachment[] atts = oMail.Attachments;
    //        for (int i = 0; i < atts.Length; i++)
    //        {
    //            Console.WriteLine("Attachment: {0}", atts[i].Name);
    //        }
    //        addrs = oMail.To;
    //        try
    //        {
    //            using (System.IO.StreamWriter file = new System.IO.StreamWriter("mailConfirm" + stt + ".txt", true))
    //            {


    //                for (int i = 0; i < addrs.Length; i++)
    //                {

    //                    string[] text = oMail.TextBody.Split('/');
    //                    string link = "https://post.craigslist.org/u/" + text[4] + "/" + text[5].Split('\r')[0];
    //                    file.WriteLine(addrs[i].ToString().Split('<')[1].Split('>')[0] + "\t" + link);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("{0} Second exception caught.", e);
    //        }

    //    }

    //    public static void ParseEmailMain(int stt)
    //    {
    //        string curpath = Directory.GetCurrentDirectory();
    //        string mailbox = String.Format("{0}\\inbox" + stt, curpath);

    //        // If the folder is not existed, create it.
    //        if (!Directory.Exists(mailbox))
    //        {
    //            Directory.CreateDirectory(mailbox);
    //        }

    //        // Get all *.eml files in specified folder and parse it one by one.
    //        string[] files = Directory.GetFiles(mailbox, "*.eml");
    //        for (int i = 0; i < files.Length; i++)
    //        {
    //            ParseEmail(files[i], stt);
    //        }
    //    }
    //}

    public class MyThread5
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;

            string MyProxyHostString = proxy.Split(':')[0];
            int MyProxyPort = int.Parse(proxy.Split(':')[1]);
            Demo w = new Demo();
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create("http://losangeles.craigslist.org/flag/?async=async&flagCode=28&postingID=5376841491");
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                string filename = thr.Name.Replace(".", " ");
                filename = filename.Replace(":", " ");
                filename = "temp\\output" + filename + ".txt";
                File.WriteAllText(filename, responseFromServer);
                //string info = File.ReadLines(filename).Skip(145).First() + File.ReadLines(filename).Skip(146).First() + File.ReadLines(filename).Skip(147).First();
                //info = info.Remove(0, info.IndexOf("png\">") + 6);
                //info = info.Replace("</p>							<p><em>State:</em> ", "\t");
                //info = info.Replace("</p>							<p><em>City:</em> ", "\t");
                //info = info.Replace("</p>", "");
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
                //{
                //    try
                //    {
                //        file.WriteLine(proxy + "\t" + info);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}   
                //w.WriteToFileThreadSafe(proxy + "\t" + info, "proxyWithCityHttpRequest.txt");
            }
            catch (Exception e)
            {
                //w.WriteToFileThreadSafe(proxy, "proxyWithCityHttpRequest.txt");
                Console.WriteLine("{0} Second exception caught.", e);
            }

            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();
        }
    }

    public class MyThread6
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;

            string MyProxyHostString = proxy.Split(':')[0];
            int MyProxyPort = int.Parse(proxy.Split(':')[1]);
            Demo w = new Demo();
            try
            {
                string link = "http://losangeles.craigslist.org/flag/?async=async&flagCode=28&postingID=5382772242";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 8_4 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Version/8.0 Mobile/12H143 Safari/600.1.4";
                request.CookieContainer = new CookieContainer();

                CookieContainer myContainer = new CookieContainer();
                //for (int i = 0; i < response.Cookies.Count; i++)
                //{
                System.Net.Cookie cookie = new System.Net.Cookie("cl_b", "YjGr9Uyw5RGjNsH1jDDZngH0REE");
                myContainer.Add(new System.Uri(link), cookie);
                //}
                request.CookieContainer = myContainer;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //link = "http://losangeles.craigslist.org/flag/?async=async&flagCode=28&postingID=5382772242";
                //request = (HttpWebRequest)WebRequest.Create(link);
                //response = (HttpWebResponse)request.GetResponse();

                // Print the properties of each cookie.
                //foreach (System.Net.Cookie cook in response.Cookies)
                //{
                //    Console.WriteLine("Cookie:");
                //    Console.WriteLine("{0} = {1}", cook.Name, cook.Value);
                //    Console.WriteLine("Domain: {0}", cook.Domain);
                //    Console.WriteLine("Path: {0}", cook.Path);
                //    Console.WriteLine("Port: {0}", cook.Port);
                //    Console.WriteLine("Secure: {0}", cook.Secure);

                //    Console.WriteLine("When issued: {0}", cook.TimeStamp);
                //    Console.WriteLine("Expires: {0} (expired? {1})",
                //        cook.Expires, cook.Expired);
                //    Console.WriteLine("Don't save: {0}", cook.Discard);
                //    Console.WriteLine("Comment: {0}", cook.Comment);
                //    Console.WriteLine("Uri for comments: {0}", cook.CommentUri);
                //    Console.WriteLine("Version: RFC {0}", cook.Version == 1 ? "2109" : "2965");

                //    // Show the string representation of the cookie.
                //    Console.WriteLine("String: {0}", cook.ToString());


                //}
                //CookieContainer myContainer = new CookieContainer();
                //for (int i = 0; i < response.Cookies.Count; i++)
                //{
                //    System.Net.Cookie cookie = new System.Net.Cookie(response.Cookies[i].Name, response.Cookies[i].Value, response.Cookies[i].Path);
                //    myContainer.Add(new System.Uri(link), cookie);
                //}

                ////later:
                //request = (HttpWebRequest)WebRequest.Create(link);
                //request.CookieContainer = myContainer;
                //response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                //w.WriteToFileThreadSafe(proxy, "proxyWithCityHttpRequest.txt");
                Console.WriteLine("{0} Second exception caught.", e);
            }

            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();
        }
    }

    public class MyThread4
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;

            string MyProxyHostString = proxy.Split(':')[0];
            int MyProxyPort = int.Parse(proxy.Split(':')[1]);
            Demo w = new Demo();
            //try
            //{
            //    Chilkat.Http http = new Chilkat.Http();
            //    http.SocksHostname = MyProxyHostString;
            //    http.SocksPort = MyProxyPort;
            //    //http.SocksUsername = "myProxyLogin";
            //    //http.SocksPassword = "myProxyPassword";
            //    //  Set the SOCKS version to 4 or 5 based on the version
            //    //  of the SOCKS proxy server:
            //    http.SocksVersion = 5;
            //    bool success1;
            //    //  Any string unlocks the component for the 1st 30-days.
            //    success1 = http.UnlockComponent("Start my 30-day Trial");
            //    if (success1 != true)
            //    {
            //        Console.WriteLine(http.LastErrorText);
            //        return;
            //    }
            //    //  Send the HTTP GET and return the content in a string.
            //    string html;
            //    html = http.QuickGetStr("http://www.ip-score.com/");
            //    // Display the content.
            //    //Console.WriteLine(responseFromServer);
            //    string filename = thr.Name.Replace(".", " ");
            //    filename = filename.Replace(":", " ");
            //    filename = "temp\\output" + filename + ".txt";
            //    File.WriteAllText(filename, html);
            //    string info = File.ReadLines(filename).Skip(145).First() + File.ReadLines(filename).Skip(146).First() + File.ReadLines(filename).Skip(147).First();
            //    info = info.Remove(0, info.IndexOf("png\">") + 6);
            //    info = info.Replace("</p>							<p><em>State:</em> ", "\t");
            //    info = info.Replace("</p>							<p><em>City:</em> ", "\t");
            //    info = info.Replace("</p>", "");
            //    //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
            //    //{
            //    //    try
            //    //    {
            //    //        file.WriteLine(proxy + "\t" + info);
            //    //    }
            //    //    catch (Exception e)
            //    //    {
            //    //        Console.WriteLine("{0} Second exception caught.", e);
            //    //    }
            //    //}   
            //    w.WriteToFileThreadSafe(proxy + "\t" + info, "sockWithCityHttpRequest.txt");
            //}
            //catch (Exception e)
            //{
            //    w.WriteToFileThreadSafe(proxy, "sockWithCityHttpRequest.txt");
            //    Console.WriteLine("{0} Second exception caught.", e);
            //}

            //// Clean up the streams and the response.
            ////reader.Close();
            ////response.Close();
        }
    }

    public class MyThread7
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            if (int.Parse(proxy) < 10)
            {
                proxy = "00" + proxy;
            }
            else if (int.Parse(proxy) < 100)
            {
                proxy = "0" + proxy;
            }

            string pvas = "\t\t" + proxy + "hoc\tlamnguyen";
            FirefoxProfileManager profileManager1 = new FirefoxProfileManager();
            FirefoxProfile profile1 = profileManager1.GetProfile("default");
            IWebDriver driver1 = new ChromeDriver(@"C:\");


            WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(20));
            try
            {
                //vao email lay link confirmation
                driver1.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?service=mail&continue=https://mail.google.com/mail/#identifier");
                //driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t').Last());
                driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t')[2]);
                driver1.FindElement(By.Id("Email")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            try
            {
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("Passwd")));
                if (pvas.Split('\t').Last().Contains("hodinhlam911"))
                {
                    driver1.FindElement(By.Id("Passwd")).SendKeys("Dangtinthue1234567");
                }
                else
                {
                    driver1.FindElement(By.Id("Passwd")).SendKeys(pvas.Split('\t')[3]);
                }
                driver1.FindElement(By.Id("Passwd")).Submit();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            //click done recovery
            System.Threading.Thread.Sleep(5000);
            try
            {
                driver1.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div/div/div/div[1]/div[3]/div[1]")).Click();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            System.Threading.Thread.Sleep(20000);
            TakeScreenshot(driver1, @"C:\devtest\screenshot" + proxy + ".png");

            driver1.Quit();
        }
        public void TakeScreenshot(IWebDriver driver, string saveLocation)
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(saveLocation, System.Drawing.Imaging.ImageFormat.Png);
        }
    }

    public static string checkanno(string html)
    {
        string info = "";
        if (html.Contains("high-anonymous (elite)"))
        {
            info = "high";
        }
        else if (html.Contains("Your proxy is not anonymous"))
        {
            info = "low";
        }
        else
            info = "medium";
        return info;
    }

    public class MyThread8
    {
        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;

            string MyProxyHostString = proxy.Split(':')[0];
            int MyProxyPort = int.Parse(proxy.Split(':')[1]);
            Demo w = new Demo();
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create("http://ip.cc/anonymity-test.php");
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                string filename = thr.Name.Replace(".", " ");
                filename = filename.Replace(":", " ");
                filename = "temp\\output" + filename + ".txt";
                File.WriteAllText(filename, responseFromServer);
                string info = checkanno(responseFromServer);
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
                //{
                //    try
                //    {
                //        file.WriteLine(proxy + "\t" + info);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}   
                w.WriteToFileThreadSafe(proxy + "\t" + info, "proxyWithCityHttpRequest.txt");
            }
            catch (Exception e)
            {
                w.WriteToFileThreadSafe(proxy, "proxyWithCityHttpRequest.txt");
                Console.WriteLine("{0} Second exception caught.", e);
            }

            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();
        }
    }

    public class MyThread3
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;

            string MyProxyHostString = proxy.Split(':')[0];
            int MyProxyPort = int.Parse(proxy.Split(':')[1]);
            Demo w = new Demo();
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create("http://losangeles.craigslist.org/sgv/lss/5392939400.html");
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
                // If required by the server, set the credentials.
                //request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                request.Headers["Host"] = "losangeles.craigslist.org";
                request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                request.Headers["Accept"] = "*/*";
                request.Headers["Accept-Language"] = "en-US,en;q=0.5";
                request.Headers["Accept-Encoding"] = "gzip, deflate";
                request.Headers["X-Requested-With"] = "XMLHttpRequest";
                request.Headers["Referer"] = "http://losangeles.craigslist.org/sgv/lss/5392939400.html";
                request.Headers["Cookie"] = "cl_b=eBJN3Me15RG2TkEGlSKHxwKx19I";
                request.Headers["Connection"] = "keep-alive";
                request.Headers["If-Modified-Since"] = "Fri, 08 Jan 2016 05:22:50 GMT";
                WebResponse response = request.GetResponse();
                // Display the status.
                //request = WebRequest.Create("http://www.craigslist.org/styles/cl.css?v=e345170d7ac6289e7d2888f3158792e9");
                //response = request.GetResponse();
                //request = WebRequest.Create("http://www.craigslist.org/styles/leaflet.css?v=52966d0fad2afe8dd918e5f48abb0db3");
                //response = request.GetResponse();
                //request = WebRequest.Create("http://www.craigslist.org/js/general-concat.min.js?v=a8cd0d261becd1bbef20c8a065fa9e0c");
                //response = request.GetResponse();
                //request = WebRequest.Create("http://www.craigslist.org/js/leaflet-concat.min.js?v=b4b2f425fe7d41e79ecfda3198df6ca3");
                //response = request.GetResponse();
                //request = WebRequest.Create("http://www.craigslist.org/js/postings-concat.min.js?v=30d4105eda563e8ed73607386b2dc846");
                //response = request.GetResponse();
                //request = WebRequest.Create("http://www.craigslist.org/static/localstorage.html?v=51a29e41f8e978141e4085ed4a77d170");
                //response = request.GetResponse();
                request = WebRequest.Create("http://losangeles.craigslist.org/flag/?async=async&flagCode=28&postingID=5392939400");


                response = request.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

        }
    }

    //proxy.txt => proxyWithCityHttpRequest.txt
    public static void GetCityOfProxyHttpRequest(System.Windows.Forms.Label label3)
    {
        File.Delete("proxyWithCityHttpRequest.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("proxy.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("proxy.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread[] thr = new MyThread[1000];
            Thread[] tid = new Thread[1000];

            //MyThread thr1 = new MyThread();
            //MyThread thr2 = new MyThread();

            //Thread tid1 = new Thread(new ThreadStart(thr1.Thread1));
            //Thread tid2 = new Thread(new ThreadStart(thr2.Thread1));

            //tid1.Start();
            //tid2.Start();
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("proxy.txt").Count() / block)
                num = File.ReadAllLines("proxy.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = ReadProxyAtLine(i + 1, "proxy.txt");
                thr[i] = new MyThread();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    //socks.txt => sockWithCityHttpRequest.txt
    public static void GetCityOfSockHttpRequest(System.Windows.Forms.Label label3)
    {
        File.Delete("sockWithCityHttpRequest.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("socks.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("socks.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread4[] thr = new MyThread4[1000];
            Thread[] tid = new Thread[1000];

            //MyThread thr1 = new MyThread();
            //MyThread thr2 = new MyThread();

            //Thread tid1 = new Thread(new ThreadStart(thr1.Thread1));
            //Thread tid2 = new Thread(new ThreadStart(thr2.Thread1));

            //tid1.Start();
            //tid2.Start();
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("socks.txt").Count() / block)
                num = File.ReadAllLines("socks.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = ReadProxyAtLine(i + 1, "socks.txt");
                thr[i] = new MyThread4();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }

        //copy socks live to socks.txt
        File.Delete("socks.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("socks.txt"))
        {
            var titles = File.ReadLines("sockWithCityHttpRequest.txt");
            foreach (string link in titles)
            {
                if (link.Split('\t').Count() > 1)
                {
                    file.WriteLine(link.Split('\t')[0]);
                }
            }
        }

        File.Delete("temp.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            var titles = File.ReadLines("sockWithCityHttpRequest.txt");
            foreach (string link in titles)
            {
                if (link.Split('\t').Count() > 1)
                {
                    file.WriteLine(link);
                }
            }
        }

        File.Delete("sockWithCityHttpRequest.txt");
        File.Copy("temp.txt", "sockWithCityHttpRequest.txt");
    }

    //socks.txt => sockWithCityHttpRequest.txt
    public static void checkAnonymousSock(System.Windows.Forms.Label label3)
    {
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("C:\\devtest\\");

        try
        {
            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        int numline = 1000;
        File.Delete("sockWithCityHttpRequest.txt");
        int block = 1;
        for (int j = 0; j <= numline / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + numline / block));
            ResetProxySockEntireComputer();

            MyThread7[] thr = new MyThread7[1000];
            Thread[] tid = new Thread[1000];

            int num;
            if (j == numline / block)
                num = numline;
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {

                thr[i] = new MyThread7();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = i + 1 + "";
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    //proxy.txt => proxyWithCityHttpRequest.txt
    public static void checkAnonymousProxy(System.Windows.Forms.Label label3)
    {
        File.Delete("proxyWithCityHttpRequest.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("proxy.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("proxy.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread8[] thr = new MyThread8[1000];
            Thread[] tid = new Thread[1000];

            //MyThread thr1 = new MyThread();
            //MyThread thr2 = new MyThread();

            //Thread tid1 = new Thread(new ThreadStart(thr1.Thread1));
            //Thread tid2 = new Thread(new ThreadStart(thr2.Thread1));

            //tid1.Start();
            //tid2.Start();
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("proxy.txt").Count() / block)
                num = File.ReadAllLines("proxy.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = ReadProxyAtLine(i + 1, "proxy.txt");
                thr[i] = new MyThread8();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    // => adstitle.txt, adsbody.txt
    public static void GetRandomAds(IWebDriver driver)
    {
        File.Delete("adstitle.txt");
        File.Delete("adsbody.txt");

        if (driver == null)
        {
            driver = new FirefoxDriver();
        }
        while (true)
        {
            driver.Navigate().GoToUrl("https://en.wikipedia.org/wiki/Special:Random");
            var content = driver.FindElement(By.Id("mw-content-text"));
            if (content.Text.Length > 2000)
            {
                File.WriteAllText("temp.txt", content.Text.Substring(500, 60).Replace("\n", " "));
                var titles = File.ReadLines("temp.txt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("adstitle.txt"))
                {
                    foreach (string link in titles)
                    {
                        file.Write(link);
                    }
                }
                File.WriteAllText("adsbody.txt", content.Text.Substring(561, 1100).Replace("http://", ""));
                break;
            }
        }
    }

    // => 
    public static string getzipcode(IWebDriver driver, string city)
    {
        if (driver == null)
        {
            driver = new FirefoxDriver();
        }
        driver.Navigate().GoToUrl("http://www.unitedstateszipcodes.org/");
        driver.FindElement(By.Id("q")).SendKeys(city + ", TX, USA");
        driver.FindElement(By.Id("q")).Submit();
        return driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[4]/div[2]/div/div[2]/div/div[1]/h3")).Text.Split(' ').Last();
    }

    // => 
    public static string getStateOfCity(IWebDriver driver, string city)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        if (driver == null)
        {
            driver = new FirefoxDriver();
        }
        driver.Navigate().GoToUrl("http://www.unitedstateszipcodes.org/");
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("q")));
        driver.FindElement(By.Id("q")).SendKeys(city);
        driver.FindElement(By.Id("q")).Submit();
        return driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[4]/dl/dd")).Text.Split(' ').Last();
    }

    // pvas => temp
    public static void Nearest5SocksCityStep1(IWebDriver driver, System.Windows.Forms.Label label3)
    {
        File.Delete("temp.txt");
        File.Delete("testdemo1.txt");
        File.Delete("testdemo2.txt");
        File.Delete("socks.txt");

        if (driver == null)
        {
            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
            driver = new FirefoxDriver(profile);
        }

        //IWebDriver driver1 = new FirefoxDriver();
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        //get distinct city pvas
        var links = File.ReadLines("pvas.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo1.txt"))
        {
            foreach (string link in links)
            {
                var elements = link.Split('\t');
                foreach (string element in elements)
                {
                    if (element.Contains(","))
                    {
                        file.WriteLine(element);
                        break;
                    }
                }
            }
        }
        string[] lines = File.ReadAllLines("testdemo1.txt");
        File.WriteAllLines("testdemo1.txt", lines.Distinct().ToArray());


        login5socks(driver);
        string record = "";
        //get distinct city 5socks page ung voi moi distinct city pvas
        foreach (var line1 in File.ReadLines("pvas.txt"))
        {
            string linedemo1 = "";
            string state = "";
            var elements = line1.Split('\t');
            foreach (string element in elements)
            {
                if (element.Contains(","))
                {
                    linedemo1 = element.Split(',')[0];
                    state = getStateOfCity(driver, element);
                    break;
                }
            }

            //get distinct city 5socks ung voi state cua city pvas
            driver.Navigate().GoToUrl("http://admin.5socks.net/cgi-bin/online.cgi?order=0&city=&page=1&country=&s_city=&s_country=&s_state=" + state + "&s_host=&s_ip=&search=1");
            File.WriteAllText("testdemo2.txt", driver.FindElement(By.TagName("body")).Text);
            int num = File.ReadLines("testdemo2.txt").Count();
            var temp = File.ReadAllLines("testdemo2.txt");
            File.Delete("testdemo2.txt");

            File.Delete("sockWithCityHttpRequest.txt");
            for (int i = 0; i < num - 5; i++)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("sockWithCityHttpRequest.txt", true))
                {
                    file.WriteLine(temp[i + 4] + "\t" + driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[" + (i + 3) + "]/td[2]/a")).GetAttribute("href"));
                }
            }

            for (int i = 0; i < num - 5; i++)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo2.txt", true))
                {
                    if (driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[" + (i + 3) + "]/td[5]")).Text.Trim() != "")
                    {
                        file.WriteLine(driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[" + (i + 3) + "]/td[5]")).Text);
                    }
                }
            }
            lines = File.ReadAllLines("testdemo2.txt");
            File.WriteAllLines("testdemo2.txt", lines.Distinct().ToArray());

            ////get distance giua cac distinct cua 5socks va mot distinct pvas tuong ung dang xet

            foreach (var line2 in File.ReadLines("testdemo2.txt"))
            {
                bool exist = false;
                foreach (var line3 in File.ReadLines("testdemo3.txt"))
                {
                    if (linedemo1 + "," + state == line3.Split('\t')[0] && line2 + "," + state == line3.Split('\t')[1])
                        exist = true;
                }

                if (!exist)
                {
                    driver.Navigate().GoToUrl("http://www.distance-cities.com/search?from=" + linedemo1 + "%2C+" + state + "%2C+United+States&to=" + line2 + "%2C+" + state + "%2C+United+States&country=");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo3.txt", true))
                    {
                        file.WriteLine(linedemo1 + "," + state + "\t" + line2 + "," + state + "\t" + driver.FindElement(By.Id("kmslinearecta")).Text.Split(' ')[0].Replace('.', ','));
                    }
                }
            }

            driver.Navigate().GoToUrl("http://admin.5socks.net/cgi-bin/online.cgi?order=0&city=&page=1&country=&s_city=&s_country=&s_state=" + state + "&s_host=&s_ip=&search=1");
            ////get sock theo thu tu khoang cach nho den lon
            string minlink = "";
            string[] separatingChars1 = { state + " " };
            string[] separatingChars2 = { " network" };
            string[] separatingChars3 = { "javascript:showinfo('" };
            {
                string line11 = "";
                float mindistance = 1000;
                string sock = "";
                foreach (var line2 in File.ReadLines("sockWithCityHttpRequest.txt"))
                {
                    if (!record.Contains(line2.Split(' ').Last().Split('=').Last().Split('\'').First()))
                    {
                        float distance = 1000;
                        foreach (var line3 in File.ReadLines("testdemo3.txt"))
                        {
                            //var elements = line1.Split('\t');
                            //foreach (string element in elements)
                            //{
                            //    if (element.Contains("http://"))
                            //    {
                            //        line11 = element.Split('/')[2].Split('.')[0];
                            //        break;
                            //    }
                            //}
                            line11 = linedemo1;
                            if (line11 + "," + state == line3.Split('\t')[0] && line2.Split(separatingChars1, System.StringSplitOptions.RemoveEmptyEntries).Last().Split(separatingChars2, System.StringSplitOptions.RemoveEmptyEntries).First() + "," + state == line3.Split('\t')[1])
                            {
                                distance = float.Parse(line3.Split('\t')[2]);
                                break;
                            }
                        }
                        if (mindistance > distance)
                        {
                            mindistance = distance;
                            minlink = "http://admin.5socks.net" + line2.Split(separatingChars3, System.StringSplitOptions.RemoveEmptyEntries).Last().Split('\'').First();
                        }
                    }
                }

                //neu chinh xac thanh pho thi lay
                if (mindistance < 25)
                {
                    //get sock
                    driver.Navigate().GoToUrl(minlink);
                    if (driver.FindElement(By.TagName("body")).Text.Contains("click here to view"))
                    {
                        driver.FindElement(By.XPath("/html/body/font[2]/table/tbody/tr/td/table/tbody/tr/td/table/tbody/tr[9]/td[2]/a")).Click();
                        System.Threading.Thread.Sleep(3000);
                        sock = minlink;
                    }
                    else
                    {
                        sock = minlink;
                    }
                    driver.Navigate().GoToUrl("http://admin.5socks.net/cgi-bin/online.cgi?order=0&city=&page=1&country=&s_city=&s_country=&s_state=" + state + "&s_host=&s_ip=&search=1");
                    record += minlink;
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
                    {
                        {
                            file.WriteLine(sock);
                        }
                    }
                }
                else
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
                    {
                        {
                            file.WriteLine("N/A");
                        }
                    }
            }
        }
        Nearest5SocksCityStep2(label3);
    }

    // pvas => temp
    public static void sameState5sock(IWebDriver driver = null, System.Windows.Forms.Label label3 = null, int duplicate = 0)
    {
        File.Delete("temp.txt");
        File.Delete("testdemo1.txt");
        File.Delete("testdemo2.txt");
        File.Delete("socks.txt");

        if (driver == null)
        {
            driver = new FirefoxDriver();
        }

        //IWebDriver driver1 = new FirefoxDriver();
        //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        ////get distinct city pvas
        //var links = File.ReadLines("pvas.txt");
        //using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo1.txt"))
        //{
        //    foreach (string link in links)
        //    {
        //        var elements = link.Split('\t');
        //        foreach (string element in elements)
        //        {
        //            if (element.Contains(","))
        //            {
        //                file.WriteLine(element);
        //                break;
        //            }
        //        }
        //    }
        //}
        //string[] lines = File.ReadAllLines("testdemo1.txt");
        //File.WriteAllLines("testdemo1.txt", lines.Distinct().ToArray());


        login5socks(driver);
        string record = "";
        //get distinct city 5socks page ung voi moi distinct city pvas
        foreach (var line1 in File.ReadLines("pvas.txt"))
        {
            string linedemo1 = "";
            string state = "";
            //var elements = line1.Split('\t');
            //foreach (string element in elements)
            //{
            //    if (element.Contains(","))
            //    {
            //        linedemo1 = element.Split(',')[0];
            state = line1.Split('\t')[3];
            //        break;
            //    }
            //}

            //get distinct city 5socks ung voi state cua city pvas
                       driver.Navigate().GoToUrl("http://admin.5socks.net/cgi-bin/online.cgi?order=0&city=&page=1&country=&s_city=&s_country=&s_state=" + state + "&s_host=&s_ip=&search=1");
            //File.WriteAllText("testdemo2.txt", driver.FindElement(By.TagName("body")).Text);
            //int num = File.ReadLines("testdemo2.txt").Count();
            //var temp = File.ReadAllLines("testdemo2.txt");
            //File.Delete("testdemo2.txt");

            File.Delete("sockWithCityHttpRequest.txt");
            //for (int i = 0; i < num - 5; i++)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("sockWithCityHttpRequest.txt", true))
                {
                    //file.WriteLine(temp[i + 4] + "\t" + driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[" + (i + 3) + "]/td[2]/a")).GetAttribute("href"));
                    file.WriteLine(driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[3]/td[2]/a")).GetAttribute("href"));
                    //file.WriteLine(driver.FindElements(By.PartialLinkText(".")).Skip(1).First().GetAttribute("href"));
                    
                }
            }

            //for (int i = 0; i < num - 5; i++)
            //{
            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo2.txt", true))
            //    {
            //        if (driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[" + (i + 3) + "]/td[5]")).Text.Trim() != "")
            //        {
            //            file.WriteLine(driver.FindElement(By.XPath("/html/body/font[2]/center/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[" + (i + 3) + "]/td[5]")).Text);
            //        }
            //    }
            //}
            //lines = File.ReadAllLines("testdemo2.txt");
            //File.WriteAllLines("testdemo2.txt", lines.Distinct().ToArray());

            ////get distance giua cac distinct cua 5socks va mot distinct pvas tuong ung dang xet

            //foreach (var line2 in File.ReadLines("testdemo2.txt"))
            //{
            //    bool exist = false;
            //    foreach (var line3 in File.ReadLines("testdemo3.txt"))
            //    {
            //        if (linedemo1 + "," + state == line3.Split('\t')[0] && line2 + "," + state == line3.Split('\t')[1])
            //            exist = true;
            //    }

            //    if (!exist)
            //    {
            //        driver.Navigate().GoToUrl("http://www.distance-cities.com/search?from=" + linedemo1 + "%2C+" + state + "%2C+United+States&to=" + line2 + "%2C+" + state + "%2C+United+States&country=");
            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo3.txt", true))
            //        {
            //            file.WriteLine(linedemo1 + "," + state + "\t" + line2 + "," + state + "\t" + driver.FindElement(By.Id("kmslinearecta")).Text.Split(' ')[0].Replace('.', ','));
            //        }
            //    }
            //}

            //driver.Navigate().GoToUrl("http://admin.5socks.net/cgi-bin/online.cgi?order=0&city=&page=1&country=&s_city=&s_country=&s_state=" + state + "&s_host=&s_ip=&search=1");
            ////get sock theo thu tu khoang cach nho den lon
            string minlink = "";
            string[] separatingChars1 = { state + " " };
            string[] separatingChars2 = { " network" };
            string[] separatingChars3 = { "javascript:showinfo('" };
            {
                string line11 = "";
                float mindistance = 1000;
                string sock = "";
                foreach (var line2 in File.ReadLines("sockWithCityHttpRequest.txt"))
                {
                    if (!record.Contains(line2.Split(' ').Last().Split('=').Last().Split('\'').First()))
                    {
                        float distance = 1000;
                        //foreach (var line3 in File.ReadLines("testdemo3.txt"))
                        //{
                        //    //var elements = line1.Split('\t');
                        //    //foreach (string element in elements)
                        //    //{
                        //    //    if (element.Contains("http://"))
                        //    //    {
                        //    //        line11 = element.Split('/')[2].Split('.')[0];
                        //    //        break;
                        //    //    }
                        //    //}
                        //    line11 = linedemo1;
                        //    if (line11 + "," + state == line3.Split('\t')[0] && line2.Split(separatingChars1, System.StringSplitOptions.RemoveEmptyEntries).Last().Split(separatingChars2, System.StringSplitOptions.RemoveEmptyEntries).First() + "," + state == line3.Split('\t')[1])
                        //    {
                        //        distance = float.Parse(line3.Split('\t')[2]);
                        //        break;
                        //    }
                        //}
                        //if (mindistance > distance)
                        {
                            //mindistance = distance;
                            minlink = "http://admin.5socks.net" + line2.Split(separatingChars3, System.StringSplitOptions.RemoveEmptyEntries).Last().Split('\'').First();
                            break;
                        }
                    }
                }

                //neu chinh xac thanh pho thi lay
                //if (mindistance < 25)
                {
                    //get sock
                    driver.Navigate().GoToUrl(minlink);
                    if (driver.FindElement(By.TagName("body")).Text.Contains("click here to view"))
                    {
                        driver.FindElement(By.XPath("/html/body/font[2]/table/tbody/tr/td/table/tbody/tr/td/table/tbody/tr[9]/td[2]/a")).Click();
                        System.Threading.Thread.Sleep(3000);
                        sock = minlink;
                    }
                    else
                    {
                        sock = minlink;
                    }
                    driver.Navigate().GoToUrl("http://admin.5socks.net/cgi-bin/online.cgi?order=0&city=&page=1&country=&s_city=&s_country=&s_state=" + state + "&s_host=&s_ip=&search=1");
                    //neu ko cho phep hai pvas trung ip sock
                    if (duplicate == 0)
                    {
                        record += minlink;
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
                    {
                        {
                            file.WriteLine(sock);
                        }
                    }
                }
                //else
                //    using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
                //    {
                //        {
                //            file.WriteLine("N/A");
                //        }
                //    }
            }
        }
        Nearest5SocksCityStep2(label3);
    }

    private static void login5socks(IWebDriver driver)
    {
        driver.Navigate().GoToUrl("http://admin.5socks.net/");

        driver.FindElement(By.Name("user")).SendKeys("wifi4rents222");
        driver.FindElement(By.Name("pass")).SendKeys("CHIc5SFF6D");
        //driver.FindElement(By.Name("user")).SendKeys("wifi4rents22");
        //driver.FindElement(By.Name("pass")).SendKeys("BAHG9T3r1U");
        //driver.FindElement(By.Name("user")).SendKeys("wifi4rents3");
        //driver.FindElement(By.Name("pass")).SendKeys("xGEpo8H2HK");
        driver.FindElement(By.Name("pass")).Submit();
    }

    // pvas, socks => socks
    public static void NearestSocksCity(IWebDriver driver, System.Windows.Forms.Label label3)
    {
        File.Delete("testdemo1.txt");
        File.Delete("testdemo2.txt");

        if (driver == null)
        {
            driver = new FirefoxDriver();
        }

        //get distinct city pvas
        var links = File.ReadLines("pvas.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo1.txt"))
        {
            foreach (string link in links)
            {
                var elements = link.Split('\t');
                foreach (string element in elements)
                {
                    if (element.Contains("http://"))
                    {
                        file.WriteLine(element.Split('/')[2].Split('.')[0]);
                        break;
                    }
                }
            }
        }
        string[] lines = File.ReadAllLines("testdemo1.txt");
        File.WriteAllLines("testdemo1.txt", lines.Distinct().ToArray());

        //get distinct city sock file
        links = File.ReadLines("sockWithCityHttpRequest.txt");
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo2.txt"))
        {
            foreach (string link in links)
            {
                file.WriteLine(link.Split('\t').Last());
            }
        }
        lines = File.ReadAllLines("testdemo2.txt");
        File.WriteAllLines("testdemo2.txt", lines.Distinct().ToArray());

        //get distance giua 2 distinct file
        foreach (var line1 in File.ReadLines("testdemo1.txt"))
        {
            foreach (var line2 in File.ReadLines("testdemo2.txt"))
            {
                bool exist = false;
                foreach (var line3 in File.ReadLines("testdemo3.txt"))
                {
                    if (line1 == line3.Split('\t')[0] && line2 == line3.Split('\t')[1])
                        exist = true;
                }

                if (!exist)
                {
                    driver.Navigate().GoToUrl("http://www.distance-cities.com/search?from=" + line1 + "%2C+TX%2C+United+States&to=" + line2 + "%2C+TX%2C+United+States&country=");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo3.txt", true))
                    {
                        file.WriteLine(line1 + "\t" + line2 + "\t" + driver.FindElement(By.Id("kmslinearecta")).Text.Split(' ')[0].Replace('.', ','));
                    }
                }
            }
        }

        //get sock theo thu tu khoang cach nho den lon
        File.Delete("socks.txt");
        string record = "";
        foreach (var line1 in File.ReadLines("pvas.txt"))
        {
            string line11 = "";
            float mindistance = 1000;
            string sock = "";
            foreach (var line2 in File.ReadLines("sockWithCityHttpRequest.txt"))
            {
                if (!record.Contains(line2.Split('\t').First()))
                {
                    float distance = 1000;
                    foreach (var line3 in File.ReadLines("testdemo3.txt"))
                    {
                        var elements = line1.Split('\t');
                        foreach (string element in elements)
                        {
                            if (element.Contains("http://"))
                            {
                                line11 = element.Split('/')[2].Split('.')[0];
                                break;
                            }
                        }
                        if (line11 == line3.Split('\t')[0] && line2.Split('\t').Last() == line3.Split('\t')[1])
                        {
                            distance = float.Parse(line3.Split('\t')[2]);
                            break;
                        }
                    }
                    if (mindistance > distance)
                    {
                        mindistance = distance;
                        sock = line2.Split('\t').First();
                    }
                }
            }

            record += sock;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("socks.txt", true))
            {
                {
                    file.WriteLine(sock);
                }
            }
        }

        //GetCityOfSockHttpRequest(label3);
        //string recordsock = "";

        //for (int i = 0; i < File.ReadAllLines("pvas.txt").Count(); i++)
        //{
        //    float mindistance = 1000;
        //    int numline=1000;
        //    string sock="";
        //    string pvas = File.ReadLines("pvas.txt").Skip(i).First();
        //    string pvascity="";
        //    var elements = pvas.Split('\t');
        //    foreach (string element in elements)
        //    {
        //        if (element.Contains("http://"))
        //        {
        //            pvascity = element.Split('/')[2].Split('.')[0];
        //            break;
        //        }
        //    }
        //    for (int j = 0; j < File.ReadAllLines("sockWithCityHttpRequest.txt").Count(); j++)
        //    {
        //        string sockcity = File.ReadLines("sockWithCityHttpRequest.txt").Skip(j).First().Split('\t').Last();
        //        driver.Navigate().GoToUrl("http://www.distance-cities.com/search?from="+pvascity+"%2C+TX%2C+United+States&to="+sockcity+"%2C+TX%2C+United+States&country=");
        //        using (System.IO.StreamWriter file = new System.IO.StreamWriter("testdemo1.txt", true))
        //        {
        //            try
        //            {
        //                string info = driver.FindElement(By.Id("kmslinearecta")).Text;
        //                file.WriteLine(pvascity+"\t"+sockcity+"\t"+ info.Split(' ')[0]);
        //                float distance = float.Parse(info.Split(' ')[0].Replace('.',','));
        //                if (mindistance > distance&&!recordsock.Contains(File.ReadLines("sockWithCityHttpRequest.txt").Skip(j).First().Split('\t').First()))
        //                {
        //                    mindistance = distance;
        //                    numline = j + 1;
        //                    sock = File.ReadLines("sockWithCityHttpRequest.txt").Skip(j).First().Split('\t').First();
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("{0} Second exception caught.", e);
        //            }
        //        } 
        //    }

        //    //LineToFirst("socks.txt", numline);
        //    if (sock != "")
        //    {
        //        recordsock += sock + "\t";
        //    }
        //    else
        //        break;
        //}

        //File.Delete("socks.txt");
        //File.WriteAllText("socks.txt",recordsock.Replace("\t","\n"));
    }

    // => temp
    public static void LineToFirst(string proxyFile, int numberDelete = 6)
    {
        //read file numberDelete lines
        var first6 = File.ReadLines(proxyFile).Take(numberDelete - 1);
        var theRemain = File.ReadLines(proxyFile).Skip(numberDelete + 1);

        //copy to others file
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            {
                File.ReadLines(proxyFile).Skip(numberDelete - 1).First();
            }
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
        {
            foreach (string line in first6)
            {
                file.WriteLine(line);
            }
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
        {
            foreach (string line in theRemain)
            {
                file.WriteLine(line);
            }
        }

        File.Delete(proxyFile);
        File.Copy("temp.txt", proxyFile);
    }

    //proxy.txt, links.txt =>
    public static void flagHttprequest(System.Windows.Forms.Label label3)
    {
        //File.Delete("proxyWithCityHttpRequest.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("proxy.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("proxy.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread3[] thr = new MyThread3[1000];
            Thread[] tid = new Thread[1000];

            //MyThread thr1 = new MyThread();
            //MyThread thr2 = new MyThread();

            //Thread tid1 = new Thread(new ThreadStart(thr1.Thread1));
            //Thread tid2 = new Thread(new ThreadStart(thr2.Thread1));

            //tid1.Start();
            //tid2.Start();
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("proxy.txt").Count() / block)
                num = File.ReadAllLines("proxy.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = ReadProxyAtLine(i + 1, "proxy.txt");
                thr[i] = new MyThread3();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    public static void BuildProxifierProfierWithSocks5()
    {
        string proxifierProfilePath = File.ReadAllLines("config.txt")[5].Split('=')[1] + "\\testpro.ppx";

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(proxifierProfilePath))
        {
            file.Write(File.ReadAllText("proxifierBefore.txt"));
        }

        var proxies = File.ReadAllLines("socks.txt");

        int index = 101;
        foreach (string proxy in proxies)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(proxifierProfilePath, true))
            {
                string proxyTemplate = File.ReadAllText("proxifierSocks5.txt");
                proxyTemplate = proxyTemplate.Replace("index", index.ToString());
                proxyTemplate = proxyTemplate.Replace("ip", proxy.Split(':')[0]);
                proxyTemplate = proxyTemplate.Replace("port", proxy.Split(':')[1]);
                file.Write(proxyTemplate);
                index += 1;
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(proxifierProfilePath, true))
        {
            file.Write(File.ReadAllText("proxifierAfter.txt"));
        }
    }

    public static void SetProxyWithProxifier()
    {
        string proxifierProfilePath = File.ReadAllLines("config.txt")[5].Split('=')[1] + "\\testpro.ppx";
        int proxies = File.ReadLines("proxy.txt").Count() + 100;

        //while (true)
        {
            int index = int.Parse(File.ReadAllLines("proxifierIndex.txt")[0]);
            string proxifierProfile = File.ReadAllText(proxifierProfilePath);
            if (index == proxies)
            {
                proxifierProfile = proxifierProfile.Replace("<Action type=\"Proxy\">" + index + "</Action>", "<Action type=\"Proxy\">100</Action>");
            }
            else
                proxifierProfile = proxifierProfile.Replace("<Action type=\"Proxy\">" + index + "</Action>", "<Action type=\"Proxy\">" + ++index + "</Action>");

            //copy to others file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
            {
                file.Write(proxifierProfile);
            }

            //save index to file
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxifierIndex.txt"))
            {
                if (index == proxies)
                {
                    file.Write("100");
                }
                else
                    file.Write(index);
            }

            File.Delete(proxifierProfilePath);
            File.Copy("temp.txt", proxifierProfilePath);

            //updateProxifier after change proxy
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("changeProxifier.exe");
            cmd.WaitForExit();

            Console.WriteLine(index);
            //System.Threading.Thread.Sleep(100);


            //driver.Navigate().Refresh(); 
        }
    }

    //proxifierBefore.txt, proxy.txt, proxifierAfter.txt => testpro.ppx
    public static void BuildProxifierProfierWithProxy()
    {
        string proxifierProfilePath = File.ReadAllLines("config.txt")[5].Split('=')[1] + "\\testpro.ppx";

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(proxifierProfilePath))
        {
            file.Write(File.ReadAllText("proxifierBefore.txt"));
        }

        var proxies = File.ReadAllLines("proxy.txt");

        int index = 101;
        foreach (string proxy in proxies)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(proxifierProfilePath, true))
            {
                string proxyTemplate = File.ReadAllText("proxifierProxyNoAuthen.txt");
                proxyTemplate = proxyTemplate.Replace("index", index.ToString());
                proxyTemplate = proxyTemplate.Replace("ip", proxy.Split(':')[0]);
                //proxyTemplate = proxyTemplate.Replace("user", proxy.Split(':')[2]);
                file.Write(proxyTemplate);
                index += 1;
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(proxifierProfilePath, true))
        {
            file.Write(File.ReadAllText("proxifierAfter.txt"));
        }
    }

    //adsproxy.txt => adsProxyWithZipcode.txt
    public static void GetZipcodeOfProxy()
    {
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        driver.Quit();
        driver = RestartFirefox(profile);
        File.Delete("adsProxyWithZipcode.txt");


        for (int i = 0; i < File.ReadAllLines("adsproxy.txt").Count(); i++)
        {
            string proxy = ReadProxyAtLine(i + 1, "adsproxy.txt");
            SetProxyEntireComputer(proxy);

            driver.Navigate().GoToUrl("http://www.ip2location.com");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("adsProxyWithZipcode.txt", true))
            {
                file.WriteLine(File.ReadAllLines("adsproxy.txt")[i] + ":" + driver.FindElement(By.Id("zipCode")).Text);
            }
        }
    }

    public static string RandomReplaceCharacter(string text, int percentTextBeReplace)
    {

        Random rn = new Random();
        string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";

        MatchEvaluator RandomChar = delegate(Match m)
        {
            return charsToUse[rn.Next(charsToUse.Length)].ToString();
        };

        //Console.WriteLine(Regex.Replace("XXXX-XXXX-XXXX-XXXX-XXXX", "X", RandomChar));
        //// Lv2U-jHsa-TUep-NqKa-jlBx
        //Console.WriteLine(Regex.Replace("XXXX", "X", RandomChar));
        //// 8cPD

        Random rand = new Random();

        for (int i = 0; i < text.Count() * percentTextBeReplace / 100; i++)
        {
            char[] array = text.ToCharArray();
            int index = rand.Next(text.Count());
            if (array[index] != ' ')
            {
                array[index] = 'X';
            }
            text = new string(array);
            //Console.WriteLine(Regex.Replace(text, "X", RandomChar));
            text = Regex.Replace(text, "X", RandomChar);
        }
        return text;
    }

    public static string RandomUppercaseCharacter(string text, int percentTextBeReplace)
    {

        Random rn = new Random();
        string charsToUse = "AzByCxDwEvFuGtHsIrJqKpLoMnNmOlPkQjRiShTgUfVeWdXcYbZa1234567890";

        MatchEvaluator RandomChar = delegate(Match m)
        {
            return charsToUse[rn.Next(charsToUse.Length)].ToString();
        };

        Random rand = new Random();

        if (percentTextBeReplace > 0)
        {
            for (int i = 0; i < text.Count() * percentTextBeReplace / 100; i++)
            {
                char[] array = text.ToCharArray();
                int index = rand.Next(text.Count());
                if (array[index] != ' ')
                {
                    text = Regex.Replace(text, array[index].ToString(), array[index].ToString().ToUpper());
                }
                //Console.WriteLine(Regex.Replace(text, "X", RandomChar));

            }
        }
        else
            for (int i = 0; i < 1; i++)
            {
                char[] array = text.ToCharArray();
                int index = rand.Next(text.Count());
                if (array[index] != ' ')
                {
                    text = Regex.Replace(text, array[index].ToString(), array[index].ToString().ToUpper());
                }
                //Console.WriteLine(Regex.Replace(text, "X", RandomChar));

            }
        return text;
    }

    public static void ChangeAnnotation(string value = "login")
    {
        //file chua annotation can thay doi
        string profile = File.ReadAllLines("config.txt")[0].Split('=')[1];
        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(profile);
        foreach (string subdirectory in subdirectoryEntries)
            if (subdirectory.Contains(File.ReadAllLines("config.txt")[3].Split('=')[1]))
                profile = subdirectory + "\\";

        profile += "logins.json";

        //thay doi annotation
        //string value = File.ReadAllLines("config.txt")[1].Split('=')[1];
        string annotation = File.ReadAllLines(profile)[0];
        var proxies = File.ReadLines("proxy.txt");
        foreach (string proxy in proxies)
        {
            string[] proxyElement = proxy.Split(':');
            annotation = annotation.Replace("httpRealm\":\"moz-proxy://" + proxyElement[0] + ":" + proxyElement[1], "httpRealm\":\"" + value);
        }

        //copy vao lai file
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            file.WriteLine(annotation);
        }

        File.Delete(profile);
        File.Copy("temp.txt", profile);
    }

    public static void FlagProxyModuleWithPR()
    {
        System.Diagnostics.Process cmd;
        cmd = System.Diagnostics.Process.Start("flagProxyModuleWithPR.exe");
        cmd.WaitForExit();
    }

    public static void FlagProxySystemNoPR(System.Windows.Forms.Label label4)
    {
        int loop = 10;
        int numlink = 8;
        System.Diagnostics.Process cmd;
        //DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();
        //ReplacePR();

        //FirefoxProfileManager profileManager = new FirefoxProfileManager();
        //FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        //IWebDriver driver = new FirefoxDriver(profile);
        IWebDriver driver = null;

        CheckLinkDie(label4);
        File.Delete("links.txt");
        File.Copy("survivalLinks.txt", "links.txt");

        for (int j = 1; j < 1800; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver = restartIE(driver, numlink);

            for (int i = 1; i <= loop; i++)
            {
                //changeUAFirefox
                //cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                //cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                if (i != 0)
                {
                    string proxy = ReadProxyAtLine(i, "proxy.txt");
                    SetProxyEntireComputer(proxy);
                    //SetSockEntireComputer(proxy);
                }

                var links = ReadnLinks(numlink);

                //load link into 4tabs of firefox
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
                {
                    file.Write(numlink);
                }
                cmd = System.Diagnostics.Process.Start("load4LinksIE.exe");
                cmd.WaitForExit();

                //cho web load xong
                //int tab = 1;
                //foreach (string link in links)
                //{
                //    try
                //    {
                //        IWebElement body = driver.FindElement(By.TagName("body"));
                //        body.SendKeys(Keys.Control + tab);
                //        driver.SwitchTo().DefaultContent();
                //        tab += 1;
                //        System.Threading.Thread.Sleep(500);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}

                System.Threading.Thread.Sleep(10000);
                //click flag
                cmd = System.Diagnostics.Process.Start("clickflagIE.exe");
                cmd.WaitForExit();
                System.Threading.Thread.Sleep(2000);

                //get link song
                //int[] live = new int[5];
                //tab = 1;

                //foreach (string link in links)
                //{
                //    try
                //    {
                //        IWebElement body = driver.FindElement(By.TagName("body"));
                //        body.SendKeys(Keys.Control + tab);
                //        driver.SwitchTo().DefaultContent();
                //        tab += 1;
                //        string a = driver.FindElement(By.TagName("body")).Text;
                //        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                //            live[tab - 1] = 1;
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}

                ////deleteDeadLink 
                //if (i != 0)
                //{
                //    DeleteDeadLink(driver, links, live);
                //}

                //dem so link con lai
                label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = File.ReadLines("links.txt").Count().ToString()));
                File.WriteAllText("linkcount.txt", File.ReadLines("links.txt").Count().ToString());
            }

            ReplaceOverloadLink(label4, driver, j);

            DeleteFirstLineSock("proxy.txt", loop);
            //ReplacePR();
            //ResetProxySockEntireComputer();
            //cmd = System.Diagnostics.Process.Start("clickPR.exe");
            //cmd.WaitForExit();
        }
    }

    private static IWebDriver restartIE(IWebDriver driver, int numlink = 4,InternetExplorerOptions options=null)
    {
        if (driver != null)
        {
            driver.Quit();
        }

        foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
        {
            if (myProc.ProcessName == "iexplore")
            {
                myProc.Kill();
            }
        }

        //Delete(All) ie file
        System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
        cmd.WaitForExit();
        if (options==null)
        {
            driver = new InternetExplorerDriver(@"C:\"); 
        }
        else
            driver = new InternetExplorerDriver(options); 

        for (int i = 1; i < numlink; i++)
        {
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Control + 't');
        }

        return driver;
    }

    public static void SeoProxyFFNoPR(System.Windows.Forms.Label label4)
    {
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                if (i != 0)
                {
                    //string proxy = ReadProxyAtLine(i, "proxy.txt");
                    //SetProxyEntireComputer(proxy);

                    //set proxy firefox
                    string proxy = ReadProxyAtLine(i, "proxy.txt");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
                    {
                        file.Write(proxy);
                    }
                    cmd = System.Diagnostics.Process.Start("ChangeProxyFirefox.exe");
                    cmd.WaitForExit();
                }
                driver.Navigate().GoToUrl("http://craigslistpostingandflaggingservices.blogspot.com/");
                System.Threading.Thread.Sleep(45000);
            }
            DeleteFirstLineSock("proxy.txt");
        }
    }

    public static void SeoProxySystemNoPR(System.Windows.Forms.Label label4)
    {
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                //doc line socks thu $i de nap vao firefox
                if (i != 0)
                {
                    string proxy = ReadProxyAtLine(i, "proxy.txt");
                    SetProxyEntireComputer(proxy);
                }
                driver.Navigate().GoToUrl("http://craigslistpostingandflaggingservices.blogspot.com/");
                System.Threading.Thread.Sleep(45000);
            }
            DeleteFirstLineSock("proxy.txt");
        }
    }

    public static void FlagProxyFFNoPR(System.Windows.Forms.Label label4)
    {
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();
        ReplacePR();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        CheckLinkDie(label4);
        File.Delete("links.txt");
        File.Copy("survivalLinks.txt", "links.txt");

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                if (i != 0)
                {
                    string proxy = ReadProxyAtLine(i, "proxy.txt");
                    SetProxyEntireComputer(proxy);
                    //SetSockEntireComputer(proxy);
                }

                var links = ReadnLinks();

                //load link into 4tabs of firefox
                cmd = System.Diagnostics.Process.Start("load4Links.exe");
                cmd.WaitForExit();

                //wait 15s <= read from file config

                //click flag
                int tab = 1;

                if (i != 0)
                {
                    foreach (string link in links)
                    {
                        try
                        {
                            IWebElement body = driver.FindElement(By.TagName("body"));
                            body.SendKeys(Keys.Control + tab);
                            driver.SwitchTo().DefaultContent();
                            tab += 1;
                            driver.FindElement(By.PartialLinkText("prohibited")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                }

                //get link song
                int[] live = new int[5];
                tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        string a = driver.FindElement(By.TagName("body")).Text;
                        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                            live[tab - 1] = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //deleteDeadLink 
                if (i != 0)
                {
                    DeleteDeadLink(driver, links, live);
                }

                //dem so link con lai
                label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = File.ReadLines("links.txt").Count().ToString()));
            }

            if (j % 4 == 0)
            {
                Replace4Link(driver);
            }

            DeleteFirstLineSock("proxy.txt");
            ReplacePR();
            //ResetProxySockEntireComputer();
            //cmd = System.Diagnostics.Process.Start("clickPR.exe");
            //cmd.WaitForExit();
        }
    }

    public static void Flag1Link()
    {
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();
        ReplacePR();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                string proxy = ReadProxyAtLine(i, "proxy.txt");
                SetProxyEntireComputer(proxy);

                var links = Read1Links();

                //load link into 4tabs of firefox
                //cmd = System.Diagnostics.Process.Start("load4Links.exe");
                //cmd.WaitForExit();

                foreach (string link in links)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(link);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //wait 15s <= read from file config

                //click flag
                int tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        driver.FindElement(By.PartialLinkText("prohibited")).Click();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //get link song
                int[] live = new int[5];
                tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        string a = driver.FindElement(By.TagName("body")).Text;
                        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                            live[tab - 1] = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //deleteDeadLink 
                //DeleteDeadLink(driver, links, live);
            }

            DeleteFirstLineSock("proxy.txt");
            ReplacePR();
            //ResetProxySockEntireComputer();
            //cmd = System.Diagnostics.Process.Start("clickPR.exe");
            //cmd.WaitForExit();
        }
    }

    public static void FlagVIP72ModuleWithPR(System.Windows.Forms.Label label3, System.Windows.Forms.ProgressBar progressBar2)
    {
        System.Diagnostics.Process cmd;
        ResetProxySockEntireComputer();
        ReplacePR();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        CheckLinkDie(label3);
        File.Delete("links.txt");
        File.Copy("survivalLinks.txt", "links.txt");

        for (int j = 1; j < 1800; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 0; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                {
                    cmd = System.Diagnostics.Process.Start("changeSockVIP.exe");
                    cmd.WaitForExit();
                    //SetSockEntireComputer(proxy);
                }

                var links = ReadnLinks();

                //load link into 4tabs of firefox
                cmd = System.Diagnostics.Process.Start("load4Links.exe");
                cmd.WaitForExit();

                //wait 15s <= read from file config

                //click flag
                int tab = 1;

                if (i != 0)
                {
                    foreach (string link in links)
                    {
                        try
                        {
                            IWebElement body = driver.FindElement(By.TagName("body"));
                            body.SendKeys(Keys.Control + tab);
                            driver.SwitchTo().DefaultContent();
                            tab += 1;
                            driver.FindElement(By.PartialLinkText("prohibited")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                }

                //get link song
                int[] live = new int[5];
                tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        string a = driver.FindElement(By.TagName("body")).Text;
                        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                            live[tab - 1] = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //deleteDeadLink 
                if (i != 0)
                {
                    DeleteDeadLink(driver, links, live);
                }

                //dem so link con lai
                label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = File.ReadLines("links.txt").Count().ToString()));
            }
            ReplaceOverloadLink(label3, driver, j);
            ReplacePR();
        }
    }

    private static void FlagClick(IWebDriver driver, IEnumerable<string> links, int tab)
    {
        foreach (string link in links)
        {
            try
            {
                IWebElement body = driver.FindElement(By.TagName("body"));
                body.SendKeys(Keys.Control + tab);
                driver.SwitchTo().DefaultContent();
                tab += 1;
                driver.FindElement(By.PartialLinkText("prohibited")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }
    }

    private static void loadLinkToBrowser()
    {
        System.Diagnostics.Process cmd;
        cmd = System.Diagnostics.Process.Start("load4Links.exe");
        cmd.WaitForExit();
    }

    public static void changeStatusInputIP(System.Windows.Forms.Label label3, System.Windows.Forms.ProgressBar progressBar2)
    {
        for (int timeCount = 11 - 1; timeCount >= 0; timeCount--)
        {
            //label3.Text = "time to change IP, time remaining in seconds " + timeCount;
            //label3.Refresh();
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "time to change IP, time remaining in seconds " + timeCount));
            progressBar2.Invoke((System.Windows.Forms.MethodInvoker)(() => progressBar2.Value = (10 - timeCount) * 10));
            System.Threading.Thread.Sleep(1000);
        }

        label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "flagging"));
    }

    public static void FlagSocksEscortModuleWithPR(System.Windows.Forms.Label label3, System.Windows.Forms.ProgressBar progressBar2)
    {
        //System.Diagnostics.Process cmd;
        //cmd = System.Diagnostics.Process.Start("flagEscortModuleWithPR.exe");
        //cmd.WaitForExit();

        System.Diagnostics.Process cmd;
        ResetProxySockEntireComputer();
        //ReplacePR();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j < 180; j++)
        {
            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                changeStatusInputIP(label3, progressBar2);

                var links = ReadnLinks();

                //load link into 4tabs of firefox
                cmd = System.Diagnostics.Process.Start("load4Links.exe");
                cmd.WaitForExit();

                //wait 15s <= read from file config

                //click flag
                int tab = 1;

                FlagClick(driver, links, tab);

                //get link song
                int[] live = new int[5];
                tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        string a = driver.FindElement(By.TagName("body")).Text;
                        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                            live[tab - 1] = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //deleteDeadLink 
                DeleteDeadLink(driver, links, live);
            }

            //ReplacePR(); 
            cmd = System.Diagnostics.Process.Start("clickPR.exe");
            cmd.WaitForExit();
        }
    }

    public static void PostingProxyModuleNoPR(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost)
    {
        System.Diagnostics.Process cmd;
        ResetProxySockEntireComputer();
        WriteLinePostingLog();
        //WritePostingLog("-----------------------------------");
        //WriteLinePostingLog();

        FirefoxProfileManager profileManager1 = new FirefoxProfileManager();
        FirefoxProfile profile1 = profileManager1.GetProfile("default");
        ChangeSockFirefox(profile1, ReadProxyAtLine(1, "socks.txt"));
        IWebDriver driver1 = new FirefoxDriver(profile1);

        //IWebDriver driver1 = new InternetExplorerDriver(@"C:\");
        //IWebDriver driver1 = new ChromeDriver(@"C:\");

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile("newprofile");
        ChangeSockFirefox(profile, ReadProxyAtLine(1, "socks.txt"));
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j <= int.Parse(numpva); j++)
        {
            //dua cap mail va proxy o tren cung xuong duoi cung cua hang doi
            //DeleteFirstLineSock("adsProxyWithZipcode.txt", 1);
            //DeleteFirstLineSock("pvas.txt", 1);

            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            //ReplacePR();
            if (j != 1)
            {
                driver.Quit();
                driver1.Quit();
                driver = new FirefoxDriver(profile);
                ChangeSockFirefox(profile1, ReadProxyAtLine(1, "socks.txt"));
                driver1 = new FirefoxDriver(profile1);
            }

            //doc line socks thu $i de nap vao firefox
            //if (new FileInfo("proxy.txt").Length != 0)
            //{
            //    string proxy = ReadProxyAtLine(1, "proxy.txt");
            //    SetProxyEntireComputer(proxy);
            //}

            //doc line socks thu $i de nap vao firefox
            //if (new FileInfo("socks.txt").Length != 0)
            //{
            //    //doc line socks thu $i de nap vao firefox
            //    string proxy = ReadProxyAtLine(1, "socks.txt");
            //    //set socks for firefox
            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
            //    {
            //        file.Write(proxy);
            //    }
            //    cmd = System.Diagnostics.Process.Start("ChangeSockFirefox.exe");
            //    cmd.WaitForExit();
            //}

            //changeUAFirefox
            //cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
            //cmd.WaitForExit();

            for (int i = 1; i <= int.Parse(adsPerPva); i++)
            {
                label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = i.ToString()));

                //doc line socks thu $i de nap vao firefox
                //if (new FileInfo("proxy.txt").Length != 0)
                //{
                //    string proxy = ReadProxyAtLine(1, "proxy.txt");
                //    SetProxyEntireComputer(proxy);
                //}

                WritePostingLog(GetIPOfProxyByHttpRequest());

                //---------------------------------------------------------------------

                //PostingProcessMyAccount(driver,driver1, 1);
                PostingProcessPost2Classsified(driver, driver1, 1, mucpost);
                WritePostingLog(DateTime.Now + "|" + TimeZoneInfo.Local);
                WriteLinePostingLog();
                //System.Threading.Thread.Sleep(30000);
                //-----------------------------------------------------------------
                //cho mail confirm vao hom mail
                try
                {
                    IWebElement body = driver.FindElement(By.TagName("body"));
                    driver.SwitchTo().DefaultContent();
                    string a = driver.FindElement(By.TagName("body")).Text;
                    if (driver.FindElement(By.TagName("body")).Text.Contains("You should receive an email shortly"))
                    {
                        System.Threading.Thread.Sleep(30000);
                        ClickConfirmationLinkInGmail(driver, driver1);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }




                //Console.WriteLine(proxy);

                ////click flag
                //int tab = 1;

                //foreach (string link in links)
                //{
                //    try
                //    {
                //        IWebElement body = driver.FindElement(By.TagName("body"));
                //        body.SendKeys(Keys.Control + tab);
                //        driver.SwitchTo().DefaultContent();
                //        tab += 1;
                //        driver.FindElement(By.PartialLinkText("prohibited")).Click();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}


                ////get link song
                //int[] live = new int[5];
                //tab = 1;

                //foreach (string link in links)
                //{
                //    try
                //    {
                //        IWebElement body = driver.FindElement(By.TagName("body"));
                //        body.SendKeys(Keys.Control + tab);
                //        driver.SwitchTo().DefaultContent();
                //        tab += 1;
                //        string a = driver.FindElement(By.TagName("body")).Text;
                //        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                //            if (driver.Url.Contains("craigslist.org") && !driver.Url.Contains("post.craigslist.org"))
                //                live[tab - 1] = 1;
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}


                ////deleteDeadLink 
                //DeleteDeadLink(driver, links, live);
            }

            DeleteFirstLineSock("proxy.txt", 1);
            DeleteFirstLineSock("socks.txt", 1);
            DeleteFirstLineSock("pvas.txt", 1);
            //ReplacePR();
        }

        driver.Quit();
        driver1.Quit();
    }

    public static void PostTutorAndCreatePvasAndForwardmail(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton4, System.Windows.Forms.GroupBox groupbox2)
    {
        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }

        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            if (!proxy.Contains("@outlook.com"))
            {
                ForwardMailModule();
            }

            var checkedButton = groupbox2.Controls.OfType<System.Windows.Forms.RadioButton>()
                        .FirstOrDefault(r => r.Checked);

            CreatePvas(radioButton1.Checked);
            PostTutorOutlookModule(proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked + "\t" + checkedButton.TabIndex, 1);
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    private static void CreatePvas(bool outlook)
    {
    Restart:
        //Delete(All) ie file
        System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
        cmd.WaitForExit();

        IWebDriver driver = null;
        string proxy = "";

        InternetExplorerOptions options = new InternetExplorerOptions();
        string pvas = File.ReadLines("pvas.txt").First() + "\t1\t" + outlook;
        proxy = ReadProxyAtLine(1, "socks.txt");
        Proxy proxy1 = new Proxy();
        proxy1.SocksProxy = proxy;
        options.Proxy = proxy1;

        driver = new InternetExplorerDriver(options);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        try
        {
            //                  driver.Navigate().GoToUrl("http://craigslist.org");

            //driver.FindElement(By.LinkText("post to classifieds")).Click();
            driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
            driver.FindElement(By.LinkText("my account")).Click();
        }
        catch (Exception e)
        {
            //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            //cmd.WaitForExit();
            //return;
            Console.WriteLine("{0} Second exception caught.", e);
        }

        //create pva
        int pass = 0;
        while (true)
        {
            if (pass == 1) break;
            try
            {
                while (!driver.FindElement(By.TagName("body")).Text.Contains("Thanks for signing up for a craigslist account."))
                {
                    if (driver.FindElement(By.TagName("body")).Text.Contains("post to classifieds"))
                    {
                        driver.Quit();
                        goto Restart;
                    }

                    try
                    {
                        driver.FindElement(By.Id("emailAddress")).SendKeys(pvas.Split('\t')[0]);
                        driver.FindElement(By.Id("emailAddress")).Submit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
                pass = 1;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }

        string codebody = "";
        try
        {
            codebody = driver.FindElement(By.TagName("body")).Text;
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
            //goto publish;
        }
        while (!codebody.Contains("Thanks for posting with us. We really appreciate it!"))
        {
            int ok = 0;
        //lay mail confirm ve may
        getmail:
            ResetProxySockEntireComputer();
            int stt = MyThread10.getMailConfirm(pvas, 1);

            //load link confirm len ff
            var links = File.ReadAllLines("socks.txt");
            try
            {
                links = File.ReadAllLines("mailConfirm" + stt + ".txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
                goto getmail;
            }
            foreach (string link in links)
                if (codebody.Contains(link.Split('\t')[0]))
                {
                    SetSockEntireComputer(proxy);
                    driver.Navigate().GoToUrl(link.Split('\t')[1]);
                    ok = 1;
                    break;
                }
            if (ok == 1)
            {
                break;
            }
        }

        //fill pass      
        try
        {
            string password = pvas.Split('\t')[1];
            driver.FindElement(By.Id("p1")).SendKeys(password);
            driver.FindElement(By.Id("p2")).SendKeys(password);
            driver.FindElement(By.Id("p2")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        driver.Quit();
        //end create pva
    }

    public static void PostTutorOnly(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost)
    {
        bool systemfake = true;
        System.Diagnostics.Process cmd;
        ResetProxySockEntireComputer();
        WriteLinePostingLog();
        //WritePostingLog("-----------------------------------");
        //WriteLinePostingLog();

        FirefoxProfileManager profileManager1 = new FirefoxProfileManager();
        FirefoxProfile profile1 = profileManager1.GetProfile("default");
        IWebDriver driver1 = new FirefoxDriver(profile1);

        //IWebDriver driver1 = new InternetExplorerDriver(@"C:\");
        //IWebDriver driver1 = new ChromeDriver(@"C:\");

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile("newprofile");
        if (!systemfake)
        {
            ChangeSockFirefox(profile, ReadProxyAtLine(1, "socks.txt"));
        }
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j <= int.Parse(numpva); j++)
        {
            //dua cap mail va proxy o tren cung xuong duoi cung cua hang doi
            //DeleteFirstLineSock("adsProxyWithZipcode.txt", 1);
            //DeleteFirstLineSock("pvas.txt", 1);

            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            //changeUAFirefox
            //cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
            //cmd.WaitForExit();

            //ReplacePR();
            if (j != 1)
            {
                driver.Quit();
                driver1.Quit();
                if (!systemfake)
                {
                    ChangeSockFirefox(profile, ReadProxyAtLine(1, "socks.txt"));
                }
                driver = new FirefoxDriver(profile);
                driver1 = new FirefoxDriver(profile1);
            }
            for (int i = 1; i <= int.Parse(adsPerPva); i++)
            {
                label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = i.ToString()));
                //WritePostingLog(GetIPOfProxyByHttpRequest());

                //---------------------------------------------------------------------
                PostTutor(driver, driver1, 1, mucpost);
                WritePostingLog(DateTime.Now + "|" + TimeZoneInfo.Local);
                WriteLinePostingLog();
                //System.Threading.Thread.Sleep(30000);
                //-----------------------------------------------------------------
                //cho mail confirm vao hom mail
                try
                {
                    IWebElement body = driver.FindElement(By.TagName("body"));
                    driver.SwitchTo().DefaultContent();
                    string a = driver.FindElement(By.TagName("body")).Text;
                    if (driver.FindElement(By.TagName("body")).Text.Contains("You should receive an email shortly"))
                    {
                        //System.Threading.Thread.Sleep(30000);
                        ClickConfirmationHodinhlam(driver, driver1);
                    }
                    else if (driver.FindElement(By.TagName("body")).Text.Contains("Thanks for posting with us. We really appreciate it!"))
                    {
                        WriteLinePostingLog(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t\t" + getTimeNowVietnam(), "adsLogLink.txt");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }

            DeleteFirstLineSock("proxy.txt", 1);
            DeleteFirstLineSock("socks.txt", 1);
            DeleteFirstLineSock("pvas.txt", 1);
        }

        driver.Quit();
        driver1.Quit();
    }

    public static void PostCanada(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost)
    {
        bool systemfake = true;
        System.Diagnostics.Process cmd;
        IWebDriver driver = null;

        try
        {
            System.Uri uri = new System.Uri("http://localhost:7055/hub");
            driver = new RemoteWebDriver(uri, DesiredCapabilities.Firefox());
            Console.WriteLine("Executed on remote driver");
        }
        catch (Exception)
        {
            driver = new FirefoxDriver();
            Console.WriteLine("Executed on New FireFox driver");
        }

        for (int j = 1; j <= int.Parse(numpva); j++)
        {
            //dua cap mail va proxy o tren cung xuong duoi cung cua hang doi
            //DeleteFirstLineSock("adsProxyWithZipcode.txt", 1);
            //DeleteFirstLineSock("pvas.txt", 1);

            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            //changeUAFirefox
            //cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
            //cmd.WaitForExit();

            //ReplacePR();
            for (int i = 1; i <= int.Parse(adsPerPva); i++)
            {
                label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = i.ToString()));
                //WritePostingLog(GetIPOfProxyByHttpRequest());

                //---------------------------------------------------------------------
                PostCanadaToimage(driver, 1, mucpost);
            }
        }
    }

    public static void PostTutorOutlook(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton4, System.Windows.Forms.GroupBox groupbox2)
    {
        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }

        ResetProxySockEntireComputer();
        var checkedButton = groupbox2.Controls.OfType<System.Windows.Forms.RadioButton>()
                                .FirstOrDefault(r => r.Checked);
        if (checkedButton.TabIndex == 78)
        {
            sameState5sock(null, null, 1);
        }
        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            PostTutorOutlookModule(proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked + "\t" + checkedButton.TabIndex);
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void PostTutorOutlookMulti(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton2)
    {

        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }


        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked;
            tid[i].Start();
        }

        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void IEPostTutorOutlookMulti(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton2)
    {

        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }


        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread4));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked;
            tid[i].Start();
        }

        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void ChangePassPVAMulti(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton2)
    {

        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }


        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread3));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked;
            tid[i].Start();
        }

        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void PostandCreatePva(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton4, System.Windows.Forms.GroupBox groupbox2)
    {

        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }
        var checkedButton = groupbox2.Controls.OfType<System.Windows.Forms.RadioButton>()
                               .FirstOrDefault(r => r.Checked);

        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread5));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked + "\t" + checkedButton.TabIndex;
            tid[i].Start();
        }


        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void PostMultiChrome(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton4,System.Windows.Forms.GroupBox groupbox2)
    {
        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }

        var checkedButton = groupbox2.Controls.OfType<System.Windows.Forms.RadioButton>()
                               .FirstOrDefault(r => r.Checked);
   
        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread7));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked + "\t" + checkedButton.TabIndex;
            tid[i].Start();
                    }

        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void CreateOutlookAlias(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton2)
    {

        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }


        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread6));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked;
            tid[i].Start();
        }

        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void PostCanadaOutlookMulti(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton2)
    {
        //change name image upload
        string link = @"C:\imageupload\";
        var files = new DirectoryInfo(link).GetFiles();
        foreach (var file in files)
        {
            System.IO.File.Move(link + file.Name, link + Path.GetRandomFileName().Replace(".", "") + ".jpg");
        }

        int num = File.ReadLines("pvas.txt").Count();
        MyThread10[] thr = new MyThread10[1000];
        Thread[] tid = new Thread[1000];
        System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

        foreach (FileInfo file in downloadedMessageInfo.GetFiles())
        {
            file.Delete();
        }


        for (int i = 0; i < num; i++)
        {
            string proxy = File.ReadLines("pvas.txt").Skip(i).First();
            thr[i] = new MyThread10();
            tid[i] = new Thread(new ThreadStart(thr[i].Thread2));
            tid[i].Name = proxy + "\t" + (i + 1) + "\t" + radioButton1.Checked;
            tid[i].Start();
            //System.Threading.Thread.Sleep(20000);
        }

        for (int i = 0; i < num; i++)
        {
            tid[i].Join();
        }

        label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = "finish check !!!"));
    }

    public static void forwardMail(System.Windows.Forms.Label label4, string numpva, string adsPerPva, string mucpost)
    {
        System.Diagnostics.Process cmd;
        ResetProxySockEntireComputer();
        WriteLinePostingLog();
        //WritePostingLog("-----------------------------------");
        //WriteLinePostingLog();

        FirefoxProfileManager profileManager1 = new FirefoxProfileManager();
        FirefoxProfile profile1 = profileManager1.GetProfile("default");
        ChangeSockFirefox(profile1, ReadProxyAtLine(1, "socks.txt"));
        IWebDriver driver1 = new FirefoxDriver(profile1);

        //IWebDriver driver1 = new InternetExplorerDriver(@"C:\");
        //IWebDriver driver1 = new ChromeDriver(@"C:\");

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j <= int.Parse(numpva); j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);
            if (j != 1)
            {
                driver.Quit();
                driver1.Quit();
                driver = new FirefoxDriver(profile);
                ChangeSockFirefox(profile1, ReadProxyAtLine(1, "socks.txt"));
                driver1 = new FirefoxDriver(profile1);
            }
            ForwardMailModule();


            DeleteFirstLineSock("socks.txt", 1);
            DeleteFirstLineSock("pvas.txt", 1);
            //ReplacePR();
        }

        driver.Quit();
        driver1.Quit();
    }

    private static void ForwardMailModule()
    {
        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile("default");
        IWebDriver driver = new FirefoxDriver(profile);
        sendConfirmation(driver);
        string code = verifyConfirmation();
        addMailAfterConfirmation(driver, code);
        driver.Quit();
    }

    public static void createGmail(string phone)
    {
        ResetProxySockEntireComputer();
        IWebDriver driver1 = new InternetExplorerDriver();

        createGmailModule(driver1, phone);
        driver1.Quit();
    }

    private static void sendConfirmation(IWebDriver driver1)
    {
        string pvas = File.ReadLines("pvas.txt").First();
        WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(40));
        try
        {
            //vao email lay link confirmation
            driver1.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?service=mail&continue=https://mail.google.com/mail/#identifier");
            //driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t').Last());
            driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t')[0]);
            driver1.FindElement(By.Id("Email")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("Passwd")));
            if (pvas.Split('\t').Last().Contains("hodinhlam911"))
            {
                driver1.FindElement(By.Id("Passwd")).SendKeys("Dangtinthue1234567");
            }
            else
            {
                driver1.FindElement(By.Id("Passwd")).SendKeys(pvas.Split('\t')[1]);
            }
            driver1.FindElement(By.Id("Passwd")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        driver1.Navigate().GoToUrl("https://mail.google.com/mail/?ui=html&zy=h");
        try
        {
            driver1.FindElement(By.ClassName("maia-button")).Submit();
        }
        catch (System.Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        driver1.FindElement(By.LinkText("Settings")).Click();
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.LinkText("Forwarding and POP/IMAP")));
        driver1.FindElement(By.LinkText("Forwarding and POP/IMAP")).Click();
        string code = "";
        var elements = driver1.PageSource.Split('/');
        foreach (string element in elements)
        {
            if (element.Length == 13)
            {
                code = element;
                break;
            }
        }
        driver1.Navigate().GoToUrl("https://mail.google.com/mail/u/0/h/" + code + "/?v=prufw");
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("/html/body/div[2]/form/input[1]")));
        driver1.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).SendKeys("hodinhlam911@gmail.com");
        driver1.FindElement(By.XPath("/html/body/div[2]/form/input[1]")).Submit();
        if (driver1.FindElement(By.TagName("body")).Text.Contains("You already have the forwarding address"))
            return;
        try
        {
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Name("nvp_bu_pfwd")));
        }
        catch (System.Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
            return;
        }
        driver1.FindElement(By.Name("nvp_bu_pfwd")).Submit();
    }

    private static void addMailAfterConfirmation(IWebDriver driver1, string code)
    {
        WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(40));
        driver1.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?service=mail&continue=https://mail.google.com/mail/#identifier");
        try
        {
            driver1.Navigate().GoToUrl("https://mail.google.com/mail/?ui=html&zy=h");
        }
        catch (System.Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        driver1.FindElement(By.LinkText("Settings")).Click();
        driver1.FindElement(By.LinkText("Forwarding and POP/IMAP")).Click();
        //nhap code confirm
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Name("fwvc")));
        driver1.FindElement(By.Name("fwvc")).SendKeys(code);
        driver1.FindElement(By.Name("fwvc")).Submit();
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/div/table/tbody/tr[2]/td[2]/form[1]/span[2]/input[1]")));
        driver1.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/div/table/tbody/tr[2]/td[2]/form[1]/span[2]/input[1]")).Click();
        driver1.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/div/table/tbody/tr[2]/td[2]/form[1]/input[2]")).Click();
    }

    private static void createGmailModule(IWebDriver driver1, string phone)
    {
        int time = 5000;
        string mail = ReadRandomLineOfFile("usaname.txt");
        System.Threading.Thread.Sleep(1000);
        mail += ReadRandomLineOfFile("usaname.txt") + Path.GetRandomFileName().Replace(".", "");
        string pass = Path.GetRandomFileName().Replace(".", "");

        WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(20));
        string pvas = File.ReadLines("pvas.txt").First();
        driver1.Navigate().GoToUrl("https://accounts.google.com/signUp?service=mail");
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("FirstName")).SendKeys(ReadRandomLineOfFile("usaname.txt"));
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("LastName")).SendKeys(ReadRandomLineOfFile("usaname.txt"));
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("GmailAddress")).SendKeys(mail);
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("Passwd")).SendKeys(pass);
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("PasswdAgain")).SendKeys(pass);
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("BirthDay")).SendKeys("12");
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("BirthYear")).SendKeys("1990");
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("RecoveryPhoneNumber")).SendKeys(phone);
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("SkipCaptcha")).Click();
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("TermsOfService")).Click();
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/form/div[5]/fieldset/label[1]/span/div/div[1]")).Click();
        System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressCreateGmail.exe");
        cmd.WaitForExit();
        //driver1.FindElement(By.Id(":7")).Click();
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div[1]/div/form/div[6]/label/div/div")).Click();
        cmd = System.Diagnostics.Process.Start("pressCreateGmail.exe");
        cmd.WaitForExit();
        //driver1.FindElement(By.Id(":f")).Click();
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("TermsOfService")).Submit();

        //sang trang 2 confirm sdt
        System.Threading.Thread.Sleep(time);
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("signupidvinput")));
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.Id("signupidvmethod-sms")).Click();
        System.Threading.Thread.Sleep(time);
        driver1.FindElement(By.XPath("/html/body/div[1]/div[2]/form/div[2]/input")).Click();

        WriteLinePostingLog(mail + "@gmail.com\t" + pass);
    }

    private static string verifyConfirmation()
    {
        string code = "";
        while (code == "")
        {
            MyThread10.getMailGoogle(1, 0, 2);
            //load link confirm len ff
            var alinks = File.ReadAllLines("socks.txt");
            try
            {
                alinks = File.ReadAllLines("mailConfirm1.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            foreach (string link in alinks)
                if (File.ReadLines("pvas.txt").First().Contains(link.Split('\t')[0]))
                {
                    code = link.Split('\t')[1];
                    break;
                }
        }
        return code;
    }

    public static void WriteLinePostingLog(string content = "", string filewrite = "adsLog.txt")
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filewrite, true))
        {

            file.WriteLine(content);

        }
    }

    public static void PostingProcessMyAccount(IWebDriver driver, IWebDriver driver1, int index)
    {
        bool us = true;
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        string pvas = File.ReadLines("pvas.txt").Skip(index - 1).First();
        WritePostingLog(pvas.Split('\t')[2]);
        WritePostingLog(pvas.Split('\t')[3]);

        try
        {

            //driver.Navigate().GoToUrl("http://craigslist.org/");
            if (us)
            {
                //driver.Navigate().GoToUrl("http://houston.craigslist.org");
                var elements = pvas.Split('\t');
                foreach (string element in elements)
                {
                    if (element.Contains("http://"))
                    {
                        driver.Navigate().GoToUrl(element);
                        break;
                    }
                }
            }
            else
            {
                driver.Navigate().GoToUrl("http://vietnam.craigslist.org");
            }
            WritePostingLog(driver.Url);
            driver.FindElement(By.LinkText("my account")).Click();
        }
        catch (Exception)
        {
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);
            return;
        }

        try
        {
            //login account
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("inputEmailHandle")));
            //driver.FindElement(By.Id("inputEmailHandle")).SendKeys("NellieSuttonXR@outlook.com");
            //driver.FindElement(By.Id("inputPassword")).SendKeys("fdSJBR9475");
            driver.FindElement(By.Id("inputEmailHandle")).SendKeys(pvas.Split('\t')[2]);
            driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[3]);
            driver.FindElement(By.Id("inputPassword")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            // select the drop down list
            var education = driver.FindElement(By.Name("areaabb"));
            //create select element object 
            var selectElement = new SelectElement(education);

            //select by value
            if (us)
            {
                selectElement.SelectByValue("hou");
            }
            // select by text
            //selectElement.SelectByText("HighSchool");
            education.Submit();

            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(1, 10);

            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[7]")).Click();
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label["+Numrd+"]")).Click();

            //Numrd = rd.Next(1, 40);
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[2]")).Click();


        }
        catch (Exception)
        {
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            cmd.WaitForExit();
            return;
        }
        try
        {
            if (us)
            {
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }


        try
        {
            //input adds content
            string zip = GetZipcodeOfProxyByHttpRequest();
            if (zip.Contains("N/A"))
            {
                zip = "59000";
            }
            driver.FindElement(By.Id("postal_code")).SendKeys(zip);
            //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
            //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            //string textBody = "<pre>                                                                                                                                                                                                              ";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
            //textBody += "</pre>";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(167, 255);

            GetRandomAds(driver1);
            string title = File.ReadAllText("adstitle.txt");
            //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
            string textBody = File.ReadAllText("adsbody.txt");
            //string title = "&#" + Numrd + ";" + "&#" + Numrd + ";" + "&#" + Numrd + ";" + " ";
            //title += File.ReadAllText("adstitle.txt");
            //title += " " + "&#" + Numrd + ";" + "&#" + Numrd + ";" + "&#" + Numrd + ";";

            //string textBody = File.ReadAllText("adsbody.txt");
            //string textBody = "&#" + Numrd + ";" + "&#" + Numrd + ";" + "&#" + Numrd + ";" + " ";
            //textBody += File.ReadAllText("adsbody.txt");
            //textBody += " " + "&#" + Numrd + ";" + "&#" + Numrd + ";" + "&#" + Numrd + ";";
            //textBody += "<pre>                                                                                                                                                                                                              ";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
            //textBody += "</pre>";
            WritePostingLog(title);
            driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
            driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);
            if (us)
            {
                driver.FindElement(By.Id("wantamap")).Click();
            }
            driver.FindElement(By.Id("PostingBody")).Submit();

            //publish
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
        }
        catch (Exception e)
        {
            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            Console.WriteLine("{0} Second exception caught.", e);
        }

    }

    public static void PostingProcessPost2Classsified(IWebDriver driver, IWebDriver driver1, int index, string mucpost)
    {
        bool us = true;
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        string pvas = File.ReadLines("pvas.txt").Skip(index - 1).First();
        WritePostingLog(pvas.Split('\t')[2]);
        WritePostingLog(pvas.Split('\t')[3]);
        string city = "";
        string zipcode = "";

        driver.Navigate().GoToUrl("http://ip-score.com");
        try
        {
            //file.WriteLine(File.ReadLines("socks.txt").Skip(i).First() + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[3]/span[2]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[4]/span")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[5]/span")).Text); 
            zipcode = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[1]/p[4]")).Text;
            zipcode = zipcode.Replace("ZIP: ", "");
            zipcode = zipcode.Replace(".", "");
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            if (us)
            {
                //driver.Navigate().GoToUrl("http://houston.craigslist.org");
                var elements = pvas.Split('\t');
                foreach (string element in elements)
                {
                    if (element.Contains("http://"))
                    {
                        city = element.Split('.')[0].Split('/').Last();
                        driver.Navigate().GoToUrl("http://" + city + ".craigslist.org/");
                        break;
                    }
                }
            }
            else
            {
                driver.Navigate().GoToUrl("http://vietnam.craigslist.org");
            }
            WritePostingLog(driver.Url);
            driver.FindElement(By.LinkText("post to classifieds")).Click();
        }
        catch (Exception)
        {
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);
            return;
        }

        try
        {
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[7]")).Click();
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label["+Numrd+"]")).Click();

            //Numrd = rd.Next(1, 40);
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + mucpost + "]")).Click();
        }
        catch (Exception)
        {
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            cmd.WaitForExit();
            return;
        }

        while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
        {
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[2]")).Click();
        }

        try
        {
            string email = pvas.Split('\t')[2];
            if (!email.Contains("@")) email += "@gmail.com";
            driver.FindElement(By.Id("FromEMail")).SendKeys(email);
            driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);

            //input adds content
            //string zip = GetZipcodeOfProxyByHttpRequest();
            //if (zip.Contains("N/A"))
            //{
            //    zip = "59000";
            //}
            driver.FindElement(By.Id("postal_code")).SendKeys(zipcode);
            //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
            //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            //string textBody = "<pre>                                                                                                                                                                                                              ";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
            //textBody += "</pre>";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

            //GetRandomAds(driver1);
            string title = File.ReadAllText("adstitle.txt").Split('|')[Numrd];
            //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
            Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
            string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];

            WritePostingLog(title);
            driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);
            driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            cmd.WaitForExit();

            if (us)
            {
                driver.FindElement(By.Id("wantamap")).Click();
            }
            driver.FindElement(By.Id("contact_name")).Submit();

            //publish
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
        }
        catch (Exception e)
        {
            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            Console.WriteLine("{0} Second exception caught.", e);
        }

    }

    public static void PostTutor(IWebDriver driver, IWebDriver driver1, int index, string mucpost)
    {
        bool us = true;
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        string pvas = File.ReadLines("pvas.txt").Skip(index - 1).First();
        WritePostingLog(pvas.Split('\t')[2]);
        WritePostingLog(pvas.Split('\t')[3]);
        string city = "";
        string zipcode = pvas.Split('\t')[8];

        //driver.Navigate().GoToUrl("http://ip-score.com");
        //try
        //{
        //    //file.WriteLine(File.ReadLines("socks.txt").Skip(i).First() + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[3]/span[2]")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[4]/span")).Text + "\t" + driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div[2]/div/div[2]/ul/li[5]/span")).Text); 
        //    zipcode = driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[1]/div/div[1]/p[4]")).Text;
        //    zipcode = zipcode.Replace("ZIP: ", "");
        //    zipcode = zipcode.Replace(".", "");
        //    if (zipcode.Contains("N"))
        //        zipcode = pvas.Split('\t')[8];
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine("{0} Second exception caught.", e);
        //}

        try
        {
            if (us)
            {
                //driver.Navigate().GoToUrl("http://houston.craigslist.org");
                var elements = pvas.Split('\t');
                foreach (string element in elements)
                {
                    if (element.Contains("http://"))
                    {
                        city = element.Split('.')[0].Split('/').Last();
                        driver.Navigate().GoToUrl("http://" + city + ".craigslist.org/");
                        //driver.Navigate().GoToUrl("http://" + city + ".craigslist.org/");
                        break;
                    }
                }
            }
            else
            {
                driver.Navigate().GoToUrl("http://vietnam.craigslist.org");
            }
            WritePostingLog(driver.Url);
            driver.FindElement(By.LinkText("post to classifieds")).Click();
        }
        catch (Exception)
        {
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);
            return;
        }

        try
        {
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[10]")).Click();
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label["+Numrd+"]")).Click();

            //Numrd = rd.Next(1, 40);
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[12]")).Click();
        }
        catch (Exception)
        {
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            cmd.WaitForExit();
            return;
        }

        while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
        {
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[2]")).Click();
            }
            catch (Exception e)
            {
                string email = pvas.Split('\t')[2];
                if (!email.Contains("@")) email += "@gmail.com";
                driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[3]);
                driver.FindElement(By.Id("inputPassword")).Submit();
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }

        try
        {
            string email = pvas.Split('\t')[2];
            if (!email.Contains("@")) email += "@gmail.com";
            try
            {
                driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            //input adds content
            //string zip = GetZipcodeOfProxyByHttpRequest();
            //if (zip.Contains("N/A"))
            //{
            //    zip = "59000";
            //}
            driver.FindElement(By.Id("postal_code")).SendKeys(zipcode);
            //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
            //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            //string textBody = "<pre>                                                                                                                                                                                                              ";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
            //textBody += "</pre>";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

            //GetRandomAds(driver1);
            string title = File.ReadAllText("adstitle.txt").Split('|')[Numrd];
            //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
            Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
            string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];
            string phone = "Cell no: 706 801 7213";
            textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

            WritePostingLog(title);
            driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);
            driver.FindElement(By.Id("PostingTitle")).Clear();
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("postingtitle.exe");
            cmd.WaitForExit();
            driver.FindElement(By.Id("PostingTitle")).SendKeys(pvas.Split('\t')[4].Split(',')[0]);
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            //driver.FindElement(By.Id("wantamap")).Click();
            driver.FindElement(By.Id("contact_name")).Submit();
            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            //publish
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
        }
        catch (Exception e)
        {
            //driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
            Console.WriteLine("{0} Second exception caught.", e);
        }
    }

    public static void PostTutorOutlookModule(string pvas, int pauseForPhone = 0)
    {
    Restart:
        //Delete(All) ie file
        System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
        cmd.WaitForExit();

        IWebDriver driver = null;
        string proxy = "";

        InternetExplorerOptions options = new InternetExplorerOptions();
        if (int.Parse(pvas.Split('\t')[6]) != 77)
        {
            proxy = ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt");
            Proxy proxy1 = new Proxy();
            proxy1.SocksProxy = proxy;
            options.Proxy = proxy1;
        }
        driver = new InternetExplorerDriver(options);

        //FirefoxProfileManager profileManager = new FirefoxProfileManager();
        //FirefoxProfile profile = profileManager.GetProfile("default");
        //if (int.Parse(pvas.Split('\t')[6]) != 77)
        //{
        //    ChangeSockFirefox(profile, ReadProxyAtLine(int.Parse(pvas.Split('\t')[4]), "socks.txt")); 
        //}
        //driver = new FirefoxDriver(profile);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        //neu renew duoc 2 ads, ko post moi
        //if (RenewAds(pvas, driver, proxy,options) == 2)
        //{
        //    driver.Quit();
        //    ResetProxySockEntireComputer();
        //    return;
        //}

        //post moi
        try
        {
            //                  driver.Navigate().GoToUrl("http://craigslist.org");

            //driver.FindElement(By.LinkText("post to classifieds")).Click();
            driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
            if (driver.Url.Contains(".cn"))
            {
                driver.Navigate().GoToUrl(driver.Url + "?lang=en&cc=us");
            }
            driver.FindElement(By.LinkText("post to classifieds")).Click();
            Random rd = new Random();
            while (!driver.FindElement(By.TagName("body")).Text.Contains("please choose a category"))
            {
                driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[10]/input")).Click();
            }
            driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[12]/input")).Click();
        }
        catch (Exception e)
        {
            //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            //cmd.WaitForExit();
            //return;
            Console.WriteLine("{0} Second exception caught.", e);
        }

        int pass = 0;
        while (true)
        {
            if (pass == 1) break;
            try
            {
                while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
                {
                    if (driver.FindElement(By.TagName("body")).Text.Contains("post to classifieds"))
                    {
                        driver.Quit();
                        goto Restart;
                    }

                    try
                    {
                        driver.FindElement(By.XPath("//*[@id=\"pagecontainer\"]/section/form/blockquote/label[1]/input")).Click();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                    try
                    {
                        driver.FindElement(By.XPath("/html/body/article/section/form/table/tbody/tr/td[1]/blockquote/label[1]")).Click();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                    try
                    {
                        string email = pvas.Split('\t')[0];
                        if (!email.Contains("@")) email += "@gmail.com";
                        driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                        driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                        driver.FindElement(By.Id("inputPassword")).Submit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }

                    //accept tos
                    try
                    {
                        driver.FindElement(By.XPath("/html/body/form[1]/input[4]")).Submit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
                pass = 1;
            }
            catch (System.Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }

        try
        {
            string email = pvas.Split('\t')[0];
            if (!email.Contains("@")) email += "@gmail.com";
            try
            {
                driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            //accept tos
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
            FillAdsInformation(pvas, driver);
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        try
        {

            driver.FindElement(By.Id("wantamap")).Click();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
    publish:
        try
        {

            driver.FindElement(By.Id("contact_name")).Submit();
            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            //driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            //publish
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        string codebody = "";
        try
        {
            codebody = driver.FindElement(By.TagName("body")).Text;
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
            goto publish;
        }
        while (!codebody.Contains("Thanks for posting with us. We really appreciate it!"))
        {
            int ok = 0;
        //lay mail confirm ve may
        getmail:
            ResetProxySockEntireComputer();
            int stt = MyThread10.getMailConfirm(pvas, 0, 1);

            if (proxy != "")
            {
                SetSockEntireComputer(proxy);
            }
            //load link confirm len ff
            var links = File.ReadAllLines("socks.txt");
            try
            {
                links = File.ReadAllLines("mailConfirm" + stt + ".txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
                goto getmail;
            }
            foreach (string link in links)
                if (codebody.Contains(link.Split('\t')[0]) && driver.Url.Contains(link.Split('\t')[1].Split('/')[4]))
                {
                    driver.Navigate().GoToUrl(link.Split('\t')[1]);
                    //breakpoint
                    if (pauseForPhone == 1)
                    {
                        ok = 1;
                    }
                    else ok = 1;
                    break;
                }
            if (ok == 1)
            {
                break;
            }
        }

        if (proxy != "")
        {
            SetSockEntireComputer(proxy);
        }
        //click link confirm & accept ToS        
        try
        {
            driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        WritePostingLog(pvas, driver, proxy);
        driver.Quit();
        ResetProxySockEntireComputer();
    }

    private static int RenewAds(string pvas, IWebDriver driver, string proxy,InternetExplorerOptions options=null)
    {
        //renew
        Start:
        try
        {
            driver.Navigate().GoToUrl(pvas.Split('\t')[2].Split('.')[0] + ".craigslist.org");
            if (driver.Url.Contains(".cn"))
            {
                driver.Navigate().GoToUrl(driver.Url + "?lang=en&cc=us");
            }
            driver.FindElement(By.LinkText("my account")).Click();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        //login
        try
        {
            while (!driver.FindElement(By.TagName("body")).Text.Contains("Showing all postings"))
            {
                try
                {
                    string email = pvas.Split('\t')[0];
                    if (!email.Contains("@")) email += "@gmail.com";
                    driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                    driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                    driver.FindElement(By.Id("inputPassword")).Submit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
            driver= restartIE(driver,1,options);
            goto Start;
                    }

            driver.Navigate().GoToUrl("https://accounts.craigslist.org/login/home?filter_active=active&filter_cat=0&show_tab=postings");
        string[] separatingChars1 = { "Active" };
        //return 2;
        string[] separatingChars2 = { " page:" };
        string info = driver.FindElement(By.TagName("body")).Text;

        int renew = 0;
        try
        {
            foreach (string info1 in info.Split(separatingChars1, System.StringSplitOptions.RemoveEmptyEntries).Reverse())
            {
                if (!info1.Contains(pvas.Split('\t')[0]))
                {
                    string link;
                    if (info1.Contains(" page:"))
                    {
                        link = info1.Split(separatingChars2, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    else
                        link = info1;
                    driver.Navigate().GoToUrl("post.craigslist.org/manage/" + link.Substring(link.Length - 12, 10));
                    //click renew
                    if (driver.FindElement(By.TagName("body")).Text.Contains("This will move your posting to the top of the list."))
                    {
                        for (int i = 6; i >= 0; i--)
                        {
                            try
                            {
                                driver.FindElement(By.XPath("/html/body/article/section/div[1]/table/tbody/tr[" + i + "]/td[1]/div/form/input[3]")).Click();
                                renew += 1;
                                WritePostingLog(pvas, driver, proxy);
                                if (renew == 2)
                                {
                                    return renew;
                                }
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("{0} Second exception caught.", e);
                            }
                        }
                    }
                    //driver.Navigate().GoToUrl("https://accounts.craigslist.org/login/home?filter_active=active&filter_cat=0&show_tab=postings");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        return renew;
        //end renew
    }

    private static void FillAdsInformation(string pvas, IWebDriver driver)
    {
        int Numrd;
        Random rd = new Random();
        Fillzipcode(pvas, driver);

        Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);
        string title = removeSpecialChar(ReadRandomLineOfFile("adstitle.txt"));
        Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
        string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];

        string pre = "", post = "";
        //for (int i = 0; i < rd.Next(20); i++)
        //{
        //    pre += Path.GetRandomFileName().Replace(".", "") + " ";
        //}
        for (int i = 0; i < rd.Next(30); i++)
        {
            post += Path.GetRandomFileName().Replace(".", "") + " ";
        }
        textBody = textBody.Replace("xxx", MyThread10.getphoneNum());
        textBody = textBody + "\n" + post;
        driver.FindElement(By.Id("PostingBody")).SendKeys(textBody);
        
           title = title + " " + Path.GetRandomFileName().Replace(".", "");
        driver.FindElement(By.Id("PostingTitle")).SendKeys(title);
        //driver.FindElement(By.Id("PostingTitle")).Clear();
        //cmd = System.Diagnostics.Process.Start("postingtitle.exe");
        //cmd.WaitForExit();
    }

    private static string removeSpecialChar(string title)
    {
        return Regex.Replace(title, "[^a-zA-Z0-9% ]", " ").Replace("  ", " ").ToLower();
    }

    private static void WritePostingLog(string pvas, IWebDriver driver = null, string proxy = "")
    {
        string curpath = Directory.GetCurrentDirectory();
        string folder = String.Format("{0}\\postinglog", curpath);
        string link = @"postinglog\" + getTimeNowVietnam().Date.ToShortDateString().Replace('/', '-') + ".txt";

        // If the folder is not existed, create it.
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        if (!File.Exists(link))
        {
            File.Create(link);
        }

        try
        {
            Demo w = new Demo();
            w.WriteToFileThreadSafe(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t" + getTimeNowVietnam() + "\t" + pvas.Split('\t')[0] + "\t" + proxy, link);
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
    }

    private static void Fillzipcode(string pvas, IWebDriver driver)
    {
        if (!pvas.Split('\t')[2].Contains("craigslist.ca"))
        {
            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(10000, 99999);
            driver.FindElement(By.Id("postal_code")).SendKeys("90011");
        }
        else
            driver.FindElement(By.Id("postal_code")).SendKeys("a0a");
    }

    public static void PostCanadaToimage(IWebDriver driver, int index, string mucpost)
    {
        bool us = true;
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        string pvas = File.ReadLines("pvas.txt").Skip(index - 1).First();

        driver.Navigate().GoToUrl(pvas.Split('\t')[2]);
        driver.FindElement(By.LinkText("post")).Click();

        try
        {
            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(6, 10);
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[6]")).Click();
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();

            //Numrd = rd.Next(1, 40);
            //driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[" + Numrd + "]")).Click();
            if (Numrd == 6)
            {
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[21]")).Click();
            }
            else
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[20]")).Click();
        }
        catch (Exception e)
        {
            //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            //cmd.WaitForExit();
            //return;
            Console.WriteLine("{0} Second exception caught.", e);
        }

        while (!driver.FindElement(By.TagName("body")).Text.Contains("ok for others to contact you about other services, products or commercial interests"))
        {
            try
            {
                driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[1]")).Click();
            }
            catch (Exception e)
            {
                string email = pvas.Split('\t')[0];
                if (!email.Contains("@")) email += "@gmail.com";
                driver.FindElement(By.Id("inputEmailHandle")).SendKeys(email);
                driver.FindElement(By.Id("inputPassword")).SendKeys(pvas.Split('\t')[1]);
                driver.FindElement(By.Id("inputPassword")).Submit();
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }

        try
        {
            string email = pvas.Split('\t')[0];
            if (!email.Contains("@")) email += "@gmail.com";
            try
            {
                driver.FindElement(By.Id("FromEMail")).SendKeys(email);
                driver.FindElement(By.Id("ConfirmEMail")).SendKeys(email);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            //input adds content
            //string zip = GetZipcodeOfProxyByHttpRequest();
            //if (zip.Contains("N/A"))
            //{
            //    zip = "59000";
            //}
            driver.FindElement(By.Id("postal_code")).SendKeys("A0A");
            //driver.FindElement(By.Id("postal_code")).SendKeys(File.ReadLines("adsProxyWithZipcode.txt").Skip(index - 1).First().Split(':')[2]);
            //string title = RandomReplaceCharacter(File.ReadAllText("adstitle.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            //string textBody = "<pre>                                                                                                                                                                                                              ";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsjunk.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));
            //textBody += "</pre>";
            //textBody += RandomReplaceCharacter(File.ReadAllText("adsbody.txt"), int.Parse(File.ReadAllLines("config.txt")[4].Split('=')[1]));

            int Numrd;
            Random rd = new Random();
            Numrd = rd.Next(0, File.ReadAllText("adstitle.txt").Split('|').Count() - 1);

            //GetRandomAds(driver1);
            string title = File.ReadAllText("adstitle.txt").Split('|')[Numrd];
            title = RandomUppercaseCharacter(title, 2);
            //title = ReadRandomLineOfFile("specialSymbol.txt") + title + ReadRandomLineOfFile("specialSymbol.txt");
            Numrd = rd.Next(0, File.ReadAllText("adsbody.txt").Split('|').Count() - 1);
            string textBody = File.ReadAllText("adsbody.txt").Split('|')[Numrd];
            textBody = RandomUppercaseCharacter(textBody, 1);
            //string phone = "Cell no: 706 801 7213";
            //textBody = textBody + "\n" + phone.Replace(" ", ReadRandomLineOfFile("specialSymbol.txt"));

            Numrd = rd.Next(6, 20);
            string subtring = "";
            for (int i = 0; i < Numrd; i++)
            {
                subtring += ReadRandomLineOfFile("specialSymbol.txt");
            }
            driver.FindElement(By.Id("PostingBody")).SendKeys(subtring + textBody + "\n" + subtring);
            //driver.FindElement(By.Id("PostingTitle")).Clear();
            //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("postingtitle.exe");
            //cmd.WaitForExit();
            Numrd = rd.Next(1, 4);
            subtring = "";
            for (int i = 0; i < Numrd; i++)
            {
                subtring += ReadRandomLineOfFile("specialSymbol.txt");
            }
            driver.FindElement(By.Id("PostingTitle")).SendKeys(ReadRandomLineOfFile("specialSymbol.txt") + title + subtring);
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            driver.FindElement(By.Id("wantamap")).Click();
            driver.FindElement(By.Id("contact_name")).Submit();

            //upload imageg
            driver.FindElement(By.Id("plupload")).Click();

            string link = @"C:\Users\newpc\Desktop\hinh macbook\";
            var files = new DirectoryInfo(link).GetFiles();
            int index1 = new Random().Next(0, files.Length);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
            {
                file.Write(link + files[index1].Name);
            }
            System.Diagnostics.Process cmd;
            cmd = System.Diagnostics.Process.Start("uploadImage.exe");
            cmd.WaitForExit();
            System.Threading.Thread.Sleep(60000);

            driver.FindElement(By.XPath("/html/body/article/section/form/button")).Submit();
            ////publish
            driver.FindElement(By.XPath("/html/body/article/section/div[1]/form/button")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        //skip map
        try
        {
            driver.FindElement(By.ClassName("skipmap")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
    }

    public static void CreatePvas(IWebDriver driver, IWebDriver driver1, int index, string mucpost)
    {
        bool us = true;
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        string pvas = File.ReadLines("pvas.txt").Skip(index - 1).First();
        WritePostingLog(pvas.Split('\t')[2]);
        WritePostingLog(pvas.Split('\t')[3]);
        string city = "";

        try
        {
            if (us)
            {
                var elements = pvas.Split('\t');
                foreach (string element in elements)
                {
                    if (element.Contains("http://"))
                    {
                        city = element.Split('.')[0].Split('/').Last();
                        driver.Navigate().GoToUrl("http://" + city + ".craigslist.org/");
                        break;
                    }
                }
            }
            else
            {
                driver.Navigate().GoToUrl("http://vietnam.craigslist.org");
            }
            WritePostingLog(driver.Url);
            driver.FindElement(By.LinkText("post to classifieds")).Click();
        }
        catch (Exception)
        {
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Escape);
            return;
        }

        try
        {
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[10]")).Click();
            driver.FindElement(By.XPath("/html/body/article/section/form/blockquote/label[12]")).Click();
            driver.FindElement(By.Id("emailAddress")).SendKeys(pvas.Split('\t')[2]);
            driver.FindElement(By.Id("emailAddress")).Submit();
        }
        catch (Exception)
        {
            System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("pressEscape.exe");
            cmd.WaitForExit();
            return;
        }
    }

    public static string GetZipcodeOfProxyByHttpRequest()
    {
        //string MyProxyHostString = "";
        //int MyProxyPort = 0;
        //if (new FileInfo("proxy.txt").Length != 0)
        //{
        //    string proxy = File.ReadLines("proxy.txt").First();

        //    MyProxyHostString = proxy.Split(':')[0];
        //    MyProxyPort = int.Parse(proxy.Split(':')[1]);
        //}

        //Demo w = new Demo();
        string info = "78578";
        try
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create("http://ip-score.com");
            //if (new FileInfo("proxy.txt").Length != 0)
            //{
            //    request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
            //}
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            string filename = "tempo";
            filename = filename.Replace(":", " ");
            filename = "temp\\output" + filename + ".txt";
            File.WriteAllText(filename, responseFromServer);
            info = File.ReadLines(filename).Skip(148).First();
            //info = info.Remove(0, info.IndexOf("png\">") + 6);
            //info = info.Replace("</p>							<p><em>State:</em> ", "\t");
            //info = info.Replace("</p>							<p><em>City:</em> ", "\t");
            info = info.Replace("							<p><em>ZIP:</em> ", "");
            info = info.Replace("</p>", "");
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
            //{
            //    try
            //    {
            //        file.WriteLine(proxy + "\t" + info);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Second exception caught.", e);
            //    }
            //}   
            //w.WriteToFileThreadSafe(proxy + "\t" + info, "proxyWithCityHttpRequest.txt");
        }
        catch (Exception e)
        {
            //w.WriteToFileThreadSafe(proxy, "proxyWithCityHttpRequest.txt");
            Console.WriteLine("{0} Second exception caught.", e);
            return info;
        }
        return info;
    }

    public static string GetIPOfProxyByHttpRequest()
    {
        string MyProxyHostString = "";
        int MyProxyPort = 0;
        if (new FileInfo("proxy.txt").Length != 0)
        {
            string proxy = File.ReadLines("proxy.txt").First();

            MyProxyHostString = proxy.Split(':')[0];
            MyProxyPort = int.Parse(proxy.Split(':')[1]);
        }

        //Demo w = new Demo();
        string info = "N/A";
        try
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create("http://ip-score.com");
            if (new FileInfo("proxy.txt").Length != 0)
            {
                request.Proxy = new WebProxy(MyProxyHostString, MyProxyPort);
            }
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            string filename = "tempo";
            filename = filename.Replace(":", " ");
            filename = "temp\\output" + filename + ".txt";
            File.WriteAllText(filename, responseFromServer);
            info = File.ReadLines(filename).Skip(145).First();
            info = info.Remove(0, info.IndexOf("hlink\">") + 7);
            info = info.Remove(info.IndexOf("</a>"));
            //info = info.Replace("</p>							<p><em>State:</em> ", "\t");
            //info = info.Replace("</p>							<p><em>City:</em> ", "\t");
            //info = info.Replace("</p>", "");
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
            //{
            //    try
            //    {
            //        file.WriteLine(proxy + "\t" + info);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Second exception caught.", e);
            //    }
            //}   
            //w.WriteToFileThreadSafe(proxy + "\t" + info, "proxyWithCityHttpRequest.txt");
        }
        catch (Exception e)
        {
            //w.WriteToFileThreadSafe(proxy, "proxyWithCityHttpRequest.txt");
            Console.WriteLine("{0} Second exception caught.", e);
            return info;
        }
        return info;
    }



    public static void ClickConfirmationLinkInGmailTesting()
    {
        ResetProxySockEntireComputer();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        //FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        FirefoxProfile profile = profileManager.GetProfile("posting");
        IWebDriver driver = new FirefoxDriver(profile);
        //ClickConfirmationLinkInGmail(driver);
    }

    public static void ClickConfirmationLinkInGmail(IWebDriver driver, IWebDriver driver1)
    {
        string pvas = File.ReadLines("pvas.txt").First();
        WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(60));
        try
        {
            //vao email lay link confirmation
            driver1.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?service=mail&continue=https://mail.google.com/mail/#identifier");
            //driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t').Last());
            driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t')[2]);
            driver1.FindElement(By.Id("Email")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("Passwd")));
            if (pvas.Split('\t').Last().Contains("hodinhlam911"))
            {
                driver1.FindElement(By.Id("Passwd")).SendKeys("Dangtinthue1234567");
            }
            else
            {
                driver1.FindElement(By.Id("Passwd")).SendKeys(pvas.Split('\t')[3]);
            }
            driver1.FindElement(By.Id("Passwd")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        driver1.Navigate().GoToUrl("https://mail.google.com/mail/#inbox");
        try
        {
            //filter mail
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("gbqfq")));
            driver1.FindElement(By.Id("gbqfq")).SendKeys("craigslist - automated message, do not reply");
            driver1.FindElement(By.Id("gbqfb")).Click();

            //mo mail dau tien
            System.Threading.Thread.Sleep(5000);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div/div/div/div/div[1]/div/div/div/div/div[2]/div/div[1]")));
            IWebElement body = driver1.FindElement(By.TagName("body"));
            body.SendKeys(Keys.ArrowDown);
            body.SendKeys(Keys.Enter);
        }
        catch (Exception e)
        {
            //filter mail
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("sbq")));
            driver1.FindElement(By.Id("sbq")).SendKeys("craigslist - automated message, do not reply");
            driver1.FindElement(By.Id("sbq")).Submit();
            //driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr[2]/td/table/tbody/tr/td[1]/input[2]")).Click();

            //mo mail dau tien
            System.Threading.Thread.Sleep(5000);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]/td[3]/a/span")));
            driver1.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]/td[3]/a/span")).Click();

            Console.WriteLine("{0} Second exception caught.", e);
        }

        //click link confirm & accept ToS
        System.Threading.Thread.Sleep(5000);
        var linkConfirm = driver1.FindElements(By.PartialLinkText("https://post.craigslist.org"));
        try
        {
            driver.Navigate().GoToUrl(linkConfirm.Last().Text);
            driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        WriteLinePostingLog(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t\t" + getTimeNowVietnam(), "adsLogLink.txt");
        //WritePostingLog("-----------------------------------");
    }

    public static void ClickConfirmationHodinhlam(IWebDriver driver, IWebDriver driver1)
    {
        string pvas = File.ReadLines("pvas.txt").First();
        WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(40));
        try
        {
            //vao email lay link confirmation
            driver1.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin?service=mail&continue=https://mail.google.com/mail/#identifier");
            //driver1.FindElement(By.Id("Email")).SendKeys(pvas.Split('\t').Last());
            driver1.FindElement(By.Id("Email")).SendKeys("hodinhlam911");
            driver1.FindElement(By.Id("Email")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        try
        {
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.Id("Passwd")));
            driver1.FindElement(By.Id("Passwd")).SendKeys("Dangtinthue1234567");
            driver1.FindElement(By.Id("Passwd")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
        //mo mail dau tien
        driver1.Navigate().GoToUrl("https://mail.google.com/mail/?ui=html&zy=h");
        driver1.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]/td[3]/a")).Click();
        var linkConfirm = driver1.FindElements(By.PartialLinkText("https://post.craigslist.org"));

        //click link confirm & accept ToS        
        try
        {
            driver.Navigate().GoToUrl(linkConfirm.Last().Text);
            driver.FindElement(By.XPath("/html/body/article/section/section[1]/form[1]/button")).Click();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        WriteLinePostingLog(driver.FindElements(By.PartialLinkText("htm")).First().Text + "\t\t" + getTimeNowVietnam(), "adsLogLink.txt");
        //WritePostingLog("-----------------------------------");
    }

    public static void ConfirmPvas(IWebDriver driver, IWebDriver driver1)
    {
        string pvas = File.ReadLines("pvas.txt").First();
        WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(60));
        //mo mail dau tien
        driver1.Navigate().GoToUrl("https://mail.google.com/mail/?ui=html&zy=h");
        driver1.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]/td[3]/a")).Click();
        var linkConfirm = driver1.FindElements(By.PartialLinkText("https://accounts.craigslist.org"));

        //click link confirm & accept ToS
        try
        {
            driver.Navigate().GoToUrl(linkConfirm.Last().Text);
            //ghi password pvas
            driver.FindElement(By.Id("p1")).SendKeys(pvas.Split('\t')[3]);
            driver.FindElement(By.Id("p2")).SendKeys(pvas.Split('\t')[3]);
            driver.FindElement(By.Id("p2")).Submit();
            driver.FindElement(By.LinkText("Continue creating your post")).Click();
            driver.FindElement(By.XPath("/html/body/form[1]/input[4]")).Click();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }
    }

    public static void FlagSockModuleNoPR(System.Windows.Forms.Label label4)
    {
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("socks.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();
        ReplacePR();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        CheckLinkDie(label4);
        File.Delete("links.txt");
        File.Copy("survivalLinks.txt", "links.txt");

        for (int j = 1; j < 1800; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);

            for (int i = 0; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                if (i != 0)
                {
                    //doc line socks thu $i de nap vao firefox
                    string proxy = ReadProxyAtLine(i, "socks.txt");
                    //set socks for firefox
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
                    {
                        file.Write(proxy);
                    }
                    cmd = System.Diagnostics.Process.Start("ChangeSockFirefox.exe");
                    cmd.WaitForExit();
                    //SetSockEntireComputer(proxy);
                }

                var links = ReadnLinks();

                //load link into 4tabs of firefox
                cmd = System.Diagnostics.Process.Start("load4Links.exe");
                cmd.WaitForExit();

                //wait 15s <= read from file config

                //click flag
                int tab = 1;

                if (i != 0)
                {
                    foreach (string link in links)
                    {
                        try
                        {
                            IWebElement body = driver.FindElement(By.TagName("body"));
                            body.SendKeys(Keys.Control + tab);
                            driver.SwitchTo().DefaultContent();
                            tab += 1;
                            driver.FindElement(By.PartialLinkText("prohibited")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                }

                //get link song
                int[] live = new int[5];
                tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        string a = driver.FindElement(By.TagName("body")).Text;
                        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                            live[tab - 1] = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //deleteDeadLink 
                if (i != 0)
                {
                    DeleteDeadLink(driver, links, live);
                }

                //dem so link con lai
                label4.Invoke((System.Windows.Forms.MethodInvoker)(() => label4.Text = File.ReadLines("links.txt").Count().ToString()));
            }

            ReplaceOverloadLink(label4, driver, j);

            DeleteFirstLineSock("socks.txt");
            ReplacePR();
            //ResetProxySockEntireComputer();
            //cmd = System.Diagnostics.Process.Start("clickPR.exe");
            //cmd.WaitForExit();
        }
    }

    private static void ReplaceOverloadLink(System.Windows.Forms.Label label4, IWebDriver driver, int j)
    {
        if (j % 4 == 0)
        {
            Replace4Link(driver);
        }

        if (j % 4 == 0 && File.ReadLines("links.txt").Count() < 4)
        {
            ResetProxySockEntireComputer();
            File.Delete("links.txt");
            File.Copy("survivalLinks.txt", "links.txt");
            CheckLinkDie(label4);
            File.Delete("links.txt");
            File.Copy("survivalLinks.txt", "links.txt");
        }
    }

    public static void fakePRSoftware(System.Windows.Forms.Label label4)
    {
        System.Diagnostics.Process cmd;

        //Process[] processNames = Process.GetProcessesByName("firefox");

        //foreach (Process item in processNames)
        //{
        //    item.Kill();
        //}

        IWebDriver driver = null;


        try
        {
            System.Uri uri = new System.Uri("http://localhost:7055/hub");
            driver = new RemoteWebDriver(uri, DesiredCapabilities.Firefox());

            //get link from firefox to file before close firefox
            cmd = System.Diagnostics.Process.Start("get4linktofile.exe");
            cmd.WaitForExit();

            driver.Quit();
            Console.WriteLine("Executed on remote driver");
        }
        catch (Exception)
        {
            //driver = new FirefoxDriver();
            Console.WriteLine("Executed on New FireFox driver");
        }
        finally
        {
            ReplacePR();

            FirefoxProfileManager profileManager = new FirefoxProfileManager();
            FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
            driver = RestartFirefox(profile);
        }


        //ResetProxySockEntireComputer();

        System.Threading.Thread.Sleep(3000);
        ////load link into 4tabs of firefox
        cmd = System.Diagnostics.Process.Start("load4Links.exe");
        cmd.WaitForExit();
    }

    // temp => sock,sock1
    public static void Nearest5SocksCityStep2(System.Windows.Forms.Label label4 = null)
    {
        IWebDriver driver = null;


        try
        {
            System.Uri uri = new System.Uri("http://localhost:7055/hub");
            driver = new RemoteWebDriver(uri, DesiredCapabilities.Firefox());
            Console.WriteLine("Executed on remote driver");
        }
        catch (Exception)
        {
            //driver = new FirefoxDriver();
            Console.WriteLine("Executed on New FireFox driver");
        }

        var links = File.ReadLines("temp.txt");
        File.Delete("socks.txt");
        File.Delete("socks1.txt");
        foreach (string link in links)
        {
            if (link != "N/A")
            {
                try
                {
                    driver.Navigate().GoToUrl(link);
                    System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("get5sock.exe");
                    cmd.WaitForExit();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("socks.txt", true))
                    {
                        try
                        {
                            file.WriteLine(File.ReadLines("socktemp.txt").First());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("socks1.txt", true))
                    {
                        try
                        {
                            file.WriteLine(File.ReadLines("socktemp.txt").First() + '\t' + driver.FindElement(By.XPath("/html/body/font[2]/table/tbody/tr/td/table/tbody/tr/td/table/tbody/tr[5]/td[2]")).Text);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }
            else
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("socks.txt", true))
                {
                    try
                    {
                        file.WriteLine("N/A");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
        }
        driver.Quit();
    }

    // => links
    public static void getlinkCL(string city)
    {
        File.Delete("temp.txt");
        File.Delete("links.txt");
        WebRequest request = WebRequest.Create("http://" + city + ".craigslist.org/search/lss");
        // If required by the server, set the credentials.
        request.Credentials = CredentialCache.DefaultCredentials;
        // Get the response.
        WebResponse response = request.GetResponse();
        // Display the status.
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        // Get the stream containing content returned by the server.
        Stream dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.
        string responseFromServer = reader.ReadToEnd();
        // Display the content.
        //Console.WriteLine(responseFromServer);
        //string filename = thr.Name.Replace(".", " ");
        //filename = filename.Replace(":", " ");
        string filename = "temp.txt";
        File.WriteAllText(filename, responseFromServer);
        string info = "";
        foreach (string item in File.ReadLines(filename))
        {
            if (item.Contains("<p class=\"row\" data-pid="))
            {
                info = item;
                break;
            }
        }
        string[] separatingChars1 = { "</time> <a href=\"" };
        string[] separatingChars2 = { "\" data-id=\"" };
        foreach (string info1 in info.Split(separatingChars1, System.StringSplitOptions.RemoveEmptyEntries))
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("links.txt", true))
            {
                try
                {
                    if (info1.Split(separatingChars2, System.StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                    {
                        file.WriteLine("http://" + city + ".craigslist.org" + info1.Split(separatingChars2, System.StringSplitOptions.RemoveEmptyEntries)[0]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }
        }
        //info = info.Remove(0, info.IndexOf("png\">") + 6);
        //info = info.Replace("</p>							<p><em>State:</em> ", "\t");
        //info = info.Replace("</p>							<p><em>City:</em> ", "\t");
        //info = info.Replace("</p>", "");
        //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
        //{
        //    try
        //    {
        //        file.WriteLine(proxy + "\t" + info);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("{0} Second exception caught.", e);
        //    }
        //}   
    }

    public static void configFakePRSoftware(System.Windows.Forms.Label label4)
    {
        string path = Reader.ReadFirefoxProfile();

        CleanDirectory(path);

        //DirectoryCopyExample.DirectoryCopy("temp", @"C:\Windows\Temp", true);
        DirectoryCopyExample.DirectoryCopy("a5sidtqu.newprofile", path, true);
        System.Diagnostics.Process cmd;
        cmd = System.Diagnostics.Process.Start("firefox.exe");
        System.Threading.Thread.Sleep(15000);
    }

    private static void CleanDirectory(string path)
    {
        System.IO.DirectoryInfo di = new DirectoryInfo(path);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }

    class DirectoryCopyExample
    {
        //static void Main()
        //{
        //    // Copy from the current directory, include subdirectories.
        //    DirectoryCopy(".", @".\temp", true);
        //}

        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }

    class Reader
    {
        public static string ReadFirefoxProfile()
        {
            string apppath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string mozilla = System.IO.Path.Combine(apppath, "Mozilla");

            bool exist = System.IO.Directory.Exists(mozilla);

            if (exist)
            {

                string firefox = System.IO.Path.Combine(mozilla, "firefox");

                if (System.IO.Directory.Exists(firefox))
                {
                    string prof_file = System.IO.Path.Combine(firefox, "profiles.ini");

                    bool file_exist = System.IO.File.Exists(prof_file);

                    if (file_exist)
                    {
                        StreamReader rdr = new StreamReader(prof_file);

                        string resp = rdr.ReadToEnd();

                        string[] lines = resp.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        string location = lines.First(x => x.Contains("Path=") && x.Contains("newprofile")).Split(new string[] { "=" }, StringSplitOptions.None)[1];

                        string prof_dir = System.IO.Path.Combine(firefox, location);

                        return prof_dir;
                    }
                }
            }
            return "";
        }
    }


    public static void ChromeFlagSockModuleNoPR()
    {
        DeleteFirstLineSock("socks.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();

        IWebDriver driver = new ChromeDriver(@"C:\Users\Administrator\Downloads\");
        //driver.Navigate().GoToUrl("http://www.joecolantonio.com/HpSupport.html");
        //IWebElement myField = driver.FindElement(By.Id("tools"));
        //myField.SendKeys("QTP10");

        //FirefoxProfileManager profileManager = new FirefoxProfileManager();
        //FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        //IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            //driver = RestartFirefox(profile);
            driver = new ChromeDriver(@"C:\Users\Administrator\Downloads\");

            for (int i = 1; i < 7; i++)
            {
                //changeUAFirefox
                //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                //cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                string proxy = ReadProxyAtLine(i, "socks.txt");
                SetSockEntireComputer(proxy);

                //set socks for firefox
                //ProcessStartInfo psi = new ProcessStartInfo("ChangeSockFirefox.exe", i.ToString())
                //{
                //    CreateNoWindow = true,
                //    WindowStyle = ProcessWindowStyle.Hidden,
                //    UseShellExecute = false,
                //    RedirectStandardOutput = true
                //};
                //var process = System.Diagnostics.Process.Start(psi);
                //process.WaitForExit();

                var links = ReadnLinks();

                //load link into 4tabs of firefox
                //cmd = System.Diagnostics.Process.Start("load4Links.exe");
                //cmd.WaitForExit();

                //load links
                int tab = 1;
                int[] live = new int[5];

                {
                    try
                    {
                        foreach (string link in links)
                        {
                            IWebElement body = driver.FindElement(By.TagName("body"));
                            body.SendKeys(Keys.Control + tab);
                            driver.SwitchTo().DefaultContent();
                            tab += 1;
                            driver.Navigate().GoToUrl(link);


                            driver.FindElement(By.PartialLinkText("prohibited")).Click();
                            System.Threading.Thread.Sleep(2000);


                            string a = driver.FindElement(By.TagName("body")).Text;
                            if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                                if (driver.Url.Contains("craigslist.org") && !driver.Url.Contains("post.craigslist.org"))
                                    live[tab - 1] = 1;
                        }
                    }
                    catch (Exception e)
                    {
                        for (int i1 = 0; i1 < 4; i1++)
                        {
                            live[i1] = 1;
                        }
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //wait 15s <= read from file config
                //System.Threading.Thread.Sleep(int.Parse(File.ReadAllLines("config.txt")[2].Split('=')[1])*1000);

                Console.WriteLine(proxy);




                //deleteDeadLink 
                DeleteDeadLink(driver, links, live);
            }

            DeleteFirstLineSock("proxy.txt");
            ReplacePR();
        }
    }

    public class MyThread1
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string link = thr.Name;
            link = link.Replace("?lang=en&cc=us", "");
            string filename = link.Split('/')[link.Split('/').Count() - 1];
            Demo w = new Demo();
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create(link);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access. 
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                //string filename = thr.Name.Replace(".", " ");
                //filename = filename.Replace(":", " ");
                filename = "temp\\output" + filename + ".txt";
                File.WriteAllText(filename, responseFromServer);
                //string info = File.ReadLines(filename).Skip(145).First() + File.ReadLines(filename).Skip(146).First() + File.ReadLines(filename).Skip(147).First();
                //info = info.Remove(0, info.IndexOf("png\">") + 6);
                //info = info.Replace("</p>							<p><em>State:</em> ", "\t");
                //info = info.Replace("</p>							<p><em>City:</em> ", "\t");
                //info = info.Replace("</p>", "");
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
                //{
                //    try
                //    {
                //        file.WriteLine(proxy + "\t" + info);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}   
                if (!File.ReadAllText(filename).Contains("This posting has been flagged for removal."))
                {
                    w.WriteToFileThreadSafe(link, "survivalLinks.txt");
                }
            }
            catch (Exception e)
            {
                w.WriteToFileThreadSafe(link, "proxyWithCityHttpRequest.txt");
                Console.WriteLine("{0} Second exception caught.", e);
            }

            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();
        }
    }

    public class MyThread9
    {

        public void Thread1()
        {
            Thread thr = Thread.CurrentThread;
            string link = thr.Name;

            string filename = link.Split('/')[link.Split('/').Count() - 1];
            Demo w = new Demo();
            try
            {
                // Create a request for the URL. 
                WebRequest request = WebRequest.Create(link);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                //string filename = thr.Name.Replace(".", " ");
                //filename = filename.Replace(":", " ");
                filename = "temp\\output" + filename + ".txt";
                File.WriteAllText(filename, responseFromServer);
                if (!File.ReadAllText(filename).Contains("This posting has been flagged for removal."))
                {
                    w.WriteToFileThreadSafe(link, "survivalLinks.txt");
                    //title
                    string[] separatingChars1 = { "<span class=\"postingtitletext\">" };
                    string[] separatingChars2 = { "<span class=\"js-only banish-unbanish\">" };
                    string info = responseFromServer.Split(separatingChars1, System.StringSplitOptions.RemoveEmptyEntries)[1].Split(separatingChars2, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    info = info.Replace("<small>", "");
                    info = info.Replace("</small>", "");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("adstitle.txt", true))
                    {
                        try
                        {
                            file.WriteLine(info.Split('(')[0] + "|");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }

                    //body
                    string[] separatingChars3 = { "<section id=\"postingbody\">" };
                    string[] separatingChars4 = { "</section>" };
                    info = responseFromServer.Split(separatingChars3, System.StringSplitOptions.RemoveEmptyEntries)[1].Split(separatingChars4, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    info = Regex.Replace(info, @"<[^>]+>|&nbsp;", "").Trim();
                    info = info.Replace("show contact info", "");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("adsbody.txt", true))
                    {
                        try
                        {
                            file.WriteLine(info + "|");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }
        }
    }

    public class MyThread2
    {

        public void Thread2()
        {
            Thread thr = Thread.CurrentThread;
            string proxy = thr.Name;
            Demo w = new Demo();
            try
            {
                //lay dia diem trong url
                string location = proxy.Split('/')[2].Split('.')[0];
                //lay title ads
                string title = proxy.Split('\t')[4];
                if (title.Trim().Length == 0) title = "child porn";
                title = title.Replace("(", "");
                title = title.Replace(")", "");

                string link = location + '|' + proxy.Split('\t')[1] + '|' + proxy.Split('\t')[0] + '|' + title;

                int Numrd;
                Random rd = new Random();
                Numrd = rd.Next(1, 10000);
                string filename = Numrd.ToString();

                // Create a request for the URL. 
                //http://vietnam.craigslist.org/search/sss?query=The%20pallid%20sturgeob%20%28Scrphinhynchus%20albus%29%20is%20an%20enda4gerQd%201pecie0&sort=rel
                string link1 = "http://" + location + ".craigslist.org/search/sss?query=" + title;
                WebRequest request = WebRequest.Create(link1);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);
                //string filename = thr.Name.Replace(".", " ");
                //filename = filename.Replace(":", " ");
                filename = "temp\\output" + filename + ".txt";
                File.WriteAllText(filename, responseFromServer);
                //string info = File.ReadLines(filename).Skip(145).First() + File.ReadLines(filename).Skip(146).First() + File.ReadLines(filename).Skip(147).First();
                //info = info.Remove(0, info.IndexOf("png\">") + 6);
                //info = info.Replace("</p>							<p><em>State:</em> ", "\t");
                //info = info.Replace("</p>							<p><em>City:</em> ", "\t");
                //info = info.Replace("</p>", "");
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter("proxyWithCityHttpRequest.txt", true))
                //{
                //    try
                //    {
                //        file.WriteLine(proxy + "\t" + info);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("{0} Second exception caught.", e);
                //    }
                //}   
                if (!File.ReadAllText(filename).Contains("no results"))
                {
                    w.WriteToFileThreadSafe(link, "adsLive.txt");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Second exception caught.", e);
            }

            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();
        }
    }

    //links.txt => survivalLinks.txt
    public static void CheckLinkDie(System.Windows.Forms.Label label3)
    {
        File.Delete("survivalLinks.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("links.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("links.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread1[] thr = new MyThread1[1000];
            Thread[] tid = new Thread[1000];

            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("links.txt").Count() / block)
                num = File.ReadAllLines("links.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = File.ReadLines("links.txt").Skip(i).First().Split('\t')[0];
                thr[i] = new MyThread1();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    //links.txt => adstitle.txt, adsbody.txt
    public static void getTrueAds(System.Windows.Forms.Label label3)
    {
        File.Delete("adstitle.txt");
        File.Delete("adsbody.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("links.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("links.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread9[] thr = new MyThread9[1000];
            Thread[] tid = new Thread[1000];

            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("links.txt").Count() / block)
                num = File.ReadAllLines("links.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = ReadProxyAtLine(i + 1, "links.txt");
                thr[i] = new MyThread9();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread1));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    //adsLoginput.txt => adsLive.txt
    public static void CheckAdsLive(System.Windows.Forms.Label label3)
    {
        File.Delete("adsLive.txt");
        int block = 50;
        for (int j = 0; j <= File.ReadAllLines("adsLoginput.txt").Count() / block; j++)
        {
            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "checking..." + j + "/" + File.ReadAllLines("adsLoginput.txt").Count() / block));
            ResetProxySockEntireComputer();

            MyThread2[] thr = new MyThread2[1000];
            Thread[] tid = new Thread[1000];

            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("temp\\");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }

            int num;
            if (j == File.ReadAllLines("adsLoginput.txt").Count() / block)
                num = File.ReadAllLines("adsLoginput.txt").Count();
            else
                num = (j + 1) * block;
            for (int i = j * block; i < num; i++)
            {
                string proxy = File.ReadLines("adsLoginput.txt").Skip(i).First();
                thr[i] = new MyThread2();
                tid[i] = new Thread(new ThreadStart(thr[i].Thread2));
                tid[i].Name = proxy;
                tid[i].Start();
            }

            for (int i = j * block; i < num; i++)
            {
                tid[i].Join();
            }

            label3.Invoke((System.Windows.Forms.MethodInvoker)(() => label3.Text = "finish check !!!"));
        }
    }

    public static void ReplacePR()
    {
        ChangeTimezone();
        ChangeDns(ReadRandomLineOfFile("dnslist.txt"));
        //DeleteDataFirefox();
        ResetProxySockEntireComputer();
    }

    public static string ReadRandomLineOfFile(string file)
    {
        string[] lines = File.ReadAllLines(file); //i hope that the file is not too big
        Random rand = new Random();
        return lines[rand.Next(lines.Length)];
    }

    public static void DeleteDataFirefox()
    {
        System.Diagnostics.Process.Start("deleteDataFirefox.exe");
    }

    public static void DeleteDeadLink(IWebDriver driver, IEnumerable<string> links, int[] live)
    {
        var theRemain = File.ReadLines("links.txt").Skip(4);
        int tab = 1;

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            foreach (string link in links)
            {
                if (live[tab] == 1)
                    file.WriteLine(link);
                tab += 1;
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
        {
            foreach (string line in theRemain)
            {
                file.WriteLine(line);
            }
        }

        File.Delete("links.txt");
        File.Copy("temp.txt", "links.txt");
    }

    public static void Replace4Link(IWebDriver driver)
    {
        var links = File.ReadLines("links.txt").Take(4);
        var theRemain = File.ReadLines("links.txt").Skip(4);

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            foreach (string line in theRemain)
            {
                file.WriteLine(line);
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
        {
            foreach (string link in links)
            {
                file.WriteLine(link);
            }
        }

        File.Delete("links.txt");
        File.Copy("temp.txt", "links.txt");
    }

    public static void SaveSurvivalLinkWhenCheckingLinkDie(IWebDriver driver, IEnumerable<string> links, int[] live)
    {
        var theRemain = File.ReadLines("links.txt").Skip(4);
        int tab = 1;

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("survivalLinks.txt", true))
        {
            foreach (string link in links)
            {
                if (live[tab] == 1)
                    file.WriteLine(link);
                tab += 1;
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            foreach (string line in theRemain)
            {
                file.WriteLine(line);
            }
        }

        File.Delete("links.txt");
        File.Copy("temp.txt", "links.txt");
    }

    public static IWebDriver RestartFirefox(FirefoxProfile profile)
    {
        profile.SetPreference("signon.autologin.proxy", true);
        profile.SetPreference("network.proxy.type", 5);
        profile.SetPreference("general.useragent.override", "initial");
        ChangeUAFirefox(profile);

        //disable the cache
        profile.SetPreference("browser.cache.disk.enable", false);
        profile.SetPreference("browser.cache.memory.enable", false);
        profile.SetPreference("browser.cache.offline.enable", false);
        profile.SetPreference("network.http.use-cache", false);

        IWebDriver driver = new FirefoxDriver(profile);
        for (int i = 1; i < 4; i++)
        {
            IWebElement body = driver.FindElement(By.TagName("body"));
            body.SendKeys(Keys.Control + 't');
        }
        //System.Diagnostics.Process cmd = System.Diagnostics.Process.Start("load4Links.exe");
        //cmd.WaitForExit();
        return driver;
    }

    public static string ReadProxyAtLine(int p, string file)
    {
        string proxy = File.ReadLines(file).Skip(p - 1).First();
        string[] aproxy = proxy.Split(':');
        return aproxy[0] + ':' + aproxy[1];
    }

    public static IEnumerable<string> ReadnLinks(int numlink = 4)
    {
        var first4 = File.ReadAllLines("links.txt").Take(numlink);
        return first4;
    }

    public static IEnumerable<string> Read1Links()
    {
        var first4 = File.ReadLines("links.txt").Take(1);
        return first4;
    }

    public static void DeleteFirstLineSock(string proxyFile, int numberDelete = 6)
    {
        //read file numberDelete lines
        var first6 = File.ReadLines(proxyFile).Take(numberDelete);
        var theRemain = File.ReadLines(proxyFile).Skip(numberDelete);

        //copy to others file
        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt"))
        {
            foreach (string line in theRemain)
            {
                file.WriteLine(line);
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter("temp.txt", true))
        {
            foreach (string line in first6)
            {
                file.WriteLine(line);
            }
        }

        File.Delete(proxyFile);
        File.Copy("temp.txt", proxyFile);
    }

    public static IWebDriver OpenFirefox()
    {
        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile("posting");
        IWebDriver driver = new FirefoxDriver(profile);
        return driver;
    }

    //input proxy.txt
    //output passwordExportData.xml => import firefox
    public static void BuildCredenticalXmlFileForFirefox()
    {
        string[] template = File.ReadAllLines("passwordExportTemplate.xml");

        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("passwordExportData.xml"))
        {
            foreach (string line in template)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                if (!line.Contains("/"))
                {
                    file.WriteLine(line);
                }
            }
        }

        string[] data = File.ReadAllLines("proxy.txt");

        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("passwordExportData.xml", true))
        {
            foreach (string line in data)
            {
                string[] aLine = line.Split(':');
                file.WriteLine("<entry host=\"moz-proxy://" + aLine[0] + ":" + aLine[1] + "\" user=\"" + aLine[2] + "\" password=\"" + aLine[3] + "\" formSubmitURL=\"\" httpRealm=\"login\" userFieldName=\"\" passFieldName=\"\"/>");
            }
        }

        using (System.IO.StreamWriter file =
    new System.IO.StreamWriter("passwordExportData.xml", true))
        {
            foreach (string line in template)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                if (line.Contains("/"))
                {
                    file.WriteLine(line);
                }
            }
        }
    }

    public static IWebDriver ConfigFirefoxForNewComputer()
    {
        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);

        //install addon
        profile.AddExtension("mozrepl@hyperstruct.net.xpi");
        profile.AddExtension("PasswordExporter.xpi");
        profile.AddExtension("savedpasswordeditor@daniel.dawson.xpi");
        profile.SetPreference("signon.autologin.proxy", true);

        //disable the cache
        profile.SetPreference("browser.cache.disk.enable", false);
        profile.SetPreference("browser.cache.memory.enable", false);
        profile.SetPreference("browser.cache.offline.enable", false);
        profile.SetPreference("network.http.use-cache", false);

        IWebDriver driver = new FirefoxDriver(profile);
        return driver;
    }

    public static void ChangeDns(string dns)
    {
        string[] aDns = dns.Split('|');

        ProcessStartInfo psi = new ProcessStartInfo("dns.bat", aDns[0] + " " + aDns[1])
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        var process = System.Diagnostics.Process.Start(psi);
        process.WaitForExit();
    }

    public static void ChangeTimezone()
    {
        //string timezone = "central standard time,mountain standard time,pacific standard time";
        string timezone = "pacific standard time";
        //string timezone = "Eastern Standard Time";
        string[] aTimezone = timezone.Split(',');
        int Numrd;
        Random rd = new Random();
        Numrd = rd.Next(0, aTimezone.Count());

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C tzutil /s \"" + aTimezone[Numrd] + "\"";
        process.StartInfo = startInfo;
        process.Start();
    }

    public static void ChangeSockFirefox(FirefoxProfile profile, string sock)
    {
        string[] aSocks = sock.Split(':');
        profile.SetPreference("network.proxy.type", 1);
        profile.SetPreference("network.proxy.socks", aSocks[0]);
        profile.SetPreference("network.proxy.socks_port", int.Parse(aSocks[1]));
        profile.SetPreference("network.proxy.http", "");
        profile.SetPreference("network.proxy.http_port", "");
    }

    public static void ChangeProxyFirefox(FirefoxProfile profile, string sock)
    {
        string[] aSocks = sock.Split(':');
        profile.SetPreference("network.proxy.type", 1);
        profile.SetPreference("network.proxy.http", aSocks[0]);
        profile.SetPreference("network.proxy.http_port", int.Parse(aSocks[1]));
        profile.SetPreference("network.proxy.ssl", aSocks[0]);
        profile.SetPreference("network.proxy.ssl_port", int.Parse(aSocks[1]));
        profile.SetPreference("network.proxy.socks", "");
        profile.SetPreference("network.proxy.socks_port", "");
    }

    public static void SetProxyEntireComputer(string proxy)
    {
        //RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
        //registry.SetValue("ProxyEnable", 1);
        //registry.SetValue("ProxyServer", proxy);
        ////http=192.161.225.253:80;https=192.161.225.253:80;ftp=192.161.225.253:80
        ////registry.SetValue("ProxyServer", "http="+proxy+";https="+proxy+";ftp="+proxy);
        ////registry.SetValue("ProxyServer", "http=" + proxy + ";https=" + proxy);
        //// These lines implement the Interface in the beginning of program 
        //// They cause the OS to refresh the settings, causing IP to realy update
        //settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        //refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

        ProcessStartInfo psi = new ProcessStartInfo("setProxyEntireComputer.exe", proxy)
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        var process = System.Diagnostics.Process.Start(psi);
        process.WaitForExit();
    }

    public static void SetSockEntireComputer(string proxy)
    {
        RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
        registry.SetValue("ProxyEnable", 1);
        registry.SetValue("ProxyServer", "socks=" + proxy);
        // These lines implement the Interface in the beginning of program 
        // They cause the OS to refresh the settings, causing IP to realy update
        settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
    }

    public static void ResetProxySockEntireComputer()
    {
        RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
        registry.SetValue("ProxyEnable", 0);
        // These lines implement the Interface in the beginning of program 
        // They cause the OS to refresh the settings, causing IP to realy update
        settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
        refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
    }

    public static void ChangeUAFirefox(FirefoxProfile profile)
    {
        var userAgent = ReadRandomLineOfFile("useragentswitcher.txt");
        profile.SetPreference("general.useragent.override", userAgent);
    }

    public static void ChangeSocksEscortAndIPBinding(IWebDriver driver)
    {
        System.Diagnostics.Process cmd;
        ResetProxySockEntireComputer();

        //FirefoxProfileManager profileManager = new FirefoxProfileManager();
        //FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        //IWebDriver driver = new FirefoxDriver(profile);

        //changeSockEscort
        cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
        cmd.WaitForExit();

        try
        {
            driver.Navigate().GoToUrl("http://whoer.net/");
            string ip = driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/div[1]/div/div[1]/div/strong[2]")).Text;

            driver.Navigate().GoToUrl("https://account.fineproxy.org/ip/");
            driver.FindElement(By.Id("username")).SendKeys("AMR168897");
            driver.FindElement(By.Id("password")).SendKeys("X9dG89FCts");
            driver.FindElement(By.Id("submit")).Click();
            driver.Navigate().GoToUrl("https://account.fineproxy.org/ip/");
            driver.FindElement(By.Name("ipbind")).Clear();
            driver.FindElement(By.Name("ipbind")).SendKeys(ip);
            driver.FindElement(By.Name("ipbind")).Submit();

            driver.Navigate().GoToUrl("https://billing.proxyelite.ru/ip/");
            driver.FindElement(By.Id("username")).SendKeys("AMR168899");
            driver.FindElement(By.Id("password")).SendKeys("qSVUCgSerE");
            driver.FindElement(By.Id("submit")).Click();
            driver.Navigate().GoToUrl("https://billing.proxyelite.ru/ip/");
            driver.FindElement(By.Name("ipbind")).Clear();
            driver.FindElement(By.Name("ipbind")).SendKeys(ip);
            driver.FindElement(By.Name("ipbind")).Submit();
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Second exception caught.", e);
        }

        System.Threading.Thread.Sleep(60000);
    }

    public static void testModule(IWebDriver driver11, System.Windows.Forms.Label label3, System.Windows.Forms.RadioButton radioButton1, System.Windows.Forms.RadioButton radioButton2, System.Windows.Forms.GroupBox groupbox2)
    {
        SetSockEntireComputer("1.1.1.1:1");

    }

    public static System.DateTime getTimeNowVietnam()
    {
        var indianTime = TimeZoneInfo.ConvertTime(DateTime.Now,
             TimeZoneInfo.FindSystemTimeZoneById("se asia standard Time"));
        return indianTime;
    }

    public static void sockConnectHttpRequest()
    {
        Chilkat.Http http = new Chilkat.Http();
        http.SocksHostname = "185.101.70.134";
        http.SocksPort = 1085;
        ////http.SocksUsername = "myProxyLogin";
        ////http.SocksPassword = "myProxyPassword";
        ////  Set the SOCKS version to 4 or 5 based on the version
        ////  of the SOCKS proxy server:
        http.SocksVersion = 5;
        bool success1;
        ////  Any string unlocks the component for the 1st 30-days.
        success1 = http.UnlockComponent("Anything for 30-day trial");
        if (success1 != true)
        {
            Console.WriteLine(http.LastErrorText);
            return;
        }
        ////  Send the HTTP GET and return the content in a string.
        string html;
        html = http.QuickGetStr("http://www.ip-score.com/");
        File.WriteAllText("testdemo1.txt", html);
    }

    public static void FlagProxyModuleNoPR2(string value)
    {
        int countSocks = 0;
        System.Diagnostics.Process cmd;
        DeleteFirstLineSock("proxy.txt");
        //ko can deleteDataFirefox() vi tao profile temp, chi can profile goc empty la duoc
        ResetProxySockEntireComputer();
        ReplacePR();

        FirefoxProfileManager profileManager = new FirefoxProfileManager();
        FirefoxProfile profile = profileManager.GetProfile(File.ReadAllLines("config.txt")[3].Split('=')[1]);
        IWebDriver driver = new FirefoxDriver(profile);

        for (int j = 1; j < 180; j++)
        {
            //output j de tien theo doi
            Console.WriteLine("lan lap " + j);

            driver.Quit();
            driver = RestartFirefox(profile);
            ChangeSocksEscortAndIPBinding(driver);

            for (int i = 0; i < 7; i++)
            {
                //changeUAFirefox
                cmd = System.Diagnostics.Process.Start("ChangeUAFirefox.exe");
                cmd.WaitForExit();

                //doc line socks thu $i de nap vao firefox
                if (i != 0)
                {
                    //string proxy = ReadProxyAtLine(i, "proxy.txt");
                    //SetProxyEntireComputer(proxy);

                    //set proxy firefox
                    string proxy = ReadProxyAtLine(i, "proxy.txt");
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("toAutoitData.txt"))
                    {
                        file.Write(proxy);
                    }
                    cmd = System.Diagnostics.Process.Start("ChangeProxyFirefox.exe");
                    cmd.WaitForExit();
                }

                var links = ReadnLinks();

                //load link into 4tabs of firefox
                cmd = System.Diagnostics.Process.Start("load4Links.exe");
                cmd.WaitForExit();

                //wait 15s <= read from file config

                //click flag
                int tab = 1;

                if (i != 0)
                {
                    foreach (string link in links)
                    {
                        try
                        {
                            IWebElement body = driver.FindElement(By.TagName("body"));
                            body.SendKeys(Keys.Control + tab);
                            driver.SwitchTo().DefaultContent();
                            tab += 1;
                            driver.FindElement(By.PartialLinkText("prohibited")).Click();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Second exception caught.", e);
                        }
                    }
                }

                //get link song
                int[] live = new int[5];
                tab = 1;

                foreach (string link in links)
                {
                    try
                    {
                        IWebElement body = driver.FindElement(By.TagName("body"));
                        body.SendKeys(Keys.Control + tab);
                        driver.SwitchTo().DefaultContent();
                        tab += 1;
                        string a = driver.FindElement(By.TagName("body")).Text;
                        if (!driver.FindElement(By.TagName("body")).Text.Contains("This posting has been flagged for removal."))
                            live[tab - 1] = 1;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }

                //deleteDeadLink 
                if (i != 0)
                {
                    DeleteDeadLink(driver, links, live);
                }
            }

            if (j % 2 == 0)
            {
                Replace4Link(driver);
            }

            DeleteFirstLineSock("proxy.txt");
            ReplacePR();
            if (countSocks < int.Parse(value) - 1)
            {
                cmd = System.Diagnostics.Process.Start("changeSockEscort.exe");
                cmd.WaitForExit();
                countSocks++;
            }
            else
            {
                cmd = System.Diagnostics.Process.Start("resetSockEscort.exe");
                cmd.WaitForExit();
                countSocks = 0;
            }
        }
    }

    public static void titleToLink(string city = "")
    {
        File.Delete("links.txt");
        string filename = "sample.xml";
        string info = File.ReadAllText(filename);

        foreach (string info1 in info.Split(' '))
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("links.txt", true))
            {
                try
                {
                    if (info1.Contains("craigslist"))
                    {
                        file.WriteLine(info1.Replace("Target=", "").Replace("\"", ""));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Second exception caught.", e);
                }
            }
        }
    }
}

