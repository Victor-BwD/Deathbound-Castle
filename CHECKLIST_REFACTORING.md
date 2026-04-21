# ✅ CHECKLIST DE REFATORAÇÃO - Tracker de Progresso

## 📊 Visão Geral

**Projeto**: Deathbound-Castle  
**Status**: 🔴 Não iniciado  
**Progresso**: 0%  
**Próximo**: Ler SUMARIO_PROBLEMAS.md

---

## 🟢 FASE 0: PREPARAÇÃO (Dia 1)

Estimado: 2-4 horas

- [ ] Leu `SUMARIO_PROBLEMAS.md`
- [ ] Leu `ARQUITETURA_VISUAL.md`
- [ ] Entendeu os 10 problemas principais
- [ ] Validou que refatoração é necessária
- [ ] Fez backup do projeto
- [ ] Criou branch Git: `refactor/phase-1-health`
- [ ] Slack/Notificou time que vai refatorar por 2-3 dias

**Checklist Completado**: ___% (0/7)

---

## 🔴 FASE 1: HEALTH SYSTEM (Dias 2-3)

Estimado: 8-12 horas

### 1.1 Criar Infraestrutura Base

- [ ] Criou pasta: `Assets/Game/Core/`
- [ ] Criou pasta: `Assets/Game/Core/Characters/`
- [ ] Criou pasta: `Assets/Game/Core/Services/`
- [ ] Criou pasta: `Assets/Game/Core/AI/`
- [ ] Criou pasta: `Assets/Game/Features/`

### 1.2 Criar Componentes Novos

- [ ] Criou `HealthComponent.cs` (copiar de GUIA_PRATICO_FASE1.md)
  - [ ] Compila sem erros
  - [ ] Tem UnityEvents
  - [ ] Tem TakeDamage e Heal
- [ ] Criou `AnimationComponent.cs` (copiar de GUIA_PRATICO_FASE1.md)
  - [ ] Compila sem erros
  - [ ] Tem PlayAnimation com cache
  - [ ] Tem IsInState
- [ ] Criou `ServiceLocator.cs` (copiar de GUIA_PRATICO_FASE1.md)
  - [ ] Compila sem erros
  - [ ] Funciona Register/Get

### 1.3 Refatorar Classes Existentes

- [ ] Refatorou `Characters.cs`
  - [ ] Add HealthComponent reference
  - [ ] Add AnimationComponent reference
  - [ ] Mantém compatibility com velho código
  - [ ] Compila sem erros
- [ ] Refatorou `EnemyCharacter.cs`
  - [ ] Usa novo health system
  - [ ] Dispara eventos de morte
  - [ ] Compila sem erros

### 1.4 Integrar ServiceLocator

- [ ] Modificou `SoundManager.cs`
  - [ ] Registra ServiceLocator no Awake
  - [ ] Compila sem erros
- [ ] Modificou `SoulManager.cs` (opcional para Fase 1)
  - [ ] Registra ServiceLocator no Awake
  - [ ] Compila sem erros

### 1.5 Adaptações de Cenas

- [ ] Selecionou 1 inimigo para testar (ex: Ghost)
- [ ] Adicionou `HealthComponent` no GameObject
  - [ ] Configurou maxHealth = 3
- [ ] Adicionou `AnimationComponent` no skin child
- [ ] Testou em Play:
  - [ ] [ ] Inimigo pode ser atacado (3x)
  - [ ] [ ] Inimigo toma dano visualmente
  - [ ] [ ] Inimigo morre após 3 hits
  - [ ] [ ] Animação "Die" toca
  - [ ] [ ] Nenhum erro na Console

### 1.6 Teste Completo

- [ ] Game funciona sem erros novos
- [ ] Nenhum regression (cenas antigas ainda funcionam)
- [ ] Git commit: `"refactor(Fase1): Health system + ServiceLocator"`

**Checklist Fase 1 Completado**: ___% (0/30)

---

## 🟡 FASE 2: PLAYER COMPONENTS (Dias 4-6)

Estimado: 12-16 horas

[ ] FASE 1 completada ← **PRÉ-REQUISITO**

### 2.1 Separar PlayerMovement

