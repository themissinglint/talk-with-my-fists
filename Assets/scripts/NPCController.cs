using UnityEngine;

public class NPCController : MonoBehaviour {

    public InteractionToastData Toast;
    public GameObject FXRoot;
    
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            InteractionToastDisplay.Instance.PopToast(Toast, gameObject);
            UIObjectFX.DoEffect(UIObjectFX.EffectType.NPCPerkUpPulse, FXRoot);
        }
    }
}
