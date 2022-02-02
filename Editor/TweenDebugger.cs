/*
  NoZ Unity Library

  Copyright(c) 2022 NoZ Games, LLC

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files(the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions :

  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NoZ.Tweening.Internals;

#if UNITY_EDITOR
namespace NoZ.Tweening.Editor
{
    // TODO: number of active tweens
    // TODO: number of free tweens
    // TODO: list of collapsed tweens
    // TODO: give this assembly access to internals
    // TODO: Ability to pause/resume/stop any given tween
    // TODO: Filter section (selected only, identifier, type (group, sequence, etc), paused, playing)
    // TODO: Auto-pause all tweens that are started

    public class TweenDebugger : EditorWindow
    {
        private double lastRepaint = 0.0f;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/Analysis/Tween Debugger")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            TweenDebugger window = (TweenDebugger)EditorWindow.GetWindow(typeof(TweenDebugger));
            window.titleContent = new GUIContent("Tween Debugger");
            window.Show();
        }

        private void Update()
        {
            if(Time.realtimeSinceStartupAsDouble - lastRepaint > 1.0f)
                Repaint();
        }

        void OnGUI()
        {
            lastRepaint = Time.realtimeSinceStartupAsDouble;

#if true
            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Counts");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Created", TweenContext.GetStateCount(TweenContext.State.Created).ToString());
            EditorGUILayout.LabelField("Playing", TweenContext.GetStateCount(TweenContext.State.Playing).ToString());
            EditorGUILayout.LabelField("Paused", TweenContext.GetStateCount(TweenContext.State.Paused).ToString());
            EditorGUILayout.LabelField("Manual", TweenContext.GetStateCount(TweenContext.State.Manual).ToString());
            EditorGUILayout.LabelField("Element", TweenContext.GetStateCount(TweenContext.State.Element).ToString());
            EditorGUILayout.LabelField("Free", TweenContext.GetStateCount(TweenContext.State.Free).ToString());
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Options");
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Filters");
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.BeginFoldoutHeaderGroup(true, "Tweens");

#if false
            EditorGUI.indentLevel++;
            for (var node = Tween._activeContexts.First; node != null; node = node.Next)
            {
                EditorGUILayout.LabelField($"{node.Value.target.GetType().ToString()}", node.Value.instanceId.ToString());
            }
            EditorGUI.indentLevel--;
#endif
            EditorGUILayout.EndFoldoutHeaderGroup();
#endif
        }
    }
}
#endif
