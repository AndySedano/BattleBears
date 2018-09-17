using System.Collections.Generic;
using UnityEngine;

public class Outpost : MonoBehaviour
{

	public static List<Outpost> OutpostList = new List<Outpost>();

    [SerializeField]
    private float _CaptureTime = 5f;

    internal float CaptureValue = 0f;
    internal int CurrentTeam = 0;

    private SkinnedMeshRenderer _Renderer;

    private void Awake()
    {
        _Renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		OutpostList.Add(this);
    }

    private void Update()
    {
		Color teamColor = GameManager.Instance.TeamColors[CurrentTeam];
		_Renderer.material.color = Color.Lerp(Color.white, teamColor, CaptureValue);
	}

    private void OnTriggerStay(Collider other)
    {
        Unit u = other.GetComponent<Unit>();
        if (u == null)
        {
            return;
        }

        if (u.TeamNumber == CurrentTeam)
        {
            CaptureValue += Time.deltaTime / _CaptureTime;

            if (CaptureValue > 1f)
            {
                CaptureValue = 1f;
            }
        }
        else
        {
            CaptureValue -= Time.deltaTime / _CaptureTime;
            if (CaptureValue <= 0f)
            {
                CaptureValue = 0;
                CurrentTeam = u.TeamNumber;
            }
        }
    }
}
