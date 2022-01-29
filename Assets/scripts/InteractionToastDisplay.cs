using UnityEngine;
using UnityEngine.UI;

public class InteractionToastDisplay : MonoBehaviour {

    public static InteractionToastDisplay Instance;
    
    // Anatomy
    public RectTransform RT;
    public Image BackgroundImage;
    public Image EntityImage;
    public RectTransform EntityRT;
    
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

    // Entity Scaling Fudge
    private InteractionToastData.ToastScale _latestToastTargetScale = InteractionToastData.ToastScale.none;
    private Vector2 _latestToastEntityOffsetVector;
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _scalePrev = _scaleNext = ClosedSize;
    }

    public void PopToast(InteractionToastData data) {
        BackgroundImage.color = data.BackgroundColor;
        EntityImage.sprite = data.EntitySprite;
        EntityRT.localScale = data.EntityScaleVector;
        _latestToastTargetScale = data.Scale;
        _latestToastEntityOffsetVector = data.EntityOffsetVector;
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
        _scaleNext = PxFromScale(newScale);
    }

    private void Update() {
        
        // Resize toward the appropriate toast size.
        _scaleCurrent = Mathf.SmoothDamp(_scaleCurrent, TargetScale, ref _scaleVelocity, TweenDampTime);
        RT.sizeDelta = new Vector2(_scaleCurrent, _scaleCurrent);
        
        EntityRT.anchoredPosition = _latestToastEntityOffsetVector * _scaleCurrent / PxFromScale(_latestToastTargetScale);

        if (_currentToast == null) return;
        if (CurrentToastAge > _currentToast.Time) {
            _currentToast = null;
            SetTargetScale(InteractionToastData.ToastScale.none);
        }
    }

    private float PxFromScale(InteractionToastData.ToastScale scale) {
        switch (scale) {
            case InteractionToastData.ToastScale.none:
                return ClosedSize;
            case InteractionToastData.ToastScale.small:
                return SmallSize;
            case InteractionToastData.ToastScale.large:
                return LargeSize;
        }

        return ClosedSize;
    }

}
