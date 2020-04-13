using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace NASAImageGalleryREST
{
    public class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        public static async Task Main()
        {
            await GetMarsImages();

            Console.WriteLine(Environment.NewLine);

            await GetLinksToVideos();
        }

        /// <summary>
        /// Reads the response from <paramref name="url"/> and deserializes it into an object of <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="url">URL to send the request to.</param>
        /// <typeparam name="T">Response object type.</typeparam>
        /// <returns>Returns a deserialized object of the HTTPS response.</returns>
        private static async Task<T> ReadContent<T>(string url)
        {
            string content = null;
            var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }

            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Gets five images from 2018 where either Mars' surface is visible or the picture was taken on Mars' surface.
        /// </summary>
        private static async Task GetMarsImages()
        {
            const string url = "https://images-api.nasa.gov/search?q=Mars&description=Mars%20surface&year_start=2018&year_end=2018";

            var repository = await ReadContent<NasaRepository>(url);
            var itemLinks = repository.collection.items.Take(5);

            foreach (var itemLink in itemLinks)
            {
                var link = itemLink.links
                                   .Where(link => link.render.Equals("image", StringComparison.OrdinalIgnoreCase))
                                   .FirstOrDefault();

                Console.WriteLine(link.href);
            }
        }

        /// <summary>
        /// Checks if 'video' media types contain a link to a video.
        /// </summary>
        private static async Task GetLinksToVideos()
        {
            const string url = "https://images-api.nasa.gov/search?q=Mars&year_start=2018&year_end=2018&media_type=video";

            var repository = await ReadContent<NasaRepository>(url);
            var videoCollectionUrls = repository.collection.items.Select(item => item.href).Take(5); // Limit the number of requests

            foreach (var videoUrl in videoCollectionUrls)
            {
                var links = await ReadContent<List<string>>(videoUrl);

                if (!links.Any(link => link.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"{videoUrl} does not contain a link to a video.");

                    return;
                }
            }

            Console.WriteLine("All links seem fine.");
        }
    }
}
