using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerHealth health;
    private PlayerMana mana;
    private PlayerStats stats;
    private PlayerClass playerClass;
    public Inventory inventory;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("기본 정보")]
    public string playerName = "플레이어";

    [Header("체력")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("UI")]
    [SerializeField] private GameObject inventoryPanel;

    public PlayerHealth Health => health;
    public PlayerMana Mana => mana;
    public PlayerStats Stats => stats;
    public PlayerClass Class => playerClass;
    public Inventory Inventory => inventory;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
        mana = GetComponent<PlayerMana>();
        stats = GetComponent<PlayerStats>();
        playerClass = GetComponent<PlayerClass>();
        inventory = GetComponent<Inventory>();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (health == null) health = gameObject.AddComponent<PlayerHealth>();
        if (mana == null) mana = gameObject.AddComponent<PlayerMana>();
        if (stats == null) stats = gameObject.AddComponent<PlayerStats>();
        if (playerClass == null) playerClass = gameObject.AddComponent<PlayerClass>();
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();
    }

    void Start()
    {
        if (health != null)
        {
            health.OnHealthChanged += OnHealthChanged;
        }

        if (mana != null)
        {
            mana.OnManaChanged += OnManaChanged;
        }
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);
        Move(movement);
    }

    void Move(Vector2 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            rb.linearVelocity = direction.normalized * moveSpeed;

            if (animator != null)
            {
                animator.SetFloat("Speed", direction.magnitude);
                animator.SetFloat("MoveX", direction.x);
                animator.SetFloat("MoveY", direction.y);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    void Attack()
    {
        Debug.Log($"공격! (공격력: {stats.TotalAttackPower})");

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    void OnHealthChanged(int current, int max)
    {
        Debug.Log($"체력 변경: {current}/{max}");

        if (current <= 0)
        {
            Die();
        }
    }

    void OnManaChanged(int current, int max)
    {
        Debug.Log($"마나 변경: {current}/{max}");
    }

    void Die()
    {
        Debug.Log("플레이어 사망!");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        rb.linearVelocity = Vector2.zero;
        enabled = false;
    }
}