using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Belts currentbelt;

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
            Debug.Log("Initialize belt: " + localizedString.GetLocalizedString());
        }
        else
        {
            Debug.LogError($"Falha ao carregar a string localizada para a chave: {beltKey}");
        }
    }
    public void UpdateBeltText()
    {
        if (!AreUIComponentsValid()) return;

        // Atualiza o texto da faixa
        if (localizedString != null)
        {
            beltText.text = localizedString.GetLocalizedString();
            Debug.Log("UpdateUI belt: " + localizedString.GetLocalizedString());
        }
    }
}