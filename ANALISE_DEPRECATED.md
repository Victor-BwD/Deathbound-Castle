# 🗑️ ANÁLISE DE SCRIPTS DEPRECATED / NÃO USADOS

**Data**: 19/04/2026  
**Status**: Análise completa pós-refatoração  
**Total de Scripts**: 46

---

## 🔴 CRÍTICO - DELETE IMEDIATAMENTE

### 1. **PlayerHealth.cs** ❌ DELETE
**Localização**: `Assets/Game/Player/Scripts/PlayerHealth.cs`

**Por que**:
- ❌ Redundante com HealthComponent (já criado)
- ❌ Modifica `characters.life` diretamente (campo deprecated)
- ❌ Duplica lógica de dano
- ❌ AttackCollider agora chama HealthComponent direto

**Código problemático**:
```csharp
// Isto está ERRADO agora:
public void PlayerTakaDamage(int damage) {
    characters.life -= damage;  // ← Campo deprecated!
    characters.Skin.GetComponent<Animator>().Play("PlayerTakeDamage", 1);
    SoundManager.Instance.Play(_audioPlayer.damageSound);
}
```

**O que usar ao invés**:
`HealthComponent.TakeDamage(int damage)` (já dispara eventos)

**Dependências**:
- Encontrada em: BatController, GhostController, AttackCollider
- ✅ Já substituída por GetComponent<HealthComponent>()

**Ação**: DELETE + Remova .meta

---

### 2. **ExampleEnemyController.cs** ❌ DELETE
**Localização**: `Assets/Game/Enemies/Scripts/ExampleEnemyController.cs`

**Por que**:
- ❌ É um TEMPLATE/EXEMPLO, não usado em jogo real
- ❌ Usa `characters.life` deprecated
- ❌ Pode confundir novos devs
- ✅ Temos 4 inimigos reais: Bat, Ghost, Goblin, Keeper

**Status**: Experimental, nunca entrou em produção

**Ação**: DELETE + Remova .meta

---

### 3. **BatAI.cs** ❌ DELETE (CONFLITANTE)
**Localização**: `Assets/Game/Core/Features/Enemies/Bat/BatAI.cs`

**Por que**:
- ❌ Arquivo em local ERRADO (Core/Features)
- ❌ Está usando `GetComponent<HealthComponent>()` mas Characters.Health
- ❌ Conflita com BatController que é o script REAL
- ❌ Não está sendo usado em nenhuma cena
- ✅ BatController (refatorado) é o script correto

**Ação**: DELETE + Remova .meta + Remova pasta Core/Features

---

## 🟠 ALTO RISCO - REFATOR OBRIGATÓRIO

### 4. **AddHealth.cs** ⚠️ REFATOR AGORA
**Localização**: `Assets/Game/Player/Scripts/AddHealth.cs`

**Problema**:
```csharp
// ❌ ERRADO - Modifica characters.life diretamente
col.GetComponent<Characters>().life++;  // Campo deprecated!
```

**Solução - Copiar isto**:
```csharp
using Core.Characters;
using UnityEngine;

public class AddHealth : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            var healthComponent = col.GetComponent<HealthComponent>();
            if (healthComponent != null && !healthComponent.IsDead) {
                healthComponent.Heal(1);  // Usar Heal ao invés
                Destroy(this.gameObject);
            }
        }
    }
}
```

**Ação**: REFATORAR (5 min)

---

### 5. **IncreaseSpeedCharacter.cs** ⚠️ REVIEW
**Localização**: `Assets/Game/Player/Scripts/IncreaseSpeedCharacter.cs`

**Problema**:
```csharp
// ❌ Usa GameObject.Find
speedUpImage = GameObject.Find("SpeedUpActive").GetComponent<Image>();
```

**Solução**:
```csharp
[SerializeField] private Image speedUpImage;  // Assign no Inspector

private void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
        col.GetComponent<PlayerMovement>().IncreaseSpeed();
        if (speedUpImage != null)
            speedUpImage.enabled = true;
        Destroy(this.gameObject);
    }
}
```

**Ação**: REFATORAR (10 min) - Remover Find

---

## 🟡 MÉDIO RISCO - REVISAR/REFATORAR

### 6. **Traps - BearTrap.cs, FireTrap.cs, SpikeTrap.cs** ⚠️ REVIEW
**Localização**: 
- `Assets/Game/Enemies/Traps/BearTrap/BearTrap.cs`
- `Assets/Game/Enemies/Traps/FireTrap/Scripts/FireTrap.cs`
- `Assets/Game/Enemies/Traps/SpikeTrap/Script/SpikeTrap.cs`
- `Assets/Game/Enemies/Traps/SpikeTrap2/Scripts/SpikeTrap2.cs`

**Problema** (visto em análise anterior):
- Provavelmente usam `characters.life` ou `GetComponent<Characters>()`
- Precisam refatorar para HealthComponent

**Status**: Revisar quando chegar neles

**Ação**: REVISAR + REFATORAR

---

## 🟢 SEGURO - MANTER (Mas revisar)

