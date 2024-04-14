using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.U2D.Animation;

namespace TanitakaTech.SpriteResolverTimeline.Editor
{
    [CustomTimelineEditor(typeof(SpriteResolverClip))]
    public class SpriteResolverClipTimelineEditor : ClipEditor
    {
        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            base.DrawBackground(clip, region);
        }
        
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            SpriteResolverClip spriteClip = clip.asset as SpriteResolverClip;
            if (spriteClip == null)
            {
                return base.GetClipOptions(clip);
            }

            var track = TimelineEditorExtensions.FindTrackFromClip(spriteClip);
            if (track == null)
            {
                return base.GetClipOptions(clip);
            }

            SpriteResolver resolver = track.GetBindingFromTrack<SpriteResolver>();
            if (resolver == null)
            {
                return base.GetClipOptions(clip);
            }
            
            Sprite previewSprite = resolver.spriteLibrary.GetSprite(spriteClip.Category, spriteClip.Label);
            return new ClipDrawOptions
            {
                errorText = GetErrorText(clip),
                highlightColor = GetDefaultHighlightColor(clip),
                icons = new[] { previewSprite.texture },
                tooltip = "Tooltip"
            };
        }
    }
}