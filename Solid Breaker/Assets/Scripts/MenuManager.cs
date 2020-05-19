using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject play;
    public GameObject exit;
    void Update()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position); 
                if (Physics.Raycast(ray,out hit, 400))
                {
                    if (hit.collider.gameObject == play)
                    {
                        SceneManager.LoadScene(1);
                    }
                    else if (hit.collider.gameObject == exit)
                    {
                        Application.Quit();
                    }
                }
                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 400))
            {
                if (hit.collider.gameObject == play)
                {
                    SceneManager.LoadScene(1);
                }
                else if (hit.collider.gameObject == exit)
                {
                    Application.Quit();
                }
            }
        }

    }
}
