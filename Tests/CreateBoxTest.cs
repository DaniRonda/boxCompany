using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using infrastructure.DataModels;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests;

public class CreateBox
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }


    [Test]
    public async Task ShouldSuccessfullyCreateBox()
    {
        Helper.TriggerRebuild();
        var box = new Box()
        {
            BoxName = "boxy box",
            BoxDescription = "just a cube made of cardboard",
            BoxImgUrl = "https://someimg/img.jpg"
        };
        var url = "http://localhost:5000/api/boxes";
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception( );
        }

        Box responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<Box>(
                await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
                throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(box, Helper.MyBecause(responseObject, box));
        }
    }

    [TestCase("TestName1", "Idk at this point",  "https://someimg/img.jpg")]
    [TestCase("", "Mock description",  "invalidurl")]
    [TestCase("TestName2", "Mock body",  "https://someimg/img.jpg")]
    [TestCase("Mock TestName3", "Mock body",  null)]
    public async Task ShouldFailDueToDataValidation(string boxName, string boxDescription, string boxImgUrl)
    {
        var box = new Box()
        {
            BoxName = boxName,
            BoxDescription = boxDescription,
            BoxImgUrl = boxImgUrl
        };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/boxes", box);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception();
        }
        
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}