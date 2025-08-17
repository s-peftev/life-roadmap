import { Routes } from '@angular/router';
import { ROUTES } from '../core/constants/routes.constants';
import { HomeComponent } from '../features/home/home.component';
import { RegisterComponent } from '../features/auth/register/register.component';
import { LoginComponent } from '../features/auth/login/login.component';
import { HomeLayoutComponent } from '../layout/home-layout/home-layout.component';

export const routes: Routes = [
    { 
        path: ROUTES.HOME,
        component: HomeLayoutComponent,
        children: [
            { path: ROUTES.HOME, component: HomeComponent },
            { path: ROUTES.AUTH.LOGIN, component: LoginComponent },
            { path: ROUTES.AUTH.REGISTER, component: RegisterComponent }
        ]
    },

    { path: "**", redirectTo: ROUTES.HOME, pathMatch: "full" },
];
