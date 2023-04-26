using BusinesssSelenium.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace BusinesssSelenium.Selenium
{
    public class SeleniumBrowserBusiness
    {
        public async Task GoToMainPage(IWebDriver driver)
        {
            driver.Navigate().GoToUrl(Environment.GetEnvironmentVariable("NAVIGATION_URL"));
        }
        public async Task SetCapthca(IWebDriver driver, string resolverText)
        {
            driver.FindElement(By.CssSelector("#deneme123")).SendKeys(resolverText);
        }
        public string SetData(IWebDriver driver, Model user, float width = 1)
        {
            try
            {

                driver.FindElement(By.CssSelector("#deneme_username")).SendKeys(user.UserName);
                driver.FindElement(By.CssSelector("#deneme_user_kod")).SendKeys(user.UserCode);
                var result = driver.ExecuteJavaScript<string>("var img=document.querySelector('" + "#deneme" + "') \n" +
                 "var canvas = document.createElement('canvas');\n" +
                 "canvas.width = img.width*'" + width + "';\n" +
                 "canvas.height = img.height;\n" +
                 "var ctx = canvas.getContext('2d');\n" +
                 "ctx.drawImage(img, 0, 0);\n" +
                 "var dataURL = canvas.toDataURL('image/png');\n" +
                 "dataURL = dataURL.replace('data:image/png;base64,', '');\n" +
                 "dataURL = dataURL.replace('data:image/jpg;base64,', '');\n" +
                 "return dataURL;\n");
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public (bool, string) SignIn(IWebDriver driver)
        {
            driver.FindElement(By.CssSelector("#deneme > button")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last().ToString());
            if (driver.ExecuteJavaScript<bool>("return document.body.contains(document.querySelector('#denemeUyari))"))
            {
                return (false, driver.FindElement(By.CssSelector("#denemeUyari")).Text);
            }
            return (true, string.Empty);
        }
        public void QueryEnco(IWebDriver driver, EncoModel encoModel)
        {
            driver.Navigate().GoToUrl("https://trendyol.com;");
            var userElement = driver.FindElement(By.Id("userSorgula_username"));
            userElement.Clear();
            userElement.SendKeys(encoModel.UserNameFirst);
            string userStatus = CheckUserStatus((UserStatusEnum)Enum.Parse(typeof(UserStatusEnum), encoModel.UserCode, true)).ToString();
            var userOptions = driver.FindElement(By.Id("denemesorgula2")).FindElements(By.TagName("option"));

            foreach (var item in userOptions)
            {

                if (item.GetAttribute("value").Equals(userStatus))
                {
                    item.Click();
                    break;
                }

            }
            driver.ExecuteJavaScript("usercodeDegisti('" + encoModel.UserCode + "')");
            driver.FindElement(By.Id("iduserKodu")).Click();
            var professionElement = driver.FindElement(By.Id("userInformation"));
            professionElement.Clear();
            professionElement.SendKeys(encoModel.UserCode);
            driver.ExecuteJavaScript("usercodeGetir()");
            driver.ExecuteJavaScript("usercodeDegisti('" + encoModel.UserCode + "')");
            driver.FindElement(By.Id("usercodesorgula_0")).Click();
        }
        public EncoInnerTableResult[] ReadTable(IWebDriver driver)
        {
            var result = driver.ExecuteJavaScript<object>
                (
                @"
                    var encoResultModel = [];
                    var tableElements = document.querySelectorAll('#dataTable')
                    for (var i = 1; i < tableElements.length; i++) {
                        var trElements = tableElements[i].firstElementChild.children
                        var tempObj = {}
                        for (var j = 0; j < trElements.length; j++) {
                            if (j == 0) {
                                tempObj['UserName'] = trElements[j].innerText
                            } else {
                                if (trElements[j].children[0].innerText == 'Mesajlar' || trElements[j].children.length < 2) {
                                    if (tempObj['mesaj'] == undefined) {
                                        tempObj['mesaj'] = trElements[j].children[0].innerText
                                    } else {
                                        tempObj['mesaj'] += ' ' + trElements[j].children[0].innerText
                                    }
                                } else {
                                    tempObj[trElements[j].children[0].innerText] = trElements[j].children[1].innerText
                                }
                            }
                        }
                        encoResultModel.push(tempObj);
                    }
                    return encoResultModel;
                "
                );
            var jsonString = JsonConvert.SerializeObject(result);
            var modelResult = JsonConvert.DeserializeObject<EncoInnerTableResult[]>(jsonString);
            return modelResult;
        }

        public void BatchGetAndRead(WebDriver driver, EncoModel encoModel)
        {
            var userElement = driver.FindElement(By.Id("userCodeSorgula"));
            userElement.Clear();
            userElement.SendKeys(encoModel.UserNameFirst);
            string userStatus = CheckUserStatus((UserStatusEnum)Enum.Parse(typeof(UserStatusEnum), encoModel.UserCode, true)).ToString();
            var userOptions = driver.FindElement(By.Id("userCodeSorgula")).FindElements(By.TagName("option"));

            foreach (var item in userOptions)
            {

                if (item.GetAttribute("value").Equals(userStatus))
                {
                    item.Click();
                    break;
                }

            }
        }

        private int CheckUserStatus(UserStatusEnum userStatus)
        {
            switch (userStatus)
            {
                case UserStatusEnum.UserName:
                    return 1;
                case UserStatusEnum.UserCodeTrue:
                    return 2;
                default:
                    return 2;
            }
        }
        public string RefreshCaptcha(WebDriver driver)
        {
            var model = new Model()
            {
                UserName = driver.FindElement(By.Id("user_username")).Text,
                UserCode = driver.FindElement(By.Id("user_kod")).Text,

            };
            driver.Navigate().Refresh();
            return SetData(driver, model);
        }

        public class EncoModel
        {
            public string UserNameFirst { get; set; }

            public string UserCode { get; set; }
        }
        public enum UserStatusEnum
        {
            UserName = 1,
            UserCodeTrue,
            UserCodeFalse
        }
        public class EncoInnerTableResult
        {
            [JsonProperty("UserName")]
            public string UserName { get; set; }

            [JsonProperty("UserCode")]
            public string UserCode { get; set; }
        }
    }
}
