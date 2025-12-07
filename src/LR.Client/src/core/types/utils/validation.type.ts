import { Observable } from "rxjs";

export type ValidationIndicator = {
  key: string;
  message$: Observable<string>;
  icons: ValidationIcon;
}

export type ValidationIcon = {
  OK: string;
  ERR: string;
}