using UnityEngine;
using UnityEngine.UI;

public class GameManagerButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set up the button’s onClick listener with a reference to GameManager.Instance
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ClickButton(name));
    }
}
