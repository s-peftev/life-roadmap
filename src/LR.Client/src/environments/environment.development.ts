import { AppLanguage } from "../core/enums/app-language.enum";

export const environment = {
    production: false,
    apiUrl: 'https://localhost:5001/api/',
    assetsBasePath: '/assets',
    roleClaimJwtKey: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    defaultAppLanguage: AppLanguage.En
};
