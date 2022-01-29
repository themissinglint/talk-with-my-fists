using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionToastTestButton : MonoBehaviour {

    public InteractionToastData TestToastData;

    public void ReceiveClick() {
        InteractionToastDisplay.Instance.PopToast(TestToastData);
    }

}
