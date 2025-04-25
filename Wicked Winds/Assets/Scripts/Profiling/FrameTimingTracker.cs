using UnityEngine;

public class FrameTimingTracker : MonoBehaviour
{
    public static float cpuMs, gpuMs;
    // Preparamos un buffer para la última muestra
    FrameTiming[] timings = new FrameTiming[1];

    void Update()
    {
        // 1) Captura los datos de este frame
        FrameTimingManager.CaptureFrameTimings();

        // 2) Pide la última muestra
        uint count = FrameTimingManager.GetLatestTimings(1, timings);
        if (count > 0)
        {
            // cpuFrameTime y gpuFrameTime vienen en milisegundos
            cpuMs = (float)timings[0].cpuFrameTime;
            gpuMs = (float)timings[0].gpuFrameTime;
        }
    }
}