- [ ] Criou `InputComponent.cs`
- [ ] Criou `GroundCheckComponent.cs`
- [ ] Criou `MovementComponent.cs`
- [ ] Criou `JumpComponent.cs`
- [ ] Criou `DashComponent.cs`
- [ ] Remover código velho de `PlayerMovement.cs` (depois safe)

### 2.2 Adaptar Player Controller

- [ ] `PlayerController.cs` atualizado para usar novos componentes
- [ ] `PlayerCombo.cs` integrado ou refatorado
- [ ] Testado:
  - [ ] Player move esquerda/direita
  - [ ] Player jump
  - [ ] Player dash
  - [ ] Player combo funciona
  - [ ] Sem erros na Console

### 2.3 Morte do Player

- [ ] `PlayerHealth.cs` refatorado para usar HealthComponent
- [ ] Testado:
  - [ ] Player toma dano
  - [ ] Player UI atualiza
  - [ ] Player morre após 3 hits
  - [ ] Game Over screen aparece

### 2.4 Git Commit

- [ ] Git commit: `"refactor(Fase2): Player components separation"`

**Checklist Fase 2 Completado**: ___% (0/20)

---

## 🟠 FASE 3: AI STATE MACHINE (Dias 7-10)

Estimado: 16-20 horas

[ ] FASE 1 completada ← **PRÉ-REQUISITO**

### 3.1 Criar AI Genérica

- [ ] Criou `AIState.cs` (abstract base)
- [ ] Criou `AIStateMachine.cs`
- [ ] Criou `IdleState.cs`
- [ ] Criou `ChaseState.cs`
- [ ] Criou `AttackState.cs`
- [ ] Todos compilam sem erros

### 3.2 Criar EnemyAIController Base

- [ ] Criou `EnemyAIController.cs`
  - [ ] Herda de MonoBehaviour
  - [ ] Implementa IAttackable
  - [ ] Usa HealthComponent
  - [ ] Usa AnimationComponent
  - [ ] Usa AIStateMachine
  - [ ] Compila sem erros
- [ ] Testado em 1 inimigo (ex: Bat)

### 3.3 Migrar Inimigos (Um por um)

**Bat:**
- [ ] Criou `BatAI.cs` que herda de EnemyAIController
- [ ] Testado:
  - [ ] Bat segue player
  - [ ] Bat ataca
  - [ ] Bat morre
  - [ ] Sem erros
- [ ] Git commit: `"refactor(Fase3): Bat migrated to AI system"`

**Ghost:**
- [ ] Criou `GhostAI.cs`
- [ ] Testado (movimento, ataque, morte)
- [ ] Git commit: `"refactor(Fase3): Ghost migrated"`

**Goblin:**
- [ ] Criou `GoblinAI.cs`
- [ ] Testado (movimento, ataque, morte, bounds)
- [ ] Git commit: `"refactor(Fase3): Goblin migrated"`

**Keeper:**
- [ ] Criou `KeeperAI.cs`
- [ ] Integrou `KeeperSounds.cs`
- [ ] Testado (movimento, ataque, som, morte)
- [ ] Git commit: `"refactor(Fase3): Keeper migrated"`

### 3.4 Cleanup Velho

- [ ] Deletou `BatController.cs`
- [ ] Deletou `GhostController.cs`
- [ ] Deletou `GoblinController.cs`
- [ ] Deletou `KeeperController.cs`
- [ ] Deletou `ExampleEnemyController.cs` (se não usar)

**Checklist Fase 3 Completado**: ___% (0/30)

---

## 🟠 FASE 4: UNIFICAR TRAPS (Dia 11-12)

Estimado: 6-8 horas

[ ] FASE 1 completada ← **PRÉ-REQUISITO**

### 4.1 Criar TrapBase

- [ ] Criou `TrapBase.cs` em `Core/Trap/`
- [ ] Tem logic comum
- [ ] Compila sem erros

### 4.2 Refatorar Traps

**BearTrap:**
- [ ] Refatorou para herdar TrapBase
- [ ] Testado:
  - [ ] Stun player
  - [ ] Dano correto
  - [ ] Som toca
  - [ ] Release funciona
