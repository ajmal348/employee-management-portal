import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../../core/services/employee.service';
import { AuthService } from '../../core/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile-component.html',
  styleUrl: './profile-component.scss',
})
export class ProfileComponent implements OnInit {
  showDropdown = false;
  employee: any = null;
  isAdmin: boolean = false;

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    const token = this.authService.getToken();
    if (token) {
      this.isAdmin = this.authService.getRoleFromToken(token) === 'Admin';
      setTimeout(() => {
        this.loadProfile();
      }, 100);
    } else {
      this.router.navigate(['/login']);
    }
  }
  navigateToAddEmployee() {
    this.router.navigate(['/employees']);
  }

  loadProfile() {
    this.employeeService.getProfile().subscribe({
      next: (data) => {
        this.employee = data;
      },
      error: (err) => {
        console.error('Failed to load profile', err);
      },
    });
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
  logout() {
    this.authService.logout();
  }
  comingSoon(event: Event): void {
    event.preventDefault();
    Swal.fire({
      icon: 'info',
      title: 'Coming Soon',
      text: 'This feature is not yet implemented.',
      timer: 2000,
      showConfirmButton: false,
    });
  }
}
