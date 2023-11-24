using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
TODO:
-enforce dragging along 8 directions (x/y axis and diagonals) only
    -plan: check the direction of the first square touched after drag begins
    -that direction is the main direction
    -every next node has every direction other than the main direction locked
    -player has to stop dragging to start dragging in a new direction
-allow removing letters from letterList on backdragging
    -plan: use isSelected bool as a toggle
    -if isSelected is false, node gets added to list on PointerOver, then flip isSelected
    -if isSelected is true, node gets removed from list on PointerOver, then flip isSelected
-way to track words already spotted and generate success message once all words are found
    -plan: use Remove() to take out words from list
    -once list is empty, success
-use scriptable objects to store and load wordsearch matrices and word lists (loadout style)
*/

public class WordsearchLetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler{
    [SerializeField] char letter;

    public char Letter {
        get { return letter; }
        set { letter = value; }
    }

    bool isSelected, isSearched;
    
    public bool IsSelected {
        get { return isSelected; }
        set { isSelected = value; }
    }
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

    //changes letterBg color based on whether or not letter is part of a word
    public void SetColor(){
        if(isSearched){
            letterBg.color = WordsearchManager.Instance.SearchColor;
        } else {
            letterBg.color = WordsearchManager.Instance.BaseColor;
        }
    }
    
    //changes letter shown on tile
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
        isSelected = true;

        WordsearchManager.Instance.IsDragging = true;
        WordsearchManager.Instance.LetterList.Add(this);
    }

    public void OnPointerUp(PointerEventData eventData){
        WordsearchManager.Instance.IsDragging = false;
        WordsearchManager.Instance.EndDrag();
    }

}
