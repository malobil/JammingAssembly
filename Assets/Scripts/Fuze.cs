using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fuze : MonoBehaviour
{
	public Image fuze;
	private float timeLeft = 1f;
	public float maxTime = 10f;
	public Transform objectToMove;
	public float fuzeSpeed;

	
    // Start is called before the first frame update
    void Start()
    {
		timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
		if (timeLeft > 0)
		{
			timeLeft -= Time.deltaTime;
            fuze.fillAmount = timeLeft / maxTime;
        }

        objectToMove.Translate(-1 * fuzeSpeed, 0,0);
	}
}
