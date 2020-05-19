using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreMarker : MonoBehaviour
{
    // Start is called before the first frame update
    public Mesh[] numMeshes;
    public GameObject[] numbers;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        InvokeRepeating("UpdateScoreText", 0f, 0.2f);
    }

    void UpdateScoreText()
    {
        string textScore = gameManager.score.ToString();
        int length = textScore.Length;

        for (int i = 0; i < length; ++i )
        {
            char num = textScore[i];
            GameObject obj = numbers[length - 1 - i];
            obj.GetComponent<MeshFilter>().mesh = numMeshes[(int)char.GetNumericValue(num)];
        }
    }
    // Update is called once per frame
}
