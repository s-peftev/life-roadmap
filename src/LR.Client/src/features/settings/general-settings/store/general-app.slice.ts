import { AppLanguage } from "../../../../core/enums/app-language.enum";
import { environment } from "../../../../environments/environment";

export interface GeneralAppSlice {
    readonly selectedLanguage: AppLanguage
    readonly avaiableLanguages: AppLanguage[]
}

export const initialGeneralAppSlice: GeneralAppSlice = {
    selectedLanguage: environment.defaultAppLanguage,
    avaiableLanguages: Object.values(AppLanguage)
}