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
                retstring += item.title + Environment.NewLine;
                /*
                 * Because model titles can have ';' or any character in their name, use
                 * int i = modelInfo.IndexOf(';'); //to get the first occurence
                 * string modelToken = modelInfo.Substring(0,i);
                 * string modelName = modelInfo.Substring(i);
                 */
            }
        }
        File.WriteAllText(Path.Combine(mpath, "all.txt"), retstring.Trim());
    }
    else { throw new Exception(); }
}
catch { Console.WriteLine("GET request from URI https://api.fakeyou.com/tts/list was cancelled."); }

public class ModelListResponse
{
    public bool success { get; set; }
    public Model[] models { get; set; }
}
public class Model
{
    public string model_token { get; set; }
    public string title { get; set; }
    //public string ietf_language_tag { get; set; }
    //public string ietf_primary_language_subtag { get; set; }
    //public string tts_model_type { get; set; }
    //public string created_at { get; set; }
    //public string updated_at { get; set; }
    //public string creator_user_token { get; set; }
    //public string creator_username { get; set; }
    //public string creator_display_name { get; set; }
    //public string creator_gravatar_hash { get; set; }
    //public bool is_front_page_feaured { get; set; }
    //public bool is_twitch_featured { get; set; }
    //public string[] category_tokens { get; set; }
    //public string? maybe_suggested_unique_bot_command { get; set; }
}