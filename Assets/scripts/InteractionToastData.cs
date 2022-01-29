using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Interaction Toast")]
public class InteractionToastData : ScriptableObject {

    public Color BackgroundColor;
    public Sprite EntitySprite;

    public Vector2 EntityScaleVector = Vector2.one;
    public Vector2 EntityOffsetVector = Vector2.zero;

    [Header("Expiration")]
    public bool ExpiresFromTime;
    [ShowIf("ExpiresFromTime")] public float ExpirationTime;
    public bool ExpiresFromDistance;
    [ShowIf("ExpiresFromDistance")] public float ExpirationDistance;

    [Header("Dialogue")]
    public bool HasDialogue;
    [ShowIf("HasDialogue")] [TextArea(3, 6)] public string DialogueText;
    [ShowIf("HasDialogue")] public Color DialogueColor;
    
    public enum ToastScale {
        none, small, large
    }
    public ToastScale Scale = ToastScale.small;

}
