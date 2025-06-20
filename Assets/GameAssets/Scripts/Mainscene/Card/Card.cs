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
    //[SerializeField] private SuperJokerCard superJokerCard_;


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
        CardRear.gameObject.SetActive(false);
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
        CardRear.gameObject.SetActive(false);
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
}