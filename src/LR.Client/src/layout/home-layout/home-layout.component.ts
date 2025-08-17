import { Component } from '@angular/core';
import { NavComponent } from "../../shared/nav/nav.component";
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home-layout',
  imports: [NavComponent, RouterOutlet],
  templateUrl: './home-layout.component.html',
  styleUrl: './home-layout.component.css'
})
export class HomeLayoutComponent {

}
