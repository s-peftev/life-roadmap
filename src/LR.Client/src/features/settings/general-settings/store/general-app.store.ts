import { patchState, signalStore, withMethods, withProps, withState } from "@ngrx/signals";
import { initialGeneralAppSlice } from "./general-app.slice";
import { withBusy } from "../../../../store-extentions/features/with-busy/with-busy.feature";
import { inject } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { AppLanguage } from "../../../../core/enums/app-language.enum";
import { setBusy, setIdle } from "../../../../store-extentions/features/with-busy/with-busy.updaters";
import { selectLanguage } from "./general-app.updaters";
import { STORAGE_KEYS } from "../../../../core/constants/storage-keys.constants";

export const GeneralAppStore = signalStore(
    { providedIn: 'root' },
    withState(initialGeneralAppSlice),
    withBusy(),
    withProps(() => {
        const _translateService = inject(TranslateService)

        return {
            _translateService
        }
    }),
    withMethods((store) => {
        return {
            selectLanguage: (lang: AppLanguage): void => {
                patchState(store, setBusy(), selectLanguage(lang));
                localStorage.setItem(STORAGE_KEYS.APP_LANG, lang);
                store._translateService.use(lang).subscribe({
                    complete: () => patchState(store, setIdle())
                })
            }
        }
    })
);