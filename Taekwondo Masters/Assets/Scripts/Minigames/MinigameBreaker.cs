using UnityEngine;

public class MinigameBreaker : MonoBehaviour
{
    public float maxForce = 100f; // Força máxima que o jogador pode alcançar
    public float requiredForce = 80f; // Força necessária para quebrar
    private float currentForce = 0f;

    void Update()
    {
        // Ao pressionar Espaço, calcula a força e verifica o sucesso
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentForce = Random.Range(0, maxForce);
            if (currentForce >= requiredForce)
            {
                Debug.Log("Sucesso! Você quebrou a madeira!");
                // Adicione lógica de feedback visual e transição
            }
            else
            {
                Debug.Log("Força insuficiente, tente novamente!");
            }
        }
    }
}