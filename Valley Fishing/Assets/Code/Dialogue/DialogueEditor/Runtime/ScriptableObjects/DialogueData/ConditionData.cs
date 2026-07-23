using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionData : BaseData
{
	public string trueGuidNode;
	public string falseGuidNode;
	public EventData_EventName EventData_EventName = new EventData_EventName();
}
