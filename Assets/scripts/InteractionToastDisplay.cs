using System.Runtime.CompilerServices;
using MoreMountains.CorgiEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionToastDisplay : MonoBehaviour {

    public static InteractionToastDisplay Instance;
    
    // Reference
    public GameObject PlayerObjectForTest;
    
    // Anatomy
    public RectTransform RT;
    public Image BackgroundImage;
    public Image EntityImage;
    public RectTransform EntityRT;
    public Animator CollectableAnimator;
    public TextMeshProUGUI CollectableText;
    
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
    private Toast _currentToast;

    private class Toast {
        public InteractionToastData Data;
        public GameObject SourceObject;
        public float StartTime;
        public float Age => Time.time - StartTime;
        public float Distance => SourceObject == null ? Mathf.Infinity : Vector2.Distance(Instance.PlayerObjectForTest.transform.position, SourceObject.transform.position);
        public bool IsExpiring;
        public float ExpireStartTime;
        public float TimeSpentExpiring => Time.time - ExpireStartTime;
        public int CollectableCount;

        public bool ShouldStartExpiring() {
            if (Data.ExpiresFromTime && Age > Data.ExpirationTime) return true;
            if (Data.ExpiresFromDistance && Distance > Data.ExpirationDistance) return true;
            return false;
        }

        public bool ShouldExpire() {
            if (ShouldStartExpiring() && !Data.HasDialogue) return true;
            if (IsExpiring && TimeSpentExpiring > Instance.DialogueOutTime) return true;
            return false;
        }
    }

    // Entity Scaling Fudge
    private InteractionToastData.ToastScale _latestToastTargetScale = InteractionToastData.ToastScale.none;
    private Vector2 _latestToastEntityOffsetVector;

    [Header("Dialogue")]
    public RectTransform DialogueRT;
    public TextMeshProUGUI DialogueText;
    public CanvasGroup DialogueBodyCanvasGroup;
    public CanvasGroup DialogueTextCanvasGroup;
    public float DialogueMinWidth;
    public float DialogueMaxWidth;
    public AnimationCurve DialogueCurveWidth;
    public AnimationCurve DialogueCurveTextAlpha;
    public AnimationCurve DialogueCurveBodyAlpha;
    public float DialogueInTime;
    public float DialogueOutTime;
    public float DialogueDampTime;
    private bool DialogueBoxOpen => _currentToast != null && (_currentToast.Data.HasDialogue && !_currentToast.IsExpiring);
    private bool _dialogueBoxWasOpen;
    private float _dialogueProgress;
    private float _dialogueDisplayProgressCurrent;
    private float _dialogueDisplayProgressVelocity;

    private int _collectableBounceCount;
    
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _scalePrev = _scaleNext = ClosedSize;
    }

    public void PopToast(InteractionToastData data, GameObject source, int collectableNumber = 0) {
        if (!data.ExpiresFromDistance && !data.ExpiresFromTime) {
            Debug.LogError($"{nameof(PopToast)} called with a data that cannot expire!");
        }

        if (_currentToast != null && _currentToast.Data.Scale == InteractionToastData.ToastScale.small && data.Scale == InteractionToastData.ToastScale.small) {
            _collectableBounceCount++;
            CollectableAnimator.Play("Toast Small Item Bounce" + (_collectableBounceCount % 2 == 0 ? "" : " 0"));
        }
        
        CollectableText.gameObject.SetActive(collectableNumber > 0);
        CollectableText.text = collectableNumber.ToString();

        if (CanActivateToast()) {
            _currentToast = new Toast() {
                Data = data,
                SourceObject = source,
                StartTime = Time.time,
                CollectableCount = collectableNumber
            };
            ActivateCurrentToast();
        }
    }

    private bool CanActivateToast() {
        if (_currentToast == null) return true;
        if (_currentToast.Data.Scale == InteractionToastData.ToastScale.small) return true;
        return false;
    }

    private void ActivateCurrentToast() {
        BackgroundImage.color = _currentToast.Data.BackgroundColor;
        EntityImage.sprite = _currentToast.Data.EntitySprite;
        EntityImage.color = Color.white;
        EntityRT.localScale = _currentToast.Data.EntityScaleVector;
        _latestToastTargetScale = _currentToast.Data.Scale;
        _latestToastEntityOffsetVector = _currentToast.Data.EntityOffsetVector;
        SetTargetScale(_currentToast.Data.Scale);
        if (_currentToast.Data.HasDialogue) {
            DialogueText.color = _currentToast.Data.DialogueColor;
            DialogueText.text = _currentToast.Data.DialogueText;
        }
    }
    
    private void SetTargetScale(InteractionToastData.ToastScale newScale) {
        _stateStartTime = Time.time;
        _scalePrev = _scaleCurrent;
        _scaleNext = PxFromScale(newScale);
    }

    private void Update() {

        if (PlayerObjectForTest == null) {
            PlayerObjectForTest = GameObject.FindWithTag("Player");
        }
        
        // Resize toward the appropriate toast size.
        _scaleCurrent = Mathf.SmoothDamp(_scaleCurrent, TargetScale, ref _scaleVelocity, TweenDampTime);
        RT.sizeDelta = new Vector2(_scaleCurrent, _scaleCurrent);
        
        EntityRT.anchoredPosition = _latestToastEntityOffsetVector * _scaleCurrent / PxFromScale(_latestToastTargetScale);

        // Open or close the dialogue box.
        if (DialogueBoxOpen != _dialogueBoxWasOpen) {
            _dialogueBoxWasOpen = DialogueBoxOpen;
        }

        if (DialogueBoxOpen) _dialogueProgress += Time.deltaTime / DialogueInTime;
        else _dialogueProgress -= Time.deltaTime / DialogueOutTime;
        _dialogueProgress = Mathf.Clamp01(_dialogueProgress);
        _dialogueDisplayProgressCurrent = Mathf.SmoothDamp(_dialogueDisplayProgressCurrent, _dialogueProgress, ref _dialogueDisplayProgressVelocity, DialogueDampTime);
        DialogueRT.sizeDelta = new Vector2(Mathf.Lerp(DialogueMinWidth, DialogueMaxWidth, DialogueCurveWidth.Evaluate(_dialogueDisplayProgressCurrent)), DialogueRT.sizeDelta.y);
        DialogueBodyCanvasGroup.alpha = Mathf.Lerp(0f, 1f, DialogueCurveBodyAlpha.Evaluate(_dialogueDisplayProgressCurrent));
        DialogueTextCanvasGroup.alpha = Mathf.Lerp(0f, 1f, DialogueCurveTextAlpha.Evaluate(_dialogueDisplayProgressCurrent));

        if (_currentToast != null && !_currentToast.IsExpiring && _currentToast.CollectableCount > 0) {
            CollectableText.gameObject.SetActive(_scaleCurrent >= SmallSize - 40f);
        }
        
        // Expire the current toast if appropriate.
        if (_currentToast == null) return;
        if (!_currentToast.IsExpiring && _currentToast.ShouldStartExpiring()) {
            _currentToast.IsExpiring = true;
            _currentToast.ExpireStartTime = Time.time;
            CollectableText.gameObject.SetActive(false);
        }
        if (_currentToast.ShouldExpire()) {
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
