using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private KeyCode interactKey = KeyCode.X;

    private Player player;
    private IInteractible currentTarget;

    private void Awake()
    {
        player = GetComponent<Player>();
        if(promptText != null ) promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        ScanForInteractables();

        if (currentTarget != null)
        {
            if (promptText != null)
            {
                promptText.text = currentTarget.GetInteractionPrompt();
                promptText.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(interactKey))
            {
                currentTarget.Interact(player);
            }
        }

        else if(promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    private void ScanForInteractables()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);

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

        if (currentTarget != closest)
        {
            if (currentTarget != null)
            {
                currentTarget.Highlight(false);
            }

            currentTarget = closest;

            if (currentTarget != null)
            {
                currentTarget.Highlight(true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
