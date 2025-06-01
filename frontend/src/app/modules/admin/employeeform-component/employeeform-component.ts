import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { EmployeeService } from '../../../core/services/employee.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of, map, catchError } from 'rxjs';
import Swal from 'sweetalert2';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-employeeform-component',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './employeeform-component.html',
  styleUrl: './employeeform-component.scss',
})
export class EmployeeformComponent implements OnInit {
  empForm!: FormGroup;
  isEditMode = false;
  loading = false;
  errorMsg = '';
  currentUsername: string | null = null;

  departments: string[] = [
    'IT',
    'HR',
    'Finance',
    'Marketing',
    'Admin',
    'Sales',
    'Legal',
  ];
  roles: string[] = ['Admin', 'Employee'];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!id;
    this.buildForm();
    if (this.isEditMode && id) {
      this.loadEmployee(id);
    }
  }

  buildForm(): void {
    this.empForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: [
        '',
        [Validators.required, Validators.pattern(/^\+?[0-9]{7,15}$/)],
      ],
      department: ['', Validators.required],
      designation: ['', Validators.required],
      dateOfJoining: ['', Validators.required],
      role: ['', Validators.required],
      username: [
        '',
        [Validators.required, Validators.minLength(3)],
        [this.usernameValidator()],
      ],
      ...(this.isEditMode
        ? {}
        : {
            password: [
              '',
              [
                Validators.required,
                Validators.minLength(8),
                Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/),
              ],
            ],
          }),
    });
  }

  loadEmployee(id: string): void {
    this.loading = true;
    this.employeeService.getEmployeeById(id).subscribe({
      next: (emp) => {
        this.currentUsername = emp.username;
        // Format dateOfJoining to 'YYYY-MM-DD'
        const formattedDate = emp.dateOfJoining?.split('T')[0];
        const patchedData = { ...emp, dateOfJoining: formattedDate };
        this.empForm.patchValue(patchedData);
        // Patch the username validator again
        this.empForm
          .get('username')
          ?.setAsyncValidators(this.usernameValidator(this.currentUsername));
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.errorMsg = 'Failed to load employee details.';
      },
    });
  }

  cancel(): void {
    history.back();
  }

  usernameValidator(currentUsername: string | null = null): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      const username = control.value;

      if (!username || username.length < 3 || username === currentUsername) {
        return of(null);
      }

      return this.employeeService.checkUsernameExists(username).pipe(
        map((exists) => (exists ? { usernameTaken: true } : null)),
        catchError(() => of(null))
      );
    };
  }

  onSubmit(): void {
    if (this.empForm.invalid) {
      this.empForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    const formData = this.empForm.value;
    const id = this.route.snapshot.paramMap.get('id');

    if (this.isEditMode && id) {
      this.employeeService.updateEmployee(id, formData).subscribe({
        next: () => {
          this.loading = false;

          const currentUser = this.authService.getCurrentUser();
          if (
            currentUser?.username === this.empForm.value.username &&
            currentUser?.role !== this.empForm.value.role
          ) {
            Swal.fire({
              icon: 'info',
              title: 'Session Updated',
              text: 'Your role has changed. You will be logged out.',
              confirmButtonColor: '#007bff',
            }).then(() => this.authService.logout());
          } else {
            Swal.fire({
              icon: 'success',
              title: 'Updated!',
              text: 'Employee updated successfully.',
            }).then(() => this.router.navigate(['/employees']));
          }
        },
        error: () => {
          this.loading = false;
          this.errorMsg = 'Update failed.';
        },
      });
    } else {
      this.employeeService.createEmployee(formData).subscribe({
        next: () => {
          this.loading = false;
          Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: 'Employee created successfully.',
            confirmButtonColor: '#007bff',
          }).then(() => {
            this.router.navigate(['/employees']);
          });
        },
        error: (err) => {
          this.loading = false;
          this.errorMsg = 'Failed to create employee. Please try again.';
          console.error(err);
          Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: this.errorMsg,
          });
        },
      });
    }
  }

  removeReadonly(event: Event): void {
    const input = event.target as HTMLInputElement;
    input.removeAttribute('readonly');
  }
}
