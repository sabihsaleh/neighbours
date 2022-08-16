using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using neighbours.Models;
namespace Neighbours.Test;

public class HomepageTests
{
  ChromeDriver driver;
  [SetUp]
  public void Setup()
  {
    driver = new ChromeDriver();
  }
  [TearDown]
  public void TearDown()
  {
    driver.Quit();
  }
  [Test]
  public void LandingPage_ShowsWelcomeMessage()
  {
    driver.Navigate().GoToUrl("http://127.0.0.1:5123");
    IWebElement slogan = driver.FindElement(By.Id("slogan"));
    Assert.AreEqual("Where good neighbours become good friends", slogan.GetAttribute("innerHTML"));
  }
}