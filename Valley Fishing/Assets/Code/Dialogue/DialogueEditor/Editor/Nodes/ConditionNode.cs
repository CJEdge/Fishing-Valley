using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class ConditionNode : BaseNode {

		ConditionData nPCConditionData = new ConditionData();

		public ConditionData NPCConditionData {
			get => nPCConditionData;
			set => nPCConditionData = value;
		}

		public ConditionNode() {

		}

		public ConditionNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView) {
			base.editorWindow = _editorWindow;
			base.graphView = _graphView;

			StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/EventNodeStyleSheet");
			styleSheets.Add(styleSheet);

			title = "NPC Condition";
			SetPosition(new Rect(_position, defaultNodeSize));
			nodeGuid = Guid.NewGuid().ToString();
			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("True", Port.Capacity.Single);
			AddOutputPort("False", Port.Capacity.Single);
		}

		public void TextLine(EventData_EventName data_Text = null) {
			if (data_Text != null)
				NPCConditionData.EventData_EventName = data_Text;
			else if (NPCConditionData.EventData_EventName == null)
				NPCConditionData.EventData_EventName = new EventData_EventName();

			// Add container box
			Box boxContainer = new Box();
			boxContainer.AddToClassList("EventName");

			// Add and set up the text field
			AddTextField(NPCConditionData.EventData_EventName, boxContainer);

			mainContainer.Add(boxContainer);
		}

		private void AddTextField(EventData_EventName container, Box boxContainer) {
			TextField textField = GetNewTextField(container.StringEventValue, "Text area", "TextBox");
			boxContainer.Add(textField);
		}
	}
}
