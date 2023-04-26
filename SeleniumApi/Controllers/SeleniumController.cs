using BusinesssSelenium.Models;
using BusinesssSelenium.Selenium;
using Microsoft.AspNetCore.Mvc;
using static BusinesssSelenium.Selenium.SeleniumBrowserBusiness;

namespace SeleniumApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeleniumController : ControllerBase
    {
        [HttpPost("test")]
        public IActionResult test()
        {
            try
            {
                var result = new selenium().startBrowser();
                SeleniumBrowserBusiness sel = new();
                Model user = new()
                {
                    UserName = "emreayhan",
                    UserCode = "xxxxxxxxxxx",
                };
                sel.GoToMainPage(result);
                var data = sel.SetData(result, user);
                sel.SignIn(result);

                sel.QueryEnco(result, new EncoModel
                {
                    UserNameFirst = "emreayhan",
                    UserCode = "xxxxxxxxxx"
                });
                sel.ReadTable(result);
                result.Dispose();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
