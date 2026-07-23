using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class ResponceNode : BaseNode {

		private ResponceData responceData = new ResponceData();
		public ResponceData ResponceData {
			get => responceData;
			set => responceData = value;
		}

		public ResponceNode() {

		}

		public ResponceNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) {
			base.editorWindow = editorWindow;
			base.graphView = graphView;

			StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/BranchNodeStyleSheet");
			styleSheets.Add(styleSheet);

			title = "Responce";
			SetPosition(new Rect(position, defaultNodeSize));
			nodeGuid = Guid.NewGuid().ToString();

			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("Option 1", Port.Capacity.Single);
			AddOutputPort("Option 2", Port.Capacity.Single);
		}

		public void TextLine(ResponceData_Text data_Text = null) {
			ResponceData_Text newDialogueBaseContainer_Text = new ResponceData_Text();
			ResponceData.ResponceData_Texts.Add(newDialogueBaseContainer_Text);

			// Add Container Box
			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");

			// Add Fields
			AddTextField(newDialogueBaseContainer_Text, boxContainer);

			// Load in data if it got any
			if (data_Text != null) {
				// Guid ID
				newDialogueBaseContainer_Text.GuidID = data_Text.GuidID;

				// Text
				foreach (LanguageGeneric<string> data_text in data_Text.Text) {
					foreach (LanguageGeneric<string> text in newDialogueBaseContainer_Text.Text) {
						if (text.LanguageType == data_text.LanguageType) {
							text.LanguageGenericType = data_text.LanguageGenericType;
						}
					}
				}

			} else {
				// Make New Guid ID
				newDialogueBaseContainer_Text.GuidID.Value = Guid.NewGuid().ToString();
			}

			// Reaload the current selected language
			ReloadLanguage();

			mainContainer.Add(boxContainer);
		}

		private void AddTextField(ResponceData_Text container, Box boxContainer) {
			TextField textField = GetNewTextField_TextLanguage(container.Text, "Text area", "TextBox");
			container.TextField = textField;
			boxContainer.Add(textField);
		}

		public override void ReloadLanguage() {
			base.ReloadLanguage();
		}
	}
}
