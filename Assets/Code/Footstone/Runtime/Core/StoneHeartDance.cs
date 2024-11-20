using System;
using UnityEngine;
namespace Lost.Runtime.Footstone.Core
{
    public class StoneHeartDance : MonoBehaviour
    {
        public UpdateableCollection UpdateableCollection = new();
        private GameTime gameTime;

        private void Awake() {
            gameTime = new GameTime();
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update() {
            
            gameTime.Update(TimeSpan.FromSeconds(Time.timeAsDouble), TimeSpan.FromSeconds(Time.deltaTime), true);
            foreach (var item in UpdateableCollection)
            {
                item.Update(gameTime);
            }
        }
    }
}



