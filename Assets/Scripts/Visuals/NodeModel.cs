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
    private Color _startColor, _targetColor;

    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;

    private void Awake() 
    { 
        _renderer = GetComponentInChildren<SpriteRenderer>();  
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start() 
    {   
        _collider.enabled = false;

        Material rMaterial = _renderer.material;
        _startColor = new(rMaterial.color.r, rMaterial.color.g, rMaterial.color.b, 0);
        _targetColor = new(rMaterial.color.r, rMaterial.color.g, rMaterial.color.b, rMaterial.color.a);

        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() 
    {
        Vector3 initialPosition = transform.localPosition;

        while(_currentTime != _targetTime) 
        {
            _currentTime = Mathf.MoveTowards(_currentTime, _targetTime, _animationSpeed * Time.deltaTime);
            
            _renderer.material.color = Color.Lerp(_startColor, _targetColor, _spawnCurve.Evaluate(_currentTime) * 2f);
            transform.localPosition = Vector3.Lerp(Vector3.left + initialPosition, initialPosition, _spawnCurve.Evaluate(_currentTime));

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Mark(Sprite sprite, float speed) 
    {
        _currentTime = 0f;
        Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
        Vector3 startScale = Vector3.one;

        while(_currentTime != _targetTime) 
        {
            _currentTime = Mathf.MoveTowards(_currentTime, _targetTime, speed);
            float currentCurveValue = _markCurve.Evaluate(_currentTime);

            if (currentCurveValue > 0.9f)
                _renderer.sprite = sprite;
            
            transform.localScale = Vector3.Lerp(startScale, targetScale, currentCurveValue);

            yield return new WaitForEndOfFrame();
        }
    }

    public void MarkAsTarget() => StartCoroutine(Mark(_sprites[1], _animationSpeed * Time.deltaTime * 5f));
    public void MarkAsStart() => StartCoroutine(Mark(_sprites[2], _animationSpeed * Time.deltaTime / 2f));
    public void MarkAsPath() => StartCoroutine(Mark(_sprites[3], _animationSpeed * Time.deltaTime * 5f));
    public void ClearMark() => StartCoroutine(Mark(_sprites[0], _animationSpeed * Time.deltaTime * 5f));
    public void EnableCollider() => _collider.enabled = true;
}
