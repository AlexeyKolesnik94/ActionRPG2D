using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour {
    
    [SerializeField] private Animator animator;

    private const float SpeedMove = 10f;
    private Vector2 _moveDirection;
    private bool _isFacingRight;
   
    private Controls _controls;
    private readonly CompositeDisposable _disposable = new CompositeDisposable();
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int FastAttackAnim = Animator.StringToHash("FastAttack");
    private static readonly int HeavyAttackAnim = Animator.StringToHash("HeavyAttack");
    
    


    private void Awake() {
        _controls = new Controls();

        _controls.Player.FastAttack.performed += context => FastAttack();
        _controls.Player.HeavyAttack.performed += context => HeavyAttack();
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

    private void FastAttack() {
        animator.SetTrigger(FastAttackAnim);
    }
    private void HeavyAttack() {
        animator.SetTrigger(HeavyAttackAnim);
    }

    private void Flip() {
        _isFacingRight = !_isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}