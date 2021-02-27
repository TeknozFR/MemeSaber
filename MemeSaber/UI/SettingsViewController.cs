using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using MemeSaber.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;


namespace MemeSaber.UI
{
    class SettingsViewController : IInitializable, IDisposable
    {

        [UIValue("memesaber-enabled")]
        public bool MemeSaberEnabled {
            get { return PluginConfig.Instance.MemeSaberEnabled; }
            set { PluginConfig.Instance.MemeSaberEnabled = value; }
        }

        [UIValue("default-enabled")]
        public bool DefaultMemesEnabled
        {
            get { return PluginConfig.Instance.DefaultMemesEnabled; }
            set { PluginConfig.Instance.DefaultMemesEnabled = value; }
        }

        [UIValue("custom-enabled")]
        public bool CustomMemesEnabled
        {
            get { return PluginConfig.Instance.CustomMemesEnabled; }
            set { PluginConfig.Instance.CustomMemesEnabled = value; }
        }

        

        public void Initialize() => BSMLSettings.instance.AddSettingsMenu(nameof(MemeSaber), "MemeSaber.UI.Settings.bsml", this);
        public void Dispose() => BSMLSettings.instance.RemoveSettingsMenu(this);
    }
}
