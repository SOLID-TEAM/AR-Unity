using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreMarker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject destroyParticle;
    public Mesh[] numMeshes;
    public GameObject[] numbers;
    public GameObject[] lifes;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        InvokeRepeating("UpdateScore", 0f, 0.2f);
    }

    void UpdateScore()
    {
        string textScore = gameManager.score.ToString();
        for (int i = 0; i < textScore.Length; ++i )
        {
            char num = textScore[i];
            GameObject obj = numbers[textScore.Length - 1 - i];
            obj.GetComponent<MeshFilter>().mesh = numMeshes[(int)char.GetNumericValue(num)];
        }
    }
    public void AddLife(int life)
    {
        if (life < 0 || life >= lifes.Length) return;
        lifes[life].SetActive(true);
        GameObject particle = Instantiate(destroyParticle, lifes[life].transform.position, Quaternion.identity);
        Destroy(particle, 1.1f);
    }
    public void DestroyLife(int life)
    {
        if (life < 0 || life >= lifes.Length) return;
        GameObject particle = Instantiate(destroyParticle, lifes[life].transform.position, Quaternion.identity);
        Destroy(particle, 1.1f);
        lifes[life].SetActive(false);
    } 
}
