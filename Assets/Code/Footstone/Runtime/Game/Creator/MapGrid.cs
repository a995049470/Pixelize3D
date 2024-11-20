namespace Lost.Runtime.Footstone.Game
{
    public struct MapGrid
    {
        private int val;

        //可行走 1~2 + 3

        //0:wall 1:way 3:unknow
        public int WalkableState
        {
            get => val & 3;
            set
            {
                if (!WalkableFixed)
                    val = val & (~3) | value;
            }
        }
        public bool WalkableFixed
        {
            get => (val & 4) > 0;
            set
            {
                if (value) val |= 4;
                else val &= ~4;
            }
        }
        //格子上的单位（怪物 宝箱等）4 ~ 11 + 12 
        public int UnitId
        {
            get => (val >> 3) & 255;
            set
            {

                val = val & (~(255 << 3)) | (value << 3);
            }
        }
        public bool UnitFixed {
            get => (val & (1 << 11)) > 0;
            set{
                if(value) val |= 1 << 11;
                else val &= ~(1 << 11);
            }
        }
        //地形 13 ~ 20 + 21
        public int TerrainId
        {
            get => (val >> 12) & 255;
            set
            {
                val = val & (~(255 << 12)) | (value << 12);
            }
        }

        // public bool TerrainFixed{
        //     get => (val & (1 << 20)) > 0;
        //     set{
        //         if(value) val |= 1 << 20;
        //         else val &= ~(1 << 20);
        //     }
        // }

        //通路检查 22 
        public bool PassTestChecked
        {
            get => (val & (1 << 21)) > 0;
            set
            {
                if (value) val |= 1 << 21;
                else val &= ~(1 << 21);
            }
        }

        //空无一物 23
        public bool IsNone
        {
            get => (val & (1 << 22)) > 0;
            set
            {
                if (value) val |= 1 << 22;
                else val &= ~(1 << 22);
            }
        }

        //不进行连通性测试 主要用于防止传送点成为唯一通路 24
        public bool IsNotPassTest
        {
            get => (val & (1 << 23)) > 0;
            set
            {
                if (value) val |= 1 << 23;
                else val &= ~(1 << 23);
            }
        }
    }
}
