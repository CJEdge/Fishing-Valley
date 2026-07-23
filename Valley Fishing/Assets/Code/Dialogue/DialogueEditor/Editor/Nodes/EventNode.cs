using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class EventNode : BaseNode {

		EventData eventData = new EventData();

		public EventData NPCEventData {
			get => eventData;
			set => eventData = value;
		}

		public EventNode() {

		}

		private EnumField eventTypeDropdown;

		public EventNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView) {
			base.editorWindow = _editorWindow;
			base.graphView = _graphView;

			StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/EventNodeStyleSheet");
			styleSheets.Add(styleSheet);

			title = "NPC Event";
			SetPosition(new Rect(_position, defaultNodeSize));
			nodeGuid = Guid.NewGuid().ToString();
			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("Output", Port.Capacity.Single);
		}

		public void TextLine(EventData_EventName data_Text = null, EventType type = EventType.None) {
			if (data_Text != null)
				NPCEventData.EventData_EventName = data_Text;
			else if (NPCEventData.EventData_EventName == null)
				NPCEventData.EventData_EventName = new EventData_EventName();

			NPCEventData.EventType = type;

			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");

			eventTypeDropdown = new EnumField("Event Type", NPCEventData.EventType);
			eventTypeDropdown.RegisterValueChangedCallback(evt => {
				NPCEventData.EventType = (EventType)evt.newValue;
			});
			boxContainer.Add(eventTypeDropdown);

			AddTextField(NPCEventData.EventData_EventName, boxContainer);

			mainContainer.Add(boxContainer);
		}

		private void AddTextField(EventData_EventName container, Box boxContainer) {
			TextField textField = GetNewTextField(container.StringEventValue, "Text area", "TextBox");
			boxContainer.Add(textField);
		}
	}
}
