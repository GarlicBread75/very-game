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
    [SerializeField] GameObject deathEffect;
    [SerializeField] Material deadMaterial;
    [SerializeField] UnityEvent onDeath;
    MeshRenderer rend;
    float blinkCd;
    float hitBlinkCd;
    bool blinking, hitBlinking;
    [HideInInspector] public bool dead;
    [HideInInspector] public bool victory;
    [HideInInspector] public bool hit;

    [Space]

    [Header("Health Slider")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] Image background;

    [Space]

    [Header("Eyes")]
    [SerializeField] GameObject[] eyes;
    [SerializeField] float hitBlinkDelay;
    [SerializeField] float minBlinkTime, maxBlinkTime, minBlinkCooldown, maxBlinkCooldown;
    [SerializeField] string otherPlayerName;
    Health otherPlayer;

    [Space]

    [Header("Sounds")]
    [SerializeField] AudioSource deathSound;
    [SerializeField] float minVolume, maxVolume, minPitch, maxPitch;
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
        otherPlayer = GameObject.Find(otherPlayerName).GetComponent<Health>();
        blinkCd = Random.Range(minBlinkCooldown, maxBlinkCooldown);
        hitBlinkCd = hitBlinkDelay;
    }

    void FixedUpdate()
    {
        if (currentHp <= 0 && !dead)
        {
            Die();
        }

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        hpSlider.value = Mathf.Lerp(hpSlider.value, currentHp, 20 * Time.deltaTime);
        fill.color = gradient.Evaluate(hpSlider.normalizedValue);
        background.color = new Color(fill.color.r / 5, fill.color.g / 5, fill.color.b / 5);

        if (otherPlayer == null)
        {
            otherPlayer = GameObject.Find(otherPlayerName).GetComponent<Health>();
        }

        if (dead)
        {
            foreach (GameObject eye in eyes)
            {
                if (eye == eyes[1] && !eyes[1].activeInHierarchy)
                {
                    eyes[1].SetActive(true);
                }
                else
                if (eye != eyes[1] && eye.activeInHierarchy)
                {
                    eye.SetActive(false);
                }
            }
            return;
        }
        else
        if (otherPlayer.dead)
        {
            foreach (GameObject eye in eyes)
            {
                if (eye == eyes[3] && !eyes[3].activeInHierarchy)
                {
                    eyes[3].SetActive(true);
                }
                else
                if (eye != eyes[3] && eye.activeInHierarchy)
                {
                    eye.SetActive(false);
                }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Void"))
        {
            currentHp = 0;
        }
    }

    void Die()
    {
        PlaySound(deathSound);
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        rend.material = deadMaterial;
        onDeath.Invoke();
        dead = true;
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

    public void ToggleAngry(bool thingy)
    {
        eyes[4].SetActive(thingy);
    }

    void PlaySound(AudioSource source)
    {
        source.volume = Random.Range(minVolume, maxVolume);
        source.pitch = Random.Range(minPitch, maxPitch);
        if (!source.isPlaying)
        {
            source.Play();
        }
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