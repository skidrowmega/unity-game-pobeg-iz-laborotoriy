using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    int Index;

    [SerializeField] bool debug=false;


    public static DialogueBox instance { get; private set;  }
    public string[] lines;

    [Header("Скорость появления символов")]
    public float textspeed;
    [Header("Звук при печати символа")]
    public AudioClip sound;
    AudioSource audioSource;

    public Text text;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance)
            Debug.LogError("Упс");
        instance = this;
    }

    private void Start()
    {
        if (debug)
        {
            string[] newlines = new string[]
            {
                "ПРивет я лягешкуа",
                "Я шас покажу тебе как выживаться"
            };
            DialogueBox.StartDialogue(newlines);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (text.text == lines[Index])
            {
                NextLine();
            }
            else
            {
                if(routine!=null)StopCoroutine(routine);
                text.text= lines[Index];
            }
        }
    }

    Coroutine routine;

    public void StartDialogue()
    {
        Index = 0;
        text.text = string.Empty;
        text.transform.parent.gameObject.SetActive(true);
        routine=StartCoroutine(TypeLine());
    }

    public static void StartDialogue(string[] Lines)
    {
        instance.lines = Lines;
        instance.StartDialogue();
    }

    IEnumerator TypeLine()
    {
        foreach (char line in lines[Index].ToCharArray())
        {
            text.text += line;
            audioSource.PlayOneShot(sound);
            yield return new WaitForSeconds(textspeed);
            audioSource.Stop();
        }
    }

    void NextLine()
    {
        if(Index < lines.Length - 1)
        {
            Index++;
            text.text = string.Empty;
            routine=StartCoroutine(TypeLine());
        }
        else
        {
            text.transform.parent.gameObject.SetActive(false);
        }
    }

}
