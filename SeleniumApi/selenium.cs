using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumApi
{
    
    public class selenium
    {
        IWebDriver driver;

        public IWebDriver startBrowser()
        {
            ChromeOptions options = new ChromeOptions();
            var proxy = new Proxy();
            if (Convert.ToBoolean(Environment.GetEnvironmentVariable("AUTO_PROXY")))
            {
                proxy.Kind = ProxyKind.AutoDetect;
                proxy.IsAutoDetect = true;

            }
            else
            {
                proxy.HttpProxy = "xx.xxx.xxx.x:8000";//ip:port
                proxy.SslProxy = "xx.xxx.xxx.x:8000";
                options.AddArgument("--proxy-server=xx.xxx.xxx.x:xxxx");//ip port

            }
            options.AddArgument("ignore-certificate-errors");
            driver = new ChromeDriver(options);
            return driver;
        }

        public void closeBrowser()
        {
            driver.Close();
        }
    }
}