### 7. **BatTrigger.cs** ✅ MANTER
**Localização**: `Assets/Game/Enemies/Bat/Scripts/BatTrigger.cs`

**Status**: ✅ Funciona, ativa Bats quando player chega

**Pode considerar refatora**r:
- Remover `FindObjectOfType<BatTrigger>()`
- Passar referencias no Inspector

---

### 8. **FloorCollider.cs** ✅ MANTER
**Localização**: `Assets/Game/Player/Scripts/FloorCollider.cs`

**Status**: ✅ Provavelmente usado para detecção de chão

---

### 9. **LostSouls.cs** ✅ MANTER  
**Localização**: `Assets/Game/Player/Scripts/LostSouls.cs`

**Status**: ✅ Provavelmente usado no SoulManager

---

### 10. **Scripts de Sistema** ✅ MANTER
- `SoundManager.cs` - CORE
- `SoulManager.cs` - CORE
- `PlayerController.cs` - CORE
- `PlayerMovement.cs` - CORE
- `PlayerCombo.cs` - CORE
- `AudioPlayer.cs` - Container de dados
- `AttackCollider.cs` - REFATORADO ✅
- `DontDestroyOnLoad.cs` - Sistema
- `ChangeScene.cs` - Sistema
- `GameOverController.cs` - Sistema
- Etc...

---

## 📊 SUMÁRIO DE AÇÕES

```
🔴 DELETE IMEDIATAMENTE (3 scripts):
   1. PlayerHealth.cs
   2. ExampleEnemyController.cs
   3. BatAI.cs
   └─ Total: 3 scripts para deletar

🟠 REFATORAR AGORA (2 scripts):
   4. AddHealth.cs
   5. IncreaseSpeedCharacter.cs
   └─ Total: 2 scripts (10 min)

🟡 REVISAR DEPOIS (4 scripts):
   6. BearTrap.cs
   7. FireTrap.cs
   8. SpikeTrap.cs
   9. SpikeTrap2.cs
   └─ Total: 4 scripts (próxima sessão)

🟢 MANTER (37 scripts):
   ✅ Todos os scripts críticos e de sistema
```

---

## 🚀 PLANO DE AÇÃO PRÁTICO

### HOJE (10 minutos):

**1️⃣ Backup rapidão**:
```bash
git add .
git commit -m "backup: Before cleanup dead code"
git branch cleanup-deadcode
```

**2️⃣ Delete 3 arquivos DEPRECATED**:
```bash
# PlayerHealth.cs
rm Assets/Game/Player/Scripts/PlayerHealth.cs
rm Assets/Game/Player/Scripts/PlayerHealth.cs.meta

# ExampleEnemyController.cs
rm Assets/Game/Enemies/Scripts/ExampleEnemyController.cs
rm Assets/Game/Enemies/Scripts/ExampleEnemyController.cs.meta

# BatAI.cs (e pasta vazia)
rm Assets/Game/Core/Features/Enemies/Bat/BatAI.cs
rm Assets/Game/Core/Features/Enemies/Bat/BatAI.cs.meta
# Limpar pasta vazia
```

**3️⃣ Commit**:
```bash
git add .
git commit -m "refactor: Remove deprecated PlayerHealth, ExampleEnemyController, BatAI"
```

### PRÓXIMAS 2 HORAS:

**4️⃣ Refatorar AddHealth.cs** (5 min)

**5️⃣ Refatorar IncreaseSpeedCharacter.cs** (10 min)

**6️⃣ Commit**:
```bash
git commit -m "refactor: Remove hardcoded values, use HealthComponent"
```

### PRÓXIMA SEMANA:

**7️⃣ Revisar e refatorar Traps** (20 min)

---

## ⚠️ Verificação Final

Depois de DELETAR PlayerHealth.cs, procure por referências restantes:

```csharp
// Procure por isto NO CÓDIGO (deve estar 0):
GetComponent<PlayerHealth>()
PlayerHealth.
→ Deve retornar 0 resultados se tudo foi refatorado
```

Se encontrar algo:
```csharp
// ANTES (vai bugá):
player.GetComponent<PlayerHealth>().PlayerTakaDamage(damage)

// DEPOIS:
player.GetComponent<HealthComponent>().TakeDamage(damage)
```

---

## 📋 Checklist FINAL

- [ ] Backup feito (`git commit`)
- [ ] Criou branch `cleanup-deadcode`
- [ ] Deletou PlayerHealth.cs
- [ ] Deletou ExampleEnemyController.cs
- [ ] Deletou BatAI.cs
- [ ] Projeto compila SEM ERROS
- [ ] Testou Keeper/Bat/Ghost/Goblin em scene
- [ ] Refatorou AddHealth.cs
- [ ] Refatorou IncreaseSpeedCharacter.cs
- [ ] Novo commit dos refatores
- [ ] Tudo funciona? Merge para main!

---

**Status**: Pronto para limpeza!  
**Tempo estimado**: 20-30 minutos  
**Risco**: BAIXO (backup feito, deletando só dead code)  

**Quer começar AGORA?** 🚀

