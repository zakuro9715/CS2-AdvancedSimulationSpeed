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

using System;

namespace AdvancedSimulationSpeed.Utils
{
    public class Throttle
    {
        public float Duration { get; private set; }
        public float Elapsed { get; private set; }
        public Action Action { get; }

        public Throttle(Action action) : this(1, 0, action) { }
        public Throttle(float duration, Action action) : this(duration, 0, action) { }
        public Throttle(float duration, float elapsed, Action action)
        {
            Duration = duration;
            Elapsed = elapsed;
            Action = action;
        }

        public static Throttle BySeconds(float durationSec, Action action) => new Throttle(durationSec, action);
        public static Throttle ByMilliSeconds(float durationMilliSec, Action action) => BySeconds(durationMilliSec / 1000f, action);

        public void Reset()
        {
            Elapsed = 0;
        }

        public void InvokeAction() {
            Elapsed = 0;
            Action();
        }

        public void Update(float delta)
        {
            Elapsed += delta;
            
            if (Elapsed >= Duration)
            {
                InvokeAction();
            }
        }

    }
}
