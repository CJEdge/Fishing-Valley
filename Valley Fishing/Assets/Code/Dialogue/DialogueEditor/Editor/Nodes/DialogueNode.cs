using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project.DialogueEditor {
	public class DialogueNode : BaseNode {
		private NPCDialogueData dialogueData = new NPCDialogueData();

		public NPCDialogueData DialogueData {
			get => dialogueData;
			set => dialogueData = value;
		}
		private EventReferenceHolder eventReferenceHolder;
		private class EventReferenceHolder : ScriptableObject {
			public EventReference VoiceEvent;
		}
		public DialogueNode() {

		}

		public DialogueNode(Vector2 _position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) {
			base.editorWindow = editorWindow;
			base.graphView = graphView;

			StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/DialogueNodeStyleSheet");
			styleSheets.Add(styleSheet);

			title = "NPC Dialogue";
			SetPosition(new Rect(_position, defaultNodeSize));
			nodeGuid = Guid.NewGuid().ToString();

			AddInputPort("Input", Port.Capacity.Multi);
			AddOutputPort("Continue");
		}


		public void EventReferenceBox() {
			Box boxContainer = new Box();
			boxContainer.AddToClassList("EventReferenceBox");

			Label label = new Label("Voice Event");
			boxContainer.Add(label);

			eventReferenceHolder = ScriptableObject.CreateInstance<EventReferenceHolder>();
			eventReferenceHolder.VoiceEvent = DialogueData.VoiceEvent;

			SerializedObject serializedObject = new SerializedObject(eventReferenceHolder);

			SerializedProperty property = serializedObject.FindProperty("VoiceEvent");

			PropertyField propertyField = new PropertyField(property);
			propertyField.label = "";

			propertyField.Bind(serializedObject);

			propertyField.TrackPropertyValue(property, changedProperty =>
			{
				serializedObject.ApplyModifiedProperties();

				DialogueData.VoiceEvent = eventReferenceHolder.VoiceEvent;

				Debug.Log($"Saved FMOD Event: {DialogueData.VoiceEvent.Path}");
			});

			boxContainer.Add(propertyField);
			mainContainer.Add(boxContainer);
		}


		private void UpdateEventButtonText(Button button) {
			if (DialogueData.VoiceEvent.IsNull) {
				button.text = "None";
			}
			else {
				button.text = DialogueData.VoiceEvent.Path;
			}
		}


		public void TextLine(DialogueTextData data_Text = null) {
			DialogueTextData newDialogueText = new DialogueTextData();
			DialogueData.DialogueText = newDialogueText;


			Box boxContainer = new Box();
			boxContainer.AddToClassList("DialogueBox");


			AddTextField(newDialogueText, boxContainer);


			if (data_Text != null) {
				newDialogueText.GuidID = data_Text.GuidID;

				foreach (LanguageGeneric<string> incomingText in data_Text.Text) {
					foreach (LanguageGeneric<string> existingText in newDialogueText.Text) {
						if (existingText.LanguageType == incomingText.LanguageType) {
							existingText.LanguageGenericType = incomingText.LanguageGenericType;
						}
					}
				}
			}
			else {
				newDialogueText.GuidID.Value = Guid.NewGuid().ToString();
			}


			ReloadLanguage();

			mainContainer.Add(boxContainer);
		}


		private void AddTextField(DialogueTextData container, Box boxContainer) {
			TextField textField = GetNewTextField_TextLanguage(
				container.Text,
				"Text area",
				"TextBox"
			);

			boxContainer.Add(textField);
		}


		public override void ReloadLanguage() {
			base.ReloadLanguage();
		}


		public override void LoadValueInToField() {

		}
	}
}
