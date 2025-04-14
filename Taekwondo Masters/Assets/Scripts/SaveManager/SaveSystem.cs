using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public int playerXP;
    public string playerBelt;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.SetString("PlayerBelt", playerBelt);
        Debug.Log("Jogo salvo!");
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerXP"))
        {
            playerXP = PlayerPrefs.GetInt("PlayerXP");
            playerBelt = PlayerPrefs.GetString("PlayerBelt");
            Debug.Log("Jogo carregado: XP = " + playerXP + ", Faixa = " + playerBelt);
        }
    }
}