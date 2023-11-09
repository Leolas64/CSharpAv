using System.Text;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

public class CorrectionTextResult
{
    public string id { get; set; }
    public string @object { get; set; }
    public long created { get; set; }
    public string model { get; set; }
    public List<ChatChoice> choices { get; set; }
    public ChatUsage usage { get; set; }
}

public class TraductionTextResult
{
    public List<translate> translations { get; set; }
}

public class translate
{
    public string text { get; set; }
    public string to { get; set; }
}

public class ChatChoice
{
    public int index { get; set; }
    public ChatMessage message { get; set; }
    public string finish_reason { get; set; }
}

public class ChatMessage
{
    public string role { get; set; }
    public string content { get; set; }
}

public class ChatUsage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}

class Program
{
    static async Task Translate()
    {
        string Translkey = "fd82baac3b184ef1907fc3bcc2d997e9";
        string Translendpoint = "https://api.cognitive.microsofttranslator.com";
        string location = "francecentral";
        string args;
        Console.WriteLine("Entrez le texte à traduire");
        args = Console.ReadLine();

        string route = "/translate?api-version=3.0&from=fr&to=en";
        object[] body = new object[] { new { Text = args } };
        var requestBody = JsonConvert.SerializeObject(body);

        using (var client = new HttpClient())
        using (var request = new HttpRequestMessage())
        {
            // Build the request.
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(Translendpoint + route);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", Translkey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.

            string result = await response.Content.ReadAsStringAsync();

            List<TraductionTextResult> variable = JsonConvert.DeserializeObject<List<TraductionTextResult>>(result);

            Console.WriteLine(variable[0].translations[0].text);
        }
    }

    static async Task Correc()
    {
        string apiKey = "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";
        string endpoint = "https://api.openai.com/v1/chat/completions";
        string sendText;
        string[] args = new string[1];
        Console.WriteLine("Ecrivez le texte à corriger ");
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
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var demande = new
            {
                role = "user",
                content = $"Corrige le texte suivant : '{sendText}'",
            };

            var messages = new[] { demande };

            var requestBody = new
            {
                messages,
                model = "gpt-3.5-turbo",
                max_tokens = 50
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var temp = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(endpoint, temp);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Texte source :");
                Console.WriteLine(sendText);

                Console.WriteLine("\nTexte corrigé :");
                CorrectionTextResult variable = JsonConvert.DeserializeObject<CorrectionTextResult>(responseContent);
                Console.WriteLine(variable.choices[0].message.content);
            }
            else
            {
                Console.WriteLine($"Une erreur s'est produite : {response.StatusCode}");
            }
        }
    }

    static async Task reactCreate()
    {
        Console.WriteLine("Donnez le nom de l'application :");

        string name = Console.ReadLine();

        var startLib = new ProcessStartInfo
        {
            FileName = "node",
            Arguments = $" --version",
            WorkingDirectory = "C:\\Users\\ellan\\Downloads\\Ynov_Test_C#\\ReactApps",
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var startInfo = new ProcessStartInfo
        {
            FileName = "node",
            Arguments = $" --version",
            WorkingDirectory = "C:\\Users\\ellan\\Downloads\\Ynov_Test_C#\\ReactApps",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var processLib = new Process
        {
            StartInfo = startLib,

        };
        var process = new Process
        {
            StartInfo = startInfo,
        };

        //processLib.Start();
        //processLib.WaitForExit();
        process.Start();
        process.WaitForExit();

        Console.WriteLine("\n✅  Application React done !");

        //Process.Start("code", "C:\\Users\\ellan\\Downloads\\Ynov_Test_C#\\CLI-CSharp");

    }

    static async Task Main()
    {
        await Translate();
        string reponse = "";
        string action = "";
        do
        {
            reponse = "";
            do
            {
                Console.WriteLine("Voulez vous continuer ? (y/n)");

                reponse = Console.ReadLine();
                if (reponse != "y" && reponse != "n")
                    Console.WriteLine("Donnez une réponse valide");
            } while (reponse != "y" && reponse != "n");
            if (reponse == "y")
            {
                Console.WriteLine("Que voulez-vous faire ?");
                Console.WriteLine("-c = correction de texte");
                Console.WriteLine("-t = traduction de texte");
                Console.WriteLine("-react = création d'app react");

                action = Console.ReadLine();

                if (action != "-c" && action != "-t" && action != "-react")
                {
                    Console.WriteLine("Entrez une réponse valide");
                }
                if (action == "-c")
                {
                    await Correc();
                }
                else if (action == "-t")
                {
                    await Translate();
                }
                else if (action == "-react")
                {
                    await reactCreate();
                }
            }
            else
            {
                Console.WriteLine("Au revoir");
            }
        } while (reponse == "y");
    }
}