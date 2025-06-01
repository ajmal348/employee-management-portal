import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { Employee } from '../../../shared/models/employee.model';
import { Router } from '@angular/router';
import { EmployeeService } from '../../../core/services/employee.service';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-employeelist-component',
  imports: [CommonModule],
  templateUrl: './employeelist-component.html',
  styleUrl: './employeelist-component.scss',
})
export class EmployeelistComponent {
  loading = false;
  errorMsg: string | undefined;
  employees: Employee[] = [];

  constructor(
    private router: Router,
    private employeeService: EmployeeService
  ) {}
  ngOnInit(): void {
    this.fetchAll();
  }
  fetchAll(): void {
    this.loading = true;
    this.employeeService.getAllEmployees().subscribe({
      next: (data) => {
        this.employees = data;
        this.loading = false;
      },
      error: (err) => {
        this.errorMsg = 'Failed to load employees.';
        this.loading = false;
      },
    });
  }
  edit(empId: string): void {
    if (!empId) return;
    this.router.navigate(['/employees/edit', empId]);
  }
  delete(id: string): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You won’t be able to revert this!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'Cancel',
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteById(id).subscribe((success) => {
          if (success) {
            Swal.fire('Deleted!', 'Employee has been deleted.', 'success');
            this.fetchAll();
          } else {
            Swal.fire('Error', 'Failed to delete employee.', 'error');
          }
        });
      }
    });
  }
  deleteById(id: string): Observable<boolean> {
    return this.employeeService.deleteEmployee(id).pipe(
      map(() => true),
      catchError((err) => {
        console.error('Delete error:', err);
        return of(false);
      })
    );
  }

  navigateToAddEmployee(): void {
    this.router.navigate(['/employees/add']);
  }
}
