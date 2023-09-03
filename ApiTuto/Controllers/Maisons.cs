using System.Data;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ApiTuto.Controllers;

[ApiController]
[Route("[controller]")]
public class MaisonsController : ControllerBase
{

    private readonly ILogger<MaisonsController> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public MaisonsController(ILogger<MaisonsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("dbTestVincent") ?? "";
    }

    [HttpGet(Name = "GetListeMaisons")]
    public IEnumerable<object> Get()
    {
        
        var result = new List<DetailMaison>();

        var test = TestCommand("SELECT TOP (1000) * FROM [master].[dbo].[MSreplication_options]", _connectionString);
       

        return result;
    }


    private static int TestCommand(string queryString,
    string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(
                   connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var result =  reader[0];
                }
            }
        }
        return 0;
    }
}




