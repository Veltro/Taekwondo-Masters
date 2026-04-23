using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 1. Added this to allow scene reloading

public class MinigameBreaker : MonoBehaviour
{
    public enum GameState { Timing, Power, Success, Failed }
    public GameState currentState = GameState.Timing;
    
    [Header("Configurações de UI - Etapa 1 (Timing)")]
    public Slider timingSlider;      
    public RectTransform movingCircle; 
    public TextMeshProUGUI feedbackText;
    
    [Range(0.5f, 10f)]
    public float movementSpeed = 2f; 
    public float perfectThreshold = 0.05f; 
    public float goodThreshold = 0.15f;

    [Header("Configurações de UI - Etapa 2 (Power)")]
    public Slider powerSlider;       
    public float requiredProgress = 0.8f; 
    public float progressPerClick = 0.1f;

    private float timer = 0f;
    private float currentCirclePosition = 0.5f; 

    void Start()
    {
        currentState = GameState.Timing;
        powerSlider.gameObject.SetActive(false);
        feedbackText.text = "Pressione Espaço no Centro!";
        timingSlider.value = 0.5f; 
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Timing:
                HandleTimingStage();
                break;
            case GameState.Power:
                HandlePowerStage();
                break;
            // 2. Added cases to handle input when the game is over
            case GameState.Success:
            case GameState.Failed:
                HandleRestart();
                break;
        }
    }

    // 3. New method to check for restart input
    void HandleRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reloads the active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void HandleTimingStage()
    {
        timer += Time.deltaTime * movementSpeed;
        currentCirclePosition = Mathf.PingPong(timer, 1f);
        
        float sliderWidth = timingSlider.GetComponent<RectTransform>().rect.width;
        float xOffset = (currentCirclePosition - 0.5f) * sliderWidth;
        movingCircle.anchoredPosition = new Vector2(xOffset, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float distanceToCenter = Mathf.Abs(currentCirclePosition - 0.5f);
            CheckTimingResult(distanceToCenter);
        }
    }

    void CheckTimingResult(float distance)
    {
        if (distance <= perfectThreshold) {
            feedbackText.text = "PERFEITO!";
            Debug.Log("PERFEITO");
            StartPowerStage();
        }
        else if (distance <= goodThreshold) {
            feedbackText.text = "BOM!";
            Debug.Log("BOM");
            StartPowerStage();
        }
        else {
            feedbackText.text = "ERROU! Tente novamente ou saia. \nPressione 'R' para reiniciar";
            currentState = GameState.Failed;
        }
    }

    void StartPowerStage()
    {
        currentState = GameState.Power;
        powerSlider.gameObject.SetActive(true);
        feedbackText.text = "APERTE RÁPIDO!";
        powerSlider.value = 0;
    }

    void HandlePowerStage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            powerSlider.value += progressPerClick;
            if (powerSlider.value >= requiredProgress)
            {
                feedbackText.text = "SUCESSO! O objeto quebrou! \nPressione 'R' para reiniciar";
                currentState = GameState.Success;
            }
        }
    }
}
