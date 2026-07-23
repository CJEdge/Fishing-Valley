using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ResponceData : BaseData
{
	public string FirstOptionGuid;
	public string SecondOptionGuid;

	public List<ResponceData_Text> ResponceData_Texts = new List<ResponceData_Text>();
}

[System.Serializable]
public class ResponceData_Text {
	public Container_Int ID = new Container_Int();
	public Container_String GuidID = new Container_String();
	public List<LanguageGeneric<string>> Text = new List<LanguageGeneric<string>>();

#if UNITY_EDITOR
	public TextField TextField { get; set; }
#endif
}
