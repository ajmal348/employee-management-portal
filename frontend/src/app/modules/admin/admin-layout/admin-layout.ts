import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterModule, CommonModule],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.scss',
})
export class AdminLayout {
  showDropdown = false;
  currentYear = new Date().getFullYear();

  constructor(private authService: AuthService) {}

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
