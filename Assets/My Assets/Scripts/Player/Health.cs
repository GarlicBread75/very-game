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
    [SerializeField] float hitMatDelay;
    Color colour;
    [SerializeField] UnityEvent onDeath;
    MeshRenderer rend;
    bool hit;
    public enum PlayerState { alive, dead, victory };
    [HideInInspector] public PlayerState playerState = PlayerState.alive;

    [Space]

    [Header("Health Slider")]
    [SerializeField] Slider hpSlider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill, background;

    [Space]

    [Header("Eyes")]
    [SerializeField] GameObject[] eyes;
    [SerializeField] float hitBlinkCooldown ,minBlinkTime, maxBlinkTime, minBlinkCooldown, maxBlinkCooldown;
    [SerializeField] string otherPlayerName;
    [SerializeField] bool menu;
    Health otherPlayer;
    float blinkCd;
    float hitBlinkCd;
    bool blinking, hitBlinking;

    [Space]

    [Header("Sounds")]
    [SerializeField] AudioSource deathSound;
    [SerializeField] float minVolume, maxVolume, minPitch, maxPitch;
    #endregion

    void Awake()
    {
        blinkCd = Random.Range(minBlinkCooldown, maxBlinkCooldown);

        if (menu)
        {
            return;
        }

        rend = GetComponent<MeshRenderer>();
        colour = rend.material.GetColor("_BaseColor");
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;

        otherPlayer = GameObject.Find(otherPlayerName).GetComponent<Health>();
        hitBlinkCd = hitBlinkCooldown;
    }

    void FixedUpdate()
    {
        if (menu)
        {
            if (blinkCd > 0)
            {
                blinkCd -= Time.fixedDeltaTime;
            }
            else
            {
                blinkCd = Random.Range(minBlinkCooldown, maxBlinkCooldown);
                StartCoroutine(Blink());
            }
            return;
        }

        if (currentHp <= 0 && playerState != PlayerState.dead)
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

        if (playerState == PlayerState.dead)
        {
            if (rend.material != deadMaterial)
            {
                rend.material = deadMaterial;
            }

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
        if (otherPlayer.playerState == PlayerState.dead)
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

        if (hit)
        {
            hit = false;
            StartCoroutine(Hit());
            hitBlinkCd = hitBlinkCooldown;
        }

        if (transform.position.x > 50 || transform.position.x < -50 || transform.position.y > 200 || transform.position.y < -15)
        {
            currentHp = 0;
        }
    }

    #region Health Methods

    public void TakeDmg(float dmg)
    {
        currentHp -= dmg;
        hit = true;
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
        hit = true;
    }

    #endregion

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Void") && !menu)
        {
            currentHp = 0;
        }
        else
        if ((collision.gameObject.CompareTag("Block Support") || collision.gameObject.CompareTag("Ground")) && collision.transform.position.y < -3)
        {
            currentHp = 0;
        }
    }

    void Die()
    {
        PlaySound(deathSound);
        GameObject effect;
        if (transform.position.y > -2)
        {
            effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }
        rend.material = deadMaterial;
        onDeath.Invoke();
        playerState = PlayerState.dead;
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

    IEnumerator Hit()
    {
        rend.material.SetColor("_BaseColor", Color.white);
        rend.material.SetFloat("_ShadowStep", 0);
        yield return new WaitForSeconds(hitBlinkCooldown / 2);
        rend.material.SetColor("_BaseColor", colour);
        rend.material.SetFloat("_ShadowStep", 0.7f);
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