using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleUI : MonoBehaviour
{
    [SerializeField] GameObject kong;
    [SerializeField] GameObject bp;

    [SerializeField] KongLetter k;
    [SerializeField] KongLetter o;
    [SerializeField] KongLetter n;
    [SerializeField] KongLetter g;

    [SerializeField] GameObject kImage;
    [SerializeField] GameObject oImage;
    [SerializeField] GameObject nImage;
    [SerializeField] GameObject gImage;

    [SerializeField] Text bananaText;
    [SerializeField] Text bpText;

    [SerializeField] int levelNum;

    void Start()
    {
        bananaText.text = PlayerParty.Bananas.ToString();
        k.ShowUI += Check;
        o.ShowUI += Check;
        n.ShowUI += Check;
        g.ShowUI += Check;
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
        if (show)
        {
            bp.SetActive(false);
            kong.SetActive(false);
            DisplayBanana();
        }
    }

    private void Check(bool c)
    {
        if (c)
        {
            StartCoroutine(DisplayKONG());
            StartCoroutine(DisplayBP());
        }
        else
        {
            StartCoroutine(DisplayKONG());
        }
    }
    public void DisplayBanana()
    {
        bananaText.text = PlayerParty.Bananas.ToString();
    }

    public IEnumerator DisplayKONG()
    {
        if (KongLettersManifest.KongLetters[levelNum].getLetter(Letter.K))
        {
            kImage.SetActive(true);
        }
        if (KongLettersManifest.KongLetters[levelNum].getLetter(Letter.O))
        {
            oImage.SetActive(true);
        }
        if (KongLettersManifest.KongLetters[levelNum].getLetter(Letter.N))
        {
            nImage.SetActive(true);
        }
        if (KongLettersManifest.KongLetters[levelNum].getLetter(Letter.G))
        {
            gImage.SetActive(true);
        }
        kong.SetActive(true);
        yield return new WaitForSeconds(2);
        kong.SetActive(false);
    }

    public IEnumerator DisplayBP()
    {
        bpText.text = "BP: " + PlayerParty.BP + "/" + PlayerParty.MaxBP;
        bp.SetActive(true);
        yield return new WaitForSeconds(2);
        bp.SetActive(false);
    }
}
