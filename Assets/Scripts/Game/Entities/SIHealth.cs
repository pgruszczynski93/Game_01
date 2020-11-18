using System.Collections;
using System.Collections.Generic;
using SpaceInvaders;
using UnityEngine;

public class SIHealth : MonoBehaviour
{
    [SerializeField] protected EntitySetup _entitySetup;
    [SerializeField] protected SIDamageVFX _damageVFX;

    protected float _currentHealth;

    protected void Start() => Initialise();
    
    void Initialise()
    {
        SetMaxHealth();
    }

    public void SetMaxHealth()
    {
        _currentHealth = _entitySetup.entityMaxHealth;
    }

    public void ApplyDamage(float damage)
    {
        _currentHealth -= damage;
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }
}
