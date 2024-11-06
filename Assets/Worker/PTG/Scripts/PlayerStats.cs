using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerStats
{
    public float attackPower = 10f;
    public float defense = 5f;
    public float maxHealth = 100f;
    public float currentHealth;

    public float invincibleDuration = 1.5f; // 무적 시간 (초)
    private bool isInvincible = false;
    private float invincibleTimer = 0f;

    public UnityAction OnChangedHP;

    public UnityAction Dead;

    string HitAudioClip = "PlayerHit";

    //체력 초기화
    public PlayerStats()
    {
        //currentHealth = maxHealth;
    }

    //무적 시간
    public void UpdateInvincibleTime(float time)
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    //데미지 계산
    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return; // 무적 상태일 때는 피해 무시
        }

        float actualDamage = damage - defense;
        actualDamage = Mathf.Clamp(actualDamage, 0, actualDamage);
        currentHealth -= actualDamage;

        if (!string.IsNullOrEmpty(HitAudioClip))
            SoundManager.Instance.Play(Enums.ESoundType.SFX, HitAudioClip);

        OnChangedHP?.Invoke();

        invincibleTimer = invincibleDuration;
        isInvincible = true;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //회복
    public void Heal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += maxHealth * 1f;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // 체력이 최대치를 넘지 않도록 설정
            OnChangedHP?.Invoke();
        }
    }

    //사망
    private void Die()
    {
        Dead?.Invoke();
    }
}

