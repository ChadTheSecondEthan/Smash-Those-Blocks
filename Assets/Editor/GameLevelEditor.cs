using UnityEditor;

[CustomEditor(typeof(GameLevel))]
public class GameLevelEditor : Editor
{
    bool drawOnLevelBegin = false;
    bool drawOnLevelEnd = false;
    bool drawOnCutSceneEnd = false;
    //bool allowFusion = true;

    public override void OnInspectorGUI()
    {
        GameLevel gameLevel = target as GameLevel;
        SerializedObject sGameLevel = new SerializedObject(gameLevel);

        if (drawOnLevelBegin = EditorGUILayout.Toggle("Draw On Level Begin", drawOnLevelBegin))
        {
            SerializedProperty sp = sGameLevel.FindProperty("OnLevelBegin");
            EditorGUILayout.PropertyField(sp);
            sGameLevel.ApplyModifiedProperties();
        }

        if (drawOnLevelEnd = EditorGUILayout.Toggle("Draw On Level End", drawOnLevelEnd))
        {
            SerializedProperty sp = sGameLevel.FindProperty("OnLevelEnd");
            EditorGUILayout.PropertyField(sp);
            sGameLevel.ApplyModifiedProperties();
        }

        if (drawOnCutSceneEnd = EditorGUILayout.Toggle("Draw On Cut Scene End", drawOnCutSceneEnd))
        {
            SerializedProperty sp = sGameLevel.FindProperty("OnCutSceneEnd");
            EditorGUILayout.PropertyField(sp);
            sGameLevel.ApplyModifiedProperties();
        }

        /*allowFusion = EditorGUILayout.Toggle("Allow Fusion", allowFusion);
        if (allowFusion)
        {
            SerializedProperty sp = sGameLevel.FindProperty("fusionsAllowed");
            EditorGUILayout.PropertyField(sp);
            sGameLevel.ApplyModifiedProperties();
        }
        else
            gameLevel.fusionsAllowed = 0;*/
    }
}
