// Assets/Editor/ColorShiftTool.cs
using UnityEngine;
using UnityEditor;
using System.IO;

public class ColorShiftTool : EditorWindow
{
    private Texture2D sourceTexture;
    private Texture2D previewTexture;
    private Vector2 scrollPos;

    private float sourceHueMin = 0f;
    private float sourceHueMax = 0.08f;
    private float targetHue = 0.61f;
    private float saturationBoost = 0f;

    private readonly string[] presetNames = { "Red", "Orange", "Yellow", "Yellow-Green", "Green", "Cyan", "Blue", "Purple" };
    private readonly float[] presetHues = { 0f, 0.08f, 0.17f, 0.22f, 0.33f, 0.5f, 0.63f, 0.78f };
    private int sourcePresetIndex = 0;
    private int targetPresetIndex = 5;

    [MenuItem("Tools/Color Shift Tool")]
    public static void ShowWindow()
    {
        GetWindow<ColorShiftTool>("Color Shift Tool");
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Color Shift Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        sourceTexture = (Texture2D)EditorGUILayout.ObjectField(
            "원본 이미지", sourceTexture, typeof(Texture2D), false);

        EditorGUILayout.Space();

        // ── 소스 Hue 범위 ──
        GUILayout.Label("변환할 색상 범위 (Source)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("프리셋", GUILayout.Width(50));
        int newSourcePreset = EditorGUILayout.Popup(sourcePresetIndex, presetNames);
        if (newSourcePreset != sourcePresetIndex)
        {
            sourcePresetIndex = newSourcePreset;
            float center = presetHues[sourcePresetIndex];
            sourceHueMin = Mathf.Max(0f, center - 0.06f);
            sourceHueMax = Mathf.Min(1f, center + 0.06f);
        }
        EditorGUILayout.EndHorizontal();

        sourceHueMin = EditorGUILayout.Slider("Hue Min", sourceHueMin, 0f, 1f);
        sourceHueMax = EditorGUILayout.Slider("Hue Max", sourceHueMax, 0f, 1f);

        DrawHueBar("소스 범위", sourceHueMin, sourceHueMax);

        EditorGUILayout.Space();

        // ── 타겟 Hue ──
        GUILayout.Label("바꿀 색상 (Target)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("프리셋", GUILayout.Width(50));
        int newTargetPreset = EditorGUILayout.Popup(targetPresetIndex, presetNames);
        if (newTargetPreset != targetPresetIndex)
        {
            targetPresetIndex = newTargetPreset;
            targetHue = presetHues[targetPresetIndex];
        }
        EditorGUILayout.EndHorizontal();

        targetHue = EditorGUILayout.Slider("Target Hue", targetHue, 0f, 1f);

        DrawHueBar("타겟 색상", targetHue, targetHue);

        EditorGUILayout.Space();

        saturationBoost = EditorGUILayout.Slider("채도 보정", saturationBoost, -0.5f, 0.5f);

        EditorGUILayout.Space();

        if (sourceTexture != null)
        {
            if (GUILayout.Button("미리보기"))
            {
                previewTexture = ConvertTexture(sourceTexture);
            }

            if (previewTexture != null)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                GUILayout.Label("원본");
                GUILayout.Label(sourceTexture, GUILayout.Width(150), GUILayout.Height(150));
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                GUILayout.Label("변환 결과");
                GUILayout.Label(previewTexture, GUILayout.Width(150), GUILayout.Height(150));
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (GUILayout.Button("저장 (PNG)"))
                {
                    SaveTexture(previewTexture);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("이미지를 드래그하거나 선택하세요.", MessageType.Info);
        }
    }

    private void DrawHueBar(string label, float hueMin, float hueMax)
    {
        Rect barRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
            GUILayout.Height(16), GUILayout.ExpandWidth(true));

        for (int i = 0; i < (int)barRect.width; i++)
        {
            float h = i / barRect.width;
            EditorGUI.DrawRect(new Rect(barRect.x + i, barRect.y, 1, barRect.height),
                Color.HSVToRGB(h, 1f, 1f));
        }

        float minX = barRect.x + hueMin * barRect.width;
        float maxX = barRect.x + hueMax * barRect.width;

        if (hueMin == hueMax)
        {
            EditorGUI.DrawRect(new Rect(minX - 1, barRect.y, 3, barRect.height), Color.white);
        }
        else
        {
            EditorGUI.DrawRect(new Rect(minX, barRect.y, 2, barRect.height), Color.white);
            EditorGUI.DrawRect(new Rect(maxX, barRect.y, 2, barRect.height), Color.white);
            EditorGUI.DrawRect(new Rect(minX, barRect.y, maxX - minX, 2), Color.white);
            EditorGUI.DrawRect(new Rect(minX, barRect.yMax - 2, maxX - minX, 2), Color.white);
        }

        GUILayout.Label($"{label}  ({hueMin * 360f:F0}° ~ {hueMax * 360f:F0}°)",
            EditorStyles.miniLabel);
    }

    private Texture2D ConvertTexture(Texture2D source)
    {
        Texture2D readable = MakeReadable(source);
        Texture2D result = new Texture2D(readable.width, readable.height, TextureFormat.RGBA32, false);
        Color[] pixels = readable.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a < 0.05f) continue;

            float originalAlpha = pixels[i].a;

            Color.RGBToHSV(pixels[i], out float h, out float s, out float v);

            if (IsInRange(h, sourceHueMin, sourceHueMax))
            {
                h = targetHue;
                s = Mathf.Clamp01(s + saturationBoost);
            }

            pixels[i] = Color.HSVToRGB(h, s, v);
            pixels[i].a = originalAlpha;
        }

        result.SetPixels(pixels);
        result.Apply();
        return result;
    }

    private bool IsInRange(float h, float min, float max)
    {
        if (min <= max)
            return h >= min && h <= max;
        else
            return h >= min || h <= max;
    }

    private Texture2D MakeReadable(Texture2D source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0);
        Graphics.Blit(source, rt);
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readable = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        readable.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        readable.Apply();

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return readable;
    }

    private void SaveTexture(Texture2D texture)
    {
        // SaveFilePanelInProject 는 상대경로를 반환해 File.WriteAllBytes 실패
        // SaveFilePanel 은 절대경로를 반환하므로 정상 저장됨
        string absolutePath = EditorUtility.SaveFilePanel(
            "저장", Application.dataPath, "converted_texture", "png");

        if (!string.IsNullOrEmpty(absolutePath))
        {
            File.WriteAllBytes(absolutePath, texture.EncodeToPNG());

            // Assets 폴더 안이면 유니티 에셋으로 자동 등록
            if (absolutePath.StartsWith(Application.dataPath))
            {
                string relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
                AssetDatabase.ImportAsset(relativePath);
                Debug.Log($"저장 완료 (Assets): {relativePath}");
            }
            else
            {
                Debug.Log($"저장 완료 (외부): {absolutePath}");
            }
        }
    }
}