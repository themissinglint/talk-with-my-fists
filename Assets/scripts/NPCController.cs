using UnityEngine;

public class NPCController : MonoBehaviour {

    public InteractionToastData Toast;
    public GameObject FXRoot;

    private bool firstUpdate;

    private void FirstUpdate() {
        firstUpdate = false;
        UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot, 0.1f);
    }

    private void Update() {
        if (firstUpdate) FirstUpdate();
    }
    
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            InteractionToastDisplay.Instance.PopToast(Toast, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot);
        }
    }
}
