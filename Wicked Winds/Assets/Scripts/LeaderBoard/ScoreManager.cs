/*using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Dan.Main;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton Instance
    public int score; // Almacena la puntuación

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
        scoreText.text =  score.ToString();
        UpdateScore(score);
    }
    // Método que actualiza la puntuación cuando el juego acaba
    public void UpdateScore(int elapsedTimeScore)
    {
        score = elapsedTimeScore;  // Asigna el tiempo transcurrido como la puntuación
        Debug.Log("Puntuación actualizada: " + GameManager.Instance.playerScore);
        SubmitScore();
    }


    public void SubmitScore() //ARREGLAAAAAAAAAAAR RESETEA EL SCORE AL CAMBIAR AL LEADERBOARD
    {
        // Obtener el nombre del input

        if (!string.IsNullOrEmpty(inputName.text))
        {
            // Invoca el evento para enviar el nombre y la puntuación al leaderboard
            submitScoreEvent.Invoke(inputName.text, GameManager.Instance.playerScore);
            Debug.Log("Nombre y puntuación enviados al evento."+ inputName.text + GameManager.Instance.playerScore);

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
    }
    
}
   */

