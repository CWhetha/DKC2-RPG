using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isAlly;
    [SerializeField] public int time;
    [SerializeField] public GameObject target;
    [SerializeField] public GameObject arrow;
    [SerializeField] public BattleHUD hud;
    [SerializeField] public DamageContainer damage;
    [SerializeField] public GameObject attackedImage;
    [SerializeField] public Sprite attackedSprite;

    public int Time { get; set; }
    public Character Character { get; set; }
    SpriteRenderer image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
        originalPos = gameObject.transform.localPosition;
    }

    public void Setup(Character character)
    {
        Character = character;
        image.sprite = Character.Base.CharacterSprite;
        image.DOFade(1f, 0.1f);
        PlayEnterAnimation();
    }

    public void Show(bool s)
    {
        gameObject.SetActive(s);
    }

    public void PlayEnterAnimation()
    {
        if (isAlly)
        {
            gameObject.transform.localPosition = new Vector3(-500f, originalPos.y, originalPos.z);
            gameObject.transform.DOLocalMoveX(originalPos.x, Random.Range(0.85f, 1f));
        }
        else
        {
            gameObject.transform.localPosition = originalPos;

        }
    }

    public void FleeAnimation()
    {
        Vector3 fleePos = new Vector3(-500f, originalPos.y, 0);
        gameObject.transform.DOLocalMoveX(fleePos.x, Random.Range(0.85f, 1f));
    }

    public IEnumerator AttackedAnimation(Sprite sprite)
    {
        SpriteRenderer attackEffect = attackedImage.GetComponent<SpriteRenderer>();
        attackEffect.sprite = sprite;
        attackedImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackedImage.SetActive(false);
    }

    public IEnumerator AttackedPhysicalAnimation()
    {
        SpriteRenderer attackEffect = attackedImage.GetComponent<SpriteRenderer>();
        attackEffect.sprite = attackedSprite;
        attackedImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackedImage.SetActive(false);
    }


    public void FaintAnimation()
    {
        if (IsAlly == false)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(gameObject.transform.DOLocalMoveY(originalPos.y - 100f, 0.5f));
            sequence.Join(image.DOFade(0f, 0.5f));

        }
        else if (isAlly)
        {
            image.sprite = Character.Base.FaintSprite;
        }
    }

    public void ReviveAnimation()
    {
        image.sprite = Character.Base.CharacterSprite;
    }

    public GameObject Target
    {
        get { return target; }
    }

    public GameObject Arrow
    {
        get { return arrow; }
    }

    public bool IsAlly
    {
        get { return isAlly; }
    }

    public BattleHUD HUD
    {
        get { return hud; }
    }

    public DamageContainer Damage
    {
        get { return damage; }
    }
}
