using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Honbul
{
    public static class SceneBuilderMenu
    {
        [MenuItem("혼/Build Scenes")]
        public static void BuildScenes()
        {
            try
            {
                string scenesDirectory = "Assets/Scenes";
                if (!Directory.Exists(scenesDirectory))
                {
                    Directory.CreateDirectory(scenesDirectory);
                }

                string[] sceneNames =
                {
                    GameConfig.IntroSceneName,
                    GameConfig.GameSceneName,
                    GameConfig.EndingSceneName
                };

                List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();

                for (int i = 0; i < sceneNames.Length; i++)
                {
                    string sceneName = sceneNames[i];
                    var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                    GameObject bootstrap = new GameObject("(Bootstrap)");
                    bootstrap.AddComponent<GameBootstrapper>();

                    string scenePath = scenesDirectory + "/" + sceneName + ".unity";
                    bool saved = EditorSceneManager.SaveScene(scene, scenePath);
                    if (!saved)
                    {
                        throw new Exception("Scene save failed: " + scenePath);
                    }

                    buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                }

                EditorBuildSettings.scenes = buildScenes.ToArray();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("혼/Build Scenes 완료: Intro, Game, Ending 씬을 생성하고 Build Settings에 등록했습니다.");
            }
            catch (Exception ex)
            {
                Debug.LogError("혼/Build Scenes 실패: " + ex);
            }
        }

        [MenuItem("혼/Reset Build Settings")]
        public static void ResetBuildSettings()
        {
            try
            {
                EditorBuildSettings.scenes = Array.Empty<EditorBuildSettingsScene>();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("혼/Reset Build Settings 완료: Build Settings를 초기화했습니다.");
            }
            catch (Exception ex)
            {
                Debug.LogError("혼/Reset Build Settings 실패: " + ex);
            }
        }
    }
}
