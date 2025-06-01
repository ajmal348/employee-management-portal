import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Employee } from '../../shared/models/employee.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  getProfile(): Observable<Employee> {
    return this.http.get<Employee>(`${this.baseUrl}/EmployeeProfile/me`);
  }

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.baseUrl}/Employees`);
  }
  createEmployee(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Employees`, payload);
  }
  checkUsernameExists(username: string): Observable<boolean> {
    return this.http
      .get<{ exists: boolean }>(`${this.baseUrl}/Employees/exists`, {
        params: { username },
      })
      .pipe(map((response) => response.exists));
  }
  getEmployeeById(id: string): Observable<Employee> {
    return this.http.get<Employee>(`${this.baseUrl}/Employees/${id}`);
  }

  updateEmployee(id: string, data: any): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Employees/${id}`, data);
  }
  deleteEmployee(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Employees/${id}`);
  }
}
