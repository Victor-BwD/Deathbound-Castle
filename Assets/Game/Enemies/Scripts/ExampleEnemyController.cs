using UnityEngine;

/// <summary>
/// Exemplo de como criar um novo tipo de inimigo usando IAttackable
/// Este inimigo fica parado até ser atacado, então persegue o jogador por um tempo
/// </summary>
public class ExampleEnemyController : MonoBehaviour, IAttackable
{
    [Header("Movement Settings")]
    [SerializeField] private Transform skin;
    [SerializeField] private float speedIdle = 1f;
    [SerializeField] private float speedChase = 3f;
    [SerializeField] private float chaseTime = 5f;

    private Characters characters;
    private Animator animator;
    private Collider2D enemyCollider;
    private bool isChasing;
    private float chaseTimer;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        characters = GetComponent<Characters>();
        animator = skin.GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Verificar morte
        if (characters.life <= 0)
        {
            HandleDeath();
            return;
        }

        // Atualizar perseguição
        if (isChasing)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0)
            {
                isChasing = false;
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            IdleBehavior();
        }
    }

    /// <summary>
    /// Comportamento quando inativo (pode ser personalizado)
    /// </summary>
    private void IdleBehavior()
    {
        // Exemplo: ficar parado ou patrulhar devagar
        // Implemente seu comportamento aqui
    }

    /// <summary>
    /// Perseguir a última posição conhecida do jogador
    /// </summary>
    private void ChasePlayer()
    {
        Vector3 direction = (lastPlayerPosition - transform.position).normalized;
        
        // Virar para o jogador
        if (direction.x > 0)
        {
            skin.localScale = new Vector3(Mathf.Abs(skin.localScale.x), skin.localScale.y, skin.localScale.z);
        }
        else if (direction.x < 0)
        {
            skin.localScale = new Vector3(-Mathf.Abs(skin.localScale.x), skin.localScale.y, skin.localScale.z);
        }

        // Mover em direção ao jogador
        transform.position = Vector3.MoveTowards(transform.position, lastPlayerPosition, speedChase * Time.deltaTime);
    }

    /// <summary>
    /// Implementação da interface IAttackable
    /// Chamado quando o jogador ataca este inimigo
    /// </summary>
    public void OnPlayerAttack(Vector3 attackerPosition)
    {
        // Ativar perseguição
        isChasing = true;
        chaseTimer = chaseTime;
        lastPlayerPosition = attackerPosition;
    }

    /// <summary>
    /// Lidar com a morte do inimigo
    /// </summary>
    private void HandleDeath()
    {
        if (animator != null)
        {
            animator.Play("Die", -1);
        }
        
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
        
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
}
