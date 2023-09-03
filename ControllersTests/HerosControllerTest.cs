using System.Collections.Generic;
using ApiTuto;
using ApiTuto.Controllers;
using ApiTuto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ControllersTests;

public class HerosControllerTest
{
    [Fact]
    public async Task GetResturnDataAsync()
    {
        var herosProviderMock = new Moq.Mock<IHerosProvider>();


        var listeMoq = new List<Hero>
        {
            new Hero
            {
                id = 0,
                name = "testMoq"
            }
        };

        herosProviderMock.Setup(m => m.GetAllHeroes()).Returns(Task.FromResult<IEnumerable<IHero>>(listeMoq));

        var herosController = new HerosController(herosProviderMock.Object);



        var result = (List<Hero>)((JsonResult)(await herosController.Get())).Value;

        //Assert.NotEmpty(result);

        //Assert.True(result.Count == 1);

        Assert.Equal(listeMoq, result);


    }


    [Fact]
    public async Task GetUsesHerosprovider()
    {
        var herosProviderMock = new Moq.Mock<IHerosProvider>();

        var herosController = new HerosController(herosProviderMock.Object);

        var getResult = await herosController.Get();

        herosProviderMock.Verify(m => m.GetAllHeroes(), Moq.Times.Once() );


    }




    [Fact]
    public async Task GetHeroById()
    {
        var herosProviderMock = new Moq.Mock<IHerosProvider>();
        var index = 1;

        var listeMoq = new List<Hero>
        {
            new Hero
            {
                id = 0,
                name = "testMoq"
            },
            new Hero {
                id = 1,
                name = "testMoq1"
            }
        };
        herosProviderMock.Setup(m => m.GetAllHeroes()).Returns(Task.FromResult<IEnumerable<IHero>>(listeMoq));

        var herosController = new HerosController(herosProviderMock.Object);



        var result = await herosController.Get(index);

        //Assert.Equal(listeMoq.ElementAt(index).name, result); //TODO : a faire 


    }
}
