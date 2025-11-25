import { PartialStateUpdater } from "@ngrx/signals";
import { AppLanguage } from "../../../../core/enums/app-language.enum";
import { GeneralAppSlice } from "./general-app.slice";

export function selectLanguage(selectedLanguage: AppLanguage): PartialStateUpdater<GeneralAppSlice> {
    return _ => ({
        selectedLanguage
    })
}