/* Copyright 2024 zakuro <z@kuro.red> (https://x.com/zakuro9715)
 * 
 * This file is part of AdvancedSimulationSpeed.
 * 
 * AdvancedSimulationSpeed is free software: you can redistribute it and/or modify it under the
 * terms of the GNU General Public License as published by the Free Software Foundation, either
 * version 3 of the License, or (at your option) any later version.
 * 
 * AdvancedSimulationSpeed is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE. See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with
 * AdvancedSimulationSpeed. If not, see <https://www.gnu.org/licenses/>.
 */

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using AdvancedSimulationSpeed.Systems;
using LibShared.Localization;
using System.Collections.Generic;


namespace AdvancedSimulationSpeed
{
    public class Mod : IMod
    {
        public static readonly string UI_ID = "AdvancedSimulationSpeed";

        public static ILog log = LogManager.GetLogger($"{nameof(AdvancedSimulationSpeed)}.{nameof(Mod)}").SetShowsErrorsInUI(false);
        public static Setting Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            Setting = new Setting(this);
            Setting.RegisterInOptionsUI();

            var localizationManager = GameManager.instance.localizationManager;
            foreach (var locale in localizationManager.GetSupportedLocales())
            {
                localizationManager.AddSource(locale, new AliasesLocaleSource(localizationManager, new Dictionary<string, string>
                {
                    { Setting.GetOptionLabelLocaleID(nameof(Setting.ResetSettings)), "Options.OPTION[GeneralSettings.resetSettings]" },
                    { Setting.GetOptionDescLocaleID(nameof(Setting.ResetSettings)), "Options.OPTION_DESCRIPTION[GeneralSettings.resetSettings]" },
                    { Setting.GetOptionWarningLocaleID(nameof(Setting.ResetSettings)), "Options.OPTION_DESCRIPTION[GeneralSettings.resetSettings]" },
                }));
            }
            LocaleLoader.Load(log, localizationManager);

            AssetDatabase.global.LoadSettings(nameof(AdvancedSimulationSpeed), Setting, new Setting(this));

            updateSystem.UpdateAt<UISystem>(SystemUpdatePhase.UIUpdate);

        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (Setting != null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }
        }
    }
}
