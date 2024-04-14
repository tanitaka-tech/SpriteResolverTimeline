using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace TanitakaTech.SpriteResolverTimeline.Editor
{
    [CustomEditor(typeof(SpriteResolverClip))]
    public class SpriteResolverClipInspectorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SpriteResolverClip clip = target as SpriteResolverClip;

            var track = TimelineEditorExtensions.FindTrackFromClip(clip);
            if (track == null)
            {
                return;
            }
            SpriteResolver resolver = track.GetBindingFromTrack<SpriteResolver>();
            if (resolver == null)
            {
                return;
            }

            // SpriteResolverとそのカテゴリ、ラベルに基づいてUIを構築
            string[] categories = resolver.spriteLibrary.spriteLibraryAsset.GetCategoryNames()
                .ToArray();
            int categoryIndex = Mathf.Max(0, Array.IndexOf(categories, clip.Category));
            categoryIndex = EditorGUILayout.Popup("Category", categoryIndex, categories);
            clip.Category = categories[categoryIndex];

            string[] labels = resolver.spriteLibrary.spriteLibraryAsset
                .GetCategoryLabelNames(clip.Category).ToArray();
            int labelIndex = Mathf.Max(0, Array.IndexOf(labels, clip.Label));
            labelIndex = EditorGUILayout.Popup("Label", labelIndex, labels);
            clip.Label = labels[labelIndex];

            GUILayout.Label("----- References (Read Only) -----");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("SpriteLibraryAsset");
            EditorGUILayout.ObjectField(resolver.spriteLibrary.spriteLibraryAsset,
                typeof(SpriteLibraryAsset), false);
            EditorGUILayout.EndHorizontal();

            Sprite previewSprite = resolver.spriteLibrary.GetSprite(clip.Category, clip.Label);
            if (previewSprite != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Sprite");
                EditorGUILayout.ObjectField(previewSprite, typeof(Sprite), false);
                EditorGUILayout.EndHorizontal();

                // スプライトのテクスチャ座標を取得
                Rect spriteRect = previewSprite.rect;
                Texture2D texture = previewSprite.texture;

                // スプライトのテクスチャ座標を正規化
                float texWidth = texture.width;
                float texHeight = texture.height;
                Rect texCoords = new Rect(spriteRect.x / texWidth, spriteRect.y / texHeight,
                    spriteRect.width / texWidth, spriteRect.height / texHeight);

                // GUIで表示するための矩形領域を取得
                Rect rect = GUILayoutUtility.GetRect(512, 512);

                // テクスチャを描画するためのスケーリングと位置調整
                GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
            }
        }
    }
}