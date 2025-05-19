using UnityEngine;

[RequireComponent(typeof(PlayerEconomy))]
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyboardInput input;

    private PlayerEconomy playerEconomy;
    private IInteractible currentTarget;

    private void Awake()
    {
        playerEconomy = GetComponent<PlayerEconomy>();
    }

    private void Update()
    {
        ScanForInteractables();
        if (currentTarget != null)
        {
            if (input.InteractPressed)
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
