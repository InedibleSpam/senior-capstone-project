using UnityEngine;
using System.Collections;
using TMPro;

public class RecordBehaviour : MonoBehaviour
{

    [Header("Managers")]
    public Recorder recorder;
    public AICommunicator AiCommunicator;
    public AudioManager audioManager;

    string wakeWord = "assistant";

    private bool isProcessing = false;

    // SETTINGS (tweak these if needed)
    float minVolume = 0.0001f;
    float silenceDuration = 1.5f;

    void Start()
    {
        StartCoroutine(ContinuousListening());
    }

    IEnumerator ContinuousListening()
    {
        while (true)
        {
            Debug.Log("Listening...");

            // Small delay so mic resets cleanly
            yield return new WaitForSeconds(0.3f);

            yield return StartCoroutine(recorder.RecordUntilSilence());

            AudioClip finalClip = recorder.GetRecordedClip();

            if (finalClip == null)
            {
                Debug.LogWarning("No audio recorded.");
                continue;
            }

            // CHECK IF THERE WAS ACTUAL SPEECH
            float finalVolume = GetVolume(finalClip);
            Debug.Log("Final volume: " + finalVolume);

            if (finalVolume < minVolume)
            {
                Debug.Log("Too quiet — skipping");
                continue;
            }

            // PREVENT SPAM REQUESTS
            if (isProcessing)
            {
                Debug.Log("Still processing previous request — skipping");
                continue;
            }

            isProcessing = true;

            if (AiCommunicator.GetAITextResponse() == null)
            {
                Debug.LogWarning("AI failed — resetting state");
                isProcessing = false;
                continue;
            }

            // SEND TO AI
            Debug.Log("Sending to AI...");
            yield return StartCoroutine(
                AiCommunicator.VoiceChat2(finalClip, "hello")
            );

            isProcessing = false;

            string userSpeech = AiCommunicator.LastUserQuery;

            if (string.IsNullOrEmpty(userSpeech))
            {
                Debug.Log("No transcription received.");
                continue;
            }

            Debug.Log("USER SAID: " + userSpeech);

            string cleanedSpeech = NormalizeText(userSpeech);
            string cleanedWakeWord = NormalizeText(wakeWord);

            Debug.Log("CLEANED SPEECH: " + cleanedSpeech);

            if (!cleanedSpeech.Contains(cleanedWakeWord))
            {
                Debug.Log("Wake word NOT detected — ignoring response");
                continue;
            }

            Debug.Log("Wake word detected!");

            AudioClip aiAudio = AiCommunicator.GetAIAudioResponse();
            string aiText = AiCommunicator.GetAITextResponse();

            Debug.Log("AI: " + aiText);

            // PLAY RESPONSE
            if (aiAudio != null)
            {
                audioManager.RemoveClipAndStop();
                audioManager.PlayAudio(aiAudio);
            }
            else
            {
                Debug.LogWarning("No AI audio received.");
            }

            // WAIT UNTIL AUDIO FINISHES
            while (audioManager.GetIsPlaying())
                yield return null;

            yield return new WaitForSeconds(0.5f);
        }
    }

    float GetVolume(AudioClip clip)
{
    int sampleWindow = 1024;
    float[] samples = new float[sampleWindow];

    int micPosition = Microphone.GetPosition(recorder.microphoneDevice) - sampleWindow;

    if (micPosition < 0)
        return 0;

    clip.GetData(samples, micPosition);

    float levelMax = 0;

    for (int i = 0; i < sampleWindow; i++)
    {
        float wavePeak = Mathf.Abs(samples[i]);
        if (wavePeak > levelMax)
        {
            levelMax = wavePeak;
        }
    }

    // 🔥 BOOST (CRITICAL FOR OCULUS)
    return levelMax * 50f;
}

    string NormalizeText(string input)
    {
        input = input.ToLower();

        // remove punctuation
        input = input.Replace(",", "")
                     .Replace(".", "")
                     .Replace("?", "")
                     .Replace("!", "");

        // remove extra spaces
        input = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ").Trim();

        return input;
    }
}