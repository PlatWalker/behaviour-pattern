using UnityEngine.UIElements;

namespace jbzd.Enemies.BehaviourPattern
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> {}

        public SplitView()
        {
            
        }
    }
}
