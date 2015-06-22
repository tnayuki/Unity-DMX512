using UnityEditor;
using UnityEngine;

namespace DMX512
{
	[CustomEditor(typeof(DMX512Controller))]
	public class DMX512ControllerEditor : Editor
	{	
		private bool showData = false;
		private int page = 0;

		public override void OnInspectorGUI ()
		{
			DMX512Controller controller = (DMX512Controller)target;

			controller.address = EditorGUILayout.TextField("Address", controller.address);
			controller.port = EditorGUILayout.IntField("Port", controller.port);
			controller.universe = EditorGUILayout.IntField("Universe", controller.universe);
			controller.blackOut = EditorGUILayout.Toggle("Black Out", controller.blackOut);

			showData = EditorGUILayout.Foldout(showData, "Data");
			if (showData) {
				string[] pages = new string[8];
				for (int i = 0; i < pages.Length; i++) {
					pages [i] = string.Format ("{0}-{1}", i * 64 + 1, i * 64 + 64);
				}

				page = GUILayout.SelectionGrid (page, pages, 4);
					
				for (int i = 0; i < 2; i++) {
					for (int j = 0; j < 32; j++) {
						int channelNumber = page * 64 + i * 32 + j;
						byte num = controller.data [channelNumber];
						num = (byte)EditorGUILayout.IntSlider (num, 0, 255);
						controller.data [channelNumber] = num;
					}
				}
			}

			EditorUtility.SetDirty (target);
		}
	}
}
