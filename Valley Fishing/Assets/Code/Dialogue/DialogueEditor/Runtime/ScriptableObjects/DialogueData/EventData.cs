using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EventData : BaseData {
	public EventType EventType;
	public EventData_EventName EventData_EventName;

	public EventData() {
		EventData_EventName = new EventData_EventName();
		EventType = EventType.None;
	}
}
