using UnityEngine;

[CreateAssetMenu(fileName = "novoLevelPoomsae", menuName = "Minigame/Poomsae")]
public class PoomsaeLevelData : ScriptableObject
{
    [Tooltip("Sequência de comandos da fase (ex: Up, Right, Down...)")]
    public string[] commandSequence;

    [Tooltip("Tempo total que o jogador tem para completar a sequência inteira")]
    public float totalLevelTime = 10f;

    [Tooltip("Dificuldade visual/tempo entre cada comando")]
    public float windowOfOpportunity = 1.5f; // Tempo de reação para cada nota
}
