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


        var listeMoq = new List<Heros>
        {
            new Heros
            {
                id = 0,
                name = "testMoq"
            }
        };

        herosProviderMock.Setup(m => m.GetAllHeroes()).Returns(Task.FromResult<IEnumerable<Heros>>(listeMoq));

        var herosController = new HerosController(herosProviderMock.Object);



        var result = (List<Heros>)((JsonResult)(await herosController.Get())).Value;

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


        var listeMoq = new List<Heros>
        {
            new Heros
            {
                id = 0,
                name = "testMoq"
            },
            new Heros {
                id = 1,
                name = "testMoq1"
            }
        };

        herosProviderMock.Setup(m => m.GetHeroById(1)).Returns(Task.FromResult<Heros>(listeMoq.LastOrDefault()));

        var herosController = new HerosController(herosProviderMock.Object);



        var result = (List<Heros>)((JsonResult)(await herosController.Get())).Value;


    }
}
