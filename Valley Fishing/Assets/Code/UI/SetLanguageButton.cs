using UnityEngine;

public class SetLanguageButton : MonoBehaviour
{
	#region Public Methods

	public void SetLanguage(int languageIndex) {
		PlayerPrefsManager.Save(PlayerPrefsManager.Language, languageIndex);
	}

	#endregion
}
