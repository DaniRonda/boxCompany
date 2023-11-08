using NUnit.Framework;
using FluentAssertions;
using System.Net.Http.Json;
using System.Net;
using infrastructure.DataModels;
using Newtonsoft.Json;

namespace Tests
{
    [TestFixture]
    public class BoxTests
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
            var box = new Box()
            {
                BoxName = "boxy box",
                BoxDescription = "just a cube made of cardboard",
                BoxImgUrl = "https://someimg/img.jpg"
            };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/boxes", box);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseObject = JsonConvert.DeserializeObject<Box>(await response.Content.ReadAsStringAsync());

            responseObject.Should().NotBeNull();
            responseObject.BoxName.Should().Be(box.BoxName);
            responseObject.BoxDescription.Should().Be(box.BoxDescription);
            responseObject.BoxImgUrl.Should().Be(box.BoxImgUrl);
        }

        [Test]
        public async Task ShouldSuccessfullyEditBox()
        {
            var box = new Box()
            {
                BoxName = "Original Name",
                BoxDescription = "Original Description",
                BoxImgUrl = "https://original/img.jpg"
            };

            var createResponse = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/boxes", box);
            createResponse.EnsureSuccessStatusCode();

            var createdBox = JsonConvert.DeserializeObject<Box>(await createResponse.Content.ReadAsStringAsync());

            createdBox.BoxName = "Updated Name";
            createdBox.BoxDescription = "Updated Description";

            var editResponse = await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/boxes/{createdBox.BoxId}", createdBox);
            editResponse.EnsureSuccessStatusCode();

            var editedBox = JsonConvert.DeserializeObject<Box>(await editResponse.Content.ReadAsStringAsync());

            editedBox.Should().NotBeNull();
            editedBox.BoxName.Should().Be("Updated Name");
            editedBox.BoxDescription.Should().Be("Updated Description");
            editedBox.BoxImgUrl.Should().Be("https://original/img.jpg");
        }

        [Test]
        public async Task ShouldSuccessfullyDeleteBox()
        {
            var box = new Box()
            {
                BoxName = "Box to Delete",
                BoxDescription = "To be deleted",
                BoxImgUrl = "https://img/to/delete.jpg"
            };

            var createResponse = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/boxes", box);
            createResponse.EnsureSuccessStatusCode();

            var createdBox = JsonConvert.DeserializeObject<Box>(await createResponse.Content.ReadAsStringAsync());

            var deleteResponse = await _httpClient.DeleteAsync($"http://localhost:5000/api/boxes/{createdBox.BoxId}");
            deleteResponse.EnsureSuccessStatusCode();

            var deletedBox = JsonConvert.DeserializeObject<Box>(await deleteResponse.Content.ReadAsStringAsync());

            deletedBox.Should().BeNull();
        }

        [Test]
        public async Task ShouldSuccessfullyReadBox()
        {
            var box = new Box()
            {
                BoxName = "Read Test",
                BoxDescription = "A test box for reading",
                BoxImgUrl = "https://img/read.jpg"
            };

            var createResponse = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/boxes", box);
            createResponse.EnsureSuccessStatusCode();

            var createdBox = JsonConvert.DeserializeObject<Box>(await createResponse.Content.ReadAsStringAsync());

            var readResponse = await _httpClient.GetAsync($"http://localhost:5000/api/boxes/{createdBox.BoxId}");
            readResponse.EnsureSuccessStatusCode();

            var readBox = JsonConvert.DeserializeObject<Box>(await readResponse.Content.ReadAsStringAsync());

            readBox.Should().NotBeNull();
            readBox.BoxName.Should().Be("Read Test");
            readBox.BoxDescription.Should().Be("A test box for reading");
            readBox.BoxImgUrl.Should().Be("https://img/read.jpg");
        }
        

        [Test]
        public async Task ShouldFailToDeleteNonExistentBox()
        {
            // Attempt to delete a Box that doesn't exist
            var nonExistentBoxId = 9999;
            var deleteResponse = await _httpClient.DeleteAsync($"http://localhost:5000/api/boxes/{nonExistentBoxId}");

            deleteResponse.IsSuccessStatusCode.Should().BeFalse();
        }

        [Test]
        public async Task ShouldFailToReadNonExistentBox()
        {
            // Attempt to read a Box that doesn't exist
            var nonExistentBoxId = 9999;
            var readResponse = await _httpClient.GetAsync($"http://localhost:5000/api/boxes/{nonExistentBoxId}");

            readResponse.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
