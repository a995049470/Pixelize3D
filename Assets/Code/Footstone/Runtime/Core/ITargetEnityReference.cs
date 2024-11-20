namespace Lost.Runtime.Footstone.Core
{
    /// <summary>
    /// 具有目标Entity引用， 一般用在Unity的Component类上
    /// </summary>
    public interface ITargetEnityReference
    {
        public Entity Target { get; set; }
    }
}



