using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{

	public int TeamNumber = 0;

	protected Rigidbody _RB;

	protected void Awake()
	{
		_RB = GetComponent<Rigidbody>();
	}

}
