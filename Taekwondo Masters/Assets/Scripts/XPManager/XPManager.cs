using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public int level;
    public int currentXP;
    public int xpForNextLevel = 100;
    public float xpForNextLevelMultiplier = 1.2f;
    public Belts currentbelt;
    public int currentBeltXP;
    public int xpForNextBelt = 100;
    public float xpForNextBeltElegibleMultiplier = 1f;

    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text levelXpText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text beltText;
    [SerializeField] private TMP_Text beltXpText;

    private LocalizedString localizedString;
    private LocalizedString beltLocalizedString;

    private void Start()
    {
        if (!AreUIComponentsValid())
        {
            enabled = false;
            return;
        }

        // Inicialização segura
        if (xpForNextLevel <= 0) xpForNextLevel = 100;
        if (xpForNextBelt <= 0) xpForNextBelt = 100;

        // Subscribe to locale changes
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        // Carrega as strings localizadas no início
        StartCoroutine(InitializeLocalizedStrings());
    }

    private bool AreUIComponentsValid()
    {
        if (xpBar == null || levelXpText == null || levelText == null || beltText == null || beltXpText == null)
        {
            Debug.LogError("Componentes UI não atribuídos no XPManager!");
            return false;
        }
        return true;
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        // Reinitialize localized strings when the locale changes
        StartCoroutine(InitializeLocalizedStrings());
    }

    private void OnDestroy()
    {
        // Unsubscribe from locale changes to avoid memory leaks
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private IEnumerator InitializeLocalizedStrings()
    {
        // Configura a chave do enum atual
        string beltKey = $"belt.{currentbelt.ToString().ToLower()}"; // Exemplo: "belt.gub10"
        beltLocalizedString = new LocalizedString { TableReference = "UI_Labels", TableEntryReference = beltKey };

        // Aguarda o carregamento da string localizada
        var operation = beltLocalizedString.GetLocalizedStringAsync();
        yield return operation;

        if (operation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            // Atualiza a variável "belt" no LocalizedString principal
            if (localizedString == null)
            {
                localizedString = new LocalizedString { TableReference = "UI_Labels", TableEntryReference = "menu.belt" };
            }
            localizedString["belt"] = new StringVariable { Value = operation.Result };

            // Atualiza o texto da faixa após carregar a string localizada
            beltText.text = localizedString.GetLocalizedString();
            Debug.Log("Initialize belt:" + localizedString.GetLocalizedString());
        }
        else
        {
            Debug.LogError($"Falha ao carregar a string localizada para a chave: {beltKey}");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddXP(200);
            AddBeltXP(50);
            Debug.Log("Ganhou 100 XP de level e 100 de XP de faixa");
        }
    }

    public void UpdateUI()
    {
        if (!AreUIComponentsValid()) return;

        // Atualiza a barra de XP
        xpBar.maxValue = xpForNextLevel;
        xpBar.value = currentXP;

        // Atualiza o texto de XP
        levelXpText.text = $" {currentXP}/{xpForNextLevel}";
        levelText.text = $"Level: {level}";
        beltXpText.text = $" {currentBeltXP}/{xpForNextBelt}";

        // Atualiza o texto da faixa
        beltText.text = localizedString.GetLocalizedString();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        // Primeiro level-up para evitar inconsistências
        while (currentXP >= xpForNextLevel)
        {
            LevelUp();
        }

        // Atualiza UI
        UpdateUI();
    }

    public void AddBeltXP(int amount)
    {
        currentBeltXP += amount;

        // Depois verifica faixa e atualiza UI
        CheckBeltPromotion();
        UpdateUI();
    }
    

    public void LevelUp()
    {
        level++;
        currentXP -= xpForNextLevel;
        xpForNextLevel = Mathf.RoundToInt(xpForNextLevel * xpForNextLevelMultiplier);
        Debug.Log($"Level Up! Nível atual: {level}. XP restante: {currentXP}");
    }

    public void CheckBeltPromotion()
    {
        if (currentBeltXP >= xpForNextBelt && currentbelt != Belts.Dan10)
        {
            Debug.Log("Parabéns! Você está elegível para subir para a próxima faixa: " + GetNextBelt());
            BeltLevelUp();
        }
        else
        {
            int xpNeeded = xpForNextBelt - currentBeltXP;
            Debug.Log("Você ainda precisa de " + xpNeeded + " XP para a próxima faixa");
        }
    }

    public Belts GetNextBelt()
    {
        if (currentbelt == Belts.Dan10)
        {
            Debug.Log("Você já atingiu a faixa máxima!");
            return currentbelt; // Já está no máximo
        }
        return (Belts)((int)currentbelt + 1);
    }

    public void UpdateXPForNextBelt()
    {
        // Escala xpForNextBelt com um valor fixo de 200 para cada faixa
        xpForNextBelt = Mathf.RoundToInt(100 + (200 * (int)currentbelt * xpForNextBeltElegibleMultiplier));
        Debug.Log($"XP necessário para a próxima faixa ({currentbelt}): {xpForNextBelt}");
    }

    public void BeltLevelUp()
    {
        if (currentbelt < Belts.Dan10)
        {
            currentbelt++;
            // Atualiza o valor de xpForNextBelt com base na faixa atual
            UpdateXPForNextBelt();
            Debug.Log($"Promovido para faixa: {currentbelt}");

            StartCoroutine(InitializeLocalizedStrings());
        }
        else
        {
            Debug.Log("Você já atingiu a faixa máxima!");
        }
    }
}