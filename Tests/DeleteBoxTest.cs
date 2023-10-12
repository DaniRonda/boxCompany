using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using infrastructure.DataModels;

namespace Tests;

public class DeleteBox
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [Test]
    public async Task GetFullBoxTest()
    {
        Helper.TriggerRebuild();

        var box = new Box()
            {
             BoxName = "boxy box",
             BoxDescription = "just a cube made of cardboard",
             BoxImgUrl = "https://someimg/img.jpg"
             };
        var sql = $@" 
            insert into public.boxes (boxname, boxdescription, boximgurl) VALUES (@box_name, @box_description, @box_img_url);
            ";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, box);
        }

        var url = "http://localhost:5000/api/boxes";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.DeleteAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());

        }
        catch (Exception e)
        {
            throw new Exception();
        }

        using (new AssertionScope())
        {
            using (var conn = Helper.DataSource.OpenConnection())
            {
                (conn.ExecuteScalar<int>($"SELECT COUNT(*) FROM public.boxes WHERE boxid = 1;") == 0)
                    .Should()
                    .BeTrue();
            }
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}