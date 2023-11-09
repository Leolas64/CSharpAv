using System.Net.Http.Json;

class wexrftcvyghubinjok
{
    static async Task aznoiefianzkdsnmvlsurbiukdlbvisreuvsCorrec()
    {
        string correcApiKey = "CLEAZUR";
        string correcEndpoint = "https://corrector.cognitiveservices.azure.com/";
        string sendText;
        string correcLocation = "francecentral";
        string[] args = new string[1];

        args[0] = Console.ReadLine();

        if (args.Length == 0)
        {
            Console.WriteLine("Pas de texte détecté");
            return;
        }
        else
        {
            sendText = string.Join(" ", args);
        }

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {correcApiKey}");

            var requestBody = new
            {
                Content = $"Corrige le texte suivant : '{sendText}'",
                modele = "gpt-3.5-turbo",
            };

            HttpResponseMessage response = await client.PostAsJsonAsync(correcEndpoint, requestBody);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Texte source :");
                Console.WriteLine(sendText);

                Console.WriteLine("\nTexte corrigé :");
                Console.WriteLine(responseContent);
            }
            else
            {
                Console.WriteLine($"Une erreur s'est produite : {response.StatusCode}");
            }
        }
    }
}
