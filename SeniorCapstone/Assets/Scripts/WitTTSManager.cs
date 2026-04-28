using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class WitTTSManager : MonoBehaviour
{
    [Header("Wit Settings")]
    public string witApiKey = "YOUR_SERVER_ACCESS_TOKEN";

    [Header("Audio")]
    public AudioSource audioSource;

    private const string TTS_URL = "https://api.wit.ai/synthesize";

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Speak(string text)
    {
        StartCoroutine(SendTTSRequest(text));
    }

    IEnumerator SendTTSRequest(string text)
    {
        string url = $"{TTS_URL}?q={UnityWebRequest.EscapeURL(text)}";

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV);

        request.SetRequestHeader("Authorization", "Bearer " + witApiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("TTS Error: " + request.error);
            yield break;
        }

        AudioClip clip = DownloadHandlerAudioClip.GetContent(request);

        audioSource.clip = clip;
        audioSource.Play();
    }
}