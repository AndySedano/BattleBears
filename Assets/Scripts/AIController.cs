using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AIController : Unit
{

    #region GlobalVariables

    private NavMeshAgent _Agent;
    private Animator _Anim;

    private Coroutine _CurrentState = null;
    private Outpost _TargetOutpost = null;

    #endregion GlobalVariables


    #region UnityFunctions

    private new void Awake()
    {
        base.Awake();
        _Anim = GetComponent<Animator>();
        _RB.isKinematic = true;
        _RB.useGravity = false;
        _Agent = GetComponent<NavMeshAgent>();

        Color teamColor = GameManager.Instance.TeamColors[TeamNumber];
        transform.Find("Teddy_Body").GetComponent<Renderer>().material.color = teamColor;
    }

    private void Start()
    {
        SetState(State_Idle());
    }

    // private void Update()
    // {
    //     _Anim.SetFloat("VerticalMovement", _Agent.velocity.normalized.magnitude);
    //     _Anim.SetFloat("Ver", _Agent.velocity.normalized.magnitude);
    // }

    #endregion UnityFunctions


    #region StateMachine

    private void SetState(IEnumerator newState)
    {
        if (_CurrentState != null)
        {
            StopCoroutine(_CurrentState);
        }

        _CurrentState = StartCoroutine(newState);
    }

    private IEnumerator State_Idle()
    {
        // State Enter
        _Anim.SetFloat("VerticalMovement", 0f);

        while (_TargetOutpost == null)
        {
            LookForOutpost();
            yield return null;
        }

        // State Exit 
        SetState(State_MovingToOutpost());
    }


    private IEnumerator State_MovingToOutpost()
    {
        _Anim.SetFloat("VerticalMovement", 1f);
        _Agent.SetDestination(_TargetOutpost.transform.position);

        while (_Agent.remainingDistance > _Agent.remainingDistance)
        {
            yield return null;
        }

        SetState(State_CapturingOutpost());
    }

    private IEnumerator State_CapturingOutpost()
    {
        _Anim.SetFloat("VerticalMovement", 0f);

        while (_TargetOutpost.CurrentTeam != TeamNumber || _TargetOutpost.CaptureValue < 1f)
        {
            yield return null;
        }

        _TargetOutpost = null;
        SetState(State_Idle());
    }


    #endregion StateMachine


    #region ClassFunctions

    private void LookForOutpost()
    {
        var tmp = Outpost.OutpostList[Random.Range(0, Outpost.OutpostList.Count)];
        if (tmp.CaptureValue < 1f || tmp.CurrentTeam != TeamNumber)
        {
            _TargetOutpost = tmp;
        }
    }

    #endregion ClassFunctions
    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(1))
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;
    //         if (Physics.Raycast(ray, out hit))
    //         {
    // 			_Agent.SetDestination(hit.point);
    // 			print("Hello");
    //         }
    //     }
    // }
}
