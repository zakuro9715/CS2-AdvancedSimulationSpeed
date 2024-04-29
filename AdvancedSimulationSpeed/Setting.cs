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

using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace AdvancedSimulationSpeed
{
    [FileLocation(nameof(AdvancedSimulationSpeed))]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";

        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }


        public enum Mode
        {
            Doubler,
            FixedStep,
            DisplayOnly,
        }

        [SettingsUISection(kSection)]
        public Mode ModeSelection{ get; set; }

        private bool StepValueDisabled => ModeSelection != Mode.FixedStep;

        [SettingsUISlider(min = 0, max = 8, step = 1f, unit = Unit.kInteger)]
        [SettingsUISection(kSection)]
        [SettingsUIDisableByCondition(typeof(Setting), "StepValueDisabled")]
        public float StepValue { get; set; }
        public float StepValueWhenLessThanOne => StepValue / 10;


        [SettingsUISection(kSection)]
        public bool DisplayActualSpeed { get; set; }


        public override void SetDefaults()
        {
            ModeSelection = Mode.Doubler;
            StepValue = 2;
            DisplayActualSpeed = true;
        }
    }
}
