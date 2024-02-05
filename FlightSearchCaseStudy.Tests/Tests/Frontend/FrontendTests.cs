using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace FlightSearchCaseStudy.Tests.Tests.Frontend
{
    public class FrontendTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _appUrl = "https://flights-app.pages.dev/";

        public FrontendTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl(_appUrl);
        }

        public void Dispose()
        {
            Thread.Sleep(3000);
            _driver.Quit();
        }

        [Fact]
        public void AyniSehirSecilmesi()
        {
            string sehir = "Istanbul";

            AramaYap(sehir, sehir);

            Assert.True(HataMesaji("Aynı şehir seçilemez"));
        }

        [Fact]
        public void FarkliSehirSecilmesi()
        {
            string fromSehir = "Istanbul";
            string toSehir = "Rome";

            AramaYap(fromSehir, toSehir);

            Assert.Equal(2, ListelenenUcusSayisi());
        }

        [Fact]
        public void FarkliSehirUcusBulunamadi()
        {
            string fromSehir = "Istanbul";
            string toSehir = "Paris";

            AramaYap(fromSehir, toSehir);

            Assert.True(HataMesaji("Bu iki şehir arasında uçuş bulunmuyor."));
        }

        [Fact]
        public void ListFoundXItems()
        {
            // Arrange
            string fromSehir = "Istanbul";
            string toSehir = "Los Angeles";

            // Act
            AramaYap(fromSehir, toSehir);
            int foundXItemsSayisi = FoundXItemsSayisiniAl();
            int listelenenUcusSayisi = ListelenenUcusSayisi();

            // Assert
            Assert.Equal(foundXItemsSayisi, listelenenUcusSayisi);
        }

        private void AramaYap(string fromSehir, string toSehir)
        {
            IWebElement fromInput = _driver.FindElement(By.CssSelector("input[id='headlessui-combobox-input-:Rq9lla:']"));
            IWebElement toInput = _driver.FindElement(By.CssSelector("input[id='headlessui-combobox-input-:Rqhlla:']"));

            // "From" seç
            fromInput.SendKeys(fromSehir);
            fromInput.SendKeys(Keys.Return);

            Thread.Sleep(1000);

            // "To" seç
            toInput.SendKeys(toSehir);
            toInput.SendKeys(Keys.Return);

            Thread.Sleep(1000);
        }

        private bool HataMesaji(string hataMesaji)
        {
            // Hata mesajının göründüğü elementi bul
            IWebElement errorMessageElementi = _driver.FindElement(By.CssSelector("div[class='error-message']"));

            // Hata mesajını kontrol et
            bool hataMesajiGorunuyor = errorMessageElementi.Text.Contains(hataMesaji);

            // Element bulunamazsa false döndür
            return hataMesajiGorunuyor;
        }


        private int ListelenenUcusSayisi()
        {
            IReadOnlyCollection<IWebElement> ucusItemleri = _driver.FindElements(By.CssSelector("ul.grid li"));
            return ucusItemleri.Count;
        }

        private int FoundXItemsSayisiniAl()
        {
            IWebElement foundItemsText = _driver.FindElement(By.CssSelector("p[class='mb-10']"));
            return int.Parse(foundItemsText.Text.Split(' ')[1]);
        }
    }
}
