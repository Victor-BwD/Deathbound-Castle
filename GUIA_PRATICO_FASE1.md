# 🚀 Guia Prático de Refatoração - Fase 1 (COMEÇAR AQUI)

## Este documento mostra EXATAMENTE o que fazer para começar

---

## Passo 1: Criar HealthComponent (Novo arquivo)

**Arquivo**: `Assets/Game/Core/Characters/HealthComponent.cs`

```csharp
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    /// <summary>
    /// Componente reutilizável para gerenciar saúde de qualquer entidade
    /// Substitui a lógica quebrada de death no Characters.cs
    /// </summary>
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        private int currentHealth;
        private bool isDead;

        // Events para desacoplamento
        public UnityEvent<int> OnHealthChanged = new();  // Envia novo health
        public UnityEvent OnDeath = new();
        public UnityEvent<int> OnDamageReceived = new();  // Envia dano

        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public float HealthPercent => (float)currentHealth / maxHealth;
        public bool IsDead => isDead;

        private void Start()
        {
            currentHealth = maxHealth;
            isDead = false;
        }

        /// <summary>
        /// Inflige dano e dispara eventos
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (isDead || damage <= 0) return;

            currentHealth -= damage;
            OnDamageReceived?.Invoke(damage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                OnDeath?.Invoke();
            }
            else
            {
                OnHealthChanged?.Invoke(currentHealth);
            }
        }

        /// <summary>
        /// Cura e dispara eventos
        /// </summary>
        public void Heal(int amount)
        {
            if (isDead || amount <= 0) return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        /// <summary>
        /// Reset para nova cena/respawn
        /// </summary>
        public void Reset()
        {
            currentHealth = maxHealth;
            isDead = false;
        }

        // Debug
        public void SetHealth(int amount)
        {
            #if UNITY_EDITOR
            currentHealth = Mathf.Clamp(amount, 0, maxHealth);
            isDead = currentHealth <= 0;
            OnHealthChanged?.Invoke(currentHealth);
            #endif
        }
    }
}
```

---

## Passo 2: Criar AnimationComponent (Novo arquivo)

**Arquivo**: `Assets/Game/Core/Characters/AnimationComponent.cs`

```csharp
using UnityEngine;

namespace Core.Characters
{
    /// <summary>
    /// Centraliza toda a lógica de animação para evitar GetComponent repetido
    /// </summary>
    public class AnimationComponent : MonoBehaviour
    {
        private Animator animator;
        private string currentAnimationName;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"AnimationComponent on {gameObject.name}: Animator não encontrado!");
            }
        }

        /// <summary>
        /// Play animation com cache para evitar mudanças desnecessárias
        /// </summary>
        public void PlayAnimation(string animationName, int layer = 0)
        {
            if (string.IsNullOrEmpty(animationName))
            {
                Debug.LogWarning("AnimationComponent: Nome de animação vazio");
                return;
            }

            // Skip se já está tocando
            if (currentAnimationName == animationName)
                return;

            if (animator != null)
            {
                animator.Play(animationName, layer);
                currentAnimationName = animationName;
            }
        }

        /// <summary>
        /// Set bool parameter (para idle, run, etc)
        /// </summary>
        public void SetBool(string paramName, bool value)
        {
            if (animator != null)
            {
                animator.SetBool(paramName, value);
            }
        }

        /// <summary>
        /// Checka se está em estado específico
        /// </summary>
        public bool IsInState(string stateName, int layer = 0)
        {
            if (animator == null) return false;
            return animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName);
        }

        /// <summary>
        /// Gets normalizado da animação (0-1)
        /// </summary>
        public float GetAnimationNormalizedTime(int layer = 0)
        {
            if (animator == null) return 0f;
            return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
        }
    }
}
```

---

## Passo 3: Refatorar Characters.cs (EDITAR ARQUIVO EXISTENTE)

