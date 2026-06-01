using UnityEngine;
using System.Collections;
using Convai.Runtime.Components;

public class ConvaiVRVoiceSystem : MonoBehaviour
{
    [Header("Reference")]
    public ConvaiManager convaiManager;

    [Header("Settings")]
    public float silenceDelay = 1f;
    public float cooldownAfterNPC = 60f;

    private bool micMuted;
    private bool npcSpeaking;
    private bool cooldownActive;
    private bool playerSpeaking;

    private float lastVoiceTime;

    void Update()
    {
        HandleVoiceSilence();
    }

    // --------------------------------------------------
    // USER DETECTION (BLEIBT LOCAL)
    // --------------------------------------------------
    public void OnUserStartedSpeaking()
    {
        playerSpeaking = true;
        lastVoiceTime = Time.time;
    }

    public void OnUserStoppedSpeaking()
    {
        playerSpeaking = false;
        lastVoiceTime = Time.time;
    }

    // --------------------------------------------------
    // SILENCE LOGIC
    // --------------------------------------------------
    void HandleVoiceSilence()
    {
        if (cooldownActive) return;

        if (playerSpeaking)
        {
            lastVoiceTime = Time.time;
            return;
        }

        if (!micMuted && !npcSpeaking && Time.time - lastVoiceTime > silenceDelay)
        {
            SetMic(false);
        }
    }

    // --------------------------------------------------
    // MIC CONTROL
    // --------------------------------------------------
    void SetMic(bool state)
    {
        if (micMuted == !state) return;

        micMuted = !state;
        convaiManager.ToggleMicMute();
    }

    // --------------------------------------------------
    // NPC START
    // --------------------------------------------------
    public void OnNPCStartedSpeaking()
    {
        npcSpeaking = true;
        SetMic(false);
    }

    // --------------------------------------------------
    // NPC END
    // --------------------------------------------------
    public void OnNPCFinishedSpeaking()
    {
        npcSpeaking = false;

        if (!cooldownActive)
            StartCoroutine(CooldownRoutine());
    }

    // --------------------------------------------------
    // 60s LOCK
    // --------------------------------------------------
    IEnumerator CooldownRoutine()
    {
        cooldownActive = true;

        yield return new WaitForSeconds(cooldownAfterNPC);

        SetMic(true);

        cooldownActive = false;
    }
}