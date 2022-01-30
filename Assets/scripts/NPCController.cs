using UnityEngine;

public class NPCController : MonoBehaviour {

    public GameObject FXRoot;

    private bool _firstUpdate;

    public UIObjectFX.EffectType IdleEffect;
    public float IdleBounceTime;
    public float IdleBounceAmp;
    public Vector2 IdleBounceDir;
    public float IdleBounceSpeed;
    private float _idleBounceTimer;
    
    public float PerkUpAmp;
    public Vector2 PerkUpDir;
    public float PerkUpSpeed;

    private void FirstUpdate() {
        _firstUpdate = false;
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, 0.1f);
    }

    private void Update() {
        if (_firstUpdate) FirstUpdate();
        _idleBounceTimer += Time.deltaTime;
        if (_idleBounceTimer > IdleBounceTime) {
            _idleBounceTimer = 0f;
            UIObjectFX.DoEffect(IdleEffect, FXRoot, new FXArgs() {Amplitude = IdleBounceAmp, InputVector = IdleBounceDir, Speed = IdleBounceSpeed});
        }
    }
    
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            InteractWithPlayer();
        }
    }

    protected virtual void InteractWithPlayer() {
        
    }
    
}