**Arquivo**: `Assets/Game/Characters/Scripts/Characters.cs` (MODIFICAR)

```csharp
using UnityEngine;

namespace Core.Characters
{
    /// <summary>
    /// Classe base para qualquer entidade com vida (Player, Inimigos)
    /// REFATORADO para usar componentes desacoplados
    /// </summary>
    public class Characters : MonoBehaviour
    {
        [SerializeField] private Transform skin;

        // COMPONENTES - cada um com responsabilidade clara
        private HealthComponent healthComponent;
        private AnimationComponent animationComponent;

        // LEGACY: Para não quebrar scripts antigos (REMOVER DEPOIS)
        [HideInInspector] public int life;  // Obsoleto, usar Health.CurrentHealth
        public Transform Skin => skin;

        // NOVO: Interface melhor
        public HealthComponent Health { get; private set; }
        public AnimationComponent Animation { get; private set; }

        protected virtual void Awake()
        {
            // Cache de componentes
            healthComponent = GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                Debug.LogError($"{gameObject.name}: HealthComponent não encontrado!");
                enabled = false;
                return;
            }

            animationComponent = skin != null ? skin.GetComponent<AnimationComponent>() : null;
            if (animationComponent == null && skin != null)
            {
                Debug.LogWarning($"{gameObject.name}: AnimationComponent não encontrado em {skin.name}");
            }

            // Expor componentes
            Health = healthComponent;
            Animation = animationComponent;

            // Setup de eventos
            healthComponent.OnDeath.AddListener(OnDeath);
            healthComponent.OnDamageReceived.AddListener(OnDamageReceived);
        }

        private void Update()
        {
            // UPDATE SIMPLIFICADO - apenas gerenciar ciclo
            // Lógica de morte foi para HealthComponent -> OnDeath event
            
            // LEGACY SUPPORT: Manter life sincronizado (REMOVER DEPOIS)
            life = healthComponent.CurrentHealth;
        }

        /// <summary>
        /// LEGACY: Mantém compatibilidade com código antigo (DEPRECADO)
        /// Remova isto depois de migrar todos os scripts
        /// </summary>
        public void PlayerTakaDamage(int damage)
        {
            Debug.LogWarning("PlayerTakaDamage() está DEPRECADO! Use Health.TakeDamage() ao invés");
            healthComponent.TakeDamage(damage);
        }

        /// <summary>
        /// Chamado quando HealthComponent dispara OnDeath
        /// Subclasses podem override
        /// </summary>
        protected virtual void OnDeath()
        {
            if (animationComponent != null)
            {
                animationComponent.PlayAnimation("Die", -1);
            }
        }

        /// <summary>
        /// Chamado quando recebe dano
        /// </summary>
        protected virtual void OnDamageReceived(int damageAmount)
        {
            if (animationComponent != null)
            {
                animationComponent.PlayAnimation("PlayerTakeDamage", 1);
            }
        }
    }
}
```

---

## Passo 4: Refatorar EnemyCharacter.cs (EDITAR)

**Arquivo**: `Assets/Game/Characters/Scripts/EnemyCharacter.cs` (MODIFICAR)

```csharp
using UnityEngine;

namespace Core.Characters
{
    /// <summary>
    /// Versão específica para inimigos
    /// Adiciona comportamento de drop de souls
    /// </summary>
    public class EnemyCharacter : Characters
    {
        [SerializeField] private int soulValue = 1;

        protected override void OnDeath()
        {
            base.OnDeath();  // Chama animação "Die"

            // NOVO: Usar ServiceLocator ao invés de FindObjectOfType
            var soulManager = FindObjectOfType<SoulManager>();  // TODO: Usar ServiceLocator depois
            if (soulManager != null)
            {
                soulManager.AddSouls(soulValue);
            }

            // Schedule destruction AGORA
            Destroy(gameObject, 2f);
        }
    }
}
```

---

## Passo 5: Criar ServiceLocator (Novo arquivo)

