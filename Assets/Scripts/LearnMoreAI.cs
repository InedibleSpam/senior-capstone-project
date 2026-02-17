using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class OpenAIManager : MonoBehaviour
{
    [Header("OpenAI Settings")]
    public string apiKey = "PASTE_YOUR_OPENAI_KEY_HERE";

    [TextArea(3,10)]
    public string prompt = "Explain this ISS module in simple terms.";

    public void LearnMore()
    {
        StartCoroutine(SendRequestToOpenAI());
    }

    IEnumerator SendRequestToOpenAI()
    {
        string url = "https://api.openai.com/v1/chat/completions";

        // Safely create a JSON object
        ChatRequest requestData = new ChatRequest();
        requestData.model = "gpt-3.5-turbo";
        requestData.messages = new Message[] { new Message { role = "user", content = prompt } };
        requestData.max_tokens = 200;

        string jsonBody = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("OpenAI Error: " + request.error + "\nResponse: " + request.downloadHandler.text);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Raw Response: " + responseText);

            string extractedText = ExtractMessage(responseText);
            Debug.Log("AI Says: " + extractedText);

            // 🔊 Send to your TTS here
            // tts.Speak(extractedText);
        }
    }

    string ExtractMessage(string json)
    {
        if (!json.StartsWith("{")) return "Error parsing AI response";

        int start = json.IndexOf("\"content\":\"") + 11;
        int end = json.IndexOf("\"", start);
        if(start < 11 || end < start) return "Error parsing AI response";
        return json.Substring(start, end - start).Replace("\\n", "\n");
    }

    // Helper classes for JSON
    [System.Serializable]
    public class ChatRequest
    {
        public string model;
        public Message[] messages;
        public int max_tokens;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }
}
