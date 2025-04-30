import { Component } from '@angular/core';
import { AuthService } from '../authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  username = '';
  password = '';
  message = '';

  constructor(private authService: AuthService) {}

  onSubmit() {
    this.authService.register(this.username, this.password).subscribe({
      next: (res) => {
        this.message = 'Registration successful!';
      },
      error: (err) => {
        this.message = 'Registration failed';
      }
    });
  }
}