using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    [System.Serializable]
    public class UnitState
    {
        /// <summary>
        /// 受保护的状态时间
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private float protectedDuration;
        /// <summary>
        /// 动作标签
        /// </summary>
        [SerializeField]
        private StateFlag flag;
        /// <summary>
        /// 动作Id
        /// </summary>
        [SerializeField]
        private int subIndex;
        private bool isNameHashDitry;
        private bool isNameDitry;
        [HideInInspector]
        [SerializeField]
        private int nameHash;
        public bool Loop;
        public float Speed = 1;
        public float Duration = -1;
        public bool IsOverwriteDuration { get => Duration > 0; }
        [HideInInspector]
        public float CurrentTime;
        public int NameHash
        {
            get
            {
                if(isNameHashDitry)
                {
                    isNameHashDitry = false;
                    nameHash = Animator.StringToHash(StateName);
                } 
                return nameHash;
            }
        } 

        private string stateName;
        public string StateName
        {
            get
            {
                if(isNameDitry)
                {
                    isNameDitry = false;
                    stateName = subIndex > 0 ? $"{flag.ToString()}_{subIndex}" : flag.ToString();
                }
                return stateName;
            }
        }
        //防止循环动画导致开头和结尾的事件被错误触发
        [HideInInspector]
        public float CurrentEventNormalizedTime = 0;
        
        //动作事件
        [SerializeField]
        private AnimationClipEventControllerReference eventControllerReference = new();
        public AnimationClipEventController AnimationClipEventController
        {
            get => eventControllerReference?.Asset;
        }
        public int EventPtr = 0;
        
        
        public UnitState(StateFlag _flag, int _subIndex, float _protectTime, float _currentTime, string _eventControllerKey)
        {
            Init(_flag, _subIndex, _protectTime, _currentTime, 1, true, -1, _eventControllerKey);
        }
        

        public void Init(StateFlag _flag, int _subIndex, float _protectTime, float _currentTime, float _speed, bool _loop, float _duration, string _eventControllerKey)
        {
            flag = _flag;
            protectedDuration = _protectTime;
            CurrentTime = _currentTime;
            Loop = _loop;
            isNameHashDitry = true;
            isNameDitry = true;
            Speed = _speed;
            subIndex = _subIndex;
            Duration = _duration;
            eventControllerReference.SetKey(_eventControllerKey);
            EventPtr = 0;
            CurrentEventNormalizedTime = 0;
        }

        public void Run(float time)
        {
            CurrentTime += time;
        }

        public bool Switchable(StateFlag nextFlag, int nextSubIndex)
        {
            return (flag != nextFlag || subIndex != nextSubIndex) && protectedDuration >= 0 && CurrentTime >= protectedDuration;
        }

        public bool TryChangeSpeed(StateFlag _flag, int _index, float _speed)
        {
            bool isSuccess = false;
            if(flag == _flag && subIndex == _index && Speed != _speed)
            {
                isSuccess = true;
                Speed = _speed;
            }
            return isSuccess;
        }

        public void SetDirty()
        {
            isNameHashDitry = true;
            isNameDitry = true;
        }
    }

}
