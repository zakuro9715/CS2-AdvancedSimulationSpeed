import { ModuleRegistryExtend } from "cs2/modding";
import { useValue, bindValue, trigger } from "cs2/api";
import { Button, Icon } from "cs2/ui";
import { time } from "cs2/bindings";
import { getModuleComponent, getModuleClasses } from "../utils/modding";
import styles from "./advanced-time-contrals.module.scss";
import mod from "../../mod.json";

const toolbarFieldPath =
  "game-ui/game/components/toolbar/components/field/field.tsx";
const fieldStyles = getModuleClasses<{ field: any; content: any }>(
  "game-ui/game/components/toolbar/components/field/field.module.scss",
);
const Divider = getModuleComponent(toolbarFieldPath, "Divider");

// When pausing, time.simulationSpeed$ returns `m_SpeedBeforePause`.
// But we want raw selectedSpeed value. So use another binding.
const selectedSpeed$ = bindValue<number>(mod.id, "selectedSpeed");
const displayActualSpeed$ = bindValue<boolean>(mod.id, "displayActualSpeed");
const displayOnlyMode$ = bindValue<boolean>(mod.id, "displayOnlyMode");
const actualSpeed$ = bindValue<number>(mod.id, "actualSpeed");

const selectSpeed = (x: number) => trigger(mod.id, "selectSpeed", x);
const nextSpeed = selectSpeed.bind(null, 1);
const prevSpeed = selectSpeed.bind(null, -1);
const requestRefresh = () => trigger(mod.id, "refresh");

export const AdvancedTimeControls: ModuleRegistryExtend =
  (Component) => (props) => {
    const { children, ...otherProps } = props || {};

    const selectedSpeed = useValue(selectedSpeed$);
    const actualSpeed = useValue(actualSpeed$);
    const displayOnlyMode = useValue(displayOnlyMode$);
    const displayActualSpeed = useValue(displayActualSpeed$);
    //const paused = useValue(time.simulationPaused$);

    selectedSpeed$.subscribe(requestRefresh);

    const left = (
      <>
        <Button
          focusKey="ADVANCED_SIMULATION_SPEEED_PREV_SPEED"
          className={`${styles.button} ${styles["button--left"]}`}
          onSelect={prevSpeed}
        >
          <Icon
            className={styles.icon}
            tinted
            src="coui://uil/Standard/ArrowLeftTriangle.svg"
          />
        </Button>
        <Divider />
      </>
    );
    const center = (
      <div className={styles["center-box"]}>
        <span>{selectedSpeed}x</span>
        {displayActualSpeed && <span>({actualSpeed.toFixed(6)})</span>}
      </div>
    );
    const right = (
      <>
        <Divider />
        <Button
          focusKey="ADVANCED_SIMULATION_SPEEED_NEXT_SPEED"
          className={`${styles.button} ${styles["button--right"]}`}
          onSelect={nextSpeed}
        >
          <Icon
            className={styles.icon}
            tinted
            src="coui://uil/Standard/ArrowRightTriangle.svg"
          />
        </Button>
      </>
    );
    return (
      <>
        <Component {...otherProps}>{children}</Component>
        <div
          className={`${fieldStyles.field} ${styles["advanced-time-controls"]}`}
        >
          {!displayOnlyMode && left}
          {center}
          {!displayOnlyMode && right}
        </div>
      </>
    );
  };
