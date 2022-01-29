using UnityEngine;

public class InteractionToastTestButton : MonoBehaviour {

    public InteractionToastData TestToastData;
    public GameObject SourceObject;

    public void ReceiveClick() {
        InteractionToastDisplay.Instance.PopToast(TestToastData, SourceObject);
    }

}
