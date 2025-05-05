using System.Collections;
using UnityEngine;

public class HitStopEffect : MonoBehaviour, IHitFeedback
{
    [SerializeField] private float hitStopDuration = 0.1f;
    [SerializeField] private float timeScale = 0.1f;
    [SerializeField] private bool affectGlobalTime = false;

    private float originalTimeScale;
    private bool isInHitStop = false;

    public void ApplyEffect(GameObject target, GameObject damager, HitFeedbackContext context)
    {
        if (!isInHitStop)
        {
            StartCoroutine(ApplyHitStop());
        }
    }

    private IEnumerator ApplyHitStop()
    {
        isInHitStop = true;

        if (affectGlobalTime)
        {
            originalTimeScale = Time.timeScale;
            Time.timeScale = timeScale;
        }
        else
        {
            //specific for smb
        }

        yield return new WaitForSecondsRealtime(hitStopDuration);

        if (affectGlobalTime)
        {
            Time.timeScale = originalTimeScale;
        }

        isInHitStop = false;
    }
}
