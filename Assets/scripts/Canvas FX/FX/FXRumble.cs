using UnityEngine;

// TODO: Currently this only works for Fire And Forget rumbles. Ultimately, this class will probably have
// TODO: child classes, one of which is Fire and Forget, and the other of which is toggle.
public class FXRumble : FX {

    public float Strength;
    public float Duration;
    
    public override void Trigger(FXArgs args) {
        Vector2 pos = transform.position;
        if (args.OriginPointOverride != Vector2.zero) {
            pos = args.OriginPointOverride;
        }
        // TODO: Rumble. Import the RumbleManager.
        // RumbleManager.Instance.AddFireAndForgetRumble(pos, Strength * args.Amplitude, Duration);
    }
    
}
