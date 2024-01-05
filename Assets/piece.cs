using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;




public class piece : MonoBehaviour
{

    public int[] values;
    public float speed;
    float realRotation;


    // public string original;
    // public string revised = "";


    public GameManager gm;


    // Use this for initialization
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        // Debug.Log("Piece class starting");
    }


    // Update is called once per frame
    void Update()
    {

        if (transform.rotation.eulerAngles.z != realRotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, realRotation), speed);
        }

    }


    void OnMouseDown()
    {
        // Get the current score before the update
        int previousScore = gm.puzzle.currValue;


        updateScore();


        if (gm.puzzle.currValue == gm.puzzle.winValue)
        {
            // Debug.Log(gm.thoughtArray[gm.thoughtArray.Length - 1]);
            gm.Win();
        }
        else
        {
            // Check if the current score increased by one
            if (gm.puzzle.currValue == previousScore + 1)
            {
                // Ensure index is within bounds before accessing thoughtArray
                if (gm.index >= 0 && gm.index < gm.thoughtArray.Length)
                {
                    gm.originalSentence = gm.originalThoughtArray[gm.index];
                    gm.revisedSentence += gm.thoughtArray[gm.index] + " ";
                    // Debug.Log(gm.revisedSentence.GetComponent<TMP_Text>().text + " " + gm.originalSentence.GetComponent<TMP_Text>().text);
                    // gm.dynamic.GetComponent<TMP_Text>().text = gm.eventText.GetComponent<TMP_Text>().text + " " + gm.originalSentence.GetComponent<TMP_Text>().text;
                    // Debug.Log(gm.originalSentence.GetComponent<TMP_Text>().text + " " + gm.revisedSentence.GetComponent<TMP_Text>().text);
                    gm.dynamic.GetComponent<TMP_Text>().text = gm.originalSentence + " " + gm.revisedSentence;
                    gm.index++;


                }
            }
            else if (gm.puzzle.currValue == previousScore - 1)
            {
                // Ensure index is within bounds before accessing thoughtArray
                if (gm.index > 0 && gm.index <= gm.thoughtArray.Length)
                {

                    // Get the current text from eventText
                    string currentText = gm.revisedSentence;


                    // Get the length of the string to be removed
                    int lengthToRemove = gm.thoughtArray[gm.index - 1].Length + 1; // Add 1 for the space


                    // Check if the current text contains the string to be removed
                    if (currentText.Contains(gm.thoughtArray[gm.index - 1]))
                    {
                        // Remove the string from the current text
                        currentText = currentText.Remove(currentText.LastIndexOf(gm.thoughtArray[gm.index - 1]), lengthToRemove);
                    }


                    // Update the eventText with the modified string
                    gm.revisedSentence = currentText;


                    // Ensure index is within bounds before accessing originalThoughtArray
                    if (gm.index > 0 && gm.index < gm.originalThoughtArray.Length)
                    {
                        gm.originalSentence = gm.originalThoughtArray[--gm.index];
                    }
                    else
                    {
                        gm.originalSentence = "";
                    }
                    // Debug.Log(gm.originalSentence.GetComponent<TMP_Text>().text + " " + gm.revisedSentence.GetComponent<TMP_Text>().text);
                    gm.dynamic.GetComponent<TMP_Text>().text = gm.originalSentence + " " + gm.revisedSentence;

                }
            }
        }
    }








    private void updateScore()
    {
        int difference = -gm.QuickSweep((int)transform.position.x, (int)transform.position.y);
        if (gm.allowRotation)
        {
            RotatePiece();
        }
        difference += gm.QuickSweep((int)transform.position.x, (int)transform.position.y);


        gm.puzzle.currValue += difference;


        // Debug.Log(incrementValue);
    }





    public void RotatePiece()
    {
        realRotation += 90;




        if (realRotation == 360)
            realRotation = 0;




        RotateValues();
    }




    public void RotateValues()
    {
        if (values.Length != 0)
        {
            int aux = values[0];




            for (int i = 0; i < 3; i++)
            {
                values[i] = values[i + 1];
            }
            values[3] = aux;
        }
    }
}
