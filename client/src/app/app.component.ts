import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, JsonPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit{
  private http = inject(HttpClient);
  testResponse: any;

  ngOnInit(): void {
    this.http.get('https://localhost:5001/WeatherForecast').subscribe({
      next: response => this.testResponse = response,
      error: err => console.log(err),
      complete: () => console.log("success")
    })
  }
}
