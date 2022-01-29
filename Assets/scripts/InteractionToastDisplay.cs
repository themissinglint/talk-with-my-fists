using UnityEngine;
using UnityEngine.UI;

public class InteractionToastDisplay : MonoBehaviour {

    public static InteractionToastDisplay Instance;
    
    // Anatomy
    public Image BackgroundImage;

    private void Awake() {
        Instance = this;
    }

    public void PopToast(InteractionToastData data) {
        BackgroundImage.color = data.BackgroundColor;
    }

}
