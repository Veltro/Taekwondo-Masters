using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigamePoomsae : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionsText;
    public string[] commandSequence = { "Up", "Right", "Down", "Left" }; // Sequência de comandos
    private int currentIndex = 0;
    private float timer = 3f; // Tempo para o jogador seguir os comandos

    void Start()
    {
        DisplayNextCommand();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow) && commandSequence[currentIndex] == "Up")
        {
            ProcessCommand();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && commandSequence[currentIndex] == "Right")
        {
            ProcessCommand();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && commandSequence[currentIndex] == "Down")
        {
            ProcessCommand();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && commandSequence[currentIndex] == "Left")
        {
            ProcessCommand();
        }

        if (timer <= 0f)
        {
            Debug.Log("Tempo esgotado! Tente novamente.");
            ResetSequence();
        }
    }

    void DisplayNextCommand()
    {
        if (currentIndex < commandSequence.Length)
        {
            instructionsText.text = "Pressione: " + commandSequence[currentIndex];
        }
        else
        {
            Debug.Log("Parabéns! Você completou o Poomsae!");
            ResetSequence();
        }
    }

    void ProcessCommand()
    {
        currentIndex++;
        timer = 3f; // Reseta o tempo
        DisplayNextCommand();
    }

    void ResetSequence()
    {
        currentIndex = 0;
        timer = 3f;
        DisplayNextCommand();
    }
}
