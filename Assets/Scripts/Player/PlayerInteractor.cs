using UnityEngine;

[RequireComponent(typeof(PlayerInventoryWallet))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.X;

    private PlayerInventoryWallet playerEconomy;
    private IInteractible currentTarget;

    private void Awake()
    {
        playerEconomy = GetComponent<PlayerInventoryWallet>();
    }

    private void Update()
    {
        ScanForInteractables();
        if (currentTarget != null)
        {
            if (Input.GetKeyDown(interactKey))
            {
                currentTarget.Interact(playerEconomy);
            }
        }
    }

    private void ScanForInteractables()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);

        var closest = GetClosestInteractible(hits);

        if (currentTarget != closest)
        {
            if (currentTarget != null) currentTarget.Highlight(false);
            currentTarget = closest;
            if (currentTarget != null) currentTarget.Highlight(true);
        }
    }

    private IInteractible GetClosestInteractible(Collider2D[] hits)
    {
        IInteractible closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IInteractible interactible))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = interactible;
                }
            }
        }
        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
