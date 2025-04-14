using UnityEngine;

public class XPManager : MonoBehaviour {
    public int currentXP = 0;
    public int xpForNextBelt = 100;
    public string currentBelt = "Faixa Branca";

    public void AddXP(int amount) {
        currentXP += amount;
        CheckBeltPromotion();
    }

    private void CheckBeltPromotion() {
        if (currentXP >= xpForNextBelt) {
            currentBelt = "Faixa Amarela"; // Mude conforme a lógica de progressão
            Debug.Log("Parabéns! Você subiu de faixa!");
            currentXP = 0; // Reseta XP para próxima faixa
        }
    }
}