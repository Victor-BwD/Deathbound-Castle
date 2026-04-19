using UnityEngine;

public class GoblinController : MonoBehaviour
{
    private enum GoblinState
    {
        Idle,
        Chasing,
        Attacking
    }

    [SerializeField] private Transform skin;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float attackRange = 1.9f;
    
    [SerializeField] private float attackCooldown = 1.5f; 
    
    [SerializeField] private Transform chaseBoundA;
    [SerializeField] private Transform chaseBoundB;
    [SerializeField] private bool lockBoundsOnAwake = true;

    private Animator _animator;
    private Transform _target;
    private string _currentAnimation;
    private GoblinState _state;
    
    private float _nextAttackTime; 
    
    private float _cachedMinBoundX;
    private float _cachedMaxBoundX;
    private bool _hasCachedBounds;

    private void Awake()
    {
        if (skin == null)
        {
            skin = transform;
        }
        
        _animator = skin.GetComponent<Animator>();

        if (lockBoundsOnAwake)
        {
            CacheBoundsFromTransforms();
        }

        SetState(GoblinState.Idle);
    }

    private void Update()
    {
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
            Debug.Log($"Checking Attack animation progress: {stateInfo.normalizedTime}");
            return stateInfo.normalizedTime >= 0.9f;
        }
        
        if (!_animator.IsInTransition(0))
        {
            Debug.Log($"Expected to be in Attack animation, but currently in {stateInfo.shortNameHash}");
            return true;
        }
        
        Debug.Log("Not in Attack animation or transition, treating as finished.");
        
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

        var scale = skin.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        skin.localScale = scale;
    }
}