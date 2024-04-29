import { ModRegistrar } from "cs2/modding";
import { AdvancedTimeControls } from "advanced-time-controls/advanced-time-controls";

const register: ModRegistrar = (moduleRegistry) => {
  moduleRegistry.extend(
    "game-ui/game/components/toolbar/bottom/time-controls/time-controls.tsx",
    "TimeControls",
    AdvancedTimeControls,
  );
};

export default register;
