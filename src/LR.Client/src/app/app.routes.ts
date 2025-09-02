import { Routes } from '@angular/router';
import { ROUTES } from '../core/constants/routes.constants';
import { HomeComponent } from '../features/home/home.component';
import { RegisterComponent } from '../features/auth/components/register/register.component';
import { LoginComponent } from '../features/auth/components/login/login.component';
import { HomeLayoutComponent } from '../layout/home-layout/home-layout.component';
import { MainLayoutComponent } from '../layout/main-layout/main-layout.component';
import { DashboardComponent } from '../features/dashboard/dashboard.component';
import { GuestGuard } from '../core/guards/guest.guard';
import { AuthGuard } from '../core/guards/auth.guard';

export const routes: Routes = [
    { 
        path: ROUTES.HOME,
        component: HomeLayoutComponent,
        canActivate: [GuestGuard],
        children: [
            { path: ROUTES.HOME, component: HomeComponent },
            { path: ROUTES.AUTH.LOGIN, component: LoginComponent },
            { path: ROUTES.AUTH.REGISTER, component: RegisterComponent }
        ]
    },
    {
        path: ROUTES.MAIN,
        component: MainLayoutComponent,
        canActivate: [AuthGuard],
        children: [
            { path: ROUTES.DASHBOARD, component: DashboardComponent },
        ]
    },
    { path: "**", redirectTo: ROUTES.HOME, pathMatch: "full" },
];
