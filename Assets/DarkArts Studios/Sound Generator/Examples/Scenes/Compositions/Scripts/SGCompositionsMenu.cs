using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SGCompositionsMenu : MonoBehaviour {

	private float menuBorderWidth = 10f;
	private float menuBorderHeight = 10f;

	private int columns = 3;
	private int compositionsLoaded = 0;
	private bool allCompositionsLoaded = false;
	private List<DarkArtsStudios.SoundGenerator.Composition> compositions = new List<DarkArtsStudios.SoundGenerator.Composition>();

	public GameObject compositionHolder = null;

	private AudioSource audioSource = null;

	void Awake()
	{
		compositions.Clear();
		foreach( Transform transform in compositionHolder.transform)
		{
			compositions.Add( transform.gameObject.GetComponent<DarkArtsStudios.SoundGenerator.Composition>() );
		}
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	IEnumerator Start () {
		allCompositionsLoaded = false;
		while (allCompositionsLoaded == false)
		{
			compositionsLoaded = 0;
			foreach( DarkArtsStudios.SoundGenerator.Composition composition in compositions)
			{
				if (composition.audioClips.Count == composition.modules.FindAll(item => item.GetType() == typeof(DarkArtsStudios.SoundGenerator.Module.Output)).Count )
					compositionsLoaded ++;
			}
			allCompositionsLoaded = compositionsLoaded == compositions.Count;
			if (!allCompositionsLoaded)
				yield return null;
		}

	}

	private Vector2 menuScrollPosition = new Vector2();

	void OnGUI() {
		GUILayout.BeginArea( new Rect( menuBorderWidth, menuBorderHeight, Screen.width - menuBorderWidth*2, Screen.height - menuBorderHeight*2 ) );
		{
			GUILayout.BeginVertical();
			{
				GUILayout.Label("Sound Generator Composition Examples");
				if (allCompositionsLoaded)
				{
					menuScrollPosition = GUILayout.BeginScrollView( menuScrollPosition );
					{
						GUILayout.BeginHorizontal();
						{
							int perColumn = (compositions.Count+columns-1)/columns;
							int compositionIndex = 0;
							for (int column = 0; column < columns; column++)
							{
								GUILayout.BeginVertical();
								{
									for (int perColumnIndex = 0; compositionIndex < compositions.Count && perColumnIndex < perColumn; perColumnIndex++ )
									{
										if (GUILayout.Button(compositions[compositionIndex].name) )
										{
											audioSource.PlayOneShot( compositions[compositionIndex].audioClips["Output"] );
										}
										compositionIndex++;
									}
									GUILayout.FlexibleSpace();
								}
								GUILayout.EndVertical();
							}
						}
						GUILayout.EndHorizontal();
					}
					GUILayout.EndScrollView();
				}
				else
				{
					GUILayout.Box( "Loading Compositions: " + (compositionsLoaded*100.0f/compositions.Count) + "%" );
					Rect priorRect = GUILayoutUtility.GetLastRect();
					priorRect.width = priorRect.width * compositionsLoaded / compositions.Count;
					GUI.Box( priorRect, "" );
				}
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
	
}