- [ ] Git commit: `"refactor(Fase4): BearTrap unified"`

**FireTrap:**
- [ ] Refatorou para herdar TrapBase
- [ ] Testado:
  - [ ] Dano no contato
  - [ ] Sem som (customizar se necessário)
- [ ] Git commit: `"refactor(Fase4): FireTrap unified"`

**SpikeTrap:**
- [ ] Refatorou para herdar TrapBase
- [ ] Testado (dano, morte, som)
- [ ] Git commit: `"refactor(Fase4): SpikeTrap unified"`

**SpikeTrap2:**
- [ ] Refatorou para herdar TrapBase
- [ ] Testado
- [ ] Consideró unificar com SpikeTrap (apenas 1 versão)

### 4.3 Cleanup

- [ ] Deletou duplicatas se houver
- [ ] Confirmou sem erros

**Checklist Fase 4 Completado**: ___% (0/18)

---

## 🟠 FASE 5: REORGANIZAR PASTAS (Dia 13)

Estimado: 4-6 horas

- [ ] Moveu todos characters para `Assets/Game/Core/Characters/`
- [ ] Moveu todos managers para `Assets/Game/Core/Services/`
- [ ] Moveu todos AI/States para `Assets/Game/Core/AI/`
- [ ] Moveu todos inimigos para `Assets/Game/Features/Enemies/`
- [ ] Moveu todos Traps para `Assets/Game/Features/Enemies/Traps/`
- [ ] Moveu Player para `Assets/Game/Features/Player/`
- [ ] Centralizou Prefabs em `Assets/Game/Prefabs/`
- [ ] Atualizou todos os namespaces
- [ ] Compilado sem erros
- [ ] Testado todas as cenas
- [ ] Git commit: `"refactor(Fase5): Reorganize folder structure"`

**Checklist Fase 5 Completado**: ___% (0/12)

---

## 🟢 FASE 6: POLISH & OPTIMIZE (Dia 14-15)

Estimado: 8-10 horas

### 6.1 Performance

- [ ] Executou Unity Profiler:
  - [ ] [ ] Sem memory leaks
  - [ ] [ ] FPS estável
  - [ ] [ ] Sem GC spikes anormais
- [ ] Evitou FindObjectOfType resquícios
- [ ] Evitar GetComponent em loops se possível

### 6.2 Cleanup Code

- [ ] Removeu usando statements não usados
- [ ] Removeu comentários de debug
- [ ] Adicionou XML documentation em componentes públicos
- [ ] Rodar formatter (se tiver EditorConfig)

### 6.3 Teste Final Completo

- [ ] Testou todas as cenas do jogo
- [ ] Testou todos os inimigos (Bat, Ghost, Goblin, Keeper)
- [ ] Testou todos os Traps (Bear, Fire, Spike)
- [ ] Testou morte do player (Game Over scene)
- [ ] Testou muda de cenas
- [ ] Testou SoulManager (ganhar almas, perder almas)
- [ ] Testou SoundManager (sons funcionando)
- [ ] NENHUM erro na Console

### 6.4 Documentação

- [ ] Adicionou comentários em componentes públicos
- [ ] Criou um `ARCHITECTURE.md` explicando nova estrutura
- [ ] Documentou como adicionar novo inimgo/trap

### 6.5 Final Git

- [ ] Git commit: `"refactor(Fase6): Polish & optimize"`
- [ ] Git commit: `"docs: Architecture documentation"`

**Checklist Fase 6 Completado**: ___% (0/20)

---

## 📈 RESUMO DE PROGRESSO

```
FASE          STATUS        PROGRESSO    DURAÇÃO
─────────────────────────────────────────────────
Fase 0        ⬜ Não feito    0/7          2-4h
Fase 1        ⬜ Não feito    0/30         8-12h
Fase 2        ⬜ Não feito    0/20         12-16h
Fase 3        ⬜ Não feito    0/30         16-20h
Fase 4        ⬜ Não feito    0/18         6-8h
Fase 5        ⬜ Não feito    0/12         4-6h
Fase 6        ⬜ Não feito    0/20         8-10h
─────────────────────────────────────────────────
TOTAL         ⬜ Não feito    0/157        56-76h
```

