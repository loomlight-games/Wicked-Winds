using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{
    Slider slider;
    float progressValue;

    void Start()
    {
        slider = GetComponent<Slider>();

        StartCoroutine(LoadSceneAsync(GameManager.Instance.sceneToLoad));
    }

    public IEnumerator LoadSceneAsync(string sceneName){
        SceneManager.sceneLoaded += GameManager.Instance.OnSceneLoaded;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
    
        while(!loadOperation.isDone){
            progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            slider.value = progressValue;
            yield return null;
        }
    }
}
