import { NgIf } from '@angular/common';
import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  imports: [NgIf],
  templateUrl: './pagination.component.html'
})
export class PaginationComponent {
  public currentPage = input.required<number>();
  public totalPages = input.required<number>();
  public changePage = output<number>();

  public goToPage(pageNumber: number): void {
    if (pageNumber < 1 || pageNumber > this.totalPages()) return;

    this.changePage.emit(pageNumber);
  }
}
