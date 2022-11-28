using UnityEngine.UIElements;

namespace jbzd.Enemies.BehaviourPattern
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> {}
        
        public InspectorView()
        {
            
        }
    }
}
