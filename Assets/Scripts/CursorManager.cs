using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour {

    public enum CursorStates
    {
        c_default,
        c_collect,
        c_oil,
        c_oil_depleted
    };


    public Sprite defaultCursorImage, collectCursorImage, oilCursorImage, oilDepetedCursorImage;
    public CursorStates currentState;

    private Image playerCrosshair;

    public static CursorManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        playerCrosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
        ChangeCursorState(CursorStates.c_default, true);
    }

    public void SetCursor(bool enabled)
    {
        playerCrosshair.enabled = enabled;
    }

    public void ChangeCursorState(CursorStates newState, bool overrideCheck = false)
    {
        if(currentState != newState || overrideCheck)
        {
            currentState = newState;
            switch (currentState)
            {
                case CursorStates.c_default:
                    //Cursor.SetCursor(defaultCursorImage, Vector2.zero, CursorMode.ForceSoftware);
                    playerCrosshair.sprite = defaultCursorImage;
                    break;
                case CursorStates.c_collect:
                    //Cursor.SetCursor(collectCursorImage, Vector2.zero, CursorMode.ForceSoftware);
                    playerCrosshair.sprite = collectCursorImage;
                    break;
                case CursorStates.c_oil:
                    //Cursor.SetCursor(oilCursorImage, Vector2.zero, CursorMode.ForceSoftware);
                    playerCrosshair.sprite = oilCursorImage;
                    break;
                case CursorStates.c_oil_depleted:
                    playerCrosshair.sprite = oilDepetedCursorImage;
                    break;
            }
        }
    }
}
