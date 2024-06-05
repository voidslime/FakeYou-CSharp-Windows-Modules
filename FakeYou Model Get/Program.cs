using System.Text.Json;

string mpath = args[0];
HttpClient client = new HttpClient();
string reply;
string modeltoken;
int i = 0;
string retstring = "";
List<string> blacklisted = new List<string>();

if (!Directory.Exists(mpath)) { Directory.CreateDirectory(mpath); }
if (File.Exists(Path.Combine(mpath, "blacklist.txt")))
{
    blacklisted = new List<string>(
        File.ReadAllText(Path.Combine(mpath, "blacklist.txt"))
        .Split(Environment.NewLine)
    );
}

try
{
    //https://api.fakeyou.com/tts/list
    //client.Headers.Add("Authorization:SAPI:YOUR_API_KEY_OPTIONAL");
    reply = await client.GetStringAsync("https://api.fakeyou.com/tts/list");
    ModelListResponse ReturnValue = JsonSerializer.Deserialize<ModelListResponse>(reply);
    if (ReturnValue.success == true)
    {
        Model[] models = ReturnValue.models;
        foreach (var item in models)
        {
            modeltoken = item.model_token;
            if (!blacklisted.Contains(modeltoken))
            {
                i++;
                retstring += modeltoken + ";";
                retstring += item.title + ";";
                retstring += Environment.NewLine;
            }
        }
        File.WriteAllText(Path.Combine(mpath, "all.txt"), retstring);
    }
    else { throw new Exception(); }
}
catch { Console.WriteLine("GET request from URI https://api.fakeyou.com/tts/list was cancelled."); }

public class ModelListResponse
{
    public bool success {  get; set; }
    public Model[] models { get; set; }
}
public class Model
{
    public string model_token { get; set; }
    public string title { get; set; }
}