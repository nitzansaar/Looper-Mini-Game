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
        gm.dynamic.GetComponent<TMP_Text>().text = gm.originalThoughtArray[gm.index];
        gm.scoreboard.GetComponent<TMP_Text>().text = gm.total_score.ToString();
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
            gm.Win();
        }
        else
        {
            if (gm.puzzle.currValue == previousScore + 1)
            {
                gm.total_score += 10;
                updateTextBox(1, true);
            }
            else if (gm.puzzle.currValue == previousScore + 2)
            {
                gm.total_score += 20;
                updateTextBox(2, true);
            }
            else if (gm.puzzle.currValue == previousScore + 3)
            {
                gm.total_score += 30;
                updateTextBox(3, true);
            }
            else if (gm.puzzle.currValue == previousScore - 1)
            {
                gm.total_score -= 10;
                updateTextBox(0, false);
            }
            gm.scoreboard.GetComponent<TMP_Text>().text = gm.total_score.ToString();

        }

    }



    void updateTextBox(int incrementValue, bool increase)
    {
        if (increase)
        {
            if (gm.index >= 0 && gm.index < gm.thoughtArray.Length)
            {
                while (incrementValue > 0)
                {
                    if (gm.index != gm.thoughtArray.Length - 1)
                    {
                        gm.originalSentence = gm.originalThoughtArray[gm.index];
                        gm.revisedSentence += gm.thoughtArray[gm.index] + " ";
                        gm.dynamic.GetComponent<TMP_Text>().text = gm.revisedSentence + " " + gm.originalSentence;
                        gm.index++;
                    }
                    incrementValue--;
                }

            }
        }
        else
        {
            // Ensure index is within bounds before accessing thoughtArray
            if (gm.index > 0 && gm.index <= gm.thoughtArray.Length)
            {
                string currentText = gm.revisedSentence;
                int lengthToRemove = gm.thoughtArray[gm.index - 1].Length + 1; // Add 1 for the space

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
                if (gm.index != gm.thoughtArray.Length - 1)
                    gm.dynamic.GetComponent<TMP_Text>().text = gm.revisedSentence + " " + gm.originalSentence;

            }
        }

    }




    private void updateScore()
    {
        int prevCurrValue = gm.puzzle.currValue;
        int difference = -gm.QuickSweep((int)transform.position.x, (int)transform.position.y);

        if (gm.allowRotation)
        {
            RotatePiece();
        }

        difference += gm.QuickSweep((int)transform.position.x, (int)transform.position.y);
        gm.puzzle.currValue += difference;

        int currValueChange = gm.puzzle.currValue - prevCurrValue;
        if (currValueChange > 0)
        {
            gm.total_score += currValueChange * 10;
        }

        // Update the scoreboard text
        gm.scoreboard.GetComponent<TMP_Text>().text = gm.total_score.ToString();
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