using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoneyInstance : MonoBehaviour
{
    [SerializeField] private float attractRadius = 3f;
    [SerializeField] private float attractSpeed = 10f;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);

    private Vector2 dropForce = new Vector2(0, 0);
    private MoneyData moneyData;
    private Rigidbody2D rb;
    private Transform player;
    private bool isAttracting;
    private float remainingLifetime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Initialize(MoneyData moneyData, Vector2 dropForce)
    {
        this.moneyData = moneyData;
        this.dropForce = dropForce;

        remainingLifetime = moneyData.LifetimeSeconds;
        rb.AddForce(dropForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        TryAttractToPlayer();
        CalculateLifetime();
    }

    private bool TryAttractToPlayer()
    {
        if (player == null) return false;

        var playerOffsetPosition = player.position + offset;

        float distance = Vector2.Distance(transform.position, playerOffsetPosition);
        isAttracting = distance <= attractRadius;

        if (isAttracting)
        {
            Vector2 direction = (playerOffsetPosition - transform.position).normalized;
            rb.linearVelocity = direction * attractSpeed;
        }

        return isAttracting;
    }

    private void CalculateLifetime()
    {
        remainingLifetime -= Time.deltaTime;

        if (remainingLifetime <= 0) DeleteCoin();
    }

    private void DeleteCoin()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerEconomy player))
        {
            TryRecieveMoney(player);
            Destroy(gameObject);
        }
    }

    private bool TryRecieveMoney(PlayerEconomy player)
    {
        if(player == null) return false;
        player.AddMoney(moneyData.Amount);
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractRadius);
    }
}
