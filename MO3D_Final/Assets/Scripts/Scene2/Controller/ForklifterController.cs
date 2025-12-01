using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForklifterController : MonoBehaviour
{
    [SerializeField] private GameObject lifter;
    [SerializeField] private float speedLifter;
    [SerializeField] private float maxPosition;
    [SerializeField] private float minPosition;

    private bool isLiftUp;
    private bool isLiftDown;

	private void Update()
	{
		GetInputLifter();
		MoveLifter();
	}

	private void GetInputLifter()
    {
        if (Input.GetKey(KeyCode.E))
        {
            isLiftUp = true;
            isLiftDown = false;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            isLiftUp = false;
            isLiftDown = true;
        }
        else
        {
            isLiftUp = false;
            isLiftDown = false;
        }
    }

    private void MoveLifter()
    {
        Vector3 newPosition = lifter.transform.localPosition;

        if (isLiftUp)
        {
            newPosition.y += speedLifter * Time.deltaTime;
        }
        else if (isLiftDown)
        {
            newPosition.y -= speedLifter * Time.deltaTime;
		}

        // LIMITES
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition, maxPosition);
        lifter.transform.localPosition = newPosition;
    }
}
