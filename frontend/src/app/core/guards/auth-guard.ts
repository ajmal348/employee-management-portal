import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../services/auth.service";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();
  if (!token) {
    router.navigate(['/login']);
    return false;
  }

  const role = authService.getRoleFromToken(token);

  // Allow both Employee & Admin for /me
  if (state.url.startsWith('/me')) {
    return true;
  }

  // Allow only Admins for admin routes
  const isAdminRoute = ['/employees', '/employees/add', '/employees/edit', '/dashboard'].some(path =>
    state.url.startsWith(path)
  );

  if (isAdminRoute && role !== 'Admin') {
    router.navigate(['/me']);
    return false;
  }

  return true;
};
