using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.U2D.Animation;

namespace TanitakaTech.SpriteResolverTimeline
{
    [System.Serializable]
    public class SpriteResolverClip : PlayableAsset
    {
        public string Category;
        public string Label;
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SpriteResolverBehaviour>.Create(graph);
            SpriteResolverBehaviour behaviour = playable.GetBehaviour();
            behaviour.Category = Category;
            behaviour.Label = Label;
        
            var resolver = owner.GetComponent<SpriteResolver>();
            if (resolver != null)
            {
                behaviour.SetResolver(resolver);
            }

            return playable;
        }
    }
}