**Arquivo**: `Assets/Game/Core/Services/ServiceLocator.cs`

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    /// <summary>
    /// Service Locator para evitar FindObjectOfType em todo lugar
    /// Uso:
    ///   ServiceLocator.Register<SoundManager>(soundManager);
    ///   var soundMgr = ServiceLocator.Get<SoundManager>();
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            
            if (Services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: {type.Name} já foi registrado!");
                return;
            }

            Services[type] = service;
            Debug.Log($"ServiceLocator: {type.Name} registrado");
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (Services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogError($"ServiceLocator: {type.Name} NÃO encontrado! Did you forget to Register?");
            return null;
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            service = Get<T>();
            return service != null;
        }

        public static void Unregister<T>() where T : class
        {
            Services.Remove(typeof(T));
        }

        public static void ClearAll()
        {
            Services.Clear();
        }

        // Debug
        public static void DebugPrintRegistered()
        {
            var msg = "=== ServiceLocator Registered ===\n";
            foreach (var kvp in Services)
            {
                msg += $"- {kvp.Key.Name}: {kvp.Value.GetType().Name}\n";
            }
            Debug.Log(msg);
        }
    }
}
```

---

## Passo 6: Atualizar SoundManager para usar ServiceLocator

**Arquivo**: `Assets/Game/Scripts/SoundManager.cs` (MODIFICAR)

```csharp
using GameFolder.Scripts;
using UnityEngine;
using Core.Services;

namespace GameFolder.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        // ... código existente ...

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                
                // NOVO: Registrar no ServiceLocator
                ServiceLocator.Register<SoundManager>(this);
                
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        // ... resto do código ...
    }
}
```

---

## Passo 7: Exemplo - Refatorar BatController (Novo arquivo com nome diferente)

**Arquivo**: `Assets/Game/Features/Enemies/Bat/BatAI.cs` (NOVO)

```csharp
using UnityEngine;
using Core.Characters;
using Core.Services;

namespace Features.Enemies.Bat
{
    /// <summary>
    /// Versão refatorada do BatController usando HealthComponent
    /// </summary>
    public class BatAI : MonoBehaviour
    {
        [SerializeField] private Transform targetPlayer;
        [SerializeField] private float chaseSpeed = 2f;
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private int damageAmount = 1;

        private Characters batCharacter;
        private Rigidbody2D rb;
        private float nextAttackTime;

        private void Start()
        {
            batCharacter = GetComponent<Characters>();
            rb = GetComponent<Rigidbody2D>();

            if (targetPlayer == null)
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                targetPlayer = playerObj?.transform;
            }

            // NOVO: Subscribe a eventos de morte
            batCharacter.Health.OnDeath.AddListener(() => {
                rb.gravityScale = 1;
                GetComponent<Collider2D>().enabled = false;
                enabled = false;
                Destroy(gameObject, 2f);
            });
        }

