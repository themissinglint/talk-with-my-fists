using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class FakeCharacterAnimations : MonoBehaviour {

    public CorgiController Controller;
    
    private Transform myTransform;
    public Transform SpriteTransform;
    
    public FXController StepRightFX;
    public FXController StepLeftFX;

    public float DistancePerStep;
    private float distanceTravelled;

    private Vector3 lastPosition;
    private bool lastStepWasLeft;

    private bool facingLeft;
    
    public FXController JumpFX;

    private void Start() {
        myTransform = transform;
        lastPosition = myTransform.position;
    }
    
    private void Update() {
        
        if (Controller.State.IsGrounded) {
            distanceTravelled += Vector3.Distance(myTransform.position, lastPosition);
            if (distanceTravelled > DistancePerStep) {
                distanceTravelled = 0f;
            
                if (lastStepWasLeft) {
                    StepRightFX.TriggerWithArgs(new FXArgs {InputVector = Vector2.up});
                }
                else {
                    StepLeftFX.TriggerWithArgs(new FXArgs {InputVector = Vector2.up});
                }
                lastStepWasLeft = !lastStepWasLeft;
            }
        }
        else {
            distanceTravelled = 1f;
        }

        bool travelledLeft = (myTransform.position - lastPosition).z < -0.025f;
        bool travelledRight = (myTransform.position - lastPosition).z > 0.025f;
        Vector3 spriteScale = SpriteTransform.localScale;
        if (facingLeft && travelledRight) {
            SpriteTransform.localScale = new Vector3(-spriteScale.x, spriteScale.y, spriteScale.z);
            facingLeft = false;
        } else if (!facingLeft && travelledLeft) {
            SpriteTransform.localScale = new Vector3(-spriteScale.x, spriteScale.y, spriteScale.z);
            facingLeft = true;
        }
        

        lastPosition = myTransform.position;
    }

    public void DoJumpFX() {
        JumpFX.TriggerWithArgs(new FXArgs {InputVector = Vector2.up});
    }

}
