using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Linq;
using UnityEngine;

public class WordsearchManager : MonoBehaviour{
    public static WordsearchManager Instance { get; private set; }
    void Awake(){
        if(Instance == null){
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    [SerializeField, TextArea(10,10)] string wordsearch;

    [SerializeField] Color baseColor, activeColor, searchColor;
    public Color BaseColor {
        get { return baseColor; }
        set { baseColor = value; }
    }
    public Color ActiveColor {
        get { return activeColor; }
        set { activeColor = value; }
    }
    public Color SearchColor {
        get { return searchColor; }
        set { searchColor = value; }
    }

    [SerializeField] List<string> validWords;
    [SerializeField] bool isDragging;
    public bool IsDragging {
        get { return isDragging; }
        set { isDragging = value; }
    }

    List<WordsearchLetter> letterList = new List<WordsearchLetter>();
    public List<WordsearchLetter> LetterList {
        get { return letterList; }
        set { letterList = value; }
    }
    [SerializeField] WordsearchLetter wordsearchLetterObj;
    [SerializeField] GameObject wordsearchGrid;

    void Start(){
        string[] letterRows = wordsearch.Split('\n');
        char[][] letterGrid = new char[letterRows.Length][];
        for(int i = 0; i < letterGrid.GetLength(0); i++){
            letterGrid[i] = letterRows[i].ToCharArray();
        }

        Debug.Log(letterGrid[2][4]);

        WordsearchLetter[][] matrix = new WordsearchLetter[letterGrid.Length][];
        for(int i = 0; i < matrix.Length; i++){
            matrix[i] = new WordsearchLetter[letterGrid[i].Length];
            for(int j = 0; j < matrix[i].Length; j++){
                var letterObj = Instantiate(wordsearchLetterObj);
                letterObj.SetLetter(letterGrid[i][j]);
                letterObj.gameObject.transform.SetParent(wordsearchGrid.transform);
            }
        }
    }

    public void EndDrag(){
        string word = "";
        foreach(WordsearchLetter letter in letterList){
            letter.SetColor();
            word += letter.Letter;
        }
        Debug.Log(word.ToUpper());

        if(validWords.Contains(word.ToUpper())){
            foreach(WordsearchLetter letter in letterList){
                letter.IsSearched = true;
                letter.SetColor();
            }
        }

        letterList.Clear();
    }
}
