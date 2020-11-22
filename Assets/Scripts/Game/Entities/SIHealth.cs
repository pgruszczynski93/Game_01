using System.Collections;
using System.Collections.Generic;
using SpaceInvaders;
using UnityEngine;

public class SIHealth : MonoBehaviour
{
    [SerializeField] protected EntitySetup _entitySetup;
    [SerializeField] protected SIDamageVFX _damageVFX;

    protected float _currentHealth;
    protected float _healthPercent;
    protected float _healthLossPercent;

    protected void Start() => Initialise();
    
    void Initialise()
    {
        _damageVFX.Initialise();
        SetMaxHealth();
    }

    public void SetMaxHealth()
    {
        _currentHealth = _entitySetup.entityMaxHealth;
        _damageVFX.ResetDamageVFX();
    }

    public void ApplyDamage(float damage)
    {
        _currentHealth -= damage;
        _healthPercent = _currentHealth / _entitySetup.entityMaxHealth;
        _healthLossPercent = 1 - _healthPercent;
        _damageVFX.SetDamageVFX(_healthLossPercent);
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }
}
