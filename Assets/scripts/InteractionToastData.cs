using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Interaction Toast")]
public class InteractionToastData : ScriptableObject {

    public float Time = 1f;
    public Color BackgroundColor;
    public Sprite EntitySprite;

    public Vector2 EntityScaleVector = Vector2.one;
    public Vector2 EntityOffsetVector = Vector2.zero;
    
    public enum ToastScale {
        none, small, large
    }
    public ToastScale Scale = ToastScale.small;

}
