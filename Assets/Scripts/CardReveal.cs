using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardReveal : MonoBehaviour
{

    public float degreesPerSecond = -180;

    [HideInInspector]
    public Texture2D target = null;

    private enum CardRevealState
    {
        Inactive,
        IsRevealing,
        IsDone
    }

    private CardRevealState state;
    private bool hasSwitched;
    private RawImage img;
    private RectTransform rectTrans;

    private void Start()
    {
        hasSwitched = false;
        img = GetComponent<RawImage>();
        rectTrans = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(state.Equals(CardRevealState.IsRevealing))
        {
            rectTrans.Rotate(0, degreesPerSecond * Time.deltaTime, 0);
            if(rectTrans.eulerAngles.y <= 90.5 && !hasSwitched)
            {
                img.texture = target;
                hasSwitched = true;
            }
            print(rectTrans.eulerAngles);
            if(hasSwitched && (rectTrans.eulerAngles.y <= 0.5 || rectTrans.eulerAngles.y >= 180))
            {
                rectTrans.eulerAngles = new Vector3(0, 0, 0);
                state = CardRevealState.IsDone;
            }
        }
    }


    public void Activate()
    {
        if (state.Equals(CardRevealState.Inactive) && target != null)
        {
            state = CardRevealState.IsRevealing;
        }
    }

    public bool IsDone()
    {
        return state.Equals(CardRevealState.IsDone);
    }
}
