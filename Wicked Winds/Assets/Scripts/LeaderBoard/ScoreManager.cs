using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Dan.Main;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton Instance
    private float score; // Almacena la puntuación

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TextMeshProUGUI scoreText; // Campo para mostrar la puntuación

    //comunicate w leaderboard
    public UnityEvent<string, float> submitScoreEvent;

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("instancia creada de score manager");
            DontDestroyOnLoad(gameObject);  // Asegura que no se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar el evento si es nulo
        if (submitScoreEvent == null)
        {
            submitScoreEvent = new UnityEvent<string, float>();
        }
    }
    public void Start()
    {
        // Recuperar la puntuación desde PlayerPrefs (o 0 si no hay puntuación guardada)
        score = Mathf.FloorToInt(PlayerPrefs.GetFloat("PlayerScore", 0));
      
        // Mostrar la puntuación en el UI
        scoreText.text = "Your Score: " + score.ToString();
        UpdateScore((int)score);
    }
    // Método que actualiza la puntuación cuando el juego acaba
    public void UpdateScore(int elapsedTimeScore)
    {
        score = GameManager.Instance.GetPlayerScore(); ; // Asigna el tiempo transcurrido como la puntuación
        Debug.Log("Puntuación actualizada: " + score);
        SubmitScore();
    }


    public void SubmitScore() //ARREGLAAAAAAAAAAAR RESETEA EL SCORE AL CAMBIAR AL LEADERBOARD
    {
        string playerName = inputName.text;  // Obtener el nombre del input
        float score1 = score;
        if (!string.IsNullOrEmpty(playerName))
        {
            // Invoca el evento para enviar el nombre y la puntuación al leaderboard
            submitScoreEvent.Invoke(playerName, score1);
            Debug.Log("Nombre y puntuación enviados al evento."+ playerName + score1);

        }
        else
        {
            Debug.LogWarning("El nombre del jugador está vacío.");
        }
    }
    /*
    if (!string.IsNullOrEmpty(inputName.text))
    {
        submitScoreEvent.Invoke(inputName.text, score);  // Enviar la puntuación
    }*/
}
   

