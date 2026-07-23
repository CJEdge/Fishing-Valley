using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class DialogueGraphView : GraphView {
		private string graphViewStyleSheetName = "USS/GraphView/GraphViewStyleSheet";
		private DialogueEditorWindow editorWindow;
		private NodeSearchWindow searchWindow;

		public DialogueGraphView(DialogueEditorWindow editorWindow) {
			this.editorWindow = editorWindow;
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			StyleSheet tmpStyleSheeet = Resources.Load<StyleSheet>(graphViewStyleSheetName);
			styleSheets.Add(tmpStyleSheeet);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new FreehandSelector());

			GridBackground grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddSearchWindow();
		}

		private void AddSearchWindow() {
			searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			searchWindow.Configure(editorWindow, this);
			nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
			List<Port> compatiblePorts = new List<Port>();
			Port startPortView = startPort;

			ports.ForEach((port) => {
				Port portView = port;
				if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction) {
					compatiblePorts.Add(port);
				}
			});

			return compatiblePorts;
		}

		public void ReloadLanguage() {
			List<BaseNode> allNodes = nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
			foreach (BaseNode node in allNodes) {
				node.ReloadLanguage();
			}
		}

		public StartNode CreateStartNode(Vector2 position) {
			return new StartNode(position, editorWindow, this);
		}

		public DialogueNode CreateNPCDialogueNode(Vector2 position, bool isNew = false) {
			var node = new DialogueNode(position, editorWindow, this);
			if (isNew) {
				node.TextLine();
				node.EventReferenceBox();
			}
			return node;
		}

		public ResponceNode CreateResponceNode(Vector2 position, bool isNew = false) {
			var node = new ResponceNode(position, editorWindow, this);
			if (isNew) {
				node.TextLine();
				node.TextLine();
			}
			return node;
		}

		public EventNode CreateNPCEventNode(Vector2 position, bool isNew = false) {
			var node = new EventNode(position, editorWindow, this);
			if (isNew) {
				node.TextLine();
			}
			return node;
		}
		public ConditionNode CreateNPCConditionNode(Vector2 position, bool isNew = false) {
			var node = new ConditionNode(position, editorWindow, this);
			if (isNew) {
				node.TextLine();
			}
			return node;
		}

		public EndNode CreateEndNode(Vector2 position) {
			return new EndNode(position, editorWindow, this);
		}

		public BranchNode CreateBranchNode(Vector2 position) {
			return new BranchNode(position, editorWindow, this);
		}
		public ListenNode CreateListenNode(Vector2 position) {
			return new ListenNode(position, editorWindow, this);
		}
	}
}
