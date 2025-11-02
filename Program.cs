using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using System.Diagnostics;
namespace Spotfify
{
    sealed class Program
    {
        static  async Task getMusic(string emotion)
        {
            string clientId = "";
            string clientSercet = "";
            string keyword = MapToKeyWord(emotion);
            await playMusic(keyword, clientId, clientSercet); 
        }
        static string MapToKeyWord(string emotion)
        {
            return emotion.ToLower() switch
            {
                "happy" => "happy upbeat",
                "sad" => "energetic",
                "neural" => "happy upbeat",
                "suprise" => "intense",
                "angry" => "sad mellow",
                _ => "pop"
            };
        }
        static async Task playMusic(string keyword,string id,string secret)
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest(id, secret);
            var response = await new OAuthClient(config).RequestToken(request);
            var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            var searchRequest = new SearchRequest(SearchRequest.Types.Track, keyword);
            var searchResponse = await spotify.Search.Item(searchRequest);
            if(searchResponse.Tracks.Items!.Count > 0)
            {
                var rnd = new Random();
                int index = rnd.Next(Math.Min(10, searchResponse.Tracks.Items.Count));
                var track = searchResponse.Tracks.Items[index];
                Console.WriteLine($"Track Found: {track.Name} by {track.Artists[0].Name}");
                Console.WriteLine($"Playing the Song...");
                Process.Start(new ProcessStartInfo
                 {
                  FileName = track.ExternalUrls["spotify"],
                   UseShellExecute = true
                });
            }
            else
            {
                Console.Error.WriteLine("No Music Found");
                Environment.Exit(1);
            }
        }
        static async Task Main()
        {
            string cmmd = "python3";
            string pythonfile = "main.py";
            var psi = new ProcessStartInfo
            {
                FileName = cmmd,
                Arguments = $"\"{pythonfile}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = new Process();
            process.StartInfo = psi;
            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.Error.WriteLine($"Error:{error} while running the python file");
                Environment.Exit(1);
            }
            string predictedEmotion = output.Trim();
            Console.WriteLine($"Predicted Emotion: {predictedEmotion}");
            await getMusic(predictedEmotion);
        }
    }
}