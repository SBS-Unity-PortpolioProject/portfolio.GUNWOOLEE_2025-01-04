using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    
    [SerializeField] private Vector3 moveSpeed = new Vector3(0, 75, 0);

    public float timeToFade = 1f;
    
    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;

    private float timeElapsed = 0;
    private Color startColor;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;
        
        timeElapsed += Time.deltaTime;
        if (timeElapsed < timeToFade)
        {
            float newAlpha = startColor.a * (1 - (timeElapsed / timeToFade));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    


}
