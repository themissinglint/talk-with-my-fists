using UnityEngine;
using UnityEngine.UI;

public class InteractionToastDisplay : MonoBehaviour {

    public static InteractionToastDisplay Instance;
    
    // Anatomy
    public Image BackgroundImage;
    public RectTransform RT;
    
    // Expansion
    public float ClosedSize;
    public float SmallSize;
    public float LargeSize;

    public AnimationCurve EaseOpen;
    public AnimationCurve EaseClosed;

    public float TweenDampTime;

    private float _stateStartTime;
    private float _scaleCurrent;
    private float _scaleVelocity;
    private float CurrentStateAge => Time.time - _stateStartTime;

    private float _scalePrev;
    private float _scaleNext;

    private float TargetScale => _scaleNext > _scalePrev
        ? Mathf.Lerp(_scalePrev, _scaleNext, EaseOpen.Evaluate(CurrentStateAge))
        : Mathf.Lerp(_scalePrev, _scaleNext, EaseClosed.Evaluate(CurrentStateAge));
    
    // Current Toast
    private InteractionToastData.ToastScale _targetScale = InteractionToastData.ToastScale.none;
    private InteractionToastData _currentToast;
    private float _currentToastStartTime;
    private float CurrentToastAge => Time.time - _currentToastStartTime;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _scalePrev = _scaleNext = ClosedSize;
    }

    public void PopToast(InteractionToastData data) {
        BackgroundImage.color = data.BackgroundColor;
        _currentToast = data;
        _currentToastStartTime = Time.time;

        if (data.Scale > _targetScale) {
            SetTargetScale(data.Scale);
        }
    }

    private void SetTargetScale(InteractionToastData.ToastScale newScale) {
        _stateStartTime = Time.time;
        _targetScale = newScale;
        _scalePrev = _scaleCurrent;
        switch (newScale) {
            case InteractionToastData.ToastScale.none:
                _scaleNext = ClosedSize;
                break;
            case InteractionToastData.ToastScale.small:
                _scaleNext = SmallSize;
                break;
            case InteractionToastData.ToastScale.large:
                _scaleNext = LargeSize;
                break;
        }
    }

    private void Update() {
        
        // Resize toward the appropriate toast size.
        _scaleCurrent = Mathf.SmoothDamp(_scaleCurrent, TargetScale, ref _scaleVelocity, TweenDampTime);
        RT.sizeDelta = new Vector2(_scaleCurrent, _scaleCurrent);

        if (_currentToast == null) return;
        if (CurrentToastAge > _currentToast.Time) {
            _currentToast = null;
            SetTargetScale(InteractionToastData.ToastScale.none);
        }

    }

}
