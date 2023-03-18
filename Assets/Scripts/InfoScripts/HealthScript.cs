using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public bool isTarget;
    public GameObject targetIcon; 

    public int maxHealth; 
    public int currentHealth;

    public int defense;

    public UnityEvent playOnDeath;

    [SerializeField] GameObject HealthBar; 
    [SerializeField] Image healthbarFill; 

    public void TakeDamage(SelectScript _attacker)
    {
        //Camera Shake
        Camera.main.GetComponent<CameraFollow>().StartShake(0.1f, 0.1f);

        //Play Sounds
        TileAudioManager.instance.PlayTileAudio(tileAudioType.shoot);
        TileAudioManager.instance.PlayTileAudio(tileAudioType.damage);

        int _damage = _attacker.GetComponent<UnitAttackScript>().GetDamage(this);

        _damage -= defense;
        _damage = _damage < 1 ? 1 : _damage;

        currentHealth -= _damage; 

        if(currentHealth < 0)
        {
            playOnDeath.Invoke();
        }
        //UpdateHealth(); 
    }
    private void Update()
    {
        UpdateHealth(); 
    }
    public void UpdateHealth()
    {
        if (currentHealth < maxHealth)
        {
            HealthBar.SetActive(true);
            float _targetHealth = (float)currentHealth / (float)maxHealth;
            healthbarFill.fillAmount = Mathf.Lerp(healthbarFill.fillAmount, _targetHealth, Time.deltaTime * 8);
        }
    }
}
