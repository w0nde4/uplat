using UnityEngine;

[CreateAssetMenu(fileName = "Smoke Bomb", menuName = "Abilities/Smoke Bomb")]
public class BombSmokeAbility : SmokeAbility
{
    [Header("Bomb Properties")]
    [SerializeField] private float throwDistance = 5f;
    [SerializeField] private float throwSpeed = 15f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float damageTickTime = 0.5f;
    [SerializeField] private float smokeCloudDuration = 3f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Vector3 offset;

    [Header("Bomb Visuals")]
    [SerializeField] private GameObject smokeBombPrefab;
    [SerializeField] private GameObject smokeBombExplosionPrefab;

    protected override void PerformAbility(GameObject user)
    {
        base.PerformAbility(user);

        if (smokeBombPrefab == null) return;

        Vector2 throwDirection = Vector2.right;
        IDirectionable directionable = user.GetComponent<IDirectionable>();
        if (directionable != null)
        {
            throwDirection = directionable.GetFacingDirection();
        }

        var userPosition = user.transform.position;
        Vector3 instantiatePosition = userPosition + new Vector3(throwDirection.x, throwDirection.y, 0) * 0.5f + offset;
        GameObject smokeBomb = Instantiate(
            smokeBombPrefab,
            instantiatePosition,
            Quaternion.identity
        );

        BombSmokeProjectile bombProjectile = smokeBomb.GetComponent<BombSmokeProjectile>();
        bombProjectile.Initialize(
            user,
            throwDirection,
            throwSpeed,
            smokeCloudDuration + 0.5f,
            targetLayer
        );
        bombProjectile.SetupExtra(
            throwDistance,
            explosionRadius,
            damage,
            damageTickTime,
            smokeCloudDuration,
            smokeBombExplosionPrefab,
            smokeColor
        );
    }
}
