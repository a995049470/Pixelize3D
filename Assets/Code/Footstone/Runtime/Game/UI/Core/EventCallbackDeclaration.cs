using UnityEngine.EventSystems;

namespace Lost.Runtime.Footstone.Game
{
    public delegate void EventTriggerCallback(BaseEventData eventData);
    public delegate void BagGridEventCallback(BaseEventData eventData, UIModel_BagGrid bagGrid);
    public delegate void FastGridEventCallback(BaseEventData eventData, UIModel_FastGrid fastGrid);
    public delegate void CarftGridClickEvent(UIModel_CraftGrid craftGrid);
    public delegate void OblationGridClickEvent(UIModel_OblationGrid oblationGrid);
}
