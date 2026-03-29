# Sistema de Inimigos - Guia de Uso

## Visão Geral
Este sistema permite que qualquer inimigo reaja aos ataques do jogador de forma personalizada, sem modificar o código do `AttackCollider`.

## Como Criar um Novo Inimigo

### Passo 1: Implementar a Interface `IAttackable`

Qualquer inimigo que deve reagir a ataques precisa implementar a interface `IAttackable`:

```csharp
public class MeuNovoInimigo : MonoBehaviour, IAttackable
{
    public void OnPlayerAttack(Vector3 attackerPosition)
    {
        // Seu código personalizado aqui
    }
}
```

### Passo 2: Personalizar o Comportamento

No método `OnPlayerAttack(Vector3 attackerPosition)`, você pode:

- **Mudar direção de patrulha** (como o Keeper faz)
- **Iniciar perseguição** (exemplo: perseguir por 5 segundos)
- **Ativar modo defensivo** (exemplo: ficar parado e se proteger)
- **Fugir do jogador** (exemplo: correr para o lado oposto)
- **Chamar reforços** (exemplo: ativar outros inimigos próximos)
- **Qualquer comportamento customizado**

## Exemplos de Implementação

### Exemplo 1: Keeper (Muda Direção)
```csharp
public void OnPlayerAttack(Vector3 attackerPosition)
{
    float directionToPlayer = attackerPosition.x - transform.position.x;
    
    if (directionToPlayer > 0)
    {
        goRight = (b_point.position.x > a_point.position.x);
    }
    else if (directionToPlayer < 0)
    {
        goRight = (b_point.position.x < a_point.position.x);
    }
}
```

### Exemplo 2: Inimigo que Persegue
```csharp
public void OnPlayerAttack(Vector3 attackerPosition)
{
    isChasing = true;
    chaseTimer = 5f; // Perseguir por 5 segundos
    targetPosition = attackerPosition;
}
```

### Exemplo 3: Inimigo que Foge
```csharp
public void OnPlayerAttack(Vector3 attackerPosition)
{
    Vector3 fleeDirection = transform.position - attackerPosition;
    targetPosition = transform.position + fleeDirection.normalized * 10f;
    isFleeing = true;
}
```

### Exemplo 4: Inimigo que Chama Reforços
```csharp
public void OnPlayerAttack(Vector3 attackerPosition)
{
    // Alertar todos os inimigos próximos
    Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 10f);
    foreach (var col in nearbyEnemies)
    {
        IAttackable enemy = col.GetComponent<IAttackable>();
        if (enemy != null && enemy != this)
        {
            enemy.OnPlayerAttack(attackerPosition);
        }
    }
}
```

## Vantagens do Sistema

1. ? **Reutilizável** - Funciona para qualquer tipo de inimigo
2. ? **Desacoplado** - `AttackCollider` não precisa conhecer tipos específicos de inimigos
3. ? **Flexível** - Cada inimigo define seu próprio comportamento
4. ? **Escalável** - Adicionar novos inimigos não requer modificar código existente
5. ? **Testável** - Cada inimigo pode ser testado independentemente

## Checklist para Novo Inimigo

- [ ] Criar classe que herda de `MonoBehaviour`
- [ ] Implementar interface `IAttackable`
- [ ] Implementar método `OnPlayerAttack(Vector3 attackerPosition)`
- [ ] Adicionar `Characters` component no GameObject
- [ ] Adicionar tag "Enemy" no GameObject
- [ ] Definir comportamento de ataque/patrulha
- [ ] Testar reação ao ser atacado

## Notas Importantes

- O inimigo **precisa ter a tag "Enemy"** no Unity
- O inimigo **precisa ter o component `Characters`**
- O `attackerPosition` é a posição do jogador quando atacou
- Use `attackerPosition` para calcular direções e distâncias