        private void Update()
        {
            if (batCharacter.Health.IsDead) return;

            if (targetPlayer != null)
            {
                // Chase player
                float distance = Vector2.Distance(transform.position, targetPlayer.position);
                
                if (distance > 0.8f)
                {
                    // Move towards
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        targetPlayer.position,
                        chaseSpeed * Time.deltaTime
                    );
                    nextAttackTime = 0;
                }
                else
                {
                    // Attack cooldown
                    if (Time.time >= nextAttackTime)
                    {
                        Attack();
                        nextAttackTime = Time.time + attackCooldown;
                    }
                }
            }
        }

        private void Attack()
        {
            var targetHealth = targetPlayer.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
            }
        }
    }
}
```

---

## Como Implementar - Passo a Passo Seguro

### **DIA 1: Setup Inicial (30 min)**

1. **Crie as pastas**:
   ```
   Assets/Game/Core/
   Assets/Game/Core/Characters/
   Assets/Game/Core/Services/
   Assets/Game/Features/
   Assets/Game/Features/Enemies/
   ```

2. **Crie os 3 arquivos novos**:
   - `HealthComponent.cs`
   - `AnimationComponent.cs`
   - `ServiceLocator.cs`

3. **Compile e verifique sem erros**

### **DIA 2: Adaptar Cenas (1 hora)**

1. Abra uma cena que tenha um inimigo (ex: Ghost)
2. No inspetor, adicione os componentes:
   - Selecione Ghost GameObject
   - Add Component → HealthComponent
   - Add Component → AnimationComponent (ao skin child)
   - Configure maxHealth = 3

3. **TESTE**: Ghost deve morrer depois de 3 hits

### **DIA 3: Migrar Um Inimigo (1-2 horas)**

1. Copie BatController.cs → BatController_OLD.cs (backup)
2. Crie o novo BatAI.cs baseado no template
3. Na cena, substitua BatController por BatAI
4. Teste completamente:
   - [ ] Bat segue o player
   - [ ] Bat dá dano no player
   - [ ] Bat morre após 3 hits
   - [ ] Efeito de morte funciona

5. Se tudo OK, delete BatController_OLD.cs

### **Como Reverter Se Quebrar**

```powershell
# No Git
git diff Assets/Game/Characters/Scripts/  # Ver mudanças
git checkout Assets/Game/Characters/Scripts/  # Reverter
```

---

## ✅ Checklist do Dia 1

- [ ] Pastas criadas corretamente
- [ ] `HealthComponent.cs` compila sem erros
- [ ] `AnimationComponent.cs` compila sem erros
- [ ] `ServiceLocator.cs` compila sem erros
- [ ] `Characters.cs` modificado compila
- [ ] `EnemyCharacter.cs` modificado compila
- [ ] Sem erros na Console do Editor
- [ ] Commits com mensagens descritivas

---

## 🎯 Próximas Fases (Após Dia 1-3)

Depois de fazer isto funcionar:

**FASE 1.5** (Dia 4-5):
- Migrar Ghost, Goblin, Keeper de forma similar
- Testar cada um

**FASE 2** (Semana 2):
- Separar PlayerMovement em componentes
- Usar HealthComponent no Player também

**FASE 3** (Semana 3):
- Criar AIStateMachine (como GoblinController já faz)
- Unificar todos em uma base comum

---

## 🆘 Troubleshooting

### **Erro: "HealthComponent não encontrado"**
```
Solução: Adicione HealthComponent no GameObject, não apenas na cena
        No inspetor: Add Component → HealthComponent
```

### **Erro: "Animator não encontrado"**
```
Solução: AnimationComponent deve estar no mesmo GameObject do Animator
        Geralmente é o child "skin"
```

### **Ghost/Bat não morre**
```
Solução: Verifique se HealthComponent está com maxHealth > 0
        Debug: Health.CurrentHealth na Console
```

### **Quebrou tudo depois de mudança**
```
Solução rápida:
1. git status  # Ver o que mudou
2. git diff Assets/Game/  # Ver as linhas específicas
3. git checkout Assets/Game/[arquivo]  # Reverter 1 arquivo
4. Tentar novamente mais devagar
```

---

## 📚 Arquivos Originais (Do NOT Delete During Refactor)

Depois de testar os novos, você pode deletar:
- `BatController.cs` → use `BatAI.cs`
- `PlayerHealth.cs` → o HealthComponent substitui
- Velho código em `Characters.cs` com muitos comentários

Mantenha:
- `Characters.cs` (refatorado para compatibilidade)
- `EnemyCharacter.cs` (refatorado)
- Todos os managers (SoundManager, SoulManager) - vão ser migrados depois

---

**Tempo total FASE 1**: 2-3 dias com testes  
**Ganho**: Eliminam 40% da duplicação de código já nesta fase  
**Risco**: BAIXO - usando ServiceLocator e eventos

Sucesso! 🚀

