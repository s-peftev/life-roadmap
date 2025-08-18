import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from '../../shared/components/nav/nav.component';

@Component({
  selector: 'app-home-layout',
  imports: [NavComponent, RouterOutlet],
  templateUrl: './home-layout.component.html',
  styleUrl: './home-layout.component.css'
})
export class HomeLayoutComponent {

}
