using UnityEngine;
using UnityEngine.UI;

public class InteractionToastDisplay : MonoBehaviour {

    public static InteractionToastDisplay Instance;
    
    // Anatomy
    public Image BackgroundImage;

    private void Awake() {
        Instance = this;
    }

    public void DoToast(InteractionToastData data) {
        BackgroundImage.color = data.BackgroundColor;
    }

}
