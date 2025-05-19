using TMPro;
using UnityEngine;

/// <summary>
/// Displays the current frames per second (FPS) in a TextMeshPro text field.
/// Updates at a configurable interval to reduce overhead.
/// </summary>
public class FPSDisplay : MonoBehaviour
{
    #region Public Variables

    [Header("UI")]
    [Tooltip("TextMeshPro text element to display the FPS.")]
    public TMP_Text fpsText;

    [Header("Settings")]
    [Tooltip("How often (in seconds) the FPS text is updated.")]
    public float updateInterval = 0.5f;

    #endregion

    #region Private Variables

    private float accumulated; // Sum of frame rates over the interval
    private int frames;        // Number of frames accumulated
    private float timeLeft;    // Time remaining in the current interval

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        if (fpsText == null)
        {
            Debug.LogError("⚠️ FPSDisplay: TMP_Text reference is not set.");
            enabled = false;
            return;
        }

        timeLeft = updateInterval;
    }

    private void Update()
    {
        // Accumulate frame time and frame count
        timeLeft -= Time.deltaTime;
        accumulated += Time.timeScale / Time.deltaTime;
        ++frames;

        // If the interval has elapsed, update the FPS text
        if (timeLeft <= 0.0f)
        {
            float fps = accumulated / frames;
            fpsText.text = $"FPS: <b>{Mathf.RoundToInt(fps)}</b>";

            timeLeft = updateInterval;
            accumulated = 0f;
            frames = 0;
        }
    }

    #endregion
}