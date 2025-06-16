using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGoldenCards : MonoBehaviour
{
    public WinSequence winSequence_;
    public IEnumerator rotateGoldenCards (List<(GameObject card, List<(int col, int row)> Positions)> remainingGoldenCards)
    {
        winSequence_.SetIsFlipping(true);
        Debug.Log("Rotate");
        Sequence seq = DOTween.Sequence();

        for (int i = 0 ; i < remainingGoldenCards.Count ; i++)
        {
            GameObject cardObject = remainingGoldenCards [i].card;
            Card cardComponent = cardObject.GetComponent<Card>();

            Sequence cardSeq = DOTween.Sequence(); // create a small sequence for each card

            cardSeq.Append(cardObject.transform.DORotate(new Vector3(0 , 90 , 0) , 0.25f))
                   .AppendCallback(() => cardComponent.ShowCardBg())
                   .Append(cardObject.transform.DORotate(new Vector3(0 , 180 , 0) , 0.25f));

            seq.Join(cardSeq); // join the mini sequence into the main one
        }

        yield return seq.WaitForCompletion();

        yield return null;
    }
}
