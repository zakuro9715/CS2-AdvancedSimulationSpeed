export type Styles = {
  "advanced-time-controls": string;
  button: string;
  "button--left": string;
  "button--right": string;
  "center-box": string;
  content: string;
  icon: string;
  paused: string;
};

export type ClassNames = keyof Styles;

declare const styles: Styles;

export default styles;
