using UnityEngine;
using System.Collections;

public class PoomsaeAnimator : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Animator animator;
    [SerializeField] private MinigamePoomsaeNew minigameManager;
    
    [Header("Mapeamento de Comandos para Animações")]
    [SerializeField] private AnimationClip[] commandAnimations;
    [SerializeField] private string[] commandNames; // "Up", "Right", "Down", "Left"
    
    [Header("Transições")]
    [SerializeField] private float transitionTime = 0.1f;
    [SerializeField] private string idleAnimationName = "Idle";
    
    [Header("Feedback Visual Extra")]
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private Transform effectSpawnPoint;
    
    private Coroutine animationCoroutine;
    private bool isAnimating = false;
    
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (minigameManager == null) minigameManager = GetComponent<MinigamePoomsaeNew>();
        
        // Se não encontrar, busca na cena
        if (minigameManager == null) 
            minigameManager = FindObjectOfType<MinigamePoomsaeNew>();
    }
    
    // Chamado pelo MinigamePoomsaeNew quando um comando é acertado
    public void PlayCommandAnimation(string command, float performanceRating)
    {
        if (isAnimating) StopCoroutine(animationCoroutine);
        animationCoroutine = StartCoroutine(ExecuteAnimation(command, performanceRating));
    }
    
    IEnumerator ExecuteAnimation(string command, float rating)
    {
        isAnimating = true;
        
        // Encontra o índice do comando
        int index = System.Array.IndexOf(commandNames, command);
        
        if (index >= 0 && index < commandAnimations.Length && commandAnimations[index] != null)
        {
            // Toca a animação correspondente
            animator.CrossFade(commandAnimations[index].name, transitionTime);
            
            // Efeito visual baseado na performance
            if (impactEffectPrefab != null && rating >= 0.85f) // Perfect/Excellent
            {
                GameObject effect = Instantiate(impactEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
                Destroy(effect, 0.5f);
            }
            
            // Aguarda a animação terminar (ou tempo fixo)
            float animationLength = commandAnimations[index].length;
            yield return new WaitForSeconds(animationLength - transitionTime);
        }
        
        // Volta para idle
        animator.CrossFade(idleAnimationName, transitionTime);
        isAnimating = false;
    }
    
    // Para animações de erro (Miss)
    public void PlayMissAnimation()
    {
        if (isAnimating) return;
        StartCoroutine(MissAnimationRoutine());
    }
    
    IEnumerator MissAnimationRoutine()
    {
        isAnimating = true;
        
        // Treme o personagem ou toca animação de erro
        Vector3 originalPos = transform.position;
        float shakeAmount = 0.05f;
        
        for (int i = 0; i < 3; i++)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
            yield return new WaitForSeconds(0.05f);
        }
        
        transform.position = originalPos;
        isAnimating = false;
    }
}