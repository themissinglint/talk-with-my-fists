using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour {

    public float TotalTime;
    public float Progress => Mathf.Clamp01(Time.time / TotalTime);
    public float PctLeft => 1f - Progress;

    public RectTransform BarOutlineRT;
    public RectTransform BarRT;
    public Image BarOutlineImage;
    public Image BarImage;

    public Gradient BarColorByPctLeft;
    public float OutlineBlackStrength;

    private void Update() {
        BarOutlineRT.localScale = BarRT.localScale = new Vector3(PctLeft, 1f, 1f);
        BarImage.color = BarColorByPctLeft.Evaluate(PctLeft);
        BarOutlineImage.color = BarColorByPctLeft.Evaluate(PctLeft).AddDab(Color.black, OutlineBlackStrength);
    }

}
