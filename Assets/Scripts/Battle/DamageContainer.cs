using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageContainer : MonoBehaviour
{
    [SerializeField] public TextMesh damageText;
    [SerializeField] public TextMesh healText;

    public IEnumerator setDamageDetails(int Damage, float isCrit)
    {
        if (isCrit == 2f)
        {
            damageText.text = Damage.ToString() + " Critical!";

        }
        else
        {
            damageText.text = Damage.ToString();
        }
        damageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        damageText.gameObject.SetActive(false);
    }

    public IEnumerator setMiss()
    {
        damageText.text = "Miss";
        damageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        damageText.gameObject.SetActive(false);
    }

    public IEnumerator setHealDetails(int heal)
    {
        healText.text = heal.ToString();
        healText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        healText.gameObject.SetActive(false);
    }
}

