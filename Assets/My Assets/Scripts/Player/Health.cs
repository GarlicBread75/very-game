using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    [SerializeField] int maxHp;
    [SerializeField] int currentHp;
    [SerializeField] GameObject[] deathEffects;
    [SerializeField] Slider hpSlider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Image background;
    [SerializeField] Material deadMaterial;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] float hurtEyesDelay;
    [SerializeField] GameObject[] eyes;
    [SerializeField] float minBlinkTime, maxBlinkTime, minBlinkCooldown, maxBlinkCooldown;
    float blinkCd;

    MeshRenderer rend;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool hit;

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
    }

    void FixedUpdate()
    {
        if (dead)
        {
            foreach (GameObject eye in eyes)
            {
                if (eye.activeInHierarchy)
                {
                    eye.SetActive(false);
                }
            }

            if (!eyes[1].activeInHierarchy)
            {
                eyes[1].SetActive(true);
            }
            return;
        }

        if (blinkCd > 0)
        {
            blinkCd -= Time.fixedDeltaTime;
        }
        else
        {
            blinkCd = Random.Range(minBlinkCooldown, maxBlinkCooldown);
            StartCoroutine(Blink());
        }

        hpSlider.value = currentHp;
        fill.color = gradient.Evaluate(hpSlider.normalizedValue);
        background.color = new Color(fill.color.r / 5, fill.color.g / 5, fill.color.b / 5);



        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        if (currentHp <= 0 && !dead)
        {
            Die();
        }

        if (hit && !dead)
        {
            eyes[0].SetActive(false);
            eyes[2].SetActive(true);
            Invoke("HitEyes", hurtEyesDelay);
            hit = false;
        }
    }

    #region Health Methods

    public void TakeDmg(int dmg)
    {
        currentHp -= dmg;
    }

    public void HealHp(int heal)
    {
        currentHp += heal;
    }

    public void RaiseMaxHp(int increase)
    {
        maxHp += increase;
        currentHp += increase;
    }

    public void LowerMaxHp(int decrease)
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
        dead = true;
    }

    void HitEyes()
    {
        eyes[0].SetActive(true);
        eyes[2].SetActive(false);
    }

    IEnumerator Blink()
    {
        if (eyes[0].activeInHierarchy)
        {
            eyes[0].transform.localScale = new Vector3(1, 0.25f, 1);
            yield return new WaitForSeconds(Random.Range(minBlinkTime, maxBlinkTime));
            eyes[0].transform.localScale = Vector3.one;
        }
    }
}