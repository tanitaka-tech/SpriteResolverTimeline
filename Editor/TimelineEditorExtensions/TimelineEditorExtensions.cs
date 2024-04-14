using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public static class TimelineEditorExtensions
{
    public static TimelineClip[] GetClips(this TrackAsset trackAsset)
    {
        return trackAsset.clips;
    }

    public static TrackAsset FindTrackFromClip(UnityEngine.Object clipObject)
    {
        // エディタからタイムラインの情報を取得
        if (TimelineEditor.inspectedAsset != null)
        {
            foreach (var track in TimelineEditor.inspectedAsset.GetOutputTracks())
            {
                if (track.GetClips().Any(c => c.asset == clipObject))
                {
                    return track;
                }
            }
        }
        return null;
    }

    public static T GetBindingFromTrack<T>(this TrackAsset track) where T : class
    {
        PlayableDirector director = FindCurrentPlayableDirector();
        return director != null && director.GetGenericBinding(track) is T bindingReference
            ? bindingReference
            : null;
    }

    public static PlayableDirector FindCurrentPlayableDirector()
    {
        PlayableDirector[] directors = Object.FindObjectsByType<PlayableDirector>(sortMode: FindObjectsSortMode.None);
        PlayableDirector director = directors.FirstOrDefault(d => d.playableAsset == TimelineEditor.inspectedAsset);
        if (director == null)
        {
            // PrefabStageから現在編集中のPlayableDirectorを取得
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                // 現在編集中のPrefabのルートからPlayableDirectorを探す
                directors = prefabStage.prefabContentsRoot.GetComponentsInChildren<PlayableDirector>(true);
                director = directors.FirstOrDefault(d => d.playableAsset == TimelineEditor.inspectedAsset);
            }
        }

        return director;
    }
}
