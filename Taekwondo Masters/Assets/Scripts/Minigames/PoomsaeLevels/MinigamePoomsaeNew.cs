using System.Collections; // Necessário para usar Coroutines
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePoomsaeNew : MonoBehaviour
{
    [Header("Configurações de Fase")]
    [SerializeField] private PoomsaeLevelData currentLevel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI textInstructions;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Image feedbackIcon; 
    [SerializeField] private Slider timerBar;     

    [Header("Personagem Animação")]
    [SerializeField] private PoomsaeAnimator characterAnimator;
    
    private bool _hasInstructionsText;
    private bool _hasFeedbackIcon;
    private bool _hasTimerBar;
    private bool _hasRatingText; // Novo check para o ratingText

    private int _currentIndex = 0;
    private float _levelTimer;
    private float _commandWindowTimer;
    private bool _isLevelActive = false;
    private bool _isProcessingResult = false; // Para evitar inputs durante a animação de nota
    
    void Awake()
    {
        _hasInstructionsText = textInstructions != null;
        _hasFeedbackIcon = feedbackIcon != null;
        _hasTimerBar = timerBar != null;
        _hasRatingText = ratingText != null;
    }

    void Start()
    {
        if (currentLevel != null)
            StartNewLevel();
        else
            Debug.LogError("Por favor, atribua um LevelData ao script!");
    }
    
    void StartNewLevel()
    {
        _currentIndex = 0;
        _levelTimer = currentLevel.totalLevelTime;
        _commandWindowTimer = currentLevel.windowOfOpportunity;
        _isLevelActive = true;
        DisplayNextCommand();
    }
    
    void Update()
    {
        if (!_isLevelActive || _isProcessingResult) return;

        _levelTimer -= Time.deltaTime;

        if (_hasTimerBar) 
            timerBar.value = _levelTimer / currentLevel.totalLevelTime;

        _commandWindowTimer -= Time.deltaTime;

        // Se o tempo do comando acabou (Timeout)
        if (_commandWindowTimer <= 0f)
        {
            StartCoroutine(ProcessResult(false, 0f)); // Trata como Miss
        }

        // Se o tempo total da fase acabou
        if (_levelTimer <= 0f)
        {
            HandleGameOver();
        }

        CheckInput();
    }

    void CheckInput()
    {
        string currentTarget = currentLevel.commandSequence[_currentIndex];
        string inputKey = "";
        if (Input.GetKeyDown(KeyCode.UpArrow)) inputKey = "Up";
        else if (Input.GetKeyDown(KeyCode.RightArrow)) inputKey = "Right";
        else if (Input.GetKeyDown(KeyCode.DownArrow)) inputKey = "Down";
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) inputKey = "Left";

        if (inputKey == "") return;
        StartCoroutine(inputKey == currentTarget ? ProcessResult(true, _commandWindowTimer) : ProcessResult(false, 0f)); // Trata erro de botão como Miss
    }

    // Coroutine para processar o resultado e dar um tempo para o jogador ver a nota
    IEnumerator ProcessResult(bool isSuccess, float timeRemaining)
    {
        _isProcessingResult = true;
        float percentage = (currentLevel.windowOfOpportunity > 0) ? (timeRemaining / currentLevel.windowOfOpportunity) : 0;

        string scoreText = "";
        Color textColor = Color.white;
        Color iconColor = Color.white;

        if (!isSuccess || percentage <= 0)
        {
            scoreText = "Miss";
            textColor = Color.red;
            iconColor = Color.red;
        }
        else if (percentage >= 0.85f) // 85% a 100%
        {
            scoreText = "Perfect!!!";
            textColor = Color.green;
            iconColor = Color.blue;
        }
        else if (percentage >= 0.70f) // 70% a 84.99%
        {
            scoreText = "Excelent!!";
            textColor = Color.purple;
            iconColor = Color.blue;
        }
        else if (percentage >= 0.50f) // 50% a 69.99%
        {
            scoreText = "Good!";
            textColor = Color.yellow;
            iconColor = Color.blue;
        }
        else if (percentage > 0.0099f) // 0.99% a 49.99%
        {
            scoreText = "Bad";
            textColor = Color.orange;
            iconColor = Color.blue;
        }

        // 🔥 Toca a animação baseada no resultado
        if (characterAnimator != null)
        {
            if (isSuccess && percentage > 0)
            {
                string currentCommand = currentLevel.commandSequence[_currentIndex];
                characterAnimator.PlayCommandAnimation(currentCommand, percentage);
            }
            else
            {
                characterAnimator.PlayMissAnimation();
            }
        }
        
        // Atualiza a UI de Rating e Ícone
        if (_hasRatingText) ratingText.text = scoreText;
        if (_hasRatingText) ratingText.color = textColor;
        if (_hasFeedbackIcon) feedbackIcon.color = iconColor;

        // Pequena espera para o jogador ler a nota (0.5 segundos)
        yield return new WaitForSeconds(0.5f);

        _currentIndex++;

        // Verifica se acabou a sequência de comandos
        if (_currentIndex >= currentLevel.commandSequence.Length)
        {
            HandleVictory();
        }
        else
        {
            // Reseta para o próximo comando
            _commandWindowTimer = currentLevel.windowOfOpportunity;
            DisplayNextCommand();
            _isProcessingResult = false;
        }
    }

    void DisplayNextCommand()
    {
        if(_hasInstructionsText) 
            textInstructions.text = "Siga o Ritmo: " + currentLevel.commandSequence[_currentIndex];
        if(_hasRatingText)
            ratingText.text = ""; // Limpa a nota anterior ao começar novo comando
    }

    void HandleVictory()
    {
        _isLevelActive = false;
        if(_hasInstructionsText) textInstructions.text = "PERFEITO! COMBO FINAL!";
        if(_hasFeedbackIcon) feedbackIcon.color = Color.green;
        Debug.Log("Vitória!");
    }

    void HandleGameOver()
    {
        _isLevelActive = false;
        if(_hasInstructionsText) textInstructions.text = "TEMPO ESGOTADO!";
        if(_hasFeedbackIcon) feedbackIcon.color = Color.red;
        Debug.Log("Fim de jogo!");
    }
}
