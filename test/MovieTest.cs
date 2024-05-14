using Newtonsoft.Json;
using System.Collections.Generic;

namespace FilmTests
{
    [TestFixture]
    public class FilmTests
    {
        private string filmPath = "../../../../src/DataSource/Films.Json";
        private string idFilePath = "../../../../src/DataSource/Id.json";

        [Test]
        public void TestNumberOfFilms()
        {
            // this test function compares the number of actual films in films.json and 
            //compares it to the max ID which should also correspond to the amount of movies
            //in Id.json
            // Read the JSON file and turn to list of films
            string jsonContent = File.ReadAllText(filmPath);
            List<Film> films = JsonConvert.DeserializeObject<List<Film>>(jsonContent);

            // read the ID json file to check if the movies are corresponding to the max ID
            string idJsonContent = File.ReadAllText(idFilePath);
            int[] idArray = JsonConvert.DeserializeObject<int[]>(idJsonContent);
            int expectedNumber = idArray[0];

            // Assert that the number of films is as expected
            Assert.AreEqual(expectedNumber, films.Count); // Change 10 to the expected number of films
        }
    }
}
