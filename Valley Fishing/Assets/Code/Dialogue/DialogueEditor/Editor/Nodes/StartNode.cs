using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class StartNode : BaseNode {
		public StartNode() {

		}

		public StartNode(Vector2 _position, DialogueEditorWindow _editorWindow, DialogueGraphView _graphView) {
			base.editorWindow = _editorWindow;
			base.graphView = _graphView;

			StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/StartNodeStyleSheet");
			styleSheets.Add(styleSheet);

			title = "Start";
			SetPosition(new Rect(_position, defaultNodeSize));
			nodeGuid = Guid.NewGuid().ToString();

			AddOutputPort("Output", Port.Capacity.Single);

			RefreshExpandedState();
			RefreshPorts();
		}

	}
}
