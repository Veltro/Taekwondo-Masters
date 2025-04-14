using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI beltText;

    public void UpdateXP(int currentXP, int maxXP)
    {
        xpBar.maxValue = maxXP;
        xpBar.value = currentXP;
    }

    public void UpdateBelt(string belt)
    {
        beltText.text = "Faixa: " + belt;
    }
}