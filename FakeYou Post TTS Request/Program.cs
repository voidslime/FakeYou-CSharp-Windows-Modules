using NAudio.Wave;
using System.Collections;
using System.Diagnostics;
using System.Media;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

try
{
    const string m_cdn = "https://storage.googleapis.com/vocodes-public";

    IDictionary envVars = Environment.GetEnvironmentVariables();
    string modelToken = envVars["MODELTOKEN"].ToString();
    string inputText = envVars["MESSAGE"].ToString();
    string outputPath = Path.Combine(Environment.CurrentDirectory, "tempwav.wav");

    Debug.WriteLine($"File will be writen to {outputPath}");
    
    string ReqM = await MakeTTSRequest(modelToken, inputText);
    if (ReqM == "Failed")
    {
        throw new Exception("request failed");
    }
    TTSPollStatus poll = await PollTTSRequestStatus(ReqM);
    while (poll.status == "started" | poll.status == "pending")
    {
        Thread.Sleep(100);
        poll = await PollTTSRequestStatus(ReqM);
    }

    Debug.WriteLine(poll.maybe_public_bucket_wav_audio_path);
    byte[] bytes = await StreamTTSAudioClip(m_cdn + poll.maybe_public_bucket_wav_audio_path);
    System.IO.File.WriteAllBytes(outputPath, bytes);
    Debug.WriteLine("recieved clip");

    int deviceId = -1;
    const string pat = @"CABLE-B";
    for (int n = -1; n < WaveOut.DeviceCount; n++)
    {
        WaveOutCapabilities caps = WaveOut.GetCapabilities(n);
        Debug.WriteLine($"{n}: {caps.ProductName}");
        if (Regex.IsMatch(caps.ProductName, pat))
        {
            deviceId = n;
            break;
        }
    }
    WaveOutEvent waveOutEvent = new()
    {
        DeviceNumber = deviceId
    };
    waveOutEvent.Init(new AudioFileReader(outputPath));
    waveOutEvent.Play();
    while (waveOutEvent.PlaybackState != PlaybackState.Stopped) ;
}
catch (Exception e) { Console.WriteLine(e); }

static async Task<string> MakeTTSRequest(string modelToken, string text)
{
    try
    {
        HttpClient request = new()
        {
            BaseAddress = new Uri("https://api.fakeyou.com")
        };
        //request.Headers.Add("Authorization:SAPI:YOUR_API_KEY_OPTIONAL");
        Guid myuuid = Guid.NewGuid();
        string myuuidAsString = myuuid.ToString();
        TTSPostData postData = new()
        {
            tts_model_token = modelToken,
            inference_text = text,
            uuid_idempotency_token = myuuidAsString
        };
        Console.WriteLine(JsonSerializer.Serialize(postData));
        using HttpResponseMessage response = await request.PostAsJsonAsync(
            "tts/inference", postData);
        response.EnsureSuccessStatusCode();
        TTSPostResponse jsonResponse = await response.Content.ReadFromJsonAsync<TTSPostResponse>();
        Console.WriteLine(jsonResponse);
        return jsonResponse.inference_job_token;
    }
    catch (Exception e) { return e.Message; }
}
static async Task<TTSPollStatus> PollTTSRequestStatus(string jobToken)
{
    TTSPollStatus retval = new TTSPollStatus();
    HttpClient client = new HttpClient();
    //client.Headers.Add("Authorization:SAPI:YOUR_API_KEY_OPTIONAL");
    string reply = await client.GetStringAsync("https://api.fakeyou.com/tts/job/" + jobToken);
    TTSStatusResponse ReturnValue = JsonSerializer.Deserialize<TTSStatusResponse>(reply);
    if ((bool)ReturnValue.success != true)
    {
        retval.status = "Failed";
        return retval;
    }
    var state = ReturnValue.state;
    retval.status = (string)state.status;
    retval.maybe_public_bucket_wav_audio_path = (string)state.maybe_public_bucket_wav_audio_path;
    return retval;
}
static async Task<byte[]> StreamTTSAudioClip(string uri)
{
    HttpClient client = new HttpClient();
    //client.Headers.Add("Authorization:SAPI:YOUR_API_KEY_OPTIONAL");
    return await client.GetByteArrayAsync(uri);
}

public class HolderTemp
{
    public int index = 0;
    public byte[] data;
}
public class TTSPollStatus
{
    public string status;
    public string maybe_public_bucket_wav_audio_path;
}
public class TTSPostData
{
    public string tts_model_token { get; set; }
    public string uuid_idempotency_token { get; set; }
    public string inference_text { get; set; }
}
public class TTSPostResponse
{
    public bool success { get; set; }
    public string inference_job_token { get; set; }
}

public class TTSStatusResponse
{
    public bool success { get; set; }
    public TTSStatusState state { get; set; }
}

public class TTSStatusState
{
    public string job_token { get; set; }
    public string status { get; set; }
    public string maybe_extra_status_description { get; set; }
    public int attempt_count { get; set; }
    public string maybe_result_token { get; set; }
    public string maybe_public_bucket_wav_audio_path { get; set; }
    public string model_token { get; set; }
    public string tts_model_type { get; set; }
    public string title { get; set; }
    public string raw_inference_text { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}
