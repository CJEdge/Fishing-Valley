using Project.DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ListenNode : BaseNode {

	ListenData listenData = new ListenData();

	public ListenData ListenData {
		get => listenData;
		set => listenData = value;
	}

	public ListenNode() {

	}

	public ListenNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView) {
		base.editorWindow = _editorWindow;
		base.graphView = _graphView;

		StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/EventNodeStyleSheet");
		styleSheets.Add(styleSheet);

		title = "ListenNode";
		SetPosition(new Rect(_position, defaultNodeSize));
		nodeGuid = Guid.NewGuid().ToString();
		AddInputPort("Input", Port.Capacity.Multi);
		AddOutputPort("Continue");
		AddScriptableEvent();
	}

	public void AddScriptableEvent(Container_ListenEventSO paramidaEventScriptableObjectData = null) {

		// Try to reuse an existing empty box before creating a new one
		Box existingEmptyBox = FindEmptyEventBox();

		if (paramidaEventScriptableObjectData != null && existingEmptyBox != null) {
			ObjectField existingObjectField = existingEmptyBox.Q<ObjectField>();
			Container_ListenEventSO existingData = existingObjectField?.userData as Container_ListenEventSO;

			if (existingObjectField != null && existingData != null) {
				existingData.ListenEventSO = paramidaEventScriptableObjectData.ListenEventSO;
				existingObjectField.SetValueWithoutNotify(existingData.ListenEventSO);
				return;
			}
		}

		// No reusable box found — create a new one as before
		Container_ListenEventSO tmpListenEventSO = new Container_ListenEventSO();

		if (paramidaEventScriptableObjectData != null) {
			tmpListenEventSO.ListenEventSO = paramidaEventScriptableObjectData.ListenEventSO;
		}
		listenData.Container_ListenEventSOs.Add(tmpListenEventSO);

		// Container of all object.
		Box boxContainer = new Box();
		boxContainer.AddToClassList("EventBox");

		// Scriptable Object Event.
		ObjectField objectField = GetNewObjectField_ListenEvent(tmpListenEventSO, "EventObject");
		objectField.userData = tmpListenEventSO; // tag so we can find/update it later

		// Add it to the box
		boxContainer.Add(objectField);

		mainContainer.Add(boxContainer);
		RefreshExpandedState();
	}

	private Box FindEmptyEventBox() {
		foreach (VisualElement child in mainContainer.Children()) {
			if (child is Box box && box.ClassListContains("EventBox")) {
				ObjectField field = box.Q<ObjectField>();
				if (field != null && field.value == null) {
					return box;
				}
			}
		}
		return null;
	}
}
