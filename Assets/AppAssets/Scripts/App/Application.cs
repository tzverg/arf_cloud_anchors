using AppName.App.Data;
using Evolutex.Evolunity.Components;

namespace AppName.App
{
    public class Application : SingletonBehaviour<Application>
    {
        public AppConfig Config;

        protected override void Awake()
        {
            base.Awake();

            Config.ApplyScreenSettings();
        }
    }
}