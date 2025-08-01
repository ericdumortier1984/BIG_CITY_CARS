using System.Collections.Generic; 
using UnityEngine; 

public class IntersectionZone : MonoBehaviour
{
	[SerializeField] TrafficLightsController mTrafficLightController;

	private Queue<IAController> mCarInside = new Queue<IAController>(); // FIFO (primero en entrar, primero en salir)
    public bool IsCarInside = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("IACar"))
		{
			IAController mIACar = other.GetComponent<IAController>();
			if (mIACar != null 
				&& !mCarInside.Contains(mIACar))
			{
				mCarInside.Enqueue(mIACar);
				ProcessCarQueue();
			}
		}	
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("IACar"))
		{
			IAController mIACar = other.GetComponent<IAController>();
			if (mIACar != null
				&& mIACar == mCarInside.Peek()) // Si es el primer auto en la cola
			{
				mCarInside.Dequeue();
				IsCarInside = false;
				ProcessCarQueue();
			}
		}
	}
	
	private void ProcessCarQueue() 
	{
		if (!IsCarInside 
			&& mCarInside.Count > 0)
		{
			IAController mIACar = mCarInside.Peek();
			mIACar.GoAhead();
			IsCarInside = true;
		}
	}

	public bool IsRedLight()
	{
		return mTrafficLightController != null && mTrafficLightController.IsRed();
	}

	public bool IsGreenLight()
	{
		return mTrafficLightController != null && !mTrafficLightController.IsRed();
	}
}
