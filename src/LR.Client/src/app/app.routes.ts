import { Routes } from '@angular/router';
import { ROUTES } from '../core/constants/routes.constants';
import { HomeComponent } from '../features/home/home.component';
import { RegisterComponent } from '../features/auth/components/register/register.component';
import { LoginComponent } from '../features/auth/components/login/login.component';
import { HomeLayoutComponent } from '../layout/home-layout/home-layout.component';
import { MainLayoutComponent } from '../layout/main-layout/main-layout.component';
import { FinancesComponent } from '../features/finances/finances.component';
import { GuestGuard } from '../core/guards/guest.guard';
import { AuthGuard } from '../core/guards/auth.guard';
import { ForgotPasswordComponent } from '../features/auth/components/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from '../features/auth/components/reset-password/reset-password.component';
import { SettingsLayoutComponent } from '../layout/settings-layout/settings-layout.component';
import { GeneralSettingsComponent } from '../features/settings/general-settings/general-settings.component';
import { ProfileSettingsComponent } from '../features/settings/profile-settings/profile-settings.component';
import { ChangePasswordComponent } from '../features/settings/profile-settings/components/change-password/change-password.component';
import { AdminDashboardComponent } from '../features/admin-dashboard/admin-dashboard.component';
import { AdminGuard } from '../core/guards/admin.guard';

export const routes: Routes = [
    {
        path: ROUTES.HOME,
        component: HomeLayoutComponent,
        canActivate: [GuestGuard],
        children: [
            { path: ROUTES.HOME, component: HomeComponent },
            { path: ROUTES.AUTH.LOGIN, component: LoginComponent },
            { path: ROUTES.AUTH.REGISTER, component: RegisterComponent },
            { path: ROUTES.AUTH.FORGOT_PASSWORD, component: ForgotPasswordComponent },
            { path: ROUTES.AUTH.RESET_PASSWORD, component: ResetPasswordComponent },
        ]
    },
    {
        path: ROUTES.MAIN,
        component: MainLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            { path: ROUTES.FINANCES, component: FinancesComponent },
            { path: ROUTES.ADMIN.DASHBOARD, component: AdminDashboardComponent, canActivate: [AdminGuard] },
            {
                path: ROUTES.SETTINGS.BASE,
                component: SettingsLayoutComponent,
                outlet: 'modal',
                children: [
                    { path: ROUTES.SETTINGS.PROFILE, component: ProfileSettingsComponent },
                    { path: ROUTES.SETTINGS.GENERAL, component: GeneralSettingsComponent },
                    { path: ROUTES.SETTINGS.CHANGE_PASSWORD, component: ChangePasswordComponent },
                ]
            },
        ]
    },

    { path: "**", redirectTo: ROUTES.HOME, pathMatch: "full" },
];
