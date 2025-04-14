using UnityEngine;

public class MinigameBreaker : MonoBehaviour
{
    public float maxForce = 100f; // For�a m�xima que o jogador pode alcan�ar
    public float requiredForce = 80f; // For�a necess�ria para quebrar
    private float currentForce = 0f;

    void Update()
    {
        // Ao pressionar Espa�o, calcula a for�a e verifica o sucesso
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentForce = Random.Range(0, maxForce);
            if (currentForce >= requiredForce)
            {
                Debug.Log("Sucesso! Voc� quebrou a madeira!");
                // Adicione l�gica de feedback visual e transi��o
            }
            else
            {
                Debug.Log("For�a insuficiente, tente novamente!");
            }
        }
    }
}