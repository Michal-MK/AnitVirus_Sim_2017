﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

	private static string profilesFolder = Application.persistentDataPath + Path.DirectorySeparatorChar + "Profiles" + Path.DirectorySeparatorChar;
	private static Profile_Data _currProfile;

	public static GameObject profileRepresenation;
	public static GameObject authentication;

	public static Profile_Data getCurrentProfile {
		get {
			return _currProfile;
		}
	}

	public static string profileName {
		get {
			if (_currProfile != null) {
				return _currProfile.getProfileName;
			}
			else {
				return "Not selected!";
			}
		}
	}

	private static Profile_Data[] createdProfiles {
		get {
			DirectoryInfo d = new DirectoryInfo(profilesFolder);
			BinaryFormatter bf = new BinaryFormatter();
			FileInfo[] profilesInFolder = d.GetFiles("*.gp");
			Profile_Data[] allProfiles = new Profile_Data[profilesInFolder.Length];

			for (int i = 0; i < profilesInFolder.Length; i++) {
				using (FileStream fs = profilesInFolder[i].OpenRead()) {
					Profile_Data pd = (Profile_Data)bf.Deserialize(fs);
					allProfiles[i] = pd;
				}
			}
			return allProfiles;
		}
	}

	public Profile_Data Create(string profileName) {
		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream fs = File.Create(profilesFolder + profileName + ".gp")) {
			Profile_Data data = new Profile_Data(profileName);
			bf.Serialize(fs, data);
			_currProfile = data;
			return data;
		}
	}

	public static Profile_Data SelectProfile(string p_name) {
		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream fs = File.OpenRead(profilesFolder + p_name)) {
			Profile_Data data = (Profile_Data)bf.Deserialize(fs);
			_currProfile = data;
			return data;
		}
	}

	public static void RequestProfiles() {
		profileRepresenation = (GameObject)Resources.Load("Profiles");
		authentication = (GameObject)Resources.Load("Authentication");
		Text profile_name = GameObject.Find("Profile_Name").GetComponent<Text>();
		MainMenu_Holder holder = FindObjectOfType<MainMenu_Holder>();

		Profile_Data[] pfs = createdProfiles;
		if (pfs.Length == 0) {
			GameObject auth = Instantiate(authentication, GameObject.Find("Canvas").transform);
			InputField in_field = auth.transform.Find("InputField").GetComponent<InputField>();
			in_field.onValidateInput += delegate (string input, int charIndex, char addedChar) { return Validate(addedChar); };
			in_field.onEndEdit.AddListener(delegate {
				Control.currProfile = new Profile().Create(in_field.text);
				Destroy(auth);
				profile_name.text = "Current profile: " + in_field.text;
			});
		}
		else {

			ProfileRepresentation_Holder g = Instantiate(profileRepresenation, GameObject.Find("Canvas").transform).GetComponent<ProfileRepresentation_Holder>();

			for (int i = 0; i < pfs.Length; i++) {
				g.profileNames[i].text = pfs[i].getProfileName;

				if (!string.IsNullOrEmpty(pfs[i].getProfileName)) {
					EventSystem.current.SetSelectedGameObject(g.buttons[i].gameObject);
				}

				g.buttons[i].onClick.AddListener(delegate {
					Destroy(g.gameObject);
					profile_name.text = "Current profile: " + g.profileNames[i - 1].text;
					Control.currProfile = SelectProfile(g.profileNames[i - 1].text + ".gp");
					holder.startGame.interactable = true;
					EventSystem.current.SetSelectedGameObject(holder.startGame.gameObject);
					foreach (Button b in holder.getButtons) {
						b.interactable = true;
					}
				});
			}

			foreach (Button b in g.buttons) {
				if (b.transform.GetChild(0).GetComponent<Text>().text == "Button") {
					Destroy(b.gameObject);
				}
			}

			DisableMenuInteraction(holder);
		}
	}

	public static char Validate(char ch) {
		if (ch == '$' || ch == '~' || ch == '@' || ch == '_' || ch == '#') {
			ch = '\0';
		}
		return ch;
	}

	private static void DisableMenuInteraction(MainMenu_Holder holder) {
		holder.startGame.interactable = false;
		foreach (Button b in holder.getButtons) {
			b.interactable = false;
		}
	}
}

[System.Serializable]
public class Profile_Data {
	private string _profileName = "";

	public Profile_Data(string name) {
		_profileName = name;
	}

	public string getProfileName {
		get { return _profileName; }
	}
}
