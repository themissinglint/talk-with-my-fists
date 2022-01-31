using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour {

    public float PctLeft => 1f - WorldStatus.GameTimeProgress;

    public RectTransform BarOutlineRT;
    public RectTransform BarRT;
    public Image BarOutlineImage;
    public Image BarImage;

    public Gradient BarColorByPctLeft;
    public float OutlineBlackStrength;

    [Header("TimerText")]
    public TextMeshProUGUI TimerText;
    private float TimeLeft => WorldStatus.GameDuration * (1f - WorldStatus.GameTimeProgress);
    private int TimeLeftCeil => Mathf.CeilToInt(TimeLeft);

    public int FinalSecondsThreshold;
    public AnimationCurve TimerTextScaleByTimeSinceLastSecond;
    public Gradient ColorByTimeSinceLastThreshold;

    private void Update() {
        BarOutlineRT.localScale = BarRT.localScale = new Vector3(PctLeft, 1f, 1f);
        BarImage.color = BarColorByPctLeft.Evaluate(PctLeft);
        BarOutlineImage.color = BarColorByPctLeft.Evaluate(PctLeft).AddDab(Color.black, OutlineBlackStrength);
        
        // Update Timer Text
        TimerText.text = TimeLeftCeil.ToString();
        if (TimeLeft < FinalSecondsThreshold) {
            float timeSinceLastSecond = (float) TimeLeftCeil - TimeLeft;
            TimerText.transform.localScale = Vector2.one * TimerTextScaleByTimeSinceLastSecond.Evaluate(timeSinceLastSecond);
            TimerText.color = ColorByTimeSinceLastThreshold.Evaluate(timeSinceLastSecond);
        }
    }

}
