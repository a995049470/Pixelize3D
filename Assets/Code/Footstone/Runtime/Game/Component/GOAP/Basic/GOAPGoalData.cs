using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [CreateAssetMenu(menuName = "Lost/GOAP/Goal")]
    public class GOAPGoalData : ScriptableObject
    {
        //优先度越高越会被优先执行
        public int Order = 0;
        [System.NonSerialized]
        public int Weight = 0;
        
        [SerializeField]
        private GOAPSerializableStatusDictionary serializedPreconditions = new();
        [SerializeField]
        private GOAPStatus target = new();
        
        private GOAPStatusDictionary preconditions { get => serializedPreconditions.Dictionary; }
        public GOAPStatus Target { get => target; }

        public bool IsAchievable(GOAPStatusDictionary worldStatus)
        {
            bool isSuccess = true;
            foreach (var kvp in preconditions)
            {
                isSuccess &= worldStatus.TryGetValue(kvp.Key, out var status) && status.Pass(kvp.Value);
                if(!isSuccess) break;
            }
            return isSuccess;
        }
        
        private void OnValidate() {
            serializedPreconditions.SetDirty();
        }
    }

}



