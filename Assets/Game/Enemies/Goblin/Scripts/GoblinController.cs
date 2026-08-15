using Core.Characters;
using Core.Combat;
using UnityEngine;

public class GoblinController : EnemyCharacter
{
    private enum GoblinState
    {
        Idle,
        Chasing,
        Attacking
    }

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float attackRange = 1.9f;
    
    [SerializeField] private float attackCooldown = 1.5f; 
    
    [SerializeField] private Transform chaseBoundA;
    [SerializeField] private Transform chaseBoundB;
    [SerializeField] private bool lockBoundsOnAwake = true;

    private Animator _animator;
    private EnemyAttackComponent _attackComponent;
    private Transform _target;
    private string _currentAnimation;
    private GoblinState _state;
    
    private float _nextAttackTime; 
    
    private float _cachedMinBoundX;
    private float _cachedMaxBoundX;
    private bool _hasCachedBounds;

    protected override void Awake()
    {
        base.Awake();

        if (Skin == null)
        {
            Skin = transform;
        }

        _animator = Skin.GetComponent<Animator>();
        _attackComponent = GetComponent<EnemyAttackComponent>();

        if (lockBoundsOnAwake)
        {
            CacheBoundsFromTransforms();
        }

        SetState(GoblinState.Idle);
    }

    private void Update()
    {
        if (Health.IsDead)
        {
            return;
        }

        if (!_target)
        {
            ClearTarget();
            return;
        }

        var directionToTargetX = _target.position.x - transform.position.x;
        UpdateFacing(directionToTargetX);

        switch (_state)
        {
            case GoblinState.Attacking:
                UpdateAttackingState();
                break;
            case GoblinState.Chasing:
                UpdateChasingState();
                break;
            case GoblinState.Idle:
                SetState(GoblinState.Chasing);
                UpdateChasingState();
                break;
            default:
                SetState(GoblinState.Idle);
                break;
        }
    }

    private void UpdateAttackingState()
    {
        var isAttackAnimationFinished = IsAttackAnimationFinished();
        if (!isAttackAnimationFinished)
        {
            return;
        }

        SetState(GoblinState.Chasing);
    }

    private void UpdateChasingState()
    {
        if (IsTargetInAttackRange())
        {
            if (Time.time >= _nextAttackTime)
            {
                if (_attackComponent != null && _target != null)
                {
                    var targetCollider = _target.GetComponent<Collider2D>();
                    if (targetCollider != null)
                    {
                        _attackComponent.DoAttack(targetCollider);
                    }
                }

                _nextAttackTime = Time.time + attackCooldown;
                SetState(GoblinState.Attacking);
            }
            else
            {
                PlayAnimation("Idle");
            }
            
            return; 
        }
        
        var current = transform.position;
        var targetX = GetClampedTargetX();
        var nextX = Mathf.MoveTowards(current.x, targetX, moveSpeed * Time.deltaTime);

        transform.position = new Vector3(nextX, current.y, current.z);

        if (Mathf.Abs(targetX - current.x) > 0.01f)
        {
            PlayAnimation("Run");
            return;
        }

        PlayAnimation("Idle");
    }

    private bool IsTargetInAttackRange()
    {
        var toTarget = (Vector2)(_target.position - transform.position);
        var attackRangeSqr = attackRange * attackRange;
        return toTarget.sqrMagnitude <= attackRangeSqr;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        _target = collision.transform;
        SetState(GoblinState.Chasing);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (_target != collision.transform)
        {
            return;
        }

        ClearTarget();
    }

    private float GetClampedTargetX()
    {
        var targetX = _target.position.x;

        if (_hasCachedBounds)
        {
            return Mathf.Clamp(targetX, _cachedMinBoundX, _cachedMaxBoundX);
        }

        if (!chaseBoundA || !chaseBoundB)
        {
            return targetX;
        }

        var minX = Mathf.Min(chaseBoundA.position.x, chaseBoundB.position.x);
        var maxX = Mathf.Max(chaseBoundA.position.x, chaseBoundB.position.x);
        return Mathf.Clamp(targetX, minX, maxX);
    }

    private void CacheBoundsFromTransforms()
    {
        if (!chaseBoundA || !chaseBoundB)
        {
            _hasCachedBounds = false;
            return;
        }

        _cachedMinBoundX = Mathf.Min(chaseBoundA.position.x, chaseBoundB.position.x);
        _cachedMaxBoundX = Mathf.Max(chaseBoundA.position.x, chaseBoundB.position.x);
        _hasCachedBounds = true;
    }

    private bool IsAttackAnimationFinished()
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack"))
        {
            return stateInfo.normalizedTime >= 0.9f;
        }

        if (!_animator.IsInTransition(0))
        {
            return true;
        }

        return false;
    }

    private void SetState(GoblinState nextState)
    {
        if (_state == nextState)
        {
            return;
        }

        _state = nextState;

        if (_state == GoblinState.Idle)
        {
            PlayAnimation("Idle");
        }
        else if (_state == GoblinState.Attacking)
        {
            PlayAnimation("Attack");
        }
    }

    private void ClearTarget()
    {
        _target = null;
        SetState(GoblinState.Idle);
    }

    protected override void OnDeath()
    {
        _animator.Play("Die", -1);
        this.enabled = false;

        base.OnDeath();
    }

    public void OnPlayerAttack(Vector3 attackerPosition)
    {
        // Pode ser usado para reação ao ataque do player
        _target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void PlayAnimation(string animationName)
    {
        if (_currentAnimation == animationName)
        {
            return;
        }

        _animator.Play(animationName, 0);
        _currentAnimation = animationName;
    }

    private void UpdateFacing(float directionX)
    {
        if (Mathf.Abs(directionX) < 0.01f)
        {
            return;
        }

        var scale = Skin.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        Skin.localScale = scale;
    }
}