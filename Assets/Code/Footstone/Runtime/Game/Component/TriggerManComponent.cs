using System;
using Lost.Runtime.Footstone.Core;
using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{

    //触发者
    //TODO:触发机关看成一种行为？
    [DefaultEntityComponentProcessor(typeof(TriggerManProcessor))]
    [DefaultEntityComponentProcessor(typeof(TriggerProcessor))]
    public class TriggerManComponent : EntityComponent
    {
        //触发事件后可能导致触发行为被静止
        [HideInInspector]
        public ActionFlag InvaildActions = ActionFlag.None;
        [HideInInspector]
        public Vector3 LastTriggerPosition = Vector3.one * float.MaxValue;

        public void AddInvaildActions(ActionFlag action)
        {
            InvaildActions |= action;
        }
    }

}
