using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    //TODO:思考class和struct
    [System.Serializable]
    public class GOAPStatus
    {
        [SerializeField]
        private GOAPStatusFlag flag;
        //条件上下限 都包括
        [SerializeField]
        private int leftValue;
        [SerializeField]
        private int rightValue;
        public GOAPStatusFlag Flag { get => flag; }
        public bool IsVaild { get => rightValue >= leftValue; }
        
        public GOAPStatus()
        {

        }

        public GOAPStatus(GOAPStatusFlag _flag)
        {
            flag = _flag;
        }

        public bool Pass(GOAPStatus status)
        {
            var min = Mathf.Max(leftValue, status.leftValue);
            var max = Mathf.Min(rightValue, status.rightValue);
            bool isPass = status.flag == flag && max >= min;
            return isPass;
        }

        // public bool CanCombine(GOAPStatus status)
        // {
        //     var leftValue = Mathf.Max(this.leftValue, status.leftValue);
        //     var rightValue = Mathf.Min(this.rightValue, status.rightValue);
        //     return rightValue >= leftValue;
        // }

        
        public GOAPStatus Combine(GOAPStatus status)
        {
            var reslut = new GOAPStatus();
            reslut.flag = this.flag;
            reslut.leftValue = Mathf.Max(this.leftValue, status.leftValue);
            reslut.rightValue = Mathf.Min(this.rightValue, status.rightValue);
            return reslut;
        }

        public void Set(bool val)
        {
            Set(val ? 1 : 0);
        }

        public void Set(int val)
        {
            leftValue = val;
            rightValue = val;
        }
        
        public void Set(int left, int right)
        {
            leftValue = left;
            rightValue = right;
        }

        private int FloatToInt(float val, float scale)
        {
            return Mathf.CeilToInt(val * scale);
        }

        private float IntToFloat(int val, float scale)
        {
            return val / scale;
        }

        public void Set(float val, float scale)
        {
            int v = FloatToInt(val, scale);
            leftValue = v;
            rightValue = v;
        }

        public void Set(float left, float right, float scale)
        {
            leftValue = FloatToInt(left, scale);
            rightValue = FloatToInt(right, scale);
        }
        
        public int GetInt()
        {
            return leftValue;
        }

        public bool GetBool()
        {
            return GetInt() != 0;
        }

        public float GetFloat(float scale)
        {
            return IntToFloat(leftValue, scale);
        }

        public Vector2Int GetV2Int()
        {
            return new Vector2Int(leftValue, rightValue);
        }

        public Vector2 GetV2(float scale)
        {
            return new Vector2(
                IntToFloat(leftValue, scale),
                IntToFloat(rightValue, scale)
            );
        }

    }

}



