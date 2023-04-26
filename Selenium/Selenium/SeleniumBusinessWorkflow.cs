namespace BusinesssSelenium.Selenium
{
    public class SeleniumBusinessWorkflow
    {
        SeleniumBrowserBusiness browser { get; set; }
        public SeleniumBusinessWorkflow(SeleniumBrowserBusiness browser)
        {
            this.browser = browser;
        }
    }
}
