using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Crosshair : MonoBehaviour
{
    public GameObject crosshairSway;
    public GameObject crosshairAim;

    //public Vector3 SwayOffset;
    //public Vector3 RecoilOffset;
    [SerializeField] private Sprite crosshairDot;
    public Sprite CrosshairDot { get => crosshairDot; }

    void Awake()
    {
        Cursor.visible = false;
        crosshairSway = new GameObject("Crosshair_Sway");
        crosshairAim = new GameObject("Crosshair_Aim");

        crosshairSway.transform.parent = transform;
        crosshairAim.transform.parent = crosshairSway.transform;

        SpriteRenderer AimSprite = crosshairAim.AddComponent<SpriteRenderer>();
        AimSprite.sprite = crosshairDot;
        AimSprite.sortingLayerName = "UI";

    }
    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
