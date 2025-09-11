import { Component, inject } from '@angular/core';
import { AuthStore } from '../auth/store/auth.store';
import { BusyComponent } from "../../shared/components/busy/busy.component";

@Component({
  selector: 'app-dashboard',
  imports: [BusyComponent],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
  public authStore = inject(AuthStore);
  
}
