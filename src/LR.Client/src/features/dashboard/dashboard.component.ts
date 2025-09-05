import { Component, inject } from '@angular/core';
import { AuthStore } from '../auth/store/auth.store';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  public authStore = inject(AuthStore);
}
