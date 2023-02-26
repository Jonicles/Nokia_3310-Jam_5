using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private int points;
    Text pointText;

    // Start is called before the first frame update
    void Start()
    {
        pointText = gameObject.GetComponent<Text>();

        //Startar spelet med 0 poäng och uppdaterar UI elementet så att det alltid stämmer
        points = 0;
        pointText.text = "Score:" + points;
    }

    private void Update()
    {
        Debug.Log(points);
    }

    //Metod som kan kallas utifrån för att lägga till poäng till UI
    public void AddPoints(int addedPoints)
    {
        points += addedPoints;
        pointText.text = "Score:" + points;

    }

    public int GetPoints()
    {
        return points;
    }

    public void ResetPoints()
    {
        points = 0;
        pointText.text = "Score:" + points;
    }

}
