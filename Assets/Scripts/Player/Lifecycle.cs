using System.Linq;
using UnityEngine;

public class Lifecycle : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private MonoBehaviour[] disableOnDeath;
    [SerializeField] private BaseDamageInterceptor[] damageInterceptors;

    private DeathHandler deathHandler;
    private DamageHandler damageHandler;
    private Health health;

    public DamageHandler DamageHandler => 
        damageHandler ??= new DamageHandler(
        health,
        this,
        damageInterceptors.Cast<IDamageInterceptor>().ToList());

    public Health Health => 
        health ??= new Health(maxHealth);

    public DeathHandler DeathHandler => 
        deathHandler ??= new DeathHandler(
            health, 
            disableOnDeath.ToList());

    public void TakeDamage(int damage, GameObject damager)
    {
        DamageHandler.TakeDamage(damage, damager);
    }

    private void Awake()
    {
        _ = Health;
        _ = DeathHandler;
        _ = DamageHandler;
    }
}
