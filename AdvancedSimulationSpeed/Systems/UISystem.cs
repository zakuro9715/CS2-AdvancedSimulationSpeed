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
using Colossal.UI.Binding;
using Game.UI;
using Game.Simulation;
using AdvancedSimulationSpeed.Utils;
using Unity.Mathematics;
using System;

namespace AdvancedSimulationSpeed.Systems
{
    public partial class UISystem : UISystemBase
    {
        private Throttle _actualSpeedUpdateThrottole;
        private ValueBinding<float> _actualSpeed;
        private ValueBinding<bool> _displayActualSpeed;
        private ValueBinding<bool> _displayOnlyMode;

        private SimulationSystem _simulationSystem;

        private float SelectedSpeed
        {
            get => _simulationSystem.selectedSpeed;
            set => _simulationSystem.selectedSpeed = value;
        }

        public string BindGroupName => Mod.UI_ID;

        private float GetActualSpeed() => _simulationSystem.smoothSpeed;
        private float GetSelectedSpeeed() => _simulationSystem.selectedSpeed;

        private bool DisplayActualSpeed => Mod.Setting.DisplayActualSpeed;
        private bool DisplayOnlyMode => Mod.Setting.ModeSelection == Setting.Mode.DisplayOnly;

        private void Refresh() {
            _actualSpeedUpdateThrottole.InvokeAction();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            _simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();

            Mod.Setting.onSettingsApplied += (Game.Settings.Setting setting) => {
                _displayActualSpeed.Update(DisplayActualSpeed);
                _displayOnlyMode.Update(DisplayOnlyMode);
            };

            AddUpdateBinding(new GetterValueBinding<float>(BindGroupName, "selectedSpeed", GetSelectedSpeeed));
            AddBinding(_displayActualSpeed = new ValueBinding<bool>(BindGroupName, "displayActualSpeed", DisplayActualSpeed));
            AddBinding(_displayOnlyMode = new ValueBinding<bool>(BindGroupName, "displayOnlyMode", DisplayOnlyMode));
            AddBinding(_actualSpeed = new ValueBinding<float>(BindGroupName, "actualSpeed", GetActualSpeed()));
            _actualSpeedUpdateThrottole = Throttle.ByMilliSeconds(500, () => { _actualSpeed.Update(GetActualSpeed()); });

            AddBinding(new TriggerBinding(BindGroupName, "refresh", Refresh));
            AddBinding(new TriggerBinding<float>(BindGroupName, "setSpeed", (v) => SelectedSpeed = v));
            AddBinding(new TriggerBinding<float>(BindGroupName, "selectSpeed", (n) => {
                var oldSpeed = GetSelectedSpeeed();
                var newSpeed = Mod.Setting.ModeSelection switch
                {
                    Setting.Mode.Doubler => SelectedSpeed * math.pow(2, n),
                    Setting.Mode.FixedStep => SelectedSpeed + (n * ((oldSpeed, n) switch
                    {
                        (_, 0) => 0, // No Change
                        (1, > 0) => Mod.Setting.StepValue - 1, // FixedStep Mode is 0-based step. like 1 2 4 6 8. So when value == 1, add StepValue - 1 
                        (1, < 0) => Mod.Setting.StepValueWhenLessThanOne,
                        ( > 1, _) => Mod.Setting.StepValue,
                        ( < 1, _) => Mod.Setting.StepValueWhenLessThanOne,
                        (var x1, var x2) => throw new Exception($"Unreachable: ({x1}, {x2})")
                    })),
                    var e => throw new Exception($"Invalid enum value {e}")
                };
                if (math.min(oldSpeed, newSpeed) < 1 && math.max(oldSpeed, newSpeed) > 1)
                {
                    // Stop once at 1x speed.
                    newSpeed = 1;
                }
                SelectedSpeed = math.clamp(newSpeed, 0, 8);
            }
          ));

            Mod.log.Info("UISystem OnCreate Done");
        }

        protected override void OnUpdate() {
            base.OnUpdate();

            _actualSpeedUpdateThrottole.Update(World.Time.DeltaTime);
        }
    }
}
