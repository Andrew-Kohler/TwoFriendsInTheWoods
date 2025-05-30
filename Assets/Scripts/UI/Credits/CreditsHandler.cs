using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsHandler : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField, Tooltip("Animator for the image of the two hiking back")] private Animator _fullCreditsAnimator;
    [SerializeField, Tooltip("Animator for mini Chara 1")] private Animator _charaOneAnimator;
    [SerializeField, Tooltip("Animator for mini Chara 2")] private Animator _charaTwoAnimator;
    [SerializeField, Tooltip("Animator for mini Chara 2")] private Animator _fadeAnimator;
    [Header("Text Blocks")]
    [SerializeField, Tooltip("Poem text in part 1")] private TextMeshProUGUI _poemText;
    [SerializeField, Tooltip("Poem credit text in part 1")] private TextMeshProUGUI _poemCreditText;
    [SerializeField, Tooltip("Credit text in main credits")] private TextMeshProUGUI _creditText;
    [SerializeField, Tooltip("Dedication text in main credits")] private TextMeshProUGUI _dedicationText;
    [Header("Game Objects")]
    [SerializeField, Tooltip("Part 1")] private GameObject _part1;
    [SerializeField, Tooltip("Part 2")] private GameObject _part2;

    private AudioSource _source;
    private bool _activeC;
    void Start()
    {
        _poemText.text = "";
        _poemCreditText.text = "";
        _creditText.text = "";
        _dedicationText.text = "";
        _source = GetComponent<AudioSource>();
        StartCoroutine(DoCredits());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DoCredits()
    {
        GameManager.GameComplete = true;
        // Part 1: Poetry ---------------------------------------------------------------------------------

        // Wait for the fade
        _fadeAnimator.speed = 2f;
        yield return new WaitForSeconds(2.5f);
        _fadeAnimator.speed = 1f;

        // Read the poem
        StartCoroutine(DoTextRead(_poemText, "\"The morning breaks; the steeds in their stalls \nStamp and neigh, as the hostler calls; \nThe day returns, but nevermore\nReturns the traveller to the shore,\n      And the tide rises, the tide falls.\"", 999f));
        yield return new WaitForSeconds(14f);

        StartCoroutine(DoTextRead(_poemCreditText, "- Henry Wadsworth Longfellow\n   \"The Tide Rises, The Tide Falls\"", 999f));
        yield return new WaitForSeconds(10f);

        _fadeAnimator.Play("SceneExit", 0, 0);
        yield return new WaitForSeconds(2f);
        _part1.SetActive(false);

        // Part 2: The actual credits ---------------------------------------------------------------------

        // Wait for the fade in
        _part2.SetActive(true);
        _fadeAnimator.Play("SceneEnter", 0, 0);
        yield return new WaitForSeconds(4.5f);

        // Start the credits scroll animation
        _fullCreditsAnimator.enabled = true;
        _charaOneAnimator.Play("Walk", 0, 0);
        yield return new WaitForSeconds(2f);
        _charaTwoAnimator.Play("Walk", 0, 0);
        yield return new WaitForSeconds(.2f);
        _source.Play();

        // Wait a little, then start doing text
        yield return new WaitForSeconds(1.8f);
        StartCoroutine(DoTextRead(_creditText, "A game by Andrew Kohler", 10f)); 
        yield return new WaitForSeconds(10f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "Made in completion of an undergraduate honors thesis at the University of Florida", 10f));
        yield return new WaitForSeconds(10f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "Sounds by Traian1984, Fabrizio84, Garuda1982, and bspiller5", 11f));
        yield return new WaitForSeconds(11f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "'Wild Mountain Thyme' performed by Susan Greenwood", 11f));
        yield return new WaitForSeconds(11f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "Tested by Susan G., Brianna L., and Snake Enthusiast Chloe M. Van Horn", 10f));
        yield return new WaitForSeconds(10f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "Special thanks to Professor Joshua Fox, my advisor...", 10f));
        yield return new WaitForSeconds(10f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "...and to everyone who believed.", 11f));
        yield return new WaitForSeconds(11f);

        yield return new WaitUntil(() => !_activeC);
        StartCoroutine(DoTextRead(_creditText, "(c) Narwhal Productions 2025", 9f));
        yield return new WaitForSeconds(9f);

        yield return new WaitUntil(() => !_activeC);
        yield return new WaitForSeconds(2.25f);
        StartCoroutine(DoTextRead(_dedicationText, "For Rue, my friend in the woods", 999f));

        // Wait a bit, then fade out, then back to the main menu
        yield return new WaitForSeconds(8f);
        _fadeAnimator.Play("SceneExit", 0, 0);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");

    }

    private IEnumerator DoTextRead(TextMeshProUGUI text, string line, float readTime)
    {
        _activeC = true;
        for (int j = 0; j < line.Length; j++)
        {
            if (line[j].Equals(' '))
            {
                j++;
            }
            text.text = line.Substring(0, j);
            yield return new WaitForSeconds(.07f);
            readTime -= .07f;

        }
        text.text = line;
        yield return new WaitForSeconds(readTime);
        text.text = "";
        _activeC = false;
        
    }
}
