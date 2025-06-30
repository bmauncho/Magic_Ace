using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Card Movement")]
    [SerializeField] private float duration;
    [SerializeField] private bool canMove;
    [SerializeField] private Transform target;
    private float elapsedTime;
    private Vector3 startPosition;
    public Action OnComplete;
    Slot Slot;
    bool isInitializarion = false;

    [Header("card Info")]
    [SerializeField] private CardType cardType;
    [SerializeField] private Image CardBg;
    [SerializeField] private Image card;
    [SerializeField] private Image Outline;
    [SerializeField] private Image CardRear;
    [SerializeField] private bool normalCard;
    [SerializeField] private bool goldenCard;
    [SerializeField] private bool superJockerCard;
    [SerializeField] private bool bigJockerCard;
    [SerializeField] private bool smallJockerCard;
    [SerializeField] private bool wildCard;
    [SerializeField] private RectTransform cardImgHolder;

    [Header("Card Animations")]
    [SerializeField] private Animator WildAnim_;
    [SerializeField] private GameObject wildAnimEffects;
    [SerializeField] private GameObject goldenCardEffect;
    [SerializeField] private Animator wildBounceAnim;
    [SerializeField] private GameObject SuperJokerFlip;
    [SerializeField] private GameObject BigJokerFlip;


    [Header("Card Effects")]
    [SerializeField] private GameObject winCardGlow;


    public Action OnWildAnimComplete;

    private void OnEnable ()
    {
        transform.localScale = Vector3.one;
    }
    #region
    void FixedUpdate ()
    {
        if (canMove && target)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            transform.position = Vector3.Lerp(startPosition , target.position , t);

            if (elapsedTime >= duration)
            {
                canMove = false; // Stop movement
                transform.position = target.position; // Ensure it reaches the target
                OnComplete?.Invoke();
                SetAsOwner();
                ShowGoldenEffect();
                if (CommandCenter.Instance.gridManager_.IsGridRefilling())
                {
                    CommandCenter.Instance.soundManager_.PlaySound("Base_NewIcon_Land");
                }
            }
        }
    }

    public void moveCard ( Transform _target , Slot _Slot , bool isInitialization_ = false )
    {
        SetDuration();
        target = _target;
        startPosition = transform.position;
        elapsedTime = 0f;
        canMove = true;
        Slot = _Slot;
        isInitializarion = isInitialization_;
    }

    public void SetAsOwner ()
    {
        if (!isInitializarion)
        {
            Slot.AddOwner(this.gameObject);
        }
    }

    public void SetDuration ()
    {
        if (CommandCenter.Instance)
        {
            if (CommandCenter.Instance.spinManager_.NormalSpin())
            {
                duration = 0.15f;
            }
            else if (CommandCenter.Instance.spinManager_.QuickSpin())
            {
                duration = 0.15f;
            }
            else if (CommandCenter.Instance.spinManager_.TurboSpin())
            {
                duration = 0.05f;
            }
        }
    }
    #endregion

    #region
    public void ShowNormalCard ( Sprite _cardBg , Sprite _card )
    {
        normalCard = true;
        goldenCard = false;
        superJockerCard = false;
        bigJockerCard = false;
        smallJockerCard = false;
        wildCard = false;
        Outline.gameObject.SetActive(false);
        CardBg.gameObject.SetActive(true);
        card.gameObject.SetActive(true);
        CardBg.rectTransform.localScale = new Vector3(1f , 1f , 1f);
        card.rectTransform.localScale = new Vector3(1f , 1f , 1f);
        cardImgHolder.localRotation = Quaternion.identity;
        CardRear.gameObject.SetActive(false);
        wildBounceAnim.Rebind();
        DisableWildBounceAnim();
        wildBounceAnim.transform.rotation = Quaternion.identity;
        CardBg.sprite = _cardBg;
        card.sprite = _card;
    }

    public void showGoldenCard ( Sprite _cardBg , Sprite _card )
    {
        normalCard = false;
        goldenCard = true;
        superJockerCard = false;
        bigJockerCard = false;
        smallJockerCard = false;
        wildCard = false;
        Outline.gameObject.SetActive(false);
        CardBg.gameObject.SetActive(true);
        card.gameObject.SetActive(true);
        CardBg.rectTransform.localScale = new Vector3(1f , 1f , 1f);
        card.rectTransform.localScale = new Vector3(1f , 1f , 1f);
        cardImgHolder.localRotation = Quaternion.identity;
        CardRear.gameObject.SetActive(false);
        wildBounceAnim.Rebind();
        DisableWildBounceAnim();
        wildBounceAnim.transform.rotation = Quaternion.identity;
        CardBg.sprite = _cardBg;
        card.sprite = _card;
    }

    public void showSuperJocKer ( Sprite _cardBg , Sprite _card )
    {
        normalCard = false;
        goldenCard = false;
        superJockerCard = true;
        bigJockerCard = false;
        smallJockerCard = false;
        wildCard = false;
        Outline.gameObject.SetActive(false);
        CardBg.gameObject.SetActive(true);
        card.gameObject.SetActive(false);
        CardBg.rectTransform.localScale = new Vector3(1.1f , 1.1f , 1.1f);
        card.rectTransform.localScale = new Vector3(1.5f , 1.5f , 1.5f);
        cardImgHolder.localRotation = Quaternion.identity;
        wildBounceAnim.Rebind();
        DisableWildBounceAnim();
        CardBg.sprite = _cardBg;
        card.sprite = _card;
    }

    public void showBigJocKer ( Sprite _cardBg , Sprite _card , Sprite _outline )
    {
        normalCard = false;
        goldenCard = false;
        superJockerCard = false;
        bigJockerCard = true;
        smallJockerCard = false;
        wildCard = false;
        Outline.gameObject.SetActive(true);
        CardBg.gameObject.SetActive(true);
        card.gameObject.SetActive(true);
        CardRear.gameObject.SetActive(false);
        CardBg.rectTransform.localScale = new Vector3(1, 1, 1);
        card.rectTransform.localScale = new Vector3(1.5f , 1.5f , 1.5f);
        cardImgHolder.localRotation = Quaternion.identity;
        wildBounceAnim.Rebind();
        DisableWildBounceAnim();
        CardBg.sprite = _cardBg;
        card.sprite = _card;
        Outline.sprite = _outline;
    }

    public void showSmallJocKer ( Sprite _cardBg , Sprite _card , Sprite _outline )
    {
        normalCard = false;
        goldenCard = false;
        superJockerCard = false;
        bigJockerCard = false;
        smallJockerCard = true;
        wildCard = false;
        Outline.gameObject.SetActive(true);
        CardBg.gameObject.SetActive(true);
        card.gameObject.SetActive(true);
        CardRear.gameObject.SetActive(false);
        CardBg.rectTransform.localScale = new Vector3(1 , 1 , 1);
        card.rectTransform.localScale = new Vector3(1.5f , 1.5f , 1.5f);
        cardImgHolder.localRotation = Quaternion.identity;
        wildBounceAnim.Rebind();
        DisableWildBounceAnim();
        CardBg.sprite = _cardBg;
        card.sprite = _card;
        Outline.sprite = _outline;
    }

    public void showWild ( Sprite _card )
    {
        normalCard = false;
        goldenCard = false;
        superJockerCard = false;
        bigJockerCard = false;
        smallJockerCard = false;
        wildCard = true;
        Outline.gameObject.SetActive(false);
        CardBg.gameObject.SetActive(false);
        card.gameObject.SetActive(true);
        CardRear.gameObject.SetActive(false);
        card.rectTransform.localScale = new Vector3(1.5f , 1.5f , 1.5f);
        cardImgHolder.localRotation = Quaternion.identity;
        wildBounceAnim.transform.rotation = Quaternion.identity;
        wildBounceAnim.Rebind();
        EnableWildBounceAnim();
        card.sprite = _card;
    }

    public void SetCardType ( CardType Type = CardType.ACE )
    {
        cardType = Type;
    }

    public CardType GetCardType ()
    {
        return cardType;
    }

    public bool isNormalCard () { return normalCard; }
    public bool isGoldenCard () { return goldenCard; }
    public bool isSuperJockerCard () { return superJockerCard; }
    public bool isBigJockerCard () { return bigJockerCard; }
    public bool isSmallJockerCard () { return smallJockerCard; }
    public bool isWildCard () { return wildCard; }

    #endregion

    public void ShowCardBg ()
    {
        CardBg.gameObject.SetActive(false);
        card.gameObject.SetActive(false);
        CardRear.gameObject.SetActive(true);
    }

    public void HideCardBg ()
    {
        if (cardType == CardType.SCATTER)
        {
            CardBg.gameObject.SetActive(false);
        }
        else
        {
            CardBg.gameObject.SetActive(true);
        }
        card.gameObject.SetActive(true);

        CardRear.gameObject.SetActive(false);
    }

    public RectTransform GetCardImgHolder ()
    {
        return cardImgHolder;
    }

    public RectTransform GetCard ()
    {
        return card.rectTransform;
    }

    public void ShowCardWinGlow ()
    {
        winCardGlow.gameObject.SetActive(true);
    }

    public void HideCardWinGlow ()
    {
        winCardGlow.gameObject.SetActive(false);
    }

    public void showWildAnim ()
    {
        StartCoroutine(wildAnim());
    }

    private IEnumerator wildAnim ()
    {
        DisableWildBounceAnim();
        WildAnim_.gameObject.SetActive(true);
        int loopCount = 3;
        card.gameObject.SetActive(false);
        for (int i = 0 ; i < loopCount ; i++)
        {
            WildAnim_.Rebind();
            WildAnim_.Play("WildCardAnim");

            // Wait for animation to start
            yield return new WaitUntil(() => WildAnim_.GetCurrentAnimatorStateInfo(0).IsName("WildCardAnim"));

            // Wait for animation to finish
            yield return new WaitWhile(() =>
                WildAnim_.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

            //Debug.Log($"Wild Animation {i + 1} Done");

            // Optional delay
            yield return new WaitForSeconds(0.1f);
        }
        WildAnim_.gameObject.SetActive(false);
        card.gameObject.SetActive(true);
        OnWildAnimComplete?.Invoke();
        DisableWildBounceAnim();
    }

    public void EnableWildBounceAnim ()
    {
        wildBounceAnim.Rebind();
        wildBounceAnim.Play("WildIdleAnim");
        wildBounceAnim.enabled = true;
    }

    public void DisableWildBounceAnim ()
    {
        wildBounceAnim.Rebind();
        wildBounceAnim.Play("scatterIdle");
        wildBounceAnim.enabled = false;
        wildBounceAnim.transform.localScale = Vector3.one;
    }


    public GameObject wildEffects ()
    {
        return wildAnimEffects;
    }

    public void ShowGoldenEffect ()
    {
        if (!goldenCard) { return; }
        goldenCardEffect.gameObject.SetActive(true);
        Invoke(nameof(HideGoldenEffect) , 1f);
    }

    public void HideGoldenEffect ()
    {
        goldenCardEffect.gameObject.SetActive(false);
    }

    public void FlipSuperJoker ()
    {
        SuperJokerFlip.SetActive(true);
        Animator anim = SuperJokerFlip.GetComponentInChildren<Animator>();
        anim.Rebind();
        CommandCenter.Instance.soundManager_.PlaySound("Base_W3_Show");
        StartCoroutine(DeactivateSuperJokerFlipAnim(anim));
    }

    IEnumerator DeactivateSuperJokerFlipAnim (Animator Anim)
    {
        Anim.Play("SuperJokerTurnEffect");
        yield return new WaitUntil(() => Anim.GetCurrentAnimatorStateInfo(0).IsName("SuperJokerTurnEffect"));

        // Wait for animation to finish
        yield return new WaitWhile(() =>
            Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        //Debug.Log($"Wild Animation {i + 1} Done");

        // Optional delay
        yield return new WaitForSeconds(0.1f);
        SuperJokerFlip.SetActive(false);
        yield return null;
    }

    public void flipBigJoker ()
    {
        BigJokerFlip.SetActive(true);
        Animator anim = BigJokerFlip.GetComponentInChildren<Animator>();
        anim.Rebind();
        CommandCenter.Instance.soundManager_.PlaySound("Base_W2_Show");
        StartCoroutine(DeactivateBigJokerFlipAnim(anim));
    }


    IEnumerator DeactivateBigJokerFlipAnim ( Animator Anim )
    {
        Anim.Play("BigJokerTurnAnim");
        yield return new WaitUntil(() => Anim.GetCurrentAnimatorStateInfo(0).IsName("BigJokerTurnAnim"));

        // Wait for animation to finish
        yield return new WaitWhile(() =>
            Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

        //Debug.Log($"Wild Animation {i + 1} Done");

        // Optional delay
        yield return new WaitForSeconds(0.1f);
        BigJokerFlip.SetActive(false);
        yield return null;
    }

    public void flipSmallJoker ()
    {
        BigJokerFlip.SetActive(true);
        Animator anim = BigJokerFlip.GetComponentInChildren<Animator>();
        anim.Rebind();
        CommandCenter.Instance.soundManager_.PlaySound("Base_W1_Show");
        StartCoroutine(DeactivateBigJokerFlipAnim(anim));
    }
}