using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class LoadingBar : MonoBehaviour
{
    Slider slider;
    float progressValue;
    public GameObject StardustTown;
    public GameObject SandyLandy;
    public GameObject FrostpeakHollow;

    void Start()
    {
        slider = GetComponent<Slider>();
        StartCoroutine(LoadSceneAsync(GameManager.Instance.sceneToLoad));
        switch (GameManager.Instance.town)
        {
            case TownGenerator.Town.StardustTown:
                StardustTown.SetActive(true);
                break;
            case TownGenerator.Town.SandyLandy:
                SandyLandy.SetActive(true);
                break;
            case TownGenerator.Town.FrostpeakHollow:
                FrostpeakHollow.SetActive(true);
                break;

        }
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        SceneManager.sceneLoaded += GameManager.Instance.OnSceneLoaded;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            slider.value = progressValue;
            yield return null;
        }
    }
}
