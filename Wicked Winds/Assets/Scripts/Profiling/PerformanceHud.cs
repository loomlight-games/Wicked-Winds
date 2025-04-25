using UnityEngine;
using Unity.Profiling;

public class PerformanceHud : MonoBehaviour
{
    /* ---------- Inspector ---------- */
    [SerializeField] int targetFps = 60;   // 60 fps → 16,67 ms
    [SerializeField] int baseFontSize = 32;
    /* -------------------------------- */

    GUIStyle m_Text, m_Dot;
    readonly FrameTiming[] m_FrameTimings = new FrameTiming[1];

    bool wasOverBudgetLastFrame = false;     // evita spam en consola
    string cat;
    double time;

    void Awake()
    {
        int size = Mathf.RoundToInt(baseFontSize * (Screen.height / 1080f));
        m_Text = new GUIStyle { fontSize = size, normal = { textColor = Color.white } };
        m_Dot = new GUIStyle { fontSize = size * 2, alignment = TextAnchor.MiddleCenter };
    }

    void Update()
    {
        // 1) Capturamos y leemos las timings (frame n-4)
        FrameTimingManager.CaptureFrameTimings();
        int got = (int)FrameTimingManager.GetLatestTimings((uint)m_FrameTimings.Length, m_FrameTimings);
        if (got == 0) return;

        FrameTiming frametime= m_FrameTimings[0];
        // 2) Determinamos el cuello de botella y rellenamos `cat` y `time`
        DetermineBottleneck(frametime);

        // 3) Calculamos si superamos el budget
        float budgetMs = 1000f / targetFps;
        float cpu = (float)frametime.cpuFrameTime;
        float gpu = (float)frametime.gpuFrameTime;
        bool overCpu = cpu > budgetMs;
        bool overGpu = gpu > budgetMs;

        // 4) Log en consola SOLO al pasar de under → over
        if ((overCpu || overGpu) && !wasOverBudgetLastFrame)
        {
            string culprit = (overCpu && cpu >= gpu) ? "CPU" : "GPU";
            string extra = culprit == "CPU"
                ? $" (mayor: {cat} {time:0.00} ms)"
                : "";

            Debug.LogWarning(
                $"[PerformanceHud] Frame OVER budget {budgetMs:0.00} ms – " +
                $"{culprit} tardó {(culprit == "CPU" ? cpu : gpu):0.00} ms{extra}"
            );

            wasOverBudgetLastFrame = true;
        }
        else if (!overCpu && !overGpu)
        {
            // Volvemos a under → reseteamos flag
            wasOverBudgetLastFrame = false;
        }
    }

    void OnGUI()
    {
        float budgetMs = 1000f / targetFps;
        float cpu = (float)m_FrameTimings[0].cpuFrameTime;
        float gpu = (float)m_FrameTimings[0].gpuFrameTime;

        Color dot;
        bool overCpu = cpu > budgetMs;
        bool overGpu = gpu > budgetMs;

        if (!overCpu && !overGpu)
            dot = new Color(0.30f, 0.90f, 0.30f);         // verde
        else if (overCpu && cpu >= gpu)
            dot = new Color(1f, 0.20f, 0.20f);            // rojo
        else
            dot = new Color(1f, 0.55f, 0.15f);            // naranja

        m_Dot.normal.textColor = dot;
        DrawWindow(cpu, gpu, budgetMs);
    }

    void DrawWindow(float cpu, float gpu, float budgetMs)
    {
        string[] lines =
        {
            $"CPU total: {cpu:00.00} ms",
            $"GPU:       {gpu:00.00} ms",
            $"Budget:    {budgetMs:00.00} ms  (> {targetFps} fps)"
        };

        float w = 0f;
        foreach (var l in lines) w = Mathf.Max(w, m_Text.CalcSize(new GUIContent(l)).x);

        float h = lines.Length * (m_Text.lineHeight + 4) + 20f;
        var area = new Rect(32, 50, w + 40f, h + 20f);

        GUILayout.BeginArea(area, "Frame Stats", GUI.skin.window);
        foreach (var l in lines) GUILayout.Label(l, m_Text);
        GUILayout.EndArea();

        GUI.Label(new Rect(area.x - 30f, area.y, 30f, 30f), "●", m_Dot);
    }

    double DetermineBottleneck(FrameTiming ft)
    {
        // CPU es mayor
        if (ft.cpuFrameTime > ft.gpuFrameTime)
        {
            if (ft.cpuMainThreadFrameTime > ft.cpuRenderThreadFrameTime)
            {
                time = ft.cpuMainThreadFrameTime;
                cat = "CPU.MainThread";
            }
            else
            {
                time = ft.cpuRenderThreadFrameTime;
                cat = "CPU.RenderThread";
            }
            return time;
        }
        // GPU es mayor o empate
        else
        {
            time = ft.gpuFrameTime;
            cat = "GPU";
            return time;
        }
    }
}
