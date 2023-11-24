using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WordsearchLetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler{
    [SerializeField] char letter;

    public char Letter {
        get { return letter; }
        set { letter = value; }
    }

    bool isSearched;
    public bool IsSearched {
        get { return isSearched; }
        set { isSearched = value; }
    }

    WordsearchLetter n, s, e, w;
    Image letterBg;
    public Image LetterBg {
        get { return letterBg; }
        set { letterBg = value; }
    }

    void Awake(){
        letterBg = GetComponent<Image>();
    }

    public void SetColor(){
        if(isSearched){
            letterBg.color = WordsearchManager.Instance.SearchColor;
        } else {
            letterBg.color = WordsearchManager.Instance.BaseColor;
        }
    }

    public void SetLetter(char letter){
        this.letter = letter;

        var letterText = transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
        letterText.text = this.letter.ToString().ToUpper();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData){
        //change color to activecolor
        letterBg.color = WordsearchManager.Instance.ActiveColor;

        if(WordsearchManager.Instance.IsDragging){
            WordsearchManager.Instance.LetterList.Add(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        //change color to basecolor
        //or searchcolor if isSearched is true
        if(!WordsearchManager.Instance.IsDragging){
            if(isSearched){
               letterBg.color = WordsearchManager.Instance.SearchColor; 
            } else {
                letterBg.color = WordsearchManager.Instance.BaseColor;
            }
        }
    }
    
    public void OnPointerDown(PointerEventData eventData){
        WordsearchManager.Instance.IsDragging = true;
        WordsearchManager.Instance.LetterList.Add(this);
    }

    public void OnPointerUp(PointerEventData eventData){
        WordsearchManager.Instance.IsDragging = false;
        WordsearchManager.Instance.EndDrag();
    }

}
