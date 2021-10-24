using UniRx;
using UnityEngine;


public class PlayerController : MonoBehaviour {
    
    [SerializeField] private Animator animator;

    private const float SpeedMove = 10f;
    private Vector2 _moveDirection;
    private bool _isFacingRight;
    

    private Controls _controls;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int AttackAnim = Animator.StringToHash("Attack");
    
    


    private void Awake() {
        _controls = new Controls();

        _controls.Player.Attack.performed += context => Attack();
    }

    private void Start() {
        Observable.EveryUpdate()
            .Subscribe(_ => {
                _moveDirection = _controls.Player.Move.ReadValue<Vector2>();
                Move(_moveDirection);
            }).AddTo(_disposable);
    }

    private void OnEnable() {
        _controls.Enable();
    }

    private void OnDisable() {
        _controls.Disable();
        _disposable?.Dispose();
    }


    private void Move(Vector2 direction) {
        
        //MOVE
        float scaledMoveSpeed = SpeedMove * Time.deltaTime;
        Vector3 moveDirection = new Vector3(direction.x, direction.y,0);
        transform.position += moveDirection * scaledMoveSpeed;
        animator.SetFloat(Speed, _moveDirection.sqrMagnitude);
        
        // FLIP
        switch (_isFacingRight) {
            case false when _moveDirection.x < 0:
            case true when _moveDirection.x > 0:
                Flip();
                break;
        }
    }

    private void Attack() {
        animator.SetTrigger(AttackAnim);
    }
    
    private void Flip() {
        _isFacingRight = !_isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}