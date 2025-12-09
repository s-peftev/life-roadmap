import { AppLanguage } from "../core/enums/app-language.enum";

export const environment = {
    production: true,
    apiUrl: 'api/',
    assetsBasePath: '/assets',
    roleClaimJwtKey: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role',
    defaultAppLanguage: AppLanguage.En,
    paginationDefaults: {
        pageNumber: 1,
        pageSize: 10
    }
};
