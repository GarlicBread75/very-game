using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [SerializeField] float maxHp;
    [SerializeField] float currentHp;
    [SerializeField] GameObject[] deathEffects;
    [SerializeField] Slider hpSlider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Image background;
    [SerializeField] Material deadMaterial;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] float hitBlinkDelay;
    [SerializeField] GameObject[] eyes;
    [SerializeField] float minBlinkTime, maxBlinkTime, minBlinkCooldown, maxBlinkCooldown;
    float blinkCd;
    float hitBlinkCd;
    bool blinking, hitBlinking;

    [HideInInspector] public bool dead;
    [HideInInspector] public bool victory;
    [HideInInspector] public bool hit;
    MeshRenderer rend;

    [Space]

    [Header("Sounds")]
    [SerializeField] UnityEvent[] sounds;
    #endregion

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        currentHp = maxHp;

        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    void Awake()
    {
        blinkCd = Random.Range(minBlinkCooldown, maxBlinkCooldown);
        hitBlinkCd = hitBlinkDelay;
    }

    void FixedUpdate()
    {

        hpSlider.value = currentHp;
        fill.color = gradient.Evaluate(hpSlider.normalizedValue);
        background.color = new Color(fill.color.r / 5, fill.color.g / 5, fill.color.b / 5);

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        if (dead || victory)
        {
            return;
        }

        if (blinkCd > 0)
        {
            blinkCd -= Time.fixedDeltaTime;
        }
        else
        {
            blinkCd = Random.Range(minBlinkCooldown, maxBlinkCooldown);
            if (!hitBlinking)
            {
                StartCoroutine(Blink());
            }
        }

        if (hitBlinkCd > 0)
        {
            hitBlinkCd -= Time.fixedDeltaTime;
            HitBlinkOn();
        }
        else
        {
            if (!blinking)
            {
                HitBlinkOff();
            }
        }

        if (currentHp <= 0 && !dead)
        {
            Die();
        }

        if (hit && !dead)
        {
            hit = false;
            hitBlinkCd = hitBlinkDelay;
        }
    }

    #region Health Methods

    public void TakeDmg(float dmg)
    {
        currentHp -= dmg;
    }

    public void Heal(float heal)
    {
        currentHp += heal;
    }

    public void RaiseMaxHp(float increase)
    {
        maxHp += increase;
        currentHp += increase;
    }

    public void LowerMaxHp(float decrease)
    {
        maxHp -= decrease;
        currentHp -= decrease;
    }

    #endregion

    void Die()
    {
        GameObject effect = Instantiate(deathEffects[Random.Range(0, deathEffects.Length)], transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        rend.material = deadMaterial;
        onDeath.Invoke();
        Invoke("DeadEyes", 0.1f);
        Invoke("DeadEyes", 0.15f);
        Invoke("DeadEyes", 0.2f);
        dead = true;
    }

    void DeadEyes()
    {
        eyes[0].SetActive(false);
        eyes[1].SetActive(true);
        eyes[2].SetActive(false);
        eyes[3].SetActive(false);
    }

    void HitBlinkOn()
    {
        hitBlinking = true;
        if (eyes[0].activeInHierarchy)
        {
            eyes[0].SetActive(false);
        }

        if (!eyes[2].activeInHierarchy)
        {
            eyes[2].SetActive(true);
        }
    }

    void HitBlinkOff()
    {
        if (!eyes[0].activeInHierarchy)
        {
            eyes[0].SetActive(true);
        }

        if (eyes[2].activeInHierarchy)
        {
            eyes[2].SetActive(false);
        }
        hitBlinking = false;
    }

    public void Victory()
    {
        victory = true;
    }

    IEnumerator Blink()
    {
        if (eyes[0].activeInHierarchy)
        {
            blinking = true;
            eyes[0].transform.localScale = new Vector3(1, 0.25f, 1);
            yield return new WaitForSeconds(Random.Range(minBlinkTime, maxBlinkTime));
            eyes[0].transform.localScale = Vector3.one;
            blinking = false;
        }
    }
}