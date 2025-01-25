using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;
    
    public bool IsRunning { get; private set; }
    
    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>() { '.', '!', '?' }, 0.1f ),
        new Punctuation(new HashSet<char>() { ',', ';', ':' }, 0.1f )
    };
    
    private Coroutine typeingCoroutine;
    
    public void Run(string textToType, TMP_Text textLabel)
    {
        typeingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(typeingCoroutine);
        IsRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        IsRunning = true;
        
        textLabel.text = string.Empty;
        
        float t = 0;
        int CharIndex = 0;

        while (CharIndex < textToType.Length)
        {
            int lastCharIndex = CharIndex;
            
            t += typeWriterSpeed * Time.deltaTime;
            
            CharIndex = Mathf.FloorToInt(t);
            CharIndex = Mathf.Clamp(CharIndex, 0, textToType.Length);

            for (int i = lastCharIndex; i < CharIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;
                
                textLabel.text  = textToType.Substring(0, i + 1);

                if (IsPunctuation(textToType[i], out float waitTiem) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTiem);
                }
            }
            
            yield return null;
        }
        IsRunning = false;
    }

    private bool IsPunctuation(char character, out float waitime)
    {
        foreach (Punctuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitime = punctuationCategory.waitTime;
                return true;
            }
        }
        
        waitime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float waitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            this.waitTime = waitTime;
        }
    }
}
