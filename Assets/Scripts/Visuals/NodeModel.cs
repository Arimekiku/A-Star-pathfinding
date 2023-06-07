using System.Collections;
using UnityEngine;

public class NodeModel : MonoBehaviour
{
    [SerializeField] private AnimationCurve _spawnCurve;
    [SerializeField] private AnimationCurve _markCurve;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private Vector3 _targetRotation;
    [Space, SerializeField] private Sprite[] _sprites;

    private float _currentTime = 0f, _targetTime = 1f, _animationSpeed = 1f; 

    private SpriteRenderer _renderer;

    private void Awake() => _renderer = GetComponentInChildren<SpriteRenderer>();  

    private void Start() 
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() 
    {
        while(_currentTime != _targetTime) 
        {
            _currentTime = Mathf.MoveTowards(_currentTime, _targetTime, _animationSpeed * Time.deltaTime);

            transform.localScale = Vector3.Lerp(Vector3.zero, _targetScale, _spawnCurve.Evaluate(_currentTime));
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 0f, 180f), Quaternion.Euler(_targetRotation), _spawnCurve.Evaluate(_currentTime));

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Mark(Sprite sprite) 
    {
        _currentTime = 0f;
        Vector3 targetScale = new Vector3(1.2f, 1.2f, 1.2f);
        Vector3 currentScale = transform.localScale;

        while(_currentTime != _targetTime) 
        {
            _currentTime = Mathf.MoveTowards(_currentTime, _targetTime, _animationSpeed * Time.deltaTime);
            float currentCurveValue = _markCurve.Evaluate(_currentTime);

            if (currentCurveValue > 0.9f)
                _renderer.sprite = sprite;
            
            transform.localScale = Vector3.Lerp(currentScale, targetScale, currentCurveValue);

            yield return new WaitForEndOfFrame();
        }
    }

    public void MarkAsTarget() => StartCoroutine(Mark(_sprites[1]));
    public void MarkAsStart() => _renderer.sprite = _sprites[2];
    public void MarkAsPath() => StartCoroutine(Mark(_sprites[3]));
    public void ClearMark() => StartCoroutine(Mark(_sprites[0]));
}
