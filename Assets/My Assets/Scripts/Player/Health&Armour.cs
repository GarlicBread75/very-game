using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthArmour : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHp;
    [SerializeField] int currentHp;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpCount;
    [SerializeField] GameObject deathEffect;

    [Space]

    [Header("Armour")]
    [SerializeField] int maxArmour;
    [SerializeField] int currentArmour;
    [SerializeField] Slider armourSlider;
    [SerializeField] TextMeshProUGUI armourCount;
    [SerializeField] float regenerationCd;
    float regenCd;
    bool canRegen;
    bool shieldBroken;

    [Space]

    [Header("Invulnerability")]
    [SerializeField] float invulnerableDuration;
    [SerializeField] int numberOfFlashes;
    [SerializeField] Color flashColor1;
    [SerializeField] Color flashColor2;
    [SerializeField] Vector2 knockback;
    [SerializeField] SpriteRenderer sr;
    Rigidbody2D rb;
    Color originalColour;

    [Space]

    [Header("Respawning")]
    [SerializeField] float respawnDelay;
    [SerializeField] Transform respawnPoint;
    [SerializeField] Transform temporaryPosition;

    [Space]

    [Header("Sounds")]
    [SerializeField] UnityEvent[] sounds;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalColour = sr.color;

        currentHp = maxHp;
        currentArmour = maxArmour;

        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        armourSlider.maxValue = maxArmour;
        armourSlider.value = currentArmour;

        regenCd = regenerationCd;
        StartCoroutine(RegenArmour());
    }

    void FixedUpdate()
    {
        hpSlider.value = currentHp;
        hpCount.text = $"{currentHp}/{maxHp}";
        armourSlider.value = currentArmour;
        armourCount.text = $"{currentArmour}/{maxArmour}";

        if (currentArmour == 0)
        {
            shieldBroken = true;
        }
        else
        if (currentArmour > 0)
        {
            shieldBroken = false;
        }

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        if (currentArmour > maxArmour)
        {
            currentArmour = maxArmour;
        }

        if (currentHp <= 0)
        {
            StartCoroutine(Respawn());
        }

        if (currentArmour < maxArmour)
        {
            if (regenCd > 0)
            {
                regenCd -= Time.fixedDeltaTime;
            }
            else
            {
                canRegen = true;
            }
        }
    }

    #region HitPoints

    public void TakeDmg(int dmg)
    {
        if (shieldBroken)
        {
            currentHp -= dmg;
        }
        else
        {
            currentArmour -= dmg;
        }
        StartCoroutine(Hit());
    }

    public void HealHp(int heal)
    {
        currentHp += heal;
    }

    public void HealArmour(int heal)
    {
        currentArmour += heal;
    }

    public void RaiseMaxHp(int increase)
    {
        maxHp += increase;
        currentHp += increase;
    }

    public void RaiseMaxArmour(int increase)
    {
        maxArmour += increase;
        currentArmour += increase;
    }

    public void LowerMaxHp(int decrease)
    {
        maxHp -= decrease;
        currentHp -= decrease;
    }

    public void LowerMaxArmour(int decrease)
    {
        maxArmour -= decrease;
        currentArmour -= decrease;
    }

    #endregion

    void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.1f);
        transform.position = temporaryPosition.position;
    }

    IEnumerator Respawn()
    {
        Die();
        currentHp = maxHp;
        currentArmour = maxArmour;
        yield return new WaitForSeconds(respawnDelay);
        transform.position = respawnPoint.position;
    }

    IEnumerator Hit()
    {
        regenCd = regenerationCd;
        gameObject.layer = 12;
        StartCoroutine(InvulnerabilityFlash());
        yield return new WaitForSeconds(invulnerableDuration);
        gameObject.layer = 6;
    }

    IEnumerator InvulnerabilityFlash()
    {
        sr.color = Color.white;
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i <= numberOfFlashes; i++)
        {
            if (i % 2 == 0)
            {
                sr.color = flashColor1;
            }
            else
            {
                sr.color = flashColor2;
            }
            yield return new WaitForSeconds(invulnerableDuration/numberOfFlashes);
        }
        sr.color = originalColour;
    }

    IEnumerator RegenArmour()
    {
        while (true)
        {
            if (canRegen)
            {
                if (currentArmour < maxArmour)
                {
                    yield return new WaitForSeconds(1);
                    currentArmour++;
                }
                else
                {
                    canRegen = false;
                    regenCd = regenerationCd;
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}