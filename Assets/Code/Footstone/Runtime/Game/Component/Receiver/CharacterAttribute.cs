using System;
using Lost.Runtime.Footstone.Collection;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //角色属性
    [Serializable]
    public class CharacterAttribute
    {
        [SerializeField]
        private float baseValue = 0; 
        public float BaseVale { get => baseValue; }
        [SerializeField][HideInInspector]
        private SerializableDictionary<ulong, float> powerDic = new();
        [SerializeField][HideInInspector]
        private SerializableDictionary<ulong, float> percentPowerDic = new();
        
        //临时值需要记录当前帧序号
        private float tempPower = 0;
        private float tempPercentPower = 0;
        private int tempSetFrame = -1;

        private float finalValue = 0;
        private bool isDirty = true;
        public CharacterAttribute(float _baseValue = 0)
        {
            baseValue = _baseValue;
        }

        public float GetFinalValue(int frame)
        {
            if (frame != tempSetFrame)
            {
                tempSetFrame = frame;
                ClearTempPower();
            }

            if (isDirty)
            {
                isDirty = false;
                var value = tempPower + baseValue;
                foreach (var val in powerDic.Values)
                {
                    value += val;
                }
                value = Mathf.Max(0, value);

                var percent = 1 + tempPercentPower;
                foreach (var val in percentPowerDic.Values)
                {
                    percent += val;
                }
                percent = Mathf.Max(0, percent);
                
                finalValue = value * percent;
            }
            return finalValue;
        }

        public void ReceivePower(ulong uid, float value)
        {
            powerDic.Add(uid, value);
            isDirty = true;
        }

        public void LostPower(ulong uid)
        {
            powerDic.Remove(uid);
            isDirty = true;
        }

        public void ReceivePercentPower(ulong uid, float value)
        {
            percentPowerDic.Add(uid, value);
            isDirty = true;
        }
        
        public void LostPercentPower(ulong uid)
        {
            percentPowerDic.Remove(uid);
            isDirty = true;
        }
        //改变当前帧的修改值
        public void ReceiveTempPower(float value, int frame)
        {
            if(frame != tempSetFrame)
            {
                tempSetFrame = frame;
                ClearTempPower();
            }
            tempPower += value;
            isDirty = true;
        }
        //改变当前值的百分比修改值
        public void ReceiveTempPercentPower(int frame, float value)
        {
            if(frame != tempSetFrame)
            {
                tempSetFrame = frame;
                ClearTempPower();
            }
            tempPercentPower += value;
            isDirty = true;
        }

        public void ClearTempPower()
        {
            if(tempPower != 0)
            {
                tempPower = 0;
                isDirty = true;
            }
            if(tempPercentPower != 0)
            {
                tempPercentPower = 0;
                isDirty = true;
            }
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void OnBeforeSave()
        {
            powerDic.OnBeforeSave();
            percentPowerDic.OnBeforeSave();
        }

        public void OnAfterLoad()
        {
            powerDic.OnAfterLoad();
            percentPowerDic.OnAfterLoad();
            isDirty = true;
        }
        
    }

}



