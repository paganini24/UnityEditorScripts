﻿using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(TouchObjectControl))]
[CanEditMultipleObjects()]
public class TouchActionEditor : Editor {

	TouchObjectControl touchObjectControl;
	TouchAction touchAction;
	int toolBar;
	void OnEnable ()
	{
		touchObjectControl = target as TouchObjectControl;
		touchAction = touchObjectControl.touchAction;
		toolBar = EditorPrefs.GetInt("index",0);
	}
	void OnDisable()
	{
		EditorPrefs.SetInt("index",toolBar);
	}
	public override void OnInspectorGUI ()
	{
		checkMouse();
		touchObjectControl.worldMode = (TouchObjectControl.World)EditorGUILayout.EnumPopup("World mode",touchObjectControl.worldMode);
		if(touchObjectControl.worldMode == TouchObjectControl.World.World2D && !Camera.main.isOrthoGraphic)
		{
			EditorGUILayout.HelpBox("Your camera is not on ortographic!",MessageType.Warning);
		}
		EditorGUILayout.BeginHorizontal();
		touchObjectControl.safeClick = EditorGUILayout.Toggle("Safe Click",touchObjectControl.safeClick);
		if(touchObjectControl.safeClick )
		{
			touchObjectControl.safeClickMultiplier = EditorGUILayout.FloatField("Multiplier",touchObjectControl.safeClickMultiplier);
		}
		EditorGUILayout.EndHorizontal();

		// Can make property drawer below. But if you dont want to mess with Rect's it's easier to do like this.
		string[] menuOptions = new string[3];

		menuOptions[0] = "Drag";
		menuOptions[1] = "Rotate";
		menuOptions[2] = "Scale";
		toolBar = GUILayout.Toolbar(toolBar, menuOptions);

		EditorGUILayout.Space();
		switch(toolBar)
		{
		case 0:
			touchAction.drag = EditorGUILayout.Toggle("Enabled",touchAction.drag);
			if(touchAction.drag)
			{
				touchAction.dragAxis = (TouchAction.DragAxis)EditorGUILayout.EnumPopup("Axis",touchAction.dragAxis);
				EditorGUILayout.BeginHorizontal();
				touchAction.smoothDrag = EditorGUILayout.Toggle("Smooth Drag",touchAction.smoothDrag);
				if(touchAction.smoothDrag)
					touchAction.smoothFactor = EditorGUILayout.FloatField("Multiplier",touchAction.smoothFactor);
				EditorGUILayout.EndHorizontal();
				touchAction.onDragScale = EditorGUILayout.Vector3Field("Drag scale",touchAction.onDragScale);
				if((touchAction.onDragScale.x != 1f || touchAction.onDragScale.y != 1f) && touchObjectControl.worldMode == TouchObjectControl.World.World2D)
				{
					if(touchObjectControl.transform.childCount == 0)
					{
						EditorGUILayout.HelpBox("Add a duplicate of this object as a child for drag scale. Scaling collider2d may cause unwanted collision actions!",MessageType.Warning);
					}
				}
				touchAction.useWallHit = EditorGUILayout.BeginToggleGroup("Use wall hit",touchAction.useWallHit);
				touchAction.wallHitTreshold = EditorGUILayout.FloatField("Treshold",touchAction.wallHitTreshold);
				touchAction.layer = EditorGUILayout.LayerField("Layer",touchAction.layer);
				EditorGUILayout.EndToggleGroup();
			}

			break;
		case 1:
			touchAction.rotate = EditorGUILayout.Toggle("Enabled",touchAction.rotate);
			if(touchAction.rotate)
			{
				touchAction.rotateAxis = (TouchAction.RotateAxis)EditorGUILayout.EnumPopup("Axis",touchAction.rotateAxis);
				touchAction.rotateSpeed = EditorGUILayout.FloatField("Speed",touchAction.rotateSpeed);
				touchAction.rotateTreshold = EditorGUILayout.FloatField("Treshold",touchAction.rotateTreshold);
				EditorGUILayout.BeginHorizontal();
				touchAction.rotateOnTouch = EditorGUILayout.Toggle("Rotate on touch",touchAction.rotateOnTouch);
				if(touchAction.rotateOnTouch)
					touchAction.rotateTouchEndTime = EditorGUILayout.FloatField("Touch end time",touchAction.rotateTouchEndTime);
				EditorGUILayout.EndHorizontal();
			}
			break;
		case 2:
			touchAction.scale = EditorGUILayout.Toggle("Enabled",touchAction.scale);

			if(touchAction.scale)
			{
				if(touchObjectControl.worldMode == TouchObjectControl.World.World2D)
					EditorGUILayout.HelpBox("Do not scale collider2D may cause unwanted action on Collision",MessageType.Warning);
				touchAction.scaleAxis = (TouchAction.ScaleAxis)EditorGUILayout.EnumPopup("Axis",touchAction.scaleAxis);
				touchAction.scaleBetween = EditorGUILayout.Vector2Field("Scale between",touchAction.scaleBetween );
				EditorGUILayout.BeginHorizontal();
				touchAction.scaleTreshold = EditorGUILayout.FloatField("Scale Treshold",touchAction.scaleTreshold);
				touchAction.scaleMultiplier = EditorGUILayout.FloatField("Scale Multiplier",touchAction.scaleMultiplier);
				EditorGUILayout.EndHorizontal();
//				touchAction.touchOnScale = EditorGUILayout.Toggle("Scale on touch",touchAction.touchOnScale);
			}
			break;
		default:break;
		}

	}


	void checkMouse() {
		Event e = Event.current;
		if ((e.button == 0 && e.isMouse) ||(e.keyCode == KeyCode.KeypadEnter)) {
			Undo.RecordObject(touchObjectControl,"record");
		}
	}
}
