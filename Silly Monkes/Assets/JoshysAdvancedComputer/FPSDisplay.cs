using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;
    private TextMeshPro fpsText;

    private void Start()
    {
        fpsText = GetComponent<TextMeshPro>();
        if (fpsText == null)
        {
            Debug.LogError("FPSDisplay: No TextMeshPro component found.");
        }
        timeleft = updateInterval;
    }

    private void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            fpsText.text = string.Format("{0:F2} FPS", fps);

            if (fps < 30)
                fpsText.color = Color.yellow;
            else
                if (fps < 10)
                fpsText.color = Color.red;
            else
                fpsText.color = Color.green;

            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}