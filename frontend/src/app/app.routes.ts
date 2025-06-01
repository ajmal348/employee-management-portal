import { Routes } from '@angular/router';
import { AdminLayout } from './modules/admin/admin-layout/admin-layout';
import { EmployeelistComponent } from './modules/admin/employeelist-component/employeelist-component';
import { EmployeeformComponent } from './modules/admin/employeeform-component/employeeform-component';
import { authGuard } from './core/guards/auth-guard';

export const appRoutes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./modules/auth/login-component').then((m) => m.LoginComponent),
  },
  {
    path: 'me',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./modules/employee/profile-component').then(
        (m) => m.ProfileComponent
      ),
  },
  {
    path: '',
    component: AdminLayout,
    canActivate: [authGuard],
    children: [
      { path: 'employees', component: EmployeelistComponent },
      { path: 'employees/add', component: EmployeeformComponent },
      { path: 'employees/edit/:id', component: EmployeeformComponent },
      { path: '', redirectTo: 'login', pathMatch: 'full' },
    ],
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
];
