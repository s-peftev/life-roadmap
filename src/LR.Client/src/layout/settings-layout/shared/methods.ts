import { Router } from "@angular/router";
import { ROUTES } from "../../../core/constants/routes.constants";

export function openSettingTab(settingTabName: string, router: Router): void {
    router.navigate(
      [{ outlets: { modal: [ROUTES.SETTINGS.BASE, settingTabName] } }],
    );
  }