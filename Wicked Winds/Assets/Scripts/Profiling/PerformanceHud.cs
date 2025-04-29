using UnityEngine;
using Unity.Profiling;

/// <summary>
/// HUD: dos bolitas independientes (CPU y GPU).
/// - Verdes por defecto.
/// - La de CPU se vuelve ROJA cuando la CPU se pasa del presupuesto.
/// - La de GPU se vuelve NARANJA cuando la GPU se pasa del presupuesto.
/// Persisten unos segundos para que dé tiempo a verlas.
/// Singleton para que solo haya uno en todo el juego.
/// </summary>
public class PerformanceHud : MonoBehaviour
{
    /* ================  SINGLETON  ================ */
    public static PerformanceHud Instance;   // Mi única instancia

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   // no me destruyas al cambiar de escena
        }
        else
        {
            Destroy(gameObject);            
            return;
        }

        // -- Configuro estilos de texto y de las bolitas --
        int size = Mathf.RoundToInt(baseFontSize * (Screen.height / 1080f));
        m_Text = new GUIStyle { fontSize = size, normal = { textColor = Color.white } };
        m_Dot = new GUIStyle { fontSize = size * 2, alignment = TextAnchor.MiddleCenter };
    }

    /* ------------- Ajustes visibles en el Inspector ------------- */
    [SerializeField] int targetFps = 60;
    [SerializeField] int baseFontSize = 32;
    [Tooltip("Segundos que la bolita mantiene el color de alerta")]
    [SerializeField] float overBudgetHoldSec = 0.75f;
    /* ------------------------------------------------------------ */

    GUIStyle m_Text, m_Dot;
    readonly FrameTiming[] m_FrameTimings = new FrameTiming[1];

    float lastCpuTime, lastGpuTime; // ms del último frame
    bool haveTimings;

    /* ---------- Estado de cada bolita ---------- */
    Color dotCpuColor = new Color(0.30f, 0.90f, 0.30f); // verde
    Color dotGpuColor = new Color(0.30f, 0.90f, 0.30f); // verde
    float latchCpuUntil, latchGpuUntil;                  // cuándo caduca cada color
    /* ------------------------------------------- */

    void Update()
    {
        // Pido timings al sistema
        FrameTimingManager.CaptureFrameTimings();
        if (FrameTimingManager.GetLatestTimings(1, m_FrameTimings) == 0)
        {
            haveTimings = false;
            return;
        }

        haveTimings = true;
        lastCpuTime = (float)m_FrameTimings[0].cpuFrameTime;
        lastGpuTime = (float)m_FrameTimings[0].gpuFrameTime;

        float budgetMs = 1000f / targetFps;
        bool overCpu = lastCpuTime > budgetMs;
        bool overGpu = lastGpuTime > budgetMs;

        /* ---- Actualizo latches por componente ---- */
        if (overCpu)
        {
            dotCpuColor = new Color(1f, 0.20f, 0.20f); // rojo
            latchCpuUntil = Time.unscaledTime + overBudgetHoldSec;
        }
        if (overGpu)
        {
            dotGpuColor = new Color(1f, 0.55f, 0.15f); // naranja
            latchGpuUntil = Time.unscaledTime + overBudgetHoldSec;
        }
        /* ----------------------------------------- */
    }

    void OnGUI()
    {
        if (!haveTimings) return;

        float budgetMs = 1000f / targetFps;

        /* ---- Compruebo si caducó cada latch ---- */
        if (Time.unscaledTime > latchCpuUntil)
            dotCpuColor = new Color(0.30f, 0.90f, 0.30f);   // verde
        if (Time.unscaledTime > latchGpuUntil)
            dotGpuColor = new Color(0.30f, 0.90f, 0.30f);   // verde
        /* ---------------------------------------- */

        // Dibujo ventanita con números
        DrawWindow(lastCpuTime, lastGpuTime, budgetMs);

        // Dibujo bolita CPU (arriba) y bolita GPU (abajo)
        float dotX = 2f;          // margen desde la izquierda
        float dotY = 52f;         // altura inicial (alineada con ventana)
        m_Dot.normal.textColor = dotCpuColor;
        GUI.Label(new Rect(dotX, dotY, 30f, 30f), "●", m_Dot);
        m_Dot.normal.textColor = dotGpuColor;
        GUI.Label(new Rect(dotX, dotY + 34f, 30f, 30f), "●", m_Dot);
    }

    /* =================  Helpers de dibujo  ================= */
    void DrawWindow(float cpu, float gpu, float budgetMs)
    {
        string[] lines =
        {
            $"CPU total: {cpu:00.00} ms",
            $"GPU:       {gpu:00.00} ms",
            $"Budget:    {budgetMs:00.00} ms  (> {targetFps} fps)"
        };

        float w = 0f;
        foreach (var l in lines)
            w = Mathf.Max(w, m_Text.CalcSize(new GUIContent(l)).x);
        float h = lines.Length * (m_Text.lineHeight + 4) + 20f;

        var area = new Rect(32, 50, w + 40f, h + 20f);
        GUILayout.BeginArea(area, "Frame Stats", GUI.skin.window);
        foreach (var l in lines) GUILayout.Label(l, m_Text);
        GUILayout.EndArea();
    }
}