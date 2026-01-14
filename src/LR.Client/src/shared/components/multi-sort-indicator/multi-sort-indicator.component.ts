import { Component, input } from '@angular/core';
import { SortOrder } from '../../../core/types/utils/sort-order.type';
import { NgIf } from '@angular/common';
import { SortDescriptor } from '../../../core/interfaces/sort-descriptor.interface';

@Component({
  selector: 'app-multi-sort-indicator',
  imports: [
    NgIf
  ],
  templateUrl: './multi-sort-indicator.component.html'
})
export class MultiSortIndicatorComponent<T extends string> {
  public sortOrder = input.required<SortOrder>();
  public sortIndex = input.required<number>();
  public sortCriteria = input.required<SortDescriptor<T>[]>();
}
