import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './components/sidebar/sidebar.component';

@Component({
  selector: 'app-main-layout',
  imports: [SidebarComponent, RouterOutlet],
  templateUrl: './main-layout.component.html'
})
export class MainLayoutComponent {
}
