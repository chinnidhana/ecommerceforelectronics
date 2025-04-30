import { Component } from '@angular/core';
import { AuthService } from '../authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(private authService: AuthService) {}

  onSubmit() {
    this.authService.login(this.username, this.password).subscribe({
      next: (res) => {
        alert('Login successful!');
        // You can redirect to another page here
      },
      error: (err) => {
        this.errorMessage = err.error || 'Login failed';
      }
    });
  }
}