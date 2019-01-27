using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedWakeUpZone : MonoBehaviour
{
	[SerializeField]
	private GameObject wakeUpZone;
	public GameObject WakeUpZone { get { return wakeUpZone; } }

	[SerializeField]
	private GameObject aButtonHelp;
	public GameObject AButtonHelp { get { return aButtonHelp; } }


	private void Start()
	{
		Timer.Instance.OnShowBedHelp += () => { aButtonHelp.SetActive(true); };
		Timer.Instance.DaronneIntervention += () => { aButtonHelp.SetActive(false); };
	}
}