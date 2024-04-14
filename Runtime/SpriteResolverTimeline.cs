using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.U2D.Animation;

namespace TanitakaTech.SpriteResolverTimeline
{
    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(SpriteResolverClip))]
    [TrackBindingType(typeof(SpriteResolver))]
    public class SpriteResolverTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var mixer = ScriptPlayable<SpriteResolverMixerBehaviour>.Create(graph, inputCount);
            return mixer;
        }
    }

    public class SpriteResolverBehaviour : PlayableBehaviour
    {
        public string Category;
        public string Label;
        private SpriteResolver _resolver;

        // SpriteResolverをセットするためのメソッド
        public void SetResolver(SpriteResolver resolver)
        {
            _resolver = resolver;
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (_resolver != null)
            {
                _resolver.SetCategoryAndLabel(Category, Label);
            }
        }
    }
    
    
    public class SpriteResolverMixerBehaviour : PlayableBehaviour
    {
        private SpriteResolver _spriteResolver;
        private readonly Dictionary<Playable, SpriteResolverBehaviour> clipBehaviours = new Dictionary<Playable, SpriteResolverBehaviour>();
        
        public void SetResolverToAllClip(SpriteResolver resolver)
        {
            _spriteResolver = resolver;
        }
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _spriteResolver = playerData as SpriteResolver;

            if (_spriteResolver == null)
                return;

            int inputCount = playable.GetInputCount();
            string activeCategory = "";
            string activeLabel = "";

            for (int i = 0; i < inputCount; i++)
            {
                float weight = playable.GetInputWeight(i);
                ScriptPlayable<SpriteResolverBehaviour> inputPlayable = (ScriptPlayable<SpriteResolverBehaviour>)playable.GetInput(i);
                SpriteResolverBehaviour input = inputPlayable.GetBehaviour();

                if (weight > 0f)
                {
                    activeCategory = input.Category;
                    activeLabel = input.Label;
                    // Store the behaviours for potential blending or other effects
                    clipBehaviours[inputPlayable] = input;
                }
            }

            if (!string.IsNullOrEmpty(activeCategory) && !string.IsNullOrEmpty(activeLabel))
            {
                _spriteResolver.SetCategoryAndLabel(activeCategory, activeLabel);
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            clipBehaviours.Clear();
        }
    }
}