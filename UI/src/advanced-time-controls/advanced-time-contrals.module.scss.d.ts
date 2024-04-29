export type Styles = {
  "advanced-time-controls": string;
  button: string;
  "button--left": string;
  "button--right": string;
  "center-box": string;
  content: string;
  "divider--left": string;
  "divider--right": string;
  icon: string;
};

export type ClassNames = keyof Styles;

declare const styles: Styles;

export default styles;
