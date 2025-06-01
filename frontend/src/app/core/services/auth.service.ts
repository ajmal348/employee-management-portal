import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7053/api/Auth/login';

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post(this.apiUrl, { username, password });
  }
  getToken(): string | null {
    return localStorage.getItem('token');
  }
  getRoleFromToken(token: string): string | null {
    try {
      const decoded: any = jwtDecode(token);
      return decoded[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ];
    } catch {
      return null;
    }
  }

  getRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
    const decoded: any = jwtDecode(token);
    return decoded[
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    ];
  }
  logout() {
    localStorage.removeItem('token'); 
    this.router.navigate(['/login']); 
  }
  getCurrentUser(): { username: string; role: string } | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return {
        username:
          decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        role: decoded[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ],
      };
    } catch {
      return null;
    }
  }
}