**Tempo Estimado**: 2-3 semanas (8-10 horas/dia)  
**Tempo Realista**: 3-4 semanas (5-6 horas/dia)

---

## 🎯 METAS POR SEMANA

### SEMANA 1: Foundation
- [ ] Fase 0 completa (Dia 1)
- [ ] Fase 1 completa (Dias 2-3)
- [ ] Fase 2 progresso (Dias 4-5)

**Meta**: Sistema de Health funcionando, Player components iniciados

### SEMANA 2: AI System
- [ ] Fase 2 completa (Dias 6-7)
- [ ] Fase 3 progresso (Dias 8-10)

**Meta**: Todos os inimigos usando novo AI system

### SEMANA 3: Cleanup
- [ ] Fase 3 completa (Dias 11-12)
- [ ] Fase 4 completa (Dias 13-14)
- [ ] Fase 5 progresso (Dias 15)

**Meta**: Código antigo deletado, pasta reorganizada

### SEMANA 4: Polish
- [ ] Fase 5 completa (Dia 16)
- [ ] Fase 6 completa (Dias 17-18)
- [ ] Testes finais e merge para main

**Meta**: Codebase refatorado, pronto para produção

---

## 🔴 RED FLAGS - Coisas que Dariam Errado

Se você notar isto, PAUSE e revise:

- [ ] ❌ Game crashes ao abrir cena
  - **Solução**: Deletou asset que script usava? Restaure do Git
- [ ] ❌ FindObjectOfType/GameObject.Find ainda sendo usado
  - **Solução**: Substitua por ServiceLocator ou cache no Awake
- [ ] ❌ Compilação com muitos erros
  - **Solução**: Commit anterior funciona? Reverter com `git checkout`
- [ ] ❌ Inimigos não movem/atacam/morrem
  - **Solução**: Verifique HealthComponent foi adicionado no Inspector
- [ ] ❌ Animator errors na Console
  - **Solução**: AnimationComponent está no GameObject certo? (skin child)
- [ ] ❌ Performance piorou drasticamente
  - **Solução**: Use Profiler, pode ter FindObjectOfType em loop

---

## 📝 NOTAS DE DESENVOLVIMENTO

**Semana 1:**
```
Dia 1: ✓ Preparação
Dia 2: ✓ HealthComponent + AnimationComponent + ServiceLocator criados
Dia 3: ✓ Characters.cs refatorado, Ghost testado com novo system
Dia 4: ▢ PlayerMovement components começados
Dia 5: ▢ Player components terminados e testados
```

**Semana 2:**
```
Dia 6: ▢ AIStateMachine base criada
Dia 7: ▢ Bat migrado para BatAI
Dia 8: ▢ Ghost, GoblinAI, KeeperAI migrados
Dia 9: ▢ Testado todos inimigos
Dia 10: ▢ Inimigos antigos deletados
```

**Semana 3:**
```
Dia 11: ▢ Traps refatorados
Dia 12: ▢ Traps testados
Dia 13: ▢ Pastas reorganizadas
Dia 14: ▢ Namespaces atualizados
Dia 15: ▢ Compilado sem erros
```

**Semana 4:**
```
Dia 16: ▢ Performance profiling
Dia 17: ▢ Cleanup e documentação
Dia 18: ▢ Testes finais, merge para main
```

---

## ✅ ASSINATURA DE CONCLUSÃO

Quando TUDO estiver feito:

```
Refatoração Completada: _____ (data)
Desenvolvedor: _____
Tempo Total Gasto: _____ horas
Bugs Encontrados: _____
Commits feitos: _____
Código Duplicação Antes: ~500 linhas
Código Duplicação Depois: ~50 linhas
Satisfação: _____ / 10
```

---

**COMECE AGORA!** 🚀

1. Print este arquivo
2. Abre o GUIA_PRATICO_FASE1.md
3. Comece Fase 0 hoje! ✅

---

**Versão**: 1.0  
**Criado em**: 2026-04-19  
**Status**: 🟢 Pronto para usar  
**Última atualização**: Now